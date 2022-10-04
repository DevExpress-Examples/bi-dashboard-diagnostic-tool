Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports DashboardDiagnosticTool
Imports DashboardDiagnosticTool.Data
Imports DiagnosticTool.LinuxTools
Imports NUnit.Framework

Namespace DiagnosticToolTest

    Public Class WorkEmulator
        Implements IDisposable

        Friend Const DefaultThreadId As Integer = 1

        Private Shared Function CreateEnterEvent(ByVal methodName As String, ByVal Optional threadId As Integer = DefaultThreadId) As DXTraceEvent
            Return New DXTraceEvent("EnterScope", Date.Now, GetConfiguredPayload(threadId, methodName, Nothing))
        End Function

        Private Shared Function CreateLeaveEvent(ByVal methodName As String, ByVal Optional threadId As Integer = DefaultThreadId) As DXTraceEvent
            Return New DXTraceEvent("LeaveScope", Date.Now, GetConfiguredPayload(threadId, methodName, Nothing))
        End Function

        Private Shared Function CreateTraceEvent(ByVal data As String, ByVal Optional threadId As Integer = DefaultThreadId) As DXTraceEvent
            Return New DXTraceEvent("TraceInformation", Date.Now, WorkEmulator.GetConfiguredPayload(threadId, "TraceEvent", TraceEventType.Information, data))
        End Function

        Private Shared Function GetConfiguredPayload(ByVal threadId As Integer, ByVal methodName As String, ByVal eventType As TraceEventType?, ByVal Optional data As String? = Nothing) As Func(Of String, Object)
            Return Function(ByVal name)
                Select Case name
                    Case "Id"
                        Return threadId
                    Case "Name"
                        Return methodName
                    Case "Data"
                        Return data
                    Case "EventType"
                        Return eventType
                    Case Else
                        Return Nothing
                End Select
            End Function
        End Function

        Private ReadOnly methodName As String

        Private ReadOnly processor As TraceDataProcessor

        Public Sub New(ByVal methodName As String, ByVal processor As TraceDataProcessor)
            Me.processor = processor
            Me.methodName = methodName
            processor.Process(CreateEnterEvent(methodName))
        End Sub

        Public Sub Trace(ByVal data As String)
            processor.Process(CreateTraceEvent(data))
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            processor.Process(CreateLeaveEvent(methodName))
        End Sub
    End Class

    <TestFixture>
    Public Class TraceDataProcessorTests

        Const DefaultThreadId As Integer = WorkEmulator.DefaultThreadId

        Const DefaultSessionId As Integer = 222

        Private Shared Function CountWithChildren(ByVal items As IEnumerable(Of BenchmarkItem)) As Integer
            Return items.Count() + Enumerable.Select(Of BenchmarkItem, Global.System.Int32)(items, CType(Function(i) CInt(CountWithChildren(CType(i.Children, IEnumerable(Of BenchmarkItem)))), Func(Of BenchmarkItem, Integer))).Sum()
        End Function

        <Test>
        Public Sub CreateSingleBenchmark()
            Dim processor As TraceDataProcessor = New TraceDataProcessor(DefaultSessionId)
            Using em1 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
            End Using

            Assert.AreEqual(2, CountWithChildren(processor.Benchmarks)) ' root benchmark + 1 workload
        End Sub

        <Test>
        Public Sub CreateSingleBenchmarkWithTraceEvent()
            Dim processor As TraceDataProcessor = New TraceDataProcessor(DefaultSessionId)
            Using em1 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
                em1.Trace("Test trace data")
            End Using

            Call Assert.AreEqual(1, processor.TraceItems(DefaultThreadId).Count)
            Dim benchmarks = processor.Benchmarks
            Assert.AreEqual(2, CountWithChildren(benchmarks)) ' root benchmark + 1 workload
            Dim benchmark = benchmarks(0).Children(0)
            Assert.AreEqual(0, benchmark.Start)
            Assert.AreEqual(1, benchmark.End)
        End Sub

        <Test>
        Public Sub CheckBenchmarkTree()
            Dim processor As TraceDataProcessor = New TraceDataProcessor(DefaultSessionId)
            Using em1 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
                Using em11 As WorkEmulator = New WorkEmulator("TestMethod11", processor)
                End Using

                Using em12 As WorkEmulator = New WorkEmulator("TestMethod12", processor)
                End Using
            End Using

            Using em2 As WorkEmulator = New WorkEmulator("TestMethod2", processor)
            End Using

            Assert.AreEqual(5, CountWithChildren(processor.Benchmarks)) ' root benchmark + 4 workload
        End Sub

        <Test>
        Public Sub CheckBenchmarkStartsAndEnds()
            Dim processor As TraceDataProcessor = New TraceDataProcessor(DefaultSessionId)
            Using em1 As WorkEmulator = New WorkEmulator("TestMethod1", processor) ' Start 0
                em1.Trace("Trace1 method1")
                Using em11 As WorkEmulator = New WorkEmulator("TestMethod11", processor) ' Start 1
                    em11.Trace("Trace1 method11")
                    em11.Trace("Trace2 method11")
                End Using ' End 3

                Using em12 As WorkEmulator = New WorkEmulator("TestMethod12", processor) ' Start 3
                    em12.Trace("Trace method12")
                End Using ' End 4

                em1.Trace("Trace2 method1")
            End Using ' End 5

            Using em2 As WorkEmulator = New WorkEmulator("TestMethod2", processor) ' Start 5
                em2.Trace("Trace1 method2")
                em2.Trace("Trace2 method2")
            End Using ' End 7

            Dim benchmarks = processor.Benchmarks
            Assert.AreEqual(5, CountWithChildren(benchmarks))
            Call Assert.AreEqual(7, processor.TraceItems(DefaultThreadId).Count)
            Dim thread1Benchmarks = benchmarks(0).Children
            Call Assert.AreEqual(0, thread1Benchmarks(0).Start)
            Call Assert.AreEqual(5, thread1Benchmarks(0).End)
            Call Assert.AreEqual(1, thread1Benchmarks(0).Children(0).Start)
            Call Assert.AreEqual(3, thread1Benchmarks(0).Children(0).End)
            Call Assert.AreEqual(3, thread1Benchmarks(0).Children(1).Start)
            Call Assert.AreEqual(4, thread1Benchmarks(0).Children(1).End)
            Call Assert.AreEqual(5, thread1Benchmarks(1).Start)
            Call Assert.AreEqual(7, thread1Benchmarks(1).End)
        End Sub

        <Test>
        Public Sub CheckBenchmarkItemsCount()
            Dim processor As TraceDataProcessor = New TraceDataProcessor(DefaultSessionId)
            Using em1 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
            End Using

            Using em2 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
            End Using

            Using em3 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
            End Using

            Assert.AreEqual(2, CountWithChildren(processor.Benchmarks))
            Call Assert.AreEqual(3, processor.Benchmarks(0).Children(0).Count)
        End Sub

        <Test>
        Public Sub JoinTraceItemInSameBenchmarkItems()
            Dim processor As TraceDataProcessor = New TraceDataProcessor(DefaultSessionId)
            Using em1 As WorkEmulator = New WorkEmulator("TestMethod1", processor)
            End Using

            Using em2 As WorkEmulator = New WorkEmulator("TestMethod2", processor) ' Start 0
                em2.Trace("Trace1 method1")
            End Using ' End 1

            Using em3 As WorkEmulator = New WorkEmulator("TestMethod2", processor) ' Start 1
                em3.Trace("Trace1 method2")
                em3.Trace("Trace2 method2")
            End Using ' End 3

            Using em4 As WorkEmulator = New WorkEmulator("TestMethod2", processor) ' Start 3
                em4.Trace("Trace1 method3")
            End Using ' End 4

            Dim thread1Benchmarks = processor.Benchmarks(0).Children
            Assert.AreEqual(2, CountWithChildren(thread1Benchmarks))
            Call Assert.AreEqual(0, thread1Benchmarks(0).Start)
            Call Assert.AreEqual(0, thread1Benchmarks(0).End)
            Call Assert.AreEqual(0, thread1Benchmarks(1).Start)
            Call Assert.AreEqual(4, thread1Benchmarks(1).End)
        End Sub
    End Class
End Namespace
