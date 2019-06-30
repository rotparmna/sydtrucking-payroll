namespace sydtrucking_payroll_front.business
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;

    public class PayrollEmployee : BusinessBase, IBusiness<model.PayrollEmployee>
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

        public void Update(model.PayrollEmployee model)
        {
            Add(model);
        }

        private void Add(model.PayrollEmployee model)
        {
            context.PayrollEmployees.InsertOne(model);
        }
    }
}
