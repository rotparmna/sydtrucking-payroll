namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class Truck : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Number { get; set; }
        public int Year { get; set; }
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Plate { get; set; }
        public DateTime Registration { get; set; }
        public DateTime Inspection { get; set; }
    }
}
