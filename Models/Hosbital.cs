
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class Hosbital
    {
        public List<Department> departments { get; set; }
        public List<Doctor> doctors { get; set; }
        public List<User> Users { get; set; }
        public List<DoctorCandidate> doctorCandidates { get; set; } = new List<DoctorCandidate>();
        public Hosbital(List<Department> departments, List<Doctor> doctors, List<User> users)
        {
            this.departments = departments;
            this.doctors = doctors;
            this.Users = users;
        }

        public bool SearchUser(string username) //tapildisa true
        {
            //fayldan oxu
            foreach (var item in Users)
            {
                if (item.username == username)
                {
                    return true;
                }
            }
            return false;
        }
        public bool SearchDepartment(string departmentName)
        {
            //fayldan oxu
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
            //fayldan oxu
            foreach (var item in doctorCandidates)
            {
                if (item.email == email)
                {
                    return item;
                }
            }
            return null;
        }
        public void ShowDoctorCandidate(DoctorCandidate candidate)
        {
            Console.WriteLine($" Name: {candidate.name} {candidate.surname}");
            Console.WriteLine($" Application status {candidate.status}");

        }
        public bool SearchPhone(string phone)
        {
            //fayldan oxu
            foreach (var item in Users)
            {
                if (item.phoneNumber == phone)
                {
                    return true;
                }
            }
            return false;
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
