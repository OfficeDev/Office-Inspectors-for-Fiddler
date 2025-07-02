using System.Collections.Generic;
using System.Linq;

namespace BlockParser
{
    public class BlockBytes : Block
    {
        private byte[] _data = new byte[] { };
        internal int cbBytes;
        internal int cbMaxBytes;

        public BlockBytes() { }

        public IReadOnlyList<byte> Data => _data;
        public int Count => _data.Length;
        public bool Empty => _data.Length == 0;

        public string ToTextStringA(bool multiLine = false) => Strings.StripCharacter(Strings.BinToTextStringA(_data, multiLine), '\0');

        public string ToHexString(bool multiLine = false) => Strings.BinToHexString(_data, multiLine, 0);

        public bool Equal(int cb, byte[] bin)
        {
            if (cb != _data.Length) return false;
            return _data.SequenceEqual(bin);
        }

        protected override void Parse()
        {
            Parsed = false;
            if (cbBytes > 0 && parser.CheckSize(cbBytes) &&
                (cbMaxBytes == -1 || cbBytes <= cbMaxBytes))
            {
                _data = parser.ReadBytes(cbBytes);
                Parsed = true;
            }
        }

        protected override void ParseBlocks()
        {
            SetText(ToHexString(false));
            AddHeader($"bin: {ToTextStringA()}");
            AddHeader($"cb: {_data.Length}");
        }
    }
}
