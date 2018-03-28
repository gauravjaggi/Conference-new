using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;

namespace EventList.Droid.Activities
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/SplashTheme")]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
			var intent = new Intent(this, typeof(MainActivity));
			StartActivity(intent);
			Finish();
        }
		public override void OnBackPressed() { }
    }

}
