using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class Appointment
    {
        public User user { get; set; }
        public Doctor doctor { get; set; }
        public ReceptionDay receptionDay { get; set; }
        public ReceptionHour receptionHour { get; set; }

           public Appointment(User user, Doctor doctor, ReceptionDay receptionDay, ReceptionHour receptionHour)
            {
                this.user = user;
                this.doctor = doctor;
                this.receptionDay = receptionDay;
                this.receptionHour = receptionHour;
            }
        }
}
