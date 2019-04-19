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
    }
}
