using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using DashboardDiagnosticTool;
using DashboardDiagnosticTool.Data;
using DevExpress.DashboardCommon.Diagnostics;
using DevExpress.XtraPrinting.Native;
using NUnit.Framework;

namespace DiagnosticToolTest {
    [TestFixture]
    public class TestTracing {
        #region Help Functions
        static void CompareBenchmarks(BenchmarkItem expected, BenchmarkItem actual, bool checkThreads) {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Count, actual.Count);
            if(checkThreads)
                Assert.AreEqual(expected.ThreadId, actual.ThreadId);
            Assert.AreEqual(expected.SessionId, actual.SessionId);
            Assert.AreEqual(expected.Start, actual.Start);
            Assert.AreEqual(expected.End, actual.End);
            Assert.AreEqual(expected.Children.Count, actual.Children.Count);
            for(int i = 0; i < expected.Children.Count; i++) {
                CompareBenchmarks(expected.Children[i], actual.Children[i], checkThreads);
            }
        }

        static void CompareTraceItems(List<TraceItem> expected, List<TraceItem> actual, bool checkThreads) {
            Assert.AreEqual(expected.Count, actual.Count);
            for(int i = 0; i < expected.Count; i++) {
                if(checkThreads)
                    Assert.AreEqual(expected[i].ThreadId, actual[i].ThreadId);
                Assert.AreEqual(expected[i].SessionId, actual[i].SessionId);
                Assert.AreEqual(expected[i].EventType, actual[i].EventType);
                Assert.AreEqual(expected[i].Data, actual[i].Data);
            }
        }

        static void CheckTrees(List<Pair<BenchmarkItem, List<TraceItem>>> expected, List<Pair<BenchmarkItem, List<TraceItem>>> actual, bool checkThreads = true) {
            Comparison<Pair<BenchmarkItem, List<TraceItem>>> BenchmarkComparator = (first, second) => {
                return first.First.ThreadId.CompareTo(second.First.ThreadId);
            };
            expected.Sort(BenchmarkComparator);
            actual.Sort(BenchmarkComparator);
            Assert.AreEqual(expected.Count, actual.Count);
            for(int i = 0; i < expected.Count; i++) {
                CompareBenchmarks(expected[i].First, actual[i].First, checkThreads);
                CompareTraceItems(expected[i].Second, actual[i].Second, checkThreads);
            }
        }
        static List<Pair<BenchmarkItem, List<TraceItem>>> ImitateWork(int threads,
            List<List<List<int>>> Calls,
            List<List<string>> Names,
            List<List<List<string>>> Information,
            List<List<List<string>>> Warning,
            List<List<List<string>>> Error,
            List<bool> Straight,
            int sessionId) {
            Dictionary<int, Pair<BenchmarkItem, List<TraceItem>>> expected = new();
            for(int thread = 0; thread < threads; thread++) {
                Pair<BenchmarkItem, List<TraceItem>> item = null;
                Thread t = new Thread(new ThreadStart(() => { // Force new ThreadId
                    item = CreateTree(Calls[thread],
                        Names[thread],
                        Information[thread],
                        Warning[thread],
                        Error[thread],
                        Straight[thread],
                        sessionId);
                }));
                t.Start();
                t.Join();
                if(expected.TryGetValue(item.First.ThreadId, out Pair<BenchmarkItem, List<TraceItem>> have)) {
                    int plus = have.First.End;
                    int old = item.First.End;
                    Queue<BenchmarkItem> order = new();
                    order.Enqueue(item.First);
                    while(order.Count != 0) {
                        BenchmarkItem current = order.Dequeue();
                        current.Start = plus;
                        foreach(BenchmarkItem child in current.Children)
                            order.Enqueue(child);
                    }
                    have.First.Children.AddRange(item.First.Children);
                    have.First.End = plus + old;

                    have.Second.AddRange(item.Second);
                } else {
                    expected[item.First.ThreadId] = item;
                }
            }
            return expected.Values.ToList();
        }
        static Pair<BenchmarkItem, List<TraceItem>> CreateTree(List<List<int>> Calls,
            List<string> Names,
            List<List<string>> Information,
            List<List<string>> Warning,
            List<List<string>> Error,
            bool Straight,
            int sessionId) {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            List<TraceItem> traceItems = new();
            CallTraces(Information[0], Warning[0], Error[0], traceItems, threadId, sessionId);
            BenchmarkItem threadBenchmarkItem = new($"Thread{threadId}", sessionId, threadId) {
                Children = CallChildren(0, Calls, Names, traceItems, Information, Warning, Error, Straight, sessionId, threadId)
            };
            threadBenchmarkItem.Start = 0;
            threadBenchmarkItem.End = traceItems.Count;
            return new Pair<BenchmarkItem, List<TraceItem>>(threadBenchmarkItem, traceItems);
        }

