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
    /// Логика взаимодействия для CaptainsList.xaml
    /// </summary>
    public partial class CaptainsList : Window
    {
        public static PassengerShippingEntities Dbcontext;

        public CaptainsList()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            UpdateCaptainsListView();
        }

        private void UpdateCaptainsListView()
        {
            CaptainsListView.Items.Clear();

            List<Captain> displayCaptains = new List<Captain>();
            displayCaptains = Dbcontext.Captain.ToList();

            foreach (Captain captain in displayCaptains)
            {
                CaptainsListView.Items.Add($"{captain.Name} {captain.SecondName} {captain.LastName}");
            }
        }

        private void AddNewCaptainBtn_Click(object sender, RoutedEventArgs e)
        {
            CaptainRegistration captainRegistration = new CaptainRegistration(null);
            captainRegistration.ShowDialog();

            UpdateCaptainsListView();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CaptainsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Captain selectedCaptain = Dbcontext.Captain.Where(c => 
            CaptainsListView.SelectedItem.ToString().Contains(c.Name) &&
            CaptainsListView.SelectedItem.ToString().Contains(c.SecondName)).FirstOrDefault();

            CaptainRegistration captainRegistration = new CaptainRegistration(selectedCaptain);
            captainRegistration.ShowDialog();

            UpdateCaptainsListView();
        }
    }
}
