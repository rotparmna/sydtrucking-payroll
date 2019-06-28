namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using sydtrucking_payroll_front.model;

    public class Employee : BusinessBase, IBusiness<model.Driver>
    {

        public Employee() : base() { }

        public List<model.Driver> GetAll()
        {
            return context.Drivers.Find(FilterDefinition<model.Driver>.Empty).ToList();
        }

        private void Add(model.Driver employee)
        {
            context.Drivers.InsertOne(employee);
        }

        private void Edit(model.Driver employee)
        {
            if (employee.IsDetele)
            {
                Delete(employee);
            }
            else
            {
                var upd = Builders<model.Driver>.Update.Set(u => u.Address, employee.Address)
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

                context.Drivers.UpdateOne(f => f.Id == employee.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        private void Delete(model.Driver employee)
        {
            context.Drivers.DeleteOne(f => f.Id == employee.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.Driver = employee;
            trashBusiness.Update(trash);
        }

        public void Update(model.Driver employee)
        {
            var isEdit = GetAll().Where(x => x.SocialSecurity == employee.SocialSecurity)
                                    .Count() > 0;

            if (isEdit)
                Edit(employee);
            else
                Add(employee);
        }

        public model.Driver Get(string id)
        {
            var builder = Builders<model.Driver>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.Drivers.Find(filter).FirstOrDefault();
        }
    }
}
