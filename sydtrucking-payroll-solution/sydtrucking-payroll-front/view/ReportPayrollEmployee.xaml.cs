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
    public partial class ReportPayrollEmployee : Window, IReportPayrollView
    {
        private IBusiness<model.PayrollEmployee> _payrollBusiness;
        private IBusiness<model.Employee> _employeeBusiness;
        private List<PrintPayrollEmployeeView> _printView;

        public ReportPayrollEmployee()
        {
            InitializeComponent();
            _payrollBusiness = new business.PayrollEmployee();
            _printView = new List<PrintPayrollEmployeeView>();
            _employeeBusiness = new business.Employee();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employees.SelectedValuePath = "Id";
            Employees.DisplayMemberPath = "Fullname";
            Employees.ItemsSource = _employeeBusiness.GetAll();
        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollEmployeeView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            PrintPayrollEmployee print = new PrintPayrollEmployee(payroll);
            print.Print(true);
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchPayrolls();
        }

        public void SearchPayrolls()
        {
            _printView = ((IPayroll<PrintPayrollEmployeeView, model.Employee>)_payrollBusiness).GetListPayroll(From.SelectedDate.Value.Date, To.SelectedDate.Value.Date, Employees.SelectedItem as model.Employee);
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
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollEmployeeView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            SendEmail(payroll);
        }

        private void SendEmail(model.PayrollEmployee payroll)
        {
            PayrollMail.Send(new PrintPayrollEmployee(payroll), (IEmail<model.PayrollEmployee>)_payrollBusiness, payroll);
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

            PayrollEmployee payroll = new PayrollEmployee();
            payroll.Show();
            payroll.LoadPayroll(id);

            Close();
        }
    }
}
