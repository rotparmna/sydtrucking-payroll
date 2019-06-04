namespace sydtrucking_payroll_front.util
{
    using System;
    using System.Globalization;

    public static class CustomDateTime
    {
        public static DateTime NextFriday(this DateTime date)
        {
            DateTime today = date;
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysUntilFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilFriday);
        }
    }
}
