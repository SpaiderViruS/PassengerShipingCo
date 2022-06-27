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
using PassengerShipingCo.UserControls;

namespace PassengerShipingCo.Windows
{
    /// <summary>
    /// Логика взаимодействия для ShipList.xaml
    /// </summary>
    public partial class ShipList : Window
    {
        public static PassengerShippingEntities Dbcontext;

        public ShipList()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateShipsListView();
        }

        private void UpdateShipsListView()
        {
            ShipsListView.Items.Clear();

            List<Ship> displayShips = new List<Ship>();
            displayShips = Dbcontext.Ship.ToList();

            foreach (Ship ship in displayShips)
            {
                ShipsListView.Items.Add(new ShipControl(ship)
                {
                    Width = GetOprimizedWidth()
                });
            }
        }

        private double GetOprimizedWidth()
        {
            if (WindowState == WindowState.Maximized)
            {
                return RenderSize.Width - 55;
            }
            else
            {
                return Width - 55;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateShipsListView();
        }

        private void ShipsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ShipsListView.SelectedItem != null)
            {
                Ship ship = ((ShipControl)ShipsListView.SelectedItem).Ship;
                AddEditShip addEditShip = new AddEditShip(ship);
                addEditShip.ShowDialog();
            }
            UpdateShipsListView();
        }

        private void AddNewShipBtn_Click(object sender, RoutedEventArgs e)
        {
            AddEditShip addEditShip = new AddEditShip(null);
            addEditShip.ShowDialog();
            UpdateShipsListView();
        }
    }
}
