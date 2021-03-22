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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            ComboLogin.ItemsSource = UserPaymentsDBEntities.GetContext().User.ToList();
        }

        /// <summary>
        /// Проверка логина и пароля
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComboLogin.SelectedItem is User currentUser)
            {
                if (TextPassword.Password == currentUser.Password)
                {
                    UserPaymentsDBEntities.CurrentUserID = currentUser.ID; 
                    var paymentWindow = new MainWindow
                    {
                        Owner = this,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };
                    paymentWindow.Show();
                    paymentWindow.Title = currentUser.FIO;
                    Hide();
                }

                else
                {
                    MessageBox.Show("Неверный пароль", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// При нажатии на крестик (закрытие приложения)
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
