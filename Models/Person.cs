using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class Person
    {
        private string _email;
        private string username;
        private string phoneNumber;
        public string regionCode;
        public string name { get; set; }
        public string surname { get; set; }
        public string Username
        {
            get { return username; }
            set
            {
                if (value.Length < 6)
                {
                    Console.WriteLine(" ~ username cannot be less than 6");
                }
                else
                {
                    username = value;
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (value.EndsWith("@gmail.com"))
                {
                    _email = value;
                }
                else
                {
                    Console.WriteLine(" ~ Email is wrong");
                }
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();

                try
                {
                    var number = phoneUtil.Parse(value, regionCode);

                    if (phoneUtil.IsValidNumber(number))
                    {
                        phoneNumber = phoneUtil.Format(number, PhoneNumberFormat.E164);
                    }
                    else
                    {
                        phoneNumber = null;
                    }
                }
                catch (NumberParseException)
                {
                    phoneNumber = null;
                }
            }
        }
        public Person(string name, string surname, string email, string phoneNumber)
        {
            this.name = name;
            this.surname = surname;
            Email = email;
            this.phoneNumber = phoneNumber;
        }
    }
}
