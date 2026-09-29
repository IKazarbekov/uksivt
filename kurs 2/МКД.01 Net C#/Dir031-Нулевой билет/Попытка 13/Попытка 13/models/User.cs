using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка_13.models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int Number { get; set; }
        public UserRole Role { get; set; }
    }
}
