using System;
using Xamarin.Forms;

namespace EventList
{
    public class ScheduleTemplateSelector:DataTemplateSelector
    {
		public DataTemplate SessionTemplate { get; set; }

		public DataTemplate EventTemplate { get; set; }

        public ScheduleTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
			ScheduleItemViewModel model = (ScheduleItemViewModel)item;

			DataTemplate selectedtemplate = null;

            if (model.ScheduledSession == null)
				selectedtemplate = EventTemplate;
            if (model.ScheduledEvent == null)
				selectedtemplate = SessionTemplate;
			return selectedtemplate;
           
        }
    }
}
