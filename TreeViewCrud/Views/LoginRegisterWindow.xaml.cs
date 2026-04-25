using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using TreeViewCrud.Models;
using TreeViewCrud.Services;

namespace TreeViewCrud.Views
{
    public partial class LoginRegisterWindow : Window
    {
        public AppUser? CurrentUser { get; private set; }

        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowLoginError("Введите логин и пароль.");
                return;
            }

            using (var db = new AppUserDbContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
                if (user == null)
                {
                    ShowLoginError("Неверный логин или пароль.");
                    return;
                }

                if (!user.IsActive)
                {
                    ShowLoginError("Пользователь заблокирован.");
                    return;
                }

                if (!PasswordHelper.VerifyPassword(password, user.Salt, user.PasswordHash))
                {
                    ShowLoginError("Неверный логин или пароль.");
                    return;
                }

                CurrentUser = user;
                DialogResult = true;
                Close();
            }
        }

        private async void Register_Click(object sender, RoutedEventArgs e)
        {
            string login = txtRegLogin.Text.Trim();
            string password = txtRegPassword.Password;
            string confirm = txtConfirmPassword.Password;

            // Валидация
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ShowRegError("Заполните все поля.");
                return;
            }

            if (password != confirm)
            {
                ShowRegError("Пароли не совпадают.");
                return;
            }

            if (!PasswordHelper.IsPasswordComplex(password))
            {
                ShowRegError("Пароль должен содержать минимум 6 символов, включая цифры и буквы.");
                return;
            }

            using (var db = new AppUserDbContext())
            {
                // Проверка уникальности логина
                var existing = await db.Users.FirstOrDefaultAsync(u => u.Login == login);
                if (existing != null)
                {
                    ShowRegError("Пользователь с таким логином уже существует.");
                    return;
                }

                // Создание нового пользователя
                string salt = PasswordHelper.GenerateSalt();
                string hash = PasswordHelper.HashPassword(password, salt);

                var newUser = new AppUser
                {
                    Login = login,
                    Salt = salt,
                    PasswordHash = hash,
                    IsActive = true,
                    RegistrationDate = DateTime.Now,
                    Role = "admin",
                };

                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                MessageBox.Show("Регистрация прошла успешно! Теперь войдите.", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                // Очистка полей и переключение на вкладку входа
                txtRegLogin.Clear();
                txtRegPassword.Clear();
                txtConfirmPassword.Clear();
                MainTabControl.SelectedIndex = 0;
                txtLogin.Text = login;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ShowLoginError(string message)
        {
            txtLoginError.Text = message;
            txtLoginError.Visibility = Visibility.Visible;
        }

        private void ShowRegError(string message)
        {
            txtRegError.Text = message;
            txtRegError.Visibility = Visibility.Visible;
        }
    }
}