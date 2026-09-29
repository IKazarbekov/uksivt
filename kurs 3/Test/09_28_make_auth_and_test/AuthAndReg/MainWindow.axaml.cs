using Avalonia.Controls;

namespace AuthAndReg;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click_To_Reg(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new RegWindow().Show();
        this.Close();
    }
}