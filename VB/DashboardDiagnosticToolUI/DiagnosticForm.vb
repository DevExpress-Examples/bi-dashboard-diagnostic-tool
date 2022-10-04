Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Threading
Imports System.Windows.Forms
Imports DashboardDiagnosticTool
Imports DashboardDiagnosticTool.Data
Imports DevExpress.Utils.About
Imports DevExpress.Utils.CommonDialogs.Internal
Imports DevExpress.XtraBars
Imports DevExpress.XtraEditors
Imports DevExpress.XtraPrinting.Native
Imports DevExpress.XtraTreeList
Imports DevExpress.XtraTreeList.Nodes
Imports System.Runtime.InteropServices

Namespace DashboardDiagnosticToolUI

    Public Partial Class DiagnosticForm
        Inherits XtraForm

        Private ReadOnly Property Sessions As IList(Of SessionItem)
            Get
                Return controller.Sessions
            End Get
        End Property

        Private ReadOnly Property InProcess As Boolean
            Get
                Return controller.InProcess
            End Get
        End Property

        Private processCount As Integer = 0

        Private benchmarkState As Dictionary(Of BenchmarkItem, Boolean) = New Dictionary(Of BenchmarkItem, Boolean)()

        Private controller As DiagnosticController = New DiagnosticController(New FileController())

        Private waitForm As WaitForm1

        Public Sub GetCommand(ByVal command As String, ByVal args As Object())
            Select Case command.ToLower()
                Case "open"
                    controller.Open()
                Case "save"
                    controller.Save()
                Case "saveas"
                    controller.SaveAs()
                Case "start"
                    controller.Start()
                Case "stop"
                    controller.[Stop]()
                Case "delete"
                    controller.Delete(CType(args(0), SessionItem))
                Case "about"
                    AboutHelper.Show(ProductKind.XtraReports, ProductInfoHelper.GetProductInfo(ProductKind.XtraReports))
            End Select
        End Sub

        Public Function CanHandleCommand(ByVal command As String) As Boolean
            Select Case command.ToLower()
                Case "open"
                    Return controller.CanHandleCommand(ControllerCommand.Open)
                Case "save"
                    Return controller.CanHandleCommand(ControllerCommand.Save)
                Case "saveas"
                    Return controller.CanHandleCommand(ControllerCommand.SaveAs)
                Case "start"
                    Return controller.CanHandleCommand(ControllerCommand.Start)
                Case "stop"
                    Return controller.CanHandleCommand(ControllerCommand.[Stop])
                Case "delete"
                    Return controller.CanHandleCommand(ControllerCommand.Delete)
                Case "about"
                    Return True
            End Select

            Return False
        End Function

        Public Sub New()
            InitializeComponent()
            treeListSession.DataSource = Sessions
            Me.controller.ThrowException += AddressOf OnThrowException
            Me.controller.SessionsChanged += AddressOf OnSessionsChanged
            Me.controller.StartProcessing += AddressOf OnStartProcessing
            Me.controller.EndProcessing += AddressOf OnEndProcessing
            UpdateBarItems()
        End Sub

        Private Sub OnStartProcessing()
            Interlocked.Increment(processCount)
            If processCount > 0 Then Return
            BeginMethod(Sub()
                waitForm = New WaitForm1() With {.StartPosition = FormStartPosition.Manual}
                waitForm.Location = GetWaitLocation(waitForm.ClientRectangle)
                waitForm.Show(Me)
            End Sub)
        End Sub

        Private Function GetWaitLocation(ByVal waitRect As Rectangle) As Point
            Dim rect = RectHelper.AlignRectangle(waitRect, ClientRectangle, ContentAlignment.MiddleCenter)
            Return PointToScreen(rect.Location)
        End Function

        Private Sub OnEndProcessing()
            Interlocked.Decrement(processCount)
            If processCount > 0 Then Return
            BeginMethod(Sub()
                waitForm?.Close()
                waitForm = Nothing
            End Sub)
        End Sub

        Private Sub OnThrowException(ByVal e As Exception)
            If InvokeRequired Then
                BeginMethod(New Action(Of Exception)(AddressOf ShowError), e)
            Else
                ShowError(e)
            End If
        End Sub

        Private Sub OnSessionsChanged()
            BeginMethod(Sub()
                treeListSession.RefreshDataSource()
                If InProcess Then FocusNode(treeListSession, Sessions.Count - 1)
                SyncState()
                UpdateBarItems()
            End Sub)
        End Sub

        Private Sub BeginMethod(ByVal method As Action)
            BeginInvoke(method)
        End Sub

        Private Sub BeginMethod(Of T)(ByVal method As Action(Of T), ByVal arg As T)
            Me.BeginInvoke(New Action(Of T)(method), arg)
        End Sub

        Private Sub SyncState()
            Dim todelete = benchmarkState.Keys.Where(Function(x) Not controller.Benchmarks.ContainsKey(x.SessionId)).ToArray()
            For Each item In todelete
                benchmarkState.Remove(item)
            Next
        End Sub

        Private Shared Sub FocusNode(ByVal treeList As TreeList, ByVal id As Integer)
            Dim node = treeList.FindNodeByID(id)
            If node IsNot Nothing Then
                node.ParentNode?.Expand()
                treeList.FocusedNode = node
            End If
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                controller.Dispose()
                components?.Dispose()
                waitForm?.Close()
                waitForm = Nothing
            End If

            MyBase.Dispose(disposing)
        End Sub

        Private Sub UpdateBarItems()
            barItemStatus.Caption = If(InProcess, "In process", "Stopped")
            For Each item In New BarItem() {barItemStop, barItemStart, barItemOpen, barItemDelete, barItemSave, barItemSaveAs}
                item.Enabled = CanHandleCommand(TryCast(item.Tag, String))
            Next
        End Sub

        Private Sub barManager_ItemClick(ByVal sender As Object, ByVal e As ItemClickEventArgs)
            Dim command = TryCast(e.Item.Tag, String)
            If String.IsNullOrEmpty(command) Then Return
            Dim item = TryCast(treeListSession.GetFocusedRow(), SessionItem)
            Me.HandleCommand(command, New Object() {item})
        End Sub

        Private Overloads Sub OnKeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            Select Case e.KeyCode
                Case Keys.Delete
                    Dim item = TryCast(treeListSession.GetFocusedRow(), SessionItem)
                    Me.HandleCommand("delete", New Object() {item})
                    e.Handled = True
                Case Keys.F5
                    HandleCommand(If(e.Shift, "stop", "start"))
                    e.Handled = True
                Case Keys.S
                    If e.Control Then
                        HandleCommand(If(e.Shift, "saveas", "save"))
                        e.Handled = True
                    End If

                Case Keys.O
                    If e.Control Then
                        HandleCommand("open")
                        e.Handled = True
                    End If

                Case Keys.Right
                    Dim focusedNodeExpand As TreeListNode = treeListBenchmark.FocusedNode
                    If focusedNodeExpand.HasChildren Then focusedNodeExpand.Expand()
                Case Keys.Left
                    Dim focusedNodeCollapse As TreeListNode = treeListBenchmark.FocusedNode
                    focusedNodeCollapse.Collapse()
            End Select
        End Sub

        Private Sub HandleCommand(ByVal command As String, ByVal Optional args As Object() = Nothing)
            Select Case command.ToLower()
                Case "exit"
                    Close()
                    Return
            End Select

            If CanHandleCommand(command) Then
                GetCommand(command, args)
                UpdateBarItems()
            End If
        End Sub

        Private Sub ShowError(ByVal e As Exception)
            XtraMessageBox.Show(LookAndFeel, Me, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Sub

        Private Sub ShowSession(ByVal session As Integer)
            treeListBenchmark.DataSource = controller.GetBenchmarks(session)
            gridEvent.DataSource = controller.GetTraceEvents(session)
        End Sub

        Private Sub ClearSession()
            treeListBenchmark.DataSource = Nothing
            gridEvent.DataSource = Nothing
        End Sub

        Private Sub treeListSession_FocusedNodeChanged(ByVal sender As Object, ByVal e As FocusedNodeChangedEventArgs)
            Dim item = TryCast(treeListSession.GetFocusedRow(), SessionItem)
            If item IsNot Nothing Then
                Me.IterateBenchmarks(Sub(node, bm) benchmarkState(bm) = node.Expanded)
                ShowSession(item.ID)
                Me.IterateBenchmarks(Sub(node, bm) node.Expanded = Me.GetExpanded(bm))
            Else
                ClearSession()
            End If
        End Sub

        Private Sub treeListBenchmark_FocusedNodeChanged(ByVal sender As Object, ByVal e As FocusedNodeChangedEventArgs)
            Dim item = TryCast(treeListBenchmark.GetFocusedRow(), BenchmarkItem)
            If item IsNot Nothing Then
                gridEvent.DataSource = controller.GetTraceEvents(item.SessionId, item)
            End If
        End Sub

        Private Function GetExpanded(ByVal bm As BenchmarkItem) As Boolean
            Dim value As Boolean
            Return If(benchmarkState.TryGetValue(bm, value), value, False)
        End Function

        Private Sub IterateBenchmarks(ByVal action As Action(Of TreeListNode, BenchmarkItem))
            treeListBenchmark.NodesIterator.[Do](Sub(x)
                Dim item = TryCast(treeListBenchmark.GetDataRecordByNode(x), BenchmarkItem)
                If item IsNot Nothing Then action(x, item)
            End Sub)
        End Sub
    End Class

    Friend Class FileController
        Inherits IFileController

        Const fileExt As String = "xml"

        Public Function TryOpenFile(<Out> ByRef openName As String, ByVal Optional fileName As String = "") As Boolean
            Using dlg = CommonDialogProvider.Instance.CreateDefaultOpenFileDialog()
                dlg.Title = "Open"
                dlg.ValidateNames = True
                dlg.Filter = $"Diagnostic files (*.{fileExt})|*.{fileExt}"
                dlg.FilterIndex = 1
                If dlg.ShowDialog() = DevExpress.Utils.CommonDialogs.Internal.DialogResult.OK Then
                    openName = dlg.FileName
                    Return True
                End If

                openName = ""
                Return False
            End Using
        End Function

        Public Function TrySaveFile(<Out> ByRef outFileName As String, ByVal Optional fileName As String = "") As Boolean
            Using dlg = CommonDialogProvider.Instance.CreateDefaultSaveFileDialog()
                dlg.Title = "Saving"
                dlg.ValidateNames = True
                dlg.Filter = $"Diagnostic files (*.{fileExt})|*.{fileExt}"
                dlg.FilterIndex = 1
                dlg.FileName = fileName
                dlg.OverwritePrompt = True
                If dlg.ShowDialog() = DevExpress.Utils.CommonDialogs.Internal.DialogResult.OK Then
                    outFileName = dlg.FileName
                    Return True
                End If

                outFileName = ""
                Return False
            End Using
        End Function
    End Class
End Namespace
