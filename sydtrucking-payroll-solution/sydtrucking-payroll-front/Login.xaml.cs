namespace sydtrucking_payroll_front
{
    using sydtrucking_payroll_front.business;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        data.PayrollContext _context;
        Authenticate _authenticate;

        public Login()
        {
            _context = new data.PayrollContext(Properties.Settings.Default);
            _authenticate = new Authenticate();
            InitializeComponent();
        }

        private void TruckButton_Click(object sender, RoutedEventArgs e)
        {
            view.Trucks truck = new view.Trucks();
            truck.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _authenticate.Login(Username.Text, Password.Password);
            if (_authenticate.IsAuthenticate)
            {
                Ticket.Instance.Authenticate = _authenticate;

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
