using Hospital_Project.FileHelpers;
using Hospital_Project.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Project.Pages
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
            doctor.Appointments = FileHelper.ReadAppointmentsForUserOrDoctor(doctor.email, "doctor");
            user.Appointments = FileHelper.ReadAppointmentsForUserOrDoctor(user.email, "user");
            doctor.doctorsNotifications = FileHelper.ReadNotificationsFromFile(doctor.email);
            user.userNotifications = FileHelper.ReadNotificationsFromFile(user.email);

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            while (true)
            {
                Console.Clear();

                var ReceptionHourlist = doctor.receptionDays[dayIndex].TimeSlots;

                string title = $"\n ~ These are the office hours of Dr.{doctor.surname}.\n  Please select the time you would like to schedule an appointment:\n";
                int choiceIndex = Program.NavigateMenu(ReceptionHourlist, title, true);

                if (choiceIndex == -1)
                    break;

                if (!ReceptionHourlist[choiceIndex].isReserved)
                {
                    if (choiceIndex >= 0 && choiceIndex < ReceptionHourlist.Count)
                    {
                        var selectedSlot = ReceptionHourlist[choiceIndex];
                        selectedSlot.isReserved = true;
                        FileHelper.WriteReceptionDaysToFile(doctor.receptionDays, doctor.email);
                        string day = receptionDays[dayIndex].ToString();
                        string hour = selectedSlot.ToString(true);

                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.WriteLine($"\n ~ Thank you, {user.name} {user.surname}. You have successfully booked an appointment with Dr.{doctor.surname} on {day} at {hour}.");
                        Console.WriteLine("\n ~ Please wait for the email to be sent... Check your inbox after a few moments.");

                        var appointment = new Appointment(
                            userName: $"{user.name} {user.surname}",
                            doctorName: $"Dr.{doctor.name} {doctor.surname}",
                            userEmail: user.email,
                            doctorEmail: doctor.email,
                            department: doctor.department.departmentName,
                            day: day,
                            hour: hour
                        );

                        user.Appointments.Add(appointment);
                        doctor.Appointments.Add(appointment);
                        Log.Information("user booked an appointment with doctor {name}",doctor.name);

                        FileHelper.WriteAppointmentsToFile(user.Appointments);
                        FileHelper.WriteAppointmentsToFile(doctor.Appointments);

                        string subjectUser = "Appointment Confirmed – Hope Medical Center";
                        string shortMessageUser = $"You have scheduled an appointment with Dr.{doctor.name} on {day} at {hour}.";
                        string emailBodyUser = $"Dear {user.name},\n\n" +
                                               $"Your appointment has been successfully scheduled.\n\n" +
                                               $"Doctor: Dr.{doctor.name}\n" +
                                               $"Date & Time: {day} at {hour}\n" +
                                               $"Location: Hope Medical Center, Room 305\n\n" +
                                               $"Please arrive 10 minutes early.\n\n" +
                                               $"Stay healthy,\nHope Medical Center Team";

                        var userNotification = new Notification(subjectUser, shortMessageUser, emailBodyUser, user.email);
                        user.userNotifications.Add(userNotification);
                        FileHelper.WriteNotificationsToFile(user.userNotifications, user.email);

                        string subjectDoctor = "New Appointment Scheduled – Hope Medical Center";
                        string messageDoctor = $"Patient {user.name} has booked an appointment with you on {day} at {hour}.";
                        string emailBodyDoctor = $"Dear Dr.{doctor.name},\n\n" +
                                                 $"A new appointment has been scheduled.\n\n" +
                                                 $"Patient: {user.name} {user.surname}\n" +
                                                 $"Date & Time: {day} at {hour}\n" +
                                                 $"Location: Room 305\n\n" +
                                                 $"Please be ready 10 minutes in advance.\n\n" +
                                                 $"Regards,\nHope Medical Center Team";

                        var doctorNotification = new Notification(subjectDoctor, messageDoctor, emailBodyDoctor, doctor.email);
                        doctor.doctorsNotifications.Add(doctorNotification);
                        FileHelper.WriteNotificationsToFile(doctor.doctorsNotifications, doctor.email);
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.WriteLine(" ~ This time is already reserved. Please choose another slot.");
                    break;            
                }
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