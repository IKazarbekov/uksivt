using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    public static class Link
    {
        public static void ToRegister(this object window) 
        {
            Window windowRegister = new WindowRegistration();
            windowRegister.Show();
            ((Window)window).Close();
        }

        public static void ToLogin(this object window)
        {
            Window windowRegister = new WindowLogin();
            windowRegister.Show();
            ((Window)window).Close();
        }

        public static void ToStart(this object window)
        {
            Window windowRegister = new MainWindow();
            windowRegister.Show();
            ((Window)window).Close();
        }
    }
}
