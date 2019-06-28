namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class Driver : Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Truck Truck { get; set; }
        public Contract Contract { get; set; }
    }

    public class DriverLicense
    {
        public string Number { get; set; }
        public string State { get; set; }
        public DateTime Expiration { get; set; }
    }

    public class Contract
    {
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public bool Actually
        {
            get
            {
                return !TerminationDate.HasValue;
            }
        }
    }
}
