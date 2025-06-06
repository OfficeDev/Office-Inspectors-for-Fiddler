using System.Text;

namespace BlockParser
{
    public class BlockStringA : Block
    {
        private string data = string.Empty;
        private int cchChar = -1;

        public static implicit operator string(BlockStringA block) => block.data;

        public string Data => data;
        public int Length => data.Length;
        public bool Empty => string.IsNullOrEmpty(data);

        public static BlockStringA Parse(BinaryParser parser, int cchChar = -1)
        {
            var ret = new BlockStringA
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
            int length = cchChar;

            if (cchChar == -1)
            {
                length = 0;
                while (length < size && bytes[length] != 0)
                    length++;
            }

            if (length >= 0)
            {
                data = Strings.RemoveInvalidCharacters(Encoding.ASCII.GetString(bytes, 0, length));
                SetText(data);
                parser.Advance(length);
                // If we were given a length, that's all we read. But if we were not given a length, we read until the null terminator.
                // So we must now skip the null terminator.
                if (!fixedLength) parser.Advance(1);
                Parsed = true;
            }
        }

        public static BlockStringA EmptySA() => new BlockStringA();
    }
}
