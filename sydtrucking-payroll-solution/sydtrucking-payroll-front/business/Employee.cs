namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;

    public class Employee : BusinessBase, IBusiness<model.Employee>
    {
        public model.Employee Get(string id)
        {
            var builder = Builders<model.Employee>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.Employees.Find(filter).FirstOrDefault();
        }

        public List<model.Employee> GetAll()
        {
            return context.Employees.Find(FilterDefinition<model.Employee>.Empty)
                .ToList()
                .OrderBy(x => x.Name)
                .ToList(); ;
        }

        public void Update(model.Employee model)
        {
            var isEdit = GetAll().Where(x => x.SocialSecurity == model.SocialSecurity)
                                    .Count() > 0;

            if (isEdit)
                Edit(model);
            else
                Add(model);
        }

        private void Add(model.Employee model)
        {
            context.Employees.InsertOne(model);
        }

        private void Edit(model.Employee model)
        {
            if (model.IsDetele)
            {
                Delete(model);
            }
            else
            {
                var upd = Builders<model.Employee>.Update.Set(u => u.Address, model.Address)
                                                         .Set(u => u.Birthdate, model.Birthdate)
                                                         .Set(u => u.LastName, model.LastName)
                                                         .Set(u => u.License, model.License)
                                                         .Set(u => u.Name, model.Name)
                                                         .Set(u => u.PaymentMethod, model.PaymentMethod)
                                                         .Set(u => u.PhoneNumber, model.PhoneNumber)
                                                         .Set(u => u.Rate, model.Rate)
                                                         .Set(u => u.State, model.State)
                                                         .Set(u => u.City, model.City)
                                                         .Set(u => u.ZipCode, model.ZipCode)
                                                         .Set(u => u.TaxForm, model.TaxForm)
                                                         .Set(u => u.Email, model.Email)
                                                         .Set(u => u.Job, model.Job)
                                                         .Set(u => u.IsWeeklyPayment, model.IsWeeklyPayment);

                context.Employees.UpdateOne(f => f.Id == model.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        private void Delete(model.Employee employee)
        {
            context.Employees.DeleteOne(f => f.Id == employee.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.Employee = employee;
            trashBusiness.Update(trash);
        }
    }
}
