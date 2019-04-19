namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class ReportPayroll : Window
    {
        private business.Payroll _payrollBusiness;
        private List<PrintPayrollView> _printView;

        public ReportPayroll()
        {
            InitializeComponent();
            _payrollBusiness = new business.Payroll();
            _printView = new List<PrintPayrollView>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            _printView = _payrollBusiness.GetListPayroll(From.SelectedDate.Value.Date, To.SelectedDate.Value.Date);
            Details.ItemsSource = _printView;
        }
    }
}
