namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
    using System.Collections.Generic;
    using System.Linq;

    public class Payroll
    {
        private PayrollContext context;

        public Payroll()
        {
            context = new PayrollContext(Properties.Settings.Default);
        }

        public List<model.Payroll> GetAll()
        {
            return context.Payrolls.Find(FilterDefinition<model.Payroll>.Empty).ToList();
        }

        private void Add(model.Payroll payroll)
        {
            context.Payrolls.InsertOne(payroll);
        }

        public void Update(model.Payroll payroll)
        {
            Add(payroll);
        }
    }
}
