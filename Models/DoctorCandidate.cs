
using Hospital_Project.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hospital_Project.Models
{
    internal class DoctorCandidate : Person
    {
        public int experienceYear { get; set; }

        [JsonIgnore]
        public Department department { get; set; }

        public string DepartmentName
        {
            get => department?.departmentName ?? _departmentNameFromJson;
            set => _departmentNameFromJson = value;
        }

        [JsonIgnore]
        public string _departmentNameFromJson;

        public enum ApplicationStatus
        {
            Pending,
            Accepted,
            Rejected
        }

        public ApplicationStatus status { get; set; }
        public string reason { get; set; }


        public DoctorCandidate()
        {
        }
        public DoctorCandidate(Hospital hosbital, string name, string surname, string email, string password, string phoneNumber, int experienceYear, Department department, string reason, string regionCode) : base(name, surname, password, email, phoneNumber, regionCode)
        {
            this.experienceYear = experienceYear;
            this.department = department;
            this.status = ApplicationStatus.Pending;
            this.reason = reason;
        }


        public override string ToString() => $@"Name: {name} {surname}
 | {experienceYear} years in {department?.departmentName ?? "Unknown department"}
 | Status: {status}


";
    }
}
