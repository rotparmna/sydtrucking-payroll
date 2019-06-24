namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para LeaseCompanies.xaml
    /// </summary>
    public partial class LeaseCompanies : Window, IView<LeaseCompany>, IValidation
    {
        private List<LeaseCompany> _companiesModel;
        private List<TruckDetailView> _trucksView;
        private business.IBusiness<LeaseCompany> _companyBusiness;
        private business.IBusiness<Truck> _truckBusiness;
        private string _idCompanySelected;

        public string ValidationMessage { get; set; }

        public LeaseCompanies()
        {
            InitializeComponent();
            _idCompanySelected = string.Empty;
            _companiesModel = new List<LeaseCompany>();
            _trucksView = new List<TruckDetailView>();
            _companyBusiness = new business.LeaseCompany();
            _truckBusiness = new business.Truck();
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
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(LeaseCompany))
            {
                LoadDataBySelectedRow((LeaseCompany)e.AddedItems[0]);
            }
        }

        public void ClearView()
        {
            General.IsEnabled = false;
            TrucksGroup.IsEnabled = false;
            Save.IsEnabled = false;
            Name.Text = string.Empty;
            Trucks.ItemsSource = null;
        }

        public void CreateView()
        {
            General.IsEnabled = true;
            TrucksGroup.IsEnabled = true;
            Save.IsEnabled = true;
        }

        public void SaveView()
        {
            if (IsViewValid())
            {
                ChangeControlsEnabled(false);

                LeaseCompany company = new LeaseCompany()
                {
                    Id = _idCompanySelected,
                    Name = Name.Text
                };

                _trucksView.Where(x => x.IsActive)
                       .ToList()
                       .ForEach(x => company.Trucks.Add(_truckBusiness.Get(x.Id)));

                _companyBusiness.Update(company);
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

            List<Truck> trucks = _truckBusiness.GetAll();
            _trucksView.Clear();
            Trucks.ItemsSource = null;

            trucks.ForEach(x =>
            {
                _trucksView.Add(new TruckDetailView()
                {
                    Id = x.Id,
                    IsActive = false,
                    Truck = x.Number
                });
            });

            Trucks.ItemsSource = _trucksView;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            ClearView();
            CreateView();
            FillGrid();
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            TrucksGroup.IsEnabled = isEnable;
            Save.IsEnabled = isEnable;
            New.IsEnabled = !isEnable;
        }

        public void LoadDataBySelectedRow(LeaseCompany company)
        {
            _idCompanySelected = company.Id;
            Name.Text = company.Name;

            List<Truck> trucks = _truckBusiness.GetAll();
            _trucksView.Clear();
            Trucks.ItemsSource = null;

            trucks.ForEach(x =>
            {
                _trucksView.Add(new TruckDetailView()
                {
                    Id = x.Id,
                    IsActive = company.Trucks.Where(y => y.Id == x.Id).Count() > 0,
                    Truck = x.Number
                });
            });

            Trucks.ItemsSource = _trucksView;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditView();
        }

        public bool IsViewValid()
        {
            ValidationMessage = string.Empty;

            if (string.IsNullOrEmpty(Name.Text)) ValidationMessage += "The Name field is required. \n";
            if (_trucksView.Where(x => x.IsActive).Count()<=0) ValidationMessage += "At least one truck must be selected. \n";

            return string.IsNullOrEmpty(ValidationMessage);
        }
    }
}
