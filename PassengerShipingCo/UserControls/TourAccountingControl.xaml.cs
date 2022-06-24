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
    /// Логика взаимодействия для TourAccountingControl.xaml
    /// </summary>
    public partial class TourAccountingControl : UserControl
    {
        public Tour Tour { get; set; } 

        public TourAccountingControl(Tour tour)
        {
            InitializeComponent();

            Tour = tour;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadLabels();
        }

        private void LoadLabels()
        {
            DepartureArrivalLabel.Content = $"Криуз: {Tour.Cruise.Port1.NamePort} - {Tour.Cruise.Port.NamePort}";
            NumberSoldTicketsLabel.Content = $"Кол - во проданных билетов: {Tour.NumberOfTicketSold}";

            int profit = Tour.NumberOfTicketSold * Tour.Cruise.CostCruise;

            ProfitLabel.Content = $"Прибыль тура: {profit} ₽";
        }
    }
}
