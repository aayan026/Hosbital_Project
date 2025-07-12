using Hospital_Project.FileHelpers;
using Hospital_Project.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hospital_Project.Pages
{
    internal class UserPage
    {

        public static void ChangeProfile(Hospital hosbital, User user)
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
                        Console.WriteLine("Your username has been updated successfully.");
                        Log.Information("user changed his/her username");
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
        public static void UserMainMenu(Authentication auth, List<Department> departments, User user, Hospital hosbital)
        {
            user.userNotifications = FileHelpers.FileHelper.ReadNotificationsFromFile(user.email);
            user.Appointments = FileHelpers.FileHelper.ReadAppointmentsForUserOrDoctor(user.email, "user");
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
                        Log.Information("user viewed his/her profile");
                        Console.WriteLine("\nPress any key to return to the main menu.");
                        Console.ReadKey();
                        break;
                    case 2:
                        Console.Clear();
                        if (user.userNotifications.Count == 0)
                        {
                            Console.WriteLine("\n You don't have any notifications.");
                            Log.Information("user viewed his/her notifications");

                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            user.ViewNotifications();
                            Console.ReadKey();
                            int choiceindex = Program.NavigateMenu(new List<string> { "Delete Notification", "Back" }, "\n ~ Want to clear notifications?");
                            if (choiceindex == 0)
                            {
                                user.userNotifications.Clear();
                                FileHelper.WriteNotificationsToFile(new List<Notification>(), user.email);
                                Log.Information("user deleted notifications");


                            }
                            else if (choiceindex == 1)
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
                        Log.Information("user viewed his/her appointments");

                        Console.WriteLine("\nPress any key to return to the main menu.");
                        Console.ReadKey();
                        break;
                    case 5:
                        var doctors = FileHelpers.FileHelper.ReadDoctorsFromFile();
                        Console.Clear();
                        if (user.Appointments.Count == 0)
                        {
                            Console.WriteLine("\n You don't have any appointments.");
                            Console.ReadKey();
                            break;
                        }

                        int cancelIndex = Program.NavigateMenu(
                            user.Appointments.Select(ds => $" {ds.DoctorName} - {ds.Day} - {ds.Hour} ").ToList(),
                            "Select an appointment to cancel",
                            true
                        );

                        if (cancelIndex == -1)
                            break;

                        var appointment = user.Appointments[cancelIndex];
                        string doctorEmail = appointment.DoctorEmail;

                        Doctor? doctor = doctors.FirstOrDefault(d => d.email == doctorEmail);
                        if (doctor == null)
                        {
                            Console.WriteLine("Doctor not found.");
                            Console.ReadKey();
                            break;
                        }

                        Console.WriteLine("\n ~ Appointment cancelled successfully.");
                        Log.Information("The user canceled the appointment with doctor {name}.",doctor.name);

                        Console.WriteLine("\n ~ Please wait for the email to be sent...");

                        string subjectUser = "Appointment Cancellation Confirmation – Hope Medical Center";
                        string messageUser = $"You cancelled your meeting with Dr.{doctor.name}";
                        string bodyUser = $"Dear {user.name},\n\n" +
                                          $"This is to confirm that your appointment with Dr.{doctor.name} on {appointment.Day} at {appointment.Hour} has been successfully cancelled as per your request.\n\n" +
                                          "If you wish to reschedule, please use your patient panel or contact us directly.\n\n" +
                                          "Thank you for keeping us informed.\n\n" +
                                          "Best regards,\nHope Medical Center Team";

                        Notification notificationUser = new Notification(subjectUser, messageUser, bodyUser, user.email);
                        user.userNotifications.Add(notificationUser);
                        FileHelpers.FileHelper.WriteNotificationsToFile(user.userNotifications, user.email);

                        string subjectDoctor = "Appointment Cancellation Notice – Hope Medical Center";
                        string messageDoctor = $"Patient {user.name} has cancelled an appointment with you.";
                        string bodyDoctor = $"Dear Dr.{doctor.name},\n\n" +
                                            $"The appointment with patient {user.name}, scheduled on {appointment.Day} at {appointment.Hour} has been cancelled by the patient.\n\n" +
                                            "Please update your schedule accordingly.\n\n" +
                                            "Regards,\nHope Medical Center Team";

                        Notification notificationDoctor = new Notification(subjectDoctor, messageDoctor, bodyDoctor, doctor.email);
                        doctor.doctorsNotifications.Add(notificationDoctor);
                        FileHelpers.FileHelper.WriteNotificationsToFile(doctor.doctorsNotifications, doctor.email);

                        var doctorAppointment = doctor.Appointments.FirstOrDefault(a =>
                            a.UserEmail == appointment.UserEmail &&
                            a.DoctorEmail == appointment.DoctorEmail &&
                            a.Day == appointment.Day &&
                            a.Hour == appointment.Hour
                        );
                        if (doctorAppointment != null)
                            doctor.Appointments.Remove(doctorAppointment);

                        user.Appointments.RemoveAt(cancelIndex);
                        FileHelpers.FileHelper.RemoveAppointmentFromFile(appointment);

                        DayOfWeek day= (DayOfWeek)Enum.Parse(typeof(DayOfWeek), appointment.Day);
                        var receptionDay = doctor.receptionDays.Find(rd => rd.dayOfWeek == day);
                        var hour = receptionDay.TimeSlots.FirstOrDefault(ts => ts.ToString(true) == appointment.Hour);
                        hour.isReserved = false;
                        FileHelper.WriteReceptionDaysToFile(doctor.receptionDays, doctorEmail);

                        Console.ReadKey();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
