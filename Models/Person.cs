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
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string regionCode { get; set; }
        private string phone1;
        public string phoneNumber
        {
            get { return phone1; }
            set
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();

                try
                {
                    var number = phoneUtil.Parse(value, regionCode);

                    if (phoneUtil.IsValidNumber(number))
                    {
                        phone1 = phoneUtil.Format(number, PhoneNumberFormat.E164);
                    }
                    else
                    {
                        phone1 = null;
                    }
                }
                catch (NumberParseException)
                {
                    phone1 = null;
                }
            }
        }
        public Person() { }
        public Person(string name, string surname, string password, string email, string phoneNumber, string regionCode)
        {
            this.name = name;
            this.surname = surname;
            this.email = email;
            this.regionCode = regionCode;
            this.phoneNumber = phoneNumber;
            this.password = password;
        }
    }
}