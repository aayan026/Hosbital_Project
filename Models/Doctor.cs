
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class Doctor : Person, IProfile, IViewAppointmets, INotification
    {
        public int workExperienceYear { get; set; }

        [JsonIgnore]
        public Department department { get; set; }
        public string DepartmentName
        {
            get => department?.departmentName ?? _departmentNameFromJson;
            set => _departmentNameFromJson = value;
        }

        [JsonIgnore]
        public string _departmentNameFromJson;
        public List<ReceptionDay> receptionDays { get; set; }

        public ReceptionScheduleManager receptionSchedule { get; set; }

        [JsonIgnore]
        public List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)> Appointments { get; set; } = new();

        [JsonIgnore]
        public List<Notification> doctorsNotifications { get; set; }

        public Doctor() { }
        public Doctor(string name, string surname, string email, string password, string phoneNumber, int workExperienceYear, Department department, string regionCode) : base(name, surname, password, email, phoneNumber, regionCode)
        {
            this.workExperienceYear = workExperienceYear;
            this.department = department;
            receptionDays = new List<ReceptionDay> { };
            receptionSchedule = new ReceptionScheduleManager { doctor = this };
            doctorsNotifications = new List<Notification> { };
            department.doctors.Add(this);
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

        public void ViewAppointments()
        {
            if (Appointments.Count == 0)
            {
                Console.WriteLine("You have no appointments.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\t\t\t\t\t~ Appointments ~\n");
            foreach (var appointment in Appointments)
            {
                Console.WriteLine($" User: {appointment.user.name} {appointment.user.surname}");
                Console.WriteLine($" Date: {appointment.receptionDay}");
                Console.WriteLine($" Time: {appointment.receptionHour.ToString(true)}");
                Console.WriteLine("---------------------------------------------------------------");
            }
        }

        public override string ToString() => $@"Name: {name}
 | Surname: {surname}
 | Experience Year: {workExperienceYear} years
---------------------------------------------------------------";

        public void ViewNotifications()
        {
            Console.WriteLine("\n\t\t\t\t\t~ Notifications ~");
            foreach (var notification in doctorsNotifications)
            {
                Console.WriteLine($"\n{notification}");
                Console.WriteLine("_____________________________________________________________________________________");
            }
        }
    }
}
