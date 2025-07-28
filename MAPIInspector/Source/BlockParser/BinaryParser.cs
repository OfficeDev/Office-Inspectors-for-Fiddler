using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BlockParser
{
    /// <summary>
    /// Helper class for parsing binary data from a stream or buffer, 
    /// providing safe access and offset management, including support for artificial caps.
    /// </summary>
    public class BinaryParser
    {
        /// <summary>
        /// Gets whether the parser has reached the end of the buffer.
        /// </summary>
        public bool Empty => Offset == size;

        /// <summary>
        /// Gets or sets the current offset within the buffer.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets the number of bytes remaining from the current offset to the end of the buffer.
        /// Returns 0 if the offset is out of bounds.
        /// </summary>
        public int RemainingBytes => Offset > size ? 0 : size - Offset;

        private readonly byte[] bin;
        private int size; // When uncapped, this is bin.Length. When capped, this is our artificial capped size.
        private readonly Stack<int> sizes = new Stack<int>();

        /// <summary>
        /// Initializes a new, empty BinaryParser.
        /// </summary>
        public BinaryParser()
        {
            bin = Array.Empty<byte>();
            size = 0;
            Offset = 0;
        }

        /// <summary>
        /// Initializes a BinaryParser with a byte array and a specified count.
        /// If the array is longer than the count, only the first count bytes are used.
        /// </summary>
        /// <param name="cb">The number of bytes to use from the array.</param>
        /// <param name="_bin">The byte array to parse.</param>
        public BinaryParser(int cb, byte[] _bin)
        {
            if (_bin != null && cb > 0)
            {
                if (_bin.Length > cb)
                {
                    bin = new byte[cb];
                    Buffer.BlockCopy(_bin, 0, bin, 0, cb);
                }
                else
                {
                    bin = new byte[_bin.Length];
                    Buffer.BlockCopy(_bin, 0, bin, 0, _bin.Length);
                }
            }
            else
            {
                bin = Array.Empty<byte>();
            }

            size = bin.Length;
            Offset = 0;
        }

        /// <summary>
        /// Initializes a BinaryParser with a byte array.
        /// </summary>
        /// <param name="_bin">The byte array to parse.</param>
        public BinaryParser(byte[] _bin)
        {
            if (_bin != null)
            {
                bin = new byte[_bin.Length];
                Buffer.BlockCopy(_bin, 0, bin, 0, _bin.Length);
            }
            else
            {
                bin = Array.Empty<byte>();
            }

            size = bin.Length;
            Offset = 0;
        }

        /// <summary>
        /// Initializes a BinaryParser from a stream, starting at a given position and reading a specified number of bytes.
        /// If cb is negative, reads to the end of the stream.
        /// </summary>
        /// <param name="sourceStream">The source stream to read from.</param>
        /// <param name="position">The position in the stream to start reading from.</param>
        /// <param name="cb">The number of bytes to read.</param>
        public BinaryParser(Stream sourceStream, long position, int cb)
        {
            Offset = 0;
            if (sourceStream == null || !sourceStream.CanSeek)
            {
                bin = Array.Empty<byte>();
                size = 0;
                return;
            }

            long originalPosition = sourceStream.Position;
            try
            {
                sourceStream.Position = position;
                int bytesToRead = cb >= 0 && cb + position < sourceStream.Length ? cb : (int)(sourceStream.Length - position);
                bin = new byte[bytesToRead];
                int read = sourceStream.Read(bin, 0, bytesToRead);
                if (read < bytesToRead)
                {
                    Array.Resize(ref bin, read);
                }
            }
            finally
            {
                size = bin.Length;
                if (sourceStream.CanSeek) sourceStream.Position = originalPosition;
            }
        }

        /// <summary>
        /// Initializes a BinaryParser from a stream, starting at position 0 and reading a specified number of bytes.
        /// If cb is negative, reads to the end of the stream.
        /// </summary>
        /// <param name="sourceStream">The source stream to read from.</param>
        /// <param name="cb">The number of bytes to read, or -1 to read to the end.</param>
        public BinaryParser(Stream sourceStream, int cb = -1) : this(sourceStream, 0, cb) { }

        /// <summary>
        /// Advances the current offset by the specified number of bytes.
        /// </summary>
        /// <param name="cb">The number of bytes to advance.</param>
        public void Advance(int cb) => Offset += cb;

        /// <summary>
        /// Resets the current offset to the beginning of the buffer.
        /// </summary>
        public void Rewind() => Offset = 0;

        /// <summary>
        /// Pushes a cap onto the size stack, limiting the accessible buffer size to the current offset plus the specified cap.
        /// Used to temporarily restrict parsing to a subsection of the buffer.
        /// </summary>
        /// <param name="cap">The number of bytes to cap from the current offset.</param>
        public void PushCap(int cap)
        {
            sizes.Push(size);
            if (cap != 0 && Offset + cap < bin.Length)
            {
                size = Offset + cap;
            }
        }

        /// <summary>
        /// Pops the most recent cap from the size stack, restoring the previous accessible buffer size.
        /// </summary>
        public void PopCap()
        {
            if (sizes.Count == 0)
            {
                size = bin.Length;
            }
            else
            {
                size = sizes.Pop();
            }
        }

        /// <summary>
        /// Checks if the specified number of bytes can be read from the current offset without exceeding the buffer.
        /// </summary>
        /// <param name="cb">The number of bytes to check.</param>
        /// <returns>True if the bytes can be read; otherwise, false.</returns>
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
                Buffer.BlockCopy(bin, Offset, bytes, 0, cb);
                Advance(cb);
                return bytes;
            }

            return Array.Empty<byte>();
        }

        /// <summary>
        /// Returns the entire binary stream as a hexadecimal string.
        /// Only used for debugging purposes.
        /// </summary>
        /// <returns>A string representation of the binary data in hexadecimal format.</returns>
        public string PeekBytes()
        {
            return Strings.BinToHexString(bin, bin.Length);
        }

        /// <summary>
        /// Outputs a sample of bytes from the current offset in the binary stream to the debug output.
        /// Only used for debugging purposes.
        /// </summary>
        /// <param name="cb">The number of bytes to output (default is 20).</param>
        public void SampleBytes(int cb = 20)
        {
            var offset = Offset;
            var length = Math.Min(cb, RemainingBytes);
            var _data = ReadBytes(length);
            var hex = Strings.BinToHexString(_data, 0);
            var text = Strings.BinToTextStringA(_data, true);
            Debug.WriteLine($"cb: 0x{length:X}={length} lpb: {hex}={text}");
            Offset = offset;
        }
    }
}
