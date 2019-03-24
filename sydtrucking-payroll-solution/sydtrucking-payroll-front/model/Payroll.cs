namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using sydtrucking_payroll_front.enums;
    using System.Collections.Generic;

    public class Payroll
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public Employee EmployeeId { get; set; }
        public int TruckNumber { get; set; }
        public double Rate { get; set; }
        public PaymentType PaymentType { get; set; }
        public ICollection<PayrollDetail> Details { get; set; }
        public int TotalHours { get; set; }
        public int RegularHour { get; set; }
        public int OvertimeHour { get; set; }
        public double Payment { get; set; }
        public double Deductions { get; set; }
        public double Reimbursements { get; set; }
        public double TotalPaymen { get; set; }
    }
}
