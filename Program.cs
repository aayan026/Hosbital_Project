

namespace Hosbital_Project.Models;

using Hosbital_Project.FileHelpers;
using Hosbital_Project.Pages;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Mail;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.IO;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    public static int NavigateMenu<T>(List<T> options, string title, bool showBack = false, string lastOptionLabel = "<-back")
    {
        int selectedIndex = 0;
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.WriteLine($"{title}\n");

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($" | {options[i]}");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                    Console.WriteLine($" | {options[i]}");
            }

            if (showBack)
            {
                if (selectedIndex >= options.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"\n {lastOptionLabel}");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                    Console.WriteLine($"\n {lastOptionLabel}");
            }

            var key = Console.ReadKey(true).Key;

            int maxIndex = showBack ? options.Count : options.Count - 1;

            if (key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1) < 0 ? maxIndex : selectedIndex - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) > maxIndex ? 0 : selectedIndex + 1;
            }
            else if (key == ConsoleKey.Enter)
            {
                if (selectedIndex == options.Count)
                    return -1;
                return selectedIndex;
            }
        }
    }

    static void ChangeProfile(Hosbital hosbital, User user)
    {

        List<string> changeOptions = new List<string> { "Change Username", "Change Email", "ChangePassord", "Change Phone Number" };
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear(); int changeIndex = NavigateMenu(changeOptions, "\n ~ Change Profile Options", true);
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
    static void UserMainMenu(Authentication auth, List<Department> departments, User user, Hosbital hosbital)
    {
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            List<string> options = new List<string> { "View Departments", "View Profile", "Notifications", "Change Profile", "View Appointments", "Cancel Appointment" };
            int selectedIndex = NavigateMenu(options, $"\n ~ Welcome {user.name} {user.surname}\n", true, "~ Logout ");
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
                        int cancelIndex = NavigateMenu(user.Appointments.Select(ds => $" Dr.{ds.doctor.name} - {ds.receptionDay} - {ds.receptionHour.start} - {ds.receptionHour.end} ").ToList(), "Select an appointment to cancel", true);
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
                        //fayla yaz
                        string messageDoctor = $"Patient {user.name} has cancelled an appointment with you.";

                        string subjectdoctor = "Appointment Cancellation Notice – Hope Medical Center";

                        string bodydoctor = $"Dear Dr.{doctor.name},\n\n" +
                                      $"The appointment with patient {user.name}, scheduled on {day} at {time} has been cancelled by the patient.\n\n" +
                                      "Please update your schedule accordingly.\n\n" +
                                      "Regards,\nHope Medical Center Team";

                        Notification notification2 = new Notification(subjectdoctor, messageDoctor, bodydoctor, doctor.email);
                        user.Appointments[cancelIndex].doctor.doctorsNotifications.Add(notification2);

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
    static void MainMenu()
    {
        List<User> users = FileHelper.ReadUsersFromFile();
        Authentication auth = new Authentication(users);
        List<Doctor> doctors = FileHelper.ReadDoctorsFromFile();

        foreach (var doc in doctors)
        {
            Console.WriteLine($"\nDoctor: {doc.email}");

            doc.receptionDays = FileHelper.ReadReceptionDaysFromFile(doc.email);
  
        }

        Doctor CreateDoctor(string name, string surname, string email, string password, string phone, int id, Department dept, string country, params DayOfWeek[] days)
        {
            var doc = new Doctor(name, surname, email, password, phone, id, dept, country);
            doc.receptionDays = FileHelper.ReadReceptionDaysFromFile(doc.email);
            doc.receptionSchedule = new ReceptionScheduleManager { doctor = doc };

            foreach (var day in days)
            {
                doc.receptionDays.Add(new ReceptionDay(day));
            }
            foreach(var doctor in doctors)
            {
                doctor.doctorsNotifications = FileHelper.ReadNotificationsFromFile(doctor.email);
            }
            FileHelper.WriteReceptionDaysToFile(doc.receptionDays, doc.email);
            return doc;
        }

        var neurology = new Department("Neurology");
        var surgery = new Department("Surgery");
        var psychiatry = new Department("Psychiatry");
        var obgyn = new Department("Obstetrics and Gynecology");
        List<Department> departments = FileHelper.ReadDepartmentsFromFile();
        if (departments.Count == 0)
        {
            departments.Add(neurology);
            departments.Add(surgery);
            departments.Add(psychiatry);
            departments.Add(obgyn);
            FileHelper.WriteDepartmentsToFile(departments);
        }

        List<DoctorCandidate> candidates = FileHelper.ReadCandidatesFromFile();
        Hosbital.LinkDepartmentsToDoctors(doctors, departments);
        Hosbital.LinkDoctorsToDepartments(doctors, departments);
        Hosbital.LinkDepartmentsToCandidates(candidates, departments);

        if (doctors.Count == 0)
        {
            var doc1 = CreateDoctor("Jack", "Shephard", "jack@gmail.com", "1234", "0501234567", 8, surgery, "AZ", DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday);
            var doc11 = CreateDoctor("Juliet", "Burke", "juliet@gmail.com", "1234", "0506787676", 4, obgyn, "AZ", DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Friday);
            var docHurley = CreateDoctor("Hugo", "Reyes", "hurley@gmail.com", "1234", "0501234567", 5, psychiatry, "US", DayOfWeek.Monday, DayOfWeek.Thursday);
            var docLibby = CreateDoctor("Libby", "Smith", "libby@gmail.com", "5678", "0502345678", 6, psychiatry, "US", DayOfWeek.Tuesday, DayOfWeek.Friday);
            var docLenny = CreateDoctor("Leonard", "Simms", "lenny@gmail.com", "9999", "0503456789", 7, psychiatry, "US", DayOfWeek.Wednesday, DayOfWeek.Saturday);
            var doc2 = CreateDoctor("Emily", "Johnson", "emily@gmail.com", "1234", "0502345678", 5, obgyn, "AZ", DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday);
            var doc3 = CreateDoctor("Ethan", "Rom", "ethan@gmail.com", "1234", "0503456789", 7, surgery, "AZ", DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday);
            var doc4 = CreateDoctor("Christian", "Shephard", "christian@gmail.com", "1234", "0504567890", 6, surgery, "AZ", DayOfWeek.Monday);
            var doc5 = CreateDoctor("David", "Wilson", "david@gmail.com", "1234", "0505678901", 9, neurology, "AZ", DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Monday);
            var doc6 = CreateDoctor("Benjamin", "Linus", "ben@gmail.com", "1234", "0501111111", 6, surgery, "AZ", DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday);
            var doc7 = CreateDoctor("John", "Carter", "carter@gmail.com", "1234", "0502222222", 9, surgery, "AZ", DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday);
            var doc8 = CreateDoctor("Allison", "Cameron", "cam@gmail.com", "1234", "0503333333", 5, obgyn, "AZ", DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday);
            var doc9 = CreateDoctor("Michael", "Brown", "michael@gmail.com", "1234", "0504444444", 11, neurology, "AZ", DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday);
            var doc10 = CreateDoctor("Rachel", "Green", "rachel@gmail.com", "1234", "0505555555", 4, neurology, "AZ", DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday);
            doctors.Add(doc1);
            doctors.Add(doc2);
            doctors.Add(doc3);
            doctors.Add(doc4);
            doctors.Add(doc5);
            doctors.Add(doc6);
            doctors.Add(doc7);
            doctors.Add(doc8);
            doctors.Add(doc9);
            doctors.Add(doc10);
            doctors.Add(doc11);
            doctors.Add(docHurley);
            doctors.Add(docLibby);
            doctors.Add(docLenny);
            FileHelper.WriteDoctorsToFile(doctors);
        }

        User? user1 = new User("aya_aliye283", "ayan1929", "ayan", "aliyeva", "aliyevanar1986a@gmail.com", "0707897878", "AZ");
        Hosbital hosbital = new Hosbital(departments, doctors, users, candidates);
        Admin admin = new Admin();

        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            List<string> roles = new List<string> { "Admin", "User", "Doctor", "Candidate" };

            string title = $"\n\t\t\t\t\t\t  --- Hospital --- \n\n - Select your role to log in:";
            int choiceIndex = NavigateMenu(roles, title, true, "~ Exit");
            {
                if (choiceIndex == -1)
                    break;
                if (choiceIndex == 0)
                {
                    AdminPage.AdminSignIn(admin, hosbital, auth);
                }
                else if (choiceIndex == 1)
                {
                    while (true)
                    {
                        List<string> ans = new List<string> { "Yes", "No" };
                        int ansIndex = NavigateMenu(ans, "\n ~ Do you have an account?", true);
                        {
                            if (ansIndex == -1)
                                break;
                            if (ansIndex == 0)
                            {
                                User? user = AuthenticationMethods.SignInUser(auth);
                                if (user == null)
                                {
                                    Console.WriteLine(" ~ User not found. Please try again.");
                                    Console.ReadKey();
                                    continue;
                                }
                                else
                                {
                                    Console.WriteLine("Successfully signed in!");
                                    Console.ReadKey();
                                    UserMainMenu(auth, departments, user, hosbital);
                                }
                            }
                            else if (ansIndex == 1)
                            {
                                User? user = AuthenticationMethods.RegistrUser(auth, departments, hosbital);

                                if (user != null)
                                {
                                    users.Add(user);
                                    FileHelper.WriteUsersToFile(users);
                                    UserMainMenu(auth, departments, user, hosbital);
                                }
                            }
                        }
                    }
                }
                else if (choiceIndex == 2)
                {
                    Console.Clear();
                    var doctor = AuthenticationMethods.DoctorSignIn(hosbital, auth);
                    if (doctor != null)
                    {
                        DoctorPage.DoctorPaGe(hosbital, auth, doctor);
                    }
                }
                else if (choiceIndex == 3)
                {
                    CandidatePage.CandidatePaGe(hosbital, auth, departments);
                }
            }
        }

    }

    static void Main(string[] args)
    {
        MainMenu();


    }




}
