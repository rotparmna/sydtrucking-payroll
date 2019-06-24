namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using sydtrucking_payroll_front.model;

    public class Employee : BusinessBase, IBusiness<model.Employee>
    {

        public Employee() : base() { }

        public List<model.Employee> GetAll()
        {
            return context.Employees.Find(FilterDefinition<model.Employee>.Empty).ToList();
        }

        private void Add(model.Employee employee)
        {
            context.Employees.InsertOne(employee);
        }

        private void Edit(model.Employee employee)
        {
            if (employee.IsDetele)
            {
                Delete(employee);
            }
            else
            {
                var upd = Builders<model.Employee>.Update.Set(u => u.Address, employee.Address)
                                                         .Set(u => u.Birthdate, employee.Birthdate)
                                                         .Set(u => u.Contract, employee.Contract)
                                                         .Set(u => u.LastName, employee.LastName)
                                                         .Set(u => u.License, employee.License)
                                                         .Set(u => u.Name, employee.Name)
                                                         .Set(u => u.PaymentMethod, employee.PaymentMethod)
                                                         .Set(u => u.PhoneNumber, employee.PhoneNumber)
                                                         .Set(u => u.Rate, employee.Rate)
                                                         .Set(u => u.State, employee.State)
                                                         .Set(u => u.City, employee.City)
                                                         .Set(u => u.ZipCode, employee.ZipCode)
                                                         .Set(u => u.TaxForm, employee.TaxForm)
                                                         .Set(u => u.Truck, employee.Truck)
                                                         .Set(u => u.Email, employee.Email);

                context.Employees.UpdateOne(f => f.Id == employee.Id, upd, new UpdateOptions() { IsUpsert = false });
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

        public void Update(model.Employee employee)
        {
            var isEdit = GetAll().Where(x => x.SocialSecurity == employee.SocialSecurity)
                                    .Count() > 0;

            if (isEdit)
                Edit(employee);
            else
                Add(employee);
        }

        public model.Employee Get(string id)
        {
            var builder = Builders<model.Employee>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.Employees.Find(filter).FirstOrDefault();
        }
    }
}
