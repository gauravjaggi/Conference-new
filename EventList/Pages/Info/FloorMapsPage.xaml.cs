﻿using System;
using System.Collections.Generic;
using EventList.Models;
using Xamarin.Forms;
namespace EventList.Pages.Info
{
    public partial class FloorMapsPage : ContentPage
    {
        public FloorMapsPage(ConferenceFloor floormap)
        {
			InitializeComponent();

            CarouselMaps.ItemsSource = new List<EvolveMap>
			{
				new EvolveMap
				{
					Local = "floor_1.png",
                    Url = floormap.Image1,//"https://s3.amazonaws.com/xamarin-releases/evolve-2016/floor_1.png",
					Title = "Floor Maps (1/2)"
				},
				new EvolveMap
				{
					Local = "floor_2.png",
                    Url =floormap.Image2,// "https://s3.amazonaws.com/xamarin-releases/evolve-2016/floor_2.png",
					Title = "Floor Maps (2/2)"
				}
			};


			if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
			{

				Title = "Floor Maps (1/2)";
				CarouselMaps.ItemSelected += (sender, args) =>
				{
					var current = args.SelectedItem as EvolveMap;
					if (current == null)
						return;
					Title = current.Title;
				};
			}
		}
	}
}