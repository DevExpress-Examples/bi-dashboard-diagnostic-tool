namespace DashboardDiagnosticToolUI {
    public partial class DiagnosticForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiagnosticForm));
            this.bindingSourceBenchmark = new System.Windows.Forms.BindingSource(this.components);
            this.treeListBenchmark = new DevExpress.XtraTreeList.TreeList();
            this.colBMName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colBMCount = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colBMSecs = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.barMainMenu = new DevExpress.XtraBars.Bar();
            this.barItemFile = new DevExpress.XtraBars.BarSubItem();
            this.barItemOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barItemSave = new DevExpress.XtraBars.BarButtonItem();
            this.barItemSaveAs = new DevExpress.XtraBars.BarButtonItem();
            this.barItemExit = new DevExpress.XtraBars.BarButtonItem();
            this.barItemDiagnostic = new DevExpress.XtraBars.BarSubItem();
            this.barItemStart = new DevExpress.XtraBars.BarButtonItem();
            this.barItemStop = new DevExpress.XtraBars.BarButtonItem();
            this.barItemDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barItemHelp = new DevExpress.XtraBars.BarSubItem();
            this.barItemViewHelp = new DevExpress.XtraBars.BarButtonItem();
            this.barStatus = new DevExpress.XtraBars.Bar();
            this.barItemStatus = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelSession = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.treeListSession = new DevExpress.XtraTreeList.TreeList();
            this.colSSName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.bindingSourceSession = new System.Windows.Forms.BindingSource(this.components);
            this.panelEvents = new DevExpress.XtraBars.Docking.DockPanel();
            this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
            this.gridEvent = new DevExpress.XtraGrid.GridControl();
            this.bindingSourceEvent = new System.Windows.Forms.BindingSource(this.components);
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colEventType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colData = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelResult = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBenchmark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListBenchmark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            this.panelSession.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSession)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSession)).BeginInit();
            this.panelEvents.SuspendLayout();
            this.controlContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEvent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEvent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.panelResult.SuspendLayout();
            this.dockPanel2_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingSourceBenchmark
            // 
            this.bindingSourceBenchmark.DataSource = typeof(DashboardDiagnosticTool.Data.BenchmarkItem);
            // 
            // treeListBenchmark
            // 
            this.treeListBenchmark.ChildListFieldName = "Children";
            this.treeListBenchmark.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colBMName,
            this.colBMCount,
            this.colBMSecs});
            this.treeListBenchmark.DataSource = this.bindingSourceBenchmark;
            this.treeListBenchmark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListBenchmark.Location = new System.Drawing.Point(0, 0);
            this.treeListBenchmark.Name = "treeListBenchmark";
            this.treeListBenchmark.OptionsBehavior.Editable = false;
            this.treeListBenchmark.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListBenchmark.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
            this.treeListBenchmark.OptionsView.ShowIndicator = false;
            this.treeListBenchmark.OptionsView.ShowSummaryFooter = true;
            this.treeListBenchmark.Size = new System.Drawing.Size(674, 329);
            this.treeListBenchmark.TabIndex = 0;
            this.treeListBenchmark.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListBenchmark_FocusedNodeChanged);
            this.treeListBenchmark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // colBMName
            // 
            this.colBMName.FieldName = "Name";
            this.colBMName.Name = "colBMName";
            this.colBMName.Visible = true;
            this.colBMName.VisibleIndex = 0;
            // 
            // colBMCount
            // 
            this.colBMCount.FieldName = "Count";
            this.colBMCount.Name = "colBMCount";
            this.colBMCount.Visible = true;
            this.colBMCount.VisibleIndex = 1;
            // 
            // colBMSecs
            // 
            this.colBMSecs.FieldName = "MSecs";
            this.colBMSecs.Format.FormatString = "{0:0.0}";
            this.colBMSecs.Format.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colBMSecs.Name = "colBMSecs";
            this.colBMSecs.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
            this.colBMSecs.SummaryFooterStrFormat = "Total: {0:0,0.0}";
            this.colBMSecs.Visible = true;
            this.colBMSecs.VisibleIndex = 2;
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.MenuManager = this.barManager;
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelSession,
            this.panelEvents,
            this.panelResult});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.barMainMenu,
            this.barStatus});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barItemFile,
            this.barItemOpen,
            this.barItemSave,
            this.barItemSaveAs,
            this.barItemExit,
            this.barItemDiagnostic,
            this.barItemStart,
            this.barItemStatus,
            this.barItemStop,
            this.barItemDelete,
            this.barItemHelp,
            this.barItemViewHelp});
            this.barManager.MainMenu = this.barMainMenu;
            this.barManager.MaxItemId = 15;
            this.barManager.StatusBar = this.barStatus;
            this.barManager.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barManager_ItemClick);
            // 
            // barMainMenu
            // 
            this.barMainMenu.BarName = "Main menu";
            this.barMainMenu.DockCol = 0;
            this.barMainMenu.DockRow = 0;
            this.barMainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.barMainMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemFile),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemDiagnostic),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemHelp)});
            this.barMainMenu.OptionsBar.MultiLine = true;
            this.barMainMenu.OptionsBar.UseWholeRow = true;
            this.barMainMenu.Text = "Main menu";
            // 
            // barItemFile
            // 
            this.barItemFile.Caption = "File";
            this.barItemFile.Id = 0;
            this.barItemFile.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemOpen),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemSaveAs),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemExit, true)});
            this.barItemFile.Name = "barItemFile";
            // 
            // barItemOpen
            // 
            this.barItemOpen.Caption = "Open";
            this.barItemOpen.ContentHorizontalAlignment = DevExpress.XtraBars.BarItemContentAlignment.Far;
            this.barItemOpen.Id = 1;
            this.barItemOpen.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemOpen.ImageOptions.SvgImage")));
            this.barItemOpen.Name = "barItemOpen";
            this.barItemOpen.ShortcutKeyDisplayString = "Ctrl+O";
            this.barItemOpen.Tag = "Open";
            // 
            // barItemSave
            // 
            this.barItemSave.Caption = "Save";
            this.barItemSave.Id = 2;
            this.barItemSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemSave.ImageOptions.SvgImage")));
            this.barItemSave.Name = "barItemSave";
            this.barItemSave.ShortcutKeyDisplayString = "Ctrl+S";
            this.barItemSave.Tag = "Save";
            // 
            // barItemSaveAs
            // 
            this.barItemSaveAs.Caption = "Save As...";
            this.barItemSaveAs.Id = 3;
            this.barItemSaveAs.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemSaveAs.ImageOptions.SvgImage")));
            this.barItemSaveAs.Name = "barItemSaveAs";
            this.barItemSaveAs.ShortcutKeyDisplayString = "Ctrl+Shift+S";
            this.barItemSaveAs.Tag = "SaveAs";
            // 
            // barItemExit
            // 
            this.barItemExit.Caption = "Exit";
            this.barItemExit.Id = 4;
            this.barItemExit.Name = "barItemExit";
            this.barItemExit.ShortcutKeyDisplayString = "Alt+F4";
            this.barItemExit.Tag = "Exit";
            // 
            // barItemDiagnostic
            // 
            this.barItemDiagnostic.Caption = "Diagnostic";
            this.barItemDiagnostic.Id = 5;
            this.barItemDiagnostic.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemStart),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemStop),
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemDelete)});
            this.barItemDiagnostic.Name = "barItemDiagnostic";
            // 
            // barItemStart
            // 
            this.barItemStart.Caption = "Start Session";
            this.barItemStart.Id = 6;
            this.barItemStart.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemStart.ImageOptions.SvgImage")));
            this.barItemStart.Name = "barItemStart";
            this.barItemStart.ShortcutKeyDisplayString = "F5";
            this.barItemStart.Tag = "Start";
            // 
            // barItemStop
            // 
            this.barItemStop.Caption = "Stop Session";
            this.barItemStop.Id = 8;
            this.barItemStop.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemStop.ImageOptions.SvgImage")));
            this.barItemStop.Name = "barItemStop";
            this.barItemStop.ShortcutKeyDisplayString = "Shift+F5";
            this.barItemStop.Tag = "Stop";
            // 
            // barItemDelete
            // 
            this.barItemDelete.Caption = "Delete";
            this.barItemDelete.Id = 9;
            this.barItemDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemDelete.ImageOptions.SvgImage")));
            this.barItemDelete.Name = "barItemDelete";
            this.barItemDelete.ShortcutKeyDisplayString = "Delete";
            this.barItemDelete.Tag = "Delete";
            // 
            // barItemHelp
            // 
            this.barItemHelp.Caption = "Help";
            this.barItemHelp.Id = 12;
            this.barItemHelp.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemViewHelp)});
            this.barItemHelp.Name = "barItemHelp";
            // 
            // barItemViewHelp
            // 
            this.barItemViewHelp.Caption = "View Help";
            this.barItemViewHelp.Id = 13;
            this.barItemViewHelp.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barItemViewHelp.ImageOptions.SvgImage")));
            this.barItemViewHelp.Name = "barItemViewHelp";
            this.barItemViewHelp.Tag = "Help";
            this.barItemViewHelp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItemViewHelp_ItemClick);
            // 
            // barStatus
            // 
            this.barStatus.BarName = "Status bar";
            this.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.barStatus.DockCol = 0;
            this.barStatus.DockRow = 0;
            this.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.barStatus.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barItemStatus)});
            this.barStatus.OptionsBar.AllowQuickCustomization = false;
            this.barStatus.OptionsBar.DrawDragBorder = false;
            this.barStatus.OptionsBar.UseWholeRow = true;
            this.barStatus.Text = "Status bar";
            // 
            // barItemStatus
            // 
            this.barItemStatus.Id = 7;
            this.barItemStatus.Name = "barItemStatus";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(880, 20);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 528);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(880, 22);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 20);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 508);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(880, 20);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 508);
            // 
            // panelSession
            // 
            this.panelSession.Controls.Add(this.dockPanel1_Container);
            this.panelSession.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
            this.panelSession.ID = new System.Guid("f086354d-9202-4dd1-beae-521e60004ff5");
            this.panelSession.Location = new System.Drawing.Point(0, 20);
            this.panelSession.Name = "panelSession";
            this.panelSession.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelSession.Size = new System.Drawing.Size(200, 508);
            this.panelSession.Text = "Sessions";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.treeListSession);
            this.dockPanel1_Container.Location = new System.Drawing.Point(3, 26);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(193, 479);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // treeListSession
            // 
            this.treeListSession.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colSSName});
            this.treeListSession.DataSource = this.bindingSourceSession;
            this.treeListSession.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListSession.Location = new System.Drawing.Point(0, 0);
            this.treeListSession.Name = "treeListSession";
            this.treeListSession.OptionsBehavior.Editable = false;
            this.treeListSession.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListSession.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
            this.treeListSession.OptionsView.ShowColumns = false;
            this.treeListSession.OptionsView.ShowHorzLines = false;
            this.treeListSession.OptionsView.ShowIndicator = false;
            this.treeListSession.OptionsView.ShowVertLines = false;
            this.treeListSession.Size = new System.Drawing.Size(193, 479);
            this.treeListSession.TabIndex = 1;
            this.treeListSession.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListSession_FocusedNodeChanged);
            this.treeListSession.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // colSSName
            // 
            this.colSSName.FieldName = "Name";
            this.colSSName.Name = "colSSName";
            this.colSSName.OptionsColumn.ReadOnly = true;
            this.colSSName.Visible = true;
            this.colSSName.VisibleIndex = 0;
            // 
            // bindingSourceSession
            // 
            this.bindingSourceSession.DataSource = typeof(DashboardDiagnosticTool.Data.SessionItem);
            // 
            // panelEvents
            // 
            this.panelEvents.Controls.Add(this.controlContainer1);
            this.panelEvents.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelEvents.ID = new System.Guid("d5d6f4ee-da0b-416b-81b7-8daf7baae03d");
            this.panelEvents.Location = new System.Drawing.Point(200, 378);
            this.panelEvents.Name = "panelEvents";
            this.panelEvents.OriginalSize = new System.Drawing.Size(200, 150);
            this.panelEvents.Size = new System.Drawing.Size(680, 150);
            this.panelEvents.Text = "Events";
            // 
            // controlContainer1
            // 
            this.controlContainer1.Controls.Add(this.gridEvent);
            this.controlContainer1.Location = new System.Drawing.Point(3, 27);
            this.controlContainer1.Name = "controlContainer1";
            this.controlContainer1.Size = new System.Drawing.Size(674, 120);
            this.controlContainer1.TabIndex = 0;
            // 
            // gridEvent
            // 
            this.gridEvent.DataSource = this.bindingSourceEvent;
            this.gridEvent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEvent.Location = new System.Drawing.Point(0, 0);
            this.gridEvent.MainView = this.gridView;
            this.gridEvent.Name = "gridEvent";
            this.gridEvent.Size = new System.Drawing.Size(674, 120);
            this.gridEvent.TabIndex = 0;
            this.gridEvent.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            this.gridEvent.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
            // 
            // bindingSourceEvent
            // 
            this.bindingSourceEvent.DataSource = typeof(DashboardDiagnosticTool.Data.TraceItem);
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colEventType,
            this.colData});
            this.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridView.GridControl = this.gridEvent;
            this.gridView.Name = "gridView";
            this.gridView.OptionsBehavior.Editable = false;
            this.gridView.OptionsCustomization.AllowSort = false;
            this.gridView.OptionsEditForm.PopupEditFormWidth = 686;
            this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView.OptionsView.ShowGroupPanel = false;
            this.gridView.OptionsView.ShowIndicator = false;
            // 
            // colEventType
            // 
            this.colEventType.FieldName = "EventType";
            this.colEventType.Name = "colEventType";
            this.colEventType.Visible = true;
            this.colEventType.VisibleIndex = 0;
            // 
            // colData
            // 
            this.colData.FieldName = "Data";
            this.colData.Name = "colData";
            this.colData.Visible = true;
            this.colData.VisibleIndex = 1;
            // 
            // panelResult
            // 
            this.panelResult.Controls.Add(this.dockPanel2_Container);
            this.panelResult.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.panelResult.ID = new System.Guid("5ba4a236-9f40-46cb-bd6e-cf89edce4c63");
            this.panelResult.Location = new System.Drawing.Point(200, 20);
            this.panelResult.Name = "panelResult";
            this.panelResult.OriginalSize = new System.Drawing.Size(680, 200);
            this.panelResult.Size = new System.Drawing.Size(680, 358);
            this.panelResult.Text = "Results";
            // 
            // dockPanel2_Container
            // 
            this.dockPanel2_Container.Controls.Add(this.treeListBenchmark);
            this.dockPanel2_Container.Location = new System.Drawing.Point(3, 26);
            this.dockPanel2_Container.Name = "dockPanel2_Container";
            this.dockPanel2_Container.Size = new System.Drawing.Size(674, 329);
            this.dockPanel2_Container.TabIndex = 0;
            // 
            // DiagnosticForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 550);
            this.Controls.Add(this.panelSession);
            this.Controls.Add(this.panelResult);
            this.Controls.Add(this.panelEvents);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("DiagnosticForm.IconOptions.SvgImage")));
            this.Name = "DiagnosticForm";
            this.Text = "Dashboard Diagnostic Tool";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBenchmark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListBenchmark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            this.panelSession.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListSession)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSession)).EndInit();
            this.panelEvents.ResumeLayout(false);
            this.controlContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEvent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceEvent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.panelResult.ResumeLayout(false);
            this.dockPanel2_Container.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource bindingSourceBenchmark;
        private DevExpress.XtraTreeList.TreeList treeListBenchmark;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBMName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBMCount;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colBMSecs;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar barMainMenu;
        private DevExpress.XtraBars.BarSubItem barItemFile;
        private DevExpress.XtraBars.BarButtonItem barItemOpen;
        private DevExpress.XtraBars.Bar barStatus;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem barItemSave;
        private DevExpress.XtraBars.BarButtonItem barItemSaveAs;
        private DevExpress.XtraBars.BarButtonItem barItemExit;
        private DevExpress.XtraBars.BarSubItem barItemDiagnostic;
        private DevExpress.XtraBars.BarButtonItem barItemStart;
        private DevExpress.XtraBars.BarStaticItem barItemStatus;
        private DevExpress.XtraBars.BarButtonItem barItemStop;
        private DevExpress.XtraBars.Docking.DockPanel panelResult;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraBars.Docking.DockPanel panelSession;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraTreeList.TreeList treeListSession;
        private DevExpress.XtraBars.Docking.DockPanel panelEvents;
        private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colSSName;
        private System.Windows.Forms.BindingSource bindingSourceSession;
        private DevExpress.XtraBars.BarButtonItem barItemDelete;
        private DevExpress.XtraGrid.GridControl gridEvent;
        private System.Windows.Forms.BindingSource bindingSourceEvent;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn colEventType;
        private DevExpress.XtraGrid.Columns.GridColumn colData;
        private DevExpress.XtraBars.BarSubItem barItemHelp;
        private DevExpress.XtraBars.BarButtonItem barItemViewHelp;
    }
}