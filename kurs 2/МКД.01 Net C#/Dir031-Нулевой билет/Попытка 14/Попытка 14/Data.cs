using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Попытка_14
{
    public static class Data
    {
         public static ObservableCollection<User> Users { get; set; }
         public static ObservableCollection<Brone> Brones { get; set; }
         public static ObservableCollection<Room> Rooms { get; set; }

        public static void Read()
        {
            if (!File.Exists("users.json"))
            {
                Users = new ObservableCollection<User>()
                {
                    new User(){Id = 0, Login = "admin", Password = "admin", Phone="7777777", Role=UserRole.ADMIN}
                };
                Brones = new ObservableCollection<Brone>();
                Rooms = new ObservableCollection<Room>();
                return;
            }

            Users = JsonSerializer.Deserialize<ObservableCollection<User>>(File.ReadAllText("users.json"));
            Brones = JsonSerializer.Deserialize<ObservableCollection<Brone>>(File.ReadAllText("brones.json"));
            Rooms = JsonSerializer.Deserialize<ObservableCollection<Room>>(File.ReadAllText("rooms.json"));
        }

        public static void Write()
        {
            File.WriteAllText("users.json", JsonSerializer.Serialize(Users, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}));
            File.WriteAllText("brones.json", JsonSerializer.Serialize(Brones, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}));
            File.WriteAllText("rooms.json", JsonSerializer.Serialize(Rooms, new JsonSerializerOptions() { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping}));
        }
    }
}
