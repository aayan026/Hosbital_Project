using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hosbital_Project.FileHelpers
{
    internal static class FileHelper
    {
        static string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

        static string filePath = Path.Combine(projectRoot, "users.json");

        static string filePathDoctor = Path.Combine(projectRoot, "doctors.json");
        static string filePathCandidate = Path.Combine(projectRoot, "candidates.json");

        static string filePathDepartment = Path.Combine(projectRoot, "departments.json");

        static string filePathAppointment = Path.Combine(projectRoot, "appointments.json");
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
            return  candidates ?? new List<DoctorCandidate>();
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

        // appointments write to file
        public static void WriteAppointmentsToFile(List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)> appointments)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(appointments, options);
            File.WriteAllText(filePathAppointment, json);
        }

        // read appointments from file

        public static List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)> ReadAppointmentsFromFile()
        {
            if (!File.Exists(filePathAppointment))
            {
                return new List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)>();
            }
            string json = File.ReadAllText(filePathAppointment);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)> appointments = JsonSerializer.Deserialize<List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)>>(json, options)!;
            return appointments ?? new List<(User user, ReceptionDay receptionDay, ReceptionHour receptionHour)>();
        }
    }
}
