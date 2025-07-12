
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital_Project.Models
{
    internal class Appointment
    {
        public string UserName { get; set; }
        public string DoctorName { get; set; }
        public string UserEmail { get; set; }
        public string DoctorEmail { get; set; }
        public string Department { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }

        public Appointment(string userName, string doctorName, string userEmail, string doctorEmail, string department, string day, string hour)
        {
            UserName = userName;
            DoctorName = doctorName;
            UserEmail = userEmail;
            DoctorEmail = doctorEmail;
            Department = department;
            Day = day;
            Hour = hour;
        }



    }
}
