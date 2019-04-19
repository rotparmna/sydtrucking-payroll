﻿namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class User
    {
        private PayrollContext context;

        public User()
        {
            context = new PayrollContext(Properties.Settings.Default);
        }

        public List<model.User> GetAll()
        {
            return context.Users.Find(FilterDefinition<model.User>.Empty).ToList();
        }

        private void Add(model.User user)
        {
            context.Users.InsertOne(user);
        }

        private void Edit(model.User user)
        {
            var upd = Builders<model.User>.Update.Set(u => u.Email, user.Email)
                                                    .Set(u => u.Fullname, user.Fullname)
                                                    .Set(u => u.IsActive, user.IsActive)
                                                    .Set(u => u.Password, user.Password)
                                                    .Set(u=>u.Roles, user.Roles);

            context.Users.UpdateOne(f => f.Id == user.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public void Update(model.User user)
        {
            var isEdit = GetAll().Where(x => x.SocialSecurity == user.SocialSecurity)
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
    }
}