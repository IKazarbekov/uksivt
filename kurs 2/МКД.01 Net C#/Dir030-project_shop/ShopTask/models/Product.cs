using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTask.models
{
    internal class Product
    {
        public string Description {  get; set; }
        public Category category { get; set; }
        public Status status { get; set; }
        public int Price { get; set; }
    }
}
