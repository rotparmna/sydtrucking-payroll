﻿namespace sydtrucking_payroll_front.model
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
    }
}