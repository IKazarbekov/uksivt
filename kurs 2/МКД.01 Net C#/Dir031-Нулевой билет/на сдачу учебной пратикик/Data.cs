using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using на_сдачу_учебной_пратикик.models;

namespace на_сдачу_учебной_пратикик
{
    public class Data
    {
        public static ObservableCollection<User> Users = new ObservableCollection<User>();
        public static ObservableCollection<Lesson> Lessons = new ObservableCollection<Lesson>();
        public static ObservableCollection<Brone> Brones = new ObservableCollection<Brone>();

        public static void Write()
        {
            string json_user = JsonSerializer.Serialize(Users, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            string json_lesson = JsonSerializer.Serialize(Lessons, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            string json_brone = JsonSerializer.Serialize(Brones, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText("users.json", json_user);
            File.WriteAllText("lesson.json", json_lesson);
            File.WriteAllText("brone.json", json_brone);
        }

        public static void Read()
        {
            string json_user = File.ReadAllText("users.json");
            string json_brone = File.ReadAllText("brone.json");
            string json_lesson = File.ReadAllText("lesson.json");

            Users = JsonSerializer.Deserialize<ObservableCollection<User>>(json_user);
            Brones = JsonSerializer.Deserialize<ObservableCollection<Brone>>(json_brone);
            Lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(json_lesson);
        }
    }
}
