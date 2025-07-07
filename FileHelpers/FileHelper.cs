
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;


namespace Hosbital_Project.FileHelpers
{
    internal static class FileHelper
    {
        static string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

        static string filePath = Path.Combine(projectRoot, "users.json");

        static string filePathDoctor = Path.Combine(projectRoot, "doctors.json");

        static string filePathCandidate = Path.Combine(projectRoot, "candidates.json");

        static string filePathDepartment = Path.Combine(projectRoot, "departments.json");

        static string filePathReceptionHours = Path.Combine(projectRoot, "receptionHours.json");

        static string filePathNotifications = Path.Combine(projectRoot, $"notifications.json");

        static string filePathAppointments = Path.Combine(projectRoot, "appointments.json");


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //write user to file
        public static void WriteUsersToFile(List<User> users)
        {

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(users, options);
            File.WriteAllText(filePath, json);
        }

        // read users from file

        public static List<User> ReadUsersFromFile()
        {
            if (!File.Exists(filePath))
            {
                return new List<User>();
            }
            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<User> users = JsonSerializer.Deserialize<List<User>>(json, options)!;
            return users ?? new List<User>();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // write doctor to file

        public static void WriteDoctorsToFile(List<Doctor> doctors)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(doctors, options);
            File.WriteAllText(filePathDoctor, json);
        }
        // read doctors from file
        public static List<Doctor> ReadDoctorsFromFile()
        {
            if (!File.Exists(filePathDoctor))
            {
                return new List<Doctor>();
            }
            string json = File.ReadAllText(filePathDoctor);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Doctor> doctors = JsonSerializer.Deserialize<List<Doctor>>(json, options)!;
            return doctors ?? new List<Doctor>();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // candidate write to file
        public static void WriteCandidateToFile(List<DoctorCandidate> candidates)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(candidates, options);
            File.WriteAllText(filePathCandidate, json);
        }

        // read candidate from file
        public static List<DoctorCandidate> ReadCandidatesFromFile()
        {
            if (!File.Exists(filePathCandidate))
            {
                return new List<DoctorCandidate>();
            }
            string json = File.ReadAllText(filePathCandidate);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<DoctorCandidate> candidates = JsonSerializer.Deserialize<List<DoctorCandidate>>(json, options)!;
            return candidates ?? new List<DoctorCandidate>();
        }



        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // departments write to file
        public static void WriteDepartmentsToFile(List<Department> departments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(departments, options);
            File.WriteAllText(filePathDepartment, json);
        }
        // read departments from file
        public static List<Department> ReadDepartmentsFromFile()
        {
            if (!File.Exists(filePathDepartment))
            {
                return new List<Department>();
            }
            string json = File.ReadAllText(filePathDepartment);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Department> departments = JsonSerializer.Deserialize<List<Department>>(json, options)!;
            return departments ?? new List<Department>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // write reception days to file
        public static void WriteReceptionDaysToFile(List<ReceptionDay> newDays, string email)
        {
            string path = Path.Combine(projectRoot, "receptionDays.json");

            List<ReceptionDay> allReceptionDays = new();
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                allReceptionDays = JsonSerializer.Deserialize<List<ReceptionDay>>(json)
                                   ?? new List<ReceptionDay>();
            }

            allReceptionDays = allReceptionDays.Where(d => d.doctorEmail != email).ToList();

            allReceptionDays.AddRange(newDays);

            string updatedJson = JsonSerializer.Serialize(allReceptionDays, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, updatedJson);
        }

        // read reception days from file
        public static List<ReceptionDay> ReadReceptionDaysFromFile(string email)
        {
            string path = Path.Combine(projectRoot, "receptionDays.json");

            if (!File.Exists(path))
                return new List<ReceptionDay>();

            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var allReceptionDays = JsonSerializer.Deserialize<List<ReceptionDay>>(json, options)
                                   ?? new List<ReceptionDay>();

            return allReceptionDays.Where(r => r.doctorEmail == email).ToList();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // write notification to file
        public static void WriteNotificationsToFile(List<Notification> notifications, string email)
        {

            List<Notification> allNotifications = new();
            if (File.Exists(filePathNotifications))
            {
                string json = File.ReadAllText(filePathNotifications);
                allNotifications = JsonSerializer.Deserialize<List<Notification>>(json)
                                   ?? new List<Notification>();
            }

            allNotifications = allNotifications.Where(n => n.toEmail != email).ToList();

            allNotifications.AddRange(notifications);

            string updatedJson = JsonSerializer.Serialize(allNotifications, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePathNotifications, updatedJson);
        }

        public static List<Notification> ReadNotificationsFromFile(string email)
        {
            if (!File.Exists(filePathNotifications))
                return new List<Notification>();

            string json = File.ReadAllText(filePathNotifications);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var allNotifications = JsonSerializer.Deserialize<List<Notification>>(json, options)
                                   ?? new List<Notification>();

            return allNotifications.Where(n => n.toEmail == email).ToList();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public static void WriteAppointmentsToFile(List<Appointment> appointments)
        {
            List<Appointment> allAppointments = new();

            if (File.Exists(filePathAppointments))
            {
                string json = File.ReadAllText(filePathAppointments);
                allAppointments = JsonSerializer.Deserialize<List<Appointment>>(json)
                                   ?? new List<Appointment>();
            }

            foreach (var app in appointments)
            {
                allAppointments = allAppointments
                    .Where(existing =>
                        existing.UserEmail != app.UserEmail ||
                        existing.DoctorEmail != app.DoctorEmail ||
                        existing.Day != app.Day ||
                        existing.Hour != app.Hour
                    )
                    .ToList();
            }

            allAppointments.AddRange(appointments);

            string updatedJson = JsonSerializer.Serialize(allAppointments, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePathAppointments, updatedJson);
        }
        public static void RemoveAppointmentFromFile(Appointment appointmentToRemove)
        {
            List<Appointment> allAppointments = new();

            if (File.Exists(filePathAppointments))
            {
                string json = File.ReadAllText(filePathAppointments);
                allAppointments = JsonSerializer.Deserialize<List<Appointment>>(json)
                                 ?? new List<Appointment>();
            }

            allAppointments = allAppointments
                .Where(app => !(
                    app.UserEmail == appointmentToRemove.UserEmail &&
                    app.DoctorEmail == appointmentToRemove.DoctorEmail &&
                    app.Day == appointmentToRemove.Day &&
                    app.Hour == appointmentToRemove.Hour
                ))
                .ToList();

            string updatedJson = JsonSerializer.Serialize(allAppointments, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePathAppointments, updatedJson);
        }

        public static List<Appointment> ReadAppointmentsForUserOrDoctor(string email, string role)
        {
            if (!File.Exists(filePathAppointments))
                return new List<Appointment>();

            string json = File.ReadAllText(filePathAppointments);
            var allAppointments = JsonSerializer.Deserialize<List<Appointment>>(json)
                                   ?? new List<Appointment>();

            return role.ToLower() switch
            {
                "user" => allAppointments.Where(a => a.UserEmail == email).ToList(),
                "doctor" => allAppointments.Where(a => a.DoctorEmail == email).ToList(),
                _ => new List<Appointment>()
            };
        }
    }
}
