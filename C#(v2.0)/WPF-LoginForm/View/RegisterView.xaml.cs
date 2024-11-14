using System;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.DbSettings;
using WPF_LoginForm.Model;
using WPF_LoginForm.Services;
using System.Linq;

namespace WPF_LoginForm.View
{
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void LogIn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();
            this.Close();
        }


        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;


            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = "DefaultFirstName",
                LastName = "DefaultLastName"
            };


            var authService = new AuthenticationService();
            var result = await authService.RegisterUser(userName, password, email);

            if (result.Succeeded)
            {
                MessageBox.Show("Регистрация успешна! Добро пожаловать!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoginView loginView = new LoginView();
                loginView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при регистрации: " + string.Join(", ", result.Errors), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
