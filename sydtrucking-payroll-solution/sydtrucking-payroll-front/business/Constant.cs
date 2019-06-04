namespace sydtrucking_payroll_front.business
{
    using sydtrucking_payroll_front.model;
    using System.Linq;

    public static class Constant
    {
        private static readonly int regularHour = Properties.Settings.Default.RegularHour;
        private static readonly double factorRateOvertimeHour = Properties.Settings.Default.FactorRateOvertimeHour;
        private static readonly double percentLeaseFeeValue = Properties.Settings.Default.PercentLeaseFeeValue;
        private static readonly double percentWorkerCompValue = Properties.Settings.Default.PercentWorkerCompValue;
        private static readonly int daysWeek = Properties.Settings.Default.DaysWeek;
        private static readonly int daysWeekPayment = Properties.Settings.Default.DaysWeekPayment;
        private static readonly string pathReportPayroll = Properties.Settings.Default.PathReportPayroll;
        private static readonly Smtp smtp = new Configuration().GetAll().Select(x => x.Smtp).FirstOrDefault();
        private static readonly int lastThreeFridayPayrollLeaseCompany = Properties.Settings.Default.LastThreeFridayPayrollLeaseCompany;

        public static int RegularHour => regularHour;
        public static double FactorRateOvertimeHour => factorRateOvertimeHour;
        public static double PercentLeaseFeeValue => percentLeaseFeeValue;
        public static double PercentWorkerCompValue => percentWorkerCompValue;
        public static int DaysWeek => daysWeek;
        public static int DaysWeekPayment => daysWeekPayment;
        public static string PathReportPayroll => pathReportPayroll;
        public static Smtp Smtp => smtp;
        public static int LastThreeFridayPayrollLeaseCompany => lastThreeFridayPayrollLeaseCompany;
    }
}
