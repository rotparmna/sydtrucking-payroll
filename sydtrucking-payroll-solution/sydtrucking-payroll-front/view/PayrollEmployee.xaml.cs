namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Linq;
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Lógica de interacción para Payroll.xaml
    /// </summary>
    public partial class PayrollEmployee : Window
    {
        business.IBusiness<model.PayrollEmployee> _payrollBusiness;
        business.IBusiness<Employee> _employeeBusiness;
        model.PayrollEmployee _payroll;

        public PayrollEmployee()
        {
            InitializeComponent();
            _payrollBusiness = new business.PayrollEmployee();
            _employeeBusiness = new business.Employee();
            _payroll = new model.PayrollEmployee();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employees.SelectedValuePath = "Id";
            Employees.DisplayMemberPath = "Fullname";
            Employees.ItemsSource = _employeeBusiness.GetAll();

            Clear();
        }

        private void Employees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Rate.Text = ((Employee)e.AddedItems[0]).Rate.ToString("C");
            }
        }

        private void CalculatePayment()
        {
            var rate = double.Parse(Rate.Text.Replace("$", string.Empty));
            var hours = int.Parse(TotalHours.Text);
           
            _payroll.Rate = rate;
            _payroll.TotalHours = hours;

            Payment.Text = _payroll.PaymentTotalHours.ToString("C");

            CalculateTotalPayment();
        }

        private void CalculateTotalPayment()
        {
            var totalPayment = 0.0;
            var payment = 0.0;
            var deductions = 0.0;
            var reimbursements = 0.0;

            double.TryParse(Payment.Text.Replace("$", string.Empty), out payment);
            double.TryParse(Deductions.Text, out deductions);
            double.TryParse(Reimbursements.Text, out reimbursements);

            totalPayment = payment - (deductions + reimbursements);

            _payroll.TotalPayment = totalPayment;

            TotalPayment.Text = totalPayment.ToString("C");
        }

        private void Deductions_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateTotalPayment();
        }

        private void Reimbursements_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateTotalPayment();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string message = Validations();

            if (message == string.Empty)
            {
                var deductions = 0.0;
                var reimbursements = 0.0;
                double.TryParse(Deductions.Text, out deductions);
                double.TryParse(Reimbursements.Text, out reimbursements);

                _payroll.Employee = (Employee)Employees.SelectedItem;
                _payroll.From = FromPayment.SelectedDate.Value;
                _payroll.To = ToPayment.SelectedDate.Value;
                _payroll.PaymentDate = ToPayment.SelectedDate.Value.AddDays(business.Constant.Payroll.DaysWeekPayment);
                _payroll.Deductions = deductions;
                _payroll.Reimbursements = reimbursements;
                _payroll.DeductionsDetail = DeductionsText.Text;
                _payroll.ReimbursmentsDetail = ReimbursementsText.Text;

                try
                {
                    _payrollBusiness.Update(_payroll);
                }
                catch (Exception)
                {
                    throw;
                }

                MessageBox.Show("Saved information", "Payroll", MessageBoxButton.OK, MessageBoxImage.Information);
                Clear();
            }
            else
                MessageBox.Show(message, "Validations", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Clear()
        {
            _payroll = new model.PayrollEmployee();

            Employees.SelectedItem = null;
            Rate.Text = string.Empty;
            FromPayment.SelectedDate = null;
            ToPayment.SelectedDate = null;
            TotalHours.Text = "0";
            Payment.Text = 0.ToString("C");
            Deductions.Text = 0.ToString("C");
            DeductionsText.Text = string.Empty;
            Reimbursements.Text = 0.ToString("C");
            ReimbursementsText.Text = string.Empty;
            TotalPayment.Text = 0.ToString("C");
        }

        private string Validations()
        {
            string message = string.Empty;

            if (Employees.SelectedItem == null) message += "No employee selected.\n";
            if (!FromPayment.SelectedDate.HasValue) message += "Date not selected.\n";
            if (!ToPayment.SelectedDate.HasValue) message += "Date not selected.\n";

            return message;
        }

        private void FromPayment_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FromPayment.SelectedDate.HasValue)
                ToPayment.SelectedDate = FromPayment.SelectedDate.Value.AddDays(business.Constant.Payroll.DaysWeek);
        }

        private void TotalHours_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculatePayment();
        }
    }
}