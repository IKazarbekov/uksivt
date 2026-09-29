using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка_14
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }

        public override string ToString()
        {
            return Login;
        }
    }
}
