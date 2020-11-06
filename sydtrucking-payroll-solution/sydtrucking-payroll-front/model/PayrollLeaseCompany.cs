namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class PayrollLeaseCompany : ModelBase
    {
        public PayrollLeaseCompany()
        {
            Payrolls = new List<Payroll>();
            Details = new List<GenericCollection>();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Truck Truck { get; set; }
        public LeaseCompany LeaseCompany { get; set; }
        public DateTime Date { get; set; }
        public DateTime To { get; set; }
        public DateTime From { get; set; }
        public double PercentLeaseFeeValue { get; set; }
        public double PercentWorkerCompValue { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }
        public ICollection<GenericCollection> Details { get; set; }
        public ICollection<GenericCollection> Deductions { get; set; }
        public ICollection<GenericCollection> Reimbursements { get; set; }
        public ICollection<RateDetail> Rates
        {
            get
            {
                ICollection<RateDetail> rates = new List<RateDetail>();
                if (Payrolls.Count > 0)
                {
                    IList<PayrollDetail> details = new List<PayrollDetail>();
                    Payrolls.Select(x => x.Details).ToList().ForEach(x => x.ToList().ForEach(y=>details.Add(y)));
                    details.Select(y => y.OilCompany)
                                                    .GroupBy(z => z.Rate)
                                                    .Select(m => new RateDetail()
                                                    {
                                                        Rate = m.Key,
                                                        Hours = details.Where(x=>x.OilCompany.Rate == m.Key).Sum(r => r.Hours),
                                                        Companies = string.Join(",", m.Select(c => c.Name).Distinct())
                                                    })
                    .ToList()
                    .ForEach(g => rates.Add(g));
                }
                return rates;
            }
        }
        public double DriverPaycheck
        {
            get
            {
                return Payrolls.Sum(x => x.Payment);
            }
        }
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
        public double TotalReimbursements
        {
            get
            {
                return Reimbursements.Sum(x => x.Value);
            }
        }
    }

    public class RateDetail
    {
        public string Companies { get; set; }
        public double Hours { get; set; }
        public double Rate { get; set; }
        public double Subtotal
        {
            get
            {
                return Hours * Rate;
            }
        }
    }
}
