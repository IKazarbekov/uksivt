using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace Попытка_11.models
{
    public class Lesson
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Trenner { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
