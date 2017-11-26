//  
//  Copyright (c) 2009-2010 by Johann Duscher (alias Jonny Dee)
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using System.Text;

namespace DR
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
