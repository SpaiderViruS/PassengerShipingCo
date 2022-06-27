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
    /// Логика взаимодействия для RegistrationPassenger.xaml
    /// </summary>
    public partial class RegistrationPassenger : Window
    {
        PassengerShippingEntities Dbcontext;

        public RegistrationPassenger()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            BirthDayDatePicker.DisplayDateEnd = DateTime.Now;
        }

        private void RegistrationPassengerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneTextBox.Text) && !string.IsNullOrEmpty(NameTextBox.Text)
                && !string.IsNullOrEmpty(SecondNameTextBox.Text) && !string.IsNullOrEmpty(LastNameTextBox.Text)
                && BirthDayDatePicker.SelectedDate != null
                && !string.IsNullOrEmpty(LoginTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                if (!CheckForOriginal())
                {
                    Passenger passenger = new Passenger();
                    passenger.Phone = PhoneTextBox.Text;
                    passenger.Name = NameTextBox.Text;
                    passenger.SecondName = SecondNameTextBox.Text;
                    passenger.LastName = LastNameTextBox.Text;
                    passenger.BirthDay = Convert.ToDateTime(BirthDayDatePicker.SelectedDate);
                    passenger.LoginPassenger = LoginTextBox.Text;
                    passenger.PasswordPassenger = PasswordTextBox.Text;

                    Dbcontext.Passenger.Add(passenger);
                    Dbcontext.SaveChanges();

                    MessageBox.Show("Вы успешно зарегистрировались!", "Уведомление",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Пользователь с таким номером/логином уже зарегистрирован",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, заполните все поля", "Уведомление", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private bool CheckForOriginal()
        {
            Passenger phone = Dbcontext.Passenger.Where(p => 
            p.Phone == PhoneTextBox.Text).FirstOrDefault();

            Passenger login = Dbcontext.Passenger.Where(p => 
            p.LoginPassenger == LoginTextBox.Text).FirstOrDefault();

            if (phone != null || login != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled= Char.IsDigit(e.Text, 0);
        }

        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
