using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTask.models
{
    internal class Sale
    {
        public string client {  get; set; }
        public DateTime date { get; set; }
        public Dictionary<string, int> salleNameAndCount { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder($"Покупатель: {client}; Дата покупки: {date}; Товары: ");
            foreach ( var item in salleNameAndCount)
            {
                builder.Append( item.Key + ":" + item.Value + " Раз,");
            }
            return builder.ToString();
        }
    }
}
