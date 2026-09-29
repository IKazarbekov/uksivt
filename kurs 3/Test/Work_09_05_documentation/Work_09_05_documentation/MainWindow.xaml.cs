using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Work_09_05_documentation.Data;
using Work_09_05_documentation.Models;

namespace Work_09_05_documentation
{
        public partial class MainWindow : Window
        {
            private User currentUser;

            public MainWindow()
            {
                InitializeComponent();

                // Создаём тестовых пользователей, если база пуста
                if (!DataStore.Instance.Users.Any())
                {
                    DataStore.Instance.Users.Add(new User { Id = 1, Username = "admin", Password = "123", FullName = "Администратор", Role = "Технический оператор" });
                    DataStore.Instance.Users.Add(new User { Id = 2, Username = "emp1", Password = "123", FullName = "Петров Петр", Role = "Сотрудник" });
                    DataStore.Instance.Save();
                }

                EmployeeDate.Text = DateTime.Today.ToShortDateString();
            }

            // ---- Вход ----
            private void LoginButton_Click(object sender, RoutedEventArgs e)
            {
                var user = DataStore.Instance.GetUser(LoginUsername.Text, LoginPassword.Password);
                if (user != null)
                {
                    currentUser = user;
                    LoginMessage.Text = "";
                    ShowUserPanel(user);
                }
                else
                {
                    LoginMessage.Text = "Неверный логин или пароль";
                }
            }

