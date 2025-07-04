using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Pages
{
    internal class CandidatePage
    {
        public static void DoctorCandidate(string email, List<Department> departments, Hosbital hosbital, Authentication auth)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                string title = "\n\t\t\t\t\t --- Doctor Application Form --- \n\n";
                int index = Program.NavigateMenu(departments, $"{title} ~ Please choose your department:", false);
                if (index == -1) break;
                var candidatesDepartment = departments[index];
                Console.Write(" | Please Enter your name: ");

                string? name = Console.ReadLine();
                Console.Write(" | Surname: ");
                string? surname = Console.ReadLine();

                Console.Write(" | Enter your password: ");
                string? password = Console.ReadLine();

                int regionIndex = Program.NavigateMenu(new List<string> { "AZ", "US", "TR", "RU" }, " ~ Select your region code: ", false);
                if (regionIndex == -1) break;
                string regionCode = new List<string> { "AZ", "US", "TR", "RU" }[regionIndex];
                Console.Write(" | Enter your phone number: ");
                string? phone = Console.ReadLine();
                Console.Write(" | Enter your experience year: ");
                string? experienceYear = Console.ReadLine();
                int experienceYearInt = int.TryParse(experienceYear, out int year) ? year : 0;
                Department department = candidatesDepartment;
                Console.WriteLine(" why do you want to be a doctor?");
                string? reason = Console.ReadLine();
                auth.DoctorCandidateRegistration(hosbital, password, name, surname, email, phone, regionCode, experienceYearInt, candidatesDepartment, reason);
                //cadidate faylina yaz
                Console.ReadKey();
                return;
            }
        }
        public static void CandidatePaGe(Hosbital hosbital, Authentication auth, List<Department> departments)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear(); while (true)
            {
                Console.Clear();
                Console.WriteLine("\n\t\t\t\t\t--- Candidate Page ---\n");
                Console.Write(" - Enter email: ");
                string? email = Console.ReadLine();
                DoctorCandidate find = hosbital.FindCandidate(email);
                bool find2 = hosbital.IsEmailUsedByUserOrDoctor(email);
                if (!find2)
                {
                    if (find != null)
                    {
                        while (true)
                        {
                            string title2 = $"\n\t\t\t\t\t=== Doctor Candidate Status ===\r\n\n{find} ~ Your application is being reviewed. Please wait patiently.\n";
                            List<string> choices = new List<string> { "Edit Application ", "Cancel Application " };
                            int choiceIndex = Program.NavigateMenu(choices, title2, true, " Exit");
                            while (true)
                            {
                                if (choiceIndex == -1)
                                {
                                    return;
                                }
                                if (choiceIndex == 0)
                                {
                                    Console.Clear();
                                    string title = $"\n\t\t\t\t\t --- Edit Application Form --- \n";
                                    List<string> changeOptions = new List<string> { "Change Email", "Change Phone Number", "Change Experience Year", "Change Password", "Change Reason" };
                                    int choiceInx = Program.NavigateMenu(changeOptions, title, true);
                                    if (choiceInx == -1)
                                        break;
                                    else if (choiceInx == 0)
                                    {
                                        ChangeInformations.ChangeEmail(find, hosbital);
                                    }
                                    else if (choiceInx == 1)
                                    {
                                        ChangeInformations.ChangePhone(find, hosbital);
                                    }
                                    else if (choiceInx == 2)
                                    {
                                        Console.Write(" | Enter your experience year: ");
                                        string? experienceYear = Console.ReadLine();
                                        int experienceYearInt = int.TryParse(experienceYear, out int year) ? year : 0;
                                        find.experienceYear = experienceYearInt;
                                        Console.WriteLine(" Experience year updated succefully");
                                        Console.ReadKey();
                                    }
                                    else if (choiceInx == 3)
                                    {
                                        ChangeInformations.ChangePassword(find);
                                    }
                                    else if (choiceInx == 4)
                                    {
                                        Console.WriteLine(" why do you want to be a doctor?");
                                        string? reason = Console.ReadLine();
                                        int wordCount = reason
                .Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Length;
                                        if (string.IsNullOrWhiteSpace(reason))
                                            Console.WriteLine("Reason cannot be empty.");
                                        else if (string.IsNullOrWhiteSpace(reason) || wordCount < 15)
                                        {
                                            Console.WriteLine("Reason must be at least 15 words.");
                                            continue;

                                        }
                                        else { find.reason = reason; }

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        DoctorCandidate(email, departments, hosbital, auth);
                        break;

                    }
                }
                else
                {
                    Console.WriteLine(" ! You have already registered as a user or doctor. Please use a different email.");
                    Console.ReadLine(); 
                    break;
                }
            }

        }
    }
}