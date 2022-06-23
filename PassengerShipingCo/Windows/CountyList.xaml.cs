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
    /// Логика взаимодействия для CountyList.xaml
    /// </summary>
    public partial class CountyList : Window
    {
        PassengerShippingEntities Dbcontext;

        public CountyList()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            UpdateCountysListView();
        }

        private void UpdateCountysListView()
        {
            CountrysListView.Items.Clear();

            List<Countries> displayCountys = new List<Countries>();
            displayCountys = Dbcontext.Countries.ToList();

            foreach(Countries countries in displayCountys)
            {
                CountrysListView.Items.Add(countries.NameCountry);
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CountrysListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CountrysListView.SelectedItem != null)
            {
                Countries selectedCounry = Dbcontext.Countries.Where(x => 
                CountrysListView.SelectedItem.ToString().Contains(x.NameCountry)).FirstOrDefault();

                CountrysPortsTextBlock.Text = $"Предписанные порты: \n" +
                    $"{ String.Join("\n", selectedCounry.Port.Select(s => s.NamePort).ToList())}";
            }
        }
    }
}
