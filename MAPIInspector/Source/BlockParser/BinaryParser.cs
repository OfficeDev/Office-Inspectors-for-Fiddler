using System;
using System.Collections.Generic;
using System.IO;

namespace BlockParser
{
    // binaryParser - helper class for parsing binary data without
    // worrying about whether you've run off the end of your buffer.
    public class BinaryParser
    {
        public bool Empty => Offset == size;
        public int Offset { get; set; }
        // If we're before the end of the buffer, return the count of remaining bytes
        // If we're at or past the end of the buffer, return 0
        // If we're before the beginning of the buffer, return 0
        public int RemainingBytes => Offset > size ? 0 : size - Offset;

        private readonly Stream bin;
        private int size; // When uncapped, this is bin.Length. When capped, this is our artificial capped size.
        private readonly Stack<int> sizes = new Stack<int>();

        public BinaryParser()
        {
            bin = new MemoryStream();
            size = 0;
            Offset = 0;
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
            Offset = 0;
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
            Offset = 0;
        }

        public BinaryParser(Stream sourceStream, long position, int cb = -1)
        {
            Offset = 0;
            if (sourceStream == null || !sourceStream.CanSeek)
            {
                bin = new MemoryStream();
                size = 0;
                return;
            }

            long originalPosition = sourceStream.Position;
            try
            {
                sourceStream.Position = position;

                bin = new MemoryStream();
                if (cb >= 0 && cb + position < sourceStream.Length)
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
                size = (int)bin.Length;
                if (sourceStream.CanSeek) sourceStream.Position = originalPosition;
            }
        }

        public BinaryParser(Stream sourceStream, int cb = -1) : this(sourceStream, 0, cb) { }

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
            Offset = 0;
        }

        public void Advance(int cb) => Offset += cb;

        public void Rewind() => Offset = 0;

        public void PushCap(int cap)
        {
            sizes.Push(size);
            if (cap != 0 && Offset + cap < bin.Length)
            {
                size = Offset + cap;
            }
        }

        public void PopCap()
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

        public bool CheckSize(int cb) => cb <= RemainingBytes;

        /// <summary>
        /// Reads the specified number of bytes from the current offset in the binary stream.
        /// Advances the offset by the number of bytes read.
        /// If there are not enough bytes remaining, returns an empty array.
        /// </summary>
        /// <param name="cb">The number of bytes to read.</param>
        /// <returns>
        /// A byte array containing the bytes read, or an empty array if there are not enough bytes remaining.
        /// </returns>
        public byte[] ReadBytes(int cb)
        {
            if (CheckSize(cb))
            {
                byte[] bytes = new byte[cb];
                bin.Position = Offset;
                int read = bin.Read(bytes, 0, cb);
                Advance(read);
                return bytes;
            }

            return Array.Empty<byte>();
        }

        // Only used for debugging purposes, returns the entire binary stream as a byte array
        public string PeekBytes()
        {
            var bytes = new byte[bin.Length];
            int read = bin.Read(bytes, 0, (int)bin.Length);
            return Strings.BinToHexString(new List<byte>(bytes), false, bytes.Length);
        }
    }
}
