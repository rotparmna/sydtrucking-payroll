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
        private business.Employee _employeeBusiness;
        private string _idEmployeeSelected;

        public Employees()
        {
            InitializeComponent();
            _employeesModel = new List<Employee>();
            _employeeBusiness = new business.Employee();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
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
                Contract = new Contract() { HireDate = HireDate.SelectedDate.Value, TerminationDate = TerminationDate.SelectedDate.Value },
                Id = id,
                LastName = LastName.Text,
                License = new DriverLicense() { Expiration = ExpirationDate.SelectedDate.Value, Number = DriverLicense.Text },
                Name = Name.Text,
                PaymentMethod = paymentMethod,
                PhoneNumber = PhoneNumber.Text,
                Rate = double.Parse(Rate.Text.Replace("$",string.Empty)),
                SocialSecurity = long.Parse(SocialSecurity.Text),
                State = State.Text,
                TaxForm = taxForm,
                TruckNumber = TruckNumber.Text
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
            TruckNumber.Text = employee.TruckNumber;
            DriverLicense.Text = employee.License.Number;
            ExpirationDate.SelectedDate = employee.License.Expiration;
            HireDate.SelectedDate = employee.Contract.HireDate;
            TerminationDate.SelectedDate = employee.Contract.TerminationDate;
            Address.Text = employee.Address;
            PhoneNumber.Text = employee.PhoneNumber;
            State.Text = employee.State;
            PaymentMethod.SelectedIndex = (int)employee.PaymentMethod;
            TaxForm.SelectedIndex = (int)employee.TaxForm;
            Rate.Text = employee.Rate.ToString("C");
        }
    }
}
