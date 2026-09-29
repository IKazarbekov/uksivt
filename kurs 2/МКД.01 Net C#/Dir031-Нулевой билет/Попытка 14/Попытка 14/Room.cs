using System;
using System.Collections.Generic;
using System.Text;

namespace Попытка_14
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int Atash { get; set; }
        public bool Proector { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
