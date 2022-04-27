using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DashboardDiagnosticTool;
using DashboardDiagnosticTool.Data;
using DiagnosticTool.LinuxTools;
using NUnit.Framework;

namespace DiagnosticToolTest {
    public class WorkEmulator : IDisposable {
        internal const int DefaultThreadId = 1;
        static DXTraceEvent CreateEnterEvent(string methodName, int threadId = DefaultThreadId) {
            return new DXTraceEvent("EnterScope", DateTime.Now, GetConfiguredPayload(threadId, methodName, null));
        }
        static DXTraceEvent CreateLeaveEvent(string methodName, int threadId = DefaultThreadId) {
            return new DXTraceEvent("LeaveScope", DateTime.Now, GetConfiguredPayload(threadId, methodName, null));
        }
        static DXTraceEvent CreateTraceEvent(string data, int threadId = DefaultThreadId) {
            return new DXTraceEvent(
                "TraceInformation",
                DateTime.Now,
                GetConfiguredPayload(threadId, "TraceEvent", TraceEventType.Information, data)
            );
        }
        static Func<string, object> GetConfiguredPayload(int threadId, string methodName, TraceEventType? eventType, string? data = null) {
            return (string name) => {
                switch(name) {
                    case "Id":
                        return threadId;
                    case "Name":
                        return methodName;
                    case "Data":
                        return data;
                    case "EventType":
                        return eventType;
                    default: return null;
                }
            };
        }
        readonly string methodName;
        readonly TraceDataProcessor processor;

        public WorkEmulator(string methodName, TraceDataProcessor processor) {
            this.processor = processor;
            this.methodName = methodName;
            processor.Process(CreateEnterEvent(methodName));
        }
        public void Trace(string data) {
            processor.Process(CreateTraceEvent(data));
        }
        public void Dispose() {
            processor.Process(CreateLeaveEvent(methodName));
        }
    }
    [TestFixture]
    public class TraceDataProcessorTests {
        const int DefaultThreadId = WorkEmulator.DefaultThreadId;
        const int DefaultSessionId = 222;
        static int CountWithChildren(IEnumerable<BenchmarkItem> items) {
            return items.Count() + items.Select(i => CountWithChildren(i.Children)).Sum();
        }
        [Test]
        public void CreateSingleBenchmark() {
            TraceDataProcessor processor = new TraceDataProcessor(DefaultSessionId);
            using(WorkEmulator em1 = new WorkEmulator("TestMethod1", processor)) { }
            Assert.AreEqual(2, CountWithChildren(processor.Benchmarks)); // root benchmark + 1 workload
        }

        [Test]
        public void CreateSingleBenchmarkWithTraceEvent() {
            TraceDataProcessor processor = new TraceDataProcessor(DefaultSessionId);
            using(WorkEmulator em1 = new WorkEmulator("TestMethod1", processor)) {
                em1.Trace("Test trace data");
            }
            Assert.AreEqual(1, processor.TraceItems[DefaultThreadId].Count);
            var benchmarks = processor.Benchmarks;
            Assert.AreEqual(2, CountWithChildren(benchmarks)); // root benchmark + 1 workload
            var benchmark = benchmarks[0].Children[0];
            Assert.AreEqual(0, benchmark.Start);
            Assert.AreEqual(1, benchmark.End);
        }

        [Test]
        public void CheckBenchmarkTree() {
            TraceDataProcessor processor = new TraceDataProcessor(DefaultSessionId);
            using(WorkEmulator em1 = new WorkEmulator("TestMethod1", processor)) {
                using(WorkEmulator em11 = new WorkEmulator("TestMethod11", processor)) { }
                using(WorkEmulator em12 = new WorkEmulator("TestMethod12", processor)) { }
            }
            using(WorkEmulator em2 = new WorkEmulator("TestMethod2", processor)) { }
            Assert.AreEqual(5, CountWithChildren(processor.Benchmarks)); // root benchmark + 4 workload
        }

