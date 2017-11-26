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
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DR
{
    public static class GTuple
    {
        public static GTuple<TElem, T1, T2> Create<TElem, T1, T2>(T1 o1, T2 o2)
            where T1 : TElem
            where T2 : TElem
        {
            return new GTuple<TElem, T1, T2>(o1, o2);
        }

        public static GTuple<TElem, T1, T2, T3> Create<TElem, T1, T2, T3>(T1 o1, T2 o2, T3 o3)
            where T1 : TElem
            where T2 : TElem
            where T3 : TElem
        {
            return new GTuple<TElem, T1, T2, T3>(o1, o2, o3);
        }

        public static GTuple<TElem, T1, T2, T3, T4> Create<TElem, T1, T2, T3, T4>(T1 o1, T2 o2, T3 o3, T4 o4)
            where T1 : TElem
            where T2 : TElem
            where T3 : TElem
            where T4 : TElem
        {
            return new GTuple<TElem, T1, T2, T3, T4>(o1, o2, o3, o4);
        }
    }

    public interface IGTuple<TElem> : ITuple, IEnumerable<TElem>
    {
		Type ElementType { get; }
        new TElem this[int index] { get; }
    }

    [Serializable]
    public struct GTuple<TElem, A, B> : IGTuple<TElem>
		where A : TElem
		where B : TElem
	{
        public readonly A First;
        public readonly B Second;

        public GTuple(A first, B second)
        {
            First = first;
            Second = second;
        }

        public GTuple(SerializationInfo info, StreamingContext ctxt)
        {
            First = (A)info.GetValue("First", typeof(A));
            Second = (B)info.GetValue("Second", typeof(B));
        }
            
		public Type ElementType { get { return typeof(TElem); } }
		
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("First", First);
            info.AddValue("Second", Second);
        }
        
        public int Length { get { return 2; } }

		object ITuple.this[int index] { get { return this[index]; } }
		
        public TElem this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return First;
                    case 1: return Second;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", First, Second);
        }

        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is GTuple<TElem, A, B>))
                return false;
            var other = (GTuple<TElem, A, B>)obj;
            return Equals(First, other.First) && Equals(Second, other.Second);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(First, Second);
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
        public IEnumerator<TElem> GetEnumerator()
        {
            yield return First;
            yield return Second;
        }
    }

    [Serializable]
    public struct GTuple<TElem, A, B, C> : IGTuple<TElem>
		where A : TElem
		where B : TElem
		where C : TElem
    {
        public readonly A First;
        public readonly B Second;
        public readonly C Third;

        public GTuple(A first, B second, C third)
        {
            First = first;
            Second = second;
            Third = third;
        }

        public GTuple(SerializationInfo info, StreamingContext ctxt)
        {
            First = (A)info.GetValue("First", typeof(A));
            Second = (B)info.GetValue("Second", typeof(B));
            Third = (C)info.GetValue("Third", typeof(C));
        }
            
		public Type ElementType { get { return typeof(TElem); } }
		
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("First", First);
            info.AddValue("Second", Second);
            info.AddValue("Third", Third);
        }
        
        public int Length { get { return 3; } }

		object ITuple.this[int index] { get { return this[index]; } }
		
        public TElem this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return First;
                    case 1: return Second;
                    case 2: return Third;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", First, Second, Third);
        }

        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is GTuple<TElem, A, B, C>))
                return false;
            var other = (GTuple<TElem, A, B, C>)obj;
            return Equals(First, other.First) && Equals(Second, other.Second) && Equals(Third, other.Third);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(First, Second, Third);
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
        public IEnumerator<TElem> GetEnumerator()
        {
            yield return First;
            yield return Second;
            yield return Third;
        }
    }

    [Serializable]
    public struct GTuple<TElem, A, B, C, D> : IGTuple<TElem>
		where A : TElem
		where B : TElem
		where C : TElem
		where D : TElem
    {
        public readonly A First;
        public readonly B Second;
        public readonly C Third;
        public readonly D Fourth;

        public GTuple(A first, B second, C third, D fourth)
        {
            First = first;
            Second = second;
            Third = third;
            Fourth = fourth;
        }

        public GTuple(SerializationInfo info, StreamingContext ctxt)
        {
            First = (A)info.GetValue("First", typeof(A));
            Second = (B)info.GetValue("Second", typeof(B));
            Third = (C)info.GetValue("Third", typeof(C));
            Fourth = (D)info.GetValue("Fourth", typeof(D));
        }
            
		public Type ElementType { get { return typeof(TElem); } }
		
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("First", First);
            info.AddValue("Second", Second);
            info.AddValue("Third", Third);
            info.AddValue("Fourth", Fourth);
        }
        
        public int Length { get { return 4; } }

		object ITuple.this[int index] { get { return this[index]; } }
		
        public TElem this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return First;
                    case 1: return Second;
                    case 2: return Third;
                    case 3: return Fourth;
                    default: throw new ArgumentOutOfRangeException("index");
                }
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2},{3})", First, Second, Third, Fourth);
        }

        public override bool Equals(object obj)
        {
            if (null == obj || !(obj is GTuple<TElem, A, B, C, D>))
                return false;
            var other = (GTuple<TElem, A, B, C, D>)obj;
            return Equals(First, other.First)
                && Equals(Second, other.Second)
                && Equals(Third, other.Third)
                && Equals(Fourth, other.Fourth);
        }

        public override int GetHashCode()
        {
            return HashCodeHelper.GetHashCode(First, Second, Third, Fourth);
        }

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		
        public IEnumerator<TElem> GetEnumerator()
        {
            yield return First;
            yield return Second;
            yield return Third;
            yield return Fourth;
        }
    }
}
