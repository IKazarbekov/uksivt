using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace на_сдачу_учебной_пратикик.models
{
    public class Brone
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int LessonID { get; set; }
        public bool Accept {  get; set; }
        public DateTime Date {  get; set; }
    }
}
