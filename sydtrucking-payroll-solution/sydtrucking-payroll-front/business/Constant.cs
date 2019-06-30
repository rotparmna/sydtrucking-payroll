namespace sydtrucking_payroll_front.business
{
    using sydtrucking_payroll_front.model;
    using System.Linq;

    public static class Constant
    {
        private static readonly string pathReportPayroll = Properties.Settings.Default.PathReportPayroll;
        private static readonly string pathReportPayrollLeaseCompany = Properties.Settings.Default.PathReportPayrollLeaseCompany;
        private static readonly Smtp smtp = new Configuration().GetAll().Select(x => x.Smtp).FirstOrDefault();
        private static readonly PayrollConfiguration payroll = new Configuration().GetAll().Select(x => x.Payroll).FirstOrDefault();
        private static readonly PayrollLeaseCompanyConfiguration payrollLeaseCompany = new Configuration().GetAll().Select(x => x.PayrollLeaseCompany).FirstOrDefault();
        private static readonly string pathReportPayrollEmployee = Properties.Settings.Default.PathReportPayrollEmployee;

        public static string PathReportPayroll => pathReportPayroll;
        public static string PathReportPayrollLeaseCompany => pathReportPayrollLeaseCompany;
        public static Smtp Smtp => smtp;
        public static PayrollConfiguration Payroll => payroll;
        public static PayrollLeaseCompanyConfiguration PayrollLeaseCompany => payrollLeaseCompany;
        public static string PathReportPayrollEmployee = pathReportPayrollEmployee;
    }
}
