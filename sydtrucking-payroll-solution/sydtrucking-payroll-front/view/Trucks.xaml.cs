namespace sydtrucking_payroll_front.view
{
    using System.Windows;
    using System.Linq;
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class Trucks : Window
    {
        private List<Truck> _trucksModel;
        private business.Truck _truckBusiness;
        private string _idTruckSelected;

        public Trucks()
        {
            InitializeComponent();
            _trucksModel = new List<Truck>();
            _truckBusiness = new business.Truck();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Clear();
            LoadTrucks();
        }

        private void LoadTrucks()
        {
            _trucksModel = _truckBusiness.GetAll();
            ListTrucks.ItemsSource = _trucksModel;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var id = _truckBusiness.GetAll()
                          .Where(x => x.Number == Number.Text)
                          .DefaultIfEmpty(new Truck() { Id = string.Empty })
                          .FirstOrDefault()
                          .Id;

            Truck truck = new Truck()
            {
                Id = id,
                Inspection = Inspection.SelectedDate.Value,
                Make = Make.Text,
                Number = Number.Text,
                Plate = Plate.Text,
                Registration = Registration.SelectedDate.Value,
                Vin = Vin.Text,
                Year = int.Parse(Year.Text)
            };

            _truckBusiness.Update(truck);
            LoadTrucks();
        }

        private void ListTrucks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(Truck))
            {
                LoadTruck((Truck)e.AddedItems[0]);
            }
        }

        private void LoadTruck(Truck truck)
        {
            _idTruckSelected = truck.Id;
            Number.Text = truck.Number;
            Year.Text = truck.Year.ToString();
            Vin.Text = truck.Vin;
            Make.Text = truck.Make;
            Plate.Text = truck.Plate;
            Registration.SelectedDate = truck.Registration;
            Inspection.SelectedDate = truck.Inspection;
        }

        private void Clear()
        {
            Number.Text = string.Empty;
            Year.Text = string.Empty;
            Vin.Text = string.Empty;
            Make.Text = string.Empty;
            Plate.Text = string.Empty;
            Registration.SelectedDate = null;
            Inspection.SelectedDate = null;
        }
    }
}
