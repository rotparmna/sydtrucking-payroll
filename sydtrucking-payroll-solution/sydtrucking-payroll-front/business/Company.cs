namespace sydtrucking_payroll_front.business
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;

    public class Company : BusinessBase, IBusiness<model.Company>
    {
        public Company() : base() { }

        public List<model.Company> GetAll()
        {
            return context.Companies.Find(FilterDefinition<model.Company>.Empty).ToList();
        }

        private void Add(model.Company company)
        {
            context.Companies.InsertOne(company);
        }

        private void Edit(model.Company company)
        {
            var upd = Builders<model.Company>.Update.Set(u => u.Name, company.Name)
                                                     .Set(u => u.Rate, company.Rate);

            context.Companies.UpdateOne(f => f.Id == company.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public void Update(model.Company model)
        {
            var isEdit = GetAll().Where(x => x.Name == model.Name)
                                    .Count() > 0;

            if (isEdit)
                Edit(model);
            else
                Add(model);
        }
    }
}
