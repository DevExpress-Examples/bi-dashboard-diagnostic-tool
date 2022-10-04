Imports DashboardDiagnosticTool.Data
Imports NUnit.Framework

Namespace DiagnosticToolTest

    <TestFixture>
    Public Class BenchmarkItemSetTests

        <Test>
        Public Sub GetBenchmarksBySessionIdTests()
            Dim session1Id As Integer = 1, session2Id As Integer = 2
            Dim [set] As BenchmarkItemSet = New BenchmarkItemSet()
            [set].Sessions.Add(New SessionItem With {.Name = "Session1", .SessionId = session1Id})
            [set].Sessions.Add(New SessionItem With {.Name = "Session2", .SessionId = session2Id})
            [set].Benchmarks.Add(New BenchmarkItem With {.SessionId = session2Id, .Name = "BenchmarkItem1", .ID = 1})
            [set].Benchmarks.Add(New BenchmarkItem With {.SessionId = session2Id, .Name = "BenchmarkItem2", .ID = 2})
            Dim benchmarksBySessionId = [set].GetBenchmarks()
            Assert.AreEqual(2, benchmarksBySessionId.Count)
            Call Assert.AreEqual(0, benchmarksBySessionId(session1Id).Count)
            Call Assert.AreEqual(2, benchmarksBySessionId(session2Id).Count)
        End Sub

        <Test>
        Public Sub GetTraceItemsBySessionAndBenchmarkIds()
            Dim session1Id As Integer = 1, session2Id As Integer = 2, session3Id As Integer = 3
            Dim thread1Id As Integer = 1, thread2Id As Integer = 2
            Dim [set] As BenchmarkItemSet = New BenchmarkItemSet()
            [set].Sessions.Add(New SessionItem With {.Name = "Session1", .SessionId = session1Id})
            [set].Sessions.Add(New SessionItem With {.Name = "Session2", .SessionId = session2Id})
            [set].Sessions.Add(New SessionItem With {.Name = "Session3", .SessionId = session3Id})
            [set].Benchmarks.Add(New BenchmarkItem With {.SessionId = session2Id, .Name = "BenchmarkItem1", .ID = 1})
            [set].Benchmarks.Add(New BenchmarkItem With {.SessionId = session2Id, .Name = "BenchmarkItem2", .ID = 2})
            [set].Benchmarks.Add(New BenchmarkItem With {.SessionId = session3Id, .Name = "BenchmarkItem3", .ID = 3})
            [set].Benchmarks.Add(New BenchmarkItem With {.SessionId = session3Id, .Name = "BenchmarkItem4", .ID = 4})
            [set].TraceItems.Add(New TraceItem With {.SessionId = session2Id, .ThreadId = thread1Id, .Data = "TraceItem1"})
            [set].TraceItems.Add(New TraceItem With {.SessionId = session2Id, .ThreadId = thread2Id, .Data = "TraceItem2"})
            [set].TraceItems.Add(New TraceItem With {.SessionId = session3Id, .ThreadId = thread1Id, .Data = "TraceItem3"})
            Dim traceItemsBySessionAndThreadId = [set].GetTraceItems()
            Assert.AreEqual(3, traceItemsBySessionAndThreadId.Count)
            Dim session1Items = traceItemsBySessionAndThreadId(session1Id)
            Assert.AreEqual(0, session1Items.Count)
            Dim session2Items = traceItemsBySessionAndThreadId(session2Id)
            Assert.AreEqual(2, session2Items.Count)
            Call Assert.AreEqual(1, session2Items(thread1Id).Count)
            Call Assert.AreEqual("TraceItem1", session2Items(thread1Id)(0).Data)
            Call Assert.AreEqual(1, session2Items(thread2Id).Count)
            Call Assert.AreEqual("TraceItem2", session2Items(thread2Id)(0).Data)
            Dim session3Items = traceItemsBySessionAndThreadId(session3Id)
            Assert.AreEqual(1, session3Items.Count)
            Call Assert.AreEqual(1, session3Items(thread1Id).Count)
            Call Assert.AreEqual("TraceItem3", session3Items(thread1Id)(0).Data)
        End Sub
    End Class
End Namespace