        static void AddChild(Dictionary<int, int> Positions,
            Dictionary<string, int> Numbers,
            List<BenchmarkItem> Children,
            BenchmarkItem child,
            int index) {
            if(Positions.TryGetValue(index, out int position)) {
                Children[position].Count++;
                Children[position].End = child.End;
            } else {
                Positions[index] = Children.Count;
                if(Numbers.TryGetValue(child.Name, out int number))
                    number++;
                else
                    number = 1;
                Numbers[child.Name] = number;
                Children.Add(child);
            }
        }
        static List<BenchmarkItem> CallChildren(int Index,
            List<List<int>> Calls,
            List<string> Names,
            List<TraceItem> TraceItems,
            List<List<string>> Information,
            List<List<string>> Warning,
            List<List<string>> Error,
            bool Straight,
            int sessionId,
            int threadId) {
            Dictionary<int, int> Positions = new();
            Dictionary<string, int> Numbers = new();
            List<BenchmarkItem> Children = new();
            if(Straight) {
                for(int i = 0; i < Calls[Index].Count; i++) {
                    for(int j = 0; j < Calls[Index][i]; j++) {
                        BenchmarkItem child = TestFunctions(i, Calls, Names, TraceItems, Information, Warning, Error, Straight, sessionId, threadId);
                        AddChild(Positions, Numbers, Children, child, i);
                    }
                }
            } else {
                List<int> Called = new();
                for(int i = 0; i < Calls[Index].Count; i++) {
                    Called.Add(0);
                }
                bool change = true;
                while(change) {
                    change = false;
                    for(int i = 0; i < Calls[Index].Count; i++) {
                        if(Called[i] < Calls[Index][i]) {
                            Called[i]++;
                            BenchmarkItem child = TestFunctions(i, Calls, Names, TraceItems, Information, Warning, Error, Straight, sessionId, threadId);
                            AddChild(Positions, Numbers, Children, child, i);
                            change = true;
                        }
                    }
                }
            }
            return Children;
        }
        static void CallTraces(List<string> Information,
            List<string> Warning,
            List<string> Error,
            List<TraceItem> TraceItems,
            int threadId,
            int sessionId) {
            foreach(string information in Information) {
                DashboardTelemetry.TraceInformation(information);
                TraceItems.Add(new TraceItem(sessionId, threadId) {
                    EventType = TraceEventType.Information,
                    Data = information
                });
            }
            foreach(string warning in Warning) {
                DashboardTelemetry.TraceWarning(warning);
                TraceItems.Add(new TraceItem(sessionId, threadId) {
                    EventType = TraceEventType.Warning,
                    Data = warning
                });
            }
            foreach(string error in Error) {
                DashboardTelemetry.TraceError(error);
                TraceItems.Add(new TraceItem(sessionId, threadId) {
                    EventType = TraceEventType.Error,
                    Data = error
                });
            }
        }
        static BenchmarkItem TestFunctions(int Index,
            List<List<int>> Calls,
            List<string> Names,
            List<TraceItem> TraceItems,
            List<List<string>> Information,
            List<List<string>> Warning,
            List<List<string>> Error,
            bool Straight,
            int sessionId,
            int threadId) {
            return DashboardTelemetry.Log(Names[Index], () => {
                BenchmarkItem current = new(Names[Index], sessionId, threadId);
                current.Start = TraceItems.Count;
                CallTraces(Information[Index], Warning[Index], Error[Index], TraceItems, threadId, sessionId);
                current.Children = CallChildren(Index, Calls, Names, TraceItems, Information, Warning, Error, Straight, sessionId, threadId);
                current.End = TraceItems.Count;
                return current;
            });
        }

