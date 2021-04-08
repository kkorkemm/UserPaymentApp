using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;


namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для AdminEditUserWindow.xaml
    /// </summary>
    public partial class AdminEditUserWindow : Window
    {
        User currentUser = new User();
        public AdminEditUserWindow(User user)
        {
            InitializeComponent();

            currentUser = user;
            DataContext = currentUser;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var users = UserPaymentsDBEntities.GetContext().User.ToList();
            StringBuilder errors = new StringBuilder();

            if (currentUser.FIO == null)
                errors.AppendLine("Введите ФИО");

            if (!Regex.IsMatch(currentUser.Login, @"^[a-zA-Z0-9_]+$") || currentUser.Login == null)
                errors.AppendLine("Введите корректный логин");

            if (currentUser.PIN.Length > 4 || currentUser.PIN.All(Char.IsLetter) || currentUser.PIN == null)
                errors.AppendLine("Введите корректный PIN");

            if (TextPass.Text != TextRePass.Text)
                errors.AppendLine("Пароли не совпадают!");

            if (string.IsNullOrWhiteSpace(currentUser.Password))
                errors.AppendLine("Введите корректный пароль");

            if (errors.Length == 0)
            {
                try
                {
                    UserPaymentsDBEntities.GetContext().SaveChanges();

                    MessageBox.Show("Данные успешно обновлены!", "Внимание!");
                    Close();
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
