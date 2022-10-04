Imports System
Imports System.Windows.Forms
Imports DevExpress.XtraEditors

Namespace DashboardDiagnosticToolUI

    Friend Module Program

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread>
        Sub Main()
            Call Application.EnableVisualStyles()
            Call WindowsFormsSettings.SetDPIAware()
            Application.SetCompatibleTextRenderingDefault(False)
            Call Application.Run(New DiagnosticForm())
        End Sub
    End Module
End Namespace
