namespace sydtrucking_payroll_front.data
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.Properties;

    public class PayrollContext
    {
        private readonly IMongoDatabase _database = null;

        public PayrollContext(Settings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Database);
        }

        public IMongoCollection<Employees> Locations
        {
            get
            {
                return _database.GetCollection<Employees>("Employees");
            }
        }
    }
}
