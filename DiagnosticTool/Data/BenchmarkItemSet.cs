using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace DashboardDiagnosticTool.Data {
    [XmlRoot]
    public class BenchmarkItemSet {
        public List<BenchmarkItem> Benchmarks { get; private set; } = new List<BenchmarkItem>();
        public List<TraceItem> TraceItems { get; private set; } = new List<TraceItem>();
        public List<SessionItem> Sessions { get; private set; } = new List<SessionItem>();

        public BenchmarkItemSet() {
        }
        public BenchmarkItemSet(Dictionary<int, List<BenchmarkItem>> benchmarks, Dictionary<int, Dictionary<int, List<TraceItem>>> traceItems, IEnumerable<SessionItem> sessions) {
            Benchmarks = benchmarks.Values.SelectMany(x => x).ToList();
            TraceItems = traceItems.Values.SelectMany(dict => dict.Values.SelectMany(x => x)).ToList();
            Sessions.AddRange(sessions);
        }

        public Dictionary<int, List<BenchmarkItem>> GetBenchmarks() {
            Dictionary<int, List<BenchmarkItem>> answer = new Dictionary<int, List<BenchmarkItem>>();
            Benchmarks.ForEach(benchmark => {
                int sessionId = benchmark.SessionId;
                if(!answer.ContainsKey(sessionId))
                    answer.Add(sessionId, new List<BenchmarkItem>());
                answer[sessionId].Add(benchmark);
            });
            return answer;
        }

        public Dictionary<int, Dictionary<int, List<TraceItem>>> GetTraceItems() {
            Dictionary<int, Dictionary<int, List<TraceItem>>> answer = new Dictionary<int, Dictionary<int, List<TraceItem>>>();
            TraceItems.ForEach(traceItem => {
                int sessionId = traceItem.SessionId;
                int threadId = traceItem.ThreadId;
                if(!answer.ContainsKey(sessionId))
                    answer.Add(sessionId, new Dictionary<int, List<TraceItem>>());
                if(!answer[sessionId].ContainsKey(threadId))
                    answer[sessionId].Add(threadId, new List<TraceItem>());
                answer[sessionId][threadId].Add(traceItem);
            });
            return answer;
        }
    }
}
