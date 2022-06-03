using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using XFBarcode.Services;

namespace WifiQR_Parser
{
    public class WifiQR
    {
        public enum WifiType
        {
            None,
            Wifi,
            EncWifi,
        }

        WifiType wifiType;

        private bool IsWifiCode(string qrCode, out string resultedCode)
        {
            if (string.IsNullOrEmpty(qrCode))
            {
                resultedCode = "";
                return false;
            }
            string codeType = QrCode.GetQrCodeType(qrCode).ToUpper();
            resultedCode = qrCode.Substring(codeType.Length + 1, qrCode.Length - codeType.Length - 1);
            switch (codeType)
            {
                case "WIFI":
                    wifiType = WifiType.Wifi;
                    break;
                case "ENCWIFI":
                    wifiType = WifiType.EncWifi;
                    break;
                default:
                    wifiType = WifiType.None;
                    return false;
            }
            return true;
        }

        private string getSharedKey(string password, int keyLength)
        {
            int pLen = password.Length;
            return password.Substring(pLen - keyLength, keyLength);
        }

        private Dictionary<string, string> Split(string code)
        {
            string testcode = code.Substring(0, code.Length - 2);
            int length = code.Length - 2;
            bool isKey = false;

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("T", "");
            result.Add("S", "");
            result.Add("P", "");

            string strItem = "";
            string key = "", value = "";

            for (int i = length - 1; i >= 0; i--)
            {
                if (code[i] == ';' && isKey)
                {
                    key = Reverse(strItem);
                    if (result.ContainsKey(strItem))
                    {
                        result[strItem] = Reverse(value);
                        key = value = strItem = "";
                    }
                    else
                    {
                        if (key.Contains(":"))
                        {
                            string[] s = key.Split(':');
                            if (result.ContainsKey(s[0]))
                            {
                                for (int j = 1; j < s.Length; j++)
                                    result[s[0]] += s[j] + ':';
                                result[s[0]] += Reverse(value);
                                key = value = strItem = "";
                            }
                        }
                    }
                    isKey = false;
                    continue;
                }
                else if (code[i] == ':' && !isKey)
                {
                    value += strItem;
                    isKey = true;
                    strItem = "";
                    continue;
                }
                strItem += code[i];
            }

            if (isKey && strItem != "")
            {
                key = Reverse(strItem);
                if (result.ContainsKey(strItem))
                    result[strItem] = Reverse(value);
            }

            // Decrypting Encrypted Wifi password
            if (wifiType == WifiType.EncWifi)
            {
                string sharedKey = getSharedKey(result["P"], 7);
                result["P"] = result["P"].Substring(0, result["P"].Length - 7);
                string encP = DependencyService.Get<ISecurity>().DecryptAES(result["P"], sharedKey);
                result["P"] = encP;
            }
            return result;
        }

        private string Reverse(string str)
        {
            TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(str);

            List<string> elements = new List<string>();
            while (enumerator.MoveNext())
                elements.Add(enumerator.GetTextElement());

            elements.Reverse();
            return string.Concat(elements);
        }

        public WifiQRInfo Parse(string code)
        {
            if (IsWifiCode(code, out string result))
            {
                Dictionary<string, string> wifiInfoDictionary = Split(result);

                WifiQRInfo wifiQRInfo = new WifiQRInfo();
                wifiQRInfo = (WifiQRInfo)wifiInfoDictionary;
                return wifiQRInfo;
            }
            return new WifiQRInfo();
            //throw new System.Exception("Input code of string was not in a correct Wifi format.");
        }

        public static string GenerateQR(WifiQRInfo info, bool IsEncrypted = false)
        {
            string code = (IsEncrypted) ? "ENCWIFI:" : "WIFI:";
            if (info.Authentication != "")
                code += "T:" + info.Authentication;
            if (string.IsNullOrEmpty(info.SSID))
                throw new System.ArgumentNullException("WifiQRInfo.SSID");
            else
                code += ((info.Authentication == "") ? "S:" : ";S:") + info.SSID;
            if (info.Password != "")
            {
                code += ';';
                if (IsEncrypted)
                {
                    ISecurity Security = DependencyService.Get<ISecurity>();
                    string SharedKey = Security.GenerateSharedKey(7);
                    code += "P:" + Security.EncryptAES(info.Password, SharedKey) + SharedKey;
                }
                else
                    code += "P:" + info.Password;
            }
            return code + ";;";
        }
    }
}
