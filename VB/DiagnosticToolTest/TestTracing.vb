Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Linq
Imports System.Threading
Imports DashboardDiagnosticTool
Imports DashboardDiagnosticTool.Data
Imports DevExpress.DashboardCommon.Diagnostics
Imports DevExpress.XtraPrinting.Native
Imports NUnit.Framework

Namespace DiagnosticToolTest

    <TestFixture>
    Public Class TestTracing

#Region "Help Functions"
        Private Shared Sub CompareBenchmarks(ByVal expected As BenchmarkItem, ByVal actual As BenchmarkItem, ByVal checkThreads As Boolean)
            Assert.AreEqual(expected.Name, actual.Name)
            Assert.AreEqual(expected.Count, actual.Count)
            If checkThreads Then Assert.AreEqual(expected.ThreadId, actual.ThreadId)
            Assert.AreEqual(expected.SessionId, actual.SessionId)
            Assert.AreEqual(expected.Start, actual.Start)
            Assert.AreEqual(expected.End, actual.End)
            Assert.AreEqual(expected.Children.Count, actual.Children.Count)
            For i As Integer = 0 To expected.Children.Count - 1
                CompareBenchmarks(expected.Children(i), actual.Children(i), checkThreads)
            Next
        End Sub

        Private Shared Sub CompareTraceItems(ByVal expected As List(Of TraceItem), ByVal actual As List(Of TraceItem), ByVal checkThreads As Boolean)
            Assert.AreEqual(expected.Count, actual.Count)
            For i As Integer = 0 To expected.Count - 1
                If checkThreads Then Call Assert.AreEqual(expected(i).ThreadId, actual(i).ThreadId)
                Call Assert.AreEqual(expected(i).SessionId, actual(i).SessionId)
                Call Assert.AreEqual(expected(i).EventType, actual(i).EventType)
                Call Assert.AreEqual(expected(i).Data, actual(i).Data)
            Next
        End Sub

        Private Shared Sub CheckTrees(ByVal expected As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))), ByVal actual As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))), ByVal Optional checkThreads As Boolean = True)
            Dim BenchmarkComparator As Comparison(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = Function(first, second) first.First.ThreadId.CompareTo(second.First.ThreadId)
            expected.Sort(BenchmarkComparator)
            actual.Sort(BenchmarkComparator)
            Assert.AreEqual(expected.Count, actual.Count)
            For i As Integer = 0 To expected.Count - 1
                Call CompareBenchmarks(expected(i).First, actual(i).First, checkThreads)
                Call CompareTraceItems(expected(i).Second, actual(i).Second, checkThreads)
            Next
        End Sub

        Private Shared Function ImitateWork(ByVal threads As Integer, ByVal Calls As List(Of List(Of List(Of Integer))), ByVal Names As List(Of List(Of String)), ByVal Information As List(Of List(Of List(Of String))), ByVal Warning As List(Of List(Of List(Of String))), ByVal [Error] As List(Of List(Of List(Of String))), ByVal Straight As List(Of Boolean), ByVal sessionId As Integer) As List(Of Pair(Of BenchmarkItem, List(Of TraceItem)))
            Dim expected As Dictionary(Of Integer, Pair(Of BenchmarkItem, List(Of TraceItem))) = New(_, _)()
            Dim have As Pair(Of BenchmarkItem, List(Of TraceItem)) = Nothing
            For thread As Integer = 0 To threads - 1
                Dim item As Pair(Of BenchmarkItem, List(Of TraceItem)) = Nothing
                Dim t As Thread = New Thread(New ThreadStart(Sub() item = CreateTree(Calls(thread), Names(thread), Information(thread), Warning(thread), [Error](thread), Straight(thread), sessionId))) ' Force new ThreadId
                t.Start()
                t.Join()
                If expected.TryGetValue(item.First.ThreadId, have) Then
                    Dim plus As Integer = have.First.End
                    Dim old As Integer = item.First.End
                    Dim order As Queue(Of BenchmarkItem) = New(_, _)()
                    order.Enqueue(item.First)
                    While order.Count <> 0
                        Dim current As BenchmarkItem = order.Dequeue()
                        current.Start = plus
                        For Each child As BenchmarkItem In current.Children
                            order.Enqueue(child)
                        Next
                    End While

                    have.First.Children.AddRange(item.First.Children)
                    have.First.End = plus + old
                    have.Second.AddRange(item.Second)
                Else
                    expected(item.First.ThreadId) = item
                End If
            Next

            Return expected.Values.ToList()
        End Function

        Private Shared Function CreateTree(ByVal Calls As List(Of List(Of Integer)), ByVal Names As List(Of String), ByVal Information As List(Of List(Of String)), ByVal Warning As List(Of List(Of String)), ByVal [Error] As List(Of List(Of String)), ByVal Straight As Boolean, ByVal sessionId As Integer) As Pair(Of BenchmarkItem, List(Of TraceItem))
            Dim threadId As Integer = Thread.CurrentThread.ManagedThreadId
            Dim traceItems As List(Of TraceItem) = New(_, _)()
            CallTraces(Information(0), Warning(0), [Error](0), traceItems, threadId, sessionId)
            Dim threadBenchmarkItem As BenchmarkItem = New(_, _)()
            $"Thread{threadId}"
            sessionId
            threadId
            If True Then
                Children = CallChildren(0, Calls, Names, traceItems, Information, Warning, [Error], Straight, sessionId, threadId)
            End If

            threadBenchmarkItem.Start = 0
            threadBenchmarkItem.End = traceItems.Count
            Return New Pair(Of BenchmarkItem, List(Of TraceItem))(threadBenchmarkItem, traceItems)
        End Function

        Private Shared Sub AddChild(ByVal Positions As Dictionary(Of Integer, Integer), ByVal Numbers As Dictionary(Of String, Integer), ByVal Children As List(Of BenchmarkItem), ByVal child As BenchmarkItem, ByVal index As Integer)
            Dim position As Integer = Nothing, number As Integer = Nothing
            If Positions.TryGetValue(index, position) Then
                Children(position).Count += 1
                Children(position).End = child.End
            Else
                Positions(index) = Children.Count
                If Numbers.TryGetValue(child.Name, number) Then
                    number += 1
                Else
                    number = 1
                End If

                Numbers(child.Name) = number
                Children.Add(child)
            End If
        End Sub

        Private Shared Function CallChildren(ByVal Index As Integer, ByVal Calls As List(Of List(Of Integer)), ByVal Names As List(Of String), ByVal TraceItems As List(Of TraceItem), ByVal Information As List(Of List(Of String)), ByVal Warning As List(Of List(Of String)), ByVal [Error] As List(Of List(Of String)), ByVal Straight As Boolean, ByVal sessionId As Integer, ByVal threadId As Integer) As List(Of BenchmarkItem)
            Dim Positions As Dictionary(Of Integer, Integer) = New(_, _)()
            Dim Numbers As Dictionary(Of String, Integer) = New(_, _)()
            Dim Children As List(Of BenchmarkItem) = New(_, _)()
            If Straight Then
                For i As Integer = 0 To Calls(Index).Count - 1
                    For j As Integer = 0 To Calls(Index)(i) - 1
                        Dim child As BenchmarkItem = TestFunctions(i, Calls, Names, TraceItems, Information, Warning, [Error], Straight, sessionId, threadId)
                        AddChild(Positions, Numbers, Children, child, i)
                    Next
                Next
            Else
                Dim Called As List(Of Integer) = New(_, _)()
                For i As Integer = 0 To Calls(Index).Count - 1
                    Called.Add(0)
                Next

                Dim change As Boolean = True
                While change
                    change = False
                    For i As Integer = 0 To Calls(Index).Count - 1
                        If Called(i) < Calls(Index)(i) Then
                            Called(i) += 1
                            Dim child As BenchmarkItem = TestFunctions(i, Calls, Names, TraceItems, Information, Warning, [Error], Straight, sessionId, threadId)
                            AddChild(Positions, Numbers, Children, child, i)
                            change = True
                        End If
                    Next
                End While
            End If

            Return Children
        End Function

        Private Shared Sub CallTraces(ByVal Information As List(Of String), ByVal Warning As List(Of String), ByVal [Error] As List(Of String), ByVal TraceItems As List(Of TraceItem), ByVal threadId As Integer, ByVal sessionId As Integer)
            For Each lInformation As String In Information
                Call DashboardTelemetry.TraceInformation(lInformation)
                TraceItems.Add(New TraceItem(sessionId, threadId) With {.EventType = TraceEventType.Information, .Data = lInformation})
            Next

            For Each lWarning As String In Warning
                Call DashboardTelemetry.TraceWarning(lWarning)
                TraceItems.Add(New TraceItem(sessionId, threadId) With {.EventType = TraceEventType.Warning, .Data = lWarning})
            Next

            For Each lError As String In [Error]
                Call DashboardTelemetry.TraceError(lError)
                TraceItems.Add(New TraceItem(sessionId, threadId) With {.EventType = TraceEventType.Error, .Data = lError})
            Next
        End Sub

        Private Shared Function TestFunctions(ByVal Index As Integer, ByVal Calls As List(Of List(Of Integer)), ByVal Names As List(Of String), ByVal TraceItems As List(Of TraceItem), ByVal Information As List(Of List(Of String)), ByVal Warning As List(Of List(Of String)), ByVal [Error] As List(Of List(Of String)), ByVal Straight As Boolean, ByVal sessionId As Integer, ByVal threadId As Integer) As BenchmarkItem
            Return DashboardTelemetry.Log(Names(Index), Function()
                Dim current As BenchmarkItem = New(Names(), sessionId, threadId)()
                current.Start = TraceItems.Count
                CallTraces(Information(Index), Warning(Index), [Error](Index), TraceItems, threadId, sessionId)
                current.Children = CallChildren(Index, Calls, Names, TraceItems, Information, Warning, [Error], Straight, sessionId, threadId)
                current.End = TraceItems.Count
                Return current
            End Function)
        End Function

        Private Function GetActual(ByVal controller As DiagnosticController, ByVal sessionId As Integer) As List(Of Pair(Of BenchmarkItem, List(Of TraceItem)))
            Dim actualBenchmarks As List(Of BenchmarkItem) = controller.GetBenchmarks(sessionId)
            Dim actual As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = New(_, _)()
            For Each item As BenchmarkItem In actualBenchmarks
                actual.Add(New Pair(Of BenchmarkItem, List(Of TraceItem))(item, controller.GetTraceEvents(sessionId, item)))
            Next

            Return actual
        End Function

#End Region
        <Test>
        Public Sub MakeBambooTest()
            Dim controller As DiagnosticController = New(_, _)()
            controller.Start()
             ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<DevExpress.XtraPrinting.Native.Pair<DashboardDiagnosticTool.Data.BenchmarkItem, System.Collections.Generic.List<DashboardDiagnosticTool.Data.TraceItem>>> expected = ImitateWork(
'''                 threads: 1,
'''                 Calls: new() {
'''                     new() {
'''                         new() { 0, 1, 0, 0, 0 },
'''                         new() { 0, 0, 1, 0, 0 },
'''                         new() { 0, 0, 0, 1, 0 },
'''                         new() { 0, 0, 0, 0, 1 },
'''                         new() { 0, 0, 0, 0, 0 },
'''                     }
'''                 },
'''                 Names: new() {
'''                     new() {
'''                         "thread",
'''                         "one",
'''                         "two",
'''                         "three",
'''                         "four"
'''                     }
'''                 },
'''                 Information: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "inf1" },
'''                         new() { "inf2" },
'''                         new() { "inf3" },
'''                         new() { "inf4" }
'''                     }
'''                 },
'''                 Warning: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "warn1" },
'''                         new() { "warn2" },
'''                         new() { "warn3" },
'''                         new() { "warn4" }
'''                     }
'''                 },
'''                 Error: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "err1" },
'''                         new() { "err2" },
'''                         new() { "err3" },
'''                         new() { "err4" }
'''                     }
'''                 },
'''                 Straight: new() {
'''                     true
'''                 },
'''                 sessionId: 1
'''             );
''' 
'''  controller.Stop()
            Dim actual As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 1)
            TestTracing.CheckTrees(expected, actual)
        End Sub

        <Test>
        Public Sub MakeTreeTest()
            Dim controller As DiagnosticController = New(_, _)()
            controller.Start()
             ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<DevExpress.XtraPrinting.Native.Pair<DashboardDiagnosticTool.Data.BenchmarkItem, System.Collections.Generic.List<DashboardDiagnosticTool.Data.TraceItem>>> expected = ImitateWork(
'''                 threads: 1,
'''                 Calls: new() {
'''                     new() {
'''                         new() { 0, 1, 1, 0, 0, 0 },
'''                         new() { 0, 0, 1, 1, 0, 0 },
'''                         new() { 0, 0, 0, 0, 1, 1 },
'''                         new() { 0, 0, 0, 0, 0, 0 },
'''                         new() { 0, 0, 0, 0, 0, 0 },
'''                         new() { 0, 0, 0, 0, 0, 0 }
'''                     }
'''                 },
'''                 Names: new() {
'''                     new() {
'''                         "thread",
'''                         "1",
'''                         "2",
'''                         "3",
'''                         "4",
'''                         "5"
'''                     }
'''                 },
'''                 Information: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "inf11", "inf12" },
'''                         new() { "inf2" },
'''                         new() { "inf31", "inf32" },
'''                         new() { "inf4" },
'''                         new() { "inf51", "inf52", "inf53" }
'''                     }
'''                 },
'''                 Warning: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "warn1" },
'''                         new() { "warn21", "warn22", "warn23", "warn24" },
'''                         new() { "warn3" },
'''                         new() { "warn4" },
'''                         new() { "warn5" }
'''                     }
'''                 },
'''                 Error: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "err1" },
'''                         new() { },
'''                         new() { "err31", "err32" },
'''                         new() { },
'''                         new() { }
'''                     }
'''                 },
'''                 Straight: new() {
'''                     true
'''                 },
'''                 sessionId: 1
'''             );
''' 
'''  controller.Stop()
            Dim actual As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 1)
            TestTracing.CheckTrees(expected, actual)
        End Sub

        <Test>
        Public Sub MergeSameTest()
            Dim controller As DiagnosticController = New(_, _)()
            controller.Start()
            Dim tree As List(Of List(Of List(Of Integer))) = New(_, _) From {New(_, _) From {New(_, _) From {0, 5, 3, 0, 0, 0}, New(_, _) From {0, 0, 4, 6, 0, 0}, New(_, _) From {0, 0, 0, 0, 7, 8}, New(_, _) From {0, 0, 0, 0, 0, 9}, New(_, _) From {0, 0, 0, 0, 0, 3}, New(_, _) From {0, 0, 0, 0, 0, 0}}}
            Dim names As List(Of List(Of String)) = New(_, _) From {New(_, _) From {"thread", "1", "2", "3", "4", "5"}}
             ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<string>>> information = new() {
'''                 new() {
'''                     new() { },
'''                     new() { "inf11", "inf12", "inf13", "inf14" },
'''                     new() { "inf21", "inf22" },
'''                     new() { "inf31" },
'''                     new() { "inf41", "inf42", "inf43", "inf44", "inf45" },
'''                     new() { "inf51" }
'''                 }
'''             };
''' 
'''   ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<string>>> warning = new() {
'''                 new() {
'''                     new() { },
'''                     new() { "warn1" },
'''                     new() { "warn2" },
'''                     new() { "warn3" },
'''                     new() { "warn4" },
'''                     new() { "warn5" }
'''                 }
'''             };
''' 
'''   ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<string>>> error = new() {
'''                 new() {
'''                     new() { },
'''                     new() { "err1" },
'''                     new() { "err2" },
'''                     new() { "err3" },
'''                     new() { "err4" },
'''                     new() { "err5" }
'''                 }
'''             };
''' 
'''  Dim expected1 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = ImitateWork(threads:=1, Calls:=tree, Names:=names, Information:=information, Warning:=warning, [Error]:=[error], Straight:=New(_, _) From {True}, sessionId:=1)
            controller.Stop()
            Dim actual1 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 1)
            CheckTrees(expected1, actual1)
            controller.Start()
            Dim expected2 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = ImitateWork(threads:=1, Calls:=tree, Names:=names, Information:=information, Warning:=warning, [Error]:=[error], Straight:=New(_, _) From {False}, sessionId:=2)
            controller.Stop()
            Dim actual2 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 2)
            CheckTrees(expected2, actual2)
        End Sub

        <Test>
        Public Sub SameNamesTest()
            Dim controller As DiagnosticController = New(_, _)()
            controller.Start()
             ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<DevExpress.XtraPrinting.Native.Pair<DashboardDiagnosticTool.Data.BenchmarkItem, System.Collections.Generic.List<DashboardDiagnosticTool.Data.TraceItem>>> expected = ImitateWork(
'''                 threads: 1,
'''                 Calls: new() {
'''                     new() {
'''                         new() { 0, 2, 2, 0, 0, 0, 0 },
'''                         new() { 0, 0, 0, 2, 2, 2, 0 },
'''                         new() { 0, 0, 0, 2, 0, 2, 0 },
'''                         new() { 0, 0, 0, 0, 0, 4, 3 },
'''                         new() { 0, 0, 0, 0, 0, 3, 4 },
'''                         new() { 0, 0, 0, 0, 0, 0, 0 },
'''                         new() { 0, 0, 0, 0, 0, 0, 0 }
'''                     }
'''                 },
'''                 Names: new() {
'''                     new() {
'''                         "thread",
'''                         "1",
'''                         "1",
'''                         "2",
'''                         "2",
'''                         "3",
'''                         "4"
'''                     }
'''                 },
'''                 Information: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "inf11", "inf12", "inf13" },
'''                         new() { "inf21" },
'''                         new() { "inf31", "inf32" },
'''                         new() { "inf41" },
'''                         new() { "inf51", "inf51", "inf51" },
'''                         new() { "inf6" }
'''                     }
'''                 },
'''                 Warning: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "warn", "warn" },
'''                         new() { "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn" },
'''                         new() { },
'''                         new() { "warn", "warn" },
'''                         new() { "warn", "warn", "warn", "warn" },
'''                         new() { "warn", "warn", "warn", "warn", "warn", "warn" }
'''                     }
'''                 },
'''                 Error: new() {
'''                     new() {
'''                         new() { },
'''                         new() { },
'''                         new() { },
'''                         new() { },
'''                         new() { },
'''                         new() { },
'''                         new() { }
'''                     }
'''                 },
'''                 new() {
'''                     false
'''                 },
'''                 1);
''' 
'''  controller.Stop()
            Dim actual As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 1)
            TestTracing.CheckTrees(expected, actual)
        End Sub

        <Test>
        Public Sub ThreadLoggingTest()
            Dim controller As DiagnosticController = New DiagnosticController()
            controller.Start()
             ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node)
''' 
''' Input:
'''             System.Collections.Generic.List<DevExpress.XtraPrinting.Native.Pair<DashboardDiagnosticTool.Data.BenchmarkItem, System.Collections.Generic.List<DashboardDiagnosticTool.Data.TraceItem>>> expected = ImitateWork(
'''                 threads: 3,
'''                 Calls: new() {
'''                     new() {
'''                         new() { 0, 1 },
'''                         new() { 0, 0 }
'''                     },
'''                     new() {
'''                         new() { 0, 1, 0 },
'''                         new() { 0, 0, 1 },
'''                         new() { 0, 0, 0 }
'''                     },
'''                     new() {
'''                         new() { 0, 1, 2 },
'''                         new() { 0, 0, 0 },
'''                         new() { 0, 0, 0 }
'''                     }
'''                 },
'''                 Names: new() {
'''                     new() {
'''                         "thread1",
'''                         "1"
'''                     },
'''                     new() {
'''                         "thread2",
'''                         "2",
'''                         "3"
'''                     },
'''                     new() {
'''                         "thread3",
'''                         "4",
'''                         "5",
'''                     }
'''                 },
'''                 Information: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "inf1", "inf2" }
'''                     },
'''                     new() {
'''                         new() { },
'''                         new() { "inf1", "inf2", "inf1", "inf2", "inf1", "inf2" },
'''                         new() { "inf1", "inf2", "inf1", "inf2", "inf1", "inf2", "inf1", "inf2" }
'''                     },
'''                     new() {
'''                         new() { "inf3", "inf4", "inf4", "inf3" },
'''                         new() { "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3" },
'''                         new() { "int5" }
'''                     }
'''                 },
'''                 Warning: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "warn11", "warn12", "warn13" }
'''                     },
'''                     new() {
'''                         new() { },
'''                         new() { "warn11", "warn12", "warn13", "warn11", "warn12", "warn13" },
'''                         new() { "warn21", "warn22", "warn23" }
'''                     },
'''                     new() {
'''                         new() { },
'''                         new() { },
'''                         new() { }
'''                     }
'''                 },
'''                 Error: new() {
'''                     new() {
'''                         new() { },
'''                         new() { "err1" }
'''                     },
'''                     new() {
'''                         new() { },
'''                         new() { "err1", "err1", "err1", "err1", "err1" },
'''                         new() { "err1", "err2", "err1", "err1", "err2" }
'''                     },
'''                     new() {
'''                         new() { },
'''                         new() { "err1", "err2", "err3", "err4", "err5" },
'''                         new() { "err1", "err2", "err3", "err4", "err5", "err1", "err2", "err3", "err4", "err5" }
'''                     }
'''                 },
'''                 Straight: new() {
'''                     true,
'''                     true,
'''                     true
'''                 },
'''                 sessionId: 1);
''' 
'''  controller.Stop()
            Dim actual As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 1)
            TestTracing.CheckTrees(expected, actual)
        End Sub

#Region "one"
        Private Calls1 As List(Of List(Of Integer)) = New(_, _) From {New(_, _) From {0, 2, 2, 0, 0, 0, 0}, New(_, _) From {0, 0, 0, 2, 2, 2, 0}, New(_, _) From {0, 0, 0, 2, 0, 2, 0}, New(_, _) From {0, 0, 0, 0, 0, 4, 3}, New(_, _) From {0, 0, 0, 0, 0, 3, 4}, New(_, _) From {0, 0, 0, 0, 0, 0, 0}, New(_, _) From {0, 0, 0, 0, 0, 0, 0}}

        Private Names1 As List(Of String) = New(_, _) From {"thread", "1", "1", "2", "2", "3", "4"}

         ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> information1 = new() {
'''             new() { },
'''             new() { "inf11", "inf12", "inf13" },
'''             new() { "inf21" },
'''             new() { "inf31", "inf32" },
'''             new() { "inf41" },
'''             new() { "inf51", "inf51", "inf51" },
'''             new() { "inf6" }
'''         };
''' 
'''   ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> warning1 = new() {
'''             new() { },
'''             new() { "warn", "warn" },
'''             new() { "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn", "warn" },
'''             new() { },
'''             new() { "warn", "warn" },
'''             new() { "warn", "warn", "warn", "warn" },
'''             new() { "warn", "warn", "warn", "warn", "warn", "warn" }
'''         };
''' 
'''   ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> error1 = new() {
'''             new() { },
'''             new() { },
'''             new() { },
'''             new() { },
'''             new() { },
'''             new() { },
'''             new() { }
'''         };
''' 
'''  #End Region
#Region "two"
Private Calls2 As List(Of List(Of Integer)) = New(_, _) From {New(_, _) From {0, 1, 2}, New(_, _) From {0, 0, 0}, New(_, _) From {0, 0, 0}}

        Private Names2 As List(Of String) = New(_, _) From {"thread3", "5", "6"}

        Private information2 As List(Of List(Of String)) = New(_, _) From {New(_, _) From {"inf3", "inf4", "inf4", "inf3"}, New(_, _) From {"inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3", "inf3", "inf4", "inf4", "inf3"}, New(_, _) From {"int5"}}

         ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> warning2 = new() {
'''             new() { },
'''             new() { "warn11", "warn12", "warn13", "warn11", "warn12", "warn13" },
'''             new() { "warn21", "warn22", "warn23" }
'''         };
''' 
'''   ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> error2 = new() {
'''             new() { },
'''             new() { "err1", "err2", "err3", "err4", "err5" },
'''             new() { "err1", "err2", "err3", "err4", "err5", "err1", "err2", "err3", "err4", "err5" }
'''         };
''' 
'''  #End Region
#Region "three"
Private Calls3 As List(Of List(Of Integer)) = New(_, _) From {New(_, _) From {0, 5, 3, 0, 0, 0}, New(_, _) From {0, 0, 4, 6, 0, 0}, New(_, _) From {0, 0, 0, 0, 7, 8}, New(_, _) From {0, 0, 0, 0, 0, 9}, New(_, _) From {0, 0, 0, 0, 0, 3}, New(_, _) From {0, 0, 0, 0, 0, 0}}

        Private Names3 As List(Of String) = New(_, _) From {"thread", "7", "8", "9", "10", "11"}

         ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> information3 = new() {
'''             new() { },
'''             new() { "inf11", "inf12", "inf13", "inf14" },
'''             new() { "inf21", "inf22" },
'''             new() { "inf31" },
'''             new() { "inf41", "inf42", "inf43", "inf44", "inf45" },
'''             new() { "inf51" }
'''         };
''' 
'''   ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> warning3 = new() {
'''             new() { },
'''             new() { "warn1" },
'''             new() { "warn2" },
'''             new() { "warn3" },
'''             new() { "warn4" },
'''             new() { "warn5" }
'''         };
''' 
'''   ''' Cannot convert FieldDeclarationSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration)
'''    at ICSharpCode.CodeConverter.VB.NodesVisitor.VisitFieldDeclaration(FieldDeclarationSyntax node)
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingVisitorWrapper`1.Accept(SyntaxNode csNode, Boolean addSourceMapping)
''' 
''' Input:
'''         System.Collections.Generic.List<System.Collections.Generic.List<string>> error3 = new() {
'''             new() { },
'''             new() { "err1" },
'''             new() { },
'''             new() { "err31", "err32" },
'''             new() { },
'''             new() { }
'''         };
''' 
'''  #End Region
<Test>
        Public Sub ManySessionsTest()
#Region "first"
            Dim controller As DiagnosticController = New(_, _)()
            controller.Start()
            Dim expected1 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = ImitateWork(threads:=2, Calls:=New(_, _) From {Calls1, Calls2}, Names:=New(_, _) From {Names1, Names2}, Information:=New(_, _) From {Me.information1, information2}, Warning:=New(_, _) From {Me.warning1, Me.warning2}, [Error]:=New(_, _) From {Me.error1, Me.error2}, Straight:=New(_, _) From {False, False}, sessionId:=1)
            controller.Stop()
            Dim actual1 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 1)
            CheckTrees(expected1, actual1)
#End Region
#Region "second"
            controller.Start()
            Dim expected2 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = ImitateWork(threads:=2, Calls:=New(_, _) From {Calls1, Calls3}, Names:=New(_, _) From {Names1, Names3}, Information:=New(_, _) From {Me.information1, Me.information3}, Warning:=New(_, _) From {Me.warning1, Me.warning3}, [Error]:=New(_, _) From {Me.error1, Me.error3}, Straight:=New(_, _) From {False, False}, sessionId:=2)
            controller.Stop()
            Dim actual2 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 2)
            CheckTrees(expected2, actual2)
#End Region
#Region "third"
            controller.Start()
            Dim expected3 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = ImitateWork(threads:=3, Calls:=New(_, _) From {Calls1, Calls2, Calls3}, Names:=New(_, _) From {Names1, Names2, Names3}, Information:=New(_, _) From {Me.information1, information2, Me.information3}, Warning:=New(_, _) From {Me.warning1, Me.warning2, Me.warning3}, [Error]:=New(_, _) From {Me.error1, Me.error2, Me.error3}, Straight:=New(_, _) From {False, False, False}, sessionId:=3)
            controller.Stop()
            Dim actual3 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 3)
            CheckTrees(expected3, actual3)
#End Region
#Region "fourth"
            controller.Start()
            Dim expected4 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = ImitateWork(threads:=2, Calls:=New(_, _) From {Calls2, Calls3}, Names:=New(_, _) From {Names2, Names3}, Information:=New(_, _) From {information2, Me.information3}, Warning:=New(_, _) From {Me.warning2, Me.warning3}, [Error]:=New(_, _) From {Me.error2, Me.error3}, Straight:=New(_, _) From {False, False}, sessionId:=4)
            controller.Stop()
            Dim actual4 As List(Of Pair(Of BenchmarkItem, List(Of TraceItem))) = GetActual(controller, 4)
            CheckTrees(expected4, actual4)
#End Region
        End Sub

        <Test>
        Public Sub SerializationTest()
            Dim one As DiagnosticController = New(_, _)()
            one.Start()
            Dim expected1 = ImitateWork(threads:=2, Calls:=New(_, _) From {Calls1, Calls3}, Names:=New(_, _) From {Names1, Names3}, Information:=New(_, _) From {Me.information1, Me.information3}, Warning:=New(_, _) From {Me.warning1, Me.warning3}, [Error]:=New(_, _) From {Me.error1, Me.error3}, Straight:=New(_, _) From {True, False}, 1)
            one.Stop()
            one.Start()
            Dim expected2 = ImitateWork(threads:=1, Calls:=New(_, _) From {Calls2}, Names:=New(_, _) From {Names2}, Information:=New(_, _) From {information2}, Warning:=New(_, _) From {Me.warning2}, [Error]:=New(_, _) From {Me.error2}, Straight:=New(_, _) From {True}, 2)
            one.Stop()
            Dim save As String = "TestSave.xml"
            one.SaveAs(save)
            Dim two As DiagnosticController = New(_, _)()
            two.Open(save)
            Dim actual1 = GetActual(two, 1)
            CheckTrees(expected1, actual1)
            Dim actual2 = GetActual(two, 2)
            CheckTrees(expected2, actual2)
            If File.Exists(save) Then File.Delete(save)
        End Sub

        <Test>
        Public Sub DeletingSessionTest()
            Dim controller As DiagnosticController = New(_, _)()
            Dim expected As List(Of List(Of Pair(Of BenchmarkItem, List(Of TraceItem)))) = New(_, _)()
            controller.Start()
            expected.Add(ImitateWork(threads:=1, Calls:=New(_, _) From {Calls2}, Names:=New(_, _) From {Names2}, Information:=New(_, _) From {information2}, Warning:=New(_, _) From {Me.warning2}, [Error]:=New(_, _) From {Me.error2}, Straight:=New(_, _) From {True}, 1))
            controller.Stop()
            controller.Start()
            expected.Add(ImitateWork(threads:=1, Calls:=New(_, _) From {Calls2}, Names:=New(_, _) From {Names2}, Information:=New(_, _) From {information2}, Warning:=New(_, _) From {Me.warning2}, [Error]:=New(_, _) From {Me.error2}, Straight:=New(_, _) From {True}, 2))
            controller.Stop()
            controller.Start()
            expected.Add(ImitateWork(threads:=1, Calls:=New(_, _) From {Calls1}, Names:=New(_, _) From {Names1}, Information:=New(_, _) From {Me.information1}, Warning:=New(_, _) From {Me.warning1}, [Error]:=New(_, _) From {Me.error1}, Straight:=New(_, _) From {True}, 3))
            controller.Stop()
            controller.Start()
            expected.Add(ImitateWork(threads:=1, Calls:=New(_, _) From {Calls1}, Names:=New(_, _) From {Names1}, Information:=New(_, _) From {Me.information1}, Warning:=New(_, _) From {Me.warning1}, [Error]:=New(_, _) From {Me.error1}, Straight:=New(_, _) From {True}, 4))
            controller.Stop()
            controller.Start()
            expected.Add(ImitateWork(threads:=1, Calls:=New(_, _) From {Calls3}, Names:=New(_, _) From {Names3}, Information:=New(_, _) From {Me.information3}, Warning:=New(_, _) From {Me.warning3}, [Error]:=New(_, _) From {Me.error3}, Straight:=New(_, _) From {True}, 5))
            controller.Stop()
            For i As Integer = 0 To 5 - 1
                Assert.AreEqual(5 - i, controller.Sessions.Count)
                For j As Integer = i To 5 - 1
                    Dim actual = GetActual(controller, j + 1)
                    CheckTrees(expected(j), actual)
                Next

                controller.Delete(controller.Sessions(0))
            Next

            Assert.AreEqual(0, controller.Sessions.Count)
        End Sub
    End Class
End Namespace
