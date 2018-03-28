using System;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList
{
    public class FavoriteDataTemplateSelector:DataTemplateSelector
    {
		public DataTemplate SessionTemplate { get; set; }

		public DataTemplate EventTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            FavoriteItemsViewModel model = (FavoriteItemsViewModel)item;

            DataTemplate selectedtemplate = null;

            if (model.FavoriteSession == null)
                selectedtemplate = EventTemplate;
            if (model.FavoriteEvent == null)
				selectedtemplate = SessionTemplate;
            return selectedtemplate;
        }

    }
}
