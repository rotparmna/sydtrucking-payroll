namespace sydtrucking_payroll_front.business
{
    using sydtrucking_payroll_front.data;
    using sydtrucking_payroll_front.view;
    using System.Threading.Tasks;

    public class Ping
    {
        PayrollContext _context;
        IUpdateUI _updateMessage;

        public Ping(IUpdateUI updateMessage)
        {
            _updateMessage = updateMessage;
            _context = new PayrollContext(Properties.Settings.Default);
        }

        public async Task<bool> SendAsync()
        {
            bool ok = false;
            _updateMessage.Enable(false);
            _updateMessage.UpdateMessageConnection("Validating connection to database!");

            await _context.ValidateConnectionAsync().ContinueWith((x) =>
            {
                if (x.IsCompleted && !x.IsFaulted && !x.IsCanceled)
                {
                    ok = x.Result;
                    _updateMessage.UpdateMessageConnection(x.Result ?
                                                "Correct connection to database!" :
                                                "Incorrect connection to database!");
                    _updateMessage.Enable(x.Result);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());

            return ok;
        }
    }
}
