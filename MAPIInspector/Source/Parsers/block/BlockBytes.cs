using MAPIInspector.strings;
using System.IO;

namespace MAPIInspector.Parsers.block
{
    internal class BlockBytes : Block
    {
        private Stream _data;
        private int cbBytes;
        private int cbMaxBytes;

        public BlockBytes() { }

        // Mimic std::vector<BYTE>
        public new int Size => (int)_data.Length;
        public bool Empty => _data.Length == 0;
        public Stream Data => _data;

        public static BlockBytes Parse(BinaryParser parser, int cbBytes, int cbMaxBytes = -1)
        {
            var ret = new BlockBytes
            {
                parser = parser,
                enableJunk = false,
                cbBytes = cbBytes,
                cbMaxBytes = cbMaxBytes
            };
            ret.EnsureParsed();
            return ret;
        }

        public string ToTextString(bool multiLine) => Strings.StripCharacter(Strings.BinToTextString(Data, multiLine), '\0');

        public string ToHexString(bool multiLine) => Strings.BinToHexString(Data, multiLine);

        //public bool Equal(int cb, byte[] bin)
        //{
        //}

        protected override void Parse()
        {
            parsed = false;
            if (cbBytes > 0 && parser.CheckSize(cbBytes) &&
                (cbMaxBytes == -1 || cbBytes <= cbMaxBytes))
            {
                var bytes = parser.ReadBytes(cbBytes);
                _data = new MemoryStream(bytes, writable: false);
                parser.Advance(cbBytes);
                SetText(ToHexString(true));
                parsed = true;
            }
        }

        public static BlockBytes EmptyBB()
        {
            return new BlockBytes();
        }
    }
}
