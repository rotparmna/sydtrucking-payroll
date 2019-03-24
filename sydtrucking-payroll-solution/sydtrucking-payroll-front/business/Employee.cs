namespace sydtrucking_payroll_front.business
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

        public void Add(model.Employee employee)
        {
            context.Employees.InsertOne(employee);
        }
    }
}
