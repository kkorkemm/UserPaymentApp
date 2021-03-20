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

namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для AddEditWindow.xaml
    /// </summary>
    public partial class AddEditWindow : Window
    {
        private Payment currentPayment = new Payment();

        /// <summary>
        /// Вызов окна добавления и редактирования платежей
        /// Если в качестве аргумента был передан null, тогда добавляется новый платеж
        /// Иначе редактируется существующий
        /// </summary>
        public AddEditWindow(Payment selectedItem)
        {
            InitializeComponent();

            if (selectedItem != null)
                currentPayment = selectedItem;

            DataContext = currentPayment;

            ComboTypes.ItemsSource = UserPaymentsDBEntities.GetContext().PaymentType.ToList();
        }

        /// <summary>
        /// Добавление платежа. Проверка на корректность введенных данных
        /// При наличии ошибок появляется окно с их описанием
        /// При отсутствии ошибок новый платеж добавлятся в контекст данных
        /// </summary>
        private void BtnAddPayment_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (currentPayment.PaymentType == null)
                errors.AppendLine("Выберите категорию");
            if (string.IsNullOrWhiteSpace(currentPayment.PaymentName))
                errors.AppendLine("Укажите наименование платежа");
            if (currentPayment.Count < 0)
                errors.AppendLine("Количество указано некорректно");
            if (currentPayment.Price < 0)
                errors.AppendLine("Цена указана некорректно");

            if (CheckPayed.IsChecked == true)
                currentPayment.IsPayed = true;
            else
                currentPayment.IsPayed = false;

            currentPayment.PaymentDate = DateTime.Now;
            currentPayment.UserID = UserPaymentsDBEntities.CurrentUserID;


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            else
            {
                if (currentPayment.ID == 0)
                    UserPaymentsDBEntities.GetContext().Payment.Add(currentPayment);

                try
                {
                    UserPaymentsDBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные успешно сохранены!");
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Отмена добавления
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
