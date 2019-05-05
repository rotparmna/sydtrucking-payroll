namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;
    using sydtrucking_payroll_front.print;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class ReportPayroll : Window
    {
        private business.Payroll _payrollBusiness;
        private business.Employee _employeeBusiness;
        private List<PrintPayrollView> _printView;

        public ReportPayroll()
        {
            InitializeComponent();
            _payrollBusiness = new business.Payroll();
            _printView = new List<PrintPayrollView>();
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
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            PrintPayroll p = new PrintPayroll(payroll);
            p.Print();
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            _printView = _payrollBusiness.GetListPayroll(From.SelectedDate.Value.Date, To.SelectedDate.Value.Date, Employees.SelectedItem as Employee);
            Details.ItemsSource = _printView;
            if ( _printView.Count == 0)
            {
                MessageBox.Show("There is no information", "No data", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