        List<Pair<BenchmarkItem, List<TraceItem>>> GetActual(DiagnosticController controller, int sessionId) {
            List<BenchmarkItem> actualBenchmarks = controller.GetBenchmarks(sessionId);
            List<Pair<BenchmarkItem, List<TraceItem>>> actual = new();
            foreach(BenchmarkItem item in actualBenchmarks) {
                actual.Add(new Pair<BenchmarkItem, List<TraceItem>>(item, controller.GetTraceEvents(sessionId, item)));
            }
            return actual;
        }
        #endregion
        [Test]
        public void MakeBambooTest() {
            DiagnosticController controller = new();
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected = ImitateWork(
                threads: 1,
                Calls: new() {
                    new() {
                        new() { 0, 1, 0, 0, 0 },
                        new() { 0, 0, 1, 0, 0 },
                        new() { 0, 0, 0, 1, 0 },
                        new() { 0, 0, 0, 0, 1 },
                        new() { 0, 0, 0, 0, 0 },
                    }
                },
                Names: new() {
                    new() {
                        "thread",
                        "one",
                        "two",
                        "three",
                        "four"
                    }
                },
                Information: new() {
                    new() {
                        new() { },
                        new() { "inf1" },
                        new() { "inf2" },
                        new() { "inf3" },
                        new() { "inf4" }
                    }
                },
                Warning: new() {
                    new() {
                        new() { },
                        new() { "warn1" },
                        new() { "warn2" },
                        new() { "warn3" },
                        new() { "warn4" }
                    }
                },
                Error: new() {
                    new() {
                        new() { },
                        new() { "err1" },
                        new() { "err2" },
                        new() { "err3" },
                        new() { "err4" }
                    }
                },
                Straight: new() {
                    true
                },
                sessionId: 1
            );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual = GetActual(controller, 1);
            CheckTrees(expected, actual);
        }

