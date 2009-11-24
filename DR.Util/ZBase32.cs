
using System;
using System.Text;

namespace DR.Util
{
    public static class ZBase32
    {
        private static String base32Chars = "ybndrfg8ejkmcpqxot1uwisza345h769";

        public static string Encode(byte[] bytes)
        {
            int i = 0, index = 0, digit = 0;
            int currByte, nextByte;
            StringBuilder base32 = new StringBuilder((bytes.Length + 7) * 8 / 5);
            
            while (i < bytes.Length)
            {
                currByte = (bytes[i] >= 0) ? bytes[i] : (bytes[i] + 256);
                // unsign
                /* Is the current digit going to span a byte boundary? */
                if (index > 3)
                {
                    if ((i + 1) < bytes.Length)
                    {
                        nextByte = (bytes[i + 1] >= 0) ? bytes[i + 1] : (bytes[i + 1] + 256);
                    }
                    else
                    {
                        nextByte = 0;
                    }
                    
                    digit = currByte & (0xff >> index);
                    index = (index + 5) % 8;
                    digit <<= index;
                    digit |= nextByte >> (8 - index);
                    i++;
                }
                else
                {
                    digit = (currByte >> (8 - (index + 5))) & 0x1f;
                    index = (index + 5) % 8;
                    if (index == 0)
                        i++;
                }
                base32.Append(base32Chars[digit]);
            }
            
            return base32.ToString();
        }

        public static byte[] Decode(String base32)
        {
            int i = 0;
            int index = 0;
            int digit = 0;
            int offset = 0;
            byte[] bytes = new byte[base32.Length * 5 / 8];
            
            for (i = 0,index = 0,offset = 0; i < base32.Length; i++)
            {
                digit = base32Chars.IndexOf(base32[i]);
                if (index <= 3)
                {
                    index = (index + 5) % 8;
                    if (index == 0)
                    {
                        bytes[offset] |= (byte)digit;
                        offset++;
                        if (offset >= bytes.Length)
                            break;
                    }
                    else
                    {
                        bytes[offset] |= (byte)(digit << (8 - index));
                    }
                }
                else
                {
                    index = (index + 5) % 8;
                    bytes[offset] |= (byte)(digit >> index);
                    offset++;
                    if (offset >= bytes.Length)
                    {
                        break;
                    }
                    bytes[offset] |= (byte)(digit << (8 - index));
                }
            }
            return bytes;
        }
    }
}
