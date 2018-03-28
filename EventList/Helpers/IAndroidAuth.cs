using System;
using Xamarin.Forms;

namespace EventList
{
    public interface IAndroidAuth
    {
        ContentPage context { get; set; }
        void Login();
        void Logout();
    }
}
