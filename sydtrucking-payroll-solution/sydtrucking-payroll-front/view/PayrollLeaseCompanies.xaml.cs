namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Lógica de interacción para Payroll.xaml
    /// </summary>
    public partial class PayrollLeaseCompanies : Window, IPayrollView
    {
        business.IBusiness<PayrollLeaseCompany> _payrollLeaseCompanyBusiness;
        business.IBusiness<model.Payroll> _payrollBusiness;
        business.IBusiness<LeaseCompany> _leaseCompanyBusiness;
        ObservableCollection<PayrollLeaseCompanyDetails> _details;
        ObservableCollection<GenericCollection> _deductions;
        ObservableCollection<GenericCollection> _reimbursements;
        ObservableCollection<RateDetail> _rates;
        PayrollLeaseCompany _payrollLeaseCompany;
        double _percentLeaseFeeValue = 0.0;
        double _percentWorkerCompValue = 0.0;

        public PayrollLeaseCompanies()
        {
            _payrollLeaseCompanyBusiness = new business.PayrollLeaseCompany();
            _payrollBusiness = new business.Payroll();
            _leaseCompanyBusiness = new business.LeaseCompany();
            _payrollLeaseCompany = new PayrollLeaseCompany();
            _details = new ObservableCollection<PayrollLeaseCompanyDetails>();
            _deductions = new ObservableCollection<GenericCollection>();
            _reimbursements = new ObservableCollection<GenericCollection>();
            _rates = new ObservableCollection<RateDetail>();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LeaseCompanies.SelectedValuePath = "Id";
            LeaseCompanies.DisplayMemberPath = "Name";
            LeaseCompanies.ItemsSource = _leaseCompanyBusiness.GetAll();

            LoadDetails();

            Details.ItemsSource = _details;
            Deductions.ItemsSource = _deductions;
            Reimbursements.ItemsSource = _reimbursements;
            Rates.ItemsSource = _rates;

            DateTime nextFriday = DateTime.Now.NextFriday();
            DateTime toPayment = nextFriday.AddDays(business.Constant.PayrollLeaseCompany.LastThreeFridayPayrollLeaseCompany);
            DateTime fromPayment = toPayment.AddDays(business.Constant.Payroll.DaysWeek * -1);

            Date.SelectedDate = nextFriday;
            ToPayment.SelectedDate = toPayment;
            FromPayment.SelectedDate = fromPayment;

            _percentLeaseFeeValue = business.Constant.PayrollLeaseCompany.PercentLeaseFeeValue;
            _percentWorkerCompValue = business.Constant.PayrollLeaseCompany.PercentWorkerCompValue;

            PercentLeaseFeeValue.Text = _percentLeaseFeeValue.ToString();
            PercentWorkerCompValue.Text = _percentWorkerCompValue.ToString();
        }

        private void LoadDetails()
        {
            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsEnabled = true,
                Item = "Diesel",
                Value = 0,
            });

            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsEnabled = false,
                Item = "Lease Fee",
                Value = 0,
            });

            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsEnabled = false,
                Item = "Driver Paycheck",
                Value = 0,
            });

            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsEnabled = false,
                Item = "Worker's Comp",
                Value = 0,
            });
        }

        private void LeaseCompanies_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                Trucks.SelectedValuePath = "Id";
                Trucks.DisplayMemberPath = "Number";
                Trucks.Items.Clear();
                Trucks.ItemsSource = ((LeaseCompany)e.AddedItems[0]).Trucks;
            }
        }
        
        private void Details_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollectionDetails = Details.ItemsSource as ObservableCollection<PayrollLeaseCompanyDetails>;
            sourceCollectionDetails.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void Calculate()
        {
            CalculateLeaseFeeAndWorkerComp();
            CalculateTotal();
        }

        private void SourceCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Calculate();
        }

        private void CalculateTotal()
        {
            var totalRates = _rates.ToList().Sum(x => x.Rate * x.Hours);
            var totalDetails = _details.ToList().Sum(x => x.Value);
            var totalDeductions = _deductions.ToList().Sum(x => x.Value);
            var totalReimbursements = _reimbursements.ToList().Sum(x => x.Value);
            var total = totalRates - totalDetails - totalDeductions + totalReimbursements;

            Total.Text = total.ToString("C");
        }

        private void CalculateLeaseFeeAndWorkerComp()
        {
            if (_details.Count > 0)
            {
                var totalRates = _rates.ToList().Sum(x => x.Rate * x.Hours);
                var leaseFee = 0.0;
                var workerComp = 0.0;

                leaseFee = totalRates * _percentLeaseFeeValue;
                workerComp = _payrollLeaseCompany.DriverPaycheck * _percentWorkerCompValue;

                _details.Where(x => x.Item.Contains("Lease Fee")).DefaultIfEmpty(new PayrollLeaseCompanyDetails()).FirstOrDefault().Value = leaseFee;
                _details.Where(x => x.Item.Contains("Worker's Comp")).DefaultIfEmpty(new PayrollLeaseCompanyDetails()).FirstOrDefault().Value = workerComp;

                Details.Items.Refresh();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;// Validations();

            if (message == string.Empty)
            {
                CalculateTotal();
                _payrollLeaseCompany.From = FromPayment.SelectedDate.Value;
                _payrollLeaseCompany.To = ToPayment.SelectedDate.Value;
                _payrollLeaseCompany.Date = Date.SelectedDate.Value;
                _payrollLeaseCompany.PercentLeaseFeeValue = _percentLeaseFeeValue;
                _payrollLeaseCompany.PercentWorkerCompValue = _percentWorkerCompValue;
                _payrollLeaseCompany.Deductions = _deductions;
                _payrollLeaseCompany.Reimbursements = _reimbursements;
                _payrollLeaseCompany.LeaseCompany = (LeaseCompanies.SelectedItem as LeaseCompany);
                _payrollLeaseCompany.Total = double.Parse(Total.Text.Replace("$", string.Empty));
                _payrollLeaseCompany.Truck = (Trucks.SelectedItem as Truck);

                _payrollLeaseCompany.Details.Clear();
                _details.ToList().ForEach(x =>
                {
                    _payrollLeaseCompany.Details.Add(new GenericCollection()
                    {
                        Item = x.Item,
                        Value = x.Value
                    });
                });

                try
                {
                    _payrollLeaseCompanyBusiness.Update(_payrollLeaseCompany);
                }
                catch (Exception)
                {
                    throw;
                }

                MessageBox.Show("Saved information", "Payroll Lease Company", MessageBoxButton.OK, MessageBoxImage.Information);
                Clear();
            }
            else
                MessageBox.Show(message, "Validations", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Clear()
        {
            _payrollLeaseCompany = new PayrollLeaseCompany();
            _details = new ObservableCollection<PayrollLeaseCompanyDetails>();
            _deductions = new ObservableCollection<GenericCollection>();
            _reimbursements = new ObservableCollection<GenericCollection>();
            _rates = new ObservableCollection<RateDetail>();

            _percentLeaseFeeValue = business.Constant.PayrollLeaseCompany.PercentLeaseFeeValue;
            _percentWorkerCompValue = business.Constant.PayrollLeaseCompany.PercentWorkerCompValue;

            LoadDetails();

            LeaseCompanies.SelectedItem = null;
            Trucks.SelectedItem = null;
            Total.Text = "$ 0";
            Details.ItemsSource = _details;
            Deductions.ItemsSource = _deductions;
            Reimbursements.ItemsSource = _reimbursements;
            Rates.ItemsSource = _rates;
        }

        private void Trucks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(Truck))
            {
                LoadDetails((Truck)e.AddedItems[0]);
                Calculate();
            }
        }

        private void LoadDetails(Truck truck)
        {
            List<model.Payroll> payrolls = new List<model.Payroll>();

            if (truck != null)
            {
                payrolls = ((business.Payroll)_payrollBusiness).GetByDateAndTruck(ToPayment.SelectedDate.Value, FromPayment.SelectedDate.Value, truck);
            }

            _payrollLeaseCompany.Payrolls = payrolls;
            _details.Where(x => x.Item.Contains("Driver Paycheck")).FirstOrDefault().Value = _payrollLeaseCompany.DriverPaycheck;
            _rates = _payrollLeaseCompany.Rates.ToObservableCollection();
            Rates.ItemsSource = _rates;
            Details.ItemsSource = _details;
        }

        private void Deductions_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollectionDeductions = Deductions.ItemsSource as ObservableCollection<GenericCollection>;
            sourceCollectionDeductions.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void Rates_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollectionRates = Rates.ItemsSource as ObservableCollection<RateDetail>;
            sourceCollectionRates.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void FromPayment_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadDetails(Trucks.SelectedItem as Truck);
            Calculate();
        }

        private void ToPayment_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            LoadDetails(Trucks.SelectedItem as Truck);
            Calculate();
        }

        public void LoadPayroll(string id)
        {
            _payrollLeaseCompany = _payrollLeaseCompanyBusiness.Get(id);
            
            _percentLeaseFeeValue = _payrollLeaseCompany.PercentLeaseFeeValue;
            _percentWorkerCompValue = _payrollLeaseCompany.PercentWorkerCompValue;

            LeaseCompanies.SelectedValue = _payrollLeaseCompany.LeaseCompany.Id;
            Trucks.SelectedValue = _payrollLeaseCompany.Truck.Id;
            Date.SelectedDate = _payrollLeaseCompany.Date;
            FromPayment.SelectedDate = _payrollLeaseCompany.From;
            ToPayment.SelectedDate = _payrollLeaseCompany.To;
            PercentLeaseFeeValue.Text = _percentLeaseFeeValue.ToString();
            PercentWorkerCompValue.Text = _percentWorkerCompValue.ToString();
            Total.Text = _payrollLeaseCompany.Total.ToString("C");

            if (_rates.Count == 0)
                foreach (var rate in _payrollLeaseCompany.Rates)
                {
                    _rates.Add(new RateDetail()
                    {
                        Companies = rate.Companies,
                        Hours = rate.Hours,
                        Rate = rate.Rate
                    });
                }

            _details.Clear();
            foreach (var detail in _payrollLeaseCompany.Details)
            {
                _details.Add(new PayrollLeaseCompanyDetails()
                {
                    IsEnabled = !detail.Item.Contains("Diesel"),
                    Item = detail.Item,
                    Value = detail.Value
                });
            }

            foreach (var deduction in _payrollLeaseCompany.Deductions)
            {
                _deductions.Add(new GenericCollection()
                {
                    Item = deduction.Item,
                    Value = deduction.Value
                });
            }

            foreach (var reimbursements in _payrollLeaseCompany.Reimbursements)
            {
                _reimbursements.Add(new GenericCollection()
                {
                    Item = reimbursements.Item,
                    Value = reimbursements.Value
                });
            }
        }

        private void Details_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            CalculateTotal();
        }

        private void Reimbursements_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollectionReimbursements = Reimbursements.ItemsSource as ObservableCollection<GenericCollection>;
            sourceCollectionReimbursements.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void PercentLeaseFeeValue_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateLeaseFeeAndWorkerComp();
        }

        private void PercentWorkerCompValue_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateLeaseFeeAndWorkerComp();
        }

        private void ValidateLeaseFeeAndWorkerComp()
        {
            bool refreshCalculate = false;

            bool leaseFeeConvert = double.TryParse(PercentLeaseFeeValue.Text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out double leaseFeeValue);
            if (leaseFeeConvert)
            {
                refreshCalculate = true;
                _percentLeaseFeeValue = leaseFeeValue;
                PercentLeaseFeeValue.Text = leaseFeeValue.ToString();
            }
            else
            {
                MessageBox.Show("Value % Lease Fee bad format.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                PercentLeaseFeeValue.Text = _percentLeaseFeeValue.ToString();
            }

            bool workerCompConverter = double.TryParse(PercentWorkerCompValue.Text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out double workerCompValue);
            if (workerCompConverter)
            {
                refreshCalculate = true;
                _percentWorkerCompValue = workerCompValue;
                PercentWorkerCompValue.Text = workerCompValue.ToString();
            }
            else
            {
                MessageBox.Show("Value % Worker Comp bad format.", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                PercentWorkerCompValue.Text = _percentWorkerCompValue.ToString();
            }

            if (refreshCalculate)
            {
                Calculate();
            }
        }
    }
}