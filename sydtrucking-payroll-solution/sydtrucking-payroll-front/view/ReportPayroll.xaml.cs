namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.business;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.notification;
    using sydtrucking_payroll_front.print;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class ReportPayroll : Window
    {
        private IBusiness<model.Payroll> _payrollBusiness;
        private IBusiness<model.Driver> _driverBusiness;
        private List<PrintPayrollView> _printView;

        public ReportPayroll()
        {
            InitializeComponent();
            _payrollBusiness = new business.Payroll();
            _printView = new List<PrintPayrollView>();
            _driverBusiness = new business.Driver();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Drivers.SelectedValuePath = "Id";
            Drivers.DisplayMemberPath = "Fullname";
            Drivers.ItemsSource = _driverBusiness.GetAll();
        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            ICollection<model.Payroll> prints = new List<model.Payroll>();
            if (payroll.PrintRegularHoursApartOvertime)
                prints = payroll.Prints;
            else
                prints.Add(payroll);
            foreach (var item in prints)
            {
                PrintPayroll print = new PrintPayroll(item);
                print.Print(true);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchPayrolls();
        }

        private void SearchPayrolls()
        {
            _printView = ((IPayroll<PrintPayrollView, model.Driver>)_payrollBusiness).GetListPayroll(From.SelectedDate.Value.Date, To.SelectedDate.Value.Date, Drivers.SelectedItem as model.Driver);
            Details.ItemsSource = _printView;
            if (_printView.Count == 0)
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

            ((IEmail<model.Payroll>)_payrollBusiness).SendEmail(email, payroll);

            MessageBox.Show("Email sent!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure delete payroll?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var id = ((Button)sender).Tag.ToString();
                var payroll = _payrollBusiness.Get(id);

                payroll.IsDetele = true;
                _payrollBusiness.Update(payroll);

                SearchPayrolls();
            }
        }
    }
}
