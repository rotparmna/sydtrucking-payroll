﻿namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
    using System.Collections.Generic;
    using System.Linq;

    public class Employee
    {
        private PayrollContext context;

        public Employee()
        {
            context = new PayrollContext(Properties.Settings.Default);
        }

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
            var upd = Builders<model.Employee>.Update.Set(u => u.Address, employee.Address)
                                                     .Set(u => u.Birthdate, employee.Birthdate)
                                                     .Set(u => u.Contract, employee.Contract)
                                                     .Set(u => u.LastName, employee.LastName)
                                                     .Set(u => u.License, employee.License)
                                                     .Set(u => u.Name, employee.Name)
                                                     .Set(u => u.PaymentMethod, employee.PaymentMethod)
                                                     .Set(u => u.PhoneNumber, employee.PhoneNumber)
                                                     .Set(u => u.Rate, employee.Rate)
                                                     .Set(u => u.SocialSecurity, employee.SocialSecurity)
                                                     .Set(u => u.State, employee.State)
                                                     .Set(u => u.TaxForm, employee.TaxForm)
                                                     .Set(u => u.TruckNumber, employee.TruckNumber);

            context.Employees.UpdateOne(f => f.Id == employee.Id, upd, new UpdateOptions() { IsUpsert = false });
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
    }
}
