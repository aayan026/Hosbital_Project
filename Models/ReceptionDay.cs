using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class ReceptionDay
    {
        public DayOfWeek dayOfWeek { get; set; }
        public List<ReceptionHour> TimeSlots { get; set; }

        public ReceptionDay(DayOfWeek day)
        {
            dayOfWeek = day;
            if (TimeSlots == null || TimeSlots.Count == 0)
            {
                TimeSlots = new List<ReceptionHour>
            {
                new ReceptionHour( "09:00","11:00"),
                new ReceptionHour( "12:00","14:00"),
                new ReceptionHour( "15:00","17:00")
            };
            }
        }
        public void AddTimeSlot(string start, string end)
        {
            bool exist = TimeSlots.Any(slot => slot.start.ToString(@"hh\\:mm") == start && slot.end.ToString(@"hh\\:mm") == end);
            if (exist)
            {
                Console.WriteLine(" this time slot already exist");
            }
            else 
                TimeSlots.Add(new ReceptionHour(start, end));
            
        }

        public override string ToString() => $@"{dayOfWeek} ";
    }
}
