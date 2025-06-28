using System;
using System.Text;

namespace BlockParser
{
    public class BlockStringA : BlockString
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
            int length = Math.Min(size, cchChar);

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
                if (!fixedLength && parser.RemainingBytes >= 1) parser.Advance(1);
                Parsed = true;
            }
        }

        protected override void ParseBlocks()
        {
            // No blocks to parse in BlockStringA
            // TODO: Consider if a default implementation should be provided
        }
    }
}
