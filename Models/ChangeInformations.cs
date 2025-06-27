using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    public class ChangeInformations
    {
        internal static void ChangePhone(dynamic person, Hosbital hosbital)
        {

            List<string> regionCodes = new List<string> { "AZ", "US", "TR", "RU" };
            while (true)
            {
                int choiceIndex = Program.NavigateMenu(regionCodes, "\n ~ Select your region code: ");
                if (choiceIndex >= 0 && choiceIndex < regionCodes.Count)
                {
                    person.regionCode = regionCodes[choiceIndex];
                    Console.Write("  Enter your new phone number: ");
                    string rawPhone = Console.ReadLine();
                    bool find = hosbital.SearchPhone(rawPhone);
                    if (person.phoneNumber == rawPhone)
                    {
                        Console.WriteLine(" New phone number cannot be the same as old phone number");
                        Console.ReadKey();
                        continue;
                    }
                    else if (find)
                    {
                        Console.WriteLine(" This number belongs to an existing user"); Console.ReadKey();
                    }

                    var phoneUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                    try
                    {
                        var parsedNumber = phoneUtil.Parse(rawPhone, regionCodes[choiceIndex]);
                        if (!phoneUtil.IsValidNumber(parsedNumber))
                        {
                            Console.WriteLine(" Invalid phone number.");
                            Console.ReadKey();
                            continue;
                        }
                        string formattedPhone = phoneUtil.Format(parsedNumber, PhoneNumbers.PhoneNumberFormat.E164);
                        person.phoneNumber = formattedPhone;
                        //user faylina yaz
                        Console.WriteLine("Your phone number has been updated successfully.");
                        Console.ReadKey();
                        break;
                    }
                    catch (PhoneNumbers.NumberParseException)
                    {
                        Console.WriteLine(" Phone number format is invalid.");
                        Console.ReadKey();
                        continue;
                    }
                }
            }
        }
        internal static void ChangeEmail(dynamic person, Hosbital hosbital)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("\n  Enter current email: ");
                string oldEmail = Console.ReadLine();
                Console.Write("  Enter your new email: ");
                string email = Console.ReadLine();
                bool find4 = hosbital.SearchEmail(email);
                if (email == person.email)
                {
                    Console.WriteLine(" New email cannot be the same as old email");
                    Console.ReadKey();
                    continue;
                }
                if (string.IsNullOrWhiteSpace(email) || !email.EndsWith("@gmail.com") && !email.EndsWith("@yahoo.com") && !email.EndsWith("@outlook.com") && !email.EndsWith("@hotmail.com") && !email.EndsWith("@mail.ru") && !email.EndsWith("@icloud.com"))
                {
                    Console.WriteLine(" ~ Email is wrong.");
                    Console.ReadKey();
                    continue;
                }
                string first = email.Split('@').First();
                string firstPartPattern = @"^[a-zA-Z0-9._-]+$";
                if (!Regex.IsMatch(first, firstPartPattern))
                {
                    Console.WriteLine(" ~ Email cannot be changed");
                    Console.ReadKey();
                    continue;
                }
                if (find4)
                {
                    Console.WriteLine(" ~ An account with this email already exists ");
                    Console.ReadKey();
                    continue;
                }
                var newEmail = person.email = email;
                //user faylina yaz
                Console.WriteLine(" ~ Your email has been updated successfully.");
                Console.ReadKey();
                break;
            }
        }
        public static void ChangePassword(dynamic person)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("  Enter your current password: ");
                string oldPassword = Console.ReadLine();
                if (oldPassword != person.password)
                {
                    Console.WriteLine(" ~ Current password is incorrect.");
                    Console.ReadKey();
                    continue;
                }
                Console.Write("  Enter your new password: ");
                string newPassword = Console.ReadLine();
                if (newPassword.Length < 6)
                {
                    Console.WriteLine(" ~ Password must be at least 6 characters long.");
                    Console.ReadKey();
                    continue;
                }
                person.password = newPassword;
                //user faylina yaz
                Console.WriteLine(" ~ Your password has been updated successfully.");
                Console.ReadKey();
                break;
            }
        }

    }
}


