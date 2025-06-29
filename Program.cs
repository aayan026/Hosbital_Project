
namespace Hosbital_Project.Models;

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
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

            if (key == ConsoleKey.UpArrow)
            {
                int maxIndex = showBack ? options.Count : options.Count - 1;
                selectedIndex = (selectedIndex - 1) < 0 ? maxIndex : selectedIndex - 1;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                int maxIndex = showBack ? options.Count : options.Count - 1;
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
                    string newUsername = Console.ReadLine();
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
            List<string> options = new List<string> { "View Departments", "View Profile", "Change Profile", "View Appointments", "Cancel Appointment" };
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
                    ChangeProfile(hosbital, user);
                    break;
                case 3:
                    Console.Clear();
                    user.ViewAppointments();
                    Console.WriteLine("\nPress any key to return to the main menu.");
                    Console.ReadKey();
                    break;

                case 4:
                    Console.Clear();
                    if (user.Appointments.Count == 0)
                    {
                        Console.WriteLine("\n You don't have any appointments.");
                        Console.ReadKey();
                        break;
                    }
                    else
                    {
                        int cancelIndex = NavigateMenu(user.Appointments.Select(ds => $" Dr.{ds.doctor.name} - {ds.receptionDay} - {ds.receptionHour.start.ToString("hh\\:mm")} - {ds.receptionHour.end.ToString("hh\\:mm")} ").ToList(), "Select an appointment to cancel", true);
                        if (cancelIndex == -1)
                            break;
                        user.Appointments.RemoveAt(cancelIndex);
                        Console.WriteLine("Appointment cancelled successfully.");// appointment faylina yaz
                        Console.ReadKey();
                        break;
                    }

                case 5:
                    Console.WriteLine("Logging out...");
                    Thread.Sleep(1000);
                    break;
                default:
                    Console.WriteLine("Invalid selection. Please try again.");
                    break;
            }
        }
    }


    static void DoctorPage(Hosbital hosbital, Authentication auth, Doctor doctor)
    {
        string title = $"\n\t\t\t\t\t --- W E L C O M E ---";
        int choiceIndex = NavigateMenu(new List<string> { "View Appointments", "View Profile", "Change Profile" }, title, true, "~ Logout");
        if (choiceIndex == -1)
        {
            return;
        }
        switch (choiceIndex)
        {
            case 0:
                Console.Clear();
                doctor.ViewAppointments();
                Console.ReadKey();
                break;
            case 1:
                Console.Clear();
                hosbital.ProfileInfo("Doctor Profile", doctor);
                Console.ReadKey();
                break;
            case 2:
                Console.Clear();
                List<string> changeOptions = new List<string> { "Change Email", "Change Password", "Change Phone Number" };
                int selectedIndex = NavigateMenu(changeOptions, title);
                while (true)
                {
                    if (selectedIndex == -1)
                        return;
                    if(selectedIndex==0)
                    ChangeInformations.ChangeEmail(doctor, hosbital);
                    else if(selectedIndex == 1)
                    {
                        ChangeInformations.ChangePassword(doctor);
                    }
                    else if (selectedIndex == 2)
                    {
                        ChangeInformations.ChangePhone(doctor, hosbital);
                    }
                }
                break;
            case 3:
                Console.WriteLine("Logging out...");
                Thread.Sleep(1000);
                break;
        }
    }

    static void Main(string[] args)
    {
        List<User> users = new List<User> { };
        Authentication auth = new Authentication(users);

        var neurology = new Department("Neurology");
        var surgery = new Department("Surgery");
        var psychiatry = new Department("Psychiatry");
        var obgyn = new Department("Obstetrics and Gynecology");

        List<Department> departments = new List<Department> { surgery, neurology, psychiatry, obgyn };

        Doctor CreateDoctor(string name, string surname, string email, string password, string phone, int id, Department dept, string country, params DayOfWeek[] days)
        {
            var doc = new Doctor(name, surname, email, password, phone, id, dept, country);
            foreach (var day in days)
            {
                doc.receptionSchedule.AddReceptionDay(day);
            }
            return doc;
        }
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
        List<Doctor> doctors = new()
{
    doc1, doc2, doc3, doc4, doc5, doc6, doc7, doc8, doc9, doc10, doc11, docHurley, docLibby, docLenny,
};

        User? user1 = new User("aya_aliye283", "ayan1986", "ayan", "aliyeva", "ayan@gmail.com", "0707897878", "AZ");
        users.Add(user1);
        Hosbital hosbital = new Hosbital(departments, doctors, users);
        Admin admin = new Admin();
        DoctorCandidate dc = new DoctorCandidate(hosbital, "omer", "Aliyev", "omer@gmail.com", "omer123", "0776787676", 3, neurology, "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum", "AZ");
        hosbital.doctorCandidates.Add(dc);

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
                    AdminPage.AdminSignIn(auth);
                    AdminPage.AdminPaGe(auth, hosbital, admin);
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
                                users.Add(user);
                                UserMainMenu(auth, departments, user, hosbital);
                            }
                        }
                    }
                }
                else if (choiceIndex == 2)
                {
                    //doctor page
                }
                else if (choiceIndex == 3)
                {
                    CandidatePage.CandidatePaGe(hosbital, auth, departments);
                }
            }
        }
    }
}

