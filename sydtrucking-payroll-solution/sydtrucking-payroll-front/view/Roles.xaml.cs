namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Roles.xaml
    /// </summary>
    public partial class Roles : Window
    {
        private List<Role> _rolesModel;
        private business.Role _roleBusiness;
        private string _idRoleSelected;

        public Roles()
        {
            InitializeComponent();
            _idRoleSelected = string.Empty;
            _rolesModel = new List<Role>();
            _roleBusiness = new business.Role();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Clear();
            LoadRoles();
        }

        private void LoadRoles()
        {
            _rolesModel = _roleBusiness.GetAll();
            ListRoles.ItemsSource = _rolesModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Role role = new Role()
            {
                Id = _idRoleSelected,
                Name = Name.Text
            };

            _roleBusiness.Update(role);
            LoadRoles();
        }

        private void ListRoles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(Role))
            {
                LoadEmployee((Role)e.AddedItems[0]);
            }
        }

        private void LoadEmployee(Role role)
        {
            _idRoleSelected = role.Id;
            Name.Text = role.Name;
        }

        private void Clear()
        {
            _idRoleSelected = string.Empty;
            Name.Text = string.Empty;
        }
    }
}
