using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using ShopTask.models;

namespace ShopTask.data
{
    public static class UsersData
    {
        public static ObservableCollection<User> Users { get; set; }
    }
}
