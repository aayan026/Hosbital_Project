
using Hospital_Project.Base;
using Hospital_Project.FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hospital_Project.Models.DoctorCandidate;

namespace Hospital_Project.Models
{
    internal class Admin : Person
    {
        public Admin() : base("Ayan", "Aliyeva", "admin123", "admin@gmail.com", "+994501112233", "AZ")
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


        public override string ToString() => $@"
Name: {name}
Surname: {surname}
Email: {email}
Phone number: {phoneNumber}";
    }
}
