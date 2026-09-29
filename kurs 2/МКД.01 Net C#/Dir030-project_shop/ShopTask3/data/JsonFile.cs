using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopTask.data
{
    internal static class JsonFile
    {
        public static void Save<T>(string path, ObservableCollection<T> list)
        {
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            File.WriteAllText(path, json);
        }
    }
}
