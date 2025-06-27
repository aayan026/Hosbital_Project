
namespace Hosbital_Project.Models;
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
    static void ReserveDay(Doctor doctor, User user)
    {
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            var ReceptionDays = doctor.receptionDays;
            int choiceIndex = NavigateMenu(ReceptionDays, $"\n ~ Dr.{doctor.name}'s reception hours:\n", true);


            if (choiceIndex == -1)
                return;
            if (choiceIndex >= 0 && choiceIndex <= ReceptionDays.Count)
            {
                ReserveHour(doctor, user, choiceIndex, ReceptionDays);
                Console.ReadKey();
                return;
            }
        }

    }
    static void ReserveHour(Doctor doctor, User user, int dayIndex, List<ReceptionDay> receptionDays)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();
        var ReceptionHourlist = doctor.receptionDays[dayIndex].TimeSlots;

        string title = $"\n ~ These are the office hours of Dr.{doctor.name}.\n  Please select the days you would like to schedule an appointment.\n";
        var choiceIndex = NavigateMenu(ReceptionHourlist, title, true);

        if (choiceIndex == -1)
        {
            return;
        }
        if (!ReceptionHourlist[choiceIndex].isReserved)
        {
            if (choiceIndex >= 0 && choiceIndex <= ReceptionHourlist.Count)
            {
                doctor.ReserveHour(dayIndex, choiceIndex);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n ~ Thank you, {user.name} {user.surname}.~\n ! You have successfully booked an appointment with Dr.{doctor.name} at {receptionDays[dayIndex]} - {receptionDays[dayIndex].TimeSlots[choiceIndex].start.ToString("hh\\:mm")} - {receptionDays[dayIndex].TimeSlots[choiceIndex].end.ToString("hh\\:mm")}");
                user.Appointments.Add((doctor, receptionDays[dayIndex], receptionDays[dayIndex].TimeSlots[choiceIndex]));
                Console.ResetColor();
                return;
            }
        }
        else
        {
            Console.WriteLine(" This time is already reserved. Choose another one.");
            Console.ReadKey();

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
                    Departments(departments, user);
                    break;
                case 1:
                    Console.Clear();
                    user.ViewProfile();
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
    static void ChoiceDoctor(Department department, User user)
    {
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            int choiceIndex = NavigateMenu(department.doctors, "\n Doctors", true);
            if (choiceIndex == -1)
                return;
            if (choiceIndex >= 0 && choiceIndex <= department.doctors.Count)
            {
                ReserveDay(department.doctors[choiceIndex], user);
            }
        }
    }

    public static void Departments(List<Department> departments, User user)
    {
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            int navigator = NavigateMenu(departments, "\n  Departments", true);
            if (navigator == -1)
            {
                break;
            }
            if (navigator >= 0 && navigator < departments.Count)
            {
                ChoiceDoctor(departments[navigator], user);
            }
        }
    }

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
    public static void AdminPage(Authentication auth, Hosbital hosbital, Admin admin)
    {
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear(); Console.WriteLine("\n\t\t\t\t\t~ Admin Page ~\n");
            List<string> adminOptions = new List<string> { "View Users", "View Departments", "Add Department", "Remove Department","View Doctors","View Candidates", };
            int selectedIndex = NavigateMenu(adminOptions, "\n ~ Admin Options", true, "~ Logout ");
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
                        int index = NavigateMenu(hosbital.doctors, "\n\t\t\t\t\t --- Doctors --- \n\n", true);
                        if (index == -1)
                            break;
                        if (index >= 0 || index <= hosbital.doctors.Count())
                        {
                            Console.Clear();
                            hosbital.doctors[index].ViewProfile("~ Doctor Information ~");
                            Console.ReadKey();
                            continue;
                        }
                    }

                    break;
                case 5:
                    while(true)
                    {
                        Console.Clear();
                        int index = NavigateMenu(hosbital.doctorCandidates, "\n\t\t\t\t\t --- Candidates ---\n",true);
                        if (hosbital.doctorCandidates.Count() == 0)
                        {
                            Console.WriteLine(" There are no candidates.");
                        }
                        if (index==-1)
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
                $" Phone number: {candidate.phoneNumber}\n"+
                $" Reason: {candidate.reason}"+
                $"____________________________________________________\n";
                                List<string> list = new List<string> {"Accept","Reject"};
                                int index2 = NavigateMenu(list,title,true);
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
    static void DoctorCandidate(string email, List<Department> departments, Hosbital hosbital, Authentication auth)
    {
        while (true)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            string title = "\n\t\t\t\t\t --- Doctor Application Form --- \n\n";
            int index = NavigateMenu(departments, $"{title} ~ Please choose your department:", false);
            if (index == -1) break;
            var candidatesDepartment = departments[index];
            Console.Write(" | Please Enter your name: ");

            string name = Console.ReadLine();
            Console.Write(" | Surname: ");
            string surname = Console.ReadLine();

            Console.Write(" | Enter your password: ");
            string password = Console.ReadLine();

            int regionIndex = NavigateMenu(new List<string> { "AZ", "US", "TR", "RU" }, " ~ Select your region code: ", false);
            if (regionIndex == -1) break;
            string regionCode = new List<string> { "AZ", "US", "TR", "RU" }[regionIndex];
            Console.Write(" | Enter your phone number: ");
            string phone = Console.ReadLine();
            Console.Write(" | Enter your experience year: ");
            string experienceYear = Console.ReadLine();
            int experienceYearInt = int.TryParse(experienceYear, out int year) ? year : 0;
            Department department = candidatesDepartment;
            Console.WriteLine(" why do you want to be a doctor?");
            string reason = Console.ReadLine();
            auth.DoctorCandidateRegistration(hosbital, password, name, surname, email, phone, regionCode, experienceYearInt, candidatesDepartment, reason);
            //cadidate faylina yaz
            Console.ReadKey();
            return;
        }
    }
    static void CandidatePage(Hosbital hosbital, Authentication auth, List<Department> departments)
    {
        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear(); while (true)
        {
            Console.Clear();
            Console.WriteLine("\n\t\t\t\t\t--- Candidate Page ---\n");
            Console.Write(" - Enter email: ");
            string email = Console.ReadLine();
            DoctorCandidate find = hosbital.FindCandidate(email);
            if (find != null)
            {
                while (true) { 
                string title2 = $"\n\t\t\t\t\t=== Doctor Candidate Status ===\r\n\n{find} ~ Your application is being reviewed. Please wait patiently.\n";
                List<string> choices = new List<string> { "Edit Application ", "Cancel Application " };
                int choiceIndex = NavigateMenu(choices, title2, true, " Exit");
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
                            int choiceInx = NavigateMenu(changeOptions, title, true);
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
                                string experienceYear = Console.ReadLine();
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
                                string reason = Console.ReadLine();
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
    }
    static void Main(string[] args)
    {
        List<User> users = new List<User> { };
        Authentication auth = new Authentication(users);
        // Reception saatları
        var rh1 = new ReceptionHour("09:00", "11:00");
        var rh2 = new ReceptionHour("12:00", "14:00");
        var rh3 = new ReceptionHour("15:00", "17:00");

        ReceptionDay CreateReceptionDay(DayOfWeek day, params ReceptionHour[] hours)
        {
            var rd = new ReceptionDay(day);
            foreach (var h in hours)
                rd.AddTimeSlot(h.start.ToString("hh\\:mm"), h.end.ToString("hh\\:mm"));
            return rd;
        }

        // Department-lar
        var cardiology = new Department("Cardiology");
        var neurology = new Department("Neurology");
        var surgery = new Department("Surgery");
        List<Department> departments = new List<Department> { cardiology, neurology, surgery };

        // Doctor 1
        var doc1 = new Doctor("John", "Smith", "john@example.com", "1234", "0501234567", 8, cardiology, "AZ");
        doc1.AddReceptionDay(CreateReceptionDay(DayOfWeek.Monday, rh1, rh2));
        doc1.AddReceptionDay(CreateReceptionDay(DayOfWeek.Tuesday, rh3));

        // Doctor 2
        var doc2 = new Doctor("Emily", "Johnson", "emily@example.com", "1234", "0502345678", 5, neurology, "AZ");
        doc2.AddReceptionDay(CreateReceptionDay(DayOfWeek.Thursday, rh1));
        doc2.AddReceptionDay(CreateReceptionDay(DayOfWeek.Friday, rh3));

        // Doctor 3
        var doc3 = new Doctor("Michael", "Brown", "michael@example.com", "1234", "0503456789", 7, surgery, "AZ");
        doc3.AddReceptionDay(CreateReceptionDay(DayOfWeek.Wednesday, rh2));

        // Doctor 4
        var doc4 = new Doctor("Sarah", "Davis", "sarah@example.com", "1234", "0504567890", 6, cardiology, "AZ");
        doc4.AddReceptionDay(CreateReceptionDay(DayOfWeek.Monday, rh3));

        // Doctor 5
        var doc5 = new Doctor("David", "Wilson", "david@example.com", "1234", "0505678901", 9, neurology, "AZ");
        doc5.AddReceptionDay(CreateReceptionDay(DayOfWeek.Friday, rh1, rh2));

        // Doctor 6–10 (ReceptionDay əlavə olunmayıb)
        var doc6 = new Doctor("Benjamin", "Linus", "ben@example.com", "1234", "0501111111", 6, surgery, "AZ");
        var doc7 = new Doctor("John", "Carter", "carter@example.com", "1234", "0502222222", 9, cardiology, "AZ");
        var doc8 = new Doctor("Allison", "Cameron", "cam@example.com", "1234", "0503333333", 5, neurology, "AZ");
        var doc9 = new Doctor("Derek", "Shepherd", "derek@example.com", "1234", "0504444444", 11, surgery, "AZ");
        var doc10 = new Doctor("Rachel", "Green", "rachel@example.com", "1234", "0505555555", 4, cardiology, "AZ");
        // (İstəklə) toplu siyahı
        List<Doctor> doctors = new()
{
    doc1, doc2, doc3, doc4, doc5, doc6, doc7, doc8, doc9, doc10
};

        User? user1 = new User("aya_aliye283", "ayan1986", "ayan", "aliyeva", "ayan@gmail.com", "0707897878", "AZ");
        users.Add(user1);
        Hosbital hosbital = new Hosbital(departments, doctors, users);
        Admin admin = new Admin();
        DoctorCandidate dc = new DoctorCandidate(hosbital, "omer", "Aliyev", "omer@gmail.com", "omer123","0776787676", 3, cardiology, "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum","AZ");
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
                    AdminSignIn(auth);
                    AdminPage(auth, hosbital, admin);
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
                                User? user = Methods.SignInUser(auth);
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
                                User? user = Methods.RegistrUser(auth, departments, hosbital);
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
                    CandidatePage(hosbital, auth, departments);
                }
            }
        }
    }
}

