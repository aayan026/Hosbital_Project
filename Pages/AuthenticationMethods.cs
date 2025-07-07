using Hosbital_Project.Models;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Hosbital_Project.Pages
{
    internal static class AuthenticationMethods
    {
        public static List<User> users = FileHelpers.FileHelper.ReadUsersFromFile();
        public static User RegistrUser(Authentication auth, List<Department> departments, Hosbital hosbital)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t\t\t\t\t~ Registration Page ~\n");
                while (true)
                {
                    Console.Write(" Name: ");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        Console.WriteLine(" ~ Name cannot be empty");
                        continue;
                    }
                    else
                    {
                        while (true)
                        {
                            Console.Write(" Surname: ");
                            string surname = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(surname))
                            {
                                Console.WriteLine(" ~ Surname cannot be empty");
                                continue;
                            }
                            else
                            {
                                while (true)
                                {
                                    Console.Write(" Email: ");
                                    string email = Console.ReadLine();
                                    var find2 = hosbital.SearchEmail(email);

                                    string first = email.Split('@').First();
                                    string firstPartPattern = @"^[a-zA-Z0-9_-]+$";
                                    if (string.IsNullOrWhiteSpace(email))
                                    {
                                        Console.WriteLine(" ~ email cannot be empty");
                                        continue;
                                    }
                                    else if (!Regex.IsMatch(first, firstPartPattern))
                                    {
                                        Console.WriteLine(" Email cannot contain special characters in the first part.");
                                        continue;
                                    }
                                    else if (find2)
                                    {
                                        Console.WriteLine(" An account with this email already exists ");
                                        continue;
                                    }
                                    else if (!email.EndsWith("@gmail.com") && !email.EndsWith("@yahoo.com") && !email.EndsWith("@outlook.com") && !email.EndsWith("@hotmail.com") && !email.EndsWith("@mail.ru") && !email.EndsWith("@icloud.com"))
                                    {
                                        Console.WriteLine(" ! Wrong email..");
                                        continue;
                                    }

                                    else
                                    {
                                        name = name.Trim().ToLower();
                                        surname = surname.Trim().ToLower();
                                        string namePart = new string(name.Take(3).ToArray());
                                        string surnamePart = new string(surname.Where(char.IsLetter).Take(5).ToArray());
                                        Random rnd = new Random();
                                        int number = rnd.Next(10, 99);
                                        string newUsername = $"{namePart}_{surnamePart}{number}";
                                        Console.WriteLine($" create your username: ");
                                        string final = "";
                                        while (true)
                                        {
                                            Console.WriteLine($"Suggested username: {newUsername}");
                                            Console.Write("Do you want to use this username? (y/n): ");
                                            string answer = Console.ReadLine().ToLower();

                                            if (answer == "y")
                                            {
                                                final = newUsername;
                                                break;
                                            }
                                            else if (answer == "n")
                                            {
                                                while (true)
                                                {
                                                    Console.Write("Enter your preferred username: ");
                                                    final = Console.ReadLine();
                                                    bool find3 = hosbital.SearchUser(final);
                                                    if (find3)
                                                    {
                                                        Console.WriteLine("This username is already taken.");
                                                        continue;
                                                    }
                                                    break;
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                Console.WriteLine("Please type 'y' or 'n'.");
                                            }
                                        }
                                        while (true)
                                            {
                                                Console.Write(" Password: ");
                                                string password = Console.ReadLine();
                                                if (string.IsNullOrWhiteSpace(password))
                                                {
                                                    Console.WriteLine("Password cannot be empty.");
                                                    continue;
                                                }
                                                else if (password.Length < 6 || !password.Any(char.IsDigit))
                                                {
                                                    Console.WriteLine("Password must be at least 6 chars and contain digits.");
                                                    continue;
                                                }
                                                else
                                                {
                                                    List<string> regionCodes = new List<string> { "AZ", "US", "TR", "RU" };
                                                    string title = $"\t\t\t\t\t\tHosbital\n Name: {name}\n Surname: {surname}\n Email: {email}\n Username: {final}\n Password: {password}\n Select your country.";
                                                    int choiceIndex = Program.NavigateMenu(regionCodes, title, false);

                                                    if (choiceIndex >= 0 && choiceIndex < regionCodes.Count)
                                                    {
                                                        string regionCode = regionCodes[choiceIndex];
                                                        while (true)
                                                        {
                                                            Console.Write("\n Phone Number: ");
                                                            string phone = Console.ReadLine();

                                                            if (string.IsNullOrWhiteSpace(phone))
                                                            {
                                                                Console.WriteLine(" ~ Phone cannot be empty");
                                                                continue;
                                                            }

                                                            var phoneUtil = PhoneNumberUtil.GetInstance();
                                                            try
                                                            {
                                                                var number2 = phoneUtil.Parse(phone, regionCode);
                                                                if (!phoneUtil.IsValidNumber(number2))
                                                                {
                                                                    Console.WriteLine(" Invalid phone number.");
                                                                    continue;
                                                                }

                                                                string formattedPhone = phoneUtil.Format(number2, PhoneNumberFormat.E164);

                                                                bool exists = hosbital.SearchPhone(formattedPhone);
                                                                if (exists)
                                                                {
                                                                    Console.WriteLine(" This phone number belongs to an existing user");
                                                                    continue;
                                                                }
                                                                else
                                                                {
                                                                    var newUser = new User(final, password, name, surname, email, formattedPhone, regionCode);
                                                                    users.Add(newUser);
                                                                    FileHelpers.FileHelper.WriteUsersToFile(users);
                                                                    Console.WriteLine("\n ~ You have successfully registered..");
                                                                    Console.ReadKey();
                                                                    return newUser;
                                                                }
                                                            }
                                                            catch (NumberParseException)
                                                            {
                                                                Console.WriteLine(" Phone number format is invalid.");
                                                                continue;
                                                            }
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    break;
                                }
                            }
                            break;
                        }
                        break;
                    }
                }

            }


        public static Doctor DoctorSignIn(Hosbital hosbital, Authentication auth)
        {
            while (true)
            {
                Console.Write("\n | Enter email: ");
                string email = Console.ReadLine();
                Console.Write(" | Enter password: ");
                string password = Console.ReadLine();
                Doctor doctor = auth.DoctorSignIn(hosbital, email, password);
                if (doctor != null)
                {
                    Console.WriteLine(" ~ Successfully signed in as doctor!");
                    Console.ReadKey();
                    return doctor;
                    break;
                }
                else
                {
                    Console.WriteLine(" ! Invalid doctor credentials. Please try again.");
                    Console.ReadKey();
                    return null;
                }
            }
        }
        public static User SignInUser(Authentication auth)
        {
            Console.Clear();
            Console.WriteLine("\t\t\t\t~ Sign In Page ~\n");
            while (true)
            {
                Console.Write(" | Enter your username: ");
                string username = Console.ReadLine().Trim();
                Console.Write(" | Enter your password: ");
                string password = Console.ReadLine().Trim();
                User user = auth.SignInUser(username, password);
                return user;
            }

        }
    }
}