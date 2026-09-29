using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTask.models
{
    public class User
    {
        public string login { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public Role role { get; set; }
    }
}
