namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using sydtrucking_payroll_front.model;

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

                if (payroll.TotalHours > Constant.Payroll.TotalHoursPaying)
                {
                    var regularPayroll = new model.Payroll()
                    {
                        Deductions = payroll.Deductions,
                        Driver = payroll.Driver,
                        DeductionsDetail = payroll.DeductionsDetail,
                        From = payroll.From,
                        OvertimeHour = Constant.Payroll.TotalHoursPaying - payroll.RegularHour,
                        PaymentDate = payroll.PaymentDate,
                        PrintRegularHoursApartOvertime = payroll.PrintRegularHoursApartOvertime,
                        Rate = payroll.Rate,
                        RegularHour = payroll.RegularHour,
                        To = payroll.To,
                        TotalHours = Constant.Payroll.TotalHoursPaying,
                        TruckNumber = payroll.TruckNumber
                    };
                    regularPayroll.CalculatePayment();

                    var overtimeRegular = new model.Payroll()
                    {
                        Driver = payroll.Driver,
                        From = payroll.From,
                        OvertimeHour = payroll.TotalHours - Constant.Payroll.TotalHoursPaying,
                        PaymentDate = payroll.PaymentDate,
                        PrintRegularHoursApartOvertime = payroll.PrintRegularHoursApartOvertime,
                        Rate = payroll.Rate,
                        RegularHour = 0,
                        Reimbursements = payroll.Reimbursements,
                        ReimbursmentsDetail = payroll.ReimbursmentsDetail,
                        To = payroll.To,
                        TotalHours = payroll.TotalHours - Constant.Payroll.TotalHoursPaying,
                        TruckNumber = payroll.TruckNumber
                    };
                    overtimeRegular.CalculatePayment();

                    payroll.Details.ToList().ForEach(x =>
                    {
                        regularPayroll.Details.Add(x);
                        overtimeRegular.Details.Add(x);
                    });
                    payroll.Prints.Add(regularPayroll);
                    payroll.Prints.Add(overtimeRegular);
                }
                else
                {
                    var p = new model.Payroll()
                    {
                        Deductions = payroll.Deductions,
                        DeductionsDetail = payroll.DeductionsDetail,
                        Details = payroll.Details,
                        TotalHours = payroll.TotalHours,
                        Driver = payroll.Driver,
                        From = payroll.From,
                        OvertimeHour = payroll.OvertimeHour,
                        Payment = payroll.Payment,
                        PaymentDate = payroll.PaymentDate,
                        PaymentOvertimeHour = payroll.PaymentOvertimeHour,
                        PrintRegularHoursApartOvertime = payroll.PrintRegularHoursApartOvertime,
                        Rate = payroll.Rate,
                        RegularHour = payroll.RegularHour,
                        Reimbursements = payroll.Reimbursements,
                        ReimbursmentsDetail = payroll.ReimbursmentsDetail,
                        To = payroll.To,
                        TotalPayment = payroll.TotalPayment,
                        TruckNumber = payroll.TruckNumber
                    };
                    payroll.Prints.Add(p);
                }
            }
            context.Payrolls.InsertOne(payroll);
        }

        public void Update(model.Payroll payroll)
        {
            var isEdit = GetAll().Where(x => x.Id == payroll.Id)
                                    .Count() > 0;

            if (isEdit)
                Edit(payroll);
            else
                Add(payroll);
        }

        private void Edit(model.Payroll payroll)
        {
            if (payroll.IsDetele)
            {
                Delete(payroll);
            }
            else
            {
                var upd = Builders<model.Payroll>.Update.Set(u => u.Deductions, payroll.Deductions)
                                                         .Set(u => u.DeductionsDetail, payroll.DeductionsDetail)
                                                         .Set(u => u.Details, payroll.Details)
                                                         .Set(u => u.Driver, payroll.Driver)
                                                         .Set(u => u.From, payroll.From)
                                                         .Set(u => u.OvertimeHour, payroll.OvertimeHour)
                                                         .Set(u => u.Payment, payroll.Payment)
                                                         .Set(u => u.PaymentDate, payroll.PaymentDate)
                                                         .Set(u => u.PaymentOvertimeHour, payroll.PaymentOvertimeHour)
                                                         .Set(u => u.PrintRegularHoursApartOvertime, payroll.PrintRegularHoursApartOvertime)
                                                         .Set(u => u.Prints, payroll.Prints)
                                                         .Set(u => u.Rate, payroll.Rate)
                                                         .Set(u => u.RegularHour, payroll.RegularHour)
                                                         .Set(u => u.Reimbursements, payroll.Reimbursements)
                                                         .Set(u => u.ReimbursmentsDetail, payroll.ReimbursmentsDetail)
                                                         .Set(u => u.To, payroll.To)
                                                         .Set(u => u.TotalHours, payroll.TotalHours)
                                                         .Set(u => u.TotalPayment, payroll.TotalPayment)
                                                         .Set(u => u.TruckNumber, payroll.TruckNumber);

                context.Payrolls.UpdateOne(f => f.Id == payroll.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        private void Delete(model.Payroll payroll)
        {
            context.Payrolls.DeleteOne(f => f.Id == payroll.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.Payroll = payroll;
            trashBusiness.Update(trash);
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
