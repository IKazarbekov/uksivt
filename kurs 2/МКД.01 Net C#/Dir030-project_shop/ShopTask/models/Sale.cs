using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTask.models
{
    internal class Sale
    {
        public string client {  get; set; }
        public DateTime date { get; set; }
        public List<Product> products { get; set; }
    }
}
