namespace sydtrucking_payroll_front.model
{
    public class PayrollDetail
    {
        public Ticket Ticket { get; set; }
        public Company Company { get; set; }
        public int Hours { get; set; }
    }
}