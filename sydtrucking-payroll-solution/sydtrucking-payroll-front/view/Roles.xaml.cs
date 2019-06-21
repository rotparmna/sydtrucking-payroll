namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Roles.xaml
    /// </summary>
    public partial class Roles : Window, IView<Role>
    {
        private List<Role> _rolesModel;
        private business.IBusiness<Role> _roleBusiness;
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
            ClearView();
            FillGrid();
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveView();
        }

        private void ListRoles_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(Role))
            {
                LoadDataBySelectedRow((Role)e.AddedItems[0]);
            }
        }
                
        private void New_Click(object sender, RoutedEventArgs e)
        {
            CreateView();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditView();
        }

        public void ClearView()
        {
            _idRoleSelected = string.Empty;
            Name.Text = string.Empty;
        }

        public void CreateView()
        {
            ChangeControlsEnabled(true);
            ClearView();
        }

        public void SaveView()
        {
            ChangeControlsEnabled(false);

            Role role = new Role()
            {
                Id = _idRoleSelected,
                Name = Name.Text
            };

            _roleBusiness.Update(role);
            MessageBox.Show("The information was correctly saved!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearView();
            FillGrid();
        }

        public void FillGrid()
        {
            _rolesModel = _roleBusiness.GetAll();
            ListRoles.ItemsSource = _rolesModel;
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void LoadDataBySelectedRow(Role role)
        {
            _idRoleSelected = role.Id;
            Name.Text = role.Name;
        }

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            Save.IsEnabled = isEnable;
            New.IsEnabled = !isEnable;
        }
    }
}
