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
    /// Логика взаимодействия для UpdateInsertTour.xaml
    /// </summary>
    public partial class UpdateInsertTour : Window
    {
        PassengerShippingEntities Dbcontext;
        Tour Tour { get; set; }

        public UpdateInsertTour(Tour tour )
        {
            InitializeComponent();
            Tour = tour;
            Dbcontext = TourList.DbContext;

            ShipComboBox.ItemsSource = Dbcontext.Ship.ToList();

            if (Tour != null)
            {
                ShipComboBox.SelectedIndex = Dbcontext.Ship.ToList().IndexOf(Tour.Ship);

                DepartureDatePicker.SelectedDate = Tour.DepartureTime;
                ArrivalDatePicker.SelectedDate = Tour.ArrivalTime;

                LoadCruises();
            }
            else
            {
                Tour = new Tour();
                UpdateInsertTourBtn.Content = "Добавить тур";
                DeleteTourBtn.Visibility = Visibility.Hidden;
                LoadCruises();
            }
            DepartureDatePicker.DisplayDateStart = DateTime.Now;
            ArrivalDatePicker.DisplayDateStart = DateTime.Now;
        }

        private void LoadCruises()
        {
            List<Cruise> displayCruise = new List<Cruise>();
            displayCruise = Dbcontext.Cruise.ToList();

            List<string> DeparturePorts = new List<string>();
            List<string> ArrivalPorts = new List<string>();
            List<string> normalPorts = new List<string>();

            foreach (Cruise cruise in displayCruise)
            {
                DeparturePorts.Add(cruise.Port1.NamePort);
                ArrivalPorts.Add(cruise.Port.NamePort);
            }
            for (int i = 0; i < DeparturePorts.Count(); i++)
            {
                string tempString;
                tempString = $"{DeparturePorts[i]} - {ArrivalPorts[i]}";
                normalPorts.Add(tempString);
            }

            foreach (string s in normalPorts)
            {
                PortsComboBox.Items.Add(s);
            }
            if (Tour != null)
            {
                PortsComboBox.SelectedIndex = Tour.CruiseID - 1;
            }
            else
            {
                PortsComboBox.SelectedIndex = -1;
            }

        }

        private void UpdateInsertTour_Click(object sender, RoutedEventArgs e)
        {
            if (ShipComboBox.SelectedIndex != -1 && PortsComboBox.SelectedIndex != -1
                && DepartureDatePicker.SelectedDate != null && ArrivalDatePicker.SelectedDate != null)
            {
                if (DepartureDatePicker.SelectedDate < ArrivalDatePicker.SelectedDate)
                {
                    Ship tempShip = Dbcontext.Ship.Where(c =>
                    ShipComboBox.Text.Contains(c.NameShip)).FirstOrDefault();

                    Cruise tempCruise = Dbcontext.Cruise.Where(c =>
                    PortsComboBox.Text.Contains(c.Port.NamePort) && 
                    PortsComboBox.Text.Contains(c.Port1.NamePort)).FirstOrDefault();

                    int CruiseID = tempCruise.ID;
                    int ShipID = tempShip.ID;

                    if (UpdateInsertTourBtn.Content.ToString() == "Изменить тур")
                    {
                        Tour.ShipID = ShipID;
                        Tour.CruiseID = CruiseID;
                        Tour.DepartureTime = Convert.ToDateTime(DepartureDatePicker.SelectedDate);
                        Tour.ArrivalTime = Convert.ToDateTime(ArrivalDatePicker.SelectedDate);

                        Dbcontext.SaveChanges();
                        MessageBox.Show("Тур успешно изменен!", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    else
                    {
                        Tour.ShipID = ShipID;
                        Tour.CruiseID = CruiseID;
                        Tour.DepartureTime = Convert.ToDateTime(DepartureDatePicker.SelectedDate);
                        Tour.ArrivalTime = Convert.ToDateTime(ArrivalDatePicker.SelectedDate);

                        Dbcontext.Tour.Add(Tour);
                        Dbcontext.SaveChanges();
                        MessageBox.Show("Тур успешно добавлен!", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Дата прибытия не может быть раньше даты отбытия", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!","Информация", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        private void DeleteTour_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот тур?", "Вопрос",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Dbcontext.Tour.Remove(Tour);
                Dbcontext.SaveChanges();
                MessageBox.Show("Тур успешно удален!", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                return;
            }
        }

        private void AddNewCruise_Click(object sender, RoutedEventArgs e)
        {
            EditCruise editCruiser = new EditCruise(null);
            editCruiser.ShowDialog();
            PortsComboBox.Items.Clear();
            LoadCruises();
        }
    }
}
