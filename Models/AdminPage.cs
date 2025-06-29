using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class AdminPage
    {
        public static void AdminSignIn(Authentication auth)
        {
            while (true)
            {

                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear(); Console.WriteLine("\n\t\t\t\t\t~ Admin Sign In ~\n");
                Console.Write(" email: ");
                string email = Console.ReadLine();
                Console.Write(" Password: ");
                string password = Console.ReadLine();
                if (auth.AdminSignIn(email, password))
                {
                    Console.WriteLine("Successfully signed in as admin!");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid admin credentials. Please try again.");
                    Console.ReadKey();
                    continue;
                }
            }
        }
        public static void AdminPaGe(Authentication auth, Hosbital hosbital, Admin admin)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear(); Console.WriteLine("\n\t\t\t\t\t~ Admin Page ~\n");
                List<string> adminOptions = new List<string> { "View Users", "View Departments", "Add Department", "Remove Department", "View Doctors", "View Candidates", };
                int selectedIndex =Program. NavigateMenu(adminOptions, "\n ~ Admin Options", true, "~ Logout ");
                if (selectedIndex == -1)
                {
                    return;
                }
                switch (selectedIndex)
                {
                    case 0:
                        Console.Clear();
                        admin.ViewUsers(auth.users);
                        Console.ReadKey();
                        continue;
                    case 1:
                        Console.Clear();
                        admin.ViewDepartments(hosbital.departments);
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        admin.AddDepartment(hosbital);
                        break;
                    case 3:
                        Console.Clear();
                        admin.RemoveDepartment(hosbital);

                        break;
                    case 4:
                        while (true)
                        {
                            int index =Program. NavigateMenu(hosbital.doctors, "\n\t\t\t\t\t --- Doctors --- \n\n", true);
                            if (index == -1)
                                break;
                            if (index >= 0 || index <= hosbital.doctors.Count())
                            {
                                Console.Clear();
                                hosbital.ProfileInfo("~ Doctor Information ~", hosbital.doctors[index]);
                                Console.ReadKey();
                                continue;
                            }
                        }

                        break;
                    case 5:
                        while (true)
                        {
                            Console.Clear();
                            int index =Program. NavigateMenu(hosbital.doctorCandidates, "\n\t\t\t\t\t --- Candidates ---\n", true);
                            if (hosbital.doctorCandidates.Count() == 0)
                            {
                                Console.WriteLine(" There are no candidates.");
                            }
                            if (index == -1)
                            {
                                break;
                            }
                            else
                            {
                                if (index <= hosbital.doctorCandidates.Count() || index >= 0)
                                {
                                    var candidate = hosbital.doctorCandidates[index];
                                    string title = $"\n Name: {candidate.name}\n" +
                    $" Surname: {candidate.surname}\n" +
                    $" Department: {candidate.department}\n" +
                    $" Experience year: {candidate.experienceYear}\n" +
                    $" Phone number: {candidate.phoneNumber}\n" +
                    $" Reason: {candidate.reason}" +
                    $"____________________________________________________\n";
                                    List<string> list = new List<string> { "Accept", "Reject" };
                                    int index2 = Program.NavigateMenu(list, title, true);
                                    if (index2 == 0)
                                    {
                                        admin.AcceptedDoctor(hosbital, candidate);
                                        //emailine getsinki qebul olundu bildirim
                                    }
                                    else if (index2 == 1)
                                    {
                                        admin.RejectDoctor(hosbital, candidate);
                                        //levg olundu istek
                                    }

                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
