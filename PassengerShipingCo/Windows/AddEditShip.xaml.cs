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
using Microsoft.Win32;
using PassengerShipingCo.Entity;
using System.IO;

namespace PassengerShipingCo.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditShip.xaml
    /// </summary>
    public partial class AddEditShip : Window
    {
        PassengerShippingEntities Dbcontext;
        public Ship Ship { get; set; }
        OpenFileDialog openFileDialog = new OpenFileDialog();
        string photoPath = null;

        public AddEditShip(Ship ship)
        {
            InitializeComponent();
            Ship = ship;
            Dbcontext = ShipList.Dbcontext;
            openFileDialog.Filter = "Png files (*png)|*.png|Jpg files(*jpg)|*.jpg|All files(*)|*.*";

            if (Ship != null)
            {
                NameShipTextBox.Text = Ship.NameShip;
                LoadCapacityTextBx.Text = Ship.LoadCapacity;
                SeatsTextBox.Text = Ship.Seats.ToString();

                Title = "Редактировать судно";

                LoadImageShip();
            }
            else
            {
                AddShipBtn.Content = "Добавить судно";
                DeleteShipBtn.Visibility = Visibility.Hidden;

                LoadImageNullShip();
            }

            LoadComboBox();
        }

        private void LoadComboBox()
        {
            List<Captain> displayCaptain = new List<Captain>();
            displayCaptain = Dbcontext.Captain.ToList();

            List<string> NameCaptain = new List<string>();
            List<string> SurnameCaptain = new List<string>();
            List<string> PatronomicCaptain = new List<string>();

            List<string> SNPCaptain = new List<string>();

            foreach (Captain captain in displayCaptain)
            {
                NameCaptain.Add(captain.Name);
                SurnameCaptain.Add(captain.SecondName);
                PatronomicCaptain.Add(captain.LastName);
            }
            for (int i = 0; i < SurnameCaptain.Count(); i++)
            {
                string tempString;
                tempString = $"{NameCaptain[i]} {SurnameCaptain[i]} {PatronomicCaptain[i]}";
                SNPCaptain.Add(tempString);
            }

            foreach (string s in SNPCaptain)
            {
                CaptainsComboBox.Items.Add(s);
            }
            if (Ship != null)
            {
                CaptainsComboBox.SelectedIndex = Ship.CaptainID - 1;
            }
            else
            {
                CaptainsComboBox.SelectedIndex = -1;
            }
        }

        private void AddShipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CaptainsComboBox.SelectedIndex != -1 && !string.IsNullOrWhiteSpace(NameShipTextBox.Text)
                && !string.IsNullOrEmpty(LoadCapacityTextBx.Text) && !string.IsNullOrEmpty(SeatsTextBox.Text))
            {
                Captain tempCaptain = Dbcontext.Captain.Where(c => 
                CaptainsComboBox.Text.Contains(c.SecondName)).FirstOrDefault();
                int CaptainID = tempCaptain.ID;

                if (AddShipBtn.Content.ToString() == "Редактировать судно")
                {

                    if (photoPath != null)
                    {
                        string photoName = "\\Images\\" + System.IO.Path.GetRandomFileName() + ".jpg";
                        File.Copy(photoPath, Environment.CurrentDirectory + photoName, true);
                        Ship.ImageShip = photoName;
                    }

                    Ship.NameShip = NameShipTextBox.Text;
                    Ship.CaptainID = CaptainID;
                    Ship.Seats = Convert.ToInt32(SeatsTextBox.Text);
                    Ship.LoadCapacity = LoadCapacityTextBx.Text;

                    Dbcontext.SaveChanges();

                    MessageBox.Show("Судно успешно отредактировано", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
                else
                {
                    Ship newShip = new Ship();

                    if (photoPath != null)
                    {
                        string photoName = "\\Images\\" + System.IO.Path.GetRandomFileName() + ".jpg";
                        File.Copy(photoPath, Environment.CurrentDirectory + photoName, true);
                        newShip.ImageShip = photoName;
                    }

                    newShip.NameShip = NameShipTextBox.Text;
                    newShip.CaptainID = CaptainID;
                    newShip.Seats = Convert.ToInt32(SeatsTextBox.Text);
                    newShip.LoadCapacity = LoadCapacityTextBx.Text + "т";

                    Dbcontext.Ship.Add(newShip);
                    Dbcontext.SaveChanges();
                    MessageBox.Show("Судно успешно добавлено", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Заполните поля!", "Уведомление",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ChangeImageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                photoPath = openFileDialog.FileName;
                ShipImage.Source = new BitmapImage(new Uri(photoPath));
            }
        }

        private void DeleteShipBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить судно?\nВнимание, если вы удалите судно, " +
                "то все туры и круизы, которые совершает судно будут так же удалены", 
                "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Dbcontext.Ship.Remove(Ship);
                Dbcontext.SaveChanges();
                MessageBox.Show("Судно успешно удалено", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                return;
            }
        }

        private void LoadImageShip()
        {
            if (Ship.ImageShip != null && Ship.ImageShip.Length > 0)
            {
                Uri resourceUri = new Uri(Environment.CurrentDirectory + Ship.ImageShip);
                ShipImage.Source = new BitmapImage(resourceUri);
            }
            else
            {
                ShipImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/picture.png"));
            }
        }
        private void LoadImageNullShip()
        {
            ShipImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/picture.png"));
            photoPath = null;
        }

        private void SeatsTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void SeatsTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
