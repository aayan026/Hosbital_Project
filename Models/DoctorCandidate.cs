using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class DoctorCandidate : Person
    {
        public int experienceYear { get; set; }
        public Department department { get; set; }
        public enum ApplicationStatus
        {
            Pending,
            Accepted,
            Rejected
        }
        public ApplicationStatus status { get; set; }
        public string reason { get; set; }


        public DoctorCandidate(Hosbital hosbital, string name, string surname, string email, string password, string phoneNumber, int experienceYear, Department department, string reason,string regionCode) : base(name, surname, password, email, phoneNumber,regionCode)
        {
            this.experienceYear = experienceYear;
            this.department = department;
            var status = ApplicationStatus.Pending;
            this.reason = reason;
        }
    

        public override string ToString() => $@"Name: {name} {surname}
 | {experienceYear} years in {department.departmentName}
 | Status: {status}


";
    }
}
