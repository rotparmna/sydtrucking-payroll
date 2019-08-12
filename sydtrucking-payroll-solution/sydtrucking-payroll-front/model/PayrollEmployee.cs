namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class PayrollEmployee : ModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Employee Employee { get; set; }
        public double Rate { get; set; }
        public int TotalHours { get; set; }
        public double TotalPayment { get; set; }
        public double Deductions { get; set; }
        public double Reimbursements { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime PaymentDate { get; set; }
        public double PaymentTotalHours
        {
            get
            {
                return Rate * TotalHours;
            }
        }
        public string DeductionsDetail { get; set; }
        public string ReimbursmentsDetail { get; set; }
    }
}
