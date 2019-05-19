namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using sydtrucking_payroll_front.model;

    public class User : BusinessBase, IBusiness<model.User>
    {
        public User() : base() { }

        public List<model.User> GetAll()
        {
            var builder = Builders<model.User>.Filter;
            var filter = builder.Not(builder.Eq("Username", "admin"));

            return context.Users.Find(filter).ToList();
        }

        public model.User Get(model.User user)
        {
            var builder = Builders<model.User>.Filter;
            var filter = builder.Eq("Username", user.Username);

            return context.Users.Find(filter).FirstOrDefault();
        }

        private void Add(model.User user)
        {
            context.Users.InsertOne(user);
        }

        private void Edit(model.User user)
        {
            var userOld = Get(user);
            var upd = Builders<model.User>.Update.Set(u => u.Email, user.Email)
                                                    .Set(u => u.Fullname, user.Fullname)
                                                    .Set(u => u.IsActive, user.IsActive)
                                                    .Set(u => u.Password, string.IsNullOrEmpty(user.Password) ? userOld.Password : user.Password)
                                                    .Set(u => u.Roles, user.Roles);

            context.Users.UpdateOne(f => f.Id == user.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public void Update(model.User user)
        {
            var isEdit = GetAll().Where(x => x.Username == user.Username)
                                    .Count() > 0;

            if (isEdit)
                Edit(user);
            else
                Add(user);
        }

        public void UpdateLastLogin(model.User user)
        {
            var upd = Builders<model.User>.Update.Set(u => u.LastLogin, DateTime.Now);

            context.Users.UpdateOne(f => f.Id == user.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public bool Validate(model.User user)
        {
            var builder = Builders<model.User>.Filter;
            var filter = builder.Eq("Username", user.Username) &
                            builder.Eq("Password", user.Password) &
                            builder.Eq("IsActive", true);

            return context.Users.Find(filter).CountDocuments() == 1;
        }

        public model.User Get(string id)
        {
            throw new NotImplementedException();
        }
    }
}
