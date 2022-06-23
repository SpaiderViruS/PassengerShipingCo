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
    /// Логика взаимодействия для CaptainRegistration.xaml
    /// </summary>
    public partial class CaptainRegistration : Window
    {
        PassengerShippingEntities Dbcontext;
        Captain Captain { get; set; }

        public CaptainRegistration(Captain captain)
        {
            InitializeComponent();

            Dbcontext = CaptainsList.Dbcontext;

            if (captain != null)
            {
                Captain = captain;
                PhoneTextBox.Text = Captain.PhoneNumberCaptain;
                NameTextBox.Text = Captain.Name;
                SecondNameTextBox.Text = Captain.SecondName;
                LastNameTextBox.Text = Captain.LastName;

                LoginTextBox.Text = Captain.LoginCaptain;
                PasswordTextBox.Text = Captain.PasswordCaptain;

                RegistrationCaptain.Content = "Редактировать капитана";
                TitleLabel.Content = "Редактирование";
                Title = "Редактирование";
            }
            else
            {
                DeleteCaptain.Visibility = Visibility.Hidden;
            }

        }

        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void NameTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Char.IsDigit(e.Text, 0);
        }

        private void RegistrationCaptain_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneTextBox.Text) && !string.IsNullOrEmpty(NameTextBox.Text)
                && !string.IsNullOrEmpty(SecondNameTextBox.Text) && !string.IsNullOrEmpty(LastNameTextBox.Text)
                && !string.IsNullOrEmpty(LoginTextBox.Text) && !string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                if (Captain != null)
                {
                    Captain.Name = NameTextBox.Text;
                    Captain.SecondName = SecondNameTextBox.Text;
                    Captain.LastName = LastNameTextBox.Text;
                    Captain.PhoneNumberCaptain = PhoneTextBox.Text;
                    Captain.LoginCaptain = LoginTextBox.Text;
                    Captain.PasswordCaptain = PasswordTextBox.Text;

                    Dbcontext.SaveChanges();

                    MessageBox.Show("Капитан отредактирован", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    Close();
                }
                else
                {
                    if (!CheckForOriginal())
                    {
                        Captain newCaptain = new Captain();

                        newCaptain.Name = NameTextBox.Text;
                        newCaptain.SecondName = SecondNameTextBox.Text;
                        newCaptain.LastName = LastNameTextBox.Text;
                        newCaptain.PhoneNumberCaptain = PhoneTextBox.Text;
                        newCaptain.LoginCaptain = LoginTextBox.Text;
                        newCaptain.PasswordCaptain = PasswordTextBox.Text;

                        Dbcontext.Captain.Add(newCaptain);
                        Dbcontext.SaveChanges();

                        MessageBox.Show("Капитан успешно зарегистрирован", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Капитан с таким номером/логином уже зарегистрирован",
                        "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
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
            Captain phone = Dbcontext.Captain.Where(p =>
            p.PhoneNumberCaptain == PhoneTextBox.Text).FirstOrDefault();

            Captain login = Dbcontext.Captain.Where(p =>
            p.LoginCaptain == LoginTextBox.Text).FirstOrDefault();

            if (phone != null || login != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void DeleteCaptain_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить капитана?\n" +
                "Внимание туры, круизы и корабль к которым он приписан, будут удалены",
                "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Dbcontext.Captain.Remove(Captain);
                Dbcontext.SaveChanges();

                MessageBox.Show("Капитан успешно удален", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                return;
            }
        }
    }
}
