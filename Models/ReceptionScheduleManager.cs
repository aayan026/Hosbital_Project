using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class ReceptionScheduleManager
    {
        public void AddReceptionDay(Doctor doctor,DayOfWeek day)
        {
            doctor.receptionDays.Add(new ReceptionDay(day));
        }
        public void ReserveHour(Doctor doctor,int Dayindex, int slotIndex)
        {
            var reserved = doctor.receptionDays[Dayindex].TimeSlots[slotIndex];
            reserved.isReserved = true;
        }
        public void CancelHour(Doctor doctor,int Dayindex, int slotIndex)
        {
            var reserved = doctor.receptionDays[Dayindex].TimeSlots[slotIndex];
            reserved.isReserved = false;
        }
    }
}