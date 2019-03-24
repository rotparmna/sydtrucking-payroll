namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using sydtrucking_payroll_front.enums;
    using System;

    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public Int64 SocialSecurity { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string State { get; set; }
        public DateTime Birthdate { get; set; }
        public string TruckNumber { get; set; }
        public PaymentType PaymentMethod { get; set; }
        public TaxForm TaxForm { get; set; }
        public double Rate { get; set; }
        public DriverLicense License { get; set; }
        public Contract Contract { get; set; }
    }

    public class DriverLicense
    {
        public string Number { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class Contract
    {
        public DateTime HireDate { get; set; }
        public DateTime TerminationDate { get; set; }
    }
}
