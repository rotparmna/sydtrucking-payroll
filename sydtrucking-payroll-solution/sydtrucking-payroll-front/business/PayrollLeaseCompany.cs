namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;

    public class PayrollLeaseCompany : BusinessBase, IBusiness<model.PayrollLeaseCompany>
    {
        public model.PayrollLeaseCompany Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<model.PayrollLeaseCompany> GetAll()
        {
            return context.PayrollLeaseCompanies.Find(FilterDefinition<model.PayrollLeaseCompany>.Empty).ToList();
        }

        public void Update(model.PayrollLeaseCompany payroll)
        {
            Add(payroll);
        }

        private void Add(model.PayrollLeaseCompany payroll)
        {
            context.PayrollLeaseCompanies.InsertOne(payroll);
        }
    }
}
