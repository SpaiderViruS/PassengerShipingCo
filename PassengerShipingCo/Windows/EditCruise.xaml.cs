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
    /// Логика взаимодействия для EditCruise.xaml
    /// </summary>
    public partial class EditCruise : Window
    {
        PassengerShippingEntities Dbcontext;
        Cruise Cruise { get; set; }


        public EditCruise(Cruise cruise)
        {
            InitializeComponent();
            Cruise = cruise;
            Dbcontext = TourList.DbContext;

            PortDepartureComboBox.ItemsSource = Dbcontext.Port.ToList();
            PortArrivalComboBox.ItemsSource = Dbcontext.Port.ToList();

            if (Cruise != null)
            {
                PortDepartureComboBox.SelectedIndex = Dbcontext.Port.ToList().IndexOf(Cruise.Port1);
                PortArrivalComboBox.SelectedIndex = Dbcontext.Port.ToList().IndexOf(Cruise.Port);
                CostTextBox.Text = Cruise.CostCruise.ToString();
            }
            else
            {
                Cruise = new Cruise();
                EditCruiseBtn.Content = "Добавить тур";
                Title = "Добавить тур";
                DelteCruiseBtn.Visibility = Visibility.Hidden;
            }
        }

        private void EditCruiseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CostTextBox.Text) && PortArrivalComboBox.SelectedIndex != -1 
                && PortDepartureComboBox.SelectedIndex != -1)
            {
                if (EditCruiseBtn.Content.ToString() == "Внести измения")
                {
                    Cruise.PortDepartureID = PortDepartureComboBox.SelectedIndex + 1;
                    Cruise.PortArrivalID = PortArrivalComboBox.SelectedIndex + 1;
                    Cruise.CostCruise = Convert.ToInt32(CostTextBox.Text);

                    Dbcontext.SaveChanges();
                    MessageBox.Show("Круиз изменён!", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    Cruise.PortDepartureID = PortDepartureComboBox.SelectedIndex + 1;
                    Cruise.PortArrivalID = PortArrivalComboBox.SelectedIndex + 1;
                    Cruise.CostCruise = Convert.ToInt32(CostTextBox.Text);

                    Dbcontext.Cruise.Add(Cruise);
                    Dbcontext.SaveChanges();
                    MessageBox.Show("Криуз добавлен!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DelteCruiseBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Вы действительно хотите удалить круиз?", "Вопрос",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Dbcontext.Cruise.Remove(Cruise);
                Dbcontext.SaveChanges();
                Close();
            }
            else
            {
                return;
            }
        }

        private void CostTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void CostTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
