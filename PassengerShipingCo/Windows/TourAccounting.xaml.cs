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
    /// Логика взаимодействия для TourAccounting.xaml
    /// </summary>
    public partial class TourAccounting : Window
    {
        PassengerShippingEntities Dbcontext;

        bool DateDepartureChanged = false;
        bool DateArrivalChanged = false;

        int TotalProfitForPeriod;

        public TourAccounting()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            DepartureDatePicker.DisplayDateStart = DateTime.Now;
            ArrivalDatePicker.DisplayDateStart = DateTime.Now;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void UpdateAccountingListView()
        {
            TourAccountingListView.Items.Clear();

            List<Tour> displayTours = new List<Tour>();
            displayTours = Dbcontext.Tour.ToList();

            if (DateArrivalChanged && DateDepartureChanged)
            {
                if (ArrivalDatePicker.SelectedDate != null && 
                    DepartureDatePicker.SelectedDate != null)
                {
                    displayTours = displayTours.Where(t =>
                    t.DepartureTime >= DepartureDatePicker.SelectedDate &&
                    t.ArrivalTime <= ArrivalDatePicker.SelectedDate).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(DepartureTextBox.Text))
            {
                displayTours = displayTours.Where(t =>
                t.Cruise.Port1.NamePort.ToLower().Contains(DepartureTextBox.Text.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(ArrivalTextBox.Text))
            {
                displayTours = displayTours.Where(t =>
                t.Cruise.Port.NamePort.ToLower().Contains(ArrivalTextBox.Text.ToLower())).ToList();
            }

            foreach (Tour tour in displayTours)
            {
                TourAccountingListView.Items.Add(new TourAccountingControl(tour)
                {
                    Width = GetOptimizedWidth()
                });
                TotalProfitForPeriod += tour.NumberOfTicketSold * tour.Cruise.CostCruise;
            }

            TotalProfitForPeriodLabel.Content = $"Прибыль за выбранный период {TotalProfitForPeriod} ₽";
            TotalProfitForPeriod = 0;
        }

        private double GetOptimizedWidth()
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
            UpdateAccountingListView();
        }

        private void DepartureDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateDepartureChanged = true;
            UpdateAccountingListView();
        }

        private void ArrivalDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateArrivalChanged = true;
            UpdateAccountingListView();
        }

        private void DepartureTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void ArrivalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
