﻿namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Payroll : BusinessBase, 
        IBusiness<model.Payroll>,
        IEmail<model.Payroll>
    {
        public Payroll() : base() { }

        public List<model.Payroll> GetByDateAndTruck(DateTime dateTo, DateTime dateFrom, model.Truck truck)
        {
            var builder = Builders<model.Payroll>.Filter;
            var filter = builder.Gte(x => x.From, dateFrom) &
                            builder.Lte(x => x.To, dateTo) &
                            builder.Eq(x => x.TruckNumber, int.Parse(truck.Number));

            return context.Payrolls.Find(filter).ToList();
        }

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

        public List<model.PrintPayrollView> GetListPayroll(DateTime from, DateTime to, model.Driver employee)
        {
            var printPayrollsView = new List<model.PrintPayrollView>();

            var builder = Builders<model.Payroll>.Filter;
            var filter = builder.Gte("Details.Ticket.Date", from) &
                            builder.Lte("Details.Ticket.Date", to) &
                            builder.Eq("Driver.SocialSecurity", employee.SocialSecurity);

            List<model.Payroll> payrolls = context.Payrolls.Find(filter).ToList();

            payrolls.ForEach(x =>
            {
                printPayrollsView.Add(new model.PrintPayrollView()
                {
                    Id = x.Id,
                    Driver = x.Driver.Fullname,
                    PaymentWeek = x.Details.Min(y => y.Ticket.Date).Date.ToShortDateString() + "-" + x.Details.Max(y => y.Ticket.Date).Date.ToShortDateString(),
                    Rate = x.Rate.ToString("C"),
                    TotalHours = x.TotalHours.ToString(),
                    TotalPayment = x.TotalPayment.ToString("C")
                });
            });

            return printPayrollsView;
        }

        public void SendEmail(INotification notification, model.Payroll payroll)
        {
            notification.To = payroll.Driver.Email;
            notification.Send("Pay Stub");
        }
    }
}
