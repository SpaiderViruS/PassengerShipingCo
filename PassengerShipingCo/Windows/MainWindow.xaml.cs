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
using PassengerShipingCo.Windows;

namespace PassengerShipingCo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PassengerShippingEntities Dbcontext;

        public MainWindow()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LoginTextBox.Text)
                && !string.IsNullOrEmpty(PasswordPasswordBox.Password))
            {
                if (ShowPasswordCheckBox.IsChecked == true)
                {
                    PasswordPasswordBox.Password = CheckPasswordTextBox.Text;
                }

                Passenger passenger = Dbcontext.Passenger.Where(p => p.LoginPassenger == LoginTextBox.Text.Trim()
                && p.PasswordPassenger == PasswordPasswordBox.Password).FirstOrDefault();

                if (passenger != null)
                {
                    TourList tourList = new TourList(passenger);
                    tourList.Show();
                    Close();
                }
                else
                {
                    Captain captain = Dbcontext.Captain.Where(p => p.LoginCaptain == LoginTextBox.Text.Trim()
                    && p.PasswordCaptain == PasswordPasswordBox.Password).FirstOrDefault();

                    if (captain != null)
                    {
                        TourList tourList = new TourList(captain);
                        tourList.Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин/пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RegistrationBtn_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPassenger registrationPassenger = new RegistrationPassenger();
            registrationPassenger.ShowDialog();
        }

        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ShowPasswordCheckBox.IsChecked == true)
            {
                CheckPasswordTextBox.Text = PasswordPasswordBox.Password;

                CheckPasswordTextBox.Visibility = Visibility.Visible;
                PasswordPasswordBox.Visibility = Visibility.Hidden;
            }
        }

        private void ShowPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ShowPasswordCheckBox.IsChecked == false)
            {
                PasswordPasswordBox.Password = CheckPasswordTextBox.Text;

                CheckPasswordTextBox.Visibility = Visibility.Hidden;
                PasswordPasswordBox.Visibility = Visibility.Visible;
            }
        }
    }
}
