namespace sydtrucking_payroll_front.data
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.Properties;
    using System.Threading.Tasks;
    using System.Linq;

    public class PayrollContext
    {
        private readonly IMongoDatabase _database = null;

        public int CountCollections
        {
            get
            {
                return _database.ListCollections().ToList().Count();
            }
        }

        public PayrollContext(Settings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(settings.Database);
                //InitData();
            }
        }

        public async Task<bool> ValidateConnectionAsync()
        {
            bool isOnline = true;
            try
            {
                await _database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            }
            catch
            {
                isOnline = false;
            }
            return isOnline;
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

        public IMongoCollection<Company> Companies
        {
            get
            {
                return _database.GetCollection<Company>("Companies");
            }
        }

        public IMongoCollection<Configuration> Configurations
        {
            get
            {
                return _database.GetCollection<Configuration>("Configurations");
            }
        }
    }
}
