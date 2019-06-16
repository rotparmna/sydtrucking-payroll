﻿namespace sydtrucking_payroll_front.business
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;
    using sydtrucking_payroll_front.model;

    public class OilCompany : BusinessBase, IBusiness<model.OilCompany>
    {
        public OilCompany() : base() { }

        public List<model.OilCompany> GetAll()
        {
            return context.OilCompanies.Find(FilterDefinition<model.OilCompany>.Empty).ToList();
        }

        private void Add(model.OilCompany company)
        {
            context.OilCompanies.InsertOne(company);
        }

        private void Edit(model.OilCompany company)
        {
            var upd = Builders<model.OilCompany>.Update.Set(u => u.Name, company.Name)
                                                     .Set(u => u.Rate, company.Rate);

            context.OilCompanies.UpdateOne(f => f.Id == company.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public void Update(model.OilCompany model)
        {
            var isEdit = GetAll().Where(x => x.Name == model.Name)
                                    .Count() > 0;

            if (isEdit)
                Edit(model);
            else
                Add(model);
        }

        public model.OilCompany Get(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}