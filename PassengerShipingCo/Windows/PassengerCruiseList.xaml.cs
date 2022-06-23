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
    /// Логика взаимодействия для PassengerCruiseList.xaml
    /// </summary>
    public partial class PassengerCruiseList : Window
    {
        PassengerShippingEntities Dbcontext;

        public PassengerCruiseList()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            List<PassengerCruise> displayPassengerCrs = new List<PassengerCruise>();
            displayPassengerCrs = Dbcontext.PassengerCruise.ToList();

            PassengerCruiseDataGrid.ItemsSource = Dbcontext.PassengerCruise.Local;

            //foreach (PassengerCruise passengerCruise in displayPassengerCrs)
            //{
            //    PassengerCruiseDataGrid.ItemsSource = passengerCruise;
            //}
        }
    }
}