        [Test]
        public void MakeTreeTest() {
            DiagnosticController controller = new();
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected = ImitateWork(
                threads: 1,
                Calls: new() {
                    new() {
                        new() { 0, 1, 1, 0, 0, 0 },
                        new() { 0, 0, 1, 1, 0, 0 },
                        new() { 0, 0, 0, 0, 1, 1 },
                        new() { 0, 0, 0, 0, 0, 0 },
                        new() { 0, 0, 0, 0, 0, 0 },
                        new() { 0, 0, 0, 0, 0, 0 }
                    }
                },
                Names: new() {
                    new() {
                        "thread",
                        "1",
                        "2",
                        "3",
                        "4",
                        "5"
                    }
                },
                Information: new() {
                    new() {
                        new() { },
                        new() { "inf11", "inf12" },
                        new() { "inf2" },
                        new() { "inf31", "inf32" },
                        new() { "inf4" },
                        new() { "inf51", "inf52", "inf53" }
                    }
                },
                Warning: new() {
                    new() {
                        new() { },
                        new() { "warn1" },
                        new() { "warn21", "warn22", "warn23", "warn24" },
                        new() { "warn3" },
                        new() { "warn4" },
                        new() { "warn5" }
                    }
                },
                Error: new() {
                    new() {
                        new() { },
                        new() { "err1" },
                        new() { },
                        new() { "err31", "err32" },
                        new() { },
                        new() { }
                    }
                },
                Straight: new() {
                    true
                },
                sessionId: 1
            );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual = GetActual(controller, 1);
            CheckTrees(expected, actual);
        }
        [Test]
        public void MergeSameTest() {
            DiagnosticController controller = new();
            controller.Start();
            List<List<List<int>>> tree =
                new() {
                    new() {
                        new() { 0, 5, 3, 0, 0, 0 },
                        new() { 0, 0, 4, 6, 0, 0 },
                        new() { 0, 0, 0, 0, 7, 8 },
                        new() { 0, 0, 0, 0, 0, 9 },
                        new() { 0, 0, 0, 0, 0, 3 },
                        new() { 0, 0, 0, 0, 0, 0 }
                    }
                };
            List<List<string>> names =
                new() {
                    new() {
                        "thread",
                        "1",
                        "2",
                        "3",
                        "4",
                        "5",
                    }
                };
            List<List<List<string>>> information = new() {
                new() {
                    new() { },
                    new() { "inf11", "inf12", "inf13", "inf14" },
                    new() { "inf21", "inf22" },
                    new() { "inf31" },
                    new() { "inf41", "inf42", "inf43", "inf44", "inf45" },
                    new() { "inf51" }
                }
            };
            List<List<List<string>>> warning = new() {
                new() {
                    new() { },
                    new() { "warn1" },
                    new() { "warn2" },
                    new() { "warn3" },
                    new() { "warn4" },
                    new() { "warn5" }
                }
            };
            List<List<List<string>>> error = new() {
                new() {
                    new() { },
                    new() { "err1" },
                    new() { "err2" },
                    new() { "err3" },
                    new() { "err4" },
                    new() { "err5" }
                }
            };
            List<Pair<BenchmarkItem, List<TraceItem>>> expected1 = ImitateWork(
                threads: 1,
                Calls: tree,
                Names: names,
                Information: information,
                Warning: warning,
                Error: error,
                Straight: new() {
                    true
                },
                sessionId: 1
            );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual1 = GetActual(controller, 1);
            CheckTrees(expected1, actual1);
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected2 = ImitateWork(
                threads: 1,
                Calls: tree,
                Names: names,
                Information: information,
                Warning: warning,
                Error: error,
                Straight: new() {
                    false
                },
                sessionId: 2
            );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual2 = GetActual(controller, 2);
            CheckTrees(expected2, actual2);
        }

        [Test]
        public void SameNamesTest() {
            DiagnosticController controller = new();
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected = ImitateWork(
                threads: 1,
                Calls: new() {
                    new() {
                        new() { 0, 2, 2, 0, 0, 0, 0 },
                        new() { 0, 0, 0, 2, 2, 2, 0 },
                        new() { 0, 0, 0, 2, 0, 2, 0 },
                        new() { 0, 0, 0, 0, 0, 4, 3 },
                        new() { 0, 0, 0, 0, 0, 3, 4 },
                        new() { 0, 0, 0, 0, 0, 0, 0 },
                        new() { 0, 0, 0, 0, 0, 0, 0 }
                    }
                },
                Names: new() {
                    new() {
                        "thread",
                        "1",
                        "1",
                        "2",
                        "2",
                        "3",
                        "4"
                    }
                },
                Information: new() {
                    new() {
                        new() { },
                        new() { "inf11", "inf12", "inf13" },
                        new() { "inf21" },
                        new() { "inf31", "inf32" },
                        new() { "inf41" },
                        new() { "inf51", "inf51", "inf51" },
                        new() { "inf6" }
                    }
                },
                Warning: new() {
                    new() {
                        new() { },
                        new() { "warn", "warn" },
                        new() { "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn" },
                        new() { },
                        new() { "warn", "warn" },
                        new() { "warn", "warn", "warn", "warn" },
                        new() { "warn", "warn", "warn", "warn", "warn", "warn" }
                    }
                },
                Error: new() {
                    new() {
                        new() { },
                        new() { },
                        new() { },
                        new() { },
                        new() { },
                        new() { },
                        new() { }
                    }
                },
                new() {
                    false
                },
                1);
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual = GetActual(controller, 1);
            CheckTrees(expected, actual);
        }

