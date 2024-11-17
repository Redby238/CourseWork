using System;
using System.Windows;
using WPF_LoginForm.DbSettings;
using Microsoft.AspNet.Identity;
using WPF_LoginForm.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Windows.Controls;

namespace WPF_LoginForm.View
{
    public partial class AddUserWindow : Window
    {
        private ApplicationDbContext _context;

        public event Action<ApplicationUser> UserAdded;

        public AddUserWindow()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||  string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Пожалуйста, введите корректный email.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }

            try
            {
                var newUser = new ApplicationUser
                {
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                };

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

                var result = userManager.Create(newUser, password);  
                if (!result.Succeeded)
                {
                    MessageBox.Show($"Ошибка при создании пользователя: {string.Join(", ", result.Errors)}");
                    return;
                }
                else
                {
                    // Добавляем роль пользователю
                    userManager.AddToRole(newUser.Id, "User");

                    MessageBox.Show("Пользователь успешно добавлен!");
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
