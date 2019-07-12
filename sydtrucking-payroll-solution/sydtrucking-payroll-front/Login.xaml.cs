namespace sydtrucking_payroll_front
{
    using sydtrucking_payroll_front.business;
    using System.Windows;
    using sydtrucking_payroll_front.view;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window, IUpdateUI
    {
        Ping _ping;
        InitializeData _initializeData;
        Authenticate _authenticate;

        public Login()
        {
            _ping = new Ping(this);
            _authenticate = new Authenticate();
            _initializeData = new InitializeData(this);
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
                App.Log.Info("User was authenticated in the application. Username: " + Username.Text + ". Datetime: " + DateTime.Now);
                
                Ticket.Instance.Authenticate = _authenticate;

                MainWindow main = new MainWindow();
                main.Show();

                Close();
            }
            else
                UpdateMessageConnection("Login failed!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ping.SendAsync().ContinueWith(async (x) =>
            {
                if (x.IsCompleted && !x.IsFaulted && !x.IsCanceled && x.Result)
                {
                    await _initializeData.DoAsync();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void UpdateMessageConnection(string text)
        {
            MessageConnection.Text = text;
        }

        public void Enable(bool isEnable)
        {
            Username.IsEnabled = isEnable;
            Password.IsEnabled = isEnable;
            LogIn.IsEnabled = isEnable;
        }
    }
}
