using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class DoctorCandidate : Person
    {
        public int experienceYear { get; set; }
        public Department department { get; set; }


        public DoctorCandidate(Hosbital hosbital,string name, string surname, string email, string password, string phoneNumber, int experienceYear, Department department) : base(name, surname, password, email, phoneNumber)
        {
            this.experienceYear = experienceYear;
            this.department = department;
            hosbital.doctorCandidates.Add(this);
        }
        public void ViewProfile()
        {
            Console.WriteLine("\t\t\t\t\t~ Profile ~\n");
            Console.WriteLine($" Name: {name}");
            Console.WriteLine($" Surname: {surname}");
            Console.WriteLine($" Email: {email}");
            Console.WriteLine($" Phone number: {phoneNumber}");
            Console.WriteLine($" Experience Year: {experienceYear} years");
            Console.WriteLine($" Department: {department.departmentName}");
            Console.WriteLine("");
        }
        public override string ToString() => $@"
Name: {name} Surname: {surname}
Experience Year: {experienceYear} years Department: {department.departmentName}
";
    }
}
