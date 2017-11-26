//  Copyright (C) 2009-2010 by Markus Raufer
//
//  This file is part of MarkEmptyDirs.
//
//  MarkEmptyDirs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  MarkEmptyDirs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with MarkEmptyDirs.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace DR.IO
{
	[TestFixture]
	public class TestVirtualInputStream
	{
		public const string TestString = "Dies ist ein Test";

		private ASCIIEncoding _encoding;
		private VirtualInputStream _inputStream;

		[SetUp]
		public void Init()
		{
			var inputStream = new MemoryStream();

			_encoding = new ASCIIEncoding();
			inputStream.Write(_encoding.GetBytes(TestString), 0, TestString.Length);

            var outputStream = new MemoryStream();
            
            _inputStream = new VirtualInputStream(inputStream, outputStream);
		}
		
		[TearDown]
		public void Cleanup()
		{
			_inputStream.Dispose();
		}

		[Test]
		public void CallStreamReadMethod()
		{
			var bufferSize = (int)_inputStream.Length;
			var buffer = new byte[bufferSize];

			_inputStream.Position = 0;
			int nBytes = _inputStream.Read(buffer, 0, bufferSize);
			Assert.AreEqual(nBytes, bufferSize);

			string value = _encoding.GetString(buffer);
			Assert.AreEqual(value, TestString);
		}

		[Test]
		public void ReadStreamUsingStreamReaderReadMethod()
		{
			var reader = new StreamReader(_inputStream);

			var bufferSize = (int)_inputStream.Length;
			var buffer = new char[bufferSize];

			_inputStream.Position = 0;
			int nBytes = reader.Read(buffer, 0, bufferSize);
			reader.Close();

			Assert.AreEqual(nBytes, bufferSize);

			var value = new string(buffer);
			Assert.AreEqual(value, TestString);
		}

		[Test]
		public void ReadStreamUsingTwoStreamReaders()
		{
			var bufferSize = (int)_inputStream.Length;

			var reader1 = new StreamReader(_inputStream);
			var buffer = new char[bufferSize];

			_inputStream.Position = 0;
			int nBytes = reader1.Read(buffer, 0, bufferSize);
			Assert.AreEqual(nBytes, bufferSize);

			var value = new string(buffer);
			Assert.AreEqual(value, TestString);

		    _inputStream.OutputStream.Position = 0;
			var reader2 = new StreamReader(_inputStream.OutputStream);
			var buffer2 = new char[bufferSize];

			int nBytes2 = reader2.Read(buffer2, 0, bufferSize);
			Assert.AreEqual(nBytes2, bufferSize);

			var value2 = new string(buffer2);
			Assert.AreEqual(value2, TestString);
		}

		[Test]
		public void ReadStreamUsingStreamReaderAndShA1CryptoServiceProvider()
		{
			var sha1 = new SHA1CryptoServiceProvider();

			_inputStream.Position = 0;
			var hashBytes = sha1.ComputeHash(_inputStream);

            var bufferSize = (int)_inputStream.Length;

            var reader = new StreamReader(_inputStream.OutputStream);
            var buffer = new char[bufferSize];

            _inputStream.OutputStream.Position = 0;
            int nBytes = reader.Read(buffer, 0, bufferSize);
            Assert.AreEqual(nBytes, bufferSize);

			var value = new string(buffer);
			Assert.AreEqual(value, TestString);
		}
	}
}