        [Test]
        public void ThreadLoggingTest() {
            DiagnosticController controller = new DiagnosticController();
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected = ImitateWork(
                threads: 3,
                Calls: new() {
                    new() {
                        new() { 0, 1 },
                        new() { 0, 0 }
                    },
                    new() {
                        new() { 0, 1, 0 },
                        new() { 0, 0, 1 },
                        new() { 0, 0, 0 }
                    },
                    new() {
                        new() { 0, 1, 2 },
                        new() { 0, 0, 0 },
                        new() { 0, 0, 0 }
                    }
                },
                Names: new() {
                    new() {
                        "thread1",
                        "1"
                    },
                    new() {
                        "thread2",
                        "2",
                        "3"
                    },
                    new() {
                        "thread3",
                        "4",
                        "5",
                    }
                },
                Information: new() {
                    new() {
                        new() { },
                        new() { "inf1", "inf2" }
                    },
                    new() {
                        new() { },
                        new() { "inf1", "inf2", "inf1", "inf2", "inf1", "inf2" },
                        new() { "inf1", "inf2", "inf1", "inf2", "inf1", "inf2", "inf1", "inf2" }
                    },
                    new() {
                        new() { "inf3", "inf4", "inf4", "inf3" },
                        new() { "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3" },
                        new() { "int5" }
                    }
                },
                Warning: new() {
                    new() {
                        new() { },
                        new() { "warn11", "warn12", "warn13" }
                    },
                    new() {
                        new() { },
                        new() { "warn11", "warn12", "warn13", "warn11", "warn12", "warn13" },
                        new() { "warn21", "warn22", "warn23" }
                    },
                    new() {
                        new() { },
                        new() { },
                        new() { }
                    }
                },
                Error: new() {
                    new() {
                        new() { },
                        new() { "err1" }
                    },
                    new() {
                        new() { },
                        new() { "err1", "err1", "err1", "err1", "err1" },
                        new() { "err1", "err2", "err1", "err1", "err2" }
                    },
                    new() {
                        new() { },
                        new() { "err1", "err2", "err3", "err4", "err5" },
                        new() { "err1", "err2", "err3", "err4", "err5", "err1", "err2", "err3", "err4", "err5" }
                    }
                },
                Straight: new() {
                    true,
                    true,
                    true
                },
                sessionId: 1);
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual = GetActual(controller, 1);
            CheckTrees(expected, actual);
        }

