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
    public partial class Employees : Window
    {
        private List<Employee> _employeesModel;
        private List<Truck> _trucksModel;
        private business.Employee _employeeBusiness;
        private business.Truck _truckBusiness;
        private string _idEmployeeSelected;

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
            Clear();
            LoadTrucks();
            LoadEmployees();
        }

        private void LoadTrucks()
        {
            _trucksModel = _truckBusiness.GetAll();
            Trucks.DisplayMemberPath = "Number";
            Trucks.SelectedValuePath = "Id";
            Trucks.ItemsSource = _trucksModel;
        }

        private void LoadEmployees()
        {
            _employeesModel = _employeeBusiness.GetAll();
            ListEmployees.ItemsSource = _employeesModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
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
                ZipCode = ZipCode.Text
            };

            _employeeBusiness.Update(employee);
            LoadEmployees();
        }

        private void ListEmployees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(Employee))
            {
                LoadEmployee((Employee)e.AddedItems[0]);
            }
        }

        private void LoadEmployee(Employee employee)
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
        }

        private void Clear()
        {
            SocialSecurity.Text = string.Empty;
            Name.Text = string.Empty;
            LastName.Text = string.Empty;
            Birthdate.SelectedDate = DateTime.Now;
            Trucks.SelectedIndex = 0;
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

            LoadTrucks();
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
    }
}
