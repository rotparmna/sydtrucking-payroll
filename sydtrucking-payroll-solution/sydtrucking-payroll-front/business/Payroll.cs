namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Payroll : BusinessBase, IBusiness<model.Payroll>
    {
        public Payroll() : base() { }

        public List<model.Payroll> GetAll()
        {
            return context.Payrolls.Find(FilterDefinition<model.Payroll>.Empty).ToList();
        }

        public model.Payroll Get(string id)
        {
            var builder = Builders<model.Payroll>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.Payrolls.Find(filter).FirstOrDefault();
        }

        private void Add(model.Payroll payroll)
        {
            context.Payrolls.InsertOne(payroll);
        }

        public void Update(model.Payroll payroll)
        {
            Add(payroll);
        }

        public List<model.PrintPayrollView> GetListPayroll(DateTime from, DateTime to, model.Employee employee)
        {
            var printPayrollsView = new List<model.PrintPayrollView>();

            var builder = Builders<model.Payroll>.Filter;
            var filter = builder.Gte("Details.Ticket.Date", from) &
                            builder.Lte("Details.Ticket.Date", to) &
                            builder.Eq("Employee.SocialSecurity", employee.SocialSecurity);

            List<model.Payroll> payrolls = context.Payrolls.Find(filter).ToList();

            payrolls.ForEach(x =>
            {
                printPayrollsView.Add(new model.PrintPayrollView()
                {
                    Id = x.Id,
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
