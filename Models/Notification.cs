
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Project.Models
{
    internal class Notification
    {
        public string title { get; set; }
        public string message { get; set; }
        public string toEmail { get; set; }
        public string LongMessage { get; set; }
        public Notification(string title, string message, string Longmessage, string toEmail)
        {
            this.title = title;
            this.message = message;
            this.toEmail = toEmail;
            this.LongMessage = Longmessage;
            NotificationService.SendEmail(title, LongMessage, toEmail);

        }
        public Notification() { }
        public override string ToString() => $" - {message}";

    }

}
