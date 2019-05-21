﻿namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Linq;
    using sydtrucking_payroll_front.model;

    public class Truck : BusinessBase, IBusiness<model.Truck>
    {
        public Truck() : base() { }

        public List<model.Truck> GetAll()
        {
            return context.Trucks.Find(FilterDefinition<model.Truck>.Empty).ToList();
        }

        private void Add(model.Truck truck)
        {
            context.Trucks.InsertOne(truck);
        }

        private void Edit(model.Truck truck)
        {
            var upd = Builders<model.Truck>.Update.Set(u => u.Inspection, truck.Inspection)
                                                    .Set(u => u.Make, truck.Make)
                                                    .Set(u => u.Number, truck.Number)
                                                    .Set(u => u.Plate, truck.Plate)
                                                    .Set(u => u.Registration, truck.Registration)
                                                    .Set(u => u.Vin, truck.Vin)
                                                    .Set(u => u.Year, truck.Year);

            context.Trucks.UpdateOne(f => f.Id == truck.Id, upd, new UpdateOptions() { IsUpsert = false });
        }

        public void Update(model.Truck truck)
        {
            var isEdit = GetAll().Where(x => x.Number == truck.Number)
                                    .Count() > 0;

            if (isEdit)
                Edit(truck);
            else
                Add(truck);
        }

        public model.Truck Get(string id)
        {
            var builder = Builders<model.Truck>.Filter;
            var filter = builder.Eq(x => x.Id, id);

            return context.Trucks.Find(filter).FirstOrDefault();
        }
    }
}
