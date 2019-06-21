namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.model;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// Lógica de interacción para Employees.xaml
    /// </summary>
    public partial class Trucks : Window, IView<Truck>
    {
        private List<Truck> _trucksModel;
        private business.IBusiness<Truck> _truckBusiness;
        private string _idTruckSelected;

        public Trucks()
        {
            InitializeComponent();
            _trucksModel = new List<Truck>();
            _truckBusiness = new business.Truck();
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

        private void ListTrucks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count>0 && e.AddedItems[0].GetType() == typeof(Truck))
            {
                LoadDataBySelectedRow((Truck)e.AddedItems[0]);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditView();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            CreateView();
        }

        public void ClearView()
        {
            Number.Text = string.Empty;
            Year.Text = string.Empty;
            Vin.Text = string.Empty;
            Make.Text = string.Empty;
            Plate.Text = string.Empty;
            Registration.SelectedDate = null;
            Inspection.SelectedDate = null;
        }

        public void CreateView()
        {
            ChangeControlsEnabled(true);
            ClearView();
        }

        public void SaveView()
        {
            ChangeControlsEnabled(false);

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
            MessageBox.Show("The information was correctly saved!", "Save", MessageBoxButton.OK, MessageBoxImage.Information);

            ClearView();
            FillGrid();
        }

        public void FillGrid()
        {
            _trucksModel = _truckBusiness.GetAll();
            ListTrucks.ItemsSource = _trucksModel;
        }

        public void EditView()
        {
            ChangeControlsEnabled(true);
        }

        public void LoadDataBySelectedRow(Truck truck)
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

        public void ChangeControlsEnabled(bool isEnable)
        {
            General.IsEnabled = isEnable;
            Save.IsEnabled = isEnable;
            New.IsEnabled = !isEnable;
        }
    }
}
