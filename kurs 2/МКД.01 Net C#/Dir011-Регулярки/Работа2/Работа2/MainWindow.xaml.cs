using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;

namespace Работа2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string textFirstNameGood = "";
        string textSecondNameGood = "";
        string textThreeNameGood = "";
        List<string> emails = new List<string>(new string[] { "ikazarbekov@bk.ru" ,"hm@hm.hm", "Hello@gg.com", "yoyoy@gmail.com", "windowsgovno@microsoftgovno.onigovno"});
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateUI()
        {
            // Get text problem
            TextRange textRangeProblem = new TextRange(textBoxProblem.Document.ContentStart, textBoxProblem.Document.ContentEnd);
            string textProblem = textRangeProblem.Text;



            // Activeted button push
            if (textBoxFirstName.Text.Length > 3 &&
                textBoxSecondName.Text.Length > 3 &&
                (textBoxThreeName.Text.Length == 0 || textBoxThreeName.Text.Length > 3) &&
                textProblem.Length > 2 &&
                textBoxEmail.Text.Length > 0)
                buttonPush.IsEnabled = true;
            else buttonPush.IsEnabled = false;

            //MessageBox.Show("" + textProblem.Length);
        }

        private void textBoxProblem_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
            // Replace text of problem
            TextRange textRangeProblem = new TextRange(textBoxProblem.Document.ContentStart, textBoxProblem.Document.ContentEnd);
            string textProblem = textRangeProblem.Text;
            string newTextProblem = Regex.Replace(textProblem, "Лиана Рамилевна", "Лучший куратор в мире!!!");
            newTextProblem = Regex.Replace(newTextProblem, "Воронцов", "пж простите, не буду больше такое писать :(");
            newTextProblem = Regex.Replace(newTextProblem, "Иванов Евгений", "пж простите, не буду больше такое писать :(");
            if (textProblem != newTextProblem)
                textRangeProblem.Text = newTextProblem;
            textBoxProblem.CaretPosition = textBoxProblem.Document.ContentEnd;

            // Save good version text problem
            if (textProblem.Length > 200 ||
                Regex.IsMatch(textProblem, @"[a-zA-Z]"))
                textRangeProblem.Text = textProblemGood;
            else
                textProblemGood = textRangeProblem.Text;
        }
        string textProblemGood = "";

        private void textBoxFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
            CurrectedName(textBoxFirstName, true);
        }

        private void textBoxSecondName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
            CurrectedName(textBoxSecondName);
        }

        private void textBoxThreeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();
            CurrectedName(textBoxThreeName);
        }

        private void textBoxEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUI();

            var text = textBoxEmail.Text;

            text = Regex.Replace(text, @"/s", "");
            text = Regex.Replace(text, @"[^A-Za-z\d\@\._]", "");

            textBoxEmail.Text = text;
            textBoxEmail.SelectionStart = text.Length;
        }

        void CurrectedName(TextBox textBox, bool doubleName = false)
        {
            string str = textBox.Text;
            if (str.Length > 0)
            {
                str = str.Trim();
                if (!doubleName)
                    str = Regex.Replace(str, @"[^А-Яа-я]", "");
                else
                    str = Regex.Replace(str, @"[^А-Яа-я\-]", "");
                if (str.Length != 0)
                {
                    var stringBuilder = new StringBuilder();
                    char d = '0';
                    foreach (char c in str)
                    {
                        if (d == '-')
                            stringBuilder.Append($"{c}".ToUpper());
                        else
                            stringBuilder.Append($"{c}".ToLower());
                        d = c;
                    }
                    stringBuilder[0] = stringBuilder[0].ToString().ToUpper()[0];
                    str = stringBuilder.ToString();
                }
                
            }
            textBox.Text = str;
            textBox.SelectionStart = str.Length;
        }

        private void buttonPush_Click(object sender, RoutedEventArgs e)
        {
            bool flagGoodData = true;
            // Close if name == Artem
            if (textBoxFirstName.Text.ToLower() == "артём" || textBoxSecondName.Text.ToLower() == "артём" || textBoxThreeName.Text.ToLower() == "артём")
                Close();
            // If text is small
            TextRange textRangeProblem = new TextRange(textBoxProblem.Document.ContentStart, textBoxProblem.Document.ContentEnd);
            string text = textRangeProblem.Text;
            if (text.Length < 10)
            {
                textBoxProblem.Background = Brushes.Red;
                MessageBox.Show("Введите проблему более 10 символов");
                flagGoodData = false;
            }
            else
                textBoxProblem.Background = Brushes.White;
                
            if (textBoxFirstName.Text.Length < 3)
            {
                MessageBox.Show("Введите фамилию более 3 символов");
                textBoxFirstName.Background = Brushes.Red;
                flagGoodData = false;
            }
            else textBoxFirstName.Background = Brushes.White;
            if (textBoxFirstName.Text.Length < 3)
            {
                textBoxSecondName.Background = Brushes.Red;
                MessageBox.Show("Введите имя более 3 символов");
                flagGoodData = false;
            }
            else textBoxSecondName.Background = Brushes.White;
            int lengthThreeName = textBoxFirstName.Text.Length;
            if (lengthThreeName < 3 && lengthThreeName != 0)
            {
                textBoxThreeName.Background = Brushes.Red;
                MessageBox.Show("Введите отчество более 3 символов");
                flagGoodData = false;
            }
            else textBoxThreeName.Background = Brushes.White;

            // Error email
            var email = textBoxEmail.Text;
            

            // Contains email
            if (emails.Contains(email)) {
                textBoxEmail.Background = Brushes.Red;
                MessageBox.Show("Такая почта уже отправляла жалобу");
                flagGoodData = false;
            }
            else if (!Regex.IsMatch(email, @"^[\w\._]+@(gmail|mail|bk)\.(ru|com)$"))
            {
                textBoxEmail.Background = Brushes.Red;
                MessageBox.Show("Некоректная почта");
                flagGoodData = false;
            }
            else textBoxEmail.Background = Brushes.White;

            // if error double name
            string firstName = textBoxFirstName.Text;
            int countFirstName = 0;
            foreach(char c in firstName)
                if (c == '-')
                    countFirstName++;
            if (firstName.StartsWith("-") || firstName.EndsWith("-") || countFirstName > 1)
            {
                textBoxFirstName.Background = Brushes.Red;
                MessageBox.Show("Двойная фамилия не верно записана");
                flagGoodData = false;
            }
            else 
                textBoxFirstName.Background = Brushes.White;

            // Good
            if (flagGoodData)
            {
                MessageBox.Show("Жалоба принята и отправляется");
                emails.Add(email);
            }
        }
    }
}
