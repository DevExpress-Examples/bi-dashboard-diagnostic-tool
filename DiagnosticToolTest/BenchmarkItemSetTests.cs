using DashboardDiagnosticTool.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnosticToolTest {
    [TestFixture]
    public class BenchmarkItemSetTests {
        [Test]
        public void GetBenchmarksBySessionIdTests() {
            int session1Id = 1, session2Id = 2;
            BenchmarkItemSet set = new BenchmarkItemSet();
            set.Sessions.Add(new SessionItem { Name = "Session1", SessionId = session1Id });
            set.Sessions.Add(new SessionItem { Name = "Session2", SessionId = session2Id });
            set.Benchmarks.Add(new BenchmarkItem { SessionId = session2Id, Name = "BenchmarkItem1", ID = 1 });
            set.Benchmarks.Add(new BenchmarkItem { SessionId = session2Id, Name = "BenchmarkItem2", ID = 2 });

            var benchmarksBySessionId = set.GetBenchmarks();

            Assert.AreEqual(2, benchmarksBySessionId.Count);
            Assert.AreEqual(0, benchmarksBySessionId[session1Id].Count);
            Assert.AreEqual(2, benchmarksBySessionId[session2Id].Count); 
        }
        [Test]
        public void GetTraceItemsBySessionAndBenchmarkIds() {
            int session1Id = 1, session2Id = 2, session3Id = 3;
            int thread1Id = 1, thread2Id = 2;
            BenchmarkItemSet set = new BenchmarkItemSet();
            set.Sessions.Add(new SessionItem { Name = "Session1", SessionId = session1Id });
            set.Sessions.Add(new SessionItem { Name = "Session2", SessionId = session2Id });
            set.Sessions.Add(new SessionItem { Name = "Session3", SessionId = session3Id });
            set.Benchmarks.Add(new BenchmarkItem { SessionId = session2Id, Name = "BenchmarkItem1", ID = 1 });
            set.Benchmarks.Add(new BenchmarkItem { SessionId = session2Id, Name = "BenchmarkItem2", ID = 2 });
            set.Benchmarks.Add(new BenchmarkItem { SessionId = session3Id, Name = "BenchmarkItem3", ID = 3 });
            set.Benchmarks.Add(new BenchmarkItem { SessionId = session3Id, Name = "BenchmarkItem4", ID = 4 });
            set.TraceItems.Add(new TraceItem { SessionId = session2Id, ThreadId = thread1Id, Data = "TraceItem1" });
            set.TraceItems.Add(new TraceItem { SessionId = session2Id, ThreadId = thread2Id, Data = "TraceItem2" });
            set.TraceItems.Add(new TraceItem { SessionId = session3Id, ThreadId = thread1Id, Data = "TraceItem3" });

            var traceItemsBySessionAndThreadId = set.GetTraceItems();

            Assert.AreEqual(3, traceItemsBySessionAndThreadId.Count);
            var session1Items = traceItemsBySessionAndThreadId[session1Id];
            Assert.AreEqual(0, session1Items.Count);
            var session2Items = traceItemsBySessionAndThreadId[session2Id];
            Assert.AreEqual(2, session2Items.Count);
            Assert.AreEqual(1, session2Items[thread1Id].Count);
            Assert.AreEqual("TraceItem1", session2Items[thread1Id][0].Data);
            Assert.AreEqual(1, session2Items[thread2Id].Count);
            Assert.AreEqual("TraceItem2", session2Items[thread2Id][0].Data);
            var session3Items = traceItemsBySessionAndThreadId[session3Id];
            Assert.AreEqual(1, session3Items.Count);
            Assert.AreEqual(1, session3Items[thread1Id].Count);
            Assert.AreEqual("TraceItem3", session3Items[thread1Id][0].Data);
        }
    }
}
