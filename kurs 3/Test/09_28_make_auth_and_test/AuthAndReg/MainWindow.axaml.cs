using Avalonia.Controls;
using System.Linq;
using System;
using System.Collections;
namespace AuthAndReg;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Data.Load();
    }

    private void Button_Click_To_Reg(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new RegWindow().Show();
        this.Close();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string email = EmailBox.Text?.Trim() ?? "";
        string password = PasswordBox.Text ?? "";

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            MessageText.Text = "Заполните все поля";
            return;
        }

        var user = Data.Users.FirstOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
            (u.Password == password || u.Password == Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password))));

        if (user == null)
        {
            MessageText.Text = "Неверный email или пароль";
            return;
        }

        MessageText.Text = "";

        var after = new AftenWindow();
        after.Show();
        Close();
    }

    private void Button_Click_Code(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainCode().Show();
        Close();
    }
}