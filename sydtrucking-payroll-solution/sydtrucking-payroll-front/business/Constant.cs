namespace sydtrucking_payroll_front.business
{
    public static class Constant
    {
        private static readonly int regularHour = Properties.Settings.Default.RegularHour;
        private static readonly double factorRateOvertimeHour = Properties.Settings.Default.FactorRateOvertimeHour;

        public static int RegularHour => regularHour;
        public static double FactorRateOvertimeHour => factorRateOvertimeHour;
    }
}
