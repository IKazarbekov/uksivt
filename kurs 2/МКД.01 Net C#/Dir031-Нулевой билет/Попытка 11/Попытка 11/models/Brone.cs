using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка_11.models
{
    class Brone
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public int LessonId { get; set; }
        public bool Accept { get; set; }
    }
}
