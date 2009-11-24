
using System;
using System.Text;

namespace DJ.Util
{
    public static class Base16
    {
        public static string Encode(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                //Returns in lower case.
                //To return upper case change “x2″ to “X2″
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        
        public static byte[] Decode(string base16)
        {
            throw new NotImplementedException("Decode");
        }
    }
}
