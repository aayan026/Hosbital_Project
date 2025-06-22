
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_homework.Models
{
    internal class Authentication
    {
        public List<User> users { get; set; }

        public Authentication(List<User> users)
        {
            this.users = users;
        }
        public User? Registration(string username, string name, string surname, string email, string phone, string regionCode)
        {
            phone = phone.Replace(" ", "").Replace("-", "");

            username = username.Trim();
            email = email.Trim();
            name = name.Trim();
            surname = surname.Trim();
            if (!email.EndsWith("@gmail.com"))
            {
                return null;
            }
            if (username.Length < 6)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
                return null;

            var phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var number = phoneUtil.Parse(phone, regionCode);
                if (!phoneUtil.IsValidNumber(number))
                    return null;

                phone = phoneUtil.Format(number, PhoneNumberFormat.E164);

            }
            catch (NumberParseException)
            {
                return null;
            }

            return new User(username, name, surname, email, phone);
        }
        public User SignInUser(string username)
        {
            foreach (var item in users)
            {
                if (item.Username == username)
                {
                    return item;
                }
            }
            return null;
        }
        public void AdminSignIn(string username,string password,string email) { 
        }

    }

}
