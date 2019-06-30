namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using sydtrucking_payroll_front.model;

    public class Driver : BusinessBase, IBusiness<model.Driver>
    {

        public Driver() : base() { }

        public List<model.Driver> GetAll()
        {
            return context.Drivers.Find(FilterDefinition<model.Driver>.Empty).ToList();
        }

        private void Add(model.Driver driver)
        {
            context.Drivers.InsertOne(driver);
        }

        private void Edit(model.Driver driver)
        {
            if (driver.IsDetele)
            {
                Delete(driver);
            }
            else
            {
                var upd = Builders<model.Driver>.Update.Set(u => u.Address, driver.Address)
                                                         .Set(u => u.Birthdate, driver.Birthdate)
                                                         .Set(u => u.Contract, driver.Contract)
                                                         .Set(u => u.LastName, driver.LastName)
                                                         .Set(u => u.License, driver.License)
                                                         .Set(u => u.Name, driver.Name)
                                                         .Set(u => u.PaymentMethod, driver.PaymentMethod)
                                                         .Set(u => u.PhoneNumber, driver.PhoneNumber)
                                                         .Set(u => u.Rate, driver.Rate)
                                                         .Set(u => u.State, driver.State)
                                                         .Set(u => u.City, driver.City)
                                                         .Set(u => u.ZipCode, driver.ZipCode)
                                                         .Set(u => u.TaxForm, driver.TaxForm)
                                                         .Set(u => u.Truck, driver.Truck)
                                                         .Set(u => u.Email, driver.Email);

                context.Drivers.UpdateOne(f => f.Id == driver.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        private void Delete(model.Driver driver)
        {
            context.Drivers.DeleteOne(f => f.Id == driver.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.Driver = driver;
            trashBusiness.Update(trash);
        }

        public void Update(model.Driver driver)
        {
            var isEdit = GetAll().Where(x => x.SocialSecurity == driver.SocialSecurity)
                                    .Count() > 0;

            if (isEdit)
                Edit(driver);
            else
                Add(driver);
        }

        public model.Driver Get(string id)
        {
            var builder = Builders<model.Driver>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.Drivers.Find(filter).FirstOrDefault();
        }
    }
}
