namespace sydtrucking_payroll_front.model
{
    using System;

    public class PayrollDetailView
    {
        public PayrollDetailView()
        {
            TicketDate = DateTime.Now;
        }

        public DateTime TicketDate { get; set; }
        public string Company { get; set; }
        public int TicketNumber { get; set; }
        public int Hours { get; set; }
    }
}
