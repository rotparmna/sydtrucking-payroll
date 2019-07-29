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
        private business.IBusiness<Employee> _employeeBusiness;
        private string _idEmployeeSelected;

        public string ValidationMessage { get; set; }

        public Employees()
        {
            InitializeComponent();
            _employeesModel = new List<Employee>();
            _employeeBusiness = new business.Employee();
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

        private void ListEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(Employee))
            {
                LoadDataBySelectedRow((Employee)e.AddedItems[0]);
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
            DriverLicense.Text = string.Empty;
            ExpirationDate.SelectedDate = DateTime.Now;
            Address.Text = string.Empty;
            PhoneNumber.Text = string.Empty;
            State.Text = string.Empty;
            PaymentMethod.SelectedIndex = -1;
            TaxForm.SelectedIndex = -1;
            Rate.Text = 0.0.ToString("C");
            WeeklyPayment.Text = 0.0.ToString("C");
            StateDriverLicense.Text = string.Empty;
            City.Text = string.Empty;
            ZipCode.Text = string.Empty;
            Email.Text = string.Empty;
            JobDescription.Text = string.Empty;
            CheckRate.IsChecked = false;
            CheckWeeklyPayment.IsChecked = false;
            Rate.IsEnabled = false;
            WeeklyPayment.IsEnabled = false;
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
                              .Where(x => x.SocialSecurity == SocialSecurity.Text.ToSocialSecurity())
                              .DefaultIfEmpty(new Employee() { Id = string.Empty })
                              .FirstOrDefault()
                              .Id;

                PaymentType paymentMethod = PaymentType.NA;
                Enum.TryParse(((ComboBoxItem)PaymentMethod.SelectedItem).Content.ToString(), out paymentMethod);

                TaxType taxForm = TaxType.NA;
                Enum.TryParse(((ComboBoxItem)TaxForm.SelectedItem).Content.ToString(), out taxForm);

                Employee employee = new Employee()
                {
                    Address = Address.Text,
                    Birthdate = Birthdate.SelectedDate.HasValue?Birthdate.SelectedDate.Value:DateTime.MinValue,
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
                    Rate = 0.0,
                    SocialSecurity = SocialSecurity.Text.ToSocialSecurity(),
                    State = State.Text,
                    TaxForm = taxForm,                    
                    City = City.Text,
                    ZipCode = ZipCode.Text,
                    Email = Email.Text,
                    Job = JobDescription.Text,
                    IsWeeklyPayment = CheckWeeklyPayment.IsChecked.Value
                };
                if (CheckRate.IsChecked.Value)
                {
                    employee.Rate = double.Parse(Rate.Text.Replace("$", string.Empty));
                }
                else if (CheckWeeklyPayment.IsChecked.Value)
                {
                    employee.Rate = double.Parse(WeeklyPayment.Text.Replace("$", string.Empty));
                }

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
            SocialSecurity.Text = employee.SocialSecurityHyphen;
            Name.Text = employee.Name;
            LastName.Text = employee.LastName;
            Birthdate.SelectedDate = employee.Birthdate;
            DriverLicense.Text = employee.License.Number;
            StateDriverLicense.Text = employee.License.State;
            ExpirationDate.SelectedDate = employee.License.Expiration;
            Address.Text = employee.Address;
            PhoneNumber.Text = employee.PhoneNumber;
            State.Text = employee.State;
            City.Text = employee.City;
            ZipCode.Text = employee.ZipCode;
            PaymentMethod.SelectedIndex = (int)employee.PaymentMethod;
            TaxForm.SelectedIndex = (int)employee.TaxForm;
            CheckRate.IsChecked = !employee.IsWeeklyPayment;
            CheckWeeklyPayment.IsChecked = employee.IsWeeklyPayment;
            Email.Text = employee.Email;
            JobDescription.Text = employee.Job;
            WeeklyPayment.Text = 0.ToString("C");
            Rate.Text = 0.ToString("C");
            if (employee.IsWeeklyPayment)
            {
                WeeklyPayment.Text = employee.Rate.ToString("C");
            }
            else
            {
                Rate.Text = employee.Rate.ToString("C");
            }
        }

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            Payment.IsEnabled = isEnable;
            Job.IsEnabled = isEnable;
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
            if (PaymentMethod.SelectedIndex == -1) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Payment Method");
            if (TaxForm.SelectedIndex == -1) ValidationMessage += string.Format(business.Constant.Message.ValidationRequiredFieldMessage, "Tax Form");

            return string.IsNullOrEmpty(ValidationMessage);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var id = ((Button)sender).Tag.ToString();
            var employee = _employeeBusiness.Get(id);
            DeleteView(employee);
        }

        public void DeleteView(Employee employee)
        {
            if (MessageBox.Show("Are you sure delete employee?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                employee.IsDetele = true;
                _employeeBusiness.Update(employee);

                FillGrid();
            }
        }

        private void CheckRate_CheckChange(object sender, RoutedEventArgs e)
        {
            Rate.IsEnabled = CheckRate.IsChecked.Value;
            CheckWeeklyPayment.IsChecked = !CheckRate.IsChecked.Value;
        }

        private void CheckWeeklyPayment_CheckChange(object sender, RoutedEventArgs e)
        {
            WeeklyPayment.IsEnabled = CheckWeeklyPayment.IsChecked.Value;
            CheckRate.IsChecked = !CheckWeeklyPayment.IsChecked.Value;
        }
    }
}
