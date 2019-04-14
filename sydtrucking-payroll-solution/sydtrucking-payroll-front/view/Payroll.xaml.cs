namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.enums;
    using System;
    using System.Windows.Controls;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Payroll.xaml
    /// </summary>
    public partial class Payroll : Window
    {
        business.Payroll payrollBusiness;
        business.Employee employeeBusiness;
        ObservableCollection<PayrollDetailView> details;
        model.Payroll payroll;

        public Payroll()
        {
            InitializeComponent();
            payrollBusiness = new business.Payroll();
            employeeBusiness = new business.Employee();
            details = new ObservableCollection<PayrollDetailView>();
            payroll = new model.Payroll();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Employees.SelectedValuePath = "Id";
            Employees.DisplayMemberPath = "Fullname";
            Employees.ItemsSource = employeeBusiness.GetAll();

            Details.ItemsSource = details;

            RegularHour.Text = business.Constant.RegularHour.ToString();
        }

        private void Employees_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            TruckNumber.Text = ((Employee)e.AddedItems[0]).TruckNumber;
            Rate.Text = ((Employee)e.AddedItems[0]).Rate.ToString("C");
            PaymentMethod.Text = ((Employee)e.AddedItems[0]).PaymentMethod.ToString();
        }


        private void Details_Loaded(object sender, RoutedEventArgs e)
        {
            var sourceCollection = Details.ItemsSource as ObservableCollection<PayrollDetailView>;
            sourceCollection.CollectionChanged += SourceCollection_CollectionChanged;
        }

        private void SourceCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CalculateHours();
            ValidateTicketNumber();
        }

        private void Details_CurrentCellChanged(object sender, System.EventArgs e)
        {
            CalculateHours();
            ValidateTicketNumber();
        }

        private void CalculatePayment(int regularHours, int overtimeHour)
        {
            var rate = double.Parse(Rate.Text.Replace("$", string.Empty));
            var rateOvertime = rate * business.Constant.FactorRateOvertimeHour;
            var payment = 0.0;

            payment = (rate * regularHours) + (rateOvertime * overtimeHour);

            payroll.Rate = rate;
            payroll.Payment = payment;
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

            payroll.TotalPayment = totalPayment;
            TotalPayment.Text = totalPayment.ToString("C");
        }

        private void CalculateHours()
        {
            var totalHours = 0;
            var regularHours = 0;
            var overtimeHours = 0;

            details.ToList().ForEach(x => totalHours = totalHours + x.Hours);

            regularHours = totalHours > business.Constant.RegularHour ?
                                business.Constant.RegularHour
                                : totalHours;
            overtimeHours = totalHours > business.Constant.RegularHour ?
                                totalHours - business.Constant.RegularHour
                                : overtimeHours;

            payroll.TotalHours = totalHours;
            payroll.OvertimeHour = overtimeHours;
            TotalHours.Text = totalHours.ToString();
            OvertimeHour.Text = overtimeHours.ToString();

            CalculatePayment(regularHours, overtimeHours);
        }

        private void ValidateTicketNumber()
        {
            var isNameRepeat = false;

            details.ToList().ForEach(x =>
            {
                isNameRepeat = !isNameRepeat ?
                                    details.Where(y => x.TicketNumber == y.TicketNumber).Count() > 1
                                    : isNameRepeat;
            });

            if (isNameRepeat)
                MessageBox.Show("The ticket number is already digitized, review the information",
                                    "Ticket Number",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
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
            PaymentType paymentMethod = PaymentType.Check;
            Enum.TryParse(PaymentMethod.Text, out paymentMethod);

            payroll.PaymentType = paymentMethod;
            payroll.TruckNumber = int.Parse(TruckNumber.Text);
            payroll.Employee = (Employee)Employees.SelectedItem;
            payroll.From = FromPayment.SelectedDate.Value;
            payroll.To = ToPayment.SelectedDate.Value;
            details.ToList().ForEach(x =>
            {
                payroll.Details.Add(new PayrollDetail()
                {
                    Company = x.Company,
                    Hours = x.Hours,
                    Ticket = new Ticket() { Date = x.TicketDate, Number = x.TicketNumber }
                });
            });

            payrollBusiness.Update(payroll);
        }
    }
}