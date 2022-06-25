using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PassengerShipingCo.Entity;

namespace PassengerShipingCo.Windows
{
    /// <summary>
    /// Логика взаимодействия для CurrentPassengerCruisesList.xaml
    /// </summary>
    public partial class CurrentPassengerCruisesList : Window
    {
        PassengerShippingEntities Dbcontext;
        Passenger Passenger { get; set; }
        PassengerCruise PassengerCruise { get; set; }

        public CurrentPassengerCruisesList(Passenger passenger)
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            Passenger = passenger;

            PassengerCruise = Dbcontext.PassengerCruise.Where(p =>
            p.PassengerID == Passenger.ID).FirstOrDefault();

            if (PassengerCruise != null)
            {
                UpdateCruisesListView();
            }
            else
            {
                NoCruisesLabel.Visibility = Visibility.Visible;
            }
        }

        private void UpdateCruisesListView()
        {
            CurrentPassengerCruiseListView.Items.Clear();

            List<PassengerCruise> passengerCruises = new List<PassengerCruise>();
            passengerCruises = Dbcontext.PassengerCruise.Where(p =>
            p.PassengerID == PassengerCruise.PassengerID).ToList();

            foreach(PassengerCruise pcr in passengerCruises)
            {
                TimeSpan toDeparture = pcr.Tour.DepartureTime - DateTime.Now;

                string listLine = $"Круиз: {pcr.Tour.Cruise.Port1.NamePort} - {pcr.Tour.Cruise.Port.NamePort}\n" +
                    $"Время отбытия: {pcr.Tour.DepartureTime.ToShortDateString()}\n" +
                    $"До отбытия осталось: {toDeparture.TotalHours:F0}ч";

                CurrentPassengerCruiseListView.Items.Add(listLine);
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
