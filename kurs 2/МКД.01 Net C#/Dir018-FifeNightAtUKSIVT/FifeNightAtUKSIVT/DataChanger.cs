using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FifeNightAtUKSIVT
{
    internal static class DataChanger
    {
        private static Random random = new Random();
        public static void RunerMainInTimer()
        {
            if (random.Next(100) < 20)
                Data.StageIlyas++;
        }
    }
}
