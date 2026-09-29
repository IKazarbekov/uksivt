using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Билет0_Попытка_1.models
{
    internal class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
