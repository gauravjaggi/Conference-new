using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.Feedback
{
    public partial class RatingPage : ContentPage
    {
        FeedbackViewModel vm;
        public RatingPage()
        {

        }
        public RatingPage(Session session, FeaturedEvent featuredevent, bool isevent)
        {
            InitializeComponent();
            BindingContext = vm = new FeedbackViewModel(Navigation, session, featuredevent,isevent);
        }
        public void OnSubmitClicked(object sender, EventArgs e)
        {
            vm.ContentRating =Convert.ToInt32(lblContentRating.Text);
            vm.PresentationRating = Convert.ToInt32(lblPresRating.Text);
            vm.SkillRating = Convert.ToInt32(lblskillRating.Text);
            vm.WorkShopRating = Convert.ToInt32(lblworkRating.Text);
            vm.SubmitRatingCommand.Execute(null);
        }
		public void OnRelevantClicked(object sender, EventArgs e)
		{
			vm.IsContentRelevenant = !vm.IsContentRelevenant;
			vm.RaisePropertyChanged("ContentRelevanceIS");

		}
        public void OnRecommendedClicked(object sender, EventArgs e)
        {
            vm.IsRecommended = !vm.IsRecommended;
            vm.RaisePropertyChanged("RecommendedIS");

        }
    }
}
