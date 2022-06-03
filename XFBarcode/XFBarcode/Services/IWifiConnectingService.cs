using System.Threading.Tasks;
using WifiQR_Parser;

namespace XFBarcode.Services
{
    public interface IWifiConnectingService
    {
        void ConnectAsync(WifiQRInfo wifiQRInfo);
    }
}
