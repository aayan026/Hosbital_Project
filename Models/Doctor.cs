
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class Doctor : Person
    {
        public int workExperienceYear { get; set; }
        public Department department { get; set; }
        public List<ReceptionDay> receptionDays { get; set; }
        public Doctor() { }
        public Doctor(string name, string surname, string email,string password, string phoneNumber, int workExperienceYear, Department department,string regionCode) : base( name, surname,password, email, phoneNumber,regionCode)
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
        public void CancelHour(int Dayindex, int slotIndex)
        {
            var reserved = receptionDays[Dayindex].TimeSlots[slotIndex];
            reserved.isReserved = false;
        }
        public void ViewProfile(string title)
        {
            Console.WriteLine($"\t\t\t\t\t{title}\n");
            Console.WriteLine($" Name: {name}");
            Console.WriteLine($" Surname: {surname}");
            Console.WriteLine($" Email: {email}");
            Console.WriteLine($" Phone number: {phoneNumber}");
            Console.WriteLine($" Work Experience Year: {workExperienceYear} years");
            Console.WriteLine($" Department: {department.departmentName}");
            Console.WriteLine("");
        }


        public override string ToString() => $@"Name: {name}
 | Surname: {surname}
 | Experience Year: {workExperienceYear} years
---------------------------------------------------------------";
    }
}
