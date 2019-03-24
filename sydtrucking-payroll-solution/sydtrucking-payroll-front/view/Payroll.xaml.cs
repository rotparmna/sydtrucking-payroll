namespace sydtrucking_payroll_front.view
{
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Payroll.xaml
    /// </summary>
    public partial class Payroll : Window
    {
        business.Employee employeeBusiness;

        public Payroll()
        {
            InitializeComponent();
            employeeBusiness = new business.Employee();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employees.DataContext = employeeBusiness.GetAll();
        }
    }
}
