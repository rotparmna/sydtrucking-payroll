namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.notification;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PayrollEmployee : BusinessBase, 
        IBusiness<model.PayrollEmployee>, 
        IPayroll<PrintPayrollEmployeeView, model.Employee>,
        IEmail<model.PayrollEmployee>
    {
        public model.PayrollEmployee Get(string id)
        {
            var builder = Builders<model.PayrollEmployee>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.PayrollEmployees.Find(filter).FirstOrDefault();
        }

        public List<model.PayrollEmployee> GetAll()
        {
            return context.PayrollEmployees.Find(FilterDefinition<model.PayrollEmployee>.Empty).ToList();
        }

        public List<PrintPayrollEmployeeView> GetListPayroll(DateTime? from, DateTime? to, model.Employee entity)
        {
            var printPayrollsView = new List<model.PrintPayrollEmployeeView>();

            var builder = Builders<model.PayrollEmployee>.Filter;
            FilterDefinition<model.PayrollEmployee> filter = FilterDefinition<model.PayrollEmployee>.Empty;

            if (from.HasValue)
                filter &= builder.Gte("From", from);

            if (to.HasValue)
                filter &= builder.Lte("To", to);

            if (entity != null)
                filter &= builder.Eq("Employee.SocialSecurity", entity.SocialSecurity);

            List<model.PayrollEmployee> payrolls = context.PayrollEmployees.Find(filter).ToList();

            payrolls.ForEach(x =>
            {
                printPayrollsView.Add(new model.PrintPayrollEmployeeView()
                {
                    Id = x.Id,
                    Employee = x.Employee.Fullname,
                    PaymentWeek = x.From.Date.ToShortDateString() + "-" + x.To.Date.ToShortDateString(),
                    Rate = x.Rate.ToString("C"),
                    TotalHours = x.TotalHours.ToString(),
                    TotalPayment = x.TotalPayment.ToString("C")
                });
            });

            return printPayrollsView;
        }

        public void SendEmail(INotification notification, model.PayrollEmployee payroll)
        {
            notification.To = payroll.Employee.Email;
            notification.Send("Pay Stub");
        }

        public void Update(model.PayrollEmployee model)
        {
            var isEdit = GetAll().Where(x => x.Id == model.Id)
                                    .Count() > 0;

            if (isEdit)
                Edit(model);
            else
                Add(model);
        }

        private void Edit(model.PayrollEmployee model)
        {
            if (model.IsDetele)
            {
                Delete(model);
            }
            else
            {
                var upd = Builders<model.PayrollEmployee>.Update.Set(u => u.Deductions, model.Deductions)
                                                         .Set(u => u.DeductionsDetail, model.DeductionsDetail)
                                                         .Set(u => u.Employee, model.Employee)
                                                         .Set(u => u.From, model.From)
                                                         .Set(u => u.PaymentDate, model.PaymentDate)
                                                         .Set(u => u.PaymentTotalHours, model.PaymentTotalHours)
                                                         .Set(u => u.Rate, model.Rate)
                                                         .Set(u => u.Reimbursements, model.Reimbursements)
                                                         .Set(u => u.ReimbursmentsDetail, model.ReimbursmentsDetail)
                                                         .Set(u => u.To, model.To)
                                                         .Set(u => u.TotalHours, model.TotalHours)
                                                         .Set(u => u.TotalPayment, model.TotalPayment);

                context.PayrollEmployees.UpdateOne(f => f.Id == model.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        private void Delete(model.PayrollEmployee model)
        {
            context.PayrollEmployees.DeleteOne(f => f.Id == model.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.PayrollEmployee = model;
            trashBusiness.Update(trash);
        }

        private void Add(model.PayrollEmployee model)
        {
            context.PayrollEmployees.InsertOne(model);
        }
    }
}
