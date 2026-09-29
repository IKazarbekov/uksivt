using System;
using System.Collections.Generic;
using System.Text;

namespace Work_09_05_documentation.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Cabinet { get; set; }
        public string Status { get; set; }
    }
}