        [Test]
        public void CheckBenchmarkStartsAndEnds() {
            TraceDataProcessor processor = new TraceDataProcessor(DefaultSessionId);
            using(WorkEmulator em1 = new WorkEmulator("TestMethod1", processor)) {          // Start 0
                em1.Trace("Trace1 method1");
                using(WorkEmulator em11 = new WorkEmulator("TestMethod11", processor)) {    // Start 1
                    em11.Trace("Trace1 method11");
                    em11.Trace("Trace2 method11");
                }                                                                           // End 3
                using(WorkEmulator em12 = new WorkEmulator("TestMethod12", processor)) {    // Start 3
                    em12.Trace("Trace method12");
                }                                                                           // End 4
                em1.Trace("Trace2 method1");
            }                                                                               // End 5
            using(WorkEmulator em2 = new WorkEmulator("TestMethod2", processor)) {          // Start 5
                em2.Trace("Trace1 method2");
                em2.Trace("Trace2 method2");
            }                                                                               // End 7
            var benchmarks = processor.Benchmarks;
            Assert.AreEqual(5, CountWithChildren(benchmarks));
            Assert.AreEqual(7, processor.TraceItems[DefaultThreadId].Count);

            var thread1Benchmarks = benchmarks[0].Children;
            Assert.AreEqual(0, thread1Benchmarks[0].Start);
            Assert.AreEqual(5, thread1Benchmarks[0].End);

            Assert.AreEqual(1, thread1Benchmarks[0].Children[0].Start);
            Assert.AreEqual(3, thread1Benchmarks[0].Children[0].End);

            Assert.AreEqual(3, thread1Benchmarks[0].Children[1].Start);
            Assert.AreEqual(4, thread1Benchmarks[0].Children[1].End);

            Assert.AreEqual(5, thread1Benchmarks[1].Start);
            Assert.AreEqual(7, thread1Benchmarks[1].End);
        }
        [Test]
        public void CheckBenchmarkItemsCount() {
            TraceDataProcessor processor = new TraceDataProcessor(DefaultSessionId);
            using(WorkEmulator em1 = new WorkEmulator("TestMethod1", processor)) { }
            using(WorkEmulator em2 = new WorkEmulator("TestMethod1", processor)) { }
            using(WorkEmulator em3 = new WorkEmulator("TestMethod1", processor)) { }
            Assert.AreEqual(2, CountWithChildren(processor.Benchmarks));
            Assert.AreEqual(3, processor.Benchmarks[0].Children[0].Count);
        }
        [Test]
        public void JoinTraceItemInSameBenchmarkItems() {
            TraceDataProcessor processor = new TraceDataProcessor(DefaultSessionId);
            using(WorkEmulator em1 = new WorkEmulator("TestMethod1", processor)) { }
            using(WorkEmulator em2 = new WorkEmulator("TestMethod2", processor)) {  // Start 0
                em2.Trace("Trace1 method1");
            }                                                                       // End 1
            using(WorkEmulator em3 = new WorkEmulator("TestMethod2", processor)) {  // Start 1
                em3.Trace("Trace1 method2");
                em3.Trace("Trace2 method2");
            }                                                                       // End 3
            using(WorkEmulator em4 = new WorkEmulator("TestMethod2", processor)) {  // Start 3
                em4.Trace("Trace1 method3");
            }                                                                       // End 4

            var thread1Benchmarks = processor.Benchmarks[0].Children;
            Assert.AreEqual(2, CountWithChildren(thread1Benchmarks));
            Assert.AreEqual(0, thread1Benchmarks[0].Start);
            Assert.AreEqual(0, thread1Benchmarks[0].End);
            Assert.AreEqual(0, thread1Benchmarks[1].Start);
            Assert.AreEqual(4, thread1Benchmarks[1].End);
        }
    }
}
