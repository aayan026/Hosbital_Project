
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class Department
    {
        public string departmentName { get; set; }

        public List<Doctor> doctors { get; set; }

        public Department(string departmentName)
        {
            this.departmentName = departmentName;
            doctors = new List<Doctor> { };
        }
        public Department()
        {
        }
        public override string ToString() => $@"{departmentName}";
    }
}
