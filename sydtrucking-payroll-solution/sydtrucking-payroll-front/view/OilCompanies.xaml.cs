namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Roles.xaml
    /// </summary>
    public partial class OilCompanies : Window
    {
        private List<OilCompany> _companiesModel;
        private business.IBusiness<OilCompany> _companyBusiness;
        private string _idCompanySelected;

        public OilCompanies()
        {
            InitializeComponent();
            _idCompanySelected = string.Empty;
            _companiesModel = new List<OilCompany>();
            _companyBusiness = new business.OilCompany();
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
            OilCompany role = new OilCompany()
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
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(OilCompany))
            {
                LoadCompany((OilCompany)e.AddedItems[0]);
            }
        }

        private void LoadCompany(OilCompany company)
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
