using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Билет0_попытка_2
{
    
    internal static class Data
    {
        public static ObservableCollection<User> users { get; set; }

    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public Role role { get; set; }
    }

    public enum Role{
        Admin,
        Client
    }
}
