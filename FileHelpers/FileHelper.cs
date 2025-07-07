
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
        static string filePathReceptionDays = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "receptionDays.json");

        static string notificationsFolder = Path.Combine(projectRoot, "notifications.json");

        static string filePathAppointments = Path.Combine(projectRoot, "appointments.json");
        static FileHelper()
        {
            if (!Directory.Exists(notificationsFolder))
            {
                Directory.CreateDirectory(notificationsFolder);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //write user to file
        public static void WriteUsersToFile(List<User> users)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
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

        // read doctors from file
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

        // write reception hours to file
        static string dataFolder = Path.Combine(projectRoot, "ReceptionDays.json");
        public static string SafeFileName(string email)
        {
            return email.Replace("@", "_at_").Replace(".", "_dot_");
        }
        public static string GetReceptionDaysPath(string email) =>
            Path.Combine(dataFolder, $"{SafeFileName(email)}_receptionDays.json");

        public static void WriteReceptionDaysToFile(List<ReceptionDay> receptionDays, string email)
        {
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            string path = GetReceptionDaysPath(email);

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(receptionDays, options);
            File.WriteAllText(path, json);
        }
        // read reception days from file
        public static List<ReceptionDay> ReadReceptionDaysFromFile(string email)
        {
            string path = GetReceptionDaysPath(email);
            if (!File.Exists(path))
                return new List<ReceptionDay>();

            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<ReceptionDay>>(json, options) ?? new List<ReceptionDay>();
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // write notification to file
        public static void WriteNotificationsToFile(List<Notification> notifications, string doctorEmail)
        {
            string safeEmail = doctorEmail.Replace("@", "_at_").Replace(".", "_dot_");
            string filePathNotifications = Path.Combine(notificationsFolder, $"notifications_{safeEmail}.json");
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(notifications, options);
            File.WriteAllText(filePathNotifications, json);
        }

        // read notifications from file
        public static List<Notification> ReadNotificationsFromFile(string doctorEmail)
        {
            string safeEmail = doctorEmail.Replace("@", "_at_").Replace(".", "_dot_");
            string filePathNotifications = Path.Combine(notificationsFolder, $"notifications_{safeEmail}.json");
            if (!File.Exists(filePathNotifications))
            {
                return new List<Notification>();
            }
            string json = File.ReadAllText(filePathNotifications);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<Notification> notifications = JsonSerializer.Deserialize<List<Notification>>(json, options)!;
            return notifications ?? new List<Notification>();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void WriteAppointmentsToFile(List<Appointment> appointments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(appointments, options);
            File.WriteAllText(filePathAppointments, json);
        }

        // read appointments from file
        public static List<Appointment> ReadAppointmentsFromFile()
        {
            if (!File.Exists(filePathAppointments)) return new List<Appointment>();

            string json = File.ReadAllText(filePathAppointments);
            var appointments = JsonSerializer.Deserialize<List<Appointment>>(json);
            return appointments ?? new List<Appointment>();
        }


    }
}
