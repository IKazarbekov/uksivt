using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Work_09_05_documentation.Models;

namespace Work_09_05_documentation.Data
{
    public class DataStore
    {
        private static DataStore instance;
        public static DataStore Instance => instance ?? (instance = new DataStore());

        public List<User> Users { get; set; }
        public List<Request> Requests { get; set; }

        private string filePath = "data.xml";

        private DataStore()
        {
            Load();
        }

        public void Load()
        {
            if (File.Exists(filePath))
            {
                var serializer = new XmlSerializer(typeof(DataStore));
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    var obj = serializer.Deserialize(fs) as DataStore;
                    if (obj != null)
                    {
                        Users = obj.Users ?? new List<User>();
                        Requests = obj.Requests ?? new List<Request>();
                        return;
                    }
                }
            }
            Users = new List<User>();
            Requests = new List<Request>();
        }

        public void Save()
        {
            var serializer = new XmlSerializer(typeof(DataStore));
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }

        // ----- Методы работы с данными -----

        public User GetUser(string username, string password)
            => Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        public bool AddUser(User user)
        {
            if (Users.Any(u => u.Username == user.Username))
                return false;
            user.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
            Users.Add(user);
            Save();
            return true;
        }

        public List<Request> GetAllRequests()
            => Requests.OrderByDescending(r => r.Date).ToList();

        public List<Request> GetRequestsByUser(int userId)
            => Requests.Where(r => r.UserId == userId).OrderByDescending(r => r.Date).ToList();

        public void AddRequest(Request request)
        {
            request.Id = Requests.Any() ? Requests.Max(r => r.Id) + 1 : 1;
            Requests.Add(request);
            Save();
        }

        public void UpdateRequestStatus(int requestId, string newStatus)
        {
            var req = Requests.FirstOrDefault(r => r.Id == requestId);
            if (req != null)
            {
                req.Status = newStatus;
                Save();
            }
        }

        public List<User> GetAllEmployees()
            => Users.Where(u => u.Role == "Сотрудник").ToList();

        public List<string> GetAllCabinets()
            => Requests.Select(r => r.Cabinet).Distinct().ToList();

        public List<Request> FilterRequests(int? employeeId, string cabinet, DateTime? date)
        {
            var query = Requests.AsQueryable();
            if (employeeId.HasValue && employeeId.Value > 0)
                query = query.Where(r => r.UserId == employeeId.Value);
            if (!string.IsNullOrEmpty(cabinet))
                query = query.Where(r => r.Cabinet == cabinet);
            if (date.HasValue)
                query = query.Where(r => r.Date.Date == date.Value.Date);
            return query.OrderByDescending(r => r.Date).ToList();
        }
    }
}
