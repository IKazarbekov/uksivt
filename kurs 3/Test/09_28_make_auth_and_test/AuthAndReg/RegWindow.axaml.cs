using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AuthAndReg;

public partial class RegWindow : Window
{
    public RegWindow()
    {
        InitializeComponent();
    }

    private void Button_Click_To_Auth(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}