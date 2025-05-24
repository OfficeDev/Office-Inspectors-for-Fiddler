using System;
using System.Collections.Generic;
using System.IO;

namespace Parser
{
    // binaryParser - helper class for parsing binary data without
    // worrying about whether you've run off the end of your buffer.
    public class BinaryParser
    {
        private readonly Stream bin;
        private int offset;
        private int size; // When uncapped, this is bin.Length. When capped, this is our artificial capped size.
        private readonly Stack<int> sizes = new Stack<int>();

        public BinaryParser()
        {
            bin = new MemoryStream();
            size = 0;
            offset = 0;
        }

        public BinaryParser(int cb, byte[] _bin)
        {
            if (_bin != null && cb > 0)
            {
                if (_bin.Length > cb)
                {
                    bin = new MemoryStream(_bin, 0, cb, false);
                }
                else
                {
                    bin = new MemoryStream(_bin, false);
                }
            }
            else
            {
                bin = new MemoryStream();
            }

            size = (int)bin.Length;
            offset = 0;
        }

        public BinaryParser(byte[] _bin)
        {
            if (_bin != null)
            {
                bin = new MemoryStream(_bin, false);
            }
            else
            {
                bin = new MemoryStream();
            }

            size = (int)bin.Length;
            offset = 0;
        }

        public BinaryParser(Stream sourceStream, int cb = -1)
        {
            offset = 0;
            if (sourceStream == null)
            {
                bin = new MemoryStream();
                size = 0;
                return;
            }

            long originalPosition = sourceStream.CanSeek ? sourceStream.Position : -1;
            try
            {
                if (sourceStream.CanSeek)
                {
                    sourceStream.Position = 0;
                }

                bin = new MemoryStream();
                if (cb >= 0 && cb < sourceStream.Length)
                {
                    byte[] buffer = new byte[cb];
                    int read = sourceStream.Read(buffer, 0, cb);
                    bin.Write(buffer, 0, read);
                }
                else
                {
                    sourceStream.CopyTo(bin);
                }

            }
            finally
            {
                bin.Position = 0;
                size = (int)bin.Length;
                if (sourceStream.CanSeek) sourceStream.Position = originalPosition;
            }
        }

        public BinaryParser(List<byte> _bin)
        {
            if (_bin != null)
            {
                bin = new MemoryStream(_bin.ToArray(), false);
            }
            else
            {
                bin = new MemoryStream();
            }
            size = (int)bin.Length;
            offset = 0;
        }

        public bool Empty => offset == size;
        public void Advance(int cb)
        {
            offset += cb;
            bin.Position = offset;
        }
        public void Rewind()
        {
            offset = 0;
            bin.Position = 0;
        }
        public int Offset
        {
            get => offset;
            set
            {
                offset = value;
                bin.Position = offset;
            }
        }

        public byte[] GetAddress()
        {
            if (offset >= 0 && GetSize() > 0)
            {
                long oldPos = bin.Position;
                bin.Position = offset;
                byte[] result = new byte[GetSize()];
                bin.Read(result, 0, result.Length);
                bin.Position = oldPos;
                return result;
            }

            return Array.Empty<byte>();
        }

        public void SetCap(int cap)
        {
            sizes.Push(size);
            if (cap != 0 && offset + cap < bin.Length)
            {
                size = offset + cap;
            }
        }

        public void ClearCap()
        {
            if (sizes.Count == 0)
            {
                size = (int)bin.Length;
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

        public byte[] ReadBytes(int cb)
        {
            if (CheckSize(cb))
            {
                byte[] bytes = new byte[cb];
                bin.Position = offset;
                int read = bin.Read(bytes, 0, cb);
                Advance(read);
                return bytes;
            }
            return Array.Empty<byte>();
        }
    }
}
