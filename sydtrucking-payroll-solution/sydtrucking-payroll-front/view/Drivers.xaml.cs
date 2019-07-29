namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.enums;
    using sydtrucking_payroll_front.model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Lógica de interacción para Drivers.xaml
    /// </summary>
    public partial class Drivers : Window, IView<Driver>, IValidation
    {
        private List<Driver> _driversModel;
        private List<Truck> _trucksModel;
        private business.IBusiness<Driver> _driverBusiness;
        private business.Truck _truckBusiness;
        private string _idDriverSelected;

        public string ValidationMessage { get; set; }

        public Drivers()
        {
            InitializeComponent();
            _driversModel = new List<Driver>();
            _trucksModel = new List<Truck>();
            _driverBusiness = new business.Driver();
            _truckBusiness = new business.Truck();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ClearView();
            LoadTrucks();
            FillGrid();
        }

        private void LoadTrucks()
        {
            _trucksModel = _truckBusiness.GetAll();
            Trucks.DisplayMemberPath = "Number";
            Trucks.SelectedValuePath = "Id";
            Trucks.ItemsSource = _trucksModel;
        }
        
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveView();
        }

        private void ListDrivers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(Driver))
            {
                LoadDataBySelectedRow((Driver)e.AddedItems[0]);
            }
        }
        
        private void Actually_Checked(object sender, RoutedEventArgs e)
        {
            ChangeActuallyChecked();
        }

        private void Actually_Unchecked(object sender, RoutedEventArgs e)
        {
            ChangeActuallyChecked();
        }

        private void ChangeActuallyChecked()
        {
            TerminationDate.IsEnabled = Actually.IsChecked.Value;
        }

        private void Trucks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Trucks.SelectedIndex>=0 && e.AddedItems.Count>0 && e.AddedItems[0] is Truck)
            {
                var truck = e.AddedItems[0] as Truck;
                Year.Text = truck.Year.ToString();
                Vin.Text = truck.Vin;
                Make.Text = truck.Make;
                Plate.Text = truck.Plate;
                Registration.SelectedDate = truck.Registration;
                Inspection.SelectedDate = truck.Inspection;
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            CreateView();
        }

        public void ClearView()
        {
            SocialSecurity.Text = string.Empty;
            Name.Text = string.Empty;
            LastName.Text = string.Empty;
            Birthdate.SelectedDate = DateTime.Now;
            Trucks.SelectedIndex = -1;
            DriverLicense.Text = string.Empty;
            ExpirationDate.SelectedDate = DateTime.Now;
            HireDate.SelectedDate = DateTime.Now;
            TerminationDate.SelectedDate = DateTime.Now;
            Actually.IsChecked = false;
            TerminationDate.IsEnabled = false;
            Address.Text = string.Empty;
            PhoneNumber.Text = string.Empty;
            State.Text = string.Empty;
            PaymentMethod.SelectedIndex = -1;
            TaxForm.SelectedIndex = -1;
            Rate.Text = 0.0.ToString("C");
            Year.Text = string.Empty;
            Vin.Text = string.Empty;
            Make.Text = string.Empty;
            Plate.Text = string.Empty;
            Registration.SelectedDate = DateTime.Now;
            Inspection.SelectedDate = DateTime.Now;
            StateDriverLicense.Text = string.Empty;
            City.Text = string.Empty;
            ZipCode.Text = string.Empty;
            Email.Text = string.Empty;

            LoadTrucks();
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

                var id = _driverBusiness.GetAll()
                              .Where(x => x.SocialSecurity == SocialSecurity.Text.ToSocialSecurity())
                              .DefaultIfEmpty(new Driver() { Id = string.Empty })
                              .FirstOrDefault()
                              .Id;

                PaymentType paymentMethod = PaymentType.NA;
                Enum.TryParse(((ComboBoxItem)PaymentMethod.SelectedItem).Content.ToString(), out paymentMethod);

                TaxType taxForm = TaxType.NA;
                Enum.TryParse(((ComboBoxItem)TaxForm.SelectedItem).Content.ToString(), out taxForm);

                Driver driver = new Driver()
                {
                    Address = Address.Text,
                    Birthdate = Birthdate.SelectedDate.HasValue? Birthdate.SelectedDate.Value:DateTime.MinValue,
                    Contract = new Contract()
                    {
                        HireDate = HireDate.SelectedDate.HasValue?HireDate.SelectedDate.Value:DateTime.MinValue,
                        TerminationDate = Actually.IsChecked.Value ? TerminationDate.SelectedDate : null
                    },
                    Id = id,
                    LastName = LastName.Text,
                    License = new DriverLicense()
                    {
                        Expiration = ExpirationDate.SelectedDate.Value,
                        Number = DriverLicense.Text,
                        State = StateDriverLicense.Text
                    },
                    Name = Name.Text,
                    PaymentMethod = paymentMethod,
                    PhoneNumber = PhoneNumber.Text,
                    Rate = double.Parse(Rate.Text.Replace("$", string.Empty)),
                    SocialSecurity = SocialSecurity.Text.ToSocialSecurity(),
                    State = State.Text,
                    TaxForm = taxForm,
                    Truck = new Truck()
                    {
                        Id = (Trucks.SelectedItem as Truck).Id,
                        Inspection = Inspection.SelectedDate.Value,
                        Make = Make.Text,
                        Number = (Trucks.SelectedItem as Truck).Number,
                        Plate = Plate.Text,
                        Registration = Registration.SelectedDate.Value,
                        Vin = Vin.Text,
                        Year = int.Parse(Year.Text)
                    },
                    City = City.Text,
                    ZipCode = ZipCode.Text,
                    Email = Email.Text
                };

                _driverBusiness.Update(driver);
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
            _driversModel = _driverBusiness.GetAll();
            ListDrivers.ItemsSource = _driversModel;
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void LoadDataBySelectedRow(Driver driver)
        {
            _idDriverSelected = driver.Id;
            SocialSecurity.Text = driver.SocialSecurity.ToSocialSecurityHyphen();
            Name.Text = driver.Name;
            LastName.Text = driver.LastName;
            Birthdate.SelectedDate = driver.Birthdate;
            Trucks.SelectedValue = driver.Truck.Id;
            Year.Text = driver.Truck.Year.ToString();
            Vin.Text = driver.Truck.Vin;
            Make.Text = driver.Truck.Make;
            Plate.Text = driver.Truck.Plate;
            Registration.SelectedDate = driver.Truck.Registration;
            Inspection.SelectedDate = driver.Truck.Inspection;
            DriverLicense.Text = driver.License.Number;
            StateDriverLicense.Text = driver.License.State;
            ExpirationDate.SelectedDate = driver.License.Expiration;
            HireDate.SelectedDate = driver.Contract.HireDate;
            TerminationDate.SelectedDate = driver.Contract.TerminationDate;
            TerminationDate.IsEnabled = !driver.Contract.Actually;
            Actually.IsChecked = !driver.Contract.Actually;
            Address.Text = driver.Address;
            PhoneNumber.Text = driver.PhoneNumber;
            State.Text = driver.State;
            City.Text = driver.City;
            ZipCode.Text = driver.ZipCode;
            PaymentMethod.SelectedIndex = (int)driver.PaymentMethod;
            TaxForm.SelectedIndex = (int)driver.TaxForm;
            Rate.Text = driver.Rate.ToString("C");
            Email.Text = driver.Email;
        }

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            AssignedTruck.IsEnabled = isEnable;
            Payment.IsEnabled = isEnable;
            Contract.IsEnabled = isEnable;
            Save.IsEnabled = isEnable; 
            New.IsEnabled = !isEnable;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditView();
        }

        public bool IsViewValid()
        {
            ValidationMessage = string.Empty;

            if (string.IsNullOrEmpty(Name.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Name");
            if (string.IsNullOrEmpty(LastName.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Last Name");
            if (string.IsNullOrEmpty(SocialSecurity.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Social Security");
            if (string.IsNullOrEmpty(DriverLicense.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Driver License");
            if (string.IsNullOrEmpty(StateDriverLicense.Text)) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "State");
            if (!ExpirationDate.SelectedDate.HasValue) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Expiration Date");
            if (Trucks.SelectedIndex == -1) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Truck Number");
            if (PaymentMethod.SelectedIndex == -1) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Payment Method");
            if (TaxForm.SelectedIndex == -1) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Tax Form");

            return string.IsNullOrEmpty(ValidationMessage);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).Tag.ToString();
            var driver = _driverBusiness.Get(id);
            DeleteView(driver);
        }

        public void DeleteView(Driver driver)
        {
            if (MessageBox.Show("Are you sure delete driver?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                driver.IsDetele = true;
                _driverBusiness.Update(driver);

                FillGrid();
            }
        }
    }
}
