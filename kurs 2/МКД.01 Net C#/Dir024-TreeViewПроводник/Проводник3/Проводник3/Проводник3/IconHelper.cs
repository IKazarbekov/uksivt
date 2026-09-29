using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Проводник3
{
    public static class IconHelper
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        public static ImageSource GetFolderIcon(bool large = false)
        {
            return GetIconByAttributes(@"C:\dummy", FILE_ATTRIBUTE_DIRECTORY, large);
        }

        public static ImageSource GetFileIcon(string filePath, bool large = false)
        {
            uint flags = SHGFI_ICON | (large ? SHGFI_LARGEICON : SHGFI_SMALLICON);
            var shfi = new SHFILEINFO();
            IntPtr result = SHGetFileInfo(filePath, 0, ref shfi, (uint)Marshal.SizeOf(shfi), flags);
            if (result == IntPtr.Zero || shfi.hIcon == IntPtr.Zero)
                return GetFallbackIcon();

            var icon = (Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
            DestroyIcon(shfi.hIcon);
            var imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            icon.Dispose();
            return imageSource;
        }

        public static ImageSource GetDiskIcon(string rootPath, bool large = false)
        {
            return GetFileIcon(rootPath, large);
        }

        private static ImageSource GetIconByAttributes(string path, uint attributes, bool large)
        {
            uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES |
                         (large ? SHGFI_LARGEICON : SHGFI_SMALLICON);
            var shfi = new SHFILEINFO();
            IntPtr result = SHGetFileInfo(path, attributes, ref shfi, (uint)Marshal.SizeOf(shfi), flags);
            if (result == IntPtr.Zero || shfi.hIcon == IntPtr.Zero)
                return GetFallbackIcon();

            var icon = (Icon)System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
            DestroyIcon(shfi.hIcon);
            var imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
            icon.Dispose();
            return imageSource;
        }

        private static ImageSource GetFallbackIcon()
        {
            var bitmap = new WriteableBitmap(16, 16, 96, 96, PixelFormats.Bgra32, null);
            return bitmap;
        }
    }
}