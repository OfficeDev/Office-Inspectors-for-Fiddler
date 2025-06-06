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
                int maxChars = bytes.Length / 2;
                for (int i = 0; i < maxChars; i++)
                {
                    // Each char is 2 bytes (UTF-16LE)
                    int byteIndex = i * 2;
                    if (byteIndex + 2 > bytes.Length)
                        break;
                    ushort ch = (ushort)(bytes[byteIndex] | (bytes[byteIndex + 1] << 8));
                    if (ch == 0)
                    {
                        // Found null terminator
                        length = byteIndex;
                        break;
                    }
                }
            }

            if (length >= 0)
            {
                data = Strings.RemoveInvalidCharacters(Encoding.Unicode.GetString(bytes, 0, length));
                SetText(data);
                parser.Advance(length);
                // If we were given a length, that's all we read. But if we were not given a length, we read until the null terminator.
                // So we must now skip the null terminator.
                if (!fixedLength) parser.Advance(2);
                Parsed = true;
            }
        }

        public static BlockStringW EmptySW() => new BlockStringW();
    }
}
