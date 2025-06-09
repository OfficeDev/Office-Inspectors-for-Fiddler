using System.Text;

namespace BlockParser
{
    public class BlockStringW : Block
    {
        private string data = string.Empty;
        private int cchChar = -1;

        public static implicit operator string(BlockStringW block) => block.data;

        public string Data => data;
        public int Length => data.Length;
        public bool Empty => string.IsNullOrEmpty(data);

        public static BlockStringW Parse(string data, int size, int offset)
        {
            var ret = new BlockStringW
            {
                Parsed = true,
                EnableJunk = false,
                data = data
            };
            ret.SetText(data);
            ret.Size = size;
            ret.Offset = offset;
            return ret;
        }

        public static BlockStringW Parse(BinaryParser parser, int cchChar = -1)
        {
            var ret = new BlockStringW
            {
                parser = parser,
                EnableJunk = false,
                cchChar = cchChar
            };
            ret.EnsureParsed();
            return ret;
        }

        protected override void Parse()
        {
            Parsed = false;
            var size = parser.RemainingBytes;
            if (size <= 0)
                return;

            var fixedLength = cchChar != -1;
            var oldOffset = parser.Offset;
            var bytes = parser.ReadBytes(size);
            parser.Offset = oldOffset;
            int length = cchChar * 2;

            if (cchChar == -1)
            {
                length = 0;
                while (length + 1 < size && !(bytes[length] == 0 && bytes[length + 1] == 0))
                    length+=2;
            }

            if (length >= 0)
            {
                data = Strings.RemoveInvalidCharacters(Encoding.Unicode.GetString(bytes, 0, length));
                SetText(data);
                parser.Advance(length);
                // If we were given a length, that's all we read. But if we were not given a length, we read until the null terminator.
                // So we must now skip the null terminator.
                if (!fixedLength && parser.RemainingBytes >= 2) parser.Advance(2);
                Parsed = true;
            }
        }

        protected override void ParseBlocks()
        {
            // No blocks to parse in BlockStringW
            // TODO: Consider if a default implementation should be provided
        }

        public static BlockStringW EmptySW() => new BlockStringW();
    }
}
