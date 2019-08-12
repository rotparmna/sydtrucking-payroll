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

        public List<PrintPayrollEmployeeView> GetListPayroll(DateTime from, DateTime to, model.Employee entity)
        {
            var printPayrollsView = new List<model.PrintPayrollEmployeeView>();

            var builder = Builders<model.PayrollEmployee>.Filter;
            var filter = builder.Gte("From", from) &
                            builder.Lte("To", to) &
                            builder.Eq("Employee.SocialSecurity", entity.SocialSecurity);

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
            if (model.IsDetele)
                Delete(model);
            else
                Add(model);
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
