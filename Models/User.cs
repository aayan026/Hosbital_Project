
using Hospital_Project.Base;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Project.Models
{
    internal class User : Person, IProfile, IViewAppointmets, INotification
    {
        public string username { get; set; }
        public List<Appointment> Appointments { get; set; } = new();
        public List<Notification> userNotifications { get; set; }
        public User(string username, string password, string name, string surname, string email, string phoneNumber, string regionCode) : base(name, surname, password, email, phoneNumber, regionCode)
        {
            this.username = username;
            userNotifications = new List<Notification> { };
        }

        public User() { }
        //methods
        public void ViewProfile(string title)
        {
            Console.WriteLine($"\t\t\t\t\t~ {title} ~\n");
            Console.WriteLine($" Username: {username}");
            Console.WriteLine($" Name: {name}");
            Console.WriteLine($" Surname: {surname}");
            Console.WriteLine($" Email: {email}");
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
                Console.WriteLine($"Doctor: {appointment.DoctorName}");
                Console.WriteLine($"Department: {appointment.Department}");
                Console.WriteLine($"Date: {appointment.Day}");
                Console.WriteLine($"Time: {appointment.Hour}");
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public override string ToString() => $@"
Name: {name}
Surname: {surname}
Email: {email}
Phone number: {phoneNumber}";

        public void ViewNotifications()
        {
            Console.WriteLine("\n\t\t\t\t\t~ Notifications ~");
            foreach (var notification in userNotifications)
            {
                Console.WriteLine($"\n{notification}");
                Console.WriteLine("______________________________________________________________________________");

            }
        }
    }
}