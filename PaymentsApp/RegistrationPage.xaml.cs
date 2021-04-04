using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private User newUser = new User();

        public RegistrationPage()
        {
            InitializeComponent();
            DataContext = newUser;
        }

        private void BtnRegist_Click(object sender, RoutedEventArgs e)
        {
            var users = UserPaymentsDBEntities.GetContext().User.ToList();
            StringBuilder errors = new StringBuilder();

            if (newUser.FIO == null)
                errors.AppendLine("Введите ФИО");

            foreach (var i in users)
            {
                if (newUser.FIO == i.FIO)
                    errors.AppendLine("Такой пользователь уже есть в базе!");
            }

            if (!Regex.IsMatch(newUser.Login, @"^[a-zA-Z0-9_]+$") || newUser.Login == null)
                errors.AppendLine("Введите корректный логин");

            if (newUser.PIN.Length > 4 || newUser.PIN.All(Char.IsLetter) || newUser.PIN == null)
                errors.AppendLine("Введите корректный PIN");

            if (TextPass.Password != TextRePass.Password) 
                errors.AppendLine("Пароли не совпадают!");

            if (TextPass.Password == " " || TextPass.Password == null)
                errors.AppendLine("Введите корректный пароль");

            if (errors.Length == 0)
            {
                newUser.Password = TextPass.Password;
                newUser.RoleID = 2;

                if (newUser.ID == 0)
                    UserPaymentsDBEntities.GetContext().User.Add(newUser);

                try
                {
                    UserPaymentsDBEntities.GetContext().SaveChanges();

                    MessageBox.Show("Регистрация прошла успешно!", "Внимание!");
                    Navigation.MainFrame.Navigate(new LoginPage());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(errors.ToString(), "Внимание!");
            }
        }
    }
}
