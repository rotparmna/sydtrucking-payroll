namespace sydtrucking_payroll_front.business
{
    public static class Constant
    {
        private static readonly int regularHour = Properties.Settings.Default.RegularHour;
        private static readonly double factorRateOvertimeHour = Properties.Settings.Default.FactorRateOvertimeHour;
        private static readonly int daysWeek = Properties.Settings.Default.DaysWeek;
        private static readonly int daysWeekPayment = Properties.Settings.Default.DaysWeekPayment;
        private static readonly string pathReportPayroll = Properties.Settings.Default.PathReportPayroll;

        public static int RegularHour => regularHour;
        public static double FactorRateOvertimeHour => factorRateOvertimeHour;
        public static int DaysWeek => daysWeek;
        public static int DaysWeekPayment => daysWeekPayment;
        public static string PathReportPayroll => pathReportPayroll;
    }
}
