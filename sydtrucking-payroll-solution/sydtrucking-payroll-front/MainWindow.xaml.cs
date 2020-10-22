namespace sydtrucking_payroll_front
{
    using sydtrucking_payroll_front.Properties;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            view.Employees employees = new view.Employees();
            employees.Show();
        }

        private void PayrollButton_Click(object sender, RoutedEventArgs e)
        {
            view.Payroll payrolls = new view.Payroll();
            payrolls.Show();
        }

        private void RoleButton_Click(object sender, RoutedEventArgs e)
        {
            view.Roles roles = new view.Roles();
            roles.Show();
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            view.Users users = new view.Users();
            users.Show();
        }

        private void ReportPayroll_Click(object sender, RoutedEventArgs e)
        {
            view.ReportPayroll report = new view.ReportPayroll();
            report.Show();
        }

        private void TruckButton_Click(object sender, RoutedEventArgs e)
        {
            view.Trucks truck = new view.Trucks();
            truck.Show();
        }

        private void OilCompanies_Click(object sender, RoutedEventArgs e)
        {
            view.OilCompanies company = new view.OilCompanies();
            company.Show();
        }

        private void LeaseCompanies_Click(object sender, RoutedEventArgs e)
        {
            view.LeaseCompanies company = new view.LeaseCompanies();
            company.Show();
        }

        private void PayrollLeaseCompany_Click(object sender, RoutedEventArgs e)
        {
            view.PayrollLeaseCompanies payrollLeaseCompanies = new view.PayrollLeaseCompanies();
            payrollLeaseCompanies.Show();
        }

        private void ReportLeaseCompanyPayroll_Click(object sender, RoutedEventArgs e)
        {
            view.ReportPayrollLeaseCompanies reportPayrollLeaseCompanies = new view.ReportPayrollLeaseCompanies();
            reportPayrollLeaseCompanies.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Welcome.Content = "Welcome, " + Ticket.Instance.Authenticate.Fullname + ". V " + Settings.Default.Version;
            this.Title += ". V " + Settings.Default.Version;

            Security.IsEnabled = Ticket.Instance.Authenticate.IsInRole(enums.Role.Administrator);
            Param.IsEnabled = Ticket.Instance.Authenticate.IsInRole(enums.Role.Basic1) ||
                               Ticket.Instance.Authenticate.IsInRole(enums.Role.Administrator);
            Business.IsEnabled = Ticket.Instance.Authenticate.IsInRole(enums.Role.Basic2) ||
                                    Ticket.Instance.Authenticate.IsInRole(enums.Role.Administrator);
            Reports.IsEnabled = Ticket.Instance.Authenticate.IsInRole(enums.Role.Basic2) ||
                                    Ticket.Instance.Authenticate.IsInRole(enums.Role.Administrator);
        }

        private void DriverButton_Click(object sender, RoutedEventArgs e)
        {
            view.Drivers drivers = new view.Drivers();
            drivers.Show();
        }

        private void PayrollEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            view.PayrollEmployee payrollEmployee = new view.PayrollEmployee();
            payrollEmployee.Show();
        }

        private void ReportPayrollEmployee_Click(object sender, RoutedEventArgs e)
        {
            view.ReportPayrollEmployee payrollEmployee = new view.ReportPayrollEmployee();
            payrollEmployee.Show();
        }

        private void ReportsIcon_Click(object sender, RoutedEventArgs e)
        {
            view.Reports reports = new view.Reports();
            reports.Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Application.Current.Windows.Count>2)
            {
                MessageBox.Show("There are open windows. Close the windows first to be able to exit the application.", string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
            }
        }
    }
}
