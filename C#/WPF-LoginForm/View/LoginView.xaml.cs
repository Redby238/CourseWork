using System;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.Services;

namespace WPF_LoginForm.View
{
    public partial class LoginView : Window
    {
        public LoginView()
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
            Application.Current.Shutdown();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = txtUser.Text;
            string password = txtPass.Password;

            
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            var authService = new AuthenticationService();
            var user = await authService.FindUser(userName, password);  

            if (user != null)
            {
                MessageBox.Show("Вход выполнен успешно! Добро пожаловать.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

           
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверное имя пользователя или пароль. Пожалуйста, попробуйте еще раз.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignUp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegisterView registerView = new RegisterView();
            registerView.Show();
            this.Close();
        }
    }
}
