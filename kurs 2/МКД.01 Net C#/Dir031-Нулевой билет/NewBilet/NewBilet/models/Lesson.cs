using System.Text.Json.Serialization;

namespace NewBilet.models
{
    internal class Lesson
    {
        public DateTime Date { get; set; }
        [JsonIgnore]
        public string DateStr
        {
            get
            {
                return Date.ToShortDateString();
            }
        }
        public int ID { get; set; }
        public string TrannerName { get; set; }
        public string TrannerNameStr { get { return "Треннер: " + TrannerName; } }
        public int Count { get; set; }
        public string CountStr
        {
            get
            {
                return "Мест: " + Count;
            }
        }
    }
}
