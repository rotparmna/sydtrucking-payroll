namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Lógica de interacción para Roles.xaml
    /// </summary>
    public partial class OilCompanies : Window, IView<model.OilCompany>, IValidation
    {
        private List<model.OilCompany> _companiesModel;
        private business.IBusiness<model.OilCompany> _companyBusiness;
        private string _idCompanySelected;

        public string ValidationMessage { get; set; }

        public OilCompanies()
        {
            InitializeComponent();
            _idCompanySelected = string.Empty;
            _companiesModel = new List<model.OilCompany>();
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
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(model.OilCompany))
            {
                LoadDataBySelectedRow((model.OilCompany)e.AddedItems[0]);
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

                model.OilCompany role = new model.OilCompany()
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

        public void LoadDataBySelectedRow(model.OilCompany company)
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
            var validateName = ValidateIfNameExists();

            if (string.IsNullOrEmpty(Name.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Name");
            if (string.IsNullOrEmpty(Rate.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Rate");
            if (!string.IsNullOrEmpty(validateName)) ValidationMessage += validateName;

            return string.IsNullOrEmpty(ValidationMessage);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).Tag.ToString();
            var company = _companyBusiness.Get(id);
            DeleteView(company);
        }

        public void DeleteView(model.OilCompany data)
        {
            if (MessageBox.Show("Are you sure delete oil company?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                data.IsDetele = true;
                _companyBusiness.Update(data);

                FillGrid();
            }
        }

        private void Name_LostFocus(object sender, RoutedEventArgs e)
        {
            var validate = ValidateIfNameExists();

            if (!string.IsNullOrEmpty(validate))
            {
                MessageBox.Show(validate, "Validations", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string ValidateIfNameExists()
        {
            var count = ((business.OilCompany)_companyBusiness).CountWithoutCurrent(Name.Text, _idCompanySelected);
            var message = string.Empty;

            if (count > 0)
                message = "The written name is already registered";

            return message;
        }
    }
}
