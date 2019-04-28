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

        public IMongoCollection<Employee> Employees
        {
            get
            {
                return _database.GetCollection<Employee>("Employees");
            }
        }

        public IMongoCollection<Payroll> Payrolls
        {
            get
            {
                return _database.GetCollection<Payroll>("Payrolls");
            }
        }

        public IMongoCollection<Role> Roles
        {
            get
            {
                return _database.GetCollection<Role>("Roles");
            }
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return _database.GetCollection<User>("Users");
            }
        }

        public IMongoCollection<Truck> Trucks
        {
            get
            {
                return _database.GetCollection<Truck>("Trucks");
            }
        }
    }
}
