namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.business;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.notification;
    using sydtrucking_payroll_front.print;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mail;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class ReportPayroll : Window
    {
        private IBusiness<model.Payroll> _payrollBusiness;
        private IBusiness<model.Driver> _employeeBusiness;
        private List<PrintPayrollView> _printView;

        public ReportPayroll()
        {
            InitializeComponent();
            _payrollBusiness = new business.Payroll();
            _printView = new List<PrintPayrollView>();
            _employeeBusiness = new business.Driver();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employees.SelectedValuePath = "Id";
            Employees.DisplayMemberPath = "Fullname";
            Employees.ItemsSource = _employeeBusiness.GetAll();
        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            PrintPayroll print = new PrintPayroll(payroll);
            print.Print(true);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            _printView = ((business.Payroll)_payrollBusiness).GetListPayroll(From.SelectedDate.Value.Date, To.SelectedDate.Value.Date, Employees.SelectedItem as model.Driver);
            Details.ItemsSource = _printView;
            if ( _printView.Count == 0)
            {
                MessageBox.Show("There is no information", "No data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            _printView.ForEach(x =>
            {
                var payroll = _payrollBusiness.Get(x.Id);
                SendEmail(payroll);
            });
        }

        private void EmailReport_Click(object sender, RoutedEventArgs e)
        {
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            SendEmail(payroll);
        }

        private void SendEmail(model.Payroll payroll)
        {
            PrintPayroll print = new PrintPayroll(payroll);
            print.Print(false);

            INotification email = new Email("Pay Stub");
            ((Email)email).File = new Attachment(File.Open(print.Fullname, FileMode.Open), print.Filename);

            ((business.Payroll)_payrollBusiness).SendEmail(email, payroll);

            MessageBox.Show("Email sent!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
