using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventList.Helpers;
using EventList.iOS;
using EventList.iOS.Services;
using EventList.Models;
using Google.SignIn;
using Xamarin.Forms;

[assembly: Dependency(typeof(GoogleService))]
namespace EventList.iOS.Services
{
    public class GoogleService:IGoogleService
    {
        public ContentPage context {get; set; }

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
            NavigateBack();
        }

        public async void NavigateBack()
        {
            await context.Navigation.PopAsync(true);
        }
    }
}
