﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace EventList.Pages.Windows
{
    public partial class MenuPageUWP : ContentPage
    {
        public MenuPageUWP()
        {
            InitializeComponent();
        }
        public ListView MenuList => ListViewMenu;
    }
}
