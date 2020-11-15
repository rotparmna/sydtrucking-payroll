namespace sydtrucking_payroll_front.business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Driver;

    public class LeaseCompany : BusinessBase, IBusiness<model.LeaseCompany>
    {
        public model.LeaseCompany Get(string id)
        {
            var builder = Builders<model.LeaseCompany>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.LeaseCompanies.Find(filter).FirstOrDefault();
        }

        public List<model.LeaseCompany> GetAll()
        {
            return context.LeaseCompanies.Find(FilterDefinition<model.LeaseCompany>.Empty)
                .ToList()
                .OrderBy(x => x.Name)
                .ToList(); ;
        }

        private void Delete(model.LeaseCompany commpany)
        {
            context.LeaseCompanies.DeleteOne(f => f.Id == commpany.Id);

            Trash trashBusiness = new Trash();
            model.Trash trash = new model.Trash();
            trash.LeaseCompany = commpany;
            trashBusiness.Update(trash);
        }

        private void Add(model.LeaseCompany company)
        {
            context.LeaseCompanies.InsertOne(company);
        }

        private void Edit(model.LeaseCompany company)
        {
            if (company.IsDetele)
            {
                Delete(company);
            }
            else
            {
                var upd = Builders<model.LeaseCompany>.Update.Set(u => u.Name, company.Name)
                                                         .Set(u => u.Email, company.Email)
                                                         .Set(u => u.Trucks, company.Trucks);

                context.LeaseCompanies.UpdateOne(f => f.Id == company.Id, upd, new UpdateOptions() { IsUpsert = false });
            }
        }

        public void Update(model.LeaseCompany model)
        {
            var isEdit = GetAll().Where(x => x.Id == model.Id)
                                    .Count() > 0;

            if (isEdit)
                Edit(model);
            else
                Add(model);
        }
    }
}
