using System.Text.Json.Serialization;

namespace PROJECT_ЭКЗАМЕН_1_ПОПЫТКА.models
{
    class Brone
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int LessonID { get; set; }
        public DateTime Date { get; set; }
        public BroneStatus Status { get; set; }
        public string ToString()
        {
            return "" + ID + "-" + UserID;
        }

        [JsonIgnore]
        public string StatusStr
        {
            get
            {
                if (Status == BroneStatus.ACCEPT)
                    return "Принято";
                else
                    return "Отменено";
            }
        }

        [JsonIgnore]
        public bool StatusChk
        {
            get { return Status == BroneStatus.ACCEPT; }
            set { Status = (value) ? BroneStatus.ACCEPT : BroneStatus.CANCEL; }
        }

        [JsonIgnore]
        public string DateStr
        {
            get
            {
                return "Дата брони: " + Date.ToLongDateString();
            }
        }
    }
}
