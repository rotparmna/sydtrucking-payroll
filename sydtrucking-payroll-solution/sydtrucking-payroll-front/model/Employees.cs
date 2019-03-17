namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Employees
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string SocialSecurity { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string DriverLicense { get; set; }
        public string State { get; set; }
        public string Exp { get; set; }
        public string DOB { get; set; }
        public string TruckNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime TerminationDate { get; set; }
        public string PaymentMethod { get; set; }
        public string TaxForm { get; set; }
        public double Rate { get; set; }
    }
}
