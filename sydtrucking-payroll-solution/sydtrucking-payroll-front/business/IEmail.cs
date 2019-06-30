namespace sydtrucking_payroll_front.business
{
    using sydtrucking_payroll_front.notification;

    public interface IEmail<T>
    {
        void SendEmail(INotification notification, T payroll);
    }
}
