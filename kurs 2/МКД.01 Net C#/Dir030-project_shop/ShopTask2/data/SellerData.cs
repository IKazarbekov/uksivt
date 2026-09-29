using ShopTask.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTask2.data
{
    internal class SellerData
    {
        public static ObservableCollection<Sale> Sales {  get; set; }
    }
}
