using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hosbital_Project.Models
{
    internal interface IProfile
    {
        void ViewProfile(string title);
    }
    internal interface IViewAppointmets
    {
        void ViewAppointments();
    }
    internal interface INotification
    {
        void ViewNotifications();
    }
}
