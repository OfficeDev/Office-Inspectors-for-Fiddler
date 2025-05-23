using System;
using System.Collections.Generic;

namespace Parser
{
    // binaryParser - helper class for parsing binary data without
    // worrying about whether you've run off the end of your buffer.
    public class BinaryParser
    {
        private readonly List<byte> bin;
        private int offset;
        private int size; // When uncapped, this is bin.Count. When capped, this is our artificial capped size.
        private readonly Stack<int> sizes = new Stack<int>();

        public BinaryParser()
        {
            bin = new List<byte>();
            size = 0;
            offset = 0;
        }

        public BinaryParser(int cb, byte[] _bin)
        {
            if (_bin != null && cb > 0)
            {
                if (_bin.Length > cb)
                {
                    bin = new List<byte>(new List<byte>(new ArraySegment<byte>(_bin, 0, cb)));
                }
                else
                {
                    bin = new List<byte>(new List<byte>(_bin));
                }
            }
            else
            {
                bin = new List<byte>();
            }

            size = bin.Count;
            offset = 0;
        }

        public BinaryParser(System.IO.Stream stream, int cb = -1)
        {
            if (stream == null)
            {
                bin = new List<byte>();
                size = 0;
                offset = 0;
                return;
            }

            long originalPosition = stream.CanSeek ? stream.Position : 0;
            try
            {
                int length = (int)(stream.Length - (stream.CanSeek ? stream.Position : 0));
                int readLength = (cb > 0 && cb < length) ? cb : length;
                byte[] buffer = new byte[readLength];
                int bytesRead = 0;
                while (bytesRead < readLength)
                {
                    int n = stream.Read(buffer, bytesRead, readLength - bytesRead);
                    if (n == 0) break;
                    bytesRead += n;
                }
                if (buffer.Length > bytesRead)
                {
                    bin = new List<byte>(new List<byte>(new ArraySegment<byte>(buffer, 0, bytesRead)));
                }
                else
                {
                    bin = new List<byte>(buffer);
                }

                size = bin.Count;
                offset = 0;
            }
            finally
            {
                if (stream.CanSeek)
                    stream.Position = originalPosition;
            }
        }

        public BinaryParser(List<byte> _bin)
        {
            bin = _bin ?? new List<byte>();
            size = bin.Count;
            offset = 0;
        }

        public bool Empty => offset == size;
        public void Advance(int cb) => offset += cb;
        public void Rewind() => offset = 0;
        public int GetOffset() => offset;
        public void SetOffset(int _offset) => offset = _offset;
        public byte[] GetAddress()
        {
            if (offset >= 0 && offset < bin.Count)
            {
                return bin.GetRange(offset, bin.Count - offset).ToArray();
            }
            return Array.Empty<byte>();
        }

        public void SetCap(int cap)
        {
            sizes.Push(size);
            if (cap != 0 && offset + cap < size)
            {
                size = offset + cap;
            }
        }

        public void ClearCap()
        {
            if (sizes.Count == 0)
            {
                size = bin.Count;
            }
            else
            {
                size = sizes.Pop();
            }
        }

        // If we're before the end of the buffer, return the count of remaining bytes
        // If we're at or past the end of the buffer, return 0
        // If we're before the beginning of the buffer, return 0
        public int GetSize()
        {
            return offset > size ? 0 : size - offset;
        }

        public bool CheckSize(int cb)
        {
            return cb <= GetSize();
        }
    }
}
