namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class PayrollLeaseCompany
    {
        public PayrollLeaseCompany()
        {
            Payrolls = new List<Payroll>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public LeaseCompany LeaseCompany { get; set; }
        public DateTime Date { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }
        public ICollection<GenericCollection> Details { get; set; }
        public ICollection<GenericCollection> Deductions { get; set; }
        public string Companies
        {
            get
            {
                string companies = string.Empty;
                if (Payrolls.Count > 0)
                {
                    Payrolls.ToList().ForEach(x =>
                    {
                        x.Details.ToList().ForEach(y =>
                        {
                            companies += y.OilCompany.Name + ",";
                        });
                    });
                    companies = companies.TrimEnd(',');
                }
                return companies;
            }
        }
        public int Hours
        {
            get
            {
                int hours = 0;
                if (Payrolls.Count > 0)
                {
                    Payrolls.ToList().ForEach(x =>
                    {
                        hours = x.Details.ToList().Sum(y => y.Hours);
                    });
                }
                return hours;
            }
        }
        public double DriverPaycheck
        {
            get
            {
                return Payrolls.Sum(x => x.TotalPayment);
            }
        }
        public double Subtotal { get; set; }
        public double Total { get; set; }
        public double TotalDetails
        {
            get
            {
                return Details.Sum(x => x.Value);
            }
        }
        public double TotalDeductions
        {
            get
            {
                return Deductions.Sum(x => x.Value);
            }
        }
    }
}
