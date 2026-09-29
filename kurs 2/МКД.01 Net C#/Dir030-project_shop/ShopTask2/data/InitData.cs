using ShopTask.models;
using ShopTask2.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ShopTask.data
{
    internal static class InitData
    {
        public static void Init()
        {
            UsersData.Users = new ObservableCollection<User>();
            if (File.Exists(Config.NAME_FILE_USERS))
                UsersData.Users = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText(Config.NAME_FILE_USERS));
            ProductsData.Products = new ObservableCollection<Product>();
            if (File.Exists(Config.NAME_FILE_PRODUCTS))
                ProductsData.Products = JsonSerializer.Deserialize<ObservableCollection<Product>>(File.ReadAllText(Config.NAME_FILE_PRODUCTS));
            if (File.Exists(Config.NAME_FILE_SALES))
                SellerData.Sales = JsonSerializer.Deserialize<ObservableCollection<Sale>>(File.ReadAllText(Config.NAME_FILE_SALES));
            if (!Directory.Exists(Config.NAME_DIR_IMAGES))
                Directory.CreateDirectory(Config.NAME_DIR_IMAGES);

            if (UsersData.Users == null)
                UsersData.Users = new ObservableCollection<User>();
            if (ProductsData.Products == null)
                ProductsData.Products = new ObservableCollection<Product>();
            if (SellerData.Sales == null)
                SellerData.Sales = new ObservableCollection<Sale>();
        }

        public static void Save()
        {
            JsonFile.Save(Config.NAME_FILE_USERS, UsersData.Users);
            JsonFile.Save(Config.NAME_FILE_PRODUCTS, ProductsData.Products);
            JsonFile.Save(Config.NAME_FILE_SALES, SellerData.Sales);
        }
    }
}
