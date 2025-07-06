
using Hosbital_Project.FileHelpers;
using Hosbital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Pages
{
    public class DoctorPage
    {
        internal static void DoctorPaGe(Hosbital hosbital, Authentication auth, Doctor doctor)
        {
            doctor.doctorsNotifications = FileHelpers.FileHelper.ReadNotificationsFromFile(doctor.email);
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
                        else
                        {
                            int cancelIndex = Program.NavigateMenu(doctor.Appointments.Select(ds => $" {ds.user.name} {ds.user.surname} - {ds.receptionDay} - {ds.receptionHour.start} - {ds.receptionHour.end} ").ToList(), "Select an appointment to cancel", true);
                            if (cancelIndex == -1)
                                break;
                            Console.Write("\n ~ Appointment cancelled succesfully");

                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            string messagedoctor = $"You cancelled your meeting with patient {doctor.Appointments[cancelIndex].user.name}";
                            Console.WriteLine("\n ~ Please wait for the email to be sent...");
                            Console.ResetColor();

                            string subject = "Appointment Cancellation Notice – Hope Medical Center";

                            string emailbody = $"Dear Dr.{doctor.name},\n\n" +
                                          $"The appointment with patient {doctor.Appointments[cancelIndex].user.name}, scheduled on {doctor.Appointments[cancelIndex].receptionDay} at {doctor.Appointments[cancelIndex].receptionHour.ToString(true)} has been cancelled.\n\n" +
                                          "Please update your schedule accordingly.\n\n" +
                                          "Regards,\nHope Medical Center Team";

                            Notification notification = new Notification(subject, messagedoctor, emailbody, doctor.email);
                            doctor.doctorsNotifications.Add(notification);
                            FileHelpers.FileHelper.WriteNotificationsToFile(doctor.doctorsNotifications,doctor.email);

                            string messageuser = $" Dr.{doctor.name} has cancelled an appointment with you.";
                            string body = $"Dear {doctor.Appointments[cancelIndex].user.name},\n\n" +
                                          $"We would like to inform you that your appointment with Dr.{doctor.name} scheduled on {doctor.Appointments[cancelIndex].receptionDay} at {doctor.Appointments[cancelIndex].receptionHour.ToString(true)} has been cancelled.\n\n" +
                                          "If this cancellation was not intended, please contact us or rebook through your patient panel.\n\n" +
                                          "We apologize for any inconvenience.\n\n" +
                                          "Stay safe,\nHope Medical Center Team";

                            Notification notification2 = new Notification(subject, messageuser, body, doctor.Appointments[cancelIndex].user.email);

                            doctor.Appointments[cancelIndex].user.userNotifications.Add(notification2);
                            FileHelpers.FileHelper.WriteNotificationsToFile(doctor.Appointments[cancelIndex].user.userNotifications, doctor.Appointments[cancelIndex].user.email);

                            doctor.Appointments[cancelIndex].user.Appointments.RemoveAt(cancelIndex);
                            doctor.Appointments.RemoveAt(cancelIndex);
                            Console.ReadKey();
                            break;
                        }

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
