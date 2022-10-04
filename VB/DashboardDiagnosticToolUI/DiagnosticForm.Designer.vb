Namespace DashboardDiagnosticToolUI

    Public Partial Class DiagnosticForm

        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DashboardDiagnosticToolUI.DiagnosticForm))
            Me.bindingSourceBenchmark = New System.Windows.Forms.BindingSource(Me.components)
            Me.treeListBenchmark = New DevExpress.XtraTreeList.TreeList()
            Me.colBMName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
            Me.colBMCount = New DevExpress.XtraTreeList.Columns.TreeListColumn()
            Me.colBMSecs = New DevExpress.XtraTreeList.Columns.TreeListColumn()
            Me.dockManager = New DevExpress.XtraBars.Docking.DockManager(Me.components)
            Me.barManager = New DevExpress.XtraBars.BarManager(Me.components)
            Me.barMainMenu = New DevExpress.XtraBars.Bar()
            Me.barItemFile = New DevExpress.XtraBars.BarSubItem()
            Me.barItemOpen = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemSave = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemSaveAs = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemExit = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemDiagnostic = New DevExpress.XtraBars.BarSubItem()
            Me.barItemStart = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemStop = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemDelete = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemHelp = New DevExpress.XtraBars.BarSubItem()
            Me.barItemViewHelp = New DevExpress.XtraBars.BarButtonItem()
            Me.barItemAbout = New DevExpress.XtraBars.BarButtonItem()
            Me.barStatus = New DevExpress.XtraBars.Bar()
            Me.barItemStatus = New DevExpress.XtraBars.BarStaticItem()
            Me.barDockControlTop = New DevExpress.XtraBars.BarDockControl()
            Me.barDockControlBottom = New DevExpress.XtraBars.BarDockControl()
            Me.barDockControlLeft = New DevExpress.XtraBars.BarDockControl()
            Me.barDockControlRight = New DevExpress.XtraBars.BarDockControl()
            Me.panelSession = New DevExpress.XtraBars.Docking.DockPanel()
            Me.dockPanel1_Container = New DevExpress.XtraBars.Docking.ControlContainer()
            Me.treeListSession = New DevExpress.XtraTreeList.TreeList()
            Me.colSSName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
            Me.bindingSourceSession = New System.Windows.Forms.BindingSource(Me.components)
            Me.panelEvents = New DevExpress.XtraBars.Docking.DockPanel()
            Me.controlContainer1 = New DevExpress.XtraBars.Docking.ControlContainer()
            Me.gridEvent = New DevExpress.XtraGrid.GridControl()
            Me.bindingSourceEvent = New System.Windows.Forms.BindingSource(Me.components)
            Me.gridView = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.colEventType = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colData = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.panelResult = New DevExpress.XtraBars.Docking.DockPanel()
            Me.dockPanel2_Container = New DevExpress.XtraBars.Docking.ControlContainer()
            CType((Me.bindingSourceBenchmark), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.treeListBenchmark), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.dockManager), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.barManager), System.ComponentModel.ISupportInitialize).BeginInit()
            Me.panelSession.SuspendLayout()
            Me.dockPanel1_Container.SuspendLayout()
            CType((Me.treeListSession), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.bindingSourceSession), System.ComponentModel.ISupportInitialize).BeginInit()
            Me.panelEvents.SuspendLayout()
            Me.controlContainer1.SuspendLayout()
            CType((Me.gridEvent), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.bindingSourceEvent), System.ComponentModel.ISupportInitialize).BeginInit()
            CType((Me.gridView), System.ComponentModel.ISupportInitialize).BeginInit()
            Me.panelResult.SuspendLayout()
            Me.dockPanel2_Container.SuspendLayout()
            Me.SuspendLayout()
            ' 
            ' bindingSourceBenchmark
            ' 
            Me.bindingSourceBenchmark.DataSource = GetType(DashboardDiagnosticTool.Data.BenchmarkItem)
            ' 
            ' treeListBenchmark
            ' 
            Me.treeListBenchmark.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colBMName, Me.colBMCount, Me.colBMSecs})
            Me.treeListBenchmark.DataSource = Me.bindingSourceBenchmark
            Me.treeListBenchmark.ChildListFieldName = "Children"
            Me.treeListBenchmark.Dock = System.Windows.Forms.DockStyle.Fill
            Me.treeListBenchmark.Location = New System.Drawing.Point(0, 0)
            Me.treeListBenchmark.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.treeListBenchmark.MinWidth = 23
            Me.treeListBenchmark.Name = "treeListBenchmark"
            Me.treeListBenchmark.OptionsBehavior.Editable = False
            Me.treeListBenchmark.OptionsSelection.EnableAppearanceFocusedCell = False
            Me.treeListBenchmark.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None
            Me.treeListBenchmark.OptionsView.ShowIndicator = False
            Me.treeListBenchmark.OptionsView.ShowSummaryFooter = True
            Me.treeListBenchmark.Size = New System.Drawing.Size(819, 437)
            Me.treeListBenchmark.TabIndex = 0
            Me.treeListBenchmark.TreeLevelWidth = 21
            AddHandler Me.treeListBenchmark.FocusedNodeChanged, New DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(AddressOf Me.treeListBenchmark_FocusedNodeChanged)
            AddHandler Me.treeListBenchmark.KeyDown, New System.Windows.Forms.KeyEventHandler(AddressOf Me.OnKeyDown)
            ' 
            ' colBMName
            ' 
            Me.colBMName.FieldName = "Name"
            Me.colBMName.MinWidth = 23
            Me.colBMName.Name = "colBMName"
            Me.colBMName.Visible = True
            Me.colBMName.VisibleIndex = 0
            Me.colBMName.Width = 87
            ' 
            ' colBMCount
            ' 
            Me.colBMCount.FieldName = "Count"
            Me.colBMCount.MinWidth = 23
            Me.colBMCount.Name = "colBMCount"
            Me.colBMCount.Visible = True
            Me.colBMCount.VisibleIndex = 2
            Me.colBMCount.Width = 87
            ' 
            ' colBMSecs
            ' 
            Me.colBMSecs.FieldName = "MSecs"
            Me.colBMSecs.Format.FormatString = "{0:0.0}"
            Me.colBMSecs.Format.FormatType = DevExpress.Utils.FormatType.Custom
            Me.colBMSecs.MinWidth = 23
            Me.colBMSecs.Name = "colBMSecs"
            Me.colBMSecs.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum
            Me.colBMSecs.SummaryFooterStrFormat = "Total: {0:0,0.0}"
            Me.colBMSecs.Visible = True
            Me.colBMSecs.VisibleIndex = 3
            Me.colBMSecs.Width = 87
            ' 
            ' dockManager
            ' 
            Me.dockManager.Form = Me
            Me.dockManager.MenuManager = Me.barManager
            Me.dockManager.RootPanels.AddRange(New DevExpress.XtraBars.Docking.DockPanel() {Me.panelSession, Me.panelEvents, Me.panelResult})
            Me.dockManager.TopZIndexControls.AddRange(New String() {"DevExpress.XtraBars.BarDockControl", "DevExpress.XtraBars.StandaloneBarDockControl", "System.Windows.Forms.MenuStrip", "System.Windows.Forms.StatusStrip", "System.Windows.Forms.StatusBar", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl", "DevExpress.XtraBars.Navigation.OfficeNavigationBar", "DevExpress.XtraBars.Navigation.TileNavPane", "DevExpress.XtraBars.TabFormControl", "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl", "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"})
            ' 
            ' barManager
            ' 
            Me.barManager.Bars.AddRange(New DevExpress.XtraBars.Bar() {Me.barMainMenu, Me.barStatus})
            Me.barManager.DockControls.Add(Me.barDockControlTop)
            Me.barManager.DockControls.Add(Me.barDockControlBottom)
            Me.barManager.DockControls.Add(Me.barDockControlLeft)
            Me.barManager.DockControls.Add(Me.barDockControlRight)
            Me.barManager.Form = Me
            Me.barManager.Items.AddRange(New DevExpress.XtraBars.BarItem() {Me.barItemFile, Me.barItemOpen, Me.barItemSave, Me.barItemSaveAs, Me.barItemExit, Me.barItemDiagnostic, Me.barItemStart, Me.barItemStatus, Me.barItemStop, Me.barItemDelete, Me.barItemHelp, Me.barItemViewHelp, Me.barItemAbout})
            Me.barManager.MainMenu = Me.barMainMenu
            Me.barManager.MaxItemId = 15
            Me.barManager.StatusBar = Me.barStatus
            AddHandler Me.barManager.ItemClick, New DevExpress.XtraBars.ItemClickEventHandler(AddressOf Me.barManager_ItemClick)
            ' 
            ' barMainMenu
            ' 
            Me.barMainMenu.BarName = "Main menu"
            Me.barMainMenu.DockCol = 0
            Me.barMainMenu.DockRow = 0
            Me.barMainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top
            Me.barMainMenu.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.barItemFile), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemDiagnostic), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemHelp)})
            Me.barMainMenu.OptionsBar.MultiLine = True
            Me.barMainMenu.OptionsBar.UseWholeRow = True
            Me.barMainMenu.Text = "Main menu"
            ' 
            ' barItemFile
            ' 
            Me.barItemFile.Caption = "File"
            Me.barItemFile.Id = 0
            Me.barItemFile.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.barItemOpen), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemSave, True), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemSaveAs), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemExit, True)})
            Me.barItemFile.Name = "barItemFile"
            ' 
            ' barItemOpen
            ' 
            Me.barItemOpen.Caption = "Open"
            Me.barItemOpen.ContentHorizontalAlignment = DevExpress.XtraBars.BarItemContentAlignment.Far
            Me.barItemOpen.Id = 1
            Me.barItemOpen.ImageOptions.SvgImage = CType((resources.GetObject("barItemOpen.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemOpen.Name = "barItemOpen"
            Me.barItemOpen.ShortcutKeyDisplayString = "Ctrl+O"
            Me.barItemOpen.Tag = "Open"
            ' 
            ' barItemSave
            ' 
            Me.barItemSave.Caption = "Save"
            Me.barItemSave.Id = 2
            Me.barItemSave.ImageOptions.SvgImage = CType((resources.GetObject("barItemSave.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemSave.Name = "barItemSave"
            Me.barItemSave.ShortcutKeyDisplayString = "Ctrl+S"
            Me.barItemSave.Tag = "Save"
            ' 
            ' barItemSaveAs
            ' 
            Me.barItemSaveAs.Caption = "Save As..."
            Me.barItemSaveAs.Id = 3
            Me.barItemSaveAs.ImageOptions.SvgImage = CType((resources.GetObject("barItemSaveAs.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemSaveAs.Name = "barItemSaveAs"
            Me.barItemSaveAs.ShortcutKeyDisplayString = "Ctrl+Shift+S"
            Me.barItemSaveAs.Tag = "SaveAs"
            ' 
            ' barItemExit
            ' 
            Me.barItemExit.Caption = "Exit"
            Me.barItemExit.Id = 4
            Me.barItemExit.Name = "barItemExit"
            Me.barItemExit.ShortcutKeyDisplayString = "Alt+F4"
            Me.barItemExit.Tag = "Exit"
            ' 
            ' barItemDiagnostic
            ' 
            Me.barItemDiagnostic.Caption = "Diagnostic"
            Me.barItemDiagnostic.Id = 5
            Me.barItemDiagnostic.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.barItemStart), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemStop), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemDelete)})
            Me.barItemDiagnostic.Name = "barItemDiagnostic"
            ' 
            ' barItemStart
            ' 
            Me.barItemStart.Caption = "Start Session"
            Me.barItemStart.Id = 6
            Me.barItemStart.ImageOptions.SvgImage = CType((resources.GetObject("barItemStart.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemStart.Name = "barItemStart"
            Me.barItemStart.ShortcutKeyDisplayString = "F5"
            Me.barItemStart.Tag = "Start"
            ' 
            ' barItemStop
            ' 
            Me.barItemStop.Caption = "Stop Session"
            Me.barItemStop.Id = 8
            Me.barItemStop.ImageOptions.SvgImage = CType((resources.GetObject("barItemStop.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemStop.Name = "barItemStop"
            Me.barItemStop.ShortcutKeyDisplayString = "Shift+F5"
            Me.barItemStop.Tag = "Stop"
            ' 
            ' barItemDelete
            ' 
            Me.barItemDelete.Caption = "Delete"
            Me.barItemDelete.Id = 9
            Me.barItemDelete.ImageOptions.SvgImage = CType((resources.GetObject("barItemDelete.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemDelete.Name = "barItemDelete"
            Me.barItemDelete.ShortcutKeyDisplayString = "Delete"
            Me.barItemDelete.Tag = "Delete"
            ' 
            ' barItemHelp
            ' 
            Me.barItemHelp.Caption = "Help"
            Me.barItemHelp.Id = 12
            Me.barItemHelp.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.barItemViewHelp), New DevExpress.XtraBars.LinkPersistInfo(Me.barItemAbout)})
            Me.barItemHelp.Name = "barItemHelp"
            ' 
            ' barItemViewHelp
            ' 
            Me.barItemViewHelp.Caption = "View Help"
            Me.barItemViewHelp.Id = 13
            Me.barItemViewHelp.ImageOptions.SvgImage = CType((resources.GetObject("barItemViewHelp.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemViewHelp.Name = "barItemViewHelp"
            Me.barItemViewHelp.Tag = "Help"
            ' 
            ' barItemAbout
            ' 
            Me.barItemAbout.Caption = "About"
            Me.barItemAbout.Id = 14
            Me.barItemAbout.ImageOptions.SvgImage = CType((resources.GetObject("barItemAbout.ImageOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.barItemAbout.Name = "barItemAbout"
            Me.barItemAbout.Tag = "About"
            ' 
            ' barStatus
            ' 
            Me.barStatus.BarName = "Status bar"
            Me.barStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom
            Me.barStatus.DockCol = 0
            Me.barStatus.DockRow = 0
            Me.barStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom
            Me.barStatus.LinksPersistInfo.AddRange(New DevExpress.XtraBars.LinkPersistInfo() {New DevExpress.XtraBars.LinkPersistInfo(Me.barItemStatus)})
            Me.barStatus.OptionsBar.AllowQuickCustomization = False
            Me.barStatus.OptionsBar.DrawDragBorder = False
            Me.barStatus.OptionsBar.UseWholeRow = True
            Me.barStatus.Text = "Status bar"
            ' 
            ' barItemStatus
            ' 
            Me.barItemStatus.Id = 7
            Me.barItemStatus.Name = "barItemStatus"
            ' 
            ' barDockControlTop
            ' 
            Me.barDockControlTop.CausesValidation = False
            Me.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top
            Me.barDockControlTop.Location = New System.Drawing.Point(0, 0)
            Me.barDockControlTop.Manager = Me.barManager
            Me.barDockControlTop.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.barDockControlTop.Size = New System.Drawing.Size(1027, 25)
            ' 
            ' barDockControlBottom
            ' 
            Me.barDockControlBottom.CausesValidation = False
            Me.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.barDockControlBottom.Location = New System.Drawing.Point(0, 650)
            Me.barDockControlBottom.Manager = Me.barManager
            Me.barDockControlBottom.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.barDockControlBottom.Size = New System.Drawing.Size(1027, 27)
            ' 
            ' barDockControlLeft
            ' 
            Me.barDockControlLeft.CausesValidation = False
            Me.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left
            Me.barDockControlLeft.Location = New System.Drawing.Point(0, 25)
            Me.barDockControlLeft.Manager = Me.barManager
            Me.barDockControlLeft.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.barDockControlLeft.Size = New System.Drawing.Size(0, 625)
            ' 
            ' barDockControlRight
            ' 
            Me.barDockControlRight.CausesValidation = False
            Me.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right
            Me.barDockControlRight.Location = New System.Drawing.Point(1027, 25)
            Me.barDockControlRight.Manager = Me.barManager
            Me.barDockControlRight.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.barDockControlRight.Size = New System.Drawing.Size(0, 625)
            ' 
            ' panelSession
            ' 
            Me.panelSession.Controls.Add(Me.dockPanel1_Container)
            Me.panelSession.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left
            Me.panelSession.ID = New System.Guid("f086354d-9202-4dd1-beae-521e60004ff5")
            Me.panelSession.Location = New System.Drawing.Point(0, 25)
            Me.panelSession.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.panelSession.Name = "panelSession"
            Me.panelSession.OriginalSize = New System.Drawing.Size(200, 200)
            Me.panelSession.Size = New System.Drawing.Size(200, 625)
            Me.panelSession.Text = "Sessions"
            ' 
            ' dockPanel1_Container
            ' 
            Me.dockPanel1_Container.Controls.Add(Me.treeListSession)
            Me.dockPanel1_Container.Location = New System.Drawing.Point(4, 32)
            Me.dockPanel1_Container.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.dockPanel1_Container.Name = "dockPanel1_Container"
            Me.dockPanel1_Container.Size = New System.Drawing.Size(190, 589)
            Me.dockPanel1_Container.TabIndex = 0
            ' 
            ' treeListSession
            ' 
            Me.treeListSession.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colSSName})
            Me.treeListSession.DataSource = Me.bindingSourceSession
            Me.treeListSession.Dock = System.Windows.Forms.DockStyle.Fill
            Me.treeListSession.Location = New System.Drawing.Point(0, 0)
            Me.treeListSession.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.treeListSession.MinWidth = 23
            Me.treeListSession.Name = "treeListSession"
            Me.treeListSession.OptionsBehavior.Editable = False
            Me.treeListSession.OptionsSelection.EnableAppearanceFocusedCell = False
            Me.treeListSession.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None
            Me.treeListSession.OptionsView.ShowColumns = False
            Me.treeListSession.OptionsView.ShowHorzLines = False
            Me.treeListSession.OptionsView.ShowIndicator = False
            Me.treeListSession.OptionsView.ShowVertLines = False
            Me.treeListSession.Size = New System.Drawing.Size(190, 589)
            Me.treeListSession.TabIndex = 1
            Me.treeListSession.TreeLevelWidth = 21
            AddHandler Me.treeListSession.FocusedNodeChanged, New DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(AddressOf Me.treeListSession_FocusedNodeChanged)
            AddHandler Me.treeListSession.KeyDown, New System.Windows.Forms.KeyEventHandler(AddressOf Me.OnKeyDown)
            ' 
            ' colSSName
            ' 
            Me.colSSName.FieldName = "Name"
            Me.colSSName.MinWidth = 23
            Me.colSSName.Name = "colSSName"
            Me.colSSName.OptionsColumn.[ReadOnly] = True
            Me.colSSName.Visible = True
            Me.colSSName.VisibleIndex = 0
            Me.colSSName.Width = 87
            ' 
            ' bindingSourceSession
            ' 
            Me.bindingSourceSession.DataSource = GetType(DashboardDiagnosticTool.Data.SessionItem)
            ' 
            ' panelEvents
            ' 
            Me.panelEvents.Controls.Add(Me.controlContainer1)
            Me.panelEvents.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom
            Me.panelEvents.ID = New System.Guid("d5d6f4ee-da0b-416b-81b7-8daf7baae03d")
            Me.panelEvents.Location = New System.Drawing.Point(200, 500)
            Me.panelEvents.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.panelEvents.Name = "panelEvents"
            Me.panelEvents.OriginalSize = New System.Drawing.Size(200, 150)
            Me.panelEvents.Size = New System.Drawing.Size(827, 150)
            Me.panelEvents.Text = "Events"
            ' 
            ' controlContainer1
            ' 
            Me.controlContainer1.Controls.Add(Me.gridEvent)
            Me.controlContainer1.Location = New System.Drawing.Point(4, 34)
            Me.controlContainer1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.controlContainer1.Name = "controlContainer1"
            Me.controlContainer1.Size = New System.Drawing.Size(819, 112)
            Me.controlContainer1.TabIndex = 0
            ' 
            ' gridEvent
            ' 
            Me.gridEvent.DataSource = Me.bindingSourceEvent
            Me.gridEvent.Dock = System.Windows.Forms.DockStyle.Fill
            Me.gridEvent.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.gridEvent.Location = New System.Drawing.Point(0, 0)
            Me.gridEvent.MainView = Me.gridView
            Me.gridEvent.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.gridEvent.Name = "gridEvent"
            Me.gridEvent.Size = New System.Drawing.Size(819, 112)
            Me.gridEvent.TabIndex = 0
            Me.gridEvent.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gridView})
            AddHandler Me.gridEvent.KeyDown, New System.Windows.Forms.KeyEventHandler(AddressOf Me.OnKeyDown)
            ' 
            ' bindingSourceEvent
            ' 
            Me.bindingSourceEvent.DataSource = GetType(DashboardDiagnosticTool.Data.TraceItem)
            ' 
            ' gridView
            ' 
            Me.gridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colEventType, Me.colData})
            Me.gridView.DetailHeight = 431
            Me.gridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None
            Me.gridView.GridControl = Me.gridEvent
            Me.gridView.Name = "gridView"
            Me.gridView.OptionsBehavior.Editable = False
            Me.gridView.OptionsCustomization.AllowSort = False
            Me.gridView.OptionsSelection.EnableAppearanceFocusedCell = False
            Me.gridView.OptionsView.ShowGroupPanel = False
            Me.gridView.OptionsView.ShowIndicator = False
            ' 
            ' colEventType
            ' 
            Me.colEventType.FieldName = "EventType"
            Me.colEventType.MinWidth = 23
            Me.colEventType.Name = "colEventType"
            Me.colEventType.Visible = True
            Me.colEventType.VisibleIndex = 0
            Me.colEventType.Width = 87
            ' 
            ' colData
            ' 
            Me.colData.FieldName = "Data"
            Me.colData.MinWidth = 23
            Me.colData.Name = "colData"
            Me.colData.Visible = True
            Me.colData.VisibleIndex = 1
            Me.colData.Width = 87
            ' 
            ' panelResult
            ' 
            Me.panelResult.Controls.Add(Me.dockPanel2_Container)
            Me.panelResult.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill
            Me.panelResult.ID = New System.Guid("5ba4a236-9f40-46cb-bd6e-cf89edce4c63")
            Me.panelResult.Location = New System.Drawing.Point(200, 25)
            Me.panelResult.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.panelResult.Name = "panelResult"
            Me.panelResult.OriginalSize = New System.Drawing.Size(680, 200)
            Me.panelResult.Size = New System.Drawing.Size(827, 475)
            Me.panelResult.Text = "Results"
            ' 
            ' dockPanel2_Container
            ' 
            Me.dockPanel2_Container.Controls.Add(Me.treeListBenchmark)
            Me.dockPanel2_Container.Location = New System.Drawing.Point(4, 32)
            Me.dockPanel2_Container.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.dockPanel2_Container.Name = "dockPanel2_Container"
            Me.dockPanel2_Container.Size = New System.Drawing.Size(819, 437)
            Me.dockPanel2_Container.TabIndex = 0
            ' 
            ' DiagnosticForm
            ' 
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7F, 16F)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1027, 677)
            Me.Controls.Add(Me.panelSession)
            Me.Controls.Add(Me.panelResult)
            Me.Controls.Add(Me.panelEvents)
            Me.Controls.Add(Me.barDockControlLeft)
            Me.Controls.Add(Me.barDockControlRight)
            Me.Controls.Add(Me.barDockControlBottom)
            Me.Controls.Add(Me.barDockControlTop)
            Me.IconOptions.SvgImage = CType((resources.GetObject("DiagnosticForm.IconOptions.SvgImage")), DevExpress.Utils.Svg.SvgImage)
            Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.Name = "DiagnosticForm"
            Me.Text = "Dashboard Diagnostic Tool"
            CType((Me.bindingSourceBenchmark), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.treeListBenchmark), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.dockManager), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.barManager), System.ComponentModel.ISupportInitialize).EndInit()
            Me.panelSession.ResumeLayout(False)
            Me.dockPanel1_Container.ResumeLayout(False)
            CType((Me.treeListSession), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.bindingSourceSession), System.ComponentModel.ISupportInitialize).EndInit()
            Me.panelEvents.ResumeLayout(False)
            Me.controlContainer1.ResumeLayout(False)
            CType((Me.gridEvent), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.bindingSourceEvent), System.ComponentModel.ISupportInitialize).EndInit()
            CType((Me.gridView), System.ComponentModel.ISupportInitialize).EndInit()
            Me.panelResult.ResumeLayout(False)
            Me.dockPanel2_Container.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

#End Region
        Private bindingSourceBenchmark As System.Windows.Forms.BindingSource

        Private treeListBenchmark As DevExpress.XtraTreeList.TreeList

        Private colBMName As DevExpress.XtraTreeList.Columns.TreeListColumn

        Private colBMCount As DevExpress.XtraTreeList.Columns.TreeListColumn

        Private colBMSecs As DevExpress.XtraTreeList.Columns.TreeListColumn

        Private dockManager As DevExpress.XtraBars.Docking.DockManager

        Private barDockControlLeft As DevExpress.XtraBars.BarDockControl

        Private barManager As DevExpress.XtraBars.BarManager

        Private barMainMenu As DevExpress.XtraBars.Bar

        Private barItemFile As DevExpress.XtraBars.BarSubItem

        Private barItemOpen As DevExpress.XtraBars.BarButtonItem

        Private barStatus As DevExpress.XtraBars.Bar

        Private barDockControlTop As DevExpress.XtraBars.BarDockControl

        Private barDockControlBottom As DevExpress.XtraBars.BarDockControl

        Private barDockControlRight As DevExpress.XtraBars.BarDockControl

        Private barItemSave As DevExpress.XtraBars.BarButtonItem

        Private barItemSaveAs As DevExpress.XtraBars.BarButtonItem

        Private barItemExit As DevExpress.XtraBars.BarButtonItem

        Private barItemDiagnostic As DevExpress.XtraBars.BarSubItem

        Private barItemStart As DevExpress.XtraBars.BarButtonItem

        Private barItemStatus As DevExpress.XtraBars.BarStaticItem

        Private barItemStop As DevExpress.XtraBars.BarButtonItem

        Private panelResult As DevExpress.XtraBars.Docking.DockPanel

        Private dockPanel2_Container As DevExpress.XtraBars.Docking.ControlContainer

        Private panelSession As DevExpress.XtraBars.Docking.DockPanel

        Private dockPanel1_Container As DevExpress.XtraBars.Docking.ControlContainer

        Private treeListSession As DevExpress.XtraTreeList.TreeList

        Private panelEvents As DevExpress.XtraBars.Docking.DockPanel

        Private controlContainer1 As DevExpress.XtraBars.Docking.ControlContainer

        Private colSSName As DevExpress.XtraTreeList.Columns.TreeListColumn

        Private bindingSourceSession As System.Windows.Forms.BindingSource

        Private barItemDelete As DevExpress.XtraBars.BarButtonItem

        Private gridEvent As DevExpress.XtraGrid.GridControl

        Private bindingSourceEvent As System.Windows.Forms.BindingSource

        Private gridView As DevExpress.XtraGrid.Views.Grid.GridView

        Private colEventType As DevExpress.XtraGrid.Columns.GridColumn

        Private colData As DevExpress.XtraGrid.Columns.GridColumn

        Private barItemHelp As DevExpress.XtraBars.BarSubItem

        Private barItemViewHelp As DevExpress.XtraBars.BarButtonItem

        Private barItemAbout As DevExpress.XtraBars.BarButtonItem
    End Class
End Namespace
