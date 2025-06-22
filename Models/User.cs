
using Hosbital_Project.Models;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_homework.Models
{
    internal class User:Person
    {
        public string username { get; set; }
        public User(string username, string name, string surname, string email, string phoneNumber) : base( name, surname, email, phoneNumber)
        {
            this.username = username;
        }

        //methods
        public void ViewProfile()
        {
            Console.WriteLine("\t\t\t\t\t~ Profile ~\n");
            Console.WriteLine($" Username: {Username}");
            Console.WriteLine($" Name: {name}");
            Console.WriteLine($" Surname: {surname}");
            Console.WriteLine($" Email: {Email}");
            Console.WriteLine($" Phone number: {phoneNumber}");
            Console.WriteLine("");
        }
        public void ViewAppointments()
        {
            if (Appointments.Count == 0)
            {
                Console.WriteLine("You have no appointments.");
                return;
            }
            Console.WriteLine("\t\t\t\t\t~ Appointments ~\n");
            foreach (var appointment in Appointments)
            {
                Console.WriteLine($"Doctor: {appointment.doctor.name} {appointment.doctor.surname}");
                Console.WriteLine($"Department: {appointment.doctor.department.departmentName}");
                Console.WriteLine($"Date: {appointment.receptionDay}");
                Console.WriteLine($"Time: {appointment.receptionHour}");
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public override string ToString() => $@"
Name: {name}
Surname: {surname}
Email: {Email}
Phone number: {phoneNumber}";

    }
}
