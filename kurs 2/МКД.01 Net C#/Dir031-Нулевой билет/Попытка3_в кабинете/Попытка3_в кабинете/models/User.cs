using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка3_в_кабинете.models
{
    class User
    {
        public int ID {  get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string NumberPhone { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
