namespace sydtrucking_payroll_front.business
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
            return context.OilCompanies.Find(FilterDefinition<model.OilCompany>.Empty)
                .ToList()
                .OrderBy(x => x.Name)
                .ToList(); ;
        }

        private void Delete(model.OilCompany company)
        {
            context.OilCompanies.DeleteOne(f => f.Id == company.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.OilCompany = company;
            trashBusiness.Update(trash);
        }

        private void Add(model.OilCompany company)
        {
            context.OilCompanies.InsertOne(company);
        }

        private void Edit(model.OilCompany company)
        {
            if (company.IsDetele)
            {
                Delete(company);
            }
            else
            {
                var upd = Builders<model.OilCompany>.Update.Set(u => u.Name, company.Name)
                                                         .Set(u => u.Rate, company.Rate);

                context.OilCompanies.UpdateOne(f => f.Id == company.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        public void Update(model.OilCompany model)
        {
            var isEdit = GetAll().Where(x => x.Id == model.Id)
                                    .Count() > 0;

            if (isEdit)
                Edit(model);
            else
                Add(model);
        }

        public model.OilCompany Get(string id)
        {
            var builder = Builders<model.OilCompany>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.OilCompanies.Find(filter).FirstOrDefault();
        }

        public long CountWithoutCurrent(string name, string currentId)
        {
            var builder = Builders<model.OilCompany>.Filter;
            FilterDefinition<model.OilCompany> filter = builder.Eq(x => x.Name, name);

            if (!string.IsNullOrEmpty(currentId))
                filter &= !builder.Eq(x => x.Id, currentId);

            return context.OilCompanies.Find(filter).CountDocuments();
        }
    }
}
