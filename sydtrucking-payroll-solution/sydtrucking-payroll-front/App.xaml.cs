namespace sydtrucking_payroll_front
{
    using System;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static log4net.ILog Log => log;

        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            CreateDirectoriesToSaveReports();
        }

        private static void CreateDirectoriesToSaveReports()
        {
            CreateDirectory(business.Constant.PathReportPayroll);
            CreateDirectory(business.Constant.PathReportPayrollEmployee);
            CreateDirectory(business.Constant.PathReportPayrollLeaseCompany);
        }

        private static void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            Log.Error("Fatal error!. Datetime: " + DateTime.Now + ". Exception: " + exception);
        }
    }
}
