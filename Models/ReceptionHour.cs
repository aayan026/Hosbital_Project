
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Project.Models
{
    public class ReceptionHour
    {
        public string start { get; set; }
        public string end { get; set; }
        public bool isReserved { get; set; }

        public ReceptionHour() { }
        public ReceptionHour(string start, string end)
        {
            this.start = start;
            this.end = end;
            this.isReserved = false;
        }

        public override string ToString() => $@"{start} - {end} - {(isReserved ? "reserved" : "")}";
        public string ToString(bool isShort) => isShort ? $"{start} - {end}" : ToString();
    }
}