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
using Word = Microsoft.Office.Interop.Word;

namespace PassengerShipingCo.Windows
{
    /// <summary>
    /// Логика взаимодействия для TourAccounting.xaml
    /// </summary>
    public partial class TourAccounting : Window
    {
        PassengerShippingEntities Dbcontext;

        bool DateDepartureChanged = false;
        bool DateArrivalChanged = false;

        int TotalProfitForPeriod;

        public TourAccounting()
        {
            InitializeComponent();
            Dbcontext = new PassengerShippingEntities();

            DepartureDatePicker.DisplayDateStart = DateTime.Now;
            ArrivalDatePicker.DisplayDateStart = DateTime.Now;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void UpdateAccountingListView()
        {
            TourAccountingListView.Items.Clear();

            List<Tour> displayTours = new List<Tour>();
            displayTours = Dbcontext.Tour.ToList();

            if (DateArrivalChanged && DateDepartureChanged)
            {
                if (ArrivalDatePicker.SelectedDate != null && 
                    DepartureDatePicker.SelectedDate != null)
                {
                    displayTours = displayTours.Where(t =>
                    t.DepartureTime >= DepartureDatePicker.SelectedDate &&
                    t.ArrivalTime <= ArrivalDatePicker.SelectedDate).ToList();

                    AccountingThisTours.Visibility = Visibility.Visible;
                }
            }

            if (!string.IsNullOrWhiteSpace(DepartureTextBox.Text))
            {
                displayTours = displayTours.Where(t =>
                t.Cruise.Port1.NamePort.ToLower().Contains(DepartureTextBox.Text.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(ArrivalTextBox.Text))
            {
                displayTours = displayTours.Where(t =>
                t.Cruise.Port.NamePort.ToLower().Contains(ArrivalTextBox.Text.ToLower())).ToList();
            }

            foreach (Tour tour in displayTours)
            {
                TourAccountingListView.Items.Add(new TourAccountingControl(tour)
                {
                    Width = GetOptimizedWidth()
                });
                TotalProfitForPeriod += tour.NumberOfTicketSold * tour.Cruise.CostCruise;
            }

            TotalProfitForPeriodLabel.Content = $"Прибыль за выбранный период {TotalProfitForPeriod} ₽";
            TotalProfitForPeriod = 0;
        }

        private double GetOptimizedWidth()
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

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void DepartureDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateDepartureChanged = true;
            UpdateAccountingListView();
        }

        private void ArrivalDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateArrivalChanged = true;
            UpdateAccountingListView();
        }

        private void DepartureTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void ArrivalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAccountingListView();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AccountingSelectedTours()
        {
            List<Tour> selectedTours = new List<Tour>();

            foreach(TourAccountingControl tourAccountingControl in
                TourAccountingListView.Items)
            {
                selectedTours.Add(tourAccountingControl.Tour);
            }

            if (selectedTours.Count != 0)
            {
                Word.Application wordApp = new Word.Application();
                wordApp.Visible = true;
                Object template = Type.Missing;
                Object newTemplate = false;
                Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
                Object visible = true;
                object missing = Type.Missing;
                Word._Document wordDoc = wordApp.Documents.Add(
                    ref missing, ref missing, ref missing, ref missing);
                object start = 0, end = 0;

                Word.Range range = wordDoc.Range(ref start, ref end);
                range.Text = $"Учет 1\n".ToUpper();

                range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                range.ParagraphFormat.SpaceAfter = 0;
                range.Font.Name = "Times New Roman";

                range.Font.Size = 14;

                start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                range = wordDoc.Range(ref start, ref end);

                range.Text = $"Период от: {DepartureDatePicker.SelectedDate} до {ArrivalDatePicker.SelectedDate}";

                range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                range.ParagraphFormat.SpaceAfter = 0;
                range.Font.Name = "Times New Roman";

                range.Font.Size = 14;


                start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                range = wordDoc.Range(ref start, ref end);

                Word.Table table = wordDoc.Tables.Add(range, selectedTours.Count + 1, 6, missing, missing);

                table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

                table.Range.Font.Name = "Times New Roman";

                table.Cell(1, 1).Range.Text = "Номер тура";
                table.Cell(1, 2).Range.Text = "Судно";
                table.Cell(1, 3).Range.Text = "Капитан";
                table.Cell(1, 4).Range.Text = "Порт отбытия";
                table.Cell(1, 5).Range.Text = "Порт прибытия";
                table.Cell(1, 6).Range.Text = "Выручка";

                table.Cell(1, 1).Range.Font.Size = 14;
                table.Cell(1, 2).Range.Font.Size = 14;
                table.Cell(1, 3).Range.Font.Size = 14;
                table.Cell(1, 4).Range.Font.Size = 14;
                table.Cell(1, 5).Range.Font.Size = 14;
                table.Cell(1, 6).Range.Font.Size = 14;

                for (int i = 0; i < selectedTours.Count; i++)
                {
                    table.Cell(i + 2, 1).Range.Text = selectedTours[i].ID.ToString();

                    table.Cell(i + 2, 1).Range.Font.Size = 14;

                    table.Cell(i + 2, 2).Range.Text = selectedTours[i].Ship.NameShip;

                    table.Cell(i + 2, 2).Range.Font.Size = 14;

                    table.Cell(i + 2, 3).Range.Text = $"{selectedTours[i].Ship.Captain.Name} " +
                        $"{selectedTours[i].Ship.Captain.SecondName} {selectedTours[i].Ship.Captain.LastName}";

                    table.Cell(i + 2, 3).Range.Font.Size = 14;

                    table.Cell(i + 2, 4).Range.Text = selectedTours[i].Cruise.Port1.NamePort;

                    table.Cell(i + 2, 4).Range.Font.Size = 14;

                    table.Cell(i + 2, 5).Range.Text = selectedTours[i].Cruise.Port.NamePort;

                    table.Cell(i + 2, 5).Range.Font.Size = 14;

                    int profit = selectedTours[i].NumberOfTicketSold * selectedTours[i].Cruise.CostCruise;
                    table.Cell(i + 2, 6).Range.Text = profit.ToString();

                    table.Cell(i + 2, 6).Range.Font.Size = 14;
                }

                start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                range = wordDoc.Range(ref start, ref end);
                range.Text = "\nПодпись: ____________";
                range.Font.Name = "Times New Roman";
                range.Font.Size = 14;
            }
            else
            {
                MessageBox.Show("Нету туров за выбранный период, выберите другой период",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AccountingThisTours_Click(object sender, RoutedEventArgs e)
        {
            if (ArrivalDatePicker.SelectedDate != null && DepartureDatePicker != null)
            {
                AccountingSelectedTours();
            }
            else
            {
                MessageBox.Show("Выберите период", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TourAccountingListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TourAccountingListView.SelectedItems != null)
            {
                if (MessageBox.Show("Создать учет по выбранному туру?", "Вопрос",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Tour selectedTour = ((TourAccountingControl)TourAccountingListView.SelectedItem).Tour;

                    List<PassengerCruise> passengerCruises = new List<PassengerCruise>();
                    passengerCruises = Dbcontext.PassengerCruise.Where(p =>
                    p.TourID == selectedTour.ID).ToList();

                    Word.Application wordApp = new Word.Application();
                    wordApp.Visible = true;
                    Object template = Type.Missing;
                    Object newTemplate = false;
                    Object documentType = Word.WdNewDocumentType.wdNewBlankDocument;
                    Object visible = true;
                    object missing = Type.Missing;
                    Word._Document wordDoc = wordApp.Documents.Add(
                        ref missing, ref missing, ref missing, ref missing);
                    object start = 0, end = 0;

                    Word.Range range = wordDoc.Range(ref start, ref end);
                    range.Text = $"Учет тура {selectedTour.ID}\n".ToUpper();

                    range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    range.ParagraphFormat.SpaceAfter = 0;
                    range.Font.Name = "Times New Roman";

                    range.Font.Size = 14;

                    start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                    range = wordDoc.Range(ref start, ref end);

                    range.Text = $"Капитан: {selectedTour.Ship.Captain.Name} {selectedTour.Ship.Captain.SecondName} " +
                        $"{selectedTour.Ship.Captain.LastName}\n";
                    range.Text += $"Судно: {selectedTour.Ship.NameShip}\n";

                    range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    range.ParagraphFormat.SpaceAfter = 0;
                    range.Font.Name = "Times New Roman";

                    range.Font.Size = 14;


                    start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                    range = wordDoc.Range(ref start, ref end);

                    Word.Table table = wordDoc.Tables.Add(range, passengerCruises.Count + 1, 3, missing, missing);

                    table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                    table.Range.Font.Name = "Times New Roman";
                    table.Cell(1, 1).Range.Text = "Номер пассажира";
                    table.Cell(1, 2).Range.Text = "ФИО пассажира";
                    table.Cell(1, 3).Range.Text = "Телефон";

                    table.Cell(1, 1).Range.Font.Size = 14;
                    table.Cell(1, 2).Range.Font.Size = 14;
                    table.Cell(1, 3).Range.Font.Size = 14;
                    for (int i = 0; i < selectedTour.PassengerCruise.Count; i++)
                    {
                        table.Cell(i + 2, 1).Range.Text = i.ToString();

                        table.Cell(i + 2, 1).Range.Font.Size = 14;

                        table.Cell(i + 2, 2).Range.Text = $"{passengerCruises[i].Passenger.Name} " +
                            $"{passengerCruises[i].Passenger.SecondName} {passengerCruises[i].Passenger.LastName}";

                        table.Cell(i + 2, 2).Range.Font.Size = 14;

                        table.Cell(i + 2, 3).Range.Text = passengerCruises[i].Passenger.Phone;

                        table.Cell(i + 2, 3).Range.Font.Size = 14;
                    }

                    start = wordDoc.Range().End - 1; end = wordDoc.Range().End - 1;
                    range = wordDoc.Range(ref start, ref end);
                    range.Text = $"\nДата начала тура: {selectedTour.DepartureTime.ToShortDateString()}\n";
                    range.Text += $"Дата окончания тура: {selectedTour.ArrivalTime.ToShortDateString()}\n";
                    range.Text += $"Количество проданных билетов: {selectedTour.NumberOfTicketSold}\n";
                    range.Text += "Подпись: ____________";
                    range.Font.Name = "Times New Roman";
                    range.Font.Size = 14;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
