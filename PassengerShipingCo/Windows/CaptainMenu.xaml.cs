﻿using System;
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

namespace PassengerShipingCo.Windows
{
    /// <summary>
    /// Логика взаимодействия для CaptainMenu.xaml
    /// </summary>
    public partial class CaptainMenu : Window
    {
        public CaptainMenu()
        {
            InitializeComponent();
        }

        private void CountysBtn_Click(object sender, RoutedEventArgs e)
        {
            CountyList countyList = new CountyList();
            countyList.ShowDialog();
        }

        private void EditShipsBtn_Click(object sender, RoutedEventArgs e)
        {
            ShipList shipList = new ShipList();
            shipList.ShowDialog();
        }

        private void EditCaptainsBtn_Click(object sender, RoutedEventArgs e)
        {
            CaptainsList captainsList = new CaptainsList();
            captainsList.ShowDialog();
        }

        private void ShowPassengerCruiseBtn_Click(object sender, RoutedEventArgs e)
        {
            PassengerCruiseList passengerCruiseList = new PassengerCruiseList();
            passengerCruiseList.ShowDialog();
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void AddNewCruiseBtn_Click(object sender, RoutedEventArgs e)
        {
            UpdateInsertTour updateInsertTour = new UpdateInsertTour(null);
            updateInsertTour.ShowDialog();
        }
    }
}
