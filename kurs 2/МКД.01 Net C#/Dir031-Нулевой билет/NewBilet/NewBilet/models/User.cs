using System;
using System.Collections.Generic;
using System.Text;

namespace NewBilet.models
{
    internal class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
