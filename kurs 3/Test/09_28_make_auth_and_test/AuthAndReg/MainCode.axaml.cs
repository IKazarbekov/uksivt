using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AuthAndReg;

public partial class MainCode : Window
{
    private string _generatedCode = "";
    private User? _user;
    private bool _codeVerified = false;

    private const string SenderEmail = "IKazarbekov145@yandex.ru";
    private const string SenderPassword = "whkxtvqixdrjejxc";

    public MainCode()
    {
        InitializeComponent();
    }

    private void SendCode_Click(object? sender, RoutedEventArgs e)
    {
        var email = EmailBox.Text?.Trim() ?? "";

        if (email == "")
        {
            MessageText.Text = "Введите email";
            return;
        }

        _user = Data.Users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (_user == null)
        {
            MessageText.Text = "Пользователь с таким email не найден";
            return;
        }

        _generatedCode = new Random().Next(100000, 999999).ToString();

        try
        {
            SendEmail(email, _generatedCode);
            MessageText.Foreground = Avalonia.Media.Brushes.Green;
            MessageText.Text = "Код отправлен на почту";
        }
        catch (Exception ex)
        {
            MessageText.Foreground = Avalonia.Media.Brushes.Red;
            MessageText.Text = "Ошибка отправки: " + ex.Message;
        }
    }

    private void VerifyCode_Click(object? sender, RoutedEventArgs e)
    {
        if (_generatedCode == "")
        {
            MessageText.Text = "Сначала отправьте код";
            return;
        }

        var input = CodeBox.Text?.Trim() ?? "";

        if (input != _generatedCode)
        {
            MessageText.Text = "Неверный код";
            return;
        }

        _codeVerified = true;
        MessageText.Foreground = Avalonia.Media.Brushes.Green;
        MessageText.Text = "Код верный. Введите новый пароль";

        NewPasswordLabel.IsVisible = true;
        NewPasswordBox.IsVisible = true;
        ConfirmPasswordLabel.IsVisible = true;
        ConfirmPasswordBox.IsVisible = true;
        SaveButton.IsVisible = true;
    }

    private void SavePassword_Click(object? sender, RoutedEventArgs e)
    {
        if (!_codeVerified || _user == null)
        {
            MessageText.Text = "Сначала подтвердите код";
            return;
        }

        var newPass = NewPasswordBox.Text ?? "";
        var confirm = ConfirmPasswordBox.Text ?? "";

        if (newPass.Length < 8)
        {
            MessageText.Text = "Пароль должен быть не менее 8 символов";
            return;
        }

        if (newPass != confirm)
        {
            MessageText.Text = "Пароли не совпадают";
            return;
        }

        _user.Password = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(newPass));

        Data.Save();

        MessageText.Foreground = Avalonia.Media.Brushes.Green;
        MessageText.Text = "Пароль изменён!";

        new MainWindow().Show();
        Close();
    }

    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private static void SendEmail(string to, string code)
    {
        var from = new MailAddress(SenderEmail, "Восстановление пароля");
        var message = new MailMessage(from, new MailAddress(to))
        {
            Subject = "Код для сброса пароля",
            Body = $"Ваш код подтверждения: {code}"
        };

        using var smtp = new SmtpClient("smtp.yandex.ru", 587)
        {
            Credentials = new NetworkCredential(SenderEmail, SenderPassword),
            EnableSsl = true,
            Timeout = 30000 // 30 секунд
        };

        smtp.Send(message);
    }
}