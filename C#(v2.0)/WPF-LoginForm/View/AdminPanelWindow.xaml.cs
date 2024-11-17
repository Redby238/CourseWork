using System.Linq;
using System.Windows;
using System.Collections.Generic;
using WPF_LoginForm.DbSettings;
using Microsoft.AspNet.Identity;
using System.Windows.Controls;
using System;
using WPF_LoginForm.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WPF_LoginForm.View
{
    public partial class AdminPanelWindow : Window
    {
        private ApplicationDbContext _context;

        public AdminPanelWindow()
        {
            InitializeComponent();
            _context = new ApplicationDbContext();
            LoadUsers();  
        }

        // Метод для загрузки пользователей в ListBox
        public void LoadUsers()
        {
            var users = _context.Users.ToList();  
            userListBox.ItemsSource = users;  
            userListBox.DisplayMemberPath = "UserName";  
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.UserAdded += OnUserAdded;  
            addUserWindow.ShowDialog();
        }

       
        private void OnUserAdded(ApplicationUser newUser)
        {
            
            _context.Users.Add(newUser);
            _context.SaveChanges();  

           
            LoadUsers();
        }

        private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = userListBox.SelectedItem as ApplicationUser;

            if (selectedUser == null)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {selectedUser.UserName}?",
                                         "Подтверждение удаления",
                                         MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var userToRemove = _context.Users.FirstOrDefault(u => u.Id == selectedUser.Id);
                    if (userToRemove != null)
                    {
                        _context.Users.Remove(userToRemove);
                        _context.SaveChanges();

                        MessageBox.Show("Пользователь успешно удален.");
                        LoadUsers(); 
                    }
                    else
                    {
                        MessageBox.Show("Пользователь не найден.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}");
                }
            }
        }

        private void btnAssignRole_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = userListBox.SelectedItem as ApplicationUser;

            if (selectedUser == null)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя.");
                return;
            }

            var selectedRole = roleComboBox.SelectedItem as ComboBoxItem;
            if (selectedRole != null)
            {
                string role = selectedRole.Content.ToString();

                // Проверка, является ли роль "Owner"
                if (role == "Owner")
                {
                    MessageBox.Show($"Роль {role} назначается пользователю {selectedUser.UserName}.");
                }

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));

                if (userManager.IsInRole(selectedUser.Id, role))
                {
                    MessageBox.Show($"Пользователь {selectedUser.UserName} уже имеет роль {role}.");
                    return;
                }

                var result = userManager.AddToRole(selectedUser.Id, role);

                if (result.Succeeded)
                {
                    MessageBox.Show($"Роль {role} назначена пользователю {selectedUser.UserName}.");
                }
                else
                {
                    MessageBox.Show($"Ошибка при назначении роли: {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите роль.");
            }
        }

    }
}

