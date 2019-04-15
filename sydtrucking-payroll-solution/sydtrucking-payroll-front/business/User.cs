namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
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

        public void Update(model.User user)
        {
            var isEdit = GetAll().Where(x => x.SocialSecurity == user.SocialSecurity)
                                    .Count() > 0;

            //if (isEdit)
            //    //Edit(user);
            //else
                Add(user);
        }
    }
}
