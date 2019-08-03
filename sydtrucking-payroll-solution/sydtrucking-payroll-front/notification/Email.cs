namespace sydtrucking_payroll_front.notification
{
    using sydtrucking_payroll_front.business;
    using System;
    using System.Net;
    using System.Net.Mail;

    public class Email : INotification
    {
        private SmtpClient _stmp;
        private string _from;

        public string To { get; set; }
        public string Subject { get; set; }
        public Attachment File { get; set; }

        public Email(string subject)
        {
            _stmp = new SmtpClient(Constant.Smtp.Server, Constant.Smtp.Port);
            _from = Constant.Smtp.Email;
            Subject = subject;
        }

        public void Send(string body)
        {
            _stmp.Credentials = new NetworkCredential(Constant.Smtp.Email, Constant.Smtp.Password);
            _stmp.EnableSsl = Constant.Smtp.EnableSsl;
            MailMessage message = null;

            try
            {
                message = new MailMessage(_from, To, Subject, body);
            }
            catch (ArgumentException ex)
            {
                App.Log.Error("Fatal error!. Datetime: " + DateTime.Now + ". Exception: " + ex);
                throw;
            }
            
            message.Attachments.Add(File);

            try
            {
                _stmp.Send(message);
            }
            catch (SmtpException ex)
            {
                App.Log.Error("Fatal error!. Datetime: " + DateTime.Now + ". Exception: " + ex);
                throw;
            }
        }
    }
}
