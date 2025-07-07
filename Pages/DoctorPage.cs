
using Hosbital_Project.FileHelpers;
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Pages
{
    public class DoctorPage
    {
        internal static void DoctorPaGe(Hosbital hosbital, Authentication auth, Doctor doctor)
        {
            doctor.doctorsNotifications = FileHelpers.FileHelper.ReadNotificationsFromFile(doctor.email);
            doctor.Appointments = FileHelpers.FileHelper.ReadAppointmentsForUserOrDoctor(doctor.email, "doctor");

            var users= FileHelpers.FileHelper.ReadUsersFromFile();
            while (true)
            {
                string title = $"\n\t\t\t\t\t --- W E L C O M E ---";
                int choiceIndex = Program.NavigateMenu(new List<string> { "View Appointments", "Notifications", "View Profile", "Cancel Appointments", "Change Profile" }, title, true, "~ Logout");
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
                        if (doctor.doctorsNotifications.Count==0)
                        {
                            Console.WriteLine("\n You don't have any notifications.");
                            Console.ReadKey();
                        }
                        else
                        {
                            doctor.ViewNotifications();
                            Console.ReadKey();
                            int choiceindex = Program.NavigateMenu(new List<string> { "Delete Notification", "Back" }, "\n ~ Want to clear notifications?s");
                            if (choiceindex == 0)
                            {
                                doctor.doctorsNotifications.Clear();
                                FileHelper.WriteNotificationsToFile(new List<Notification>(), doctor.email);

                            }
                            else if (choiceindex == 1)
                            {
                            }
                            Console.ReadKey();
                            break;
                        }
                        break;
                    case 2:
                        Console.Clear();
                        hosbital.ProfileInfo("Doctor Profile", doctor);
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.Clear();

                        if (doctor.Appointments.Count == 0)
                        {
                            Console.WriteLine("\n You don't have any appointments.");
                            Console.ReadKey();
                            break;
                        }

                        int cancelIndex = Program.NavigateMenu(
                            doctor.Appointments.Select(ds => $" {ds.UserName} - {ds.Day} - {ds.Hour} ").ToList(),
                            "Select an appointment to cancel",
                            true
                        );

                        if (cancelIndex == -1)
                            break;

                        var appointment = doctor.Appointments[cancelIndex];

                        Console.Write("\n ~ Appointment cancelled successfully");

                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        string messageDoctor = $"You cancelled your meeting with patient {appointment.UserName}";
                        Console.WriteLine("\n ~ Please wait for the email to be sent...");
                        Console.ResetColor();

                        string subject = "Appointment Cancellation Notice – Hope Medical Center";

                        string emailBodyDoctor = $"Dear Dr.{doctor.name},\n\n" +
                                                 $"The appointment with patient {appointment.UserName}, scheduled on {appointment.Day} at {appointment.Hour} has been cancelled.\n\n" +
                                                 "Please update your schedule accordingly.\n\n" +
                                                 "Regards,\nHope Medical Center Team";

                        Notification notificationDoctor = new Notification(subject, messageDoctor, emailBodyDoctor, doctor.email);
                        doctor.doctorsNotifications.Add(notificationDoctor);
                        FileHelpers.FileHelper.WriteNotificationsToFile(doctor.doctorsNotifications, doctor.email);

                        string messageUser = $"Dr.{doctor.name} has cancelled an appointment with you.";
                        string emailBodyUser = $"Dear {appointment.UserName},\n\n" +
                                               $"We would like to inform you that your appointment with Dr.{doctor.name} scheduled on {appointment.Day} at {appointment.Hour} has been cancelled.\n\n" +
                                               "If this cancellation was not intended, please contact us or rebook through your patient panel.\n\n" +
                                               "We apologize for any inconvenience.\n\n" +
                                               "Stay safe,\nHope Medical Center Team";

                        Notification notificationUser = new Notification(subject, messageUser, emailBodyUser, appointment.UserEmail);

                        User? targetUser = users.FirstOrDefault(u => u.email == appointment.UserEmail);
                        if (targetUser != null)
                        {
                            targetUser.userNotifications ??= new List<Notification>();
                            targetUser.userNotifications.Add(notificationUser);

                            FileHelpers.FileHelper.WriteNotificationsToFile(targetUser.userNotifications, targetUser.email);

                            var userAppointment = targetUser.Appointments.FirstOrDefault(a =>
                                a.UserEmail == appointment.UserEmail &&
                                a.DoctorEmail == appointment.DoctorEmail &&
                                a.Day == appointment.Day &&
                                a.Hour == appointment.Hour
                            );
                            if (userAppointment != null)
                                targetUser.Appointments.Remove(userAppointment);

                            FileHelpers.FileHelper.WriteAppointmentsToFile(targetUser.Appointments);
                        }
                        doctor.Appointments.RemoveAt(cancelIndex);
                        FileHelpers.FileHelper.RemoveAppointmentFromFile(appointment);


                        DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), appointment.Day);
                        var receptionDay = doctor.receptionDays.Find(rd => rd.dayOfWeek == day);
                        var hour = receptionDay.TimeSlots.FirstOrDefault(ts => ts.ToString(true) == appointment.Hour);
                        hour.isReserved = false;
                        FileHelper.WriteReceptionDaysToFile(doctor.receptionDays, doctor.email);

                        Console.ReadKey();
                        break;
                    case 4:
                        while (true)
                        {
                            Console.Clear();
                            List<string> changeOptions = new List<string> { "Change Email", "Change Password", "Change Phone Number" };
                            int selectedIndex = Program.NavigateMenu(changeOptions, title, true);

                            if (selectedIndex == -1)
                                break;
                            if (selectedIndex == 0)
                            {
                                ChangeInformations.ChangeEmail(doctor, hosbital);
                                continue;
                            }
                            else if (selectedIndex == 1)
                            {
                                ChangeInformations.ChangePassword(doctor);
                                continue;
                            }
                            else if (selectedIndex == 2)
                            {
                                ChangeInformations.ChangePhone(doctor, hosbital);
                                continue;
                            }
                        }
                        continue;
                }
            }
        }
    }
}
