//  Copyright (C) 2009 by Markus Raufer
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

using System.IO;

namespace DR.IO
{
	public class VirtualInputStream : Stream
	{
		private readonly Stream _inputStream;
        private readonly Stream _outputStream;

        //private Stream
		public VirtualInputStream(Stream inputStream, Stream outputStream)
		{
            _inputStream = inputStream;            
            _outputStream = outputStream;
		}

		#region System.IO.Stream methods
		public override void Close()
		{
			if(null != _inputStream)
				_inputStream.Close();
		}

		public new void Dispose()
		{
			if (null != _inputStream)
				_inputStream.Dispose();
		}

		public override void Flush()
		{
			_inputStream.Flush();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return _inputStream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			_inputStream.SetLength(value);
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			var numberOfBytes = _inputStream.Read(buffer, offset, count);
            _outputStream.Write(buffer, offset, numberOfBytes);

			return numberOfBytes;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			_inputStream.Write(buffer, offset, count);
		}

		public override bool CanRead
		{
			get
			{
				return _inputStream.CanRead;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return _inputStream.CanSeek;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return _inputStream.CanWrite;
			}
		}

		public override long Length
		{
			get { return _inputStream.Length; }
		}

		public override long Position
		{
			get { return _inputStream.Position; }
			set { _inputStream.Position = value; }
		}
		#endregion

	    public Stream OutputStream
	    {
            get { return _outputStream; }
	    }
	}
}
