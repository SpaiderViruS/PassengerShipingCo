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
    /// Логика взаимодействия для ShipControl.xaml
    /// </summary>
    public partial class ShipControl : UserControl
    {
        public Ship Ship { get; set; }

        public ShipControl(Ship ship)
        {
            InitializeComponent();
            Ship = ship;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLabels();
            LoadImage();
        }

        private void LoadLabels()
        {
            CaptainShipLabel.Content = $"Капитан корабля: {Ship.Captain.Name} {Ship.Captain.SecondName} {Ship.Captain.LastName}";
            NameShipLabel.Content = $"Имя корабля {Ship.NameShip}";
            NameShipLabel.FontSize = 24;

            LoadCapacityLabel.Content = $"Вес {Ship.LoadCapacity}";
            SeatsLabel.Content = $"Кол - во мест {Ship.Seats}";
        }

        private void LoadImage()
        {
            if (Ship.ImageShip != null && Ship.ImageShip.Length > 0)
            {
                Uri resourceUri = new Uri(Environment.CurrentDirectory + Ship.ImageShip);
                ShipImage.Source = new BitmapImage(resourceUri);
            }
            else
            {
                ShipImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/picture.png"));
            }
        }
    }
}
