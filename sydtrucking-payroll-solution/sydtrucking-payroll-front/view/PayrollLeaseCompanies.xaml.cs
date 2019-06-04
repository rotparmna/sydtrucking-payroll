namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Linq;
    using System;

    /// <summary>
    /// Lógica de interacción para Payroll.xaml
    /// </summary>
    public partial class PayrollLeaseCompanies : Window
    {
        business.IBusiness<PayrollLeaseCompany> _payrollLeaseCompanyBusiness;
        business.IBusiness<model.Payroll> _payrollBusiness;
        business.IBusiness<LeaseCompany> _leaseCompanyBusiness;
        ObservableCollection<PayrollLeaseCompanyDetails> _details;
        ObservableCollection<GenericCollection> _deductions;
        PayrollLeaseCompany _payrollLeaseCompany;

        public PayrollLeaseCompanies()
        {
            _payrollLeaseCompanyBusiness = new business.PayrollLeaseCompany();
            _payrollBusiness = new business.Payroll();
            _leaseCompanyBusiness = new business.LeaseCompany();
            _payrollLeaseCompany = new PayrollLeaseCompany();
            _details = new ObservableCollection<PayrollLeaseCompanyDetails>();
            _deductions = new ObservableCollection<GenericCollection>();

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

            DateTime nextFriday = DateTime.Now.NextFriday();
            DateTime toPayment = nextFriday.AddDays(business.Constant.LastThreeFridayPayrollLeaseCompany);
            DateTime fromPayment = toPayment.AddDays(business.Constant.DaysWeek * -1);

            Date.SelectedDate = nextFriday;
            ToPayment.SelectedDate = toPayment;
            FromPayment.SelectedDate = fromPayment;
        }

        private void LoadDetails()
        {
            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsReadOnly = false,
                Item = "Diesel",
                Value = 0,
            });

            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsReadOnly = true,
                Item = "Lease Fee " + (business.Constant.PercentLeaseFeeValue * 100) + "%",
                Value = 0,
            });

            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsReadOnly = true,
                Item = "Driver Paycheck",
                Value = 0,
            });

            _details.Add(new PayrollLeaseCompanyDetails()
            {
                IsReadOnly = true,
                Item = "Worker's Comp " + (business.Constant.PercentWorkerCompValue * 100) + "%",
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

        private void SourceCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            var subttotal = double.Parse(Subtotal.Text);
            var totalDetails = _details.ToList().Sum(x => x.Value); ;
            var totalDeductions = _deductions.ToList().Sum(x => x.Value); ;
            var total = subttotal - totalDetails - totalDeductions;

            Total.Text = total.ToString("C");
        }

        private void CalculateLeaseFeeAndWorkerComp()
        {
            if (_details.Count > 0)
            {
                var subtotal = 0.0;
                var leaseFee = 0.0;
                var workerComp = 0.0;

                double.TryParse(Subtotal.Text.Replace("$", string.Empty), out subtotal);

                leaseFee = subtotal * business.Constant.PercentLeaseFeeValue;
                workerComp = subtotal * business.Constant.PercentWorkerCompValue;

                _details.Where(x => x.Item.Contains("Lease Fee")).FirstOrDefault().Value = leaseFee;
                _details.Where(x => x.Item.Contains("Worker's Comp")).FirstOrDefault().Value = workerComp;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string message = string.Empty;// Validations();

            if (message == string.Empty)
            {
                //_payroll.TruckNumber = int.Parse(TruckNumber.Text);
                //_payroll.Employee = (Employee)Employees.SelectedItem;
                _payrollLeaseCompany.From = FromPayment.SelectedDate.Value;
                _payrollLeaseCompany.To = ToPayment.SelectedDate.Value;
                
                

                try
                {
                    _payrollLeaseCompanyBusiness.Update(_payrollLeaseCompany);
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
            _payrollLeaseCompany = new model.PayrollLeaseCompany();
            _details = new ObservableCollection<PayrollLeaseCompanyDetails>();
            _deductions = new ObservableCollection<GenericCollection>();
            LoadDetails();

            Companies.Text = string.Empty;
            HoursCompanies.Text = string.Empty;
            Rate.Text = "0";
            Subtotal.Text = "0";
            Trucks.SelectedItem = null;
            Details.ItemsSource = _details;
            Deductions.ItemsSource = _deductions;
        }

        private void Trucks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0].GetType() == typeof(Truck))
            {
                _payrollLeaseCompany.Payrolls = ((business.Payroll)_payrollBusiness).GetByDateAndTruck(Date.SelectedDate.Value, (Truck)e.AddedItems[0]);
                Companies.Text = _payrollLeaseCompany.Companies;
                HoursCompanies.Text = _payrollLeaseCompany.Hours.ToString();
                _details.Where(x => x.Item.Contains("Driver Paycheck")).FirstOrDefault().Value = _payrollLeaseCompany.DriverPaycheck;
            }
        }

        private void Rate_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateSubTotal(_payrollLeaseCompany.Hours, double.Parse(Rate.Text));
        }

        private void HoursCompanies_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateSubTotal(_payrollLeaseCompany.Hours, double.Parse(Rate.Text));
        }

        private void CalculateSubTotal(int hours, double rate)
        {
            //Subtotal.Text = (hours * rate).ToString("C");
        }
        
        private void Deductions_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollectionDeductions = Deductions.ItemsSource as ObservableCollection<GenericCollection>;
            sourceCollectionDeductions.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void Subtotal_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateLeaseFeeAndWorkerComp();
        }
    }
}