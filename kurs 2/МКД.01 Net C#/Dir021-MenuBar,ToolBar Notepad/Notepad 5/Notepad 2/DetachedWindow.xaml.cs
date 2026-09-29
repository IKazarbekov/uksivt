using System;
using System.Windows;
using System.Windows.Controls;

namespace Notepad_2
{
    public partial class DetachedWindow : Window
    {
        private Grid _savedGrid;
        private TabItemData _savedData;
        private MainWindow _mainWin;

        public DetachedWindow(Grid editorGrid, TabItemData data, MainWindow main)
        {
            InitializeComponent();
            _savedGrid = editorGrid;
            _savedData = data;
            _mainWin = main;

            Title = $"[Внешнее Окно] - {data.HeaderText}";

            // Забираем Grid из старой вкладки и вставляем в это окно
            WindowContentGrid.Children.Add(editorGrid);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Перед закрытием возвращаем Grid обратно в главное окно, чтобы не потерять текст
            WindowContentGrid.Children.Remove(_savedGrid);
            _mainWin.ReturnTabToMain(_savedGrid, _savedData);
        }
    }
}