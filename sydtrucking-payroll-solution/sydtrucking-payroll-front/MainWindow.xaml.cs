namespace sydtrucking_payroll_front
{
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
            Welcome.Content = "Welcome, " + Ticket.Instance.Authenticate.Fullname;
        }
    }
}
