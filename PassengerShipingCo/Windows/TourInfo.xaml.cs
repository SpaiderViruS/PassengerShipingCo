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
    /// Логика взаимодействия для TourInfo.xaml
    /// </summary>
    public partial class TourInfo : Window
    {
        public static PassengerShippingEntities Dbcontext;
        Tour Tour { get; set; }
        Passenger Passenger { get; set; }

        public TourInfo(Tour tour, Passenger passenger)
        {
            InitializeComponent();

            Tour = tour;
            Dbcontext = new PassengerShippingEntities();

            if (passenger != null)
            {
                Passenger = passenger;
                CheckPassengersCruises();
            }

            LoadLabels();
            LoadImage();
        }

        private void LoadLabels()
        {
            DeparturePortLabel.Content = $"Откуда: {Tour.Cruise.Port1.NamePort}";
            ArrivalPortLabel.Content = $"Куда: {Tour.Cruise.Port.NamePort}";
            CountryArrivalLabel.Content = $"Страна прибытия: {Tour.Cruise.Port.Countries.NameCountry}";
            CostLabel.Content = $"Цена путешествия: {Tour.Cruise.CostCruise} р.";
        }

        private void LoadImage()
        {
            if (Tour.Ship.ImageShip != null && Tour.Ship.ImageShip.Length > 0)
            {
                ShipImage.Source = new BitmapImage(new Uri($"../Images/{Tour.Ship.ImageShip}", UriKind.Relative));
            }
            else
            {
                ShipImage.Source = new BitmapImage(new Uri($"../Images/picture.png", UriKind.Relative));
            }
        }

        private void SignupCruiseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите записаться на круиз?", "Вопрос",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                PassengerCruise passengerCruise = new PassengerCruise();
                passengerCruise.PassengerID = Passenger.ID;
                passengerCruise.TourID = Tour.ID;

                Dbcontext.PassengerCruise.Add(passengerCruise);
                Dbcontext.SaveChanges();

                SignupCruiseBtn.IsEnabled = false;

                MessageBox.Show($"Вы записались на круиз {Tour.Cruise.Port1} - {Tour.Cruise.Port}\nВремя отбытия {Tour.DepartureTime}", 
                    "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                return;
            }
        }

        private void CheckPassengersCruises()
        {
            PassengerCruise passengerCruise = Dbcontext.PassengerCruise.Where(p => p.PassengerID == Passenger.ID).FirstOrDefault();
            if (passengerCruise != null)
            {
                if (passengerCruise.Tour.DepartureTime == Tour.DepartureTime)
                {
                    SignupCruiseBtn.IsEnabled = false;
                }
                else if (passengerCruise.TourID == Tour.ID)
                {
                    SignupCruiseBtn.IsEnabled = false;
                }
            }
        }
    }
}
