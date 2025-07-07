using Hosbital_Project.FileHelpers;
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hosbital_Project.Pages
{
    internal class UserPage
    {

        public static void ChangeProfile(Hosbital hosbital, User user)
        {

            List<string> changeOptions = new List<string> { "Change Username", "Change Email", "ChangePassord", "Change Phone Number" };
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear(); int changeIndex = Program.NavigateMenu(changeOptions, "\n ~ Change Profile Options", true);
                if (changeIndex == -1)
                    break;
                switch (changeIndex)
                {
                    case 0:
                        Console.Clear();
                        Console.Write("\n  Enter your new username: ");
                        string? newUsername = Console.ReadLine();
                        bool find3 = hosbital.SearchUser(newUsername);

                        if (newUsername == user.username)
                        {
                            Console.WriteLine(" New username cannot be the same as old username.");
                            Console.ReadKey();
                            continue;
                        }
                        if (string.IsNullOrWhiteSpace(newUsername) || newUsername.Length < 6)
                        {
                            Console.WriteLine("Username must be at least 6 characters.");
                            Console.ReadKey();
                            continue;
                        }
                        if (!Regex.IsMatch(newUsername, @"^[a-zA-Z0-9_]+$"))
                        {
                            Console.WriteLine("Username can only contain letters, numbers, and underscores.");
                            Console.ReadKey();
                            continue;
                        }
                        if (find3)
                        {
                            Console.WriteLine("Username already exists.");
                            Console.ReadKey();
                            continue;
                        }

                        user.username = newUsername;
                        //user faylina yaz
                        Console.WriteLine("Your username has been updated successfully.");
                        Console.ReadKey();
                        break;

                    case 1:
                        ChangeInformations.ChangeEmail(user, hosbital);
                        break;
                    case 2:
                        ChangeInformations.ChangePassword(user);
                        break;
                    case 3:
                        ChangeInformations.ChangePhone(user, hosbital);
                        break;
                }
                break;
            }
        }
        public static void UserMainMenu(Authentication auth, List<Department> departments, User user, Hosbital hosbital)
        {
            user.userNotifications = FileHelpers.FileHelper.ReadNotificationsFromFile(user.email);
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                List<string> options = new List<string> { "View Departments", "View Profile", "Notifications", "Change Profile", "View Appointments", "Cancel Appointment" };
                int selectedIndex = Program.NavigateMenu(options, $"\n ~ Welcome {user.name} {user.surname}\n", true, "~ Logout ");
                if (selectedIndex == -1)
                {
                    return;
                }
                switch (selectedIndex)
                {
                    case 0:
                        ReservePage.Departments(departments, user);
                        break;
                    case 1:
                        Console.Clear();
                        user.ViewProfile("Profile");
                        Console.WriteLine("\nPress any key to return to the main menu.");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        if (user.userNotifications.Count == 0)
                        {
                            Console.WriteLine("\n You don't have any notifications.");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            user.ViewNotifications();
                            Console.ReadKey();
                            int choiceindex = Program.NavigateMenu(new List<string> { "Delete Notification", "Back" }, "\n ~ Want to clear notifications?s");
                            if (choiceindex == 0)
                            {
                                user.userNotifications.Clear();  
                                FileHelper.WriteNotificationsToFile(new List<Notification>(), user.email);

                            }
                            else if(choiceindex==1)
                            {
                            }
                            Console.ReadKey();

                            break;
                        }
                    case 3:
                        ChangeProfile(hosbital, user);
                        break;
                    case 4:
                        Console.Clear();
                        user.ViewAppointments();
                        Console.WriteLine("\nPress any key to return to the main menu.");
                        Console.ReadKey();
                        break;

                    case 5:
                        Console.Clear();
                        if (user.Appointments.Count == 0)
                        {
                            Console.WriteLine("\n You don't have any appointments.");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            int cancelIndex = Program.NavigateMenu(user.Appointments.Select(ds => $" Dr.{ds.doctor.name} - {ds.receptionDay} - {ds.receptionHour.start} - {ds.receptionHour.end} ").ToList(), "Select an appointment to cancel", true);
                            if (cancelIndex == -1)
                                break;

                            var doctor = user.Appointments[cancelIndex].doctor;

                            string messageUser = $"You cancelled your meeting with Dr.{doctor.name}";
                            Console.WriteLine("\n ~ Appointment cancelled successfully.");
                            Console.WriteLine("\n ~ Please wait for the email to be sent...");
                            string subject = "Appointment Cancellation Confirmation – Hope Medical Center";

                            var day = user.Appointments[cancelIndex].receptionDay;
                            var time = user.Appointments[cancelIndex].receptionHour.ToString(true);
                            string body = $"Dear {user.name},\n\n" +
                                          $"This is to confirm that your appointment with Dr.{doctor.name} on {day} at {time} has been successfully cancelled as per your request.\n\n" +
                                          "If you wish to reschedule, please use your patient panel or contact us directly.\n\n" +
                                          "Thank you for keeping us informed.\n\n" +
                                          "Best regards,\nHope Medical Center Team";

                            Notification notification = new Notification(subject, messageUser, body, user.email);
                            user.userNotifications.Add(notification);
                            FileHelpers.FileHelper.WriteNotificationsToFile(user.userNotifications, user.email);
                            string messageDoctor = $"Patient {user.name} has cancelled an appointment with you.";

                            string subjectdoctor = "Appointment Cancellation Notice – Hope Medical Center";

                            string bodydoctor = $"Dear Dr.{doctor.name},\n\n" +
                                          $"The appointment with patient {user.name}, scheduled on {day} at {time} has been cancelled by the patient.\n\n" +
                                          "Please update your schedule accordingly.\n\n" +
                                          "Regards,\nHope Medical Center Team";

                            Notification notification2 = new Notification(subjectdoctor, messageDoctor, bodydoctor, doctor.email);
                            user.Appointments[cancelIndex].doctor.doctorsNotifications.Add(notification2);
                            FileHelpers.FileHelper.WriteNotificationsToFile(user.Appointments[cancelIndex].doctor.doctorsNotifications, doctor.email);

                            user.Appointments[cancelIndex].doctor.Appointments.RemoveAt(cancelIndex);
                            user.Appointments.RemoveAt(cancelIndex);
                            //fayla yaz
                            Console.ReadKey();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }
}
