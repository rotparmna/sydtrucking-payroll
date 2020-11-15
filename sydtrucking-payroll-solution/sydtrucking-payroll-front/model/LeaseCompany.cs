namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System.Collections.Generic;
    using System;
    using System.Linq;

    public class LeaseCompany : ModelBase
    {
        public LeaseCompany()
        {
            Trucks = new List<Truck>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Truck> Trucks { get; set; }
        public string TrucksToText
        {
            get
            {
                return String.Join(", ", Trucks.Select(x => x.Number).ToArray());
            }
        }
    }
}
