﻿namespace sydtrucking_payroll_front.model
{
    public class PrintPayrollView
    {
        public string Id { get; set; }
        public string Driver { get; set; }
        public string Rate { get; set; }
        public string PaymentWeek { get; set; }
        public string TotalHours { get; set; }
        public string TotalPayment { get; set; }
    }
}
