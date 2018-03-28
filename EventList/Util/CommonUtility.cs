using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EventList.Util
{
    public class CommonUtility
    {
        public static void VisibleNavigationBar(ContentPage page)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                NavigationPage.SetHasNavigationBar(page, true);
                NavigationPage.SetHasBackButton(page, true);
            }
            else
            {
                NavigationPage.SetHasNavigationBar(page, false);
            }
        }
    }
}
