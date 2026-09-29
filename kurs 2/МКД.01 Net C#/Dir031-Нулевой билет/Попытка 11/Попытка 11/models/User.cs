using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка_11.models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole role {  get; set; }
    }
}
