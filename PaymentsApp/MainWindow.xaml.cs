using System;
using System.Collections.Generic;
using System.IO;
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

namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DGridPayments.ItemsSource = UserPaymentsDBEntities.GetContext().Payment.ToList();

            var types = UserPaymentsDBEntities.GetContext().PaymentType.ToList();
            types.Insert(0, new PaymentType { PaymentName = "Все категории" });
            ComboTypes.ItemsSource = types;
            ComboTypes.SelectedIndex = 0;

            // ImportPayments();
        }

        private void ImportPayments()
        {
            var dataFile = File.ReadAllLines(@"C:\C#\UserPaymentApp\данные для импорта\payments.txt");

            foreach (var line in dataFile)
            {
                var dataLine = line.Split('\t');

                var newPayment = new Payment
                {
                    PaymentDate = DateTime.Parse(dataLine[0]),
                    PaymentTypeID = int.Parse(dataLine[1]),
                    PaymentName = dataLine[2],
                    Count = int.Parse(dataLine[3]),
                    Price = decimal.Parse(dataLine[4]),
                    UserID = int.Parse(dataLine[5]),
                    IsPayed = dataLine[6] != "0"
                };

                try
                {
                    UserPaymentsDBEntities.GetContext().Payment.Add(newPayment);
                    UserPaymentsDBEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void BtnAddPay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeletePay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void datePicker2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
