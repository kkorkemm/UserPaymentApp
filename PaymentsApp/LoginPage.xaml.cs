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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            
            var users = UserPaymentsDBEntities.GetContext().User.ToList();
            users.Insert(0, new User { FIO = "Администратор" });
            ComboLogin.ItemsSource = users;
        }

        /// <summary>
        /// Кнопка Выйти
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Кнопка Войти
        /// </summary>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (ComboLogin.SelectedIndex == 0 && TextPassword.Password == "admin")
            {
                MessageBox.Show("Ghbdn!");
            }
            else
            {
                if (ComboLogin.SelectedItem is User currentUser)
                {
                    if (TextPassword.Password == currentUser.Password)
                    {
                        UserPaymentsDBEntities.CurrentUserID = currentUser.ID;
                        UserPaymentsDBEntities.CurrentUserName = currentUser.FIO;
                        MainWindow paymentWindow = new MainWindow
                        {
                            Title = currentUser.FIO,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        };
                        paymentWindow.Show();
                        Application.Current.MainWindow.Close(); // закрытие окна регистрации
                    }

                    else
                    {
                        MessageBox.Show("Неверный пароль", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Кнопка для перехода на страницу регистрации
        /// </summary>
        private void BtnNavToRegist_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new RegistrationPage());
        }
    }
}
