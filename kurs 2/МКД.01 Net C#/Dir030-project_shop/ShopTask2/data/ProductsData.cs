using ShopTask.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTask.data
{
    internal static class ProductsData
    {
        public static ObservableCollection<Product> Products { get; set; }
    }
}
