namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Collections.Generic;

    public class Payroll : ModelBase
    {
        public Payroll()
        {
            Details = new List<PayrollDetail>();
            Prints = new List<Payroll>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Driver Driver { get; set; }
        public int TruckNumber { get; set; }
        public double Rate { get; set; }
        public ICollection<PayrollDetail> Details { get; set; }
        public double TotalHours { get; set; }
        public double RegularHour { get; set; }
        public double OvertimeHour { get; set; }
        public double Payment { get; set; }
        public double Deductions { get; set; }
        public double Reimbursements { get; set; }
        public double TotalPayment { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime PaymentDate { get;  set; }
        public double PaymentOvertimeHour { get; set; }
        public double PaymentRegularHour
        {
            get
            {
                return Rate * RegularHour;
            }
        }

        public string DeductionsDetail { get; set; }
        public string ReimbursmentsDetail { get; set; }
        public bool PrintRegularHoursApartOvertime { get; set; }
        public ICollection<Payroll> Prints { get; set; }

        public void CalculatePayment()
        {
            var rateOvertime = Rate * business.Constant.Payroll.FactorRateOvertimeHour;
            double paymentOvertimeHour = rateOvertime * OvertimeHour;
            double payment = Rate * RegularHour + paymentOvertimeHour;

            Payment = payment;
            PaymentOvertimeHour = paymentOvertimeHour;
            
            CalculateTotalPayment();
        }

        public void CalculateTotalPayment()
        {
            TotalPayment = Payment - Deductions + Reimbursements;
        }
    }
}
