using ShopTask.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopTask.data
{
    internal static class InitData
    {
        public static void Init()
        {
            UsersData.Users = new ObservableCollection<User>();
            UsersData.Users = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText(Config.NAME_FILE_USERS));
            ProductsData.Products = new ObservableCollection<Product>();
            ProductsData.Products = JsonSerializer.Deserialize<ObservableCollection<Product>>(File.ReadAllText(Config.NAME_FILE_PRODUCTS));
        }

        public static void Save()
        {
            JsonFile.Save(Config.NAME_FILE_USERS, UsersData.Users);
            JsonFile.Save(Config.NAME_FILE_PRODUCTS, ProductsData.Products);
        }
    }
}
