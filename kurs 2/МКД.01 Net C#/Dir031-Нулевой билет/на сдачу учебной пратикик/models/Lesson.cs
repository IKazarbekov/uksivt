using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace на_сдачу_учебной_пратикик.models
{
    public class Lesson
    {
       public int Id { get; set; }
       public string Trenner { get; set; }
       public string Type { get; set; }
        public DateTime Date { get; set; }
       public int Count { get; set; }
    }
}
