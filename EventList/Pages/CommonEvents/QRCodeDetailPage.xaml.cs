using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.CommonEvents
{
    public partial class QRCodeDetailPage : ContentPage
    {
        public QRCodeDetailPage(string code)
        {
            InitializeComponent();
            StackLayout layout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(10),
                Children ={new Label(){
                        Text=code,
                        FontSize=15,
                        TextColor=Color.FromHex("#000000"),
                        HorizontalOptions=LayoutOptions.StartAndExpand,
                        VerticalOptions=LayoutOptions.StartAndExpand
                    }}
            };
            Content = layout;

        }
    }
}
