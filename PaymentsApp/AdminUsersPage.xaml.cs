using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace PaymentsApp
{
    /// <summary>
    /// Логика взаимодействия для AdminUsersPage.xaml
    /// </summary>
    public partial class AdminUsersPage : Page
    {
        User currentUser = new User();

        public AdminUsersPage()
        {
            InitializeComponent();

            DataContext = currentUser;

            GridUsers.ItemsSource = UserPaymentsDBEntities.GetContext().User.ToList().Where(p => p.RoleID != 1);
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            AdminEditUserWindow editUserWindow = new AdminEditUserWindow((sender as Button).DataContext as User);
            editUserWindow.Show();
            GridUsers.ItemsSource = UserPaymentsDBEntities.GetContext().User.Where(p => p.RoleID != 1).ToList();
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = GridUsers.SelectedItem as User;

            MessageBoxResult result = MessageBox.Show($"Вы точно хотите удалить этого пользователя {selectedUser.FIO}?", "Внимание!", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    UserPaymentsDBEntities.GetContext().User.Remove(selectedUser);
                    UserPaymentsDBEntities.GetContext().SaveChanges();

                    MessageBox.Show("Данные успешно удалены!");
                    GridUsers.ItemsSource = UserPaymentsDBEntities.GetContext().User.Where(p => p.RoleID != 1).ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
