namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.business;
    using sydtrucking_payroll_front.notification;
    using sydtrucking_payroll_front.print;
    using System.IO;
    using System.Net.Mail;
    using System.Windows;

    public static class PayrollMail
    {
        public static void Send<T>(PrintPayrollBase printPayrollBase, IEmail<T> email, T payroll)
        {
            bool error = false;
            printPayrollBase.Print(false);

            INotification notification = new Email("Pay Stub");
            ((Email)notification).File = new Attachment(File.Open(printPayrollBase.Fullname, FileMode.Open), printPayrollBase.Filename);

            try
            {
                email.SendEmail(notification, payroll);
            }
            catch
            {
                error = true;
            }

            if (error)
                MessageBox.Show("Email not sent!", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
            else
                MessageBox.Show("Email sent!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
