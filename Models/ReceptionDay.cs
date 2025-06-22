using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_homework.Models
{
    internal class ReceptionDay
    {
        public DayOfWeek dayOfWeek { get; set; }
        public List<ReceptionHour> TimeSlots { get; set; }

        public ReceptionDay(DayOfWeek day)
        {
            dayOfWeek = day;
            TimeSlots = new List<ReceptionHour> { };
        }

        public void AddTimeSlot(string start, string end)
        {
            TimeSlots.Add(new ReceptionHour(start, end));
            
        }

        public override string ToString() => $@"{dayOfWeek} ";
    }
}
