using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DashboardDiagnosticTool;
using DashboardDiagnosticTool.Data;
using DevExpress.Utils.About;
using DevExpress.Utils.CommonDialogs.Internal;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

namespace DashboardDiagnosticToolUI {
    public partial class DiagnosticForm : XtraForm {
        IList<SessionItem> Sessions => controller.Sessions;
        bool InProcess => controller.InProcess;
        int processCount = 0;

        Dictionary<BenchmarkItem, bool> benchmarkState = new Dictionary<BenchmarkItem, bool>();
        DiagnosticController controller = new DiagnosticController(new FileController());
        WaitForm1 waitForm;

        public void GetCommand(string command, object[] args) {
            switch(command.ToLower()) {
                case "open":
                    controller.Open();
                    break;
                case "save":
                    controller.Save();
                    break;
                case "saveas":
                    controller.SaveAs();
                    break;
                case "start":
                    controller.Start();
                    break;
                case "stop":
                    controller.Stop();
                    break;
                case "delete":
                    controller.Delete((SessionItem)args[0]);
                    break;
                case "about":
                    AboutHelper.Show(ProductKind.XtraReports, ProductInfoHelper.GetProductInfo(ProductKind.XtraReports));
                    break;
            }
        }

        public bool CanHandleCommand(string command) {
            switch(command.ToLower()) {
                case "open":
                    return controller.CanHandleCommand(ControllerCommand.Open);
                case "save":
                    return controller.CanHandleCommand(ControllerCommand.Save);
                case "saveas":
                    return controller.CanHandleCommand(ControllerCommand.SaveAs);
                case "start":
                    return controller.CanHandleCommand(ControllerCommand.Start);
                case "stop":
                    return controller.CanHandleCommand(ControllerCommand.Stop);
                case "delete":
                    return controller.CanHandleCommand(ControllerCommand.Delete);
                case "about":
                    return true;
            }
            return false;
        }

