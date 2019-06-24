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
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class Employees : Window, IView<Employee>, IValidation
    {
        private List<Employee> _employeesModel;
        private List<Truck> _trucksModel;
        private business.IBusiness<Employee> _employeeBusiness;
        private business.Truck _truckBusiness;
        private string _idEmployeeSelected;

        public string ValidationMessage { get; set; }

        public Employees()
        {
            InitializeComponent();
            _employeesModel = new List<Employee>();
            _trucksModel = new List<Truck>();
            _employeeBusiness = new business.Employee();
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

        private void ListEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(Employee))
            {
                LoadDataBySelectedRow((Employee)e.AddedItems[0]);
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
            Rate.Text = string.Empty;
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

                var id = _employeeBusiness.GetAll()
                              .Where(x => x.SocialSecurity == long.Parse(SocialSecurity.Text))
                              .DefaultIfEmpty(new Employee() { Id = string.Empty })
                              .FirstOrDefault()
                              .Id;

                PaymentType paymentMethod = PaymentType.Check;
                Enum.TryParse(((ComboBoxItem)PaymentMethod.SelectedItem).Content.ToString(), out paymentMethod);

                TaxType taxForm = TaxType.W4;
                Enum.TryParse(((ComboBoxItem)TaxForm.SelectedItem).Content.ToString(), out taxForm);

                Employee employee = new Employee()
                {
                    Address = Address.Text,
                    Birthdate = Birthdate.SelectedDate.Value,
                    Contract = new Contract()
                    {
                        HireDate = HireDate.SelectedDate.Value,
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
                    SocialSecurity = long.Parse(SocialSecurity.Text),
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

                _employeeBusiness.Update(employee);
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
            _employeesModel = _employeeBusiness.GetAll();
            ListEmployees.ItemsSource = _employeesModel;
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void LoadDataBySelectedRow(Employee employee)
        {
            _idEmployeeSelected = employee.Id;
            SocialSecurity.Text = employee.SocialSecurity.ToString();
            Name.Text = employee.Name;
            LastName.Text = employee.LastName;
            Birthdate.SelectedDate = employee.Birthdate;
            Trucks.SelectedValue = employee.Truck.Id;
            Year.Text = employee.Truck.Year.ToString();
            Vin.Text = employee.Truck.Vin;
            Make.Text = employee.Truck.Make;
            Plate.Text = employee.Truck.Plate;
            Registration.SelectedDate = employee.Truck.Registration;
            Inspection.SelectedDate = employee.Truck.Inspection;
            DriverLicense.Text = employee.License.Number;
            StateDriverLicense.Text = employee.License.State;
            ExpirationDate.SelectedDate = employee.License.Expiration;
            HireDate.SelectedDate = employee.Contract.HireDate;
            TerminationDate.SelectedDate = employee.Contract.TerminationDate;
            TerminationDate.IsEnabled = !employee.Contract.Actually;
            Actually.IsChecked = !employee.Contract.Actually;
            Address.Text = employee.Address;
            PhoneNumber.Text = employee.PhoneNumber;
            State.Text = employee.State;
            City.Text = employee.City;
            ZipCode.Text = employee.ZipCode;
            PaymentMethod.SelectedIndex = (int)employee.PaymentMethod;
            TaxForm.SelectedIndex = (int)employee.TaxForm;
            Rate.Text = employee.Rate.ToString("C");
            Email.Text = employee.Email;
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
            bool isValid = false;
            ValidationMessage = string.Empty;

            if (string.IsNullOrEmpty(Name.Text)) ValidationMessage += "The Name field is required. \n";
            if (string.IsNullOrEmpty(LastName.Text)) ValidationMessage += "The Last Name field is required. \n";
            if (string.IsNullOrEmpty(SocialSecurity.Text)) ValidationMessage += "The Social Security field is required. \n";
            if (string.IsNullOrEmpty(DriverLicense.Text)) ValidationMessage += "The Driver License field is required. \n";
            if (string.IsNullOrEmpty(StateDriverLicense.Text)) ValidationMessage += "The State field is required. \n";
            if (!ExpirationDate.SelectedDate.HasValue) ValidationMessage += "The Expiration Date field is required. \n";
            if (Trucks.SelectedIndex == -1) ValidationMessage += "The Truck Number is required. \n";

            return isValid;
        }
    }
}
