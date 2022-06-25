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
    /// Логика взаимодействия для TourControl.xaml
    /// </summary>
    public partial class TourControl : UserControl
    {
        public Tour Tour { get; set; }

        public TourControl(Tour tour)
        {
            InitializeComponent();
            Tour = tour;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLabels();
            LoadImages();
        }

        private void LoadLabels()
        {
            DeparturePortLabel.Content = $"Откуда: {Tour.Cruise.Port1.NamePort}";
            ArrivalPortLabel.Content = $"Куда: {Tour.Cruise.Port.NamePort}";

            DepartureTimeLabel.Content = $"Дата отбытия: {Tour.DepartureTime.ToShortDateString()}";
            ArrivalTimeLabel.Content = $"Дата приытия: {Tour.ArrivalTime.ToShortDateString()}";

            TimeSpan tempTS = Tour.ArrivalTime - Tour.DepartureTime;

            if (Tour.DepartureTime < DateTime.Now)
            {
                CruiseNotAvailable();
            }

            TotalTimeInTravelLabel.Content = $"Время в пути: {tempTS.TotalHours}ч";

            CostLabel.Content = $"Цена: {Tour.Cruise.CostCruise} ₽";
        }

        private void LoadImages()
        {
            if (Tour.Ship.ImageShip != null && Tour.Ship.ImageShip.Length > 0)
            {
                Uri resourceUri = new Uri(Environment.CurrentDirectory + Tour.Ship.ImageShip);
                ShipImage.Source = new BitmapImage(resourceUri);
            }
            else
            {
                ShipImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/picture.png"));
            }
        }

        private void CruiseNotAvailable()
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff3333"));
        }
    }
}
