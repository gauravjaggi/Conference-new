using System;
using Xamarin.Forms;

namespace EventList.Helpers
{
    public interface IGoogleService
    {
        ContentPage context { get; set; }
        void Logout();
    }
}
