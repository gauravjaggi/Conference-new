using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EventList.Pages.Info
{
    public partial class EvaluationPage : ContentPage
    {
        EvaluationViewModel vm;
        public EvaluationPage()
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

            BindingContext = vm = new EvaluationViewModel(Navigation);
        }
		public void OnSubmitClicked(object sender, EventArgs e)
		{
			vm.ContentRating = Convert.ToInt32(lblContentRating.Text);
			vm.PresentationRating = Convert.ToInt32(lblPresRating.Text);
			vm.SkillRating = Convert.ToInt32(lblskillRating.Text);
			vm.WorkShopRating = Convert.ToInt32(lblworkRating.Text);
            vm.EvaluateCommand.Execute(null);
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
