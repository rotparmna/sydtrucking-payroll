namespace sydtrucking_payroll_front.business
{
    using System;
    using System.Collections.Generic;

    internal class Trash : BusinessBase, IBusiness<model.Trash>
    {
        public model.Trash Get(string id)
        {
            throw new NotImplementedException();
        }

        public List<model.Trash> GetAll()
        {
            throw new NotImplementedException();
        }

        private void Add(model.Trash trash)
        {
            context.Trash.InsertOne(trash);
        }

        public void Update(model.Trash model)
        {
            Add(model);
        }
    }
}
