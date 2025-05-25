using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class BlockBytes : Block
    {
        private List<byte> _data = new List<byte>();
        private int cbBytes;
        private int cbMaxBytes;

        public BlockBytes() { }

        // Mimic std::vector<BYTE>
        public IReadOnlyList<byte> Data => _data;
        public int Count => _data.Count;
        public bool IsEmpty => _data.Count == 0;

        public static BlockBytes Parse(BinaryParser parser, int cbBytes, int cbMaxBytes = -1)
        {
            var ret = new BlockBytes();
            ret.parser = parser;
            ret.enableJunk = false;
            ret.cbBytes = cbBytes;
            ret.cbMaxBytes = cbMaxBytes;
            ret.EnsureParsed();
            return ret;
        }

        public string ToTextStringA(bool multiLine)
        {
            return strings.StripCharacter(strings.BinToTextStringA(_data, multiLine), '\0');
        }

        public string ToHexString(bool multiLine)
        {
            return strings.BinToHexString(_data, multiLine);
        }

        public bool Equal(int cb, byte[] bin)
        {
            if (cb != _data.Count) return false;
            return _data.SequenceEqual(bin);
        }

        protected override void Parse()
        {
            parsed = false;
            if (cbBytes > 0 && parser.CheckSize(cbBytes) &&
                (cbMaxBytes == -1 || cbBytes <= cbMaxBytes))
            {
                _data = new List<byte>(parser.ReadBytes(cbBytes));
                SetText(ToHexString(true));
                parsed = true;
            }
        }
    }
}
