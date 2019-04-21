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
            var rowData = ((FrameworkElement)sender).DataContext as PrintPayrollView;
            var payroll = _payrollBusiness.Get(rowData.Id);

            //TODO print
            PrintPayroll p = new PrintPayroll(payroll);
            p.Print();
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
