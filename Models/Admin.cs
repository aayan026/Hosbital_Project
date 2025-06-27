using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hosbital_Project.Models.DoctorCandidate;

namespace Hosbital_Project.Models
{
    internal class Admin : Person
    {
        public Admin() : base("Ayan", "Aliyeva", "admin123", "admin@gmail.com", "+994501112233","AZ")
        {
        }
        public void ViewProfile()
        {
            Console.WriteLine("\t\t\t\t\t~ Profile ~\n");
            Console.WriteLine($" Name: {name}");
            Console.WriteLine($" Surname: {surname}");
            Console.WriteLine($" Email: {email}");
            Console.WriteLine($" Phone number: {phoneNumber}");
            Console.WriteLine("");
        }
        public void ViewUsers(List<User> users)
        {
            if (users.Count == 0)
            {
                Console.WriteLine("No users found.");
                return;
            }
            Console.WriteLine("\t\t\t\t\t~ Users ~\n");
            foreach (var user in users)
            {
                Console.WriteLine($" Username: {user.username}");
                Console.WriteLine($" Name: {user.name}");
                Console.WriteLine($" Surname: {user.surname}");
                Console.WriteLine($" Email: {user.email}");
                Console.WriteLine($" Phone number: {user.phoneNumber}");
                Console.WriteLine("------------------------------------------------");
            }
        }
        public void ViewDepartments(List<Department> departments)
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
        public void AddDepartment(Hosbital hosbital)
        {
            Console.Write("Enter department name: ");
            string departmentName = Console.ReadLine();
            if (hosbital.SearchDepartment(departmentName))
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
                //fayla yaz
                hosbital.departments.Add(department);
                Console.WriteLine(" ~ Department added successfully.");
                Console.ReadKey();
            }
        }

        public void RemoveDepartment(Hosbital hosbital)
        {
            Console.Write("Enter department name to remove: ");
            string departmentName = Console.ReadLine();
            departmentName = departmentName.Trim();
            var department = hosbital.departments.FirstOrDefault(d => d.departmentName.Equals(departmentName, StringComparison.OrdinalIgnoreCase));
            //fayla yaz
            if (department != null)
            {
                hosbital.departments.Remove(department);
                Console.WriteLine(" department deleted succesfully");
            }
            else {
                Console.WriteLine( " ~ Department not found");
                Console.ReadKey();
            }
        }
        public void AddDoctor(Hosbital hosbital, Doctor doctor)
        {
            //fayla yaz
            hosbital.doctors.Add(doctor);
        }
        public void AcceptedDoctor(Hosbital hosbital,DoctorCandidate candidate)
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
                department=candidate.department,
            };
            hosbital.doctors.Add(accepted);
            hosbital.doctorCandidates.Remove(candidate);
            Console.WriteLine(" The candidate has been successfully accepted");
            Console.ReadKey();
        }
        public void RejectDoctor(Hosbital hosbital,DoctorCandidate candidate)
        {
            hosbital.doctorCandidates.Remove(candidate);
            Console.WriteLine(" The candidate has been succesfully rejected");
            Console.ReadKey();
        }

        public override string ToString() => $@"
Name: {name}
Surname: {surname}
Email: {email}
Phone number: {phoneNumber}";
    }
}
