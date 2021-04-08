using System;
using System.Linq;
using System.Windows;

// подключение пространства имен для работы с excel
using Excel = Microsoft.Office.Interop.Excel;
// подключения пространства имен для работы с word
using Word = Microsoft.Office.Interop.Word;

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

        private void BtnWord_Click(object sender, RoutedEventArgs e)
        {
            var users = UserPaymentsDBEntities.GetContext().User.ToList();
            var categories = UserPaymentsDBEntities.GetContext().PaymentType.ToList();

            // новый экземпляр ворда
            var application = new Word.Application();
            // новый документ
            Word.Document document = application.Documents.Add();

            foreach (var user in users)
            {
                // текст на документе состоит из параграфов
                Word.Paragraph userParagraph = document.Paragraphs.Add();
                // доступ к параграфам осуществляется через Range
                Word.Range userRange = userParagraph.Range;

                // заголовок страницы
                userRange.Text = user.FIO;
                userRange.Paragraphs.set_Style("Заголовок");

                // новый параграф
                userRange.InsertParagraphAfter();

                // таблица платежей
                Word.Paragraph tableParagraph = document.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                // в какой параграф, количество строк, количество столбцов
                Word.Table paymentTable = document.Tables.Add(tableRange, categories.Count + 1, 2);
                // внутренние и внешние границы таблицы
                paymentTable.Borders.InsideLineStyle =
                paymentTable.Borders.OutsideLineStyle =
                Word.WdLineStyle.wdLineStyleSingle;
                // выравнивание содержимое ячеек по центру вертикали
                paymentTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                Word.Range cellRange; // ячейка

                // названия колонок
                cellRange = paymentTable.Cell(1, 1).Range;
                cellRange.Text = "Категория";
                cellRange = paymentTable.Cell(1, 2).Range;
                cellRange.Text = "Сумма расходов";
                //cellRange = paymentTable.Cell(1, 3).Range;
                //cellRange.Text = "иконка";
                // форматирование названий (жирный текст и расположение по центру)
                paymentTable.Rows[1].Range.Bold = 1;
                paymentTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                //// колонка с иконками
                //for (int i = 0; i < categories.Count(); i++)
                //{
                //    var currentCategory = categories[i];

                //    cellRange = paymentTable.Cell(i + 2, 1).Range;
                //    Word.InlineShape icon = cellRange.InlineShapes.AddPicture(AppDomain.CurrentDomain.BaseDirectory + "" + currentCategory);
                //}


                // заполнение колонок
                for (int i = 0; i < categories.Count(); i++)
                {
                    var currentCategory = categories[i];

                    // i + 2 - нумерация начинается с нуля, но в ворде с 1, а первая строка занята заголовками
                    cellRange = paymentTable.Cell(i + 2, 1).Range;
                    cellRange.Text = currentCategory.PaymentName;

                    cellRange = paymentTable.Cell(i + 2, 2).Range;
                    cellRange.Text = user.Payment.Where(p => p.PaymentType.PaymentName == currentCategory.PaymentName).Sum(p => p.Count * p.Price).ToString("N2");
                }


                Payment maxPayment = user.Payment.OrderByDescending(p => p.Price * p.Count).FirstOrDefault();

                if (maxPayment != null)
                {
                    Word.Paragraph maxParagraph = document.Paragraphs.Add();
                    Word.Range maxRange = maxParagraph.Range;

                    maxRange.Text = $"Самый дорогостоящий платеж: {maxPayment.PaymentName} - {maxPayment.Summ}, от {maxPayment.PaymentDate.ToString("dd.MM.yyyy HH:mm")}";

                    maxParagraph.set_Style("Цитата");
                    maxRange.Font.Color = Word.WdColor.wdColorDarkRed;
                    maxRange.InsertParagraphAfter();
                }

                Payment minPayment = user.Payment.OrderBy(p => p.Price * p.Count).FirstOrDefault();

                if (minPayment != null)
                {
                    Word.Paragraph minParagraph = document.Paragraphs.Add();
                    Word.Range minRange = minParagraph.Range;

                    minRange.Text = $"Самый дешевый платеж: {minPayment.PaymentName} - {minPayment.Summ}, от {minPayment.PaymentDate.ToString("dd:MM:yyyy HH:mm")}";

                    minParagraph.set_Style("Цитата");
                    minRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                    minRange.InsertParagraphAfter();
                }

                // добавление разрыва страницы всем пользователям кроме последнего
                if (user != users.LastOrDefault())
                {
                    document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }

            application.Visible = true;
        }
    }
}
