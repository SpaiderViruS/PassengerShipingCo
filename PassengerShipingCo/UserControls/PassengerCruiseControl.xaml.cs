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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PassengerShipingCo.Entity;

namespace PassengerShipingCo.UserControls
{
    /// <summary>
    /// Логика взаимодействия для PassengerCruiseControl.xaml
    /// </summary>
    public partial class PassengerCruiseControl : UserControl
    {
        public PassengerCruise PassengerCruise { get; set; }

        public PassengerCruiseControl(PassengerCruise passengerCruise)
        {
            InitializeComponent();
            PassengerCruise = passengerCruise;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLabels();
        }

        private void LoadLabels()
        {
            SNPLabel.Content = $"Пассажир: " +
                $"{PassengerCruise.Passenger.Name} {PassengerCruise.Passenger.SecondName} {PassengerCruise.Passenger.LastName}";
            TourLabel.Content = $"Тур: {PassengerCruise.Tour.Cruise.Port1.NamePort} - {PassengerCruise.Tour.Cruise.Port.NamePort}";
        }
    }
}
