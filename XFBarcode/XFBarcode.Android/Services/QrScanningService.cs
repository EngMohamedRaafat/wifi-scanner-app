using System.Threading.Tasks;
using XFBarcode.Services;
using ZXing.Mobile;
using Xamarin.Forms;

[assembly: Dependency(typeof(XFBarcode.Droid.Services.QrScanningService))]

namespace XFBarcode.Droid.Services
{
    public class QrScanningService : IQrScanningService
    {
        public async Task<string> ScanAsync()
        {
            var optionsDefault = new MobileBarcodeScanningOptions();
            var optionsCustom = new MobileBarcodeScanningOptions();

            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Hold your phone up to the code",
                BottomText = "Scanning will happen automatically",
            };

            //optionsCustom.UseFrontCameraIfAvailable = true;
            //
            //string option = (scanner.IsTorchOn) ? "off" : "on";
            //scanner.FlashButtonText = "Turn Flash" + option;
            //
            var scanResult = await scanner.Scan(optionsCustom);
            return scanResult.Text;
        }
    }
}