using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Speech.Synthesis; // Добавьте ссылку на System.Speech в менеджере ссылок, если проект .NET Framework
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Notepad_2
{
    public partial class MainWindow : Window
    {
        private Encoding _currentEncoding = Encoding.UTF8;
        private double _globalZoom = 100;
        private double _baseFontSize = 16;
        private FontFamily _currentFontFamily = new FontFamily("Consolas");

        // Голосовой движок и Плеер фоновых звуков
        private SpeechSynthesizer _speechEngine;
        private MediaPlayer _mediaPlayer;
        private DispatcherTimer _backupTimer;
        private string _selectedSteganoImagePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            SetupBuiltInCommands();
            InitAdvancedEngines();

            // Базовый запуск среды
            CreateNewTab("Новый документ.txt");
            UpdateSidebar();

            // Принудительно выставляем начальную тему, чтобы раскрасить ВСЁ окно
            ApplyThemeEngine(0);
        }

        private void InitAdvancedEngines()
        {
            try { _speechEngine = new SpeechSynthesizer(); } catch { }
            _mediaPlayer = new MediaPlayer();

            // Таймер автосохранения (Раз в 60 секунд бэкапит измененные файлы в AppData)
            _backupTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(60) };
            _backupTimer.Tick += BackupTimer_Tick;
            _backupTimer.Start();
        }

        #region ДЕСЯТЬ ДИЗАЙНЕРСКИХ ТЕМ ДЛЯ ВСЕГО ОКНА (ГЛОБАЛЬНОЕ ПЕРЕКРАШИВАНИЕ)

        private void ComboThemes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlFiles == null || comboThemes == null || comboThemes.SelectedItem == null) return;
            ApplyThemeEngine(comboThemes.SelectedIndex);
        }

        private void ApplyThemeEngine(int themeIndex)
        {
            // Карта цветов: [WindowBg, PanelBg, MenuBg, TextDef, EditorBg, EditorFg, NumBg, NumFg]
            string[] colors = themeIndex switch
            {
                1 => new[] { "#1E1E1E", "#2D2D2D", "#252526", "#F1F1F1", "#1E1E1E", "#D4D4D4", "#2D2D2D", "#858585" }, // Глубокая Ночь
                2 => new[] { "#F4ECD8", "#FDFAF0", "#EFE6D0", "#5B4636", "#FDFAF0", "#433422", "#F4ECD8", "#8A7968" }, // Винтажная Сепия
                3 => new[] { "#0D0E15", "#161925", "#0F111A", "#00FF66", "#0D0E15", "#33FFAA", "#161925", "#557766" }, // Киберпанк
                4 => new[] { "#FFF0F5", "#FFFFFF", "#FFE4E1", "#D02090", "#FFF5EE", "#8B008B", "#FFE4E1", "#CD6889" }, // Сакура
                5 => new[] { "#282A36", "#44475A", "#21222C", "#F8F8F2", "#282A36", "#50FA7B", "#191A21", "#6272A4" }, // Дракула
                6 => new[] { "#0B1D28", "#122B3D", "#0A1721", "#E6F1F7", "#0B1D28", "#4FC3F7", "#122B3D", "#37474F" }, // Океан
                7 => new[] { "#1C2321", "#2A3432", "#171E1D", "#A9C5A0", "#1C2321", "#D8E2DC", "#2A3432", "#4F5D56" }, // Лесной Мох
                8 => new[] { "#F5F5F5", "#FFFFFF", "#EEEEEE", "#000000", "#FFFFFF", "#000000", "#F9F9F9", "#777777" }, // Монохром
                9 => new[] { "#000000", "#050505", "#000000", "#00FF00", "#000000", "#00FF00", "#0A0A0A", "#005500" }, // Скрипт-Кидди
                _ => new[] { "#F3F3F3", "#FFFFFF", "#FFFFFF", "#111111", "#FFFFFF", "#000000", "#F9F9F9", "#A0A0A0" }  // Светлый Оригинал
            };

            // Обновляем глобальные кисти приложения, на которые завязан XAML
            Resources["WindowBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[0]));
            Resources["PanelBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[1]));
            Resources["MenuBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[2]));
            Resources["TextDefBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[3]));
            Resources["EditorBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[4]));
            Resources["EditorFgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[5]));
            Resources["NumPanelBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[6]));
            Resources["NumPanelFgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[7]));

            // Красим уже открытые текстовые поля документов
            if (tabControlFiles != null)
            {
                foreach (TabItem item in tabControlFiles.Items)
                {
                    if (item.Content is Grid grid)
                    {
                        var main = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                        var num = grid.Children.OfType<TextBox>().FirstOrDefault(x => !x.Focusable);
                        if (main != null && num != null)
                        {
                            main.Background = (SolidColorBrush)Resources["EditorBgBrush"];
                            main.Foreground = (SolidColorBrush)Resources["EditorFgBrush"];
                            num.Background = (SolidColorBrush)Resources["NumPanelBgBrush"];
                            num.Foreground = (SolidColorBrush)Resources["NumPanelFgBrush"];
                        }
                    }
                }
            }
        }

        private void AccentChange_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string hex)
            {
                Resources["AccentColor"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
            }
        }

        #endregion

        #region ФОНОВЫЙ ЗВУКОВОЙ ПЛЕЕР (LO-FI АТМОСФЕРА)

        private void ComboAudioAmbience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_mediaPlayer == null || comboAudioAmbience == null) return;

            _mediaPlayer.Stop();
            string url = comboAudioAmbience.SelectedIndex switch
            {
                1 => "https://www.soundjay.com/nature/sounds/rain-07.mp3", // Дождь
                2 => "https://www.soundjay.com/nature/sounds/fire-1.mp3",    // Костер
                3 => "https://www.soundjay.com/misc/sounds/train-passing-1.mp3", // Поезд
                _ => null
            };

            if (url != null)
            {
                _mediaPlayer.Open(new Uri(url));
                _mediaPlayer.MediaEnded += (s, ev) => { _mediaPlayer.Position = TimeSpan.Zero; _mediaPlayer.Play(); };
                _mediaPlayer.Volume = sliderAudioVolume.Value;
                _mediaPlayer.Play();
                textBlockInfo.Text = "Аудио-атмосфера запущена.";
            }
        }

        private void SliderAudioVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_mediaPlayer != null) _mediaPlayer.Volume = sliderAudioVolume.Value;
        }

        #endregion

        #region РОБОТ-ТЕКСТ И ОЗВУЧКА ГОЛОСОМ (TEXT-TO-SPEECH)

        private void Voice_Speak_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null || _speechEngine == null) return;

            string txt = string.IsNullOrEmpty(box.SelectedText) ? box.Text : box.SelectedText;
            if (!string.IsNullOrWhiteSpace(txt))
            {
                _speechEngine.SpeakAsyncCancelAll();
                _speechEngine.SpeakAsync(txt);
                textBlockInfo.Text = "Голосовой движок читает текст...";
            }
        }

        private void Voice_Stop_Click(object sender, RoutedEventArgs e)
        {
            _speechEngine?.SpeakAsyncCancelAll();
            textBlockInfo.Text = "Чтение остановлено.";
        }

        #endregion

        #region НЕОЖИДАННАЯ УТИЛИТА: МАТЕМАТИЧЕСКИЙ ПАРСЕР ТЕКСТА

        private void Util_Calculate_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null) return;

            string expr = string.IsNullOrEmpty(box.SelectedText) ? box.Text : box.SelectedText;
            if (string.IsNullOrWhiteSpace(expr)) return;

            try
            {
                // Заменяем базовые операции для безопасности
                var clean = Regex.Replace(expr, @"[^0-9\+\-\*\/\(\)\.]", "");
                System.Data.DataTable dt = new System.Data.DataTable();
                var result = dt.Compute(clean, "");

                if (string.IsNullOrEmpty(box.SelectedText))
                    box.AppendText($"{Environment.NewLine}[Результат вычислений: {result}]");
                else
                    box.SelectedText = $"{expr} = {result}";

                textBlockInfo.Text = "Математическое выражение успешно посчитано.";
            }
            catch { MessageBox.Show("Не удалось распарсить математическое выражение. Убедитесь, что там только числа и знаки +, -, *, /.", "Парсер"); }
        }

        private void Util_Lorem_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box != null)
                box.SelectedText = "Кирилл ******* не знаю что добавить дор";
        }

        private void Util_MD5_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null || string.IsNullOrEmpty(box.Text)) return;
            using MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(box.Text));
            MessageBox.Show(BitConverter.ToString(hash).Replace("-", "").ToLower(), "MD5 Хеш всего текста");
        }

        private void Util_SHA256_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null || string.IsNullOrEmpty(box.Text)) return;
            using SHA256 sha = SHA256.Create();
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(box.Text));
            MessageBox.Show(BitConverter.ToString(hash).Replace("-", "").ToLower(), "SHA-256 Хеш всего текста");
        }

        private void Util_Upper_Click(object sender, RoutedEventArgs e) { var b = GetCurrentTextBox(); if (b != null) b.SelectedText = b.SelectedText.ToUpper(); }
        private void Util_Lower_Click(object sender, RoutedEventArgs e) { var b = GetCurrentTextBox(); if (b != null) b.SelectedText = b.SelectedText.ToLower(); }

        #endregion

        #region СТЕГАНОГРАФИЯ (ШИФРОВАНИЕ ДАННЫХ В ПИКСЕЛИ ФОТОГРАФИИ - LSB МЕТОД)

        private void Stegano_SelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Изображения (*.png)|*.png" };
            if (ofd.ShowDialog() == true)
            {
                _selectedSteganoImagePath = ofd.FileName;
                lblSteganoImgPath.Text = Path.GetFileName(ofd.FileName);
            }
        }

        private void Stegano_Encode_Click(object sender, RoutedEventArgs e)
        {
            TextBox box = GetCurrentTextBox();
            if (box == null || string.IsNullOrEmpty(box.Text) || string.IsNullOrEmpty(_selectedSteganoImagePath))
            {
                MessageBox.Show("Введите текст в редактор и выберите исходную картинку PNG!", "Стеганография");
                return;
            }

            try
            {
                BitmapFrame frame = BitmapFrame.Create(new Uri(_selectedSteganoImagePath), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                WriteableBitmap wbm = new WriteableBitmap(frame);

                byte[] textBytes = Encoding.UTF8.GetBytes(box.Text);
                int textLength = textBytes.Length;
                byte[] lengthBytes = BitConverter.GetBytes(textLength);

                byte[] dataToHide = lengthBytes.Concat(textBytes).ToArray();
                if (dataToHide.Length * 8 > wbm.PixelWidth * wbm.PixelHeight * 3)
                {
                    MessageBox.Show("Текст слишком велик для этого изображения!", "Ошибка");
                    return;
                }

                int byteIdx = 0, bitIdx = 0;
                wbm.Lock();
                unsafe
                {
                    byte* ptr = (byte*)wbm.BackBuffer;
                    int stride = wbm.BackBufferStride;

                    for (int y = 0; y < wbm.PixelHeight && byteIdx < dataToHide.Length; y++)
                    {
                        for (int x = 0; x < wbm.PixelWidth && byteIdx < dataToHide.Length; x++)
                        {
                            byte* pixel = ptr + y * stride + x * 4; // BGRA формат

                            for (int c = 0; c < 3 && byteIdx < dataToHide.Length; c++) // Кодируем в Каналы B, G, R
                            {
                                int bit = (dataToHide[byteIdx] >> bitIdx) & 1;
                                pixel[c] = (byte)((pixel[c] & 0xFE) | bit);

                                bitIdx++;
                                if (bitIdx == 8) { bitIdx = 0; byteIdx++; }
                            }
                        }
                    }
                }
                wbm.AddDirtyRect(new Int32Rect(0, 0, wbm.PixelWidth, wbm.PixelHeight));
                wbm.Unlock();

                SaveFileDialog sfd = new SaveFileDialog { Filter = "PNG Изображение (*.png)|*.png", FileName = "Secret_Image.png" };
                if (sfd.ShowDialog() == true)
                {
                    using FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(wbm));
                    encoder.Save(fs);
                    MessageBox.Show("Ваш секретный текст успешно имплантирован в пиксели изображения!", "Успех");
                }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка стеганографии: " + ex.Message); }
        }

        private void Stegano_Decode_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "PNG Изображения (*.png)|*.png" };
            if (ofd.ShowDialog() != true) return;

            try
            {
                BitmapFrame frame = BitmapFrame.Create(new Uri(ofd.FileName), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                WriteableBitmap wbm = new WriteableBitmap(frame);

                byte[] lengthBytes = new byte[4];
                int byteIdx = 0, bitIdx = 0;

                wbm.Lock();
                unsafe
                {
                    byte* ptr = (byte*)wbm.BackBuffer;
                    int stride = wbm.BackBufferStride;

                    // Читаем первые 4 байта длины
                    for (int y = 0; y < wbm.PixelHeight && byteIdx < 4; y++)
                    {
                        for (int x = 0; x < wbm.PixelWidth && byteIdx < 4; x++)
                        {
                            byte* pixel = ptr + y * stride + x * 4;
                            for (int c = 0; c < 3 && byteIdx < 4; c++)
                            {
                                int bit = pixel[c] & 1;
                                lengthBytes[byteIdx] |= (byte)(bit << bitIdx);
                                bitIdx++;
                                if (bitIdx == 8) { bitIdx = 0; byteIdx++; }
                            }
                        }
                    }

                    int textLength = BitConverter.ToInt32(lengthBytes, 0);
                    if (textLength <= 0 || textLength > 10 * 1024 * 1024)
                    {
                        MessageBox.Show("Скрытый текст в этой картинке не обнаружен или поврежден.", "Инфо");
                        wbm.Unlock();
                        return;
                    }

                    byte[] textBytes = new byte[textLength];
                    byteIdx = 0; bitIdx = 0;
                    int skipBytes = 0;

                    // Извлекаем тело сообщения
                    for (int y = 0; y < wbm.PixelHeight && byteIdx < textLength; y++)
                    {
                        for (int x = 0; x < wbm.PixelWidth && byteIdx < textLength; x++)
                        {
                            byte* pixel = ptr + y * stride + x * 4;
                            for (int c = 0; c < 3 && byteIdx < textLength; c++)
                            {
                                if (skipBytes < 4)
                                { // Пропускаем заголовок длины
                                    bitIdx++;
                                    if (bitIdx == 8) { bitIdx = 0; skipBytes++; }
                                    continue;
                                }

                                int bit = pixel[c] & 1;
                                textBytes[byteIdx] |= (byte)(bit << bitIdx);
                                bitIdx++;
                                if (bitIdx == 8) { bitIdx = 0; byteIdx++; }
                            }
                        }
                    }

                    wbm.Unlock();
                    string decryptedText = Encoding.UTF8.GetString(textBytes);
                    CreateNewTab("Извлеченный Секрет.txt", decryptedText);
                    MessageBox.Show("Скрытый текст успешно считан из картинки и выведен на экран!", "Дешифратор");
                }
            }
            catch { MessageBox.Show("Не удалось декодировать картинку."); }
        }

        #endregion

        #region ТАЙМ-МАШИНА АВТОБЭКАПОВ СЕССИИ

        private void BackupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                string backupDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudioNotepadBackups");
                if (!Directory.Exists(backupDir)) Directory.CreateDirectory(backupDir);

                foreach (TabItem item in tabControlFiles.Items)
                {
                    if (item.DataContext is TabItemData tabData && item.Content is Grid grid)
                    {
                        TextBox box = grid.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable);
                        if (box != null && tabData.IsDirty)
                        {
                            string bkpPath = Path.Combine(backupDir, $"{tabData.GuidId}.bak");
                            File.WriteAllText(bkpPath, box.Text, Encoding.UTF8);
                        }
                    }
                }
                lblBackupStatus.Text = $"Бэкап: {DateTime.Now:HH:mm:ss}";
            }
            catch { }
        }

        #endregion

        #region СТАНДАРТНАЯ СИНХРОНИЗАЦИЯ СРЕДЫ, АНИМАЦИИ И ДВИЖОК НУМЕРАЦИИ

        private Grid CreateNumberedTextBoxContainer(string initialText = "")
        {
            Grid mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            TextBox lineNumbersBox = new TextBox
            {
                Width = 50,
                BorderThickness = new Thickness(0, 0, 1, 0),
                BorderBrush = (SolidColorBrush)Resources["BorderBrushDef"],
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

            mainTextBox.AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler((s, e) => { lineNumbersBox.ScrollToVerticalOffset(e.VerticalOffset); }));
            mainTextBox.TextChanged += (s, e) =>
            {
                UpdateLineNumbers(mainTextBox, lineNumbersBox);
                UpdateStatusBarStats();
                if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData data)
                    if (!data.IsDirty && mainTextBox.Text.Length > 0) data.IsDirty = true;
            };

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

            // ИСПРАВЛЕНО: Теперь имена переменных совпадают с их объявлением выше
            mainTextBox.Background = (SolidColorBrush)Resources["EditorBgBrush"];
            mainTextBox.Foreground = (SolidColorBrush)Resources["EditorFgBrush"];
            lineNumbersBox.Background = (SolidColorBrush)Resources["NumPanelBgBrush"];
            lineNumbersBox.Foreground = (SolidColorBrush)Resources["NumPanelFgBrush"];

            double computedSize = _baseFontSize * (_globalZoom / 100);
            mainTextBox.FontSize = computedSize;
            lineNumbersBox.FontSize = computedSize;

            if (chkShowLineNumbers != null) lineNumbersBox.Visibility = chkShowLineNumbers.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            if (chkWordWrap != null) mainTextBox.TextWrapping = chkWordWrap.IsChecked == true ? TextWrapping.Wrap : TextWrapping.NoWrap;

            UpdateLineNumbers(mainTextBox, lineNumbersBox);
            return mainGrid;
        }
        private void UpdateLineNumbers(TextBox input, TextBox output)
        {
            int lineCount = input.LineCount < 1 ? 1 : input.LineCount;
            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= lineCount; i++) sb.AppendLine(i.ToString());
            output.Text = sb.ToString();
        }

        private void Button_Click_New_File(object sender, RoutedEventArgs e)
        {
            // Создаем вкладку с дефолтным именем "Новый документ.txt"
            CreateNewTab("Новый документ.txt");
            textBlockInfo.Text = "Создан новый лист.";
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
        private void CloseCurrentTab()
        {
            if (tabControlFiles.Items.Count > 1) { int index = tabControlFiles.SelectedIndex; tabControlFiles.Items.Remove(tabControlFiles.SelectedItem); tabControlFiles.SelectedIndex = index == 0 ? 0 : index - 1; }
            else { TextBox box = GetCurrentTextBox(); if (box != null) box.Text = ""; if (tabControlFiles.SelectedItem is TabItem tab && tab.DataContext is TabItemData data) { data.HeaderText = "Новый документ.txt"; data.FilePath = ""; data.IsDirty = false; } }
            UpdateSidebar();
        }

        private void SliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) { if (sliderFontSize == null) return; _baseFontSize = sliderFontSize.Value; ApplyFontSizeEngine(); }
        private void ComboFonts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlFiles == null || comboFonts == null || comboFonts.SelectedItem == null) return;
            _currentFontFamily = new FontFamily((comboFonts.SelectedItem as ComboBoxItem).Content.ToString());
            foreach (TabItem item in tabControlFiles.Items) if (item.Content is Grid grid) foreach (var tb in grid.Children.OfType<TextBox>()) tb.FontFamily = _currentFontFamily;
        }

        private void ApplyFontSizeEngine()
        {
            if (lblZoom == null || tabControlFiles == null) return;
            lblZoom.Text = $"Масштаб: {_globalZoom}%";
            double sz = _baseFontSize * (_globalZoom / 100);
            foreach (TabItem item in tabControlFiles.Items) if (item.Content is Grid grid) foreach (var tb in grid.Children.OfType<TextBox>()) tb.FontSize = sz;
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
                    grid.Margin = chkHighlightActiveLine.IsChecked == true ? new Thickness(1) : new Thickness(0);
                }
            }
        }

        private void ResetSettings_Click(object sender, RoutedEventArgs e)
        {
            chkShowLineNumbers.IsChecked = true; chkWordWrap.IsChecked = false; chkHighlightActiveLine.IsChecked = true;
            sliderFontSize.Value = 16; comboThemes.SelectedIndex = 0; comboFonts.SelectedIndex = 0; _globalZoom = 100;
            ApplyThemeEngine(0); ApplyFontSizeEngine();
        }

        private void ExecuteSearch(string searchText, bool searchForward)
        {
            TextBox textBox = GetCurrentTextBox();
            if (textBox == null || string.IsNullOrEmpty(searchText)) return;
            string src = textBox.Text; int start = textBox.SelectionStart;
            if (buttonRegex.IsChecked == true)
            {
                try
                {
                    var matches = Regex.Matches(src, searchText, buttonWithCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase);
                    if (matches.Count == 0) return;
                    Match m = searchForward ? matches.Cast<Match>().FirstOrDefault(x => x.Index > start) ?? matches[0] : matches.Cast<Match>().LastOrDefault(x => x.Index < start) ?? matches[matches.Count - 1];
                    textBox.Focus(); textBox.Select(m.Index, m.Length);
                }
                catch { }
            }
            else
            {
                var comp = buttonWithCase.IsChecked == true ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                int idx = searchForward ? src.IndexOf(searchText, start + textBox.SelectionLength, comp) : (start - 1 >= 0 ? src.LastIndexOf(searchText, start - 1, comp) : -1);
                if (idx != -1) { textBox.Focus(); textBox.Select(idx, searchText.Length); }
            }
        }

        private void buttonFindNext_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxFind.Text, true);
        private void buttonFindBack_Click(object sender, RoutedEventArgs e) => ExecuteSearch(textBoxFind.Text, false);
        private void Button_Click_Print(object sender, RoutedEventArgs e)
        {
            TextBox tb = GetCurrentTextBox(); if (tb == null) return;
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true) { FlowDocument doc = new FlowDocument(new Paragraph(new Run(tb.Text))); pd.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator, "Печать"); }
        }

        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*" };
            if (ofd.ShowDialog() == true) CreateNewTab(Path.GetFileName(ofd.FileName), File.ReadAllText(ofd.FileName, _currentEncoding), ofd.FileName);
        }

        private void Button_Click_Save_File(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                TextBox textBox = GetCurrentTextBox(); if (textBox == null) return;
                if (string.IsNullOrEmpty(tabData.FilePath)) { SaveFileDialog sfd = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" }; if (sfd.ShowDialog() == true) { tabData.FilePath = sfd.FileName; tabData.HeaderText = Path.GetFileName(sfd.FileName); } else return; }
                File.WriteAllText(tabData.FilePath, textBox.Text, _currentEncoding); tabData.IsDirty = false; UpdateSidebar();
            }
        }

        private void Button_Click_Save_File_As(object sender, RoutedEventArgs e)
        {
            if (tabControlFiles.SelectedItem is TabItem currentTab && currentTab.DataContext is TabItemData tabData)
            {
                TextBox textBox = GetCurrentTextBox(); if (textBox == null) return;
                SaveFileDialog sfd = new SaveFileDialog { Filter = "Текстовые файлы (*.txt)|*.txt" };
                if (sfd.ShowDialog() == true) { tabData.FilePath = sfd.FileName; tabData.HeaderText = Path.GetFileName(sfd.FileName); File.WriteAllText(tabData.FilePath, textBox.Text, _currentEncoding); tabData.IsDirty = false; UpdateSidebar(); }
            }
        }

        private void MenuItem_SaveAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (TabItem item in tabControlFiles.Items) if (item.DataContext is TabItemData data && item.Content is Grid g) { var b = g.Children.OfType<TextBox>().FirstOrDefault(x => x.Focusable); if (b != null && !string.IsNullOrEmpty(data.FilePath)) { File.WriteAllText(data.FilePath, b.Text, _currentEncoding); data.IsDirty = false; } }
            UpdateSidebar();
        }

        private void MenuItem_SortLines_Click(object sender, RoutedEventArgs e) { var b = GetCurrentTextBox(); if (b != null) b.Text = string.Join(Environment.NewLine, b.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).OrderBy(x => x)); }
        private void MenuItem_RemoveEmptyLines_Click(object sender, RoutedEventArgs e) { var b = GetCurrentTextBox(); if (b != null) b.Text = string.Join(Environment.NewLine, b.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x))); }
        private void MenuItem_InsertDateTime_Click(object sender, RoutedEventArgs e) { var b = GetCurrentTextBox(); if (b != null) b.SelectedText = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); }
        private void UpdateStatusBarStats() { TextBox box = GetCurrentTextBox(); if (box != null && lblLinesCount != null) { lblLinesCount.Text = $"Строк: {box.LineCount}"; lblCharCount.Text = $"Символов: {box.Text.Length}"; } }
        private void UpdateSidebar() { if (lstSidebarFiles == null) return; List<TabItemData> list = new List<TabItemData>(); foreach (TabItem item in tabControlFiles.Items) if (item.DataContext is TabItemData d) list.Add(d); lstSidebarFiles.ItemsSource = list; }
        private void TabControlFiles_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateStatusBarStats();
        private void LstSidebarFiles_SelectionChanged(object sender, SelectionChangedEventArgs e) { if (lstSidebarFiles.SelectedItem is TabItemData d) { TabItem t = tabControlFiles.Items.Cast<TabItem>().FirstOrDefault(x => x.DataContext == d); if (t != null) tabControlFiles.SelectedItem = t; } }
        private void SetupBuiltInCommands()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, (s, e) => { SearchPanel.Visibility = SearchPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; if (SearchPanel.Visibility == Visibility.Visible) textBoxFind.Focus(); }));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.New, Button_Click_New_File));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, Button_Click_Open_File));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, Button_Click_Save_File));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Print, Button_Click_Print));
        }

        private void ToggleSettingsSidebar_Click(object sender, RoutedEventArgs e) => RightSettingsSidebar.Visibility = RightSettingsSidebar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        private void Click_Left_Panel_Items(object sender, RoutedEventArgs e) => LeftSidebar.Visibility = LeftSidebar.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        private void CloseSearchPanel_Click(object sender, RoutedEventArgs e) => SearchPanel.Visibility = Visibility.Collapsed;
        private void Button_Click_Close(object sender, RoutedEventArgs e) => CloseCurrentTab();
        private void CloseTab_Click(object sender, RoutedEventArgs e) => CloseCurrentTab();
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) => Close();
        private void Window_Closing(object sender, CancelEventArgs e) { _speechEngine?.Dispose(); }

        private void ApplyFontSizeToContainer(TextBox mainTextBox, TextBox lineNumbersBox)
        {
            double computedSize = _baseFontSize * (_globalZoom / 100);
            if (mainTextBox != null) mainTextBox.FontSize = computedSize;
            if (lineNumbersBox != null) lineNumbersBox.FontSize = computedSize;
        }
        #endregion
    }
}