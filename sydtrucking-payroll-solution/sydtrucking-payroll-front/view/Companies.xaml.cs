namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Roles.xaml
    /// </summary>
    public partial class Companies : Window
    {
        private List<Company> _companiesModel;
        private business.IBusiness<Company> _companyBusiness;
        private string _idCompanySelected;

        public Companies()
        {
            InitializeComponent();
            _idCompanySelected = string.Empty;
            _companiesModel = new List<Company>();
            _companyBusiness = new business.Company();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Clear();
            LoadCompanies();
        }

        private void LoadCompanies()
        {
            _companiesModel = _companyBusiness.GetAll();
            ListCompanies.ItemsSource = _companiesModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Company role = new Company()
            {
                Id = _idCompanySelected,
                Name = Name.Text,
                Rate = double.Parse(Rate.Text.Replace("$", string.Empty))
            };

            _companyBusiness.Update(role);
            LoadCompanies();
        }

        private void ListCompanies_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(Company))
            {
                LoadCompany((Company)e.AddedItems[0]);
            }
        }

        private void LoadCompany(Company company)
        {
            _idCompanySelected = company.Id;
            Name.Text = company.Name;
            Rate.Text = company.Rate.ToString("C");
        }

        private void Clear()
        {
            _idCompanySelected = string.Empty;
            Name.Text = string.Empty;
            Rate.Text = string.Empty;
        }
    }
}
