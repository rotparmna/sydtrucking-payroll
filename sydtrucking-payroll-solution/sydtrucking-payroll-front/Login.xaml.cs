namespace sydtrucking_payroll_front
{
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void TruckButton_Click(object sender, RoutedEventArgs e)
        {
            view.Trucks truck = new view.Trucks();
            truck.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text=="admin" && Password.Password == "admin")
            {
                MainWindow main = new MainWindow();
                main.Show();

                Close();
            }
        }
    }
}
