namespace sydtrucking_payroll_front.business
{
    using System.Collections.Generic;
    using MongoDB.Driver;
    using sydtrucking_payroll_front.model;

    public class Configuration : BusinessBase, IBusiness<model.Configuration>
    {
        public Configuration() : base() { }

        public model.Configuration Get(string id)
        {
            throw new System.NotImplementedException();
        }

        public List<model.Configuration> GetAll()
        {
            return context.Configurations.Find(FilterDefinition<model.Configuration>.Empty).ToList();
        }

        public void Update(model.Configuration model)
        {
            throw new System.NotImplementedException();
        }
    }
}
