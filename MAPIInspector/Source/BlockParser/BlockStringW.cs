using System.Text;

namespace BlockParser
{
    public class BlockStringW : Block
    {
        internal string data = string.Empty;
        internal int cchChar = -1;

        public static implicit operator string(BlockStringW block) => block.data;

        public string Data => data;
        public int Length => data.Length;
        public bool Empty => string.IsNullOrEmpty(data);

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
    }
}
