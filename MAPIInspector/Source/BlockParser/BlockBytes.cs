using System.Collections.Generic;
using System.Linq;

namespace BlockParser
{
    public class BlockBytes : Block
    {
        private List<byte> _data = new List<byte>();
        private int cbBytes;
        private int cbMaxBytes;

        public BlockBytes() { }

        public IReadOnlyList<byte> Data => _data;
        public int Count => _data.Count;
        public bool Empty => _data.Count == 0;

        public static BlockBytes Parse(BinaryParser parser, int cbBytes, int cbMaxBytes = -1)
        {
            var ret = new BlockBytes
            {
                parser = parser,
                EnableJunk = false,
                cbBytes = cbBytes,
                cbMaxBytes = cbMaxBytes
            };
            ret.EnsureParsed();
            return ret;
        }

        public string ToTextStringA(bool multiLine) => Strings.StripCharacter(Strings.BinToTextStringA(_data, multiLine), '\0');

        public string ToHexString(bool multiLine) => Strings.BinToHexString(_data, multiLine);

        public bool Equal(int cb, byte[] bin)
        {
            if (cb != _data.Count) return false;
            return _data.SequenceEqual(bin);
        }

        protected override void Parse()
        {
            Parsed = false;
            if (cbBytes > 0 && parser.CheckSize(cbBytes) &&
                (cbMaxBytes == -1 || cbBytes <= cbMaxBytes))
            {
                _data = new List<byte>(parser.ReadBytes(cbBytes));
                Parsed = true;
            }
        }

        protected override void ParseBlocks()
        {
            SetText(ToHexString(false));
            AddHeader($"cb: {_data.Count}");
        }
    }
}
