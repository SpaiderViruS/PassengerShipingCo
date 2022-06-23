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
using PassengerShipingCo.Entity;
using PassengerShipingCo.UserControls;

namespace PassengerShipingCo.Windows
{
    /// <summary>
    /// Логика взаимодействия для TourAccounting.xaml
    /// </summary>
    public partial class TourAccounting : Window
    {
        PassengerShippingEntities Dbcontext;


        public TourAccounting()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            DepartureDatePicker.DisplayDateStart = DateTime.Now;
            ArrivalDatePicker.DisplayDateEnd = DateTime.Now;
        }
    }
}
