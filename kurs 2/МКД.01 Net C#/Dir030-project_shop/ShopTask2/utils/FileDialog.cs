using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ShopTask2.utils
{
    internal static class FileDialog
    {
        public static bool OpenImage(out string path)
        {
            var dialog = new OpenFileDialog() { Filter = "all|*.*|images|*.png" };
            dialog.ShowDialog();
            path = dialog.FileName;
            if (path == null)
                return false;
            if (!File.Exists(path))
            {
                MessageBox.Show("Файл не найден");
                return false;
            }
            if (!new List<string>() { ".png", ".jpg" }.Contains(Path.GetExtension(path)))
            {
                MessageBox.Show("Файл не является изображением:" + Path.GetExtension(path));
                return false;
            }
            return true;
        }
    }
}
