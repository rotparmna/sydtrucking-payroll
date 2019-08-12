namespace sydtrucking_payroll_front.model
{
    public class Trash
    {
        public Trash()
        {
            
        }

        public Driver Driver { get; set; }
        public Employee Employee { get; set; }
        public OilCompany OilCompany { get; set; }
        public LeaseCompany LeaseCompany { get; set; }
        public Truck Truck { get; set; }
        public User User { get; set; }
        public Payroll Payroll { get; set; }
        public PayrollEmployee PayrollEmployee { get; set; }
        public PayrollLeaseCompany PayrollLeaseCompany { get; set; }
    }
}
