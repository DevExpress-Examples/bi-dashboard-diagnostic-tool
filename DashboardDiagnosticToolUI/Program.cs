using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DashboardDiagnosticToolUI {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            WindowsFormsSettings.SetDPIAware();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DiagnosticForm());
        }
    }
}
