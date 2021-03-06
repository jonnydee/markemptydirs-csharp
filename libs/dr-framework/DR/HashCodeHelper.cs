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

using System.Collections;

namespace DR
{
    public static class HashCodeHelper
    {
        public static int GetHashCode(params object[] objects)
        {
            return GetHashCode((IEnumerable)objects);
        }
        
        public static int GetHashCode(IEnumerable objects)
        {
            var hashCode = 0;
            foreach (var o in objects)
                if (null != o)
                    hashCode ^= o.GetHashCode();
            return hashCode;
        }
    }
}
