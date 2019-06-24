namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Roles.xaml
    /// </summary>
    public partial class OilCompanies : Window, IView<OilCompany>, IValidation
    {
        private List<OilCompany> _companiesModel;
        private business.IBusiness<OilCompany> _companyBusiness;
        private string _idCompanySelected;

        public string ValidationMessage { get; set; }

        public OilCompanies()
        {
            InitializeComponent();
            _idCompanySelected = string.Empty;
            _companiesModel = new List<OilCompany>();
            _companyBusiness = new business.OilCompany();
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

        private void ListCompanies_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(OilCompany))
            {
                LoadDataBySelectedRow((OilCompany)e.AddedItems[0]);
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
            _idCompanySelected = string.Empty;
            Name.Text = string.Empty;
            Rate.Text = string.Empty;
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

                OilCompany role = new OilCompany()
                {
                    Id = _idCompanySelected,
                    Name = Name.Text,
                    Rate = double.Parse(Rate.Text.Replace("$", string.Empty))
                };

                _companyBusiness.Update(role);
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
            _companiesModel = _companyBusiness.GetAll();
            ListCompanies.ItemsSource = _companiesModel;
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void LoadDataBySelectedRow(OilCompany company)
        {
            _idCompanySelected = company.Id;
            Name.Text = company.Name;
            Rate.Text = company.Rate.ToString("C");
        }

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            Save.IsEnabled = isEnable;
            New.IsEnabled = !isEnable;
        }

        public bool IsViewValid()
        {
            ValidationMessage = string.Empty;

            if (string.IsNullOrEmpty(Name.Text)) ValidationMessage += "The Name field is required. \n";
            if (string.IsNullOrEmpty(Rate.Text)) ValidationMessage += "The Rate field is required. \n";

            return string.IsNullOrEmpty(ValidationMessage);
        }
    }
}
