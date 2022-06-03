using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WifiQR_Parser;
using Xamarin.Forms;
using XFBarcode.Services;

namespace XFBarcode
{
    public partial class MainPage : ContentPage
    {
        string qrCodeStr = "";
        WifiQRInfo wifiQRInfo { get; set; }

        public MainPage()
        {
            Task.Run(async () =>
            {
                qrCodeStr = await StartScanningAsync();
                if (!string.IsNullOrEmpty(qrCodeStr))
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                    {
                        WifiQR wifiQR = new WifiQR();
                        wifiQRInfo = wifiQR.Parse(qrCodeStr);
                        networkName_lbl.Text = wifiQRInfo.SSID;
                        security_lbl.Text = wifiQRInfo.Authentication;
                    });
            });

            InitializeComponent();
            logo.Source = ImageSource.FromResource("XFBarcode.Images.wifi_router_3.png");

        }

        //override on

        private void btnScan_Clicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                qrCodeStr = await StartScanningAsync();
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    WifiQR wifiQR = new WifiQR();
                    wifiQRInfo = wifiQR.Parse(qrCodeStr);
                    networkName_lbl.Text = wifiQRInfo.SSID;
                    security_lbl.Text = wifiQRInfo.Authentication;
                });
            });
        }

        private async Task<string> StartScanningAsync()
        {
            // Scan till get any result.
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                return result ?? "";
            }
            catch (Exception)
            {
                // Write exception to a log file.

                DependencyService.Get<ICloseApplication>().closeApplication();
                return "";
            }
        }

        private void ConnectBtn_Clicked(object sender, EventArgs e)
        {
            if (qrCodeStr != "" && !wifiQRInfo.IsEmpty())
                DependencyService.Get<IWifiConnectingService>().ConnectAsync(wifiQRInfo);
        }
    }
}
