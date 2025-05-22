using System;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers.block
{
    // BinaryParser - helper class for parsing binary data without
    // worrying about whether you've run off the end of your buffer.
    internal class BinaryParser
    {
        private readonly Stream _bin;
        private long _offset;
        private long _size; // When uncapped, this is _bin.Length. When capped, this is our artificial capped size.
        private readonly Stack<long> _sizes = new Stack<long>();

        public BinaryParser()
        {
            _bin = new MemoryStream(Array.Empty<byte>());
            _size = 0;
            _offset = 0;
        }

        public BinaryParser(long cb, Stream bin)
        {
            if (bin != null && cb > 0)
            {
                _bin = bin;
                _size = cb;
            }
            else
            {
                _bin = new MemoryStream(Array.Empty<byte>());
                _size = 0;
            }
            _offset = 0;
        }

        public BinaryParser(Stream bin)
        {
            _bin = bin;
            _size = 0;
            _offset = 0;
        }

        public bool Empty => _offset == _size;

        public void Advance(int cb)
        {
            _offset += cb;
        }

        public void Rewind()
        {
            _offset = 0;
        }

        public long GetOffset()
        {
            return _offset;
        }

        public void SetOffset(int offset)
        {
            _offset = offset;
        }

        //public byte[] GetAddress()
        //{
        //    if (_offset >= _bin.Length)
        //        return Array.Empty<byte>();
        //    int len = _size - _offset;
        //    if (len <= 0) return Array.Empty<byte>();
        //    var result = new byte[len];
        //    Array.Copy(_bin, _offset, result, 0, len);
        //    return result;
        //}

        public void SetCap(int cap)
        {
            _sizes.Push(_size);
            if (cap != 0 && _offset + cap < _size)
            {
                _size = _offset + cap;
            }
        }

        public void ClearCap()
        {
            if (_sizes.Count == 0)
            {
                _size = _bin.Length;
            }
            else
            {
                _size = _sizes.Pop();
            }
        }

        // If we're before the end of the buffer, return the count of remaining bytes
        // If we're at or past the end of the buffer, return 0
        // If we're before the beginning of the buffer, return 0
        public long GetSize()
        {
            return _offset > _size ? 0 : _size - _offset;
        }

        public bool CheckSize(int cb)
        {
            return cb <= GetSize();
        }
    }
}
