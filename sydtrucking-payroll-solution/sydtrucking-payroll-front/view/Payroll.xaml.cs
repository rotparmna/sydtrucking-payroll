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
    public partial class Payroll : Window
    {
        business.IBusiness<model.Payroll> _payrollBusiness;
        business.IBusiness<Employee> _employeeBusiness;
        business.IBusiness<Company> _companyBusiness;
        ObservableCollection<PayrollDetailView> _details;
        model.Payroll _payroll;

        public Payroll()
        {
            InitializeComponent();
            _payrollBusiness = new business.Payroll();
            _employeeBusiness = new business.Employee();
            _companyBusiness = new business.Company();
            _details = new ObservableCollection<PayrollDetailView>();
            _payroll = new model.Payroll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employees.SelectedValuePath = "Id";
            Employees.DisplayMemberPath = "Fullname";
            Employees.ItemsSource = _employeeBusiness.GetAll();

            ((CollectionViewSource)Details.FindResource("Companies")).Source = _companyBusiness.GetAll();

            Details.ItemsSource = _details;

            RegularHour.Text = business.Constant.RegularHour.ToString();
        }

        private void Employees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TruckNumber.Text = ((Employee)e.AddedItems[0]).Truck.Number;
                Rate.Text = ((Employee)e.AddedItems[0]).Rate.ToString("C");
            }
        }
        
        private void Details_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollection = Details.ItemsSource as ObservableCollection<PayrollDetailView>;
            sourceCollection.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void SourceCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateHours();
            ValidationsDetails(true);
        }

        private string ValidateTicketDateInRange(bool showMessage)
        {
            var isTicketDateNotRange = false;
            var message = string.Empty;

            _details.ToList().ForEach(x =>
            {
                isTicketDateNotRange = !isTicketDateNotRange ?
                                            _details.Where(y => x.TicketDate < FromPayment.SelectedDate.Value || x.TicketDate > ToPayment.SelectedDate.Value).Count() > 1
                                            : isTicketDateNotRange;
            });

            if (isTicketDateNotRange)
                message = "The ticket date it is not within the range, review the information.\n";

            if (isTicketDateNotRange && showMessage)
                MessageBox.Show(message,
                                    "Ticket Date",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

            return message;
        }

        private void Details_CurrentCellChanged(object sender, EventArgs e)
        {
            CalculateHours();
            ValidationsDetails(true);
        }

        private string ValidationsDetails(bool showMessage)
        {
            var message = string.Empty;
            message = ValidateTicketNumber(showMessage);
            message += ValidateTicketDateInRange(showMessage);
            return message;
        }

        private void CalculatePayment(int regularHours, int overtimeHour)
        {
            var rate = double.Parse(Rate.Text.Replace("$", string.Empty));
            var rateOvertime = rate * business.Constant.FactorRateOvertimeHour;
            var payment = 0.0;
            var paymentOvertimeHour = 0.0;

            paymentOvertimeHour = rateOvertime * overtimeHour;
            payment = (rate * regularHours) + paymentOvertimeHour;

            _payroll.Rate = rate;
            _payroll.Payment = payment;
            _payroll.PaymentOvertimeHour = paymentOvertimeHour;
            Payment.Text = payment.ToString("C");

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

        private void CalculateHours()
        {
            var totalHours = 0;
            var regularHours = 0;
            var overtimeHours = 0;

            _details.ToList().ForEach(x => totalHours = totalHours + x.Hours);

            regularHours = totalHours > business.Constant.RegularHour ?
                                business.Constant.RegularHour
                                : totalHours;
            overtimeHours = totalHours > business.Constant.RegularHour ?
                                totalHours - business.Constant.RegularHour
                                : overtimeHours;

            _payroll.TotalHours = totalHours;
            _payroll.OvertimeHour = overtimeHours;
            _payroll.RegularHour = regularHours;
            TotalHours.Text = totalHours.ToString();
            OvertimeHour.Text = overtimeHours.ToString();

            CalculatePayment(regularHours, overtimeHours);
        }

        private string ValidateTicketNumber(bool showMessage)
        {
            var isNameRepeat = false;
            var message = string.Empty;

            _details.ToList().ForEach(x =>
            {
                isNameRepeat = !isNameRepeat ?
                                    _details.Where(y => x.TicketNumber == y.TicketNumber).Count() > 1
                                    : isNameRepeat;
            });

            if (isNameRepeat)
                message = "The ticket number is already digitized, review the information.\n";

            if (isNameRepeat && showMessage) 
                MessageBox.Show(message,
                                    "Ticket Number",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);

            return message;
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
                _payroll.TruckNumber = int.Parse(TruckNumber.Text);
                _payroll.Employee = (Employee)Employees.SelectedItem;
                _payroll.From = FromPayment.SelectedDate.Value;
                _payroll.To = ToPayment.SelectedDate.Value;
                _payroll.PaymentDate = ToPayment.SelectedDate.Value.AddDays(business.Constant.DaysWeekPayment);
                _details.ToList().ForEach(x =>
                {
                    _payroll.Details.Add(new PayrollDetail()
                    {
                        Company = x.Company,
                        Hours = x.Hours,
                        Ticket = new Ticket() { Date = x.TicketDate, Number = x.TicketNumber }
                    });
                });

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
                MessageBox.Show(message, "Validations", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Clear()
        {
            _payroll = new model.Payroll();
            _details = new ObservableCollection<PayrollDetailView>();

            Employees.SelectedItem = null;
            TruckNumber.Text = string.Empty;
            Rate.Text = string.Empty;
            FromPayment.SelectedDate = null;
            ToPayment.SelectedDate = null;
            TotalHours.Text = string.Empty;
            RegularHour.Text = string.Empty;
            OvertimeHour.Text = string.Empty;
            Payment.Text = string.Empty;
            Deductions.Text = string.Empty;
            Reimbursements.Text = string.Empty;
            TotalPayment.Text = string.Empty;
            Details.ItemsSource = _details;
        }

        private string Validations()
        {
            string message = string.Empty;

            if (Employees.SelectedItem == null) message += "No employee selected.\n";
            if (!FromPayment.SelectedDate.HasValue) message += "Date not selected.\n";
            if (!ToPayment.SelectedDate.HasValue) message += "Date not selected.\n";
            if (_details.Count <= 0) message += "No detail records.\n";
            message += ValidationsDetails(false);

            return message;
        }

        private void FromPayment_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FromPayment.SelectedDate.HasValue)
                ToPayment.SelectedDate = FromPayment.SelectedDate.Value.AddDays(business.Constant.DaysWeek);
        }
    }
}