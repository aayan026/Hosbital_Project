using Hosbital_Project.FileHelpers;
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Pages
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
                var ReceptionDays = FileHelper.ReadReceptionDaysFromFile(doctor.email);
                int choiceIndex = Program.NavigateMenu(ReceptionDays, $"\n ~ Dr.{doctor.surname}'s reception Days:\n", true);
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
                    var reserved = doctor.receptionDays[dayIndex].TimeSlots[choiceIndex];
                    reserved.isReserved = true;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine($"\n ~ Thank you, {user.name} {user.surname}.~\n ! You have successfully booked an appointment with Dr.{doctor.surname} at {receptionDays[dayIndex]} - {receptionDays[dayIndex].TimeSlots[choiceIndex].start} - {receptionDays[dayIndex].TimeSlots[choiceIndex].end}");

                    var selectedDay = receptionDays[dayIndex];
                    var selectedSlot = selectedDay.TimeSlots[choiceIndex];

                    user.Appointments.Add((doctor, selectedDay, selectedSlot));
                    doctor.Appointments.Add((user, selectedDay, selectedSlot));

                    //user notification
                    string ShortmessageUser = $"You have scheduled an appointment with Dr.{doctor.name}";
                    string emailSubject = "Appointment Confirmed – Hope Medical Center";

                    string emailBody = $"Dear, {user.name}\n\n" +
                                       "Your appointment has been successfully scheduled.\n\n" +
                                       "Details:\n" +
                                       $"- Doctor: Dr.{doctor.name}\n" +
                                       $"- Date & Time: {selectedDay} at {selectedSlot.ToString(true)}\n" +
                                       "- Location: Hope Medical Center, Room 305\n\n" +
                                       "Please arrive 10 minutes early. To reschedule, use your patient panel or contact us directly.\n\n" +
                                       "Stay healthy,\n" +
                                       "Hope Medical Center Team";

                    Console.WriteLine("\n ~ Please wait for the email to be sent... Check your email after the operation is completed.");
                    Notification notification = new Notification(emailSubject, ShortmessageUser, emailBody, user.email);
                    user.userNotifications.Add(notification);
                    FileHelper.WriteNotificationsToFile(user.userNotifications, user.email); 
                    //fayla yaz

                    //doctor notification
                    string body = $"Dear {doctor.name},\n\n" +
                                  "A new appointment has been scheduled.\n\n" +
                                  "Details:\n" +
                                  $"- Patient: {user.name}\n" +
                                  $"- Date & Time: {selectedDay} at {selectedSlot.ToString(true)}\n" +
                                  "- Location: Room 305\n\n" +
                                  "Please be ready 10 minutes in advance.\n\n" +
                                  "Regards,\nHope Medical Center Team";

                    string messageDoctor = $"Patient {user.name} has booked an appointment with you on {selectedDay}";
                    Notification notification2 = new Notification(emailSubject, messageDoctor, body, user.email);
                    if (doctor.doctorsNotifications == null)
                        doctor.doctorsNotifications = new List<Notification>();

                    doctor.doctorsNotifications.Add(notification2);
                    FileHelper.WriteNotificationsToFile(doctor.doctorsNotifications, doctor.email);

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
                {
                    ReserveDay(department.doctors[choiceIndex], user);
                    return;
                }
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
                    ChoiceDoctor(departments[navigator], user);
                }
            }
        }

    }
}