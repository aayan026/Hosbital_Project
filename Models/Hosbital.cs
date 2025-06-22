
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_homework.Models
{
    internal class Hosbital
    {
        public List<Department> departments { get; set; }
        public List<User> Users { get; set; }
        public Hosbital(List<Department> departments, List<User> users)
        {
            this.departments = departments;
            this.Users = users;
        }
        public bool SearchUser(string username) //tapildisa true
        {
            //fayldan oxu
            foreach (var item in Users)
            {
               if( item.Username == username)
                {
                    return true;
                }
            }
            return false;
        }
        public bool SearchPhone(string phone)
        {
            //fayldan oxu
            foreach (var item in Users)
            {
                if (item.PhoneNumber == phone)
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
                if (item.Email==email)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
