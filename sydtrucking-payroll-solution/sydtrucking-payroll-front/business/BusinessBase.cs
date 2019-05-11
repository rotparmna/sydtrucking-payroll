namespace sydtrucking_payroll_front.business
{
    using sydtrucking_payroll_front.data;

    public abstract class BusinessBase
    {
        protected PayrollContext context;

        public BusinessBase()
        {
            context = new PayrollContext(Properties.Settings.Default);
        }
    }
}
