namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class PayrollDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public Ticket TicketId { get; set; }
        public string Company { get; set; }
        public int Hours { get; set; }
    }
}