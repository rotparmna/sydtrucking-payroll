namespace sydtrucking_payroll_front.model
{
    public class PrintPayrollEmployeeView
    {
        public string Id { get; set; }
        public string Employee { get; set; }
        public string Rate { get; set; }
        public string PaymentWeek { get; set; }
        public string TotalHours { get; set; }
        public string TotalPayment { get; set; }
    }
}
