namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Linq;
    using System;
    using System.Windows.Data;
    using System.Windows.Controls;

    /// <summary>
    /// Lógica de interacción para Payroll.xaml
    /// </summary>
    public partial class Payroll : Window, IPayrollView
    {
        business.IBusiness<model.Payroll> _payrollBusiness;
        business.IBusiness<Driver> _driverBusiness;
        business.IBusiness<OilCompany> _companyBusiness;
        ObservableCollection<PayrollDetailView> _details;
        model.Payroll _payroll;

        public Payroll()
        {
            InitializeComponent();
            _payrollBusiness = new business.Payroll();
            _driverBusiness = new business.Driver();
            _companyBusiness = new business.OilCompany();
            _details = new ObservableCollection<PayrollDetailView>();
            _payroll = new model.Payroll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Drivers.SelectedValuePath = "Id";
            Drivers.DisplayMemberPath = "Fullname";
            Drivers.ItemsSource = _driverBusiness.GetAll();

            ((CollectionViewSource)Details.FindResource("OilCompanies")).Source = _companyBusiness.GetAll();

            Details.ItemsSource = _details;

            RegularHour.Text = business.Constant.Payroll.RegularHour.ToString();
        }

        private void Drivers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                TruckNumber.Text = ((Driver)e.AddedItems[0]).Truck.Number;
                Rate.Text = ((Driver)e.AddedItems[0]).Rate.ToString("C");
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
                                            _details
                                            .Where(y => x.TicketDate < FromPayment.SelectedDate.Value || 
                                                        x.TicketDate > ToPayment.SelectedDate.Value)
                                            .Count() > 1
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
            _payroll.Rate = double.Parse(Rate.Text.Replace("$", string.Empty));
            _payroll.CalculatePayment();

            Payment.Text = _payroll.Payment.ToString("C");

            CalculateTotalPayment();
        }

        private void CalculateTotalPayment()
        {
            var deductions = 0.0;
            var reimbursements = 0.0;

            double.TryParse(Deductions.Text, out deductions);
            double.TryParse(Reimbursements.Text, out reimbursements);

            _payroll.Deductions = deductions;
            _payroll.Reimbursements = reimbursements;

            _payroll.CalculateTotalPayment();

            TotalPayment.Text = _payroll.TotalPayment.ToString("C");
        }

        private void CalculateHours()
        {
            var totalHours = 0;
            var regularHours = 0;
            var overtimeHours = 0;

            _details.ToList().ForEach(x => totalHours = totalHours + x.Hours);

            regularHours = totalHours > business.Constant.Payroll.RegularHour ?
                                business.Constant.Payroll.RegularHour
                                : totalHours;
            overtimeHours = totalHours > business.Constant.Payroll.RegularHour ?
                                totalHours - business.Constant.Payroll.RegularHour
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
                var deductions = 0.0;
                var reimbursements = 0.0;
                double.TryParse(Deductions.Text, out deductions);
                double.TryParse(Reimbursements.Text, out reimbursements);

                _payroll.TruckNumber = int.Parse(TruckNumber.Text);
                _payroll.Driver = (Driver)Drivers.SelectedItem;
                _payroll.From = FromPayment.SelectedDate.Value;
                _payroll.To = ToPayment.SelectedDate.Value;
                _payroll.PaymentDate = ToPayment.SelectedDate.Value.AddDays(business.Constant.Payroll.DaysWeekPayment);
                _payroll.DeductionsDetail = DeductionsText.Text;
                _payroll.ReimbursmentsDetail = ReimbursementsText.Text;
                _payroll.PrintRegularHoursApartOvertime = CheckPrint.IsChecked.Value;

                _payroll.Details.Clear();
                _details.ToList().ForEach(x =>
                {
                    _payroll.Details.Add(new PayrollDetail()
                    {
                        OilCompany = x.OilCompany,
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

            Drivers.SelectedItem = null;
            TruckNumber.Text = string.Empty;
            Rate.Text = string.Empty;
            FromPayment.SelectedDate = null;
            ToPayment.SelectedDate = null;
            TotalHours.Text = string.Empty;
            RegularHour.Text = string.Empty;
            OvertimeHour.Text = string.Empty;
            Payment.Text = string.Empty;
            Deductions.Text = string.Empty;
            DeductionsText.Text = string.Empty;
            Reimbursements.Text = string.Empty;
            ReimbursementsText.Text = string.Empty;
            TotalPayment.Text = string.Empty;
            Details.ItemsSource = _details;
        }

        private string Validations()
        {
            string message = string.Empty;

            if (Drivers.SelectedItem == null) message += "No driver selected.\n";
            if (!FromPayment.SelectedDate.HasValue) message += "Date not selected.\n";
            if (!ToPayment.SelectedDate.HasValue) message += "Date not selected.\n";
            if (_details.Count <= 0) message += "No detail records.\n";
            message += ValidationsDetails(false);

            return message;
        }

        private void FromPayment_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FromPayment.SelectedDate.HasValue)
                ToPayment.SelectedDate = FromPayment.SelectedDate.Value.AddDays(business.Constant.Payroll.DaysWeek);
        }

        public void LoadPayroll(string id)
        {
            _payroll = _payrollBusiness.Get(id);

            Drivers.SelectedValue = _payroll.Driver.Id;
            TruckNumber.Text = _payroll.TruckNumber.ToString();
            Rate.Text = _payroll.Rate.ToString("C");
            CheckPrint.IsChecked = _payroll.PrintRegularHoursApartOvertime;
            FromPayment.SelectedDate = _payroll.From;
            ToPayment.SelectedDate = _payroll.To;
            TotalHours.Text = _payroll.TotalHours.ToString();
            RegularHour.Text = _payroll.RegularHour.ToString();
            OvertimeHour.Text = _payroll.OvertimeHour.ToString();
            Payment.Text = _payroll.Payment.ToString("C");
            Deductions.Text = _payroll.Deductions.ToString("C");
            DeductionsText.Text = _payroll.DeductionsDetail;
            Reimbursements.Text = _payroll.Reimbursements.ToString("C");
            ReimbursementsText.Text = _payroll.ReimbursmentsDetail;
            TotalPayment.Text = _payroll.TotalPayment.ToString("C");

            foreach (var item in _payroll.Details)
            {
                _details.Add(new PayrollDetailView()
                {
                    Hours = item.Hours,
                    OilCompany = item.OilCompany,
                    TicketDate = item.Ticket.Date,
                    TicketNumber = item.Ticket.Number
                });
            }
        }
    }
}