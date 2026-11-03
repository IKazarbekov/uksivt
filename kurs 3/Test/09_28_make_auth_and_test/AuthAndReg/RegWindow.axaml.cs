using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AuthAndReg;

public partial class RegWindow : Window
{
    public RegWindow()
    {
        InitializeComponent();
    }

    private void RegisterButton_Click(object? sender, RoutedEventArgs e)
    {
        String lastName = LastNameBox.Text?.Trim() ?? "";
        var firstName = FirstNameBox.Text?.Trim() ?? "";
        var middleName = MiddleNameBox.Text?.Trim() ?? "";
        var email = EmailBox.Text?.Trim() ?? "";
        var password = PasswordBox.Text ?? "";
        var confirm = ConfirmPasswordBox.Text ?? "";

        if (lastName == "" || firstName == "" || email == "" || password == "")
        {
            MessageText.Text = "Заполните обязательные поля";
            return;
        }

        bool isCorrectName = true;
        for (int i = 0; i < lastName.Length; i++)
        {
            var ch = lastName.ToLower()[i];
            if ((int)ch < 1072 || (int)ch > 1103 && (int)ch != (int)'ё')
                isCorrectName = false;
        }
        for (int i = 0; i < firstName.Length; i++)
        {
            var ch = firstName.ToLower()[i];
            if ((int)ch < 1072 || (int)ch > 1103 && (int)ch != (int)'ё')
                isCorrectName = false;
        }
        for (int i = 0; i < middleName.Length; i++)
        {
            var ch = middleName.ToLower()[i];
            if ((int)ch < 1072 || (int)ch > 1103 && (int)ch != (int)'ё')
                isCorrectName = false;
        }
        if (!isCorrectName)
        {
            MessageText.Text = "ФИО должны быть русскими символами";
            return;
        }


        if (picker.SelectedDate == null)
        {
            MessageText.Text = "Выберите дату рождения";
            return;
        }
        var birthDate = picker.SelectedDate.Value.DateTime;

        if (password.Length < 8)
        {
            MessageText.Text = "Пароль должен быть не менее 8 символов";
            return;
        }

        if (password != confirm)
        {
            MessageText.Text = "Пароли не совпадают";
            return;
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            MessageText.Text = "Некорректный email";
            return;
        }

        if (Data.Users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            MessageText.Text = "Пользователь с таким email уже существует";
            return;
        }

        var today = DateTime.Today;
        int age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age)) age--;

        if (age < 18)
        {
            MessageText.Text = "Разрешает только совершенолетним";
            return;
        }

        if (age > 130)
        {
            MessageText.Text = "Некоректный возраст";
            return;
        }

        int newId = Data.Users.Count == 0 ? 1 : Data.Users.Max(u => u.Id) + 1;

        var user = new User
        {
            Id = newId,
            LastName = lastName,
            FirstName = firstName,
            SecondName = middleName,
            Date = birthDate,
            Email = email,
            Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password))
        };

        Data.Users.Add(user);
        Data.Save();

        MessageText.Foreground = Avalonia.Media.Brushes.Green;
        MessageText.Text = "Регистрация успешна!";

        new MainWindow().Show();
        Close();
    }

    private void Button_Click_To_Auth(object? sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}