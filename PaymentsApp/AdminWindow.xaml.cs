using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

// подключение пространства имен для работы с excel
using Excel = Microsoft.Office.Interop.Excel;

namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            AdminFrame.Navigate(new AdminUsersPage());
        }

        /// <summary>
        /// Получить графики расходов
        /// </summary>
        private void BtnDiagram_Click(object sender, RoutedEventArgs e)
        {
            PaymentDiagramm paymentDiagramm = new PaymentDiagramm
            {
                Title = "Графики расходов пользователей"
            };
            paymentDiagramm.Show();
        }

        /// <summary>
        /// Нажатие на крестик
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Выйти
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            LoginRegist loginRegist = new LoginRegist();
            loginRegist.Show();
            Hide();
        }

        private void BtnExcel_Click(object sender, RoutedEventArgs e)
        {
            var users = UserPaymentsDBEntities.GetContext().User.OrderBy(p => p.FIO).ToList();

            // добавление новой книги. Количество листов = количество пользователей
            var application = new Excel.Application
            {
                SheetsInNewWorkbook = users.Count()
            };
            Excel.Workbook workbook = application.Workbooks.Add(Type.Missing);

            // для каждой страницы
            for (int i = 0; i < users.Count(); i++)
            {
                int rowIndex = 1; // номер строки

                Excel.Worksheet worksheet = application.Worksheets.Item[i + 1];
                worksheet.Name = users[i].FIO; // название страницы

                // сначала столбец, потом строка
                worksheet.Cells[1][rowIndex] = "Дата платежа";
                worksheet.Cells[2][rowIndex] = "Название";
                worksheet.Cells[3][rowIndex] = "Стоимость";
                worksheet.Cells[4][rowIndex] = "Количество";
                worksheet.Cells[5][rowIndex] = "Сумма";

                rowIndex++;

                // группировка платежей по категориям
                var userCategories = users[i].Payment.OrderBy(p => p.PaymentDate).GroupBy(p => p.PaymentType).OrderBy(p=>p.Key.PaymentName);

                foreach (var category in userCategories)
                {
                    // объединение ячеек
                    Excel.Range header = worksheet.Range[worksheet.Cells[1][rowIndex], worksheet.Cells[5][rowIndex]];
                    header.Merge();

                    header.Value = category.Key.PaymentName;
                    header.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    header.Font.Italic = true;

                    rowIndex++;

                    foreach (var payment in category)
                    {
                        worksheet.Cells[1][rowIndex] = payment.PaymentDate.ToString("g");
                        worksheet.Cells[2][rowIndex] = payment.PaymentName;
                        worksheet.Cells[3][rowIndex] = payment.Price;
                        worksheet.Cells[4][rowIndex] = payment.Count;
                        // сумма
                        worksheet.Cells[5][rowIndex].Formula = $"= C{rowIndex} * D{rowIndex}";

                        //worksheet.Cells[3][rowIndex].NumberFormat =
                        //worksheet.Cells[5][rowIndex].NumberFormat = "#,###.00";

                        rowIndex++;
                    }

                    // для общей суммы по категории
                    Excel.Range totalSumm = worksheet.Range[worksheet.Cells[1][rowIndex], worksheet.Cells[4][rowIndex]];
                    totalSumm.Merge();
                    totalSumm.Value = "Итого:";
                    totalSumm.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    totalSumm.Font.Bold = true;

                    worksheet.Cells[5][rowIndex].Formula = $"=SUM(E{rowIndex - category.Count()}: E{rowIndex - 1}";

                    //worksheet.Cells[5][rowIndex].NumberFormat = "#,###.000";

                    rowIndex++;

                    //Excel.Border border = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[5][rowIndex - 1]];
                    //border.LineStyle = Excel.XlLineStyle.xlContinuous;


                    worksheet.Columns.AutoFit(); // автоматический размер колонов
                }

                application.Visible = true;
            }
        }
    }
}
