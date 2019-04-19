namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Payroll
    {
        private PayrollContext context;

        public Payroll()
        {
            context = new PayrollContext(Properties.Settings.Default);
        }

        public List<model.Payroll> GetAll()
        {
            return context.Payrolls.Find(FilterDefinition<model.Payroll>.Empty).ToList();
        }

        private void Add(model.Payroll payroll)
        {
            context.Payrolls.InsertOne(payroll);
        }

        public void Update(model.Payroll payroll)
        {
            Add(payroll);
        }

        public List<model.PrintPayrollView> GetListPayroll(DateTime from, DateTime to)
        {
            var printPayrollsView = new List<model.PrintPayrollView>();

            var builder = Builders<model.Payroll>.Filter;
            var filter = builder.Gte("Details.Ticket.Date", from) & 
                            builder.Lte("Details.Ticket.Date", to);

            List<model.Payroll> payrolls = context.Payrolls.Find(filter).ToList();

            payrolls.ForEach(x =>
            {
                printPayrollsView.Add(new model.PrintPayrollView()
                {
                    Driver = x.Employee.Fullname,
                    PaymentWeek = x.Details.Min(y => y.Ticket.Date).Date.ToShortDateString() + "-" + x.Details.Max(y => y.Ticket.Date).Date.ToShortDateString(),
                    Rate = x.Rate.ToString("C"),
                    TotalHours = x.TotalHours.ToString(),
                    TotalPayment = x.TotalPayment.ToString("C")
                });
            });

            return printPayrollsView;
        }
    }
}
