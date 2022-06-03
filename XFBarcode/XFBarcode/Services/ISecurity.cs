using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFBarcode.Services
{
    public interface ISecurity
    {
        string GenerateSharedKey(int length);
        string EncryptAES(string plainText, string sharedSecret);
        string DecryptAES(string cipherText, string sharedSecret);
    }
}
