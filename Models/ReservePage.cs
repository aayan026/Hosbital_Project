using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class ReservePage
    {
        internal static void ReserveDay(Doctor doctor, User user)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                var ReceptionDays = doctor.receptionDays;
                int choiceIndex = Program.NavigateMenu(ReceptionDays, $"\n ~ Dr.{doctor.surname}'s reception hours:\n", true);


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
        internal static void ReserveHour(Doctor doctor, User user, int dayIndex, List<ReceptionDay> receptionDays)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            var ReceptionHourlist = doctor.receptionDays[dayIndex].TimeSlots;

            string title = $"\n ~ These are the office hours of Dr.{doctor.surname}.\n  Please select the days you would like to schedule an appointment.\n";
            var choiceIndex = Program.NavigateMenu(ReceptionHourlist, title, true);

            if (choiceIndex == -1)
            {
                return;
            }
            if (!ReceptionHourlist[choiceIndex].isReserved)
            {
                if (choiceIndex >= 0 && choiceIndex <= ReceptionHourlist.Count)
                {
                    doctor.receptionSchedule.ReserveHour(dayIndex, choiceIndex);
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"\n ~ Thank you, {user.name} {user.surname}.~\n ! You have successfully booked an appointment with Dr.{doctor.surname} at {receptionDays[dayIndex]} - {receptionDays[dayIndex].TimeSlots[choiceIndex].start.ToString("hh\\:mm")} - {receptionDays[dayIndex].TimeSlots[choiceIndex].end.ToString("hh\\:mm")}");
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
        internal static void ChoiceDoctor(Department department, User user)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                int choiceIndex = Program.NavigateMenu(department.doctors, "\n Doctors", true);
                if (choiceIndex == -1)
                    return;
                if (choiceIndex >= 0 && choiceIndex <= department.doctors.Count)
                    ReservePage.ReserveDay(department.doctors[choiceIndex], user);
            }
        }
        internal static void Departments(List<Department> departments, User user)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                int navigator = Program.NavigateMenu(departments, "\n  Departments", true);
                if (navigator == -1)
                {
                    break;
                }
                if (navigator >= 0 && navigator < departments.Count)
                {
                    ReservePage.ChoiceDoctor(departments[navigator], user);
                }
            }
        }

    }
}
