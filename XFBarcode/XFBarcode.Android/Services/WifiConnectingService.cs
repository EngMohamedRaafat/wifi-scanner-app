using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.Widget;
using WifiQR_Parser;
using Xamarin.Forms;
using XFBarcode.Services;
using Application = Android.App.Application;

[assembly: Dependency(typeof(XFBarcode.Droid.Services.WifiConnectingService))]
namespace XFBarcode.Droid.Services
{
    class WifiConnectingService : IWifiConnectingService
    {
        public async void ConnectAsync(WifiQRInfo wifiQRInfo)
        {
            if (!wifiQRInfo.IsEmpty())
            {
                WifiConfiguration wifiConfig = new WifiConfiguration
                {
                    Ssid = string.Format("\"{0}\"", wifiQRInfo.SSID),
                    PreSharedKey = string.Format("\"{0}\"", wifiQRInfo.Password)
                };

                WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Android.Content.Context.WifiService);

                Show_Dialog msg = new Show_Dialog((Android.App.Activity)Forms.Context);

                if (!wifiManager.IsWifiEnabled)
                {
                    if (await msg.ShowDialog("Information", "Do you want to turn wifi on?", true, false, Show_Dialog.MessageResult.YES, Show_Dialog.MessageResult.NO) == Show_Dialog.MessageResult.YES)
                    {
                        wifiManager.SetWifiEnabled(true);

                        Toast.MakeText(Application.Context, await Try2Connect(wifiManager, wifiConfig), ToastLength.Long).Show();
                    }
                    else
                        Toast.MakeText(Application.Context, "Turn wifi on then, Try again!", ToastLength.Long).Show();
                }
                else
                    Toast.MakeText(Application.Context, await Try2Connect(wifiManager, wifiConfig), ToastLength.Long).Show();
            }
        }

        private async System.Threading.Tasks.Task<string> Try2Connect(WifiManager wifiManager, WifiConfiguration wifiConfig)
        {
            // Use ID
            int netId = wifiManager.AddNetwork(wifiConfig);
            bool x = wifiManager.Disconnect();
            bool y = wifiManager.EnableNetwork(netId, true);
            bool z = wifiManager.Reconnect();

            await System.Threading.Tasks.Task.Delay(2000);

            if (IsNetworkReachable())
                return "Connected to " + wifiConfig.Ssid;
            else
                return "Connection Failed!";

        }

        private bool IsNetworkReachable()
        {
            bool isNetworkActive;// = false;  
            Context context = Forms.Context; //get the current application context  

            ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo activeConnection = cm.ActiveNetworkInfo;
            isNetworkActive = (activeConnection != null) && activeConnection.IsConnected && activeConnection.Type == ConnectivityType.Wifi;

            return isNetworkActive;
        }
    }
}