using System.Collections.Generic;

namespace WifiQR_Parser
{
    public class WifiQRInfo
    {
        public string Authentication { get; set; }
        public string SSID { get; set; }
        public string Password { get; set; }

        public WifiQRInfo()
        {
            Authentication = "";
            SSID = "";
            Password = "";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(SSID) && string.IsNullOrEmpty(Password);
        }

        public static explicit operator WifiQRInfo(Dictionary<string, string> v)
        {
            if (v == null && v.Count == 0)
                return new WifiQRInfo();
            return new WifiQRInfo()
            {
                Authentication = v["T"],
                SSID = v["S"],
                Password = v["P"]
            };
        }
    }
}
