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

        /// <summary>
        /// Импорт данных о платежах в базу данных (уже импортированы)
        /// </summary>
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

        /// <summary>
        /// Добавление нового платежа
        /// </summary>
        private void BtnAddPay_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddEditWindow(null);
            addWindow.Title = "Добавление платежа";
            addWindow.Show();
        }

        /// <summary>
        /// Редактирование существующего платежа
        /// </summary>
        private void BtnEditPay_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new AddEditWindow((sender as Button).DataContext as Payment);
            editWindow.Title = "Редактирование платежа";
            editWindow.BtnAddPayment.Content = "Сохранить";
            editWindow.Show();
        }

        /// <summary>
        /// Удаление платежей
        /// </summary>
        private void BtnDeletePay_Click(object sender, RoutedEventArgs e)
        {
            var removingPayments = DGridPayments.SelectedItems.Cast<Payment>().ToList();

            if (removingPayments.Count == 0)
            {
                MessageBox.Show("Выберите элементы для удаления");
            }

            else
            {
                string message = "";
                switch (removingPayments.Count % 10)
                {
                    case 0:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9: message = $"Вы точно хотите удалить следующие {removingPayments.Count} элементов?";
                        break;
                    case 2:
                    case 3:
                    case 4: message = $"Вы точно хотите удалить следующие {removingPayments.Count} элемента?"; break;
                    case 1: message = $"Вы точно хотите удалить {removingPayments.Count} элемент?"; break;
                }

                MessageBoxResult result = MessageBox.Show(message, "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        UserPaymentsDBEntities.GetContext().Payment.RemoveRange(removingPayments);
                        UserPaymentsDBEntities.GetContext().SaveChanges();

                        MessageBox.Show("Данные успешно удалены!");

                        DGridPayments.ItemsSource = UserPaymentsDBEntities.GetContext().Payment.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Фильтрация по дате и категории платежей
        /// </summary>
        private void UpdatePayments()
        {
            var currentPayments = UserPaymentsDBEntities.GetContext().Payment.ToList();

            if (datePicker.SelectedDate != null && datePicker2.SelectedDate == null)
            {
                currentPayments = currentPayments.Where(i => i.PaymentDate >= datePicker.SelectedDate.Value).ToList();
            }

            if (datePicker.SelectedDate == null && datePicker2.SelectedDate != null)
            {
                currentPayments = currentPayments.Where(i => i.PaymentDate <= datePicker2.SelectedDate.Value).ToList();
            }

            if (datePicker.SelectedDate != null && datePicker2.SelectedDate != null
                && datePicker.SelectedDate.Value <= datePicker2.SelectedDate.Value)
            {
                currentPayments = currentPayments.Where(i => i.PaymentDate >= datePicker.SelectedDate.Value
                && i.PaymentDate <= datePicker2.SelectedDate.Value).ToList();
            }

            if (ComboTypes.SelectedIndex > 0)
            {
                currentPayments = currentPayments.Where(i => i.PaymentType == ComboTypes.SelectedItem as PaymentType).ToList();
            }

            DGridPayments.ItemsSource = currentPayments.ToList();
               
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePayments();
        }

        private void datePicker2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePayments();
        }

        private void ComboTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePayments();
        }

        /// <summary>
        /// Выход из кабинета пользователя (Кнопка Выйти)
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            Hide();
        }
        
        /// <summary>
        /// Кнопка Обновить таблицу
        /// </summary>
        private void BtnUpdateGrid_Click(object sender, RoutedEventArgs e)
        {
            DGridPayments.ItemsSource = UserPaymentsDBEntities.GetContext().Payment.ToList();
        }

        /// <summary>
        /// При нажатии на крестик (закрытие)
        /// </summary>
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
