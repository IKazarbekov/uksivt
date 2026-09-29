using System.Text.Json.Serialization;

namespace Попытка_14
{
    public class Brone
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        [JsonIgnore]
        public string UserName
        {
            get
            {
                User user = Data.Users.Where(u => u.Id == UserId).First();
                return user.Login;
            }
        }

        [JsonIgnore]
        public string RoomName
        {
            get
            {
                return Data.Rooms.Where(u => u.Id == RoomId).First().Name;
            }
        }



    }
}
