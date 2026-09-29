using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace Notepad_2
{
    public partial class MainWindow : Window
    {
        private Encoding _currentEncoding = Encoding.UTF8;
        private double _globalZoom = 100;
        private double _baseFontSize = 16;
        private FontFamily _currentFontFamily = new FontFamily("Consolas");

        // Цветовые палитры схем кастомизации
        private readonly SolidColorBrush _lightBg = Brushes.White;
        private readonly SolidColorBrush _lightFg = Brushes.Black;
        private readonly SolidColorBrush _lightNumBg = new SolidColorBrush(Color.FromRgb(245, 245, 245));

        private readonly SolidColorBrush _darkBg = new SolidColorBrush(Color.FromRgb(28, 28, 28));
        private readonly SolidColorBrush _darkFg = new SolidColorBrush(Color.FromRgb(220, 220, 220));
        private readonly SolidColorBrush _darkNumBg = new SolidColorBrush(Color.FromRgb(40, 40, 40));

        private readonly SolidColorBrush _sepiaBg = new SolidColorBrush(Color.FromRgb(250, 242, 225));
        private readonly SolidColorBrush _sepiaFg = new SolidColorBrush(Color.FromRgb(90, 60, 40));
        private readonly SolidColorBrush _sepiaNumBg = new SolidColorBrush(Color.FromRgb(240, 230, 210));

        private readonly SolidColorBrush _cyberBg = new SolidColorBrush(Color.FromRgb(10, 12, 18));
        private readonly SolidColorBrush _cyberFg = new SolidColorBrush(Color.FromRgb(0, 255, 136));
        private readonly SolidColorBrush _cyberNumBg = new SolidColorBrush(Color.FromRgb(20, 22, 30));

        public MainWindow()
        {
            InitializeComponent();
            SetupBuiltInCommands();

            // Первичная сборка рабочей среды
            CreateNewTab("Новый документ.txt");
            UpdateSidebar();
        }

        #region Высокотехнологичная Нумерация Строк и Зум Вкладок

        private Grid CreateNumberedTextBoxContainer(string initialText = "")
        {
            Grid mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBox lineNumbersBox = new TextBox
            {
                Width = 50,
                BorderThickness = new Thickness(0, 0, 1, 0),
                BorderBrush = new SolidColorBrush(Color.FromRgb(225, 225, 225)),
                Focusable = false,
                IsReadOnly = true,
                IsHitTestVisible = false,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                TextAlignment = TextAlignment.Right,
                Padding = new Thickness(0, 10, 8, 0),
                FontFamily = _currentFontFamily,
                Text = "1"
            };

            TextBox mainTextBox = new TextBox
            {
                AcceptsReturn = true,
                AcceptsTab = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(12),
                FontFamily = _currentFontFamily,
                Text = initialText
            };

            // Синхронный скролл панелей
            mainTextBox.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler((s, e) =>
            {
                lineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset);
            }));

            // Отслеживание изменений текста (Dirty-состояние флага и пересчет строк)
            mainTextBox.TextChanged += (s, e) =>
            {
                UpdateLineNumbers(mainTextBox, lineNumbersBox);
                UpdateStatusBarStats();

                if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData data)
                {
                    if (!data.IsDirty && mainTextBox.Text.Length > 0)
                    {
                        data.IsDirty = true;
                    }
                }
            };

            // Изменение глобального зума через Ctrl + Мышь
            mainTextBox.PreviewMouseWheel += (s, e) =>
            {
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    e.Handled = true;
                    if (e.Delta > 0 && _globalZoom < 300) _globalZoom += 10;
                    else if (e.Delta < 0 && _globalZoom > 60) _globalZoom -= 10;

                    ApplyFontSizeEngine();
                }
            };

            Grid.SetColumn(lineNumbersBox, 0);
            Grid.SetColumn(mainTextBox, 1);

            mainGrid.Children.Add(lineNumbersBox);
            mainGrid.Children.Add(mainTextBox);

            // Срочно рассчитываем тему для нового контейнера
            ApplyCurrentThemeToContainer(mainTextBox, lineNumbersBox);
            ApplyFontSizeToContainer(mainTextBox, lineNumbersBox);

            // Считываем первоначальные настройки чекбоксов
            lineNumbersBox.Visibility = chkShowLineNumbers.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            mainTextBox.TextWrapping = chkWordWrap.IsChecked == true ? TextWrapping.Wrap : TextWrapping.NoWrap;

            UpdateLineNumbers(mainTextBox, lineNumbersBox);
            return mainGrid;
        }

        private void UpdateLineNumbers(TextBox input, TextBox output)
        {
            int lineCount = input.LineCount;
            if (lineCount < 1) lineCount = 1;

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= lineCount; i++) sb.AppendLine(i.ToString());
            output.Text = sb.ToString();
        }

        #endregion

        #region Управление Листами и Проводником

        private void CreateNewTab(string headerText, string contentText = "", string path = "")
        {
            Grid editorGrid = CreateNumberedTextBoxContainer(contentText);
            TabItemData tabData = new TabItemData { HeaderText = headerText, FilePath = path, IsDirty = false };

            TabItem newTab = new TabItem { DataContext = tabData, Content = editorGrid };

            tabControlFiles.Items.Add(newTab);
            tabControlFiles.SelectedItem = newTab;

            UpdateSidebar();
        }

        private TextBox GetCurrentTextBox()
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.Content is Grid grid)
                return grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
            return null;
        }

        private TextBox GetCurrentLineNumbersBox()
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.Content is Grid grid)
                return grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);
            return null;
        }

        private void CloseCurrentTab()
        {
            if (tabControlFiles.Items.Count > 1)
            {
                int index = tabControlFiles.SelectedIndex;
                tabControlFiles.Items.Remove(tabControlFiles.SelectedItem);
                tabControlFiles.SelectedIndex = index == 0 ? 0 : index - 1;
            }
            else
            {
                TextBox box = GetCurrentTextBox();
                if (box != null) box.Text = "";
                if (tabControlFiles.SelectedItem is TabItem tab && tab.DataContext is TabItemData data)
                {
                    data.HeaderText = "Новый документ.txt";
                    data.FilePath = "";
                    data.IsDirty = false;
                }
            }
            UpdateSidebar();
        }

        #endregion

        #region Инструменты Кастомизации UI (Правая Супер Панель)

        private void ToggleSettingsSidebar_Click(object sender, RoutedEventArgs e)
        {
            RightSettingsSidebar.Visibility = RightSettingsSidebar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void ComboThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ЗАЩИТА: Если вкладки еще не готовы, не пытаемся красить тему
            if (tabControlFiles == null || comboThemes == null || comboThemes.SelectedItem == null)
                return;

            ApplyThemeEngine();
        }

        private void AccentChange_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string hexColor)
            {
                var color = (Color)ColorConverter.ConvertFromString(hexColor);
                Resources["AccentColor"] = new SolidColorBrush(color);
                textBlockInfo.Text = $"Цветовой акцент изменен на {hexColor}";
            }
        }

        private void ComboFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ЗАЩИТА: Если TabControl или сам комбобокс еще не созданы при инициализации XAML
            if (tabControlFiles == null || comboFonts == null || comboFonts.SelectedItem == null)
                return;

            string fontName = (comboFonts.SelectedItem as ComboBoxItem).Content.ToString().Split(' ')[0];
            _currentFontFamily = new FontFamily(fontName);

            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    foreach (var tb in grid.Children.OfType<TextBox>())
                        tb.FontFamily = _currentFontFamily;
                }
            }
        }

        private void SliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _baseFontSize = sliderFontSize.Value;
            ApplyFontSizeEngine();
        }

        private void UIOptions_Changed(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles == null) return;

            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    TextBox main = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                    TextBox num = grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);

                    if (num != null) num.Visibility = chkShowLineNumbers.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
                    if (main != null) main.TextWrapping = chkWordWrap.IsChecked == true ? TextWrapping.Wrap : TextWrapping.NoWrap;

                    if (grid.Children.Count > 0 && chkHighlightActiveLine.IsChecked == false)
                        grid.Margin = new Thickness(0);
                    else if (chkHighlightActiveLine.IsChecked == true)
                        grid.Margin = new Thickness(1);
                }
            }
        }

        private void ResetSettings_Click(object sender, RoutedEventArgs e)
        {
            chkShowLineNumbers.IsChecked = true;
            chkWordWrap.IsChecked = false;
            chkHighlightActiveLine.IsChecked = true;
            sliderFontSize.Value = 16;
            comboThemes.SelectedIndex = 0;
            comboFonts.SelectedIndex = 0;
            Resources["AccentColor"] = new SolidColorBrush(Color.FromRgb(0, 120, 212));
            _globalZoom = 100;
            ApplyThemeEngine();
            ApplyFontSizeEngine();
            MessageBox.Show("Конфигурация UI сброшена в исходное эталонное состояние.", "Система");
        }

        private void ApplyThemeEngine()
        {
            if (tabControlFiles == null || comboThemes == null) return;

            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    TextBox main = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                    TextBox num = grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);
                    if (main != null && num != null) ApplyCurrentThemeToContainer(main, num);
                }
            }
        }

        private void ApplyCurrentThemeToContainer(TextBox main, TextBox num)
        {
            if (comboThemes == null) return;
            switch (comboThemes.SelectedIndex)
            {
                case 0: // Light
                    main.Background = _lightBg; main.Foreground = _lightFg; num.Background = _lightNumBg; break;
                case 1: // Dark
                    main.Background = _darkBg; main.Foreground = _darkFg; num.Background = _darkNumBg; break;
                case 2: // Sepia
                    main.Background = _sepiaBg; main.Foreground = _sepiaFg; num.Background = _sepiaNumBg; break;
                case 3: // Cyberpunk
                    main.Background = _cyberBg; main.Foreground = _cyberFg; num.Background = _cyberNumBg; break;
            }
        }

        private void ApplyFontSizeEngine()
        {
            // ЗАЩИТА: Если элементы интерфейса еще не проинициализированы, выходим
            if (lblZoom == null || tabControlFiles == null)
                return;

            lblZoom.Text = $"Масштаб: {_globalZoom}%";
            foreach (TabItem item in tabControlFiles.Items)
            {
                if (item.Content is Grid grid)
                {
                    TextBox main = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                    TextBox num = grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);
                    if (main != null && num != null)
                        ApplyFontSizeToContainer(main, num);
                }
            }
        }

        private void ApplyFontSizeToContainer(TextBox main, TextBox num)
        {
            double computedSize = _baseFontSize * (_globalZoom / 100);
            main.FontSize = computedSize;
            num.FontSize = computedSize;
        }

        #endregion

        #region Логика Поиска, Замены и Регулярных Выражений

        private void ExecuteSearch(string searchText, bool searchForward)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox == null || string.IsNullOrEmpty(searchText)) return;

            string sourceText = textBox.Text;
            int currentStart = textBox.SelectionStart;

            if (buttonRegex.IsChecked == true)
            {
                try
                {
                    RegexOptions opts = buttonWithCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase;
                    MatchCollection matches = Regex.Matches(sourceText, searchText, opts);
                    if (matches.Count == 0) { MessageBox.Show("Совпадений Regex не найдено."); return; }

                    Match match = searchForward ?
                        matches.Cast<Match>().FirstOrDefault(m => m.Index > currentStart) ?? matches[0] :
                        matches.Cast<Match>().LastOrDefault(m => m.Index < currentStart) ?? matches[matches.Count - 1];

                    textBox.Focus();
                    textBox.Select(match.Index, match.Length);
                }
                catch (Exception ex) { MessageBox.Show("Ошибка регулярного выражения: " + ex.Message); }
            }
            else
            {
                StringComparison comp = buttonWithCase.IsChecked == true ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                int index = -1;

                if (searchForward)
                {
                    index = sourceText.IndexOf(searchText, currentStart + textBox.SelectionLength, comp);
                    if (index == -1) index = sourceText.IndexOf(searchText, 0, comp);
                }
                else
                {
                    int startIdx = currentStart - 1;
                    if (startIdx >= 0) index = sourceText.LastIndexOf(searchText, startIdx, comp);
                    if (index == -1) index = sourceText.LastIndexOf(searchText, sourceText.Length - 1, comp);
                }

                if (index != -1) { textBox.Focus(); textBox.Select(index, searchText.Length); }
                else MessageBox.Show("Текст не обнаружен.");
            }
        }

        private void buttonFindNext_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxFind.Text, true);
        private void buttonFindBack_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxFind.Text, false);
        private void Button_FindNext_ReplacePanel_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxReplaceFind.Text, true);

        private void Button_Replace_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox != null && textBox.SelectedText.Equals(textBoxReplaceFind.Text, StringComparison.OrdinalIgnoreCase))
                textBox.SelectedText = textBoxReplaceText.Text;
            ExecuteSearch(textBoxReplaceFind.Text, true);
        }

        private void Button_ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox == null) return;

            if (buttonRegex.IsChecked == true)
            {
                RegexOptions opts = buttonWithCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase;
                textBox.Text = Regex.Replace(textBox.Text, textBoxReplaceFind.Text, textBoxReplaceText.Text, opts);
            }
            else
            {
                textBox.Text = textBox.Text.Replace(textBoxReplaceFind.Text, textBoxReplaceText.Text);
            }
            MessageBox.Show("Пакетная замена завершена.");
        }

        #endregion

        #region Системная Печать

        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox == null || string.IsNullOrEmpty(textBox.Text)) return;

            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument { PagePadding = new Thickness(50), Background = Brushes.White };
                string[] lines = textBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string line in lines)
                {
                    doc.Blocks.Add(new Paragraph(new Run(line))
                    {
                        FontFamily = textBox.FontFamily,
                        FontSize = 12,
                        Margin = new Thickness(0)
                    });
                }
                printDialog.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Печать документов Studio");
            }
        }

        #endregion

        #region Файловый Ввод/Вывод (IO)

        private void Button_Click_New_File(object sender, RoutedEventArgs e) => CreateNewTab("Новый документ.txt");

        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*", Multiselect = true };
            if (ofd.ShowDialog() == true)
            {
                foreach (string path in ofd.FileNames)
                    CreateNewTab(Path.GetFileName(path), File.ReadAllText(path, _currentEncoding), path);
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
                    SaveFileDialog sfd = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" };
                    if (sfd.ShowDialog() == true)
                    {
                        tabData.FilePath = sfd.FileName;
                        tabData.HeaderText = Path.GetFileName(sfd.FileName);
                    }
                    else return;
                }

                File.WriteAllText(tabData.FilePath, textBox.Text, _currentEncoding);
                tabData.IsDirty = false;
                textBlockInfo.Text = $"Файл сохранен: {tabData.HeaderText}";
                UpdateSidebar();
            }
        }

        private void Button_Click_Save_File_As(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                TextBox textBox = GetCurrentTextBox();
                if (textBox == null) return;

                SaveFileDialog sfd = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" };
                if (sfd.ShowDialog() == true)
                {
                    tabData.FilePath = sfd.FileName;
                    tabData.HeaderText = Path.GetFileName(sfd.FileName);
                    File.WriteAllText(tabData.FilePath, textBox.Text, _currentEncoding);
                    tabData.IsDirty = false;
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
                        tabData.IsDirty = false;
                    }
                }
            }
            textBlockInfo.Text = "Сессия успешно сохранена.";
        }

        private void Button_Click_Update_File(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                if (!string.IsNullOrEmpty(tabData.FilePath) && File.Exists(tabData.FilePath))
                {
                    TextBox box = GetCurrentTextBox();
                    if (box != null) { box.Text = File.ReadAllText(tabData.FilePath, _currentEncoding); tabData.IsDirty = false; }
                }
            }
        }

        #endregion

        #region Правка Текста

        private void MenuItem_SortLines_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            var lines = box.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).OrderBy(x => x);
            box.Text = string.Join(Environment.NewLine, lines);
        }

        private void MenuItem_RemoveEmptyLines_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            var lines = box.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x));
            box.Text = string.Join(Environment.NewLine, lines);
        }

        private void MenuItem_InsertDateTime_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;
            box.SelectedText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        #endregion

        #region Статус-Бар,Синхронизация Команд и Заглушки

        private void UpdateStatusBarStats()
        {
            TextBox box = GetCurrentTextBox();
            if (box != null && lblLinesCount != null)
            {
                lblLinesCount.Text = $"Строк: {box.LineCount}";
                lblCharCount.Text = $"Символов: {box.Text.Length}";
            }
        }

        private void UpdateSidebar()
        {
            if (lstSidebarFiles == null) return;
            List<TabItemData> list = new List<TabItemData>();
            foreach (TabItem item in tabControlFiles.Items)
                if (item.DataContext is TabItemData d) list.Add(d);
            lstSidebarFiles.ItemsSource = list;
        }

        private void TabControlFiles_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateStatusBarStats();

        private void LstSidebarFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSidebarFiles.SelectedItem is TabItemData data)
            {
                TabItem target = tabControlFiles.Items.Cast<TabItem>().FirstOrDefault(x => x.DataContext == data);
                if (target != null) tabControlFiles.SelectedItem = target;
            }
        }

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

        private void Click_Left_Panel_Items(object sender, RoutedEventArgs e) => LeftSidebar.Visibility = LeftSidebar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        private void CloseSearchPanel_Click(object sender, RoutedEventArgs e) => SearchPanel.Visibility = Visibility.Collapsed;
        private void CloseReplacePanel_Click(object sender, RoutedEventArgs e) => ReplacePanel.Visibility = Visibility.Collapsed;
        private void Button_Click_Close(object sender, RoutedEventArgs e) => CloseCurrentTab();
        private void CloseTab_Click(object sender, RoutedEventArgs e) => CloseCurrentTab();
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => this.Close();
        private void Window_Closing(object sender, CancelEventArgs e) { var res = MessageBox.Show("Закрыть редактор Студии?", "Выход", MessageBoxButton.YesNo); if (res == MessageBoxResult.No) e.Cancel = true; }

        #endregion
    }
}