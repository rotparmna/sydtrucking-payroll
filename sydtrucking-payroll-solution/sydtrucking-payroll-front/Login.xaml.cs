namespace sydtrucking_payroll_front
{
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        data.PayrollContext _context;

        public Login()
        {
            _context = new data.PayrollContext(Properties.Settings.Default);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ValidateConnectionDB();
        }

        private void ValidateConnectionDB()
        {
            Enable(false);
            MessageConnection.Text = "Validating connection to database!";
            _context.ValidateConnectionAsync().ContinueWith((x) =>
            {
                if (x.IsCompleted && !x.IsFaulted && !x.IsCanceled)
                {
                    UpdateMessageConnection(x.Result ? 
                                                "Correct connection to database!" : 
                                                "Incorrect connection to database!");
                    Enable(x.Result);
                }

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UpdateMessageConnection(string text)
        {
            MessageConnection.Text = text;
        }

        private void Enable(bool isEnable)
        {
            Username.IsEnabled = isEnable;
            Password.IsEnabled = isEnable;
            LogIn.IsEnabled = isEnable;
        }
    }
}
