using EventList.Droid.Renderers;
using EventList.Pages.Chat;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ChatRoomPage), typeof(ChatPageRenderer))]
namespace EventList.Droid.Renderers
{
    public class ChatPageRenderer:PageRenderer
    {
        public ChatPageRenderer()
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
        }
    }
}
