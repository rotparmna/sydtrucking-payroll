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
    public partial class ReportPayrollLeaseCompanies : Window
    {
        private IBusiness<model.PayrollLeaseCompany> _payrollLeaseCompanyBusiness;
        private IBusiness<model.LeaseCompany> _leaseCompanyBusiness;
        private List<PrintPayrollLeaseCompanyView> _printView;

        public ReportPayrollLeaseCompanies()
        {
            InitializeComponent();
            _payrollLeaseCompanyBusiness = new business.PayrollLeaseCompany();
            _printView = new List<PrintPayrollLeaseCompanyView>();
            _leaseCompanyBusiness = new business.LeaseCompany();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LeaseCompanies.SelectedValuePath = "Id";
            LeaseCompanies.DisplayMemberPath = "Name";
            LeaseCompanies.ItemsSource = _leaseCompanyBusiness.GetAll();
        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            //var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            //var payroll = _payrollLeaseCompanyBusiness.Get(rowData.Id);

            //PrintPayroll print = new PrintPayroll(payroll);
            //print.Print(true);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            _printView = ((business.PayrollLeaseCompany)_payrollLeaseCompanyBusiness).GetListPayroll(From.SelectedDate.Value.Date, To.SelectedDate.Value.Date, LeaseCompanies.SelectedItem as model.LeaseCompany);
            Details.ItemsSource = _printView;
            if ( _printView.Count == 0)
            {
                MessageBox.Show("There is no information", "No data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //_printView.ForEach(x =>
            //{
            //    var payroll = _payrollLeaseCompanyBusiness.Get(x.Id);
            //    SendEmail(payroll);
            //});
        }

        private void EmailReport_Click(object sender, RoutedEventArgs e)
        {
            //var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            //var payroll = _payrollLeaseCompanyBusiness.Get(rowData.Id);

            //SendEmail(payroll);
        }

        private void SendEmail(model.Payroll payroll)
        {
            //PrintPayroll print = new PrintPayroll(payroll);
            //print.Print(false);

            //INotification email = new Email("Pay Stub");
            //((Email)email).File = new Attachment(File.Open(print.Fullname, FileMode.Open), print.Filename);

            //((business.Payroll)_payrollLeaseCompanyBusiness).SendEmail(email, payroll);

            //MessageBox.Show("Email sent!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
