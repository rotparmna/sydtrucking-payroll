namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class Employees
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
        public string PaymentMethod { get; set; }
        public string TaxForm { get; set; }
        public double Rate { get; set; }
        public DriverLicense License { get; set; }
        public Contract Contract { get; set; }
    }

    public class DriverLicense
    {
        public string Number { get; set; }
        public DateTime Expedition { get; set; }
    }

    public class Contract
    {
        public DateTime HireDate { get; set; }
        public DateTime TerminationDate { get; set; }
    }
}
