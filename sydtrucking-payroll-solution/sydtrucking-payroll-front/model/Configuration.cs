namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Configuration
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Smtp Smtp { get; set; }
        public PayrollConfiguration Payroll { get; set; }
        public PayrollLeaseCompanyConfiguration PayrollLeaseCompany { get; set; }
    }

    public class Smtp
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class PayrollConfiguration
    {
        public int RegularHour { get; set; }
        public int TotalHoursPaying { get; set; }
        public double FactorRateOvertimeHour { get; set; }
        public int DaysWeek { get; set; }
        public int DaysWeekPayment { get; set; }
    }

    public class PayrollLeaseCompanyConfiguration
    {
        public int LastThreeFridayPayrollLeaseCompany { get; set; }
        public double PercentLeaseFeeValue { get; set; }
        public double PercentWorkerCompValue { get; set; }
    }
}
