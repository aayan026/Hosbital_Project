
using Hosbital_homework.Models;
using System.Collections.Generic;
using System.Numerics;

class Program
{
    public static int NavigateMenu<T>(List<T> options, string title, bool showBack = false, string lastOptionLabel = "<-back")
    {
        int selectedIndex = 0;
        showBack = true;
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"{title}\n");

            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($" | {options[i]}");
                    Console.ResetColor();
                }
                else
                    Console.WriteLine($" | {options[i]}");
            }

            if (showBack)
            {
                if (selectedIndex >= options.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n {lastOptionLabel}");
                    Console.ResetColor();
                }
                else
                    Console.WriteLine($"\n {lastOptionLabel}");
            }

            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.UpArrow)
                selectedIndex = (selectedIndex == 0) ? options.Count : selectedIndex - 1;
            else if (key == ConsoleKey.DownArrow)
                selectedIndex = (selectedIndex + 1) % (options.Count + 1);
            else if (key == ConsoleKey.Enter)
            {
                if (selectedIndex == options.Count)
                    return -1;
                return selectedIndex;
            }
        }
    }
    static void ReserveHour(Doctor doctor, User user, int dayIndex, List<ReceptionDay> receptionDays)
    {
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
    static void ReserveDay(Doctor doctor, User user)
    {
        while (true)
        {
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
    static void UserMainMenu(List<Department> departments, User user, Hosbital hosbital)
    {
        while (true)
        {
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
                    List<string> changeOptions = new List<string> { "Change Username", "Change Email", "Change Phone Number" };
                    while (true)
                    {
                        Console.Clear();
                        int changeIndex = NavigateMenu(changeOptions, "\n ~ Change Profile Options", true);
                        if (changeIndex == -1)
                            break;
                        switch (changeIndex)
                        {
                            case 0:
                                Console.Clear();
                                Console.Write("\n  Enter your new username: ");
                                string newUsername = Console.ReadLine();
                                bool find3 = hosbital.SearchUser(newUsername);

                                if (newUsername == user.Username)
                                {
                                    Console.WriteLine(" New username cannot be the same as old username.");
                                    Console.ReadKey();
                                    continue;
                                }
                                else if (find3)
                                {
                                    Console.WriteLine(" This username already exists.");
                                    Console.ReadKey();
                                    continue;
                                }
                                else
                                {
                                    user.Username = newUsername;
                                    //user faylina yaz
                                    if (user.Username == null)
                                    {
                                        Console.WriteLine("Invalid username.");
                                        Console.ReadKey();
                                        continue;
                                    }
                                    //fayla yaz
                                    Console.WriteLine("Your username has been updated successfully.");
                                    Console.ReadKey();
                                }
                                break;

                            case 1:
                                Console.Clear();
                                Console.Write("\n  Enter current email: ");
                                string oldEmail = Console.ReadLine();
                                Console.Write("  Enter your new email: ");
                                string email = Console.ReadLine();
                                //bool find = 
                                if (email == user.Email)
                                {
                                    Console.WriteLine(" New email cannot be the same as old email");
                                    Console.ReadKey();
                                    continue;
                                }
                                else
                                {
                                    var newEmail = user.Email = email;
                                    //user faylina yaz
                                    if (newEmail == null)
                                    {
                                        Console.WriteLine("Invalid email.");
                                        Console.ReadKey();
                                    }
                                    else
                                    {
                                        //fayla yaz
                                        Console.WriteLine("Your email has been updated successfully.");
                                    }
                                    Console.ReadKey();
                                }
                                break;

                            case 2:
                                List<string> regionCodes = new List<string> { "AZ", "US", "TR", "RU" };
                                int choiceIndex = NavigateMenu(regionCodes, "\n ~ Select your region code: ");
                                if (choiceIndex >= 0 && choiceIndex < regionCodes.Count)
                                {
                                    user.regionCode = regionCodes[choiceIndex];
                                    Console.Write("  Enter your new phone number: ");
                                    string azPhone = Console.ReadLine();
                                    bool find = hosbital.SearchPhone(azPhone);
                                    if (user.PhoneNumber == azPhone)
                                    {
                                        Console.WriteLine(" New email cannot be the same as old email");
                                        Console.ReadKey();
                                        continue;
                                    }
                                    else if (find)
                                    {
                                        Console.WriteLine(" This number belongs to an existing user"); Console.ReadKey();
                                    }

                                    else
                                    {
                                        user.PhoneNumber = azPhone;
                                        //user faylina yaz
                                        if (user.PhoneNumber == null)
                                        {
                                            Console.WriteLine("Invalid phone number.");
                                            Console.ReadKey();
                                            continue;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Your phone number has been updated successfully.");
                                            Console.ReadKey();
                                            break;
                                        }
                                    }
                                }
                                break;
                        }
                        break;
                    }
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

    public static void AdminPage()
    {

    }
    static void Main(string[] args)
    {
        List<User> users = new List<User> { };
        Authentication auth = new Authentication(users);
        Department department1 = new Department("Pediatriya");
        Department department2 = new Department("Travmatologiya");
        Department department3 = new Department("Stamotologiya");
        List<Department> departments = new List<Department> { department1, department2, department3 };

        var receptionHour1 = new ReceptionHour("09:00", "11:00");
        var receptionHour2 = new ReceptionHour("12:00", "14:00");
        var receptionHour3 = new ReceptionHour("15:00", "17:00");

        var monday = new ReceptionDay(DayOfWeek.Monday);
        monday.AddTimeSlot(receptionHour1.start.ToString("hh\\:mm"), receptionHour1.end.ToString("hh\\:mm"));
        monday.AddTimeSlot(receptionHour2.start.ToString("hh\\:mm"), receptionHour2.end.ToString("hh\\:mm"));

        var mon1 = new ReceptionDay(DayOfWeek.Monday);
        mon1.AddTimeSlot(receptionHour1.start.ToString("hh\\:mm"), receptionHour1.end.ToString("hh\\:mm"));

        var tue1 = new ReceptionDay(DayOfWeek.Tuesday);
        tue1.AddTimeSlot(receptionHour2.start.ToString("hh\\:mm"), receptionHour2.end.ToString("hh\\:mm"));

        var wed1 = new ReceptionDay(DayOfWeek.Wednesday);
        wed1.AddTimeSlot(receptionHour3.start.ToString("hh\\:mm"), receptionHour3.end.ToString("hh\\:mm"));

        var doctor1 = new Doctor("John", "Smith", 8, department1);
        doctor1.AddReceptionDay(mon1);
        doctor1.AddReceptionDay(tue1);
        doctor1.AddReceptionDay(wed1);

        var mon2 = new ReceptionDay(DayOfWeek.Monday);
        mon2.AddTimeSlot(receptionHour2.start.ToString("hh\\:mm"), receptionHour2.end.ToString("hh\\:mm"));

        var thu2 = new ReceptionDay(DayOfWeek.Thursday);
        thu2.AddTimeSlot(receptionHour1.start.ToString("hh\\:mm"), receptionHour1.end.ToString("hh\\:mm"));

        var fri2 = new ReceptionDay(DayOfWeek.Friday);
        fri2.AddTimeSlot(receptionHour3.start.ToString("hh\\:mm"), receptionHour3.end.ToString("hh\\:mm"));
        var doctor2 = new Doctor("Emily", "Johnson", 5, department2);
        doctor2.AddReceptionDay(mon2);
        doctor2.AddReceptionDay(thu2);
        doctor2.AddReceptionDay(fri2);


        var mon3 = new ReceptionDay(DayOfWeek.Monday);
        mon3.AddTimeSlot(receptionHour3.start.ToString("hh\\:mm"), receptionHour3.end.ToString("hh\\:mm"));

        var wed3 = new ReceptionDay(DayOfWeek.Wednesday);
        wed3.AddTimeSlot(receptionHour1.start.ToString("hh\\:mm"), receptionHour1.end.ToString("hh\\:mm"));

        var fri3 = new ReceptionDay(DayOfWeek.Friday);
        fri3.AddTimeSlot(receptionHour2.start.ToString("hh\\:mm"), receptionHour2.end.ToString("hh\\:mm"));

        var doctor3 = new Doctor("Michael", "Brown", 7, department3);
        doctor3.AddReceptionDay(mon3);
        doctor3.AddReceptionDay(wed3);
        doctor3.AddReceptionDay(fri3);

        var doctor4 = new Doctor("Sarah", "Davis", 6, department1);
        doctor4.AddReceptionDay(mon1);
        doctor4.AddReceptionDay(thu2);

        var doctor5 = new Doctor("David", "Wilson", 9, department2);
        doctor5.AddReceptionDay(tue1);
        doctor5.AddReceptionDay(fri3);



        Doctor doctor6 = new Doctor("Benjamin", "Linus", 6, department3);
        Doctor doctor7 = new Doctor("John", "Carter", 9, department1);
        Doctor doctor8 = new Doctor("Allison", "Cameron", 5, department2);
        Doctor doctor9 = new Doctor("Derek", "Shepherd", 11, department3);
        Doctor doctor10 = new Doctor("Rachel", "Green", 4, department1);
        Doctor doctor11 = new Doctor("Steve", "Dent", 3, department2);

        User? user1 = new User("aya_aliye283", "ayan", "aliyeva", "ayan@gmail.com", "0707897878");
        users.Add(user1);
        Hosbital hosbital = new Hosbital(departments, users);


        while (true)
        {
            Console.Clear();

            Console.WriteLine("\n\t\t\t\t ~ Hosbital ~ \n\n");

            List<string> roles = new List<string> { "Admin", "User", "Doctor" };
            string title = $"\t\t\t\t\t   Welcome to the Hospital\n * Select your role to log in:\n";
            int choiceIndex = NavigateMenu(roles, title, true, "~ Exit");
            {
                if (choiceIndex == -1)
                    break;
                if (choiceIndex == 0)
                {
                    //adminpage
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
                                    UserMainMenu(departments, user, hosbital);
                                }
                            }
                            else if (ansIndex == 1)
                            {
                                User? user = Methods.RegistrUser(auth, departments, hosbital);
                                users.Add(user);
                                UserMainMenu(departments, user, hosbital);
                            }
                        }
                    }
                }

                else if (choiceIndex == 2)
                {
                    //doctor page
                }

            }
        }
    }
}
