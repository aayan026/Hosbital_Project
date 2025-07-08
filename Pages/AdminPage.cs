using Hosbital_Project.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Pages
{
    internal class AdminPage
    {
        public static void AdminSignIn(Admin admin, Hosbital hosbital, Authentication auth)
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
                    Log.Information(" admin succesfully signed in");
                    AdminPaGe(auth, hosbital, admin);
                }
                else
                {
                    Console.WriteLine("Invalid admin credentials. Please try again.");
                    Console.ReadKey();
                    break;
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
                int selectedIndex = Program.NavigateMenu(adminOptions, "\n ~ Admin Options", true, "~ Logout ");
                if (selectedIndex == -1)
                {
                    return;
                }
                switch (selectedIndex)
                {
                    case 0:
                        Console.Clear();
                      hosbital.ViewUsers();
                        Log.Information("admin looked at users");
                        Console.ReadKey();
                        continue;
                    case 1:
                        Console.Clear();
                        hosbital.ViewDepartments();
                        Log.Information("admin looked at departments");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        hosbital.AddDepartment();
                        break;
                    case 3:
                        Console.Clear();
                        hosbital.RemoveDepartment();

                        break;
                    case 4:
                        while (true)
                        {
                            int index = Program.NavigateMenu(hosbital.doctors, "\n\t\t\t\t\t --- Doctors --- \n\n", true);
                            if (index == -1)
                                break;
                            if (index >= 0 || index <= hosbital.doctors.Count())
                            {
                                Console.Clear();
                                hosbital.ProfileInfo("~ Doctor Information ~", hosbital.doctors[index]);
                                Console.ReadKey();
                                Log.Information("admin looked at the doctor {name}'s informations", hosbital.doctors[index].name);
                                continue;
                            }
                        }

                        break;
                    case 5:
                        while (true)
                        {
                            Console.Clear();
                            int index = Program.NavigateMenu(hosbital.doctorCandidates, "\n\t\t\t\t\t --- Candidates ---\n", true);
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
                    $"\n______________________________________________________________________________\n";
                                    Log.Information("admin looked at candidates");
                                    List<string> list = new List<string> { "Accept", "Reject" };
                                    int index2 = Program.NavigateMenu(list, title, true);
                                    if (index2 == 0)
                                    {
                                        hosbital.AcceptedDoctor(candidate);
                                        string subject = "Application Accepted – Hope Medical Center";

                                        string body = $"Dear Dr.{candidate.name} {candidate.surname},\n\n" +
                                                      "We are pleased to inform you that your application for the doctor position at Hope Medical Center has been approved.\n\n" +
                                                      "Your profile met our selection criteria and we are happy to welcome you to our medical team.\n\n" +
                                                      "Further instructions regarding your onboarding, access credentials, and responsibilities will be sent to you shortly.\n\n" +
                                                      "Congratulations and welcome aboard!\n\n" +
                                                      "Warm regards,\nHope Medical Center Team";
                                        NotificationService.SendEmail(subject, body, candidate.email);
                                    }
                                    else if (index2 == 1)
                                    {
                                        hosbital.RejectDoctor(candidate);
                                        string subject = "Application Cancellation – Hope Medical Center";

                                        string body = $"Dear {candidate.name},\n\n" +
                                                      "We regret to inform you that your application for the doctor position at Hope Medical Center has been cancelled.\n\n" +
                                                      "This may be due to incomplete information, missing documents, or a decision by the hospital board.\n\n" +
                                                      "If you believe this was a mistake or you wish to apply again, please feel free to reach out or submit a new application.\n\n" +
                                                      "We appreciate your interest in joining our team.\n\n" +
                                                      "Kind regards,\nHope Medical Center Team";

                                        NotificationService.SendEmail(subject, body, candidate.email);

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