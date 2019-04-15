namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;
    using sydtrucking_payroll_front.enums;
    using System.Windows.Controls;
    using System;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class Users : Window
    {
        private List<User> _usersModel;
        private business.User _userBusiness;
        private business.Role _roleBusiness;
        private List<RoleDetailView> _roles;
        private string _idUserSelected;

        public Users()
        {
            InitializeComponent();
            _usersModel = new List<User>();
            _roles = new List<RoleDetailView>();
            _userBusiness = new business.User();
            _roleBusiness = new business.Role();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Clear();
            LoadUsers();
        }

        private void LoadUsers()
        {
            _usersModel = _userBusiness.GetAll();
            ListUsers.ItemsSource = _usersModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var id = _userBusiness.GetAll()
                          .Where(x => x.SocialSecurity == long.Parse(SocialSecurity.Text))
                          .DefaultIfEmpty(new User() { Id = string.Empty })
                          .FirstOrDefault()
                          .Id;

            User user = new User()
            {
                Id = id,
                SocialSecurity = long.Parse(SocialSecurity.Text),
                Email = Email.Text,
                Fullname = Fullname.Text,
                IsActive = IsActive.IsChecked.Value,
                Password = Passsword.Text
            };

            _roles.Where(x => x.IsActive)
                    .ToList()
                    .ForEach(x => user.Roles.Add(new Role()
                                {
                                    Id = x.Id,
                                    Name = x.Role
                                }));

            _userBusiness.Update(user);
            LoadUsers();
        }

        private void ListUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(User))
            {
                LoadUsers((User)e.AddedItems[0]);
            }
        }

        private void LoadUsers(User user)
        {
            _idUserSelected = user.Id;
            SocialSecurity.Text = user.SocialSecurity.ToString();
            LastLogin.SelectedDate = user.LastLogin;
            Fullname.Text = user.Fullname;
            Email.Text = user.Email;
            IsActive.IsChecked = user.IsActive;

            List<Role> roles = _roleBusiness.GetAll();
            roles.ForEach(x =>
            {
                _roles.Add(new RoleDetailView()
                {
                    Id = x.Id,
                    Role = x.Name,
                    IsActive = user.Roles.Where(y => y.Id == x.Id).Count() > 0
                });
            });

            Roles.ItemsSource = _roles;
        }

        private void Clear()
        {
            SocialSecurity.Text = string.Empty;
            Fullname.Text = string.Empty;
            Email.Text = string.Empty;
            LastLogin.SelectedDate = null;
            IsActive.IsChecked = false;
            Roles.ItemsSource = null;
        }
    }
}
