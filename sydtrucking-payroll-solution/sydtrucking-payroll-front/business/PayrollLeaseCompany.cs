namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;

    public class PayrollLeaseCompany : BusinessBase, IBusiness<model.PayrollLeaseCompany>
    {
        public List<model.PrintPayrollLeaseCompanyView> GetListPayroll(DateTime from, DateTime to, model.LeaseCompany leaseCompany)
        {
            var printPayrollsView = new List<model.PrintPayrollLeaseCompanyView>();

            var builder = Builders<model.PayrollLeaseCompany>.Filter;
            var filter = builder.Gte("Date", from) &
                            builder.Lte("Date", to) &
                            builder.Eq("LeaseCompany.Id", leaseCompany.Id);

            List<model.PayrollLeaseCompany> payrollLeaseCompanies = context.PayrollLeaseCompanies.Find(filter).ToList();

            payrollLeaseCompanies.ForEach(x =>
            {
                printPayrollsView.Add(new model.PrintPayrollLeaseCompanyView()
                {
                    Id = x.Id,
                    LeaseCompany = x.LeaseCompany.Name,
                    TruckNumber = x.Truck.Number,
                    PaymentWeek = x.From.Date.ToShortDateString() + "-" + x.To.Date.ToShortDateString(),
                    TotalPayment = x.Total.ToString("C")
                });
            });

            return printPayrollsView;
        }

        public model.PayrollLeaseCompany Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<model.PayrollLeaseCompany> GetAll()
        {
            return context.PayrollLeaseCompanies.Find(FilterDefinition<model.PayrollLeaseCompany>.Empty).ToList();
        }

        public void Update(model.PayrollLeaseCompany payroll)
        {
            Add(payroll);
        }

        private void Add(model.PayrollLeaseCompany payroll)
        {
            context.PayrollLeaseCompanies.InsertOne(payroll);
        }
    }
}
