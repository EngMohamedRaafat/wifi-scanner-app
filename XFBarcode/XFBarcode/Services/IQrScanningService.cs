using System.Threading.Tasks;

namespace XFBarcode.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
