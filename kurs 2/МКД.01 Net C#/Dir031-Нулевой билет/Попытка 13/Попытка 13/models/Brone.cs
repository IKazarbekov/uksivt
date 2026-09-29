using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Попытка_13.models
{
    public class Brone
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool Accept { get; set; }
    }
}