        #region one
        List<List<int>> Calls1 = new() {
            new() { 0, 2, 2, 0, 0, 0, 0 },
            new() { 0, 0, 0, 2, 2, 2, 0 },
            new() { 0, 0, 0, 2, 0, 2, 0 },
            new() { 0, 0, 0, 0, 0, 4, 3 },
            new() { 0, 0, 0, 0, 0, 3, 4 },
            new() { 0, 0, 0, 0, 0, 0, 0 },
            new() { 0, 0, 0, 0, 0, 0, 0 }
        };
        List<string> Names1 = new() {
            "thread",
            "1",
            "1",
            "2",
            "2",
            "3",
            "4"
        };
        List<List<string>> information1 = new() {
            new() { },
            new() { "inf11", "inf12", "inf13" },
            new() { "inf21" },
            new() { "inf31", "inf32" },
            new() { "inf41" },
            new() { "inf51", "inf51", "inf51" },
            new() { "inf6" }
        };
        List<List<string>> warning1 = new() {
            new() { },
            new() { "warn", "warn" },
            new() { "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn" },
            new() { },
            new() { "warn", "warn" },
            new() { "warn", "warn", "warn", "warn" },
            new() { "warn", "warn", "warn", "warn", "warn", "warn" }
        };
        List<List<string>> error1 = new() {
            new() { },
            new() { },
            new() { },
            new() { },
            new() { },
            new() { },
            new() { }
        };
        #endregion
        #region two
        List<List<int>> Calls2 = new() {
            new() { 0, 1, 2 },
            new() { 0, 0, 0 },
            new() { 0, 0, 0 }
        };
        List<string> Names2 = new() {
            "thread3",
            "5",
            "6",
        };
        List<List<string>> information2 = new() {
            new() { "inf3", "inf4", "inf4", "inf3" },
            new() { "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3" },
            new() { "int5" }
        };
        List<List<string>> warning2 = new() {
            new() { },
            new() { "warn11", "warn12", "warn13", "warn11", "warn12", "warn13" },
            new() { "warn21", "warn22", "warn23" }
        };
        List<List<string>> error2 = new() {
            new() { },
            new() { "err1", "err2", "err3", "err4", "err5" },
            new() { "err1", "err2", "err3", "err4", "err5", "err1", "err2", "err3", "err4", "err5" }
        };
        #endregion
        #region three
        List<List<int>> Calls3 = new() {
            new() { 0, 5, 3, 0, 0, 0 },
            new() { 0, 0, 4, 6, 0, 0 },
            new() { 0, 0, 0, 0, 7, 8 },
            new() { 0, 0, 0, 0, 0, 9 },
            new() { 0, 0, 0, 0, 0, 3 },
            new() { 0, 0, 0, 0, 0, 0 }
        };
        List<string> Names3 = new() {
            "thread",
            "7",
            "8",
            "9",
            "10",
            "11",
        };
        List<List<string>> information3 = new() {
            new() { },
            new() { "inf11", "inf12", "inf13", "inf14" },
            new() { "inf21", "inf22" },
            new() { "inf31" },
            new() { "inf41", "inf42", "inf43", "inf44", "inf45" },
            new() { "inf51" }
        };
        List<List<string>> warning3 = new() {
            new() { },
            new() { "warn1" },
            new() { "warn2" },
            new() { "warn3" },
            new() { "warn4" },
            new() { "warn5" }
        };
        List<List<string>> error3 = new() {
            new() { },
            new() { "err1" },
            new() { },
            new() { "err31", "err32" },
            new() { },
            new() { }
        };
        #endregion

        [Test]
        public void ManySessionsTest() {
            #region first
            DiagnosticController controller = new();
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected1 = ImitateWork(
                    threads: 2,
                    Calls: new() {
                        Calls1,
                        Calls2
                    },
                    Names: new() {
                        Names1,
                        Names2
                    },
                    Information: new() {
                        information1,
                        information2
                    },
                    Warning: new() {
                        warning1,
                        warning2
                    },
                    Error: new() {
                        error1,
                        error2
                    },
                    Straight: new() {
                        false,
                        false,
                    },
                    sessionId: 1
                    );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual1 = GetActual(controller, 1);
            CheckTrees(expected1, actual1);
            #endregion
            #region second
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected2 = ImitateWork(
                    threads: 2,
                    Calls: new() {
                        Calls1,
                        Calls3
                    },
                    Names: new() {
                        Names1,
                        Names3
                    },
                    Information: new() {
                        information1,
                        information3
                    },
                    Warning: new() {
                        warning1,
                        warning3
                    },
                    Error: new() {
                        error1,
                        error3
                    },
                    Straight: new() {
                        false,
                        false,
                    },
                    sessionId: 2
                    );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual2 = GetActual(controller, 2);
            CheckTrees(expected2, actual2);
            #endregion
            #region third
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected3 = ImitateWork(
                    threads: 3,
                    Calls: new() {
                        Calls1,
                        Calls2,
                        Calls3
                    },
                    Names: new() {
                        Names1,
                        Names2,
                        Names3
                    },
                    Information: new() {
                        information1,
                        information2,
                        information3
                    },
                    Warning: new() {
                        warning1,
                        warning2,
                        warning3
                    },
                    Error: new() {
                        error1,
                        error2,
                        error3
                    },
                    Straight: new() {
                        false,
                        false,
                        false
                    },
                    sessionId: 3
                    );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual3 = GetActual(controller, 3);
            CheckTrees(expected3, actual3);
            #endregion
            #region fourth
            controller.Start();
            List<Pair<BenchmarkItem, List<TraceItem>>> expected4 = ImitateWork(
                    threads: 2,
                    Calls: new() {
                        Calls2,
                        Calls3
                    },
                    Names: new() {
                        Names2,
                        Names3
                    },
                    Information: new() {
                        information2,
                        information3
                    },
                    Warning: new() {
                        warning2,
                        warning3
                    },
                    Error: new() {
                        error2,
                        error3
                    },
                    Straight: new() {
                        false,
                        false,
                    },
                    sessionId: 4
                    );
            controller.Stop();
            List<Pair<BenchmarkItem, List<TraceItem>>> actual4 = GetActual(controller, 4);
            CheckTrees(expected4, actual4);
            #endregion
        }

