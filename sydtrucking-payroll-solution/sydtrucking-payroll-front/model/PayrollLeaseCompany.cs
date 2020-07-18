namespace sydtrucking_payroll_front.model
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.Linq;
    using System.Collections.Generic;

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
        public ICollection<Payroll> Payrolls { get; set; }
        public ICollection<GenericCollection> Details { get; set; }
        public ICollection<GenericCollection> Deductions { get; set; }
        public ICollection<RateDetail> Rates
        {
            get
            {
                ICollection<RateDetail> rates = new List<RateDetail>();
                if (Payrolls.Count > 0)
                {
                    rates = Payrolls.GroupBy(x => x.Rate)
                        .Select(y => new RateDetail()
                        {
                            Rate = y.Key,
                            Hours = y.Sum(h => h.Details.Sum(r => r.Hours)),
                            Companies = y.Select(p => String.Join(",", p.Details.Select(c => c.OilCompany.Name).ToArray())).FirstOrDefault()
                        })
                        .ToList();
                    
                }
                return rates;
            }
        }
        public double DriverPaycheck
        {
            get
            {
                return Payrolls.Sum(x => x.TotalPayment);
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
