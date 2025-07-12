using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Project.Base
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
