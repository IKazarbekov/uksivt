using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Shell;
using static System.Net.Mime.MediaTypeNames;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для WindowRegistration.xaml
    /// </summary>
    public partial class WindowRegistration : Window
    {
        Runner runnerEdit = null;
        bool notEnd = false;
        public WindowRegistration(Runner runnerEdit = null)
        {
            InitializeComponent();
            if (runnerEdit != null) {
                textBoxEmail.Text = runnerEdit.Email;
                textBoxFirstPassword.Text = runnerEdit.Password;
                textBoxSecondPassword.Text = runnerEdit.Password;
                textBoxFirstName.Text = runnerEdit.FirstName;
                textBoxSecondName.Text = runnerEdit.LastName;
                comboBoxCountry.Text = runnerEdit.Country;
                comboBoxGender.Text = runnerEdit.Gender;
                textBoxImage.Text = runnerEdit.PhotoPath;
                dateTimePicker.SelectedDate = runnerEdit.BirthDate;

                this.runnerEdit = runnerEdit;

                textBlockTitle.Text = "Изменение пользователя";
            }
        }

        public WindowRegistration(bool notEnd)
        {
            InitializeComponent();
            this.notEnd = notEnd;
        }

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            string email = textBoxEmail.Text;
            string firstPassword = textBoxFirstPassword.Text;
            string secondPassword = textBoxSecondPassword.Text;
            string firstName = textBoxFirstName.Text;
            string secondName = textBoxSecondName.Text;
            string country = comboBoxCountry.Text;
            string gender = comboBoxGender.Text;
            string photoPath = textBoxImage.Text;
            DateTime birthDate = DateTime.Now;

            string error = "";

            try
            {
                birthDate = dateTimePicker.SelectedDate.Value;
            }
            catch 
            {
                error += "Выберите дату рождения    ";
            }

            // 1. Проверка email (простая проверка на @ и точку)
            if (email == "")
            {
                error += "• Email обязателен    ";
            }
            else if (!email.Contains("@") || !email.Contains("."))
            {
                error += "• Email должен быть в формате x@x.x    ";
            }

            // 2. Проверка имени
            if (firstName == "")
            {
                error += "• Имя обязательно    ";
            }

            // 3. Проверка фамилии
            if (secondName == "")
            {
                error += "• Фамилия обязательна    ";
            }

            // 4. Проверка пароля
            if (firstPassword == "")
            {
                error += "• Пароль обязателен    ";
            }
            else
            {
                // Проверка длины
                if (firstPassword.Length < 6)
                    error += "• Пароль: минимум 6 символов    ";

                // Проверка на прописную букву
                bool hasUpper = false;
                for (int i = 0; i < firstPassword.Length; i++)
                {
                    if (firstPassword[i] >= 'A' && firstPassword[i] <= 'Z')
                    {
                        hasUpper = true;
                        break;
                    }
                }
                if (!hasUpper)
                    error += "• Пароль: минимум 1 прописная буква    ";

                // Проверка на цифру
                bool hasDigit = false;
                for (int i = 0; i < firstPassword.Length; i++)
                {
                    if (firstPassword[i] >= '0' && firstPassword[i] <= '9')
                    {
                        hasDigit = true;
                        break;
                    }
                }
                if (!hasDigit)
                    error += "• Пароль: минимум 1 цифра    ";

                // Проверка на спецсимволы ! @ # $ % ^
                bool hasSpecial = false;
                for (int i = 0; i < firstPassword.Length; i++)
                {
                    char c = firstPassword[i];
                    if (c == '!' || c == '@' || c == '#' || c == '$' || c == '%' || c == '^')
                    {
                        hasSpecial = true;
                        break;
                    }
                }
                if (!hasSpecial)
                    error += "• Пароль: минимум 1 спецсимвол (! @ # $ % ^)    ";
            }

            // 5. Проверка подтверждения пароля
            if (secondPassword == "")
            {
                error += "• Подтвердите пароль    ";
            }
            else if (firstPassword != secondPassword)
            {
                error += "• Пароли не совпадают    ";
            }

            // 6. Проверка пола (выбор из списка - у вас в ComboBox должны быть пункты)
            if (gender == "" || gender == "Выберите пол")
            {
                error += "• Выберите пол    ";
            }

            // 7. Проверка страны
            if (country == "" || country == "Выберите страну")
            {
                error += "• Выберите страну    ";
            }

            // 8. Проверка даты рождения
            try
            {
                birthDate = dateTimePicker.SelectedDate.Value;

                // Проверка возраста (не менее 10 лет)
                int age = DateTime.Now.Year - birthDate.Year;
                if (birthDate > DateTime.Now.AddYears(-age))
                {
                    age--;
                }

                if (age < 10)
                {
                    error += "• Возраст должен быть не менее 10 лет    ";
                }
            }
            catch
            {
                error += "• Выберите дату рождения    ";
            }

            // 9. Проверка фото
            if (photoPath == "")
            {
                error += "• Выберите фото    ";
            }

            if (runnerEdit == null)
                if (Data.Runners.Where(runner => runner.Email.Contains(email)).ToList().Count > 0)
                    error += "* почта занята";

            // Показываем ошибки
            if (error != "")
            {
                if (error.Length > 90) {
                    error = error.Insert(error.Length / 2, "\n");
                }

                textBlockError.Visibility = Visibility.Visible;
                textBlockError.Text = error;
                return;
            }

            Runner runner = new Runner()
            {
                Email = email,
                FirstName = firstName,
                LastName = secondName,
                Password = firstPassword,
                Country = country,
                Gender = gender,
                PhotoPath = photoPath,
                BirthDate = birthDate,
                Role = "Бегун"
            };
            
            // если это изменнеие бегуна
            if (runnerEdit != null)
            {
                Data.Runners.Remove(runnerEdit);
                Data.Runners.Add(runner);
                MessageBox.Show("Бегун изменён");
                string jsonn = JsonSerializer.Serialize(Data.Runners, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
                File.WriteAllText("json_file.txt", jsonn, Encoding.UTF8);
                Close();
                return;
            }

            Data.Runners.Add(runner);

            string json = JsonSerializer.Serialize(Data.Runners, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText("json_file.json", json, Encoding.UTF8);

            if (!notEnd)
                new WindowEndRegister().Show();
            else
                MessageBox.Show("Пользователь был добавлен");
            Close();
        }

        private void TopPanel_Click(object sender, RoutedEventArgs e)
        {
            if (runnerEdit != null)
            {
                Close();
                return;
            }

            new WindowQuestionIsRunner().Show();
            Close();
        }

        private void RoundButtonPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image|*.png|all|*.*";
            openFileDialog.ShowDialog();
            string path = openFileDialog.FileName;

            try
            {
                image.Source = new BitmapImage(new Uri(path));
            }
            catch
            {
                return;
            }

            textBoxImage.Text = path;
        }
    }
}
