
using Hospital_Project.FileHelpers;
using PhoneNumbers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Hospital_Project.Models.DoctorCandidate;

namespace Hospital_Project.Models
{
    internal class Hospital
    {
        public List<Department> departments { get; set; }
        public List<Doctor> doctors { get; set; }
        public List<User> Users { get; set; }
        public List<DoctorCandidate> doctorCandidates { get; set; }
        public Hospital(List<Department> departments, List<Doctor> doctors, List<User> users, List<DoctorCandidate> doctorCandidates)
        {
            this.departments = departments;
            this.doctors = doctors;
            this.Users = users;
            this.doctorCandidates = doctorCandidates;

        }
        public Hospital()
        {
        }
        static public void LinkDepartmentsToCandidates(List<DoctorCandidate> candidates, List<Department> departments)
        {
            foreach (var c in candidates)
            {
                c.department = departments.FirstOrDefault(d => d.departmentName == c.DepartmentName || d.departmentName == c._departmentNameFromJson);
            }
        }
        public static void LinkDepartmentsToDoctors(List<Doctor> doctors, List<Department> departments)
        {
            foreach (var doc in doctors)
            {
                doc.department = departments.FirstOrDefault(d => d.departmentName == doc.DepartmentName || d.departmentName == doc._departmentNameFromJson);
            }
        }
        public static void LinkDoctorsToDepartments(List<Doctor> doctors, List<Department> departments)
        {
            foreach (var dept in departments)
            {
                dept.doctors = doctors
                    .Where(d => d.department != null && d.department.departmentName == dept.departmentName)
                    .ToList();
            }
        }

        public void ProfileInfo(string title, Doctor doctor)
        {
            Console.WriteLine($"\t\t\t\t\t{title}\n");
            Console.WriteLine($" Name: {doctor.name}");
            Console.WriteLine($" Surname: {doctor.surname}");
            Console.WriteLine($" Phone number: {doctor.phoneNumber}");
            Console.WriteLine($" Email: {doctor.email}");
            Console.WriteLine($" Password: {doctor.password}");
            Console.WriteLine($" Work Experience Year: {doctor.workExperienceYear} years");
            Console.WriteLine($" Department: {doctor.department.departmentName}");
            Console.WriteLine("");
        }

        public bool SearchUser(string username) //tapildisa true
        {
            foreach (var item in Users)
            {
                if (item.username == username)
                {
                    return true;
                }
            }
            return false;
        }
        public void ViewUsers()
        {
            if (Users.Count == 0)
            {
                Console.WriteLine("No users found.");
                return;
            }
            Console.WriteLine("\t\t\t\t\t~ Users ~\n");
            foreach (var user in Users)
            {
                Console.WriteLine($" Username: {user.username}");
                Console.WriteLine($" Name: {user.name}");
                Console.WriteLine($" Surname: {user.surname}");
                Console.WriteLine($" Email: {user.email}");
                Console.WriteLine($" Phone number: {user.phoneNumber}");
                Console.WriteLine("------------------------------------------------");
            }
        }
        public void ViewDepartments()
        {
            if (departments.Count == 0)
            {
                Console.WriteLine("No departments found.");
                return;
            }
            Console.WriteLine("\t\t\t\t\t~ Departments ~\n");
            foreach (var department in departments)
            {
                Console.WriteLine($" Department: {department.departmentName}");
                Console.WriteLine("----------------------------------------");
            }
        }

        public void AddDepartment()
        {
            Console.Write("Enter department name: ");
            string departmentName = Console.ReadLine();
            if (SearchDepartment(departmentName))
            {
                Console.WriteLine("This department already exists.");
                Console.ReadKey();
                return;
            }
            else if (string.IsNullOrEmpty(departmentName))
            {
                Console.WriteLine(" Department name cannot be null");
                Console.ReadKey();
            }
            else
            {
                var department = new Department(departmentName);
                departments.Add(department);
                FileHelper.WriteDepartmentsToFile(departments);
                Console.WriteLine(" ~ Department added successfully.");
                Log.Information("admin added a new department {name}", departmentName);
                Console.ReadKey();
            }
        }
        public void RemoveDepartment()
        {
            Console.Write("Enter department name to remove: ");
            string departmentName = Console.ReadLine();
            departmentName = departmentName.Trim();
            var department = departments.FirstOrDefault(d => d.departmentName.Equals(departmentName, StringComparison.OrdinalIgnoreCase));
            if (department != null)
            {
                departments.Remove(department);
                FileHelper.WriteDepartmentsToFile(departments);
                Console.WriteLine(" department deleted succesfully");
                Log.Information("admin deleted department - {name}", departmentName);
            }
            else
            {
                Console.WriteLine(" ~ Department not found");
                Console.ReadKey();
            }
        }
        public void AcceptedDoctor(DoctorCandidate candidate)
        {
            candidate.status = ApplicationStatus.Accepted;
            Doctor accepted = new Doctor
            {
                name = candidate.name,
                surname = candidate.surname,
                email = candidate.email,
                password = candidate.password,
                phoneNumber = candidate.phoneNumber,
                workExperienceYear = candidate.experienceYear,
                department = candidate.department,
            };
            doctors.Add(accepted);
            FileHelper.WriteDoctorsToFile(doctors);

            doctorCandidates.Remove(candidate);
            FileHelper.WriteCandidateToFile(doctorCandidates);
            Console.WriteLine(" The candidate has been successfully accepted");
            Log.Information("admin accepted the doctor.");
            Console.ReadKey();
        }

        public void RejectDoctor(DoctorCandidate candidate)
        {
            doctorCandidates.Remove(candidate);
            FileHelper.WriteCandidateToFile(doctorCandidates);
            Console.WriteLine(" The candidate has been succesfully rejected");
            Log.Information("admin rejected the doctor");
            Console.ReadKey();
        }
        public bool SearchDepartment(string departmentName)
        {
            foreach (var item in departments)
            {
                if (item.departmentName.ToLower() == departmentName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        public void ShowDoctors()
        {
            foreach (var item in doctors)
            {
                Console.WriteLine($" Name:");
            }
        }

        public DoctorCandidate FindCandidate(string email)
        {
            foreach (var item in doctorCandidates)
            {
                if (item.email == email)
                {
                    return item;
                }
            }
            return null!;
        }
        public void ShowDoctorCandidate(DoctorCandidate candidate)
        {
            Console.WriteLine($" Name: {candidate.name} {candidate.surname}");
            Console.WriteLine($" Application status {candidate.status}");

        }
        public bool SearchPhone(string phone)
        {
            foreach (var item in Users)
            {
                if (item.phoneNumber == phone)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> GetAllEmails()
        {
            var emails = new List<string>();

            emails.AddRange(Users.Select(u => u.email));
            emails.AddRange(doctors.Select(d => d.email));
            emails.AddRange(doctorCandidates.Select(c => c.email));

            return emails;
        }
        public bool EmailExists(string email)
        {
            return GetAllEmails().Any(e => e.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        public bool IsEmailUsedByUserOrDoctor(string email)
        {
            return Users.Any(u => u.email.Equals(email, StringComparison.OrdinalIgnoreCase)) ||
                   doctors.Any(d => d.email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
        public bool SearchEmail(string email)
        {
            //fayldan oxu
            foreach (var item in Users)
            {
                if (item.email == email)
                {
                    return true;
                }
            }
            return false;
        }


    }
}