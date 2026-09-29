using System;
using System.Windows;
using System.Windows.Controls;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    public partial class PlaceholderTextBox : UserControl
    {
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string),
                typeof(PlaceholderTextBox),
                new PropertyMetadata("", OnPlaceholderChanged));

        private static void OnPlaceholderChanged(DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (PlaceholderTextBox)d;
            control.placeholder.Text = e.NewValue?.ToString();
            control.UpdatePlaceholderVisibility();
        }

        public string Text
        {
            get { return textBox.Text; }
            set { textBox.Text = value; }
        }

        public PlaceholderTextBox()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                placeholder.Text = Placeholder;
                UpdatePlaceholderVisibility();
            };
        }

        // ✅ ВОТ ЭТОТ МЕТОД ДОЛЖЕН БЫТЬ!
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }

        private void UpdatePlaceholderVisibility()
        {
            placeholder.Visibility = string.IsNullOrEmpty(textBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}