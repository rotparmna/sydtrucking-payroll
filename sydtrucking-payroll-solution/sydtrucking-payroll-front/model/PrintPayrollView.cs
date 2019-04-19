namespace sydtrucking_payroll_front.model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PrintPayrollView
    {
        public string Driver { get; set; }
        public string Rate { get; set; }
        public string PaymentWeek { get; set; }
        public string TotalHours { get; set; }
        public string TotalPayment { get; set; }
    }
}
