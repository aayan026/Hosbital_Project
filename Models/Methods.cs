
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Hosbital_homework.Models
{
    internal static class Methods
    {

        public static User RegistrUser(Authentication auth, List<Department> departments, Hosbital hosbital)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\t\t\t\t\t~ Registration Page ~\n");
                Console.Write(" Name: ");
                string name = Console.ReadLine();
                Console.Write(" Surname: ");
                string surname = Console.ReadLine();
                Console.Write(" Email: ");
                string email = Console.ReadLine();
                var find2 = hosbital.SearchEmail(email);
                if (find2)
                {
                    Console.WriteLine(" An account with this email already exists ");
                    Console.ReadKey();
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
                    Console.WriteLine($"create your username: ");
                    Console.WriteLine($"Suggested: {newUsername}");
                    Console.Write("Do you want to use this username? (y/n): ");
                    string answer = Console.ReadLine().ToLower();
                    string final;
                    if (answer == "y")
                    {
                        final = newUsername;
                    }
                    else
                    {
                        Console.Write("Enter your preferred username: ");
                        final = Console.ReadLine();
                        bool find3=hosbital.SearchUser(final);
                        if (find3)
                        {
                            Console.WriteLine(" this username already exist");
                            Console.ReadKey();
                            continue;
                        }
                    }
                    List<string> regionCodes = new List<string> { "AZ", "US", "TR", "RU" };
                    string title = $"\t\t\t\t\t\tHosbital\n Name: {name}\n Surname: {surname}\n Email: {email}\n Username: {final}\n Select your country."; ;
                    int choiceIndex = Program.NavigateMenu(regionCodes, title, false);
                    {
                        if (choiceIndex >= 0 && choiceIndex < regionCodes.Count)
                        {
                            Console.Write("\n Phone Number: ");
                            string phone = Console.ReadLine();
                            bool find = hosbital.SearchPhone(phone);
                            if (find == true)
                            {
                                Console.WriteLine(" This phone number belongs to an existing user");
                                continue;
                            }
                            else
                            {
                                User? user = auth.Registration(final, name, surname, email, phone, regionCodes[choiceIndex]);
                                //user faylina yaz
                                if (user == null)
                                {
                                    Console.WriteLine("\nInvalid input detected. Please check your information.");
                                    Console.ReadKey();
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine("\n~ Registration completed successfully.");
                                    Console.ReadKey();
                                    return user;
                                }
                            }
                        }
                    }
                }
            }
        }

        public static User SignInUser(Authentication auth)
        {
            //fayldan oxu
            Console.Clear();
            Console.WriteLine("\t\t\t\t~ Sign In Page ~\n");
            while (true)
            {
                Console.WriteLine("Enter your username: ");
                string username = Console.ReadLine().Trim();
                User user = auth.SignInUser(username);
                return user;
            }

        }
    }
}
