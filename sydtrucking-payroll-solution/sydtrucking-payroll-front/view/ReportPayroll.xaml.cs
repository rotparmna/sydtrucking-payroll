namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.business;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.print;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class ReportPayroll : Window, IReportPayrollView
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
            
            PrintReport(payroll);
        }

        private static void PrintReport(model.Payroll payroll)
        {
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
            _printView.ForEach(x =>
            {
                var payroll = _payrollBusiness.Get(x.Id);
                PrintReport(payroll);
            });
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchPayrolls();
        }

        public void SearchPayrolls()
        {
            bool searchByTicket = !string.IsNullOrEmpty(TicketNumber.Text);
            if (searchByTicket)
            {
                _printView = ((IPayroll<PrintPayrollView, Ticket>)_payrollBusiness).GetListPayroll(null, null, new Ticket { Number = int.Parse(TicketNumber.Text) });
            }
            else
            {
                _printView = ((IPayroll<PrintPayrollView, model.Driver>)_payrollBusiness).GetListPayroll(From.SelectedDate, To.SelectedDate, Drivers.SelectedItem as model.Driver);
            }
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
            PayrollMail.Send(new PrintPayroll(payroll), (IEmail<model.Payroll>)_payrollBusiness, payroll);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeletePayroll(((Button)sender).Tag.ToString());
        }

        public void DeletePayroll(string id)
        {
            PayrollDelete.Delete(_payrollBusiness, id, this);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).Tag.ToString();

            Payroll payroll = new Payroll();
            payroll.Show();
            payroll.LoadPayroll(id);
        }
    }
}