            // ---- Регистрация ----
            private void RegisterButton_Click(object sender, RoutedEventArgs e)
            {
                string role = (RegRole.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (string.IsNullOrEmpty(RegUsername.Text) || string.IsNullOrEmpty(RegPassword.Password) ||
                    string.IsNullOrEmpty(RegFullName.Text) || string.IsNullOrEmpty(role))
                {
                    RegisterMessage.Text = "Заполните все поля";
                    RegisterMessage.Foreground = System.Windows.Media.Brushes.Red;
                    return;
                }

                var newUser = new User
                {
                    Username = RegUsername.Text,
                    Password = RegPassword.Password,
                    FullName = RegFullName.Text,
                    Role = role
                };
                bool ok = DataStore.Instance.AddUser(newUser);
                if (ok)
                {
                    RegisterMessage.Text = "Регистрация успешна! Теперь войдите.";
                    RegisterMessage.Foreground = System.Windows.Media.Brushes.Green;
                    RegUsername.Text = RegPassword.Password = RegFullName.Text = "";
                    LoginPanel.Visibility = Visibility.Visible;
                    RegisterPanel.Visibility = Visibility.Collapsed;
                }
                else
                {
                    RegisterMessage.Text = "Пользователь с таким логином уже существует";
                    RegisterMessage.Foreground = System.Windows.Media.Brushes.Red;
                }
            }

            // ---- Переключение панелей ----
            private void ShowRegisterButton_Click(object sender, RoutedEventArgs e)
            {
                LoginPanel.Visibility = Visibility.Collapsed;
                RegisterPanel.Visibility = Visibility.Visible;
            }

            private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
            {
                RegisterPanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;
            }

            private void ShowUserPanel(User user)
            {
                LoginPanel.Visibility = Visibility.Collapsed;
                RegisterPanel.Visibility = Visibility.Collapsed;

                if (user.Role == "Сотрудник")
                {
                    EmployeePanel.Visibility = Visibility.Visible;
                    OperatorPanel.Visibility = Visibility.Collapsed;
                    EmployeeFullName.Text = user.FullName;
                    EmployeeNameText.Text = user.FullName;
                    LoadEmployeeRequests();
                }
                else // Технический оператор
                {
                    OperatorPanel.Visibility = Visibility.Visible;
                    EmployeePanel.Visibility = Visibility.Collapsed;
                    OperatorNameText.Text = user.FullName;
                    LoadAllRequests();
                    LoadFilterComboBoxes();
                    OperatorInfo.Text = "";
                }
            }

            private void LogoutButton_Click(object sender, RoutedEventArgs e)
            {
                currentUser = null;
                EmployeePanel.Visibility = Visibility.Collapsed;
                OperatorPanel.Visibility = Visibility.Collapsed;
                LoginPanel.Visibility = Visibility.Visible;
                LoginPassword.Password = "";
                LoginMessage.Text = "";
            }

            // ---- Сотрудник: Создание заявки ----
            private void SendRequestButton_Click(object sender, RoutedEventArgs e)
            {
                if (currentUser == null) return;
                if (string.IsNullOrEmpty(EmployeeCabinet.Text) || string.IsNullOrEmpty(EmployeeDescription.Text))
                {
                    MessageBox.Show("Заполните кабинет и описание");
                    return;
                }
                var request = new Request
                {
                    UserId = currentUser.Id,
                    FullName = currentUser.FullName,
                    Date = DateTime.Today,
                    Description = EmployeeDescription.Text,
                    Cabinet = EmployeeCabinet.Text,
                    Status = "Новая"
                };
                DataStore.Instance.AddRequest(request);
                MessageBox.Show("Заявка отправлена");
                EmployeeCabinet.Text = "";
                EmployeeDescription.Text = "";
                LoadEmployeeRequests();
            }

            // ---- Сотрудник: загрузка своих заявок ----
            private void LoadEmployeeRequests()
            {
                if (currentUser == null) return;
                var list = DataStore.Instance.GetRequestsByUser(currentUser.Id);
                EmployeeRequestsListView.ItemsSource = list;
            }

            private void RefreshEmployeeRequests_Click(object sender, RoutedEventArgs e) => LoadEmployeeRequests();

            // ---- Техоператор: загрузка всех заявок ----
            private void LoadAllRequests()
            {
                var list = DataStore.Instance.GetAllRequests();
                OperatorRequestsListView.ItemsSource = list;
                UpdateOperatorInfo(list);
            }

            private void UpdateOperatorInfo(List<Request> list)
            {
                int total = list.Count;
                int today = list.Count(r => r.Date.Date == DateTime.Today);
                int completedToday = list.Count(r => r.Date.Date == DateTime.Today && r.Status == "Выполнил");
                int inProgress = list.Count(r => r.Status == "Взял в работу");
                OperatorInfo.Text = $"Всего: {total} | За сегодня: {today} | Выполнено сегодня: {completedToday} | В работе: {inProgress}";
            }

            // ---- Фильтры для оператора ----
            private void LoadFilterComboBoxes()
            {
                FilterEmployee.Items.Clear();
                FilterEmployee.Items.Add("Все");
                foreach (var emp in DataStore.Instance.GetAllEmployees())
                    FilterEmployee.Items.Add(emp);

                FilterCabinet.Items.Clear();
                FilterCabinet.Items.Add("Все");
                foreach (var cab in DataStore.Instance.GetAllCabinets())
                    FilterCabinet.Items.Add(cab);
            }

            private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
            {
                int? employeeId = null;
                if (FilterEmployee.SelectedItem is User emp)
                    employeeId = emp.Id;

                string cabinet = null;
                if (FilterCabinet.SelectedItem is string cab && cab != "Все")
                    cabinet = cab;

                DateTime? date = FilterDate.SelectedDate;

                var filtered = DataStore.Instance.FilterRequests(employeeId, cabinet, date);
                OperatorRequestsListView.ItemsSource = filtered;
                UpdateOperatorInfo(filtered);
            }

            private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
            {
                FilterEmployee.SelectedIndex = 0;
                FilterCabinet.SelectedIndex = 0;
                FilterDate.SelectedDate = null;
                LoadAllRequests();
            }

            // ---- Действия с заявкой (оператор) ----
            private Request GetSelectedRequest() => OperatorRequestsListView.SelectedItem as Request;

            private void TakeRequestButton_Click(object sender, RoutedEventArgs e)
            {
                var req = GetSelectedRequest();
                if (req == null) { MessageBox.Show("Выберите заявку"); return; }
                if (req.Status != "Новая") { MessageBox.Show("Заявка уже не новая"); return; }
                DataStore.Instance.UpdateRequestStatus(req.Id, "Взял в работу");
                LoadAllRequests();
            }

            private void CompleteRequestButton_Click(object sender, RoutedEventArgs e)
            {
                var req = GetSelectedRequest();
                if (req == null) { MessageBox.Show("Выберите заявку"); return; }
                if (req.Status != "Взял в работу") { MessageBox.Show("Заявка не в работе"); return; }
                DataStore.Instance.UpdateRequestStatus(req.Id, "Выполнил");
                LoadAllRequests();
            }

            private void CancelRequestButton_Click(object sender, RoutedEventArgs e)
            {
                var req = GetSelectedRequest();
                if (req == null) { MessageBox.Show("Выберите заявку"); return; }
                DataStore.Instance.UpdateRequestStatus(req.Id, "Отмена");
                LoadAllRequests();
            }

            private void RefreshOperatorRequests_Click(object sender, RoutedEventArgs e) => LoadAllRequests();

            private void ShowTodayCompleted_Click(object sender, RoutedEventArgs e)
            {
                var all = DataStore.Instance.GetAllRequests();
                var todayCompleted = all.Where(r => r.Date.Date == DateTime.Today && r.Status == "Выполнил").ToList();
                OperatorRequestsListView.ItemsSource = todayCompleted;
                UpdateOperatorInfo(todayCompleted);
            }
        }
    }