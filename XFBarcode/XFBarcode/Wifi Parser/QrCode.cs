namespace WifiQR_Parser
{
    class QrCode
    {
        public static string GetQrCodeType(string code)
        {
            string codeType = "";
            foreach (char c in code)
            {
                if (c == ':')
                    break;
                codeType += c;
            }
            return codeType;
        }
    }
}
