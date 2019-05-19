namespace sydtrucking_payroll_front.notification
{
    public interface INotification
    {
        string To { get; set; }
        void Send(string body);
    }
}
