﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для PaymentDiagramm.xaml
    /// </summary>
    public partial class PaymentDiagramm : Window
    {
        public PaymentDiagramm()
        {
            InitializeComponent();
            PaymentChart.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Payments")
            {
                IsValueShownAsLabel = true
            };
            PaymentChart.Series.Add(currentSeries);

            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        /// <summary>
        /// Вывод диаграмма по мере выбора типа
        /// </summary>
        private void ComboChartTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = PaymentChart.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categories = UserPaymentsDBEntities.GetContext().PaymentType.ToList();
      
                foreach (var category in categories)
                {
                    currentSeries.Points.AddXY(category.PaymentName, UserPaymentsDBEntities.GetContext().Payment.ToList().Where(p => p.PaymentType == category && p.UserID == UserPaymentsDBEntities.CurrentUserID).Sum(p => p.Price * p.Count));
                }
            }
        }
    }
}
