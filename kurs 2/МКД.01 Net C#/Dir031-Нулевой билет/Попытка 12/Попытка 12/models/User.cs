using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка_12.models
{
    public class User
    {
        public int Id{ get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Number { get; set; }
    }
}
