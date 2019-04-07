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
    }
}
