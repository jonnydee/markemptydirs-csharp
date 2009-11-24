
using System;

namespace DR.Util
{
    public static class ModifiedBase64
    {
        public static string Encode(byte[] bytes)
        {
            var base64 = Convert.ToBase64String(bytes);
            var modifiedBase64 = base64.Replace("/", "-").Replace("+", "_");
            return modifiedBase64;
        }
        
        public static byte[] Decode(string modifiedBase64)
        {
            var base64 = modifiedBase64.Replace("-", "/").Replace("_", "+");
            return Convert.FromBase64String(base64);
        }
    }
}