        public DiagnosticForm() {
            InitializeComponent();
            treeListSession.DataSource = Sessions;
            controller.ThrowException += OnThrowException;
            controller.SessionsChanged += OnSessionsChanged;
            controller.StartProcessing += OnStartProcessing;
            controller.EndProcessing += OnEndProcessing;
            UpdateBarItems();
        }
        void OnStartProcessing() {
            Interlocked.Increment(ref processCount);
            if(processCount > 0) return;
            BeginMethod(() => {
                waitForm = new WaitForm1() { StartPosition = FormStartPosition.Manual };
                waitForm.Location = GetWaitLocation(waitForm.ClientRectangle);
                waitForm.Show(this);
            });
        }
        Point GetWaitLocation(Rectangle waitRect) {
            var rect = RectHelper.AlignRectangle(waitRect, ClientRectangle, DevExpress.XtraPrinting.ImageAlignment.MiddleCenter);
            return PointToScreen(rect.Location);
        }
        void OnEndProcessing() {
            Interlocked.Decrement(ref processCount);
            if(processCount > 0) return;
            BeginMethod(() => {
                waitForm?.Close();
                waitForm = null;
            });
        }
        void OnThrowException(Exception e) {
            if(InvokeRequired)
                BeginMethod(ShowError, e);
            else
                ShowError(e);
        }
        void OnSessionsChanged() {
            BeginMethod(() => {
                treeListSession.RefreshDataSource();
                if(InProcess)
                    FocusNode(treeListSession, Sessions.Count - 1);
                SyncState();
                UpdateBarItems();
            });
        }
        void BeginMethod(Action method) {
            BeginInvoke(method);
        }
        void BeginMethod<T>(Action<T> method, T arg) {
            BeginInvoke(new Action<T>(method), arg);
        }
        void SyncState() {
            var todelete = benchmarkState.Keys.Where(x => !controller.Benchmarks.ContainsKey(x.SessionId)).ToArray();
            foreach(var item in todelete)
                benchmarkState.Remove(item);
        }
        static void FocusNode(TreeList treeList, int id) {
            var node = treeList.FindNodeByID(id);
            if(node != null) {
                node.ParentNode?.Expand();
                treeList.FocusedNode = node;
            }
        }
        protected override void Dispose(bool disposing) {
            if(disposing) {
                controller.Dispose();
                components?.Dispose();
                waitForm?.Close();
                waitForm = null;
            }
            base.Dispose(disposing);
        }
        void UpdateBarItems() {
            barItemStatus.Caption = InProcess ? "In process" : "Stopped";
            foreach(var item in new BarItem[] { barItemStop, barItemStart, barItemOpen, barItemDelete, barItemSave, barItemSaveAs })
                item.Enabled = CanHandleCommand(item.Tag as string);
        }
        void barManager_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var command = (e.Item.Tag as string);
            if(string.IsNullOrEmpty(command)) return;
            var item = treeListSession.GetFocusedRow() as SessionItem;
            HandleCommand(command, new object[] { item });
        }
        void OnKeyDown(object sender, KeyEventArgs e) {
            switch(e.KeyCode) {
                case Keys.Delete:
                    var item = treeListSession.GetFocusedRow() as SessionItem;
                    HandleCommand("delete", new object[] { item });
                    e.Handled = true;
                    break;
                case Keys.F5:
                    HandleCommand(e.Shift ? "stop" : "start");
                    e.Handled = true;
                    break;
                case Keys.S:
                    if(e.Control) {
                        HandleCommand(e.Shift? "saveas": "save");
                        e.Handled = true;
                    }
                    break;
                case Keys.O:
                    if(e.Control) {
                        HandleCommand("open");
                        e.Handled = true;
                    }
                    break;
                case Keys.Right:
                    TreeListNode focusedNodeExpand = treeListBenchmark.FocusedNode;
                    if (focusedNodeExpand.HasChildren)
                        focusedNodeExpand.Expand();
                    break;
                case Keys.Left:
                    TreeListNode focusedNodeCollapse = treeListBenchmark.FocusedNode;
                    focusedNodeCollapse.Collapse();
                    break;
            }
        }
        void HandleCommand(string command, object[] args = null) {
            switch(command.ToLower()) {
                case "exit":
                    Close();
                    return;
            }
            if(CanHandleCommand(command)) {
                GetCommand(command, args);
                UpdateBarItems();
            }
        }
        void ShowError(Exception e) {
            XtraMessageBox.Show(LookAndFeel, this, e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        void ShowSession(int session) {
            treeListBenchmark.DataSource = controller.GetBenchmarks(session);
            gridEvent.DataSource = controller.GetTraceEvents(session);
        }
        void ClearSession() {
            treeListBenchmark.DataSource = null;
            gridEvent.DataSource = null;
        }
        void treeListSession_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e) {
            var item = treeListSession.GetFocusedRow() as SessionItem;
            if(item != null) {
                IterateBenchmarks((node, bm) => benchmarkState[bm] = node.Expanded);
                ShowSession(item.ID);
                IterateBenchmarks((node, bm) => node.Expanded = GetExpanded(bm));
            } else {
                ClearSession();
            }
        }

        void treeListBenchmark_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e) {
            var item = treeListBenchmark.GetFocusedRow() as BenchmarkItem;
            if(item != null) {
                gridEvent.DataSource = controller.GetTraceEvents(item.SessionId, item);
            }
        }
        bool GetExpanded(BenchmarkItem bm) {
            bool value;
            return benchmarkState.TryGetValue(bm, out value) ? value : false;
        }
        void IterateBenchmarks(Action<TreeListNode, BenchmarkItem> action) {
            treeListBenchmark.NodesIterator.Do(x => {
                var item = treeListBenchmark.GetDataRecordByNode(x) as BenchmarkItem;
                if(item != null) action(x, item);
            });
        }
    }

    class FileController : IFileController {
        const string fileExt = "xml";
        public bool TryOpenFile(out string openName, string fileName = "") {
            using(var dlg = CommonDialogProvider.Instance.CreateDefaultOpenFileDialog()) {
                dlg.Title = "Open";
                dlg.ValidateNames = true;
                dlg.Filter = $"Diagnostic files (*.{fileExt})|*.{fileExt}";
                dlg.FilterIndex = 1;

                if(dlg.ShowDialog() == DevExpress.Utils.CommonDialogs.Internal.DialogResult.OK) {
                    openName = dlg.FileName;
                    return true;
                }
                openName = "";
                return false;
            }
        }
        public bool TrySaveFile(out string outFileName, string fileName = "") {
            using(var dlg = CommonDialogProvider.Instance.CreateDefaultSaveFileDialog()) {
                dlg.Title = "Saving";
                dlg.ValidateNames = true;
                dlg.Filter = $"Diagnostic files (*.{fileExt})|*.{fileExt}";
                dlg.FilterIndex = 1;
                dlg.FileName = fileName;
                dlg.OverwritePrompt = true;

                if(dlg.ShowDialog() == DevExpress.Utils.CommonDialogs.Internal.DialogResult.OK) {
                    outFileName = dlg.FileName;
                    return true;
                }
                outFileName = "";
                return false;
            }
        }
    }
}
