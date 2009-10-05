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
using System.IO;

namespace DJ.Util
{
    [Serializable]
    public struct Pair<A,B>
    {
        public Pair(A first, B second)
        {
            First = first;
            Second = second;
        }
        
        public A First { get; private set; }
        public B Second { get; private set; }

        public override string ToString ()
        {
            return string.Format("({0}, {1})", First, Second);
        }

        public override bool Equals (object obj)
        {
            if (null == obj || !(obj is Pair<A,B>))
                return false;
            var other = (Pair<A,B>)obj;
            return Equals(First, other.First) && Equals(Second, other.Second);
        }

    }
}