        [Test]
        public void SerializationTest() {
            DiagnosticController one = new();
            one.Start();
            var expected1 = ImitateWork(
                    threads: 2,
                    Calls: new() { Calls1, Calls3 },
                    Names: new() { Names1, Names3 },
                    Information: new() { information1, information3 },
                    Warning: new() { warning1, warning3 },
                    Error: new() { error1, error3 },
                    Straight: new() { true, false },
                    1
                );
            one.Stop();
            one.Start();
            var expected2 = ImitateWork(
                    threads: 1,
                    Calls: new() { Calls2 },
                    Names: new() { Names2 },
                    Information: new() { information2 },
                    Warning: new() { warning2 },
                    Error: new() { error2 },
                    Straight: new() { true },
                    2
                );
            one.Stop();
            string save = "TestSave.xml";
            one.SaveAs(save);
            DiagnosticController two = new();
            two.Open(save);
            var actual1 = GetActual(two, 1);
            CheckTrees(expected1, actual1);
            var actual2 = GetActual(two, 2);
            CheckTrees(expected2, actual2);
            if(File.Exists(save))
                File.Delete(save);
        }

        [Test]
        public void DeletingSessionTest() {
            DiagnosticController controller = new();
            List<List<Pair<BenchmarkItem, List<TraceItem>>>> expected = new();
            controller.Start();
            expected.Add(
                ImitateWork(
                    threads: 1,
                    Calls: new() { Calls2 },
                    Names: new() { Names2 },
                    Information: new() { information2 },
                    Warning: new() { warning2 },
                    Error: new() { error2 },
                    Straight: new() { true },
                    1
                    )
                );
            controller.Stop();
            controller.Start();
            expected.Add(
                ImitateWork(
                    threads: 1,
                    Calls: new() { Calls2 },
                    Names: new() { Names2 },
                    Information: new() { information2 },
                    Warning: new() { warning2 },
                    Error: new() { error2 },
                    Straight: new() { true },
                    2
                    )
                );
            controller.Stop();
            controller.Start();
            expected.Add(
                ImitateWork(
                    threads: 1,
                    Calls: new() { Calls1 },
                    Names: new() { Names1 },
                    Information: new() { information1 },
                    Warning: new() { warning1 },
                    Error: new() { error1 },
                    Straight: new() { true },
                    3
                    )
                );
            controller.Stop();
            controller.Start();
            expected.Add(
                ImitateWork(
                    threads: 1,
                    Calls: new() { Calls1 },
                    Names: new() { Names1 },
                    Information: new() { information1 },
                    Warning: new() { warning1 },
                    Error: new() { error1 },
                    Straight: new() { true },
                    4
                    )
                );
            controller.Stop();
            controller.Start();
            expected.Add(
                ImitateWork(
                    threads: 1,
                    Calls: new() { Calls3 },
                    Names: new() { Names3 },
                    Information: new() { information3 },
                    Warning: new() { warning3 },
                    Error: new() { error3 },
                    Straight: new() { true },
                    5
                    )
                );
            controller.Stop();
            for(int i = 0; i < 5; i++) {
                Assert.AreEqual(5 - i, controller.Sessions.Count);
                for(int j = i; j < 5; j++) {
                    var actual = GetActual(controller, j + 1);
                    CheckTrees(expected[j], actual);
                }
                controller.Delete(controller.Sessions[0]);
            }
            Assert.AreEqual(0, controller.Sessions.Count);
        }
    }
}
