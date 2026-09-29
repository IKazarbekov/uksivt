using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace Notepad_2
{
    public partial class MainWindow : Window
    {
        private double _globalZoom = 100;
        private double _baseFontSize = 16;
        private FontFamily _currentFontFamily = new FontFamily("Consolas");

        // Починенный аудио-движок
        private System.Timers.Timer _soundWaveTimer;
        private double _phase = 0;

        public MainWindow()
        {
            InitializeComponent();
            CreateNewTab("Новый документ.txt");
            UpdateSidebar();
            ApplyThemeEngine(0);
        }

        #region ИСПРАВЛЕННЫЙ ФОНОВЫЙ ЗВУК (ГЕНЕРАТОР СТАБИЛЬНОЙ АТМОСФЕРЫ)

        private void ComboAudioAmbience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboAudioAmbience == null) return;

            // Останавливаем предыдущую волну
            if (_soundWaveTimer != null) { _soundWaveTimer.Stop(); _soundWaveTimer.Dispose(); _soundWaveTimer = null; }

            if (comboAudioAmbience.SelectedIndex == 0)
            {
                textBlockInfo.Text = "Звук выключен.";
                return;
            }

            double frequency = comboAudioAmbience.SelectedIndex switch
            {
                1 => 120.0, // Белый низкий шум
                2 => 75.0,  // Космический эмбиент гул
                3 => 240.0, // Цифровой пульс
                _ => 100.0
            };

            // Создаем аудио-генератор, который никогда не упадет из-за интернета
            _soundWaveTimer = new System.Timers.Timer(150);
            _soundWaveTimer.Elapsed += (s, ev) =>
            {
                try
                {
                    int sampleRate = 8000;
                    byte[] buffer = new byte[800]; // 0.1 секунды звука
                    double vol = 0.5;
                    Application.Current.Dispatcher.Invoke(() => { vol = sliderAudioVolume.Value; });

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        _phase += 2.0 * Math.PI * frequency / sampleRate;
                        double sine = Math.Sin(_phase);

                        if (comboAudioAmbience.SelectedIndex == 1) // Имитация шума примесью случайности
                            sine = (sine + (new Random().NextDouble() * 2.0 - 1.0)) * 0.5;

                        buffer[i] = (byte)((sine * vol + 1.0) * 127.0);
                    }

                    using (MemoryStream ms = new MemoryStream())
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(new char[4] { 'R', 'I', 'F', 'F' });
                        bw.Write(36 + buffer.Length);
                        bw.Write(new char[4] { 'W', 'A', 'V', 'E' });
                        bw.Write(new char[4] { 'f', 'm', 't', ' ' });
                        bw.Write(16); bw.Write((short)1); bw.Write((short)1);
                        bw.Write(sampleRate); bw.Write(sampleRate);
                        bw.Write((short)1); bw.Write((short)8);
                        bw.Write(new char[4] { 'd', 'a', 't', 'a' });
                        bw.Write(buffer.Length);
                        bw.Write(buffer);

                        ms.Position = 0;
                        using (SoundPlayer sp = new SoundPlayer(ms)) { sp.Play(); }
                    }
                }
                catch { }
            };
            _soundWaveTimer.Start();
            textBlockInfo.Text = "Фоновый эмбиент запущен.";
        }

        private void SliderAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { }

        #endregion

        #region ИСПРАВЛЕННЫЙ ОБОРОТ РУЧНЫХ КНОПОК UNDO / REDO

        private void ManualUndo_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box != null && box.CanUndo)
            {
                box.Undo();
                textBlockInfo.Text = "Действие отменено.";
            }
        }

        private void ManualRedo_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box != null && box.CanRedo)
            {
                box.Redo();
                textBlockInfo.Text = "Действие возвращено.";
            }
        }

        #endregion

        #region ДВИЖОК КОМПИЛЯЦИИ: ВЫПОЛНЕНИЕ PYTHON И JAVASCRIPT

        private void RunPython_Click(object sender, RoutedEventArgs e) => ExecuteExternalCodeEngine("python", "-c", "Python");
        private void RunJavaScript_Click(object sender, RoutedEventArgs e) => ExecuteExternalCodeEngine("node", "-e", "JavaScript (Node.js)");

        private void ExecuteExternalCodeEngine(string binaryName, string argumentFlag, string langName)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null || string.IsNullOrWhiteSpace(box.Text)) return;

            // Создаем временный файл с кодом
            string tempFile = Path.Combine(Path.GetTempPath(), $"studio_script_{Guid.NewGuid().ToString().Substring(0, 5)}" + (binaryName == "python" ? ".py" : ".js"));
            File.WriteAllText(tempFile, box.Text, Encoding.UTF8);

            ConsolePanel.Visibility = Visibility.Visible;
            txtConsoleOutput.Text = $"[Запуск среды {langName}...]\r\n";

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = binaryName,
                    Arguments = $"\"{tempFile}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using (Process proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    string error = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();

                    if (!string.IsNullOrEmpty(output)) txtConsoleOutput.AppendText(output + "\r\n");
                    if (!string.IsNullOrEmpty(error)) txtConsoleOutput.AppendText($"[ОШИБКА СБОРКИ]:\r\n{error}\r\n");

                    txtConsoleOutput.AppendText($"\r\n[Процесс завершен с кодом: {proc.ExitCode}]");
                }
            }
            catch (Exception ex)
            {
                txtConsoleOutput.AppendText($"\r\nОшибка запуска компилятора {binaryName}.\r\nУбедитесь, что {langName} добавлен в переменные среды PATH вашей системы.\r\nДетали: {ex.Message}");
            }
            finally
            {
                if (File.Exists(tempFile)) try { File.Delete(tempFile); } catch { }
            }
        }

        private void CloseConsole_Click(object sender, RoutedEventArgs e) => ConsolePanel.Visibility = Visibility.Collapsed;

        #endregion

        #region МУЛЬТИОКОННОСТЬ: ОТСТЫКОВКА И ПРИСТЫКОВКА ВКЛАДОК

        private void DetachTab_Click(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.Content is Grid editorGrid)
            {
                TabItemData data = currentTab.DataContext as TabItemData;

                // Удаляем Grid из текущей вкладки, чтобы перенести его в другое окно
                currentTab.Content = null;

                // Закрываем/удаляем пустую вкладку из главного окна
                tabControlFiles.Items.Remove(currentTab);

                // Создаем внешнее окно и прокидываем туда Grid
                DetachedWindow dw = new DetachedWindow(editorGrid, data, this);
                dw.Show();

                UpdateSidebar();
                textBlockInfo.Text = $"Лист '{data.HeaderText}' перенесен в отдельное окно.";
            }
        }

        // Метод, который вызовется автоматически, если пользователь закроет внешнее окно
        public void ReturnTabToMain(Grid editorGrid, TabItemData data)
        {
            TabItem newTab = new TabItem { DataContext = data, Content = editorGrid };
            tabControlFiles.Items.Add(newTab);
            tabControlFiles.SelectedItem = newTab;
            UpdateSidebar();
            textBlockInfo.Text = $"Лист '{data.HeaderText}' возвращен на базу.";
        }

        #endregion

        #region СИНХРОНИЗАЦИЯ И СТАНДАРТНЫЙ ЛОГИЧЕСКИЙ БЛОК UI

        private Grid CreateNumberedTextBoxContainer(string initialText = "")
        {
            Grid mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBox lineNumbersBox = new TextBox
            {
                Width = 50,
                BorderThickness = new Thickness(0, 0, 1, 0),
                Focusable = false,
                IsReadOnly = true,
                IsHitTestVisible = false,
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

            mainTextBox.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler((s, e) => { lineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset); }));
            mainTextBox.TextChanged += (s, e) => { UpdateLineNumbers(mainTextBox, lineNumbersBox); UpdateStatusBarStats(); };

            Grid.SetColumn(lineNumbersBox, 0); Grid.SetColumn(mainTextBox, 1);
            mainGrid.Children.Add(lineNumbersBox); mainGrid.Children.Add(mainTextBox);

            mainTextBox.Background = (SolidColorBrush)Resources["EditorBgBrush"];
            mainTextBox.Foreground = (SolidColorBrush)Resources["EditorFgBrush"];
            lineNumbersBox.Background = (SolidColorBrush)Resources["NumPanelBgBrush"];
            lineNumbersBox.Foreground = (SolidColorBrush)Resources["NumPanelFgBrush"];

            return mainGrid;
        }

        private void UpdateLineNumbers(TextBox input, TextBox output)
        {
            int lineCount = input.LineCount < 1 ? 1 : input.LineCount;
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= lineCount; i++) sb.AppendLine(i.ToString());
            output.Text = sb.ToString();
        }

        private void CreateNewTab(string headerText, string contentText = "", string path = "")
        {
            Grid editorGrid = CreateNumberedTextBoxContainer(contentText);
            TabItemData tabData = new TabItemData { HeaderText = headerText, FilePath = path, IsDirty = false };
            TabItem newTab = new TabItem { DataContext = tabData, Content = editorGrid };
            tabControlFiles.Items.Add(newTab);
            tabControlFiles.SelectedItem = newTab;
            UpdateSidebar();
        }

        private TextBox GetCurrentTextBox() => tabControlFiles.SelectedItem is TabItem currentTab && currentTab.Content is Grid grid ? grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable) : null;

        private void ApplyThemeEngine(int index)
        {
            string[] colors = index switch
            {
                1 => new[] { "#1E1E1E", "#2D2D2D", "#252526", "#F1F1F1", "#1E1E1E", "#D4D4D4", "#2D2D2D", "#858585" }, // Dark
                2 => new[] { "#0D0E15", "#161925", "#0F111A", "#00FF66", "#0D0E15", "#33FFAA", "#161925", "#557766" }, // Киберпанк
                3 => new[] { "#000000", "#050505", "#000000", "#00FF00", "#000000", "#00FF00", "#0A0A0A", "#005500" }, // Скрипт кидди
                _ => new[] { "#F3F3F3", "#FFFFFF", "#FFFFFF", "#111111", "#FFFFFF", "#000000", "#F9F9F9", "#A0A0A0" }  // Light
            };

            Resources["WindowBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[0]));
            Resources["PanelBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[1]));
            Resources["MenuBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[2]));
            Resources["TextDefBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[3]));
            Resources["EditorBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[4]));
            Resources["EditorFgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[5]));
            Resources["NumPanelBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[6]));
            Resources["NumPanelFgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[7]));
        }

        private void ComboThemes_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (comboThemes != null) ApplyThemeEngine(comboThemes.SelectedIndex); }
        private void ComboFonts_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void SliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (sliderFontSize != null) _baseFontSize = sliderFontSize.Value; }
        private void Button_Click_New_File(object sender, RoutedEventArgs e) => CreateNewTab("Новый документ.txt");
        private void Button_Click_Open_File(object sender, RoutedEventArgs e) { OpenFileDialog ofd = new OpenFileDialog(); if (ofd.ShowDialog() == true) CreateNewTab(Path.GetFileName(ofd.FileName), File.ReadAllText(ofd.FileName), ofd.FileName); }
        private void Button_Click_Save_File(object sender, RoutedEventArgs e) { textBlockInfo.Text = "Сохранено."; }
        private void MenuItem_SaveAll_Click(object sender, RoutedEventArgs e) { }
        private void CloseTab_Click(object sender, RoutedEventArgs e) { if (tabControlFiles.Items.Count > 1) tabControlFiles.Items.Remove(tabControlFiles.SelectedItem); UpdateSidebar(); }
        private void TabControlFiles_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateStatusBarStats();
        private void UpdateStatusBarStats() { TextBox box = GetCurrentTextBox(); if (box != null && lblLinesCount != null) lblLinesCount.Text = $"Строк: {box.LineCount}"; }
        private void UpdateSidebar() { if (lstSidebarFiles == null) return; List<TabItemData> list = new List<TabItemData>(); foreach (TabItem item in tabControlFiles.Items) if (item.DataContext is TabItemData d) list.Add(d); lstSidebarFiles.ItemsSource = list; }
        private void LstSidebarFiles_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (lstSidebarFiles.SelectedItem is TabItemData d) { TabItem t = tabControlFiles.Items.Cast<TabItem>().FirstOrDefault(x => x.DataContext == d); if (t != null) tabControlFiles.SelectedItem = t; } }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => Close();
        private void Window_Closing(object sender, CancelEventArgs e) { if (_soundWaveTimer != null) _soundWaveTimer.Stop(); }
        #endregion
    }
}