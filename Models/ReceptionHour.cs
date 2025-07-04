
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class ReceptionHour
    {
        public TimeSpan start { get; set; }
        public TimeSpan end { get; set; }
        public bool isReserved { get; set; }
        public ReceptionHour(string start, string end)
        {
            this.start = TimeSpan.Parse(start);
            this.end = TimeSpan.Parse(end);
            this.isReserved = false;
        }
        public override string ToString() => $@"{start.ToString("hh\\:mm")} - {end.ToString("hh\\:mm")} - {(isReserved? "reserved" : "")}";
        public string ToString(bool isShort) => isShort ? $"{start.ToString("hh\\:mm")} - {end.ToString("hh\\:mm")}" : ToString();
    }
}
