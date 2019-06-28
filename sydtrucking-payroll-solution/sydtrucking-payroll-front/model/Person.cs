namespace sydtrucking_payroll_front.model
{
    using sydtrucking_payroll_front.enums;
    using System;

    public class Person : ModelBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public Int64 SocialSecurity { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DriverLicense License { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime Birthdate { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public TaxType TaxForm { get; set; }
        public double Rate { get; set; }
        public string Fullname
        {
            get { return Name + " " + LastName; }
        }
    }
}
