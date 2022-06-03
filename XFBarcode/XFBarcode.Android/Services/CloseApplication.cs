using Android.App;
using Xamarin.Forms;
using XFBarcode.Services;

[assembly: Dependency(typeof(XFBarcode.Droid.Services.CloseApplication))]
namespace XFBarcode.Droid.Services
{
    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}