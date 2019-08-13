namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using sydtrucking_payroll_front.model;

    public class PayrollLeaseCompany : BusinessBase,
        IBusiness<model.PayrollLeaseCompany>,
        IPayroll<model.PrintPayrollLeaseCompanyView, model.LeaseCompany>
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
            var builder = Builders<model.PayrollLeaseCompany>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.PayrollLeaseCompanies.Find(filter).FirstOrDefault();
        }

        public List<model.PayrollLeaseCompany> GetAll()
        {
            return context.PayrollLeaseCompanies.Find(FilterDefinition<model.PayrollLeaseCompany>.Empty).ToList();
        }

        public void Update(model.PayrollLeaseCompany payroll)
        {
            var isEdit = GetAll().Where(x => x.Id == payroll.Id)
                                     .Count() > 0;

            if (isEdit)
                Edit(payroll);
            else
                Add(payroll);
        }

        private void Edit(model.PayrollLeaseCompany payroll)
        {
            if (payroll.IsDetele)
            {
                Delete(payroll);
            }
            else
            {
                var upd = Builders<model.PayrollLeaseCompany>.Update.Set(u => u.Deductions, payroll.Deductions)
                                                         .Set(u => u.Date, payroll.Date)
                                                         .Set(u => u.Details, payroll.Details)
                                                         .Set(u => u.DriverPaycheck, payroll.DriverPaycheck)
                                                         .Set(u => u.From, payroll.From)
                                                         .Set(u => u.LeaseCompany, payroll.LeaseCompany)
                                                         .Set(u => u.Payrolls, payroll.Payrolls)
                                                         .Set(u => u.Rates, payroll.Rates)
                                                         .Set(u => u.To, payroll.To)
                                                         .Set(u => u.Total, payroll.Total)
                                                         .Set(u => u.TotalDeductions, payroll.TotalDeductions)
                                                         .Set(u => u.TotalDetails, payroll.TotalDetails)
                                                         .Set(u => u.Truck, payroll.Truck);

                context.PayrollLeaseCompanies.UpdateOne(f => f.Id == payroll.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        private void Delete(model.PayrollLeaseCompany payroll)
        {
            context.PayrollLeaseCompanies.DeleteOne(f => f.Id == payroll.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.PayrollLeaseCompany = payroll;
            trashBusiness.Update(trash);
        }

        private void Add(model.PayrollLeaseCompany payroll)
        {
            context.PayrollLeaseCompanies.InsertOne(payroll);
        }
    }
}
