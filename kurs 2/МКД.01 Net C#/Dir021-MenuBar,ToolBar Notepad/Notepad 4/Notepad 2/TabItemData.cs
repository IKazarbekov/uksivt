using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Notepad_2
{
    public class TabItemData : INotifyPropertyChanged
    {
        private string _headerText;
        private string _filePath;
        private bool _isDirty;
        public string GuidId { get; } = System.Guid.NewGuid().ToString();

        public string HeaderText
        {
            get => _isDirty ? _headerText + " *" : _headerText;
            set { _headerText = value.Replace(" *", ""); OnPropertyChanged(); }
        }

        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(); }
        }

        public bool IsDirty
        {
            get => _isDirty;
            set { _isDirty = value; OnPropertyChanged(nameof(HeaderText)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}