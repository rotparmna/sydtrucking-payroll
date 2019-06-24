namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class Users : Window, IView<User>, IValidation
    {
        private List<User> _usersModel;
        private business.IBusiness<User> _userBusiness;
        private business.IBusiness<Role> _roleBusiness;
        private List<RoleDetailView> _rolesView;
        private string _idUserSelected;

        public string ValidationMessage { get; set; }

        public Users()
        {
            InitializeComponent();
            _usersModel = new List<User>();
            _rolesView = new List<RoleDetailView>();
            _userBusiness = new business.User();
            _roleBusiness = new business.Role();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClearView();
            FillGrid();
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveView();
        }

        private void ListUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(User))
            {
                LoadDataBySelectedRow((User)e.AddedItems[0]);
            }
        }

        
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditView();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            CreateView();
        }

        public void ClearView()
        {
            Username.Text = string.Empty;
            Fullname.Text = string.Empty;
            Email.Text = string.Empty;
            LastLogin.SelectedDate = null;
            IsActive.IsChecked = false;
            Passsword.Text = string.Empty;
            Roles.ItemsSource = null;
        }

        public void CreateView()
        {
            ChangeControlsEnabled(true);
            ClearView();
        }

        public void SaveView()
        {
            if (IsViewValid())
            {
                ChangeControlsEnabled(false);

                var id = _userBusiness.GetAll()
                              .Where(x => x.Username == Username.Text)
                              .DefaultIfEmpty(new User() { Id = string.Empty })
                              .FirstOrDefault()
                              .Id;

                User user = new User()
                {
                    Id = id,
                    Username = Username.Text,
                    Email = Email.Text,
                    Fullname = Fullname.Text,
                    IsActive = IsActive.IsChecked.Value,
                    Password = Passsword.Text
                };

                _rolesView.Where(x => x.IsActive)
                        .ToList()
                        .ForEach(x => user.Roles.Add(new Role()
                        {
                            Id = x.Id,
                            Name = x.Role
                        }));

                _userBusiness.Update(user);
                MessageBox.Show("The information was correctly saved!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearView();
                FillGrid();
            }
            else
            {
                MessageBox.Show(ValidationMessage, "Validations", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void FillGrid()
        {
            _usersModel = _userBusiness.GetAll();
            ListUsers.ItemsSource = _usersModel;
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void LoadDataBySelectedRow(User user)
        {
            _idUserSelected = user.Id;
            Username.Text = user.Username;
            LastLogin.SelectedDate = user.LastLogin;
            Fullname.Text = user.Fullname;
            Email.Text = user.Email;
            IsActive.IsChecked = user.IsActive;

            List<Role> roles = _roleBusiness.GetAll();
            _rolesView.Clear();
            Roles.ItemsSource = null;

            roles.ForEach(x =>
            {
                _rolesView.Add(new RoleDetailView()
                {
                    Id = x.Id,
                    Role = x.Name,
                    IsActive = user.Roles.Where(y => y.Id == x.Id).Count() > 0
                });
            });

            Roles.ItemsSource = _rolesView;
        }

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            RolesGroup.IsEnabled = isEnable;
            Save.IsEnabled = isEnable;
            New.IsEnabled = !isEnable;
        }

        public bool IsViewValid()
        {
            ValidationMessage = string.Empty;

            if (string.IsNullOrEmpty(Username.Text)) ValidationMessage += "The Username field is required. \n";
            if (string.IsNullOrEmpty(Fullname.Text)) ValidationMessage += "The Fullname field is required. \n";
            if (string.IsNullOrEmpty(Email.Text)) ValidationMessage += "The Email field is required. \n";
            if (string.IsNullOrEmpty(Passsword.Text)) ValidationMessage += "The Password field is required. \n";

            return string.IsNullOrEmpty(ValidationMessage);
        }
    }
}
