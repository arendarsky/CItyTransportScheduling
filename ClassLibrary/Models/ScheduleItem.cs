using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ScheduleItem {
        public string RouteName { get; set; }
        public string Destination { get; set; }
        public int MinutesLeft { get; set; }
    }
}
