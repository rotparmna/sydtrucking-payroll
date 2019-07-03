namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Payroll : BusinessBase, 
        IBusiness<model.Payroll>,
        IEmail<model.Payroll>,
        IPayroll<model.PrintPayrollView, model.Driver>
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
            if (payroll.PrintRegularHoursApartOvertime)
            {
                List<Payroll> payrolls = new List<Payroll>();

                if (payroll.TotalHours > Constant.Payroll.RegularHour)
                {
                    var regularPayroll = new model.Payroll()
                    {
                        //Deductions = payroll.Deductions,
                        Driver = payroll.Driver,
                        //DeductionsDetail = payroll.DeductionsDetail,
                        From = payroll.From,
                        //OvertimeHour = payroll.OvertimeHour,
                        Payment = payroll.Rate * payroll.RegularHour,
                        PaymentDate = payroll.PaymentDate,
                        //PaymentOvertimeHour = payroll.PaymentOvertimeHour,
                        //PrintRegularHoursApartOvertime = payroll.PrintRegularHoursApartOvertime,
                        Rate = payroll.Rate,
                        RegularHour = payroll.RegularHour,
                        //Reimbursements = payroll.Reimbursements,
                        //ReimbursmentsDetail = payroll.ReimbursmentsDetail,
                        To = payroll.To,
                        TotalHours = payroll.RegularHour,
                        TotalPayment = payroll.Rate * payroll.RegularHour,
                        TruckNumber = payroll.TruckNumber
                    };
                    var overtimeRegular = new model.Payroll()
                    {
                        Deductions = payroll.Deductions,
                        Driver = payroll.Driver,
                        DeductionsDetail = payroll.DeductionsDetail,
                        From = payroll.From,
                        OvertimeHour = payroll.OvertimeHour,
                        Payment = payroll.PaymentOvertimeHour,
                        PaymentDate = payroll.PaymentDate,
                        PaymentOvertimeHour = payroll.PaymentOvertimeHour,
                        //PrintRegularHoursApartOvertime = payroll.PrintRegularHoursApartOvertime,
                        Rate = payroll.Rate,
                        //RegularHour = payroll.RegularHour,
                        Reimbursements = payroll.Reimbursements,
                        ReimbursmentsDetail = payroll.ReimbursmentsDetail,
                        To = payroll.To,
                        TotalHours = payroll.OvertimeHour,
                        TotalPayment = payroll.PaymentOvertimeHour,
                        TruckNumber = payroll.TruckNumber
                    };
                    var hours = 0;
                    payroll.Details.ToList().ForEach(x =>
                    {
                        hours += x.Hours;
                        if (hours <= Constant.Payroll.RegularHour)
                        {
                            regularPayroll.Details.Add(x);
                        }
                        else
                        {
                            overtimeRegular.Details.Add(x);
                        }
                    });
                    payroll.Prints.Add(regularPayroll);
                    payroll.Prints.Add(overtimeRegular);
                }
                else
                {
                    payroll.Prints.Add(payroll);
                }
            }
            context.Payrolls.InsertOne(payroll);
        }

        public void Update(model.Payroll payroll)
        {
            Add(payroll);
        }

        public List<model.PrintPayrollView> GetListPayroll(DateTime from, DateTime to, model.Driver driver)
        {
            var printPayrollsView = new List<model.PrintPayrollView>();

            var builder = Builders<model.Payroll>.Filter;
            var filter = builder.Gte("Details.Ticket.Date", from) &
                            builder.Lte("Details.Ticket.Date", to) &
                            builder.Eq("Driver.SocialSecurity", driver.SocialSecurity);

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
