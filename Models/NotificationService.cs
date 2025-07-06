
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal class NotificationService
    {
        public static void SendEmail(string title, string message, string toEmail)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("aliyevanar1986a@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = title;
            mail.Body = message;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("aliyevanar1986a@gmail.com", "yrqk qchh xknp fqcs");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" ~ Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
