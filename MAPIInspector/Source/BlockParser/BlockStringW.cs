using System;
using System.Text;

namespace BlockParser
{
    public class BlockStringW : BlockString
    {
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
            // After calculating length, ensure it is even (since Unicode chars are 2 bytes)
            int length = Math.Min(size, cchChar * 2); // & ~1;
            if ((length & 1) == 1)
            {
                length -= 1;
            }

            if (cchChar == -1)
            {
                length = 0;
                while (length + 1 < size && !(bytes[length] == 0 && bytes[length + 1] == 0))
                    length += 2;
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
