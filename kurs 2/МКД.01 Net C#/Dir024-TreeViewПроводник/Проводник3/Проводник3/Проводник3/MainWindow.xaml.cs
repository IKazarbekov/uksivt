using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Проводник3;

namespace Проводник3
{
    public partial class MainWindow : Window
    {
        private string _currentPath = string.Empty;
        private readonly ObservableCollection<FileItem> _fileItems = new();
        private readonly Stack<string> _backStack = new();
        private readonly Stack<string> _forwardStack = new();

        private GridView _gridView;
        private GridView _listView;

        // Буфер для копирования/вырезания
        private List<string> _clipboardFiles = new List<string>();
        private bool _isCutOperation = false;

        public MainWindow()
        {
            InitializeComponent();
            ListViewFiles.ItemsSource = _fileItems;
            SetQuickAccessPaths();
            InitializeViews();
            ViewModeBox.SelectionChanged += ViewModeBox_SelectionChanged;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDrives();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void InitializeViews()
        {
            //_gridView = (GridView)ListViewFiles.PreviewDrop;

            _listView = new GridView();
            var listColumn = new GridViewColumn
            {
                Header = "Имя",
                Width = 600,
                CellTemplate = CreateListItemTemplate()
            };
            _listView.Columns.Add(listColumn);
        }

        private DataTemplate CreateListItemTemplate()
        {
            var template = new DataTemplate(typeof(FileItem));
            var panel = new FrameworkElementFactory(typeof(StackPanel));
            panel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            var image = new FrameworkElementFactory(typeof(Image));
            image.SetBinding(Image.SourceProperty, new Binding("Icon"));
            image.SetValue(Image.WidthProperty, 16.0);
            image.SetValue(Image.HeightProperty, 16.0);
            image.SetValue(Image.MarginProperty, new Thickness(0, 0, 5, 0));
            image.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);

            var text = new FrameworkElementFactory(typeof(TextBlock));
            text.SetBinding(TextBlock.TextProperty, new Binding("Name"));
            text.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

            panel.AppendChild(image);
            panel.AppendChild(text);

            template.VisualTree = panel;
            return template;
        }

        private void LoadDrives()
        {
            try
            {
                var drives = DriveInfo.GetDrives()
                                      .Where(d => d.IsReady)
                                      .ToList();

                ThisPCNode.Items.Clear();
                foreach (var drive in drives)
                {
                    var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    var icon = new Image
                    {
                        Source = IconHelper.GetDiskIcon(drive.RootDirectory.FullName, large: false),
                        Width = 16,
                        Height = 16,
                        Margin = new Thickness(0, 0, 5, 0)
                    };
                    var text = new TextBlock { Text = $"{drive.Name} ({drive.VolumeLabel})" };
                    headerPanel.Children.Add(icon);
                    headerPanel.Children.Add(text);

                    var item = new TreeViewItem
                    {
                        Header = headerPanel,
                        Tag = drive.RootDirectory.FullName
                    };
                    item.Items.Add(new TreeViewItem { Header = "Загрузка...", Tag = null });
                    item.Expanded += TreeViewItem_Expanded;
                    ThisPCNode.Items.Add(item);
                }
                ThisPCNode.IsExpanded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки дисков: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetQuickAccessPaths()
        {
            foreach (TreeViewItem node in TreeViewFolders.Items)
            {
                if (node.Tag is string && string.IsNullOrEmpty(node.Tag.ToString()))
                {
                    foreach (TreeViewItem child in node.Items)
                    {
                        string path = ResolveSpecialFolder(child.Tag);
                        child.Tag = path;
                        if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                        {
                            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
                            var icon = new Image
                            {
                                Source = IconHelper.GetFolderIcon(large: false),
                                Width = 16,
                                Height = 16,
                                Margin = new Thickness(0, 0, 5, 0)
                            };
                            string headerText = child.Header?.ToString() ?? "";
                            var textBlock = new TextBlock { Text = headerText };
                            headerPanel.Children.Add(icon);
                            headerPanel.Children.Add(textBlock);
                            child.Header = headerPanel;

                            child.Items.Add(new TreeViewItem { Header = "Загрузка...", Tag = null });
                            child.Expanded += TreeViewItem_Expanded;
                        }
                    }
                }
            }
        }

        private static string ResolveSpecialFolder(object? tag)
        {
            if (tag is not string key) return string.Empty;

            return key switch
            {
                "Desktop" => Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Documents" => Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Music" => Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                "Pictures" => Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                "Videos" => Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                "Downloads" => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                _ => string.Empty
            };
        }

        private void TreeViewFolders_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeViewFolders.SelectedItem is TreeViewItem item && item.Tag is string path)
            {
                if (Directory.Exists(path))
                {
                    NavigateTo(path);
                }
            }
        }

        private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (e.Source is not TreeViewItem item || item.Tag is not string path) return;
            if (!Directory.Exists(path)) return;

            item.Items.Clear();

            try
            {
                foreach (var directory in Directory.GetDirectories(path))
                {
                    var dirInfo = new DirectoryInfo(directory);
                    var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    var icon = new Image
                    {
                        Source = IconHelper.GetFolderIcon(large: false),
                        Width = 16,
                        Height = 16,
                        Margin = new Thickness(0, 0, 5, 0)
                    };
                    var text = new TextBlock { Text = dirInfo.Name };
                    headerPanel.Children.Add(icon);
                    headerPanel.Children.Add(text);

                    var child = new TreeViewItem
                    {
                        Header = headerPanel,
                        Tag = dirInfo.FullName
                    };
                    child.Items.Add(new TreeViewItem { Header = "Загрузка...", Tag = null });
                    child.Expanded += TreeViewItem_Expanded;
                    item.Items.Add(child);
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки папок: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void NavigateTo(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;
            if (string.Equals(_currentPath, path, StringComparison.OrdinalIgnoreCase)) return;

            if (!string.IsNullOrEmpty(_currentPath))
            {
                _backStack.Push(_currentPath);
            }
            _forwardStack.Clear();

            _currentPath = path;
            LoadFiles(path);
            SelectTreeViewNode(path);
        }

        private void LoadFiles(string path)
        {
            _fileItems.Clear();
            if (!Directory.Exists(path)) return;

            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var dirInfo = new DirectoryInfo(dir);
                    _fileItems.Add(new FileItem
                    {
                        Path = dirInfo.FullName,
                        Name = dirInfo.Name,
                        Modified = dirInfo.LastWriteTime.ToString("dd.MM.yyyy HH:mm"),
                        Type = "Папка",
                        Size = "",
                        Icon = IconHelper.GetFolderIcon(false)
                    });
                }

                foreach (var file in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(file);
                    _fileItems.Add(new FileItem
                    {
                        Path = fileInfo.FullName,
                        Name = fileInfo.Name,
                        Modified = fileInfo.LastWriteTime.ToString("dd.MM.yyyy HH:mm"),
                        Type = fileInfo.Extension,
                        Size = FormatSize(fileInfo.Length),
                        Icon = IconHelper.GetFileIcon(fileInfo.FullName, false)
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Нет доступа к папке.", "Ошибка доступа",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки содержимого: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string FormatSize(long bytes)
        {
            string[] sizes = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }

        private void ListViewFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewFiles.SelectedItem is not FileItem item) return;

            if (item.Type == "Папка")
            {
                NavigateTo(item.Path);
            }
            else
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = item.Path,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть файл: {ex.Message}", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // ---------- Контекстное меню (правая кнопка) ----------
        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            string? path = GetSelectedItemPath();
            if (string.IsNullOrEmpty(path)) return;

            if (Directory.Exists(path))
                NavigateTo(path);
            else
            {
                try { Process.Start(new ProcessStartInfo { FileName = path, UseShellExecute = true }); }
                catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
            }
        }

        private void MenuItem_CopyPath_Click(object sender, RoutedEventArgs e)
        {
            string? path = GetSelectedItemPath();
            if (!string.IsNullOrEmpty(path))
                Clipboard.SetText(path);
        }

        private void MenuItem_Properties_Click(object sender, RoutedEventArgs e)
        {
            string? path = GetSelectedItemPath();
            if (!string.IsNullOrEmpty(path))
                ShowPropertiesDialog(path);
        }

        private string? GetSelectedItemPath()
        {
            if (ListViewFiles.SelectedItem is FileItem fileItem)
                return fileItem.Path;

            if (TreeViewFolders.SelectedItem is TreeViewItem treeItem && treeItem.Tag is string treePath)
                return treePath;

            return null;
        }

        private void ShowPropertiesDialog(string path)
        {
            try
            {
                NativeMethods.ShellExecuteProperties(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть свойства: {ex.Message}");
            }
        }

        // ---------- Навигационные кнопки ----------
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (_backStack.Count == 0) return;

            _forwardStack.Push(_currentPath);
            string previousPath = _backStack.Pop();
            _currentPath = previousPath;
            LoadFiles(previousPath);
            SelectTreeViewNode(previousPath);
        }

        private void BtnForward_Click(object sender, RoutedEventArgs e)
        {
            if (_forwardStack.Count == 0) return;

            _backStack.Push(_currentPath);
            string nextPath = _forwardStack.Pop();
            _currentPath = nextPath;
            LoadFiles(nextPath);
            SelectTreeViewNode(nextPath);
        }

        private void BtnUp_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPath)) return;
            try
            {
                var parent = Directory.GetParent(_currentPath);
                if (parent != null)
                {
                    NavigateTo(parent.FullName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка перехода вверх: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentPath))
            {
                LoadFiles(_currentPath);
            }
        }

        private void ViewModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_gridView == null || _listView == null) return;

            if (ViewModeBox.SelectedIndex == 0)
            {
                //ListViewFiles.View = _gridView;
            }
            else if (ViewModeBox.SelectedIndex == 1)
            {
                //ListViewFiles.View = _listView;
            }
        }

        private void SelectTreeViewNode(string path)
        {
            TreeViewItem? node = FindNode(TreeViewFolders, path);
            if (node != null)
            {
                node.IsSelected = true;
                node.BringIntoView();
            }
        }

        private static TreeViewItem? FindNode(ItemsControl container, string path)
        {
            foreach (var item in container.Items)
            {
                if (item is TreeViewItem treeItem)
                {
                    if (treeItem.Tag is string tag && tag.Equals(path, StringComparison.OrdinalIgnoreCase))
                        return treeItem;

                    if (treeItem.Items.Count > 0)
                    {
                        var found = FindNode(treeItem, path);
                        if (found != null) return found;
                    }
                }
            }
            return null;
        }

        private void ListViewFiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewFiles.SelectedItem is FileItem selected)
                TxtPath.Text = selected.Path;
            else
                TxtPath.Text = "Выделите файл или папку...";
        }

        // ================== Методы для кнопок вкладки "Главная" ==================

        private List<string> GetSelectedPaths()
        {
            var paths = new List<string>();
            foreach (FileItem item in ListViewFiles.SelectedItems)
            {
                if (!string.IsNullOrEmpty(item.Path))
                    paths.Add(item.Path);
            }
            // Если ничего не выбрано в списке, пробуем получить из дерева
            if (paths.Count == 0)
            {
                string? path = GetSelectedItemPath();
                if (!string.IsNullOrEmpty(path))
                    paths.Add(path);
            }
            return paths;
        }

        // Копировать 
        private void InMainMenu_Copy_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedPaths();
            if (selected.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов.", "Копировать");
                return;
            }
            _clipboardFiles = new List<string>(selected);
            _isCutOperation = false;
            Clipboard.SetText(string.Join("\n", _clipboardFiles));
            MessageBox.Show($"Скопировано элементов: {selected.Count}", "Копировать");
        }

        // Вырезать (✂️)
        private void InMainMenu_Cut_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedPaths();
            if (selected.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов.", "Вырезать");
                return;
            }
            _clipboardFiles = new List<string>(selected);
            _isCutOperation = true;
            Clipboard.SetText(string.Join("\n", _clipboardFiles));
            MessageBox.Show($"Элементов помечено для вырезания: {selected.Count}", "Вырезать");
        }

        // Вставить
        private void InMainMenu_Paste_Click(object sender, RoutedEventArgs e)
        {
            if (_clipboardFiles.Count == 0)
            {
                MessageBox.Show("Буфер обмена пуст.", "Вставить");
                return;
            }
            if (string.IsNullOrEmpty(_currentPath))
            {
                MessageBox.Show("Перейдите в папку, куда нужно вставить.", "Вставить");
                return;
            }
            try
            {
                foreach (var sourcePath in _clipboardFiles)
                {
                    string destPath = Path.Combine(_currentPath, Path.GetFileName(sourcePath));
                    if (_isCutOperation)
                    {
                        if (Directory.Exists(sourcePath))
                            Directory.Move(sourcePath, destPath);
                        else
                            File.Move(sourcePath, destPath);
                    }
                    else
                    {
                        if (Directory.Exists(sourcePath))
                            CopyDirectory(sourcePath, destPath);
                        else
                            File.Copy(sourcePath, destPath, true);
                    }
                }
                if (_isCutOperation)
                {
                    _clipboardFiles.Clear();
                    _isCutOperation = false;
                }
                // Обновляем список
                LoadFiles(_currentPath);
                MessageBox.Show("Операция выполнена.", "Вставить");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка вставки: {ex.Message}", "Ошибка");
            }
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), true);
            foreach (var dir in Directory.GetDirectories(sourceDir))
                CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
        }

        // Копировать путь
        private void InMainMenu_CopyPath_Click(object sender, RoutedEventArgs e)
        {
            string? path = GetSelectedItemPath();
            if (!string.IsNullOrEmpty(path))
                Clipboard.SetText(path);
            else
                MessageBox.Show("Ничего не выделено.");
        }

        // Переместить в 
        private void InMainMenu_MoveTo_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedPaths();
            if (selected.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов.");
                return;
            }
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    foreach (var source in selected)
                    {
                        string dest = Path.Combine(dialog.FolderName, Path.GetFileName(source));
                        if (Directory.Exists(source))
                            Directory.Move(source, dest);
                        else
                            File.Move(source, dest);
                    }
                    if (!string.IsNullOrEmpty(_currentPath))
                        LoadFiles(_currentPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка перемещения: {ex.Message}");
                }
            }
        }

        // Копировать в 
        private void InMainMenu_CopyTo_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedPaths();
            if (selected.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов.");
                return;
            }
            var dialog = new OpenFolderDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    foreach (var source in selected)
                    {
                        string dest = Path.Combine(dialog.FolderName, Path.GetFileName(source));
                        if (Directory.Exists(source))
                            CopyDirectory(source, dest);
                        else
                            File.Copy(source, dest, true);
                    }
                    if (!string.IsNullOrEmpty(_currentPath))
                        LoadFiles(_currentPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка копирования: {ex.Message}");
                }
            }
        }

        // Удалить 
        private void InMainMenu_Delete_Click(object sender, RoutedEventArgs e)
        {
            var selected = GetSelectedPaths();
            if (selected.Count == 0)
            {
                MessageBox.Show("Нет выделенных элементов.");
                return;
            }
            string message = $"Вы действительно хотите удалить {selected.Count} элементов?";
            if (MessageBox.Show(message, "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    foreach (var path in selected)
                    {
                        if (Directory.Exists(path))
                            Directory.Delete(path, true);
                        else
                            File.Delete(path);
                    }
                    if (!string.IsNullOrEmpty(_currentPath))
                        LoadFiles(_currentPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}");
                }
            }
        }

        // Переименовать 
        private void InMainMenu_Rename_Click(object sender, RoutedEventArgs e)
        {
            string? path = GetSelectedItemPath();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Ничего не выделено.");
                return;
            }
            string oldName = Path.GetFileName(path);
            string? newName = Microsoft.VisualBasic.Interaction.InputBox("Введите новое имя:", "Переименовать", oldName);
            if (string.IsNullOrWhiteSpace(newName) || newName == oldName)
                return;

            try
            {
                string? directory = Path.GetDirectoryName(path);
                if (directory == null) return;
                string newPath = Path.Combine(directory, newName);
                if (Directory.Exists(path))
                    Directory.Move(path, newPath);
                else
                    File.Move(path, newPath);

                if (!string.IsNullOrEmpty(_currentPath))
                    LoadFiles(_currentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка переименования: {ex.Message}");
            }
        }

        // Новая папка
        private void InMainMenu_NewFolder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPath))
            {
                MessageBox.Show("Перейдите в папку, где хотите создать новую папку.");
                return;
            }
            string? name = Microsoft.VisualBasic.Interaction.InputBox("Имя новой папки:", "Новая папка", "Новая папка");
            if (string.IsNullOrWhiteSpace(name))
                return;
            try
            {
                string newPath = Path.Combine(_currentPath, name);
                Directory.CreateDirectory(newPath);
                LoadFiles(_currentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания папки: {ex.Message}");
            }
        }

        // Новый файл
        private void InMainMenu_NewFile_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_currentPath))
            {
                MessageBox.Show("Перейдите в папку, где хотите создать файл.");
                return;
            }
            string? name = Microsoft.VisualBasic.Interaction.InputBox("Имя файла (с расширением):", "Новый файл", "Новый текстовый документ.txt");
            if (string.IsNullOrWhiteSpace(name))
                return;
            try
            {
                string newPath = Path.Combine(_currentPath, name);
                File.Create(newPath).Close();
                LoadFiles(_currentPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка создания файла: {ex.Message}");
            }
        }

        // Свойства
        private void InMainMenu_Properties_Click(object sender, RoutedEventArgs e)
        {
            string? path = GetSelectedItemPath();
            if (!string.IsNullOrEmpty(path))
                ShowPropertiesDialog(path);
            else
                MessageBox.Show("Ничего не выделено.");
        }

        // ----------------------------------- Конец методов вкладки -----------------------------------

        // Класс для вызова системных свойств
        internal static class NativeMethods
        {
            [System.Runtime.InteropServices.DllImport("shell32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            private static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);

            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            private struct SHELLEXECUTEINFO
            {
                public int cbSize;
                public uint fMask;
                public IntPtr hwnd;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                public string lpVerb;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                public string lpFile;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                public string lpParameters;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                public string lpDirectory;
                public int nShow;
                public IntPtr hInstApp;
                public IntPtr lpIDList;
                [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
                public string lpClass;
                public IntPtr hkeyClass;
                public uint dwHotKey;
                public IntPtr hIcon;
                public IntPtr hProcess;
            }

            private const uint SEE_MASK_INVOKEIDLIST = 12;

            public static void ShellExecuteProperties(string path)
            {
                var info = new SHELLEXECUTEINFO();
                info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
                info.fMask = SEE_MASK_INVOKEIDLIST;
                info.hwnd = IntPtr.Zero;
                info.lpVerb = "properties";
                info.lpFile = path;
                info.nShow = 1;

                ShellExecuteEx(ref info);
            }
        }
    }
}