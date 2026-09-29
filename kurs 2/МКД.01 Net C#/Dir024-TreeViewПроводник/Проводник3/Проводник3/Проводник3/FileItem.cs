using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Проводник3
{
    public class FileItem : INotifyPropertyChanged
    {
        private string _path = string.Empty;
        private string _name = string.Empty;
        private string _modified = string.Empty;
        private string _type = string.Empty;
        private string _size = string.Empty;
        private ImageSource? _icon;

        public string Path
        {
            get => _path;
            set { _path = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        public string Modified
        {
            get => _modified;
            set { _modified = value; OnPropertyChanged(); }
        }

        public string Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        public string Size
        {
            get => _size;
            set { _size = value; OnPropertyChanged(); }
        }

        public ImageSource? Icon
        {
            get => _icon;
            set { _icon = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}