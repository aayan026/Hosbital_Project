
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_homework.Models
{
    internal class Doctor : Person
    {
        public int workExperienceYear { get; set; }
        public Department department { get; set; }
        public List<ReceptionDay> receptionDays { get; set; }

        public Doctor(string name, string surname, string email, string phoneNumber, int workExperienceYear, Department department) : base( name, surname, email, phoneNumber)
        {
            this.workExperienceYear = workExperienceYear;
            this.department = department;
            receptionDays = new List<ReceptionDay> { };

            department.doctors.Add(this);
        }
        public void AddReceptionDay(ReceptionDay day)
        {
            receptionDays.Add(day);
        }
        public void ReserveHour(int Dayindex, int slotIndex)
        {
            var reserved = receptionDays[Dayindex].TimeSlots[slotIndex];
            reserved.isReserved = true;
        }

        public override string ToString() => $@"Name: {name}
 | Surname: {surname}
 | Experience Year: {workExperienceYear} years
---------------------------------------------------------------";
    }
}
