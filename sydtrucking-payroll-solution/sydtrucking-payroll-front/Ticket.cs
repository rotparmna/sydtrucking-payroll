namespace sydtrucking_payroll_front
{
    using sydtrucking_payroll_front.business;

    public sealed class Ticket
    {
        public Authenticate Authenticate { get; set; }

        private readonly static Ticket _instance = new Ticket();

        private Ticket()
        {
        }

        public static Ticket Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
