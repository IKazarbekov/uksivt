using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка3_в_кабинете.models
{
    class Brone
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int LessonID { get; set; }
        public DateTime DateTime { get; set; }
        public Status Status { get; set; }
    }
}
