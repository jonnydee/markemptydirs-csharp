//  
//  Copyright (c) 2009 by Johann Duscher (alias Jonny Dee)
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

namespace DR
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
