namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
    using System.Collections.Generic;
    using System.Linq;

    public class Role
    {
        private PayrollContext context;

        public Role()
        {
            context = new PayrollContext(Properties.Settings.Default);
        }

        public List<model.Role> GetAll()
        {
            return context.Roles.Find(FilterDefinition<model.Role>.Empty).ToList();
        }

        private void Add(model.Role Role)
        {
            context.Roles.InsertOne(Role);
        }

        private void Edit(model.Role role)
        {
            var upd = Builders<model.Role>.Update.Set(u => u.Name, role.Name);

            context.Roles.UpdateOne(f => f.Id == role.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public void Update(model.Role role)
        {
            var isEdit = GetAll().Where(x => x.Id == role.Id)
                                    .Count() > 0;

            if (isEdit)
                Edit(role);
            else
                Add(role);
        }
    }
}
