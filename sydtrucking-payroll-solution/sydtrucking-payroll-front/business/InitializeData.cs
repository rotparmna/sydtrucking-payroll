﻿namespace sydtrucking_payroll_front.business
{
    using MongoDB.Driver;
    using sydtrucking_payroll_front.data;
    using sydtrucking_payroll_front.view;
    using System.Linq;
    using System.Threading.Tasks;

    public class InitializeData
    {
        PayrollContext _context;
        IUpdateUI _updateMessage;

        public InitializeData(IUpdateUI updateMessage)
        {
            _updateMessage = updateMessage;
            _context = new PayrollContext(Properties.Settings.Default);
        }

        public async Task DoAsync()
        {
            _updateMessage.Enable(false);
            _updateMessage.UpdateMessageConnection("Initiating basic data.");
            if (_context.CountCollections == 0)
            {
                await _context.Roles.InsertOneAsync(new model.Role()
                {
                    Name = "Administrador"
                });

                await _context.Roles.InsertOneAsync(new model.Role()
                {
                    Name = "Basico"
                });

                model.User admin = new model.User()
                {
                    Username = "admin",
                    Password = "admin",
                    IsActive = true,
                    Fullname = "admin",
                };
                admin.Roles.AddRange(_context.Roles.Find(FilterDefinition<model.Role>.Empty).ToList());
                await _context.Users.InsertOneAsync(admin);

                _updateMessage.UpdateMessageConnection("Data started.");
            }
            else
                _updateMessage.UpdateMessageConnection(string.Empty);

            _updateMessage.Enable(true);
        }
    }
}