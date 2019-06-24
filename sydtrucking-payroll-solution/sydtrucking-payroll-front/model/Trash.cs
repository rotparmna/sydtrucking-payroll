namespace sydtrucking_payroll_front.model
{
    using System.Collections.Generic;

    public class Trash
    {
        public Trash()
        {
            
        }

        public Employee Employee { get; set; }
        public OilCompany OilCompany { get; set; }
        public LeaseCompany LeaseCompany { get; set; }
        public Truck Truck { get; set; }
        public User User { get; set; }
    }
}
