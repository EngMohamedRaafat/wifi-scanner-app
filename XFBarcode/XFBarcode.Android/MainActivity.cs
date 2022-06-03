using Android.App;
using Android.Content.PM;
using Android.OS;
using ZXing.Mobile;

namespace XFBarcode.Droid
{
    [Activity(Label = "Wifi Scanner", Icon = "@drawable/Wifi_logo", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            MobileBarcodeScanner.Initialize(this.Application);

            LoadApplication(new App());
        }
    }
}

