namespace sydtrucking_payroll_front.model
{
    using System;

    public class PayrollDetailView
    {
        public PayrollDetailView()
        {

        }

        public DateTime TicketDate { get; set; }
        public OilCompany OilCompany { get; set; }
        public int TicketNumber { get; set; }
        public int Hours { get; set; }
    }
}
