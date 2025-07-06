
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
        static string receptionFolder = Path.Combine(projectRoot, "ReceptionDays");
        static string notificationsFolder = Path.Combine(projectRoot, "notifications.json");

        static FileHelper()
        {
            if (!Directory.Exists(notificationsFolder))
            {
                Directory.CreateDirectory(receptionFolder);
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

        // write reception daysto file
        public static void WriteReceptionDaysToFile(List<ReceptionDay> receptionDays, string doctorEmail)
        {
            Console.WriteLine("WriteReceptionDaysToFile çağırıldı");

            string safeEmail = doctorEmail.Replace("@", "_at_").Replace(".", "_dot_");

            string filePathReceptionDays = Path.Combine(projectRoot, $"receptionDays_{safeEmail}.json");
            Console.WriteLine("Write fayl yolu: " + filePathReceptionDays); // buraya yapışdır

            Console.WriteLine("Write fayl yolu: " + filePathReceptionDays);


            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(receptionDays, options);
            File.WriteAllText(filePathReceptionDays, json);
        }

        // read reception days from file
        public static List<ReceptionDay> ReadReceptionDaysFromFile(string doctorEmail)
        {

            string safeEmail = doctorEmail.Replace("@", "_at_").Replace(".", "_dot_");
            string filePathReceptionDays = Path.Combine(projectRoot, $"receptionDays_{safeEmail}.json");
            Console.WriteLine("Read fayl yolu: " + filePathReceptionDays); // buraya yapışdır

            if (!File.Exists(filePathReceptionDays))
                return new List<ReceptionDay>();

            string json = File.ReadAllText(filePathReceptionDays);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<ReceptionDay> receptionDays = JsonSerializer.Deserialize<List<ReceptionDay>>(json, options)!;
            return receptionDays ?? new List<ReceptionDay>();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // write reception hours to file
        public static void WriteReceptionHoursToFile(List<ReceptionHour> receptionHours)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(receptionHours, options);
            File.WriteAllText(filePathReceptionHours, json);
        }
        // read reception hours from file

        public static List<ReceptionHour> ReadReceptionHoursFromFile()
        {
            if (!File.Exists(filePathReceptionHours))
            {
                return new List<ReceptionHour>();
            }
            string json = File.ReadAllText(filePathReceptionHours);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<ReceptionHour> receptionHours = JsonSerializer.Deserialize<List<ReceptionHour>>(json, options)!;
            return receptionHours ?? new List<ReceptionHour>();
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
    }
}
