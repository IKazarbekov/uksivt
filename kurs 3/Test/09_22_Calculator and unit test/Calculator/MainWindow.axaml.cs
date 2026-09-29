using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Calculator;

public partial class MainWindow : Window
{
    private double _firstNumber = 0;
    private string _operation = "";
    private bool _isNewEntry = true;
    private readonly Logic _logic = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button) return;
        string content = button.Content?.ToString()?.Trim() ?? "";

        switch (content)
        {
            case "AC":
                _firstNumber = 0;
                _operation = "";
                _isNewEntry = true;
                Display.Text = "0";
                break;

            case "+/-":
                var n = Display.Text ?? "0";
                Display.Text = n.StartsWith('-') ? n.Substring(1) : "-" + n;
                break;

            case "X":
                if (Display.Text != null && Display.Text.Length > 1)
                    Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
                else
                    Display.Text = "0";
                break;

            case "+":
            case "-":
            case "*":
            case "/":
                _firstNumber = double.Parse(Display.Text ?? "0");
                _operation = content;
                _isNewEntry = true;
                break;

            case "%":
                if (double.TryParse(Display.Text, out double p))
                    Display.Text = (p / 100).ToString();
                _isNewEntry = true;
                break;

            case "=":
                if (string.IsNullOrEmpty(_operation)) return;
                double second = double.Parse(Display.Text ?? "0");
                double result = _logic.Calculate(_firstNumber, second, _operation);
                Display.Text = result.ToString();
                _firstNumber = result;
                _operation = "";
                _isNewEntry = true;
                break;

            case ",":
                if (!(Display.Text ?? "").Contains(","))
                    Display.Text = (Display.Text ?? "0") + ",";
                _isNewEntry = false;
                break;

            default:
                if (_isNewEntry)
                {
                    Display.Text = content;
                    _isNewEntry = false;
                }
                else
                {
                    if (Display.Text == "0")
                        Display.Text = content;
                    else
                        Display.Text += content;
                }
                break;
        }
    }
}