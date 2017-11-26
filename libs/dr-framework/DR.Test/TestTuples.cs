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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;

namespace DR
{
    [TestFixture]
    public class TestTuples
    {
        [Test]
        public void TestTuple2Serialization()
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            var tuple = Tuple.Create("Jonny Dee", 1976);
            Console.WriteLine(tuple);
            
            formatter.Serialize(stream, tuple);
            
            stream.Position = 0;
            var tupleCopy = formatter.Deserialize(stream);
            Console.WriteLine(tupleCopy);
            
            Assert.AreEqual(tuple, tupleCopy);
        }

        [Test]
        public void TestTuple3Serialization()
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            var tuple = Tuple.Create("Jonny Dee", 1976, 10);
            Console.WriteLine(tuple);
            
            formatter.Serialize(stream, tuple);
            
            stream.Position = 0;
            var tupleCopy = formatter.Deserialize(stream);
            Console.WriteLine(tupleCopy);
            
            Assert.AreEqual(tuple, tupleCopy);
        }

        [Test]
        public void TestTuple4Serialization()
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            var tuple = Tuple.Create("Jonny Dee", 1976, 10, 180.5);
            Console.WriteLine(tuple);
            
            formatter.Serialize(stream, tuple);
            
            stream.Position = 0;
            var tupleCopy = formatter.Deserialize(stream);
            Console.WriteLine(tupleCopy);
            
            Assert.AreEqual(tuple, tupleCopy);
        }
    }
}
