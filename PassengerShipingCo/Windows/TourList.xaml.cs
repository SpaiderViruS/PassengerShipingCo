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
    /// Логика взаимодействия для TourList.xaml
    /// </summary>
    public partial class TourList : Window
    {
        public static PassengerShippingEntities DbContext;
        Passenger Passenger;
        Captain Captain;
        bool DateChanged;

        public TourList(Passenger passenger)
        {
            InitializeComponent();
            DbContext = new PassengerShippingEntities();

            if (passenger != null)
            {
                Passenger = passenger;
                SNPLabel.Content = $"Пассажир {Passenger.SecondName} {Passenger.Name} {Passenger.LastName}";
                MenuCaptainBtn.Visibility = Visibility.Collapsed;
                MessageBox.Show("Passenger");
            }

            DepartureDatePicker.DisplayDateStart = DateTime.Now;

            DateChanged = false;
        }
        public TourList(Captain captain)
        {
            InitializeComponent();

            DbContext = new PassengerShippingEntities();

            if (captain != null)
            {
                Captain = captain;
                SNPLabel.Content = $"Капитан {Captain.SecondName} {Captain.Name} {Captain.LastName}";
                MessageBox.Show("Captain");
            }

            DepartureDatePicker.DisplayDateStart = DateTime.Now;

            DateChanged = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTourList();
        }

        private void UpdateTourList()
        {
            TourListView.Items.Clear();

            List<Tour> dtours = new List<Tour>();
            dtours = DbContext.Tour.ToList();

            if (!string.IsNullOrEmpty(ArrivalPortTextBox.Text))
            {
                dtours = dtours.Where(t => 
                t.Cruise.Port.NamePort.ToLower().Contains(ArrivalPortTextBox.Text.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(DeparturePortTextBox.Text))
            {
                dtours = dtours.Where(t =>
                t.Cruise.Port1.NamePort.ToLower().Contains(DeparturePortTextBox.Text.ToLower())).ToList();
            }
            if (DateChanged)
            {
                if (DepartureDatePicker.SelectedDate != null) 
                { 
                    dtours = dtours.Where(t =>
                    t.DepartureTime.Date >= DepartureDatePicker.SelectedDate).ToList();
                }
            }

            foreach(Tour tour in dtours)
            {
                TourListView.Items.Add(new TourControl(tour)
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
            UpdateTourList();
        }

        private void DeparturePortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTourList();
        }

        private void ArrivalPortTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTourList();
        }

        private void DepartureDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateChanged = true;
            UpdateTourList();
        }

        private void TourListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TourListView.SelectedItem != null)
            {
                Tour tour = ((TourControl)TourListView.SelectedItem).Tour;

                if (Passenger != null)
                {
                    TourInfo tourInfo = new TourInfo(tour, Passenger);
                    tourInfo.ShowDialog();
                }
                else if (Captain != null)
                {
                    UpdateInsertTour updateInsertTour = new UpdateInsertTour(tour);
                    updateInsertTour.ShowDialog();
                }


                else
                {
                    TourInfo tourInfo = new TourInfo(tour, null);
                    tourInfo.ShowDialog();
                }
            }
            UpdateTourList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void MenuCaptainBtn_Click(object sender, RoutedEventArgs e)
        {
            CaptainMenu captainMenu = new CaptainMenu();
            Hide();
            captainMenu.ShowDialog();
            Show();
            UpdateTourList();
        }
    }
}
