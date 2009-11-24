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
using System.Collections;

namespace DR
{
    [Serializable]
    public struct Tuple4<A,B,C,D> : IEnumerable
    {
        private readonly object[] _elements;
        
        public Tuple4(A first, B second, C third, D fourth)
        {
            _elements = new object[] { first, second, third, fourth };
        }
        
        public A First { get { return (A)_elements[0]; } }
        public B Second { get { return (B)_elements[1]; } }
        public C Third { get { return (C)_elements[2]; } }
        public D Fourth { get { return (D)_elements[3]; } }

        object this[int index] { get { return _elements[index]; } }
        
        public override string ToString ()
        {
            return string.Format("({0}, {1}, {2}, {3})", First, Second, Third, Fourth);
        }

        public override bool Equals (object obj)
        {
            if (null == obj || !(obj is Tuple4<A,B,C,D>))
                return false;
            var other = (Tuple4<A,B,C,D>)obj;
            return Equals(First, other.First) 
                && Equals(Second, other.Second) 
                && Equals(Third, other.Third)
                && Equals(Fourth, other.Fourth);
        }

        public override int GetHashCode ()
        {
            return HashCodeHelper.GetHashCode(_elements);
        }

        public IEnumerator GetEnumerator ()
        {
            return _elements.GetEnumerator();
        }
    }
}
