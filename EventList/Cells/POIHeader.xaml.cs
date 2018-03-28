using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EventList
{
    public class POIGroupHeader : ViewCell
    {
        public POIGroupHeader()
        {
            View = new POIHeader();
        }
    }
    public partial class POIHeader : ContentView
    {
        public POIHeader()
        {
            InitializeComponent();
        }
    }
}
