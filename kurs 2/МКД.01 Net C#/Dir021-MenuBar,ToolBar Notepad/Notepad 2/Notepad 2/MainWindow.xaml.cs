using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Notepad_2
{
    public partial class MainWindow : Window
    {
        private Encoding _currentEncoding = Encoding.UTF8;
        private double _currentZoomPercentage = 100;
        private bool _isDarkTheme = false;

        public MainWindow()
        {
            InitializeComponent();
            SetupBuiltInCommands();

            // Инициализация первой пустой вкладки
            CreateNewTab("Новый документ.txt");
            UpdateSidebar();
        }

        #region Архитектура Номеров Строк и Генерация Кастомного Текстового Блока

        /// <summary>
        /// Создает кастомный контейнер, внутри которого TextBox синхронизирован с панелью номеров строк.
        /// </summary>
        private Grid CreateNumberedTextBoxContainer(string initialText = "")
        {
            Grid mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Номера
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Поле текста

            // Панель отображения номеров строк
            TextBox lineNumbersBox = new TextBox
            {
                Width = 45,
                Background = _isDarkTheme ? new SolidColorBrush(Color.FromRgb(45, 45, 48)) : new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                Foreground = new SolidColorBrush(Color.FromRgb(128, 128, 128)),
                BorderThickness = new Thickness(0, 0, 1, 0),
                BorderBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224)),
                Focusable = false,
                IsReadOnly = true,
                IsHitTestVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                TextAlignment = TextAlignment.Right,
                Padding = new Thickness(0, 10, 5, 0),
                FontSize = 16,
                FontFamily = new FontFamily("Consolas"),
                Text = "1"
            };

            // Главное текстовое поле редактирования
            TextBox mainTextBox = new TextBox
            {
                AcceptsReturn = true,
                AcceptsTab = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(10, 10, 10, 10),
                FontSize = 16,
                FontFamily = new FontFamily("Consolas"),
                Text = initialText,
                Background = _isDarkTheme ? new SolidColorBrush(Color.FromRgb(30, 30, 30)) : Brushes.White,
                Foreground = _isDarkTheme ? Brushes.LightGray : Brushes.Black
            };

            // Синхронизация прокрутки номеров строк и основного текста
            mainTextBox.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler((s, e) =>
            {
                lineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset);
            }));

            // Логика динамического пересчета строк при вводе текста
            mainTextBox.TextChanged += (s, e) =>
            {
                UpdateLineNumbers(mainTextBox, lineNumbersBox);
                UpdateStatusBarStats();
            };

            // Поддержка изменения масштаба шрифта (Зум) через Ctrl + Колесо мыши
            mainTextBox.PreviewMouseWheel += (s, e) =>
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    e.Handled = true;
                    if (e.Delta > 0 && _currentZoomPercentage < 300) _currentZoomPercentage += 10;
                    else if (e.Delta < 0 && _currentZoomPercentage > 50) _currentZoomPercentage -= 10;

                    double newSize = 16 * (_currentZoomPercentage / 100);
                    mainTextBox.FontSize = newSize;
                    lineNumbersBox.FontSize = newSize;
                    lblZoom.Text = $"Масштаб: {_currentZoomPercentage}%";
                }
            };

            // Установка видимости на основе настроек меню
            lineNumbersBox.Visibility = menuShowLineNumbers.IsChecked ? Visibility.Visible : Visibility.Collapsed;
            mainTextBox.TextWrapping = menuWordWrap.IsChecked ? TextWrapping.Wrap : TextWrapping.NoWrap;

            Grid.SetColumn(lineNumbersBox, 0);
            Grid.SetColumn(mainTextBox, 1);

            mainGrid.Children.Add(lineNumbersBox);
            mainGrid.Children.Add(mainTextBox);

            // Первичный просчет при создании
            UpdateLineNumbers(mainTextBox, lineNumbersBox);

            return mainGrid;
        }

        private void UpdateLineNumbers(TextBox input, TextBox output)
        {
            int lineCount = input.LineCount;
            if (lineCount < 1) lineCount = 1;

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= lineCount; i++)
            {
                sb.AppendLine(i.ToString());
            }
            output.Text = sb.ToString();
        }

        #endregion

        #region Управление вкладками профессионального уровня

        private void CreateNewTab(string headerText, string contentText = "", string path = "")
        {
            Grid editorGrid = CreateNumberedTextBoxContainer(contentText);
            TabItemData tabData = new TabItemData { HeaderText = headerText, FilePath = path };

            TabItem newTab = new TabItem
            {
                DataContext = tabData,
                Content = editorGrid
            };

            tabControlFiles.Items.Add(newTab);
            tabControlFiles.SelectedItem = newTab;

            UpdateSidebar();
            UpdateStatusBarStats();
        }

        private TextBox GetCurrentTextBox()
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.Content is Grid grid)
            {
                return grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
            }
            return null;
        }

        private TextBox GetCurrentLineNumbersBox()
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.Content is Grid grid)
            {
                return grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);
            }
            return null;
        }

        private void CloseCurrentTab()
        {
            if (tabControlFiles.Items.Count > 1)
            {
                int currentIndex = tabControlFiles.SelectedIndex;
                tabControlFiles.Items.Remove(tabControlFiles.SelectedItem);
                tabControlFiles.SelectedIndex = currentIndex == 0 ? 0 : currentIndex - 1;
            }
            else
            {
                TextBox box = GetCurrentTextBox();
                if (box != null) box.Text = "";
                if (tabControlFiles.SelectedItem is TabItem tab && tab.DataContext is TabItemData data)
                {
                    data.HeaderText = "Новый документ.txt";
                    data.FilePath = "";
                }
            }
            UpdateSidebar();
            UpdateStatusBarStats();
        }

        #endregion

        #region Продвинутые Алгоритмы Поиска и Замены (Регулярные Выражения)

        private void ExecuteSearch(string searchText, bool searchForward)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox == null || string.IsNullOrEmpty(searchText)) return;

            string sourceText = textBox.Text;
            int currentSelectionStart = textBox.SelectionStart;

            try
            {
                if (buttonRegex.IsChecked == true)
                {
                    // Режим поиска по регулярным выражениям
                    RegexOptions options = buttonWithCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase;
                    MatchCollection matches = Regex.Matches(sourceText, searchText, options);

                    if (matches.Count == 0)
                    {
                        MessageBox.Show("Регулярное выражение не дало результатов.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    Match targetMatch = null;
                    if (searchForward)
                    {
                        targetMatch = matches.Cast<Match>().FirstOrDefault(m => m.Index > currentSelectionStart) ?? matches[0];
                    }
                    else
                    {
                        targetMatch = matches.Cast<Match>().LastOrDefault(m => m.Index < currentSelectionStart) ?? matches[matches.Count - 1];
                    }

                    textBox.Focus();
                    textBox.Select(targetMatch.Index, targetMatch.Length);
                }
                else
                {
                    // Обычный классический поиск текста
                    StringComparison comp = buttonWithCase.IsChecked == true ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                    int index = -1;

                    if (searchForward)
                    {
                        index = sourceText.IndexOf(searchText, currentSelectionStart + textBox.SelectionLength, comp);
                        if (index == -1) index = sourceText.IndexOf(searchText, 0, comp); // Цикл
                    }
                    else
                    {
                        int start = currentSelectionStart - 1;
                        if (start >= 0) index = sourceText.LastIndexOf(searchText, start, comp);
                        if (index == -1) index = sourceText.LastIndexOf(searchText, sourceText.Length - 1, comp); // Цикл
                    }

                    if (index != -1)
                    {
                        textBox.Focus();
                        textBox.Select(index, searchText.Length);
                    }
                    else
                    {
                        MessageBox.Show("Текст не найден.", "Поиск", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка в синтаксисе регулярного выражения:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonFindNext_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxFind.Text, true);
        private void buttonFindBack_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxFind.Text, false);
        private void Button_FindNext_ReplacePanel_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxReplaceFind.Text, true);

        private void Button_Replace_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = GetCurrentTextBox();
            string findText = textBoxReplaceFind.Text;
            string replaceText = textBoxReplaceText.Text;

            if (textBox == null || string.IsNullOrEmpty(findText)) return;

            if (textBox.SelectedText.Equals(findText, buttonWithCase.IsChecked == true ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
            {
                textBox.SelectedText = replaceText;
            }
            ExecuteSearch(findText, true);
        }

        private void Button_ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = GetCurrentTextBox();
            string findText = textBoxReplaceFind.Text;
            string replaceText = textBoxReplaceText.Text;

            if (textBox == null || string.IsNullOrEmpty(findText)) return;

            if (buttonRegex.IsChecked == true)
            {
                RegexOptions options = buttonWithCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase;
                textBox.Text = Regex.Replace(textBox.Text, findText, replaceText, options);
            }
            else
            {
                string text = textBox.Text;
                if (buttonWithCase.IsChecked == true)
                {
                    text = text.Replace(findText, replaceText);
                }
                else
                {
                    text = Regex.Replace(text, Regex.Escape(findText), replaceText, RegexOptions.IgnoreCase);
                }
                textBox.Text = text;
            }
            MessageBox.Show("Глобальная замена завершена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region Системная Печать Документов

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument();
                doc.PagePadding = new Thickness(60);
                doc.Background = Brushes.White;

                // Чтение строк для красивой разметки абзацев на листе бумаги
                string[] lines = textBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    Paragraph para = new Paragraph(new Run(line))
                    {
                        Margin = new Thickness(0, 0, 0, 2),
                        FontFamily = textBox.FontFamily,
                        FontSize = 12,
                        Foreground = Brushes.Black
                    };
                    doc.Blocks.Add(para);
                }

                doc.PageWidth = printDialog.PrintableAreaWidth;
                doc.PageHeight = printDialog.PrintableAreaHeight;

                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Печать текстового файла");
                textBlockInfo.Text = "Документ успешно отпечатан.";
            }
        }

        #endregion

        #region Базовый функционал работы с файловой системой (Ввод-Вывод)

        private void Button_Click_New_File(object sender, RoutedEventArgs e) => CreateNewTab("Новый документ.txt");

        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Лог файлы (*.log)|*.log|Все файлы (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    string content = File.ReadAllText(filename, _currentEncoding);
                    CreateNewTab(Path.GetFileName(filename), content, filename);
                }
            }
        }

        private void Button_Click_Save_File(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                TextBox textBox = GetCurrentTextBox();
                if (textBox == null) return;

                if (string.IsNullOrEmpty(tabData.FilePath))
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" };
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        tabData.FilePath = saveFileDialog.FileName;
                        tabData.HeaderText = Path.GetFileName(saveFileDialog.FileName);
                    }
                    else return;
                }

                File.WriteAllText(tabData.FilePath, textBox.Text, _currentEncoding);
                textBlockInfo.Text = $"Сохранено: {tabData.HeaderText}";
                UpdateSidebar();
            }
        }

        private void Button_Click_Save_File_As(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                TextBox textBox = GetCurrentTextBox();
                if (textBox == null) return;

                SaveFileDialog saveFileDialog = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" };
                if (saveFileDialog.ShowDialog() == true)
                {
                    tabData.FilePath = saveFileDialog.FileName;
                    tabData.HeaderText = Path.GetFileName(saveFileDialog.FileName);
                    File.WriteAllText(tabData.FilePath, textBox.Text, _currentEncoding);
                    UpdateSidebar();
                }
            }
        }

        private void MenuItem_SaveAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.DataContext is TabItemData tabData && item.Content is Grid grid)
                {
                    TextBox box = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                    if (box != null && !string.IsNullOrEmpty(tabData.FilePath))
                    {
                        File.WriteAllText(tabData.FilePath, box.Text, _currentEncoding);
                    }
                }
            }
            textBlockInfo.Text = "Все открытые файлы сохранены.";
        }

        private void Button_Click_Update_File(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                if (!string.IsNullOrEmpty(tabData.FilePath) && File.Exists(tabData.FilePath))
                {
                    TextBox box = GetCurrentTextBox();
                    if (box != null) box.Text = File.ReadAllText(tabData.FilePath, _currentEncoding);
                    textBlockInfo.Text = "Файл перечитан с диска.";
                }
            }
        }

        #endregion

        #region Умные алгоритмы трансформации текста

        private void MenuItem_Uppercase_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            if (box.SelectionLength > 0) box.SelectedText = box.SelectedText.ToUpper();
            else box.Text = box.Text.ToUpper();
        }

        private void MenuItem_Lowercase_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            if (box.SelectionLength > 0) box.SelectedText = box.SelectedText.ToLower();
            else box.Text = box.Text.ToLower();
        }

        private void MenuItem_RemoveEmptyLines_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            var nonLines = box.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                                  .Where(line => !string.IsNullOrWhiteSpace(line));
            box.Text = string.Join(Environment.NewLine, nonLines);
        }

        private void MenuItem_SortLines_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            var lines = box.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).OrderBy(x => x);
            box.Text = string.Join(Environment.NewLine, lines);
        }

        private void MenuItem_InsertDateTime_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            int caret = box.CaretIndex;
            string dtStr = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            box.Text = box.Text.Insert(caret, dtStr);
            box.CaretIndex = caret + dtStr.Length;
        }

        #endregion

        #region Темы, Шрифты, Интерфейс и Кодировки

        private void LineNumbers_Toggle(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles == null) return;
            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    TextBox numbersBox = grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);
                    if (numbersBox != null)
                        numbersBox.Visibility = menuShowLineNumbers.IsChecked ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        private void WordWrap_Toggle(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles == null) return;
            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    TextBox mainBox = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                    if (mainBox != null)
                        mainBox.TextWrapping = menuWordWrap.IsChecked ? TextWrapping.Wrap : TextWrapping.NoWrap;
                }
            }
        }

        private void Encoding_Changed(object sender, RoutedEventArgs e)
        {
            if (rbUTF8.IsChecked == true) { _currentEncoding = Encoding.UTF8; lblEncoding.Text = "Кодировка: UTF-8"; }
            else if (rbASCII.IsChecked == true) { _currentEncoding = Encoding.ASCII; lblEncoding.Text = "Кодировка: ASCII"; }
            else if (rbUnicode.IsChecked == true) { _currentEncoding = Encoding.Unicode; lblEncoding.Text = "Кодировка: UTF-16"; }
        }

        private void Theme_Light_Click(object sender, RoutedEventArgs e)
        {
            _isDarkTheme = false;
            ApplyThemeToAllTabs();
        }

        private void Theme_Dark_Click(object sender, RoutedEventArgs e)
        {
            _isDarkTheme = true;
            ApplyThemeToAllTabs();
        }

        private void ApplyThemeToAllTabs()
        {
            SolidColorBrush bg = _isDarkTheme ? new SolidColorBrush(Color.FromRgb(30, 30, 30)) : Brushes.White;
            SolidColorBrush fg = _isDarkTheme ? Brushes.LightGray : Brushes.Black;
            SolidColorBrush numBg = _isDarkTheme ? new SolidColorBrush(Color.FromRgb(45, 45, 48)) : new SolidColorBrush(Color.FromRgb(240, 240, 240));

            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    TextBox main = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                    TextBox num = grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);

                    if (main != null) { main.Background = bg; main.Foreground = fg; }
                    if (num != null) { num.Background = numBg; }
                }
            }
        }

        #endregion

        #region Синхронизация Статус-Бара, Боковой панели и Навигации

        private void UpdateStatusBarStats()
        {
            TextBox box = GetCurrentTextBox();
            if (box != null)
            {
                lblLinesCount.Text = $"Строк: {box.LineCount}";
                lblCharCount.Text = $"Символов: {box.Text.Length}";
            }
        }

        private void UpdateSidebar()
        {
            if (lstSidebarFiles == null) return;
            List<TabItemData> fileList = new List<TabItemData>();
            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.DataContext is TabItemData data) fileList.Add(data);
            }
            lstSidebarFiles.ItemsSource = fileList;
        }

        private void TabControlFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStatusBarStats();
        }

        private void LstSidebarFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSidebarFiles.SelectedItem is TabItemData selectedData)
            {
                TabItem target = tabControlFiles.Items.Cast<TabItem>().FirstOrDefault(x => x.DataContext == selectedData);
                if (target != null) tabControlFiles.SelectedItem = target;
            }
        }

        private void Click_Left_Panel_Items(object sender, RoutedEventArgs e)
        {
            LeftSidebar.Visibility = LeftSidebar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void Click_To_First_Item(object sender, RoutedEventArgs e) => tabControlFiles.SelectedIndex = 0;
        private void Click_To_End_Item(object sender, RoutedEventArgs e) => tabControlFiles.SelectedIndex = tabControlFiles.Items.Count - 1;
        private void Click_To_Before_Item(object sender, RoutedEventArgs e) { if (tabControlFiles.SelectedIndex < tabControlFiles.Items.Count - 1) tabControlFiles.SelectedIndex++; }
        private void Click_To_Back_Item(object sender, RoutedEventArgs e) { if (tabControlFiles.SelectedIndex > 0) tabControlFiles.SelectedIndex--; }

        #endregion

        #region Системная обвязка горячих клавиш и команд

        private void SetupBuiltInCommands()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, (s, e) => { ReplacePanel.Visibility = Visibility.Collapsed; SearchPanel.Visibility = SearchPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; if (SearchPanel.Visibility == Visibility.Visible) textBoxFind.Focus(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Replace, (s, e) => { SearchPanel.Visibility = Visibility.Collapsed; ReplacePanel.Visibility = ReplacePanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; if (ReplacePanel.Visibility == Visibility.Visible) textBoxReplaceFind.Focus(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, Button_Click_New_File));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Button_Click_Open_File));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Button_Click_Save_File));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Button_Click_Print));

            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, (s, e) => GetCurrentTextBox()?.Undo(), (s, e) => e.CanExecute = GetCurrentTextBox()?.CanUndo ?? false));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, (s, e) => GetCurrentTextBox()?.Redo(), (s, e) => e.CanExecute = GetCurrentTextBox()?.CanRedo ?? false));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, (s, e) => GetCurrentTextBox()?.Cut(), (s, e) => e.CanExecute = GetCurrentTextBox()?.SelectionLength > 0));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, (s, e) => GetCurrentTextBox()?.Copy(), (s, e) => e.CanExecute = GetCurrentTextBox()?.SelectionLength > 0));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (s, e) => GetCurrentTextBox()?.Paste(), (s, e) => e.CanExecute = Clipboard.ContainsText()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, (s, e) => { if (GetCurrentTextBox() != null) GetCurrentTextBox().SelectedText = ""; }, (s, e) => e.CanExecute = GetCurrentTextBox()?.SelectionLength > 0));
        }

        #endregion

        #region Заглушки, Закрытие окон и Всплывающие Окна

        private void CloseSearchPanel_Click(object sender, RoutedEventArgs e) => SearchPanel.Visibility = Visibility.Collapsed;
        private void CloseReplacePanel_Click(object sender, RoutedEventArgs e) => ReplacePanel.Visibility = Visibility.Collapsed;
        private void Button_Click_Close(object sender, RoutedEventArgs e) => CloseCurrentTab();
        private void CloseTab_Click(object sender, RoutedEventArgs e) => CloseCurrentTab();
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => this.Close();
        private void Click_Take_Out_Item(object sender, RoutedEventArgs e) => MessageBox.Show("Функция переноса листа в окно Windows 11 будет доступна в следующем обновлении.", "Архитектура Вкладок");
        private void MenuItem_Click_OpenFontSettings(object sender, RoutedEventArgs e) => MessageBox.Show("Системный диалог шрифтов будет подключен динамически через User32.", "Настройки");
        private void Button_Click_Help(object sender, RoutedEventArgs e) => MessageBox.Show("FeatherPad Pro v3.5\n\n- Номера строк работают автоматически при вводе текста!\n- Поддерживается зум шрифта (Ctrl + колесико мыши)\n- Продвинутые Regex-выражения для поиска\n- Полноценная печать с автоматической калибровкой полей.", "Справка");
        private void Click_To_Last_Item(object sender, RoutedEventArgs e) { }
        private void Window_Closing(object sender, CancelEventArgs e) { var res = MessageBox.Show("Вы хотите закрыть приложение?", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Question); if (res == MessageBoxResult.No) e.Cancel = true; }

        #endregion
    }
}