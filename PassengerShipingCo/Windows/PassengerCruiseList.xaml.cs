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
    /// Логика взаимодействия для PassengerCruiseList.xaml
    /// </summary>
    public partial class PassengerCruiseList : Window
    {
        PassengerShippingEntities Dbcontext;

        public PassengerCruiseList()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            UpdatePassengerCruiseList();
        }

        private void UpdatePassengerCruiseList()
        {
            PassengerCruiseListView.Items.Clear();

            List<PassengerCruise> displayPassengerCrs = new List<PassengerCruise>();
            displayPassengerCrs = Dbcontext.PassengerCruise.ToList();

            if (!string.IsNullOrEmpty(SearchPassengerCruiseTextBox.Text))
            {
                displayPassengerCrs = displayPassengerCrs.Where(p => 
                p.Passenger.SecondName.ToLower().Contains(SearchPassengerCruiseTextBox.Text.ToLower())).ToList();
            }

            foreach (PassengerCruise passengerCruise in displayPassengerCrs)
            {
                PassengerCruiseListView.Items.Add(new PassengerCruiseControl(passengerCruise)
                {
                    Width = GetWidth()
                });
            }
        }

        private double GetWidth()
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

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SearchPassengerCruiseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePassengerCruiseList();
        }
    }
}
