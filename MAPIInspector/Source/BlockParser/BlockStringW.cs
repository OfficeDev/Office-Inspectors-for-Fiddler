using System;
using System.Text;

namespace BlockParser
{
    public class BlockStringW : BlockString
    {
        public bool LineMode { get; set; } = false;
        protected override void Parse()
        {
            Parsed = false;
            var size = parser.RemainingBytes;
            if (size <= 0)
                return;

            // We don't want to skip a null because we stopped at the line ending
            var fixedLength = cchChar != -1|| LineMode;
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
                data = Encoding.Unicode.GetString(bytes, 0, length);

                if (LineMode)
                {
                    if (length == 0) return; // don't read nothing
                    // We want to read until the first line feed (LF) character, which is either '\n' or "\r\n"
                    // Our length should include the LF character(s), but the data should not include them
                    int lfIndexRN = data.IndexOf("\r\n");
                    int lfIndexN = data.IndexOf('\n');

                    int lfIndex = -1;
                    int lineEndingLength = 0;

                    if (lfIndexRN >= 0 && (lfIndexN == -1 || lfIndexRN < lfIndexN))
                    {
                        lfIndex = lfIndexRN;
                        lineEndingLength = 2;
                    }
                    else if (lfIndexN >= 0)
                    {
                        lfIndex = lfIndexN;
                        lineEndingLength = 1;
                    }

                    if (lfIndex >= 0)
                    {
                        data = data.Substring(0, lfIndex);
                        length = (lfIndex + lineEndingLength) * 2; // Multiply by 2 for UTF-16 bytes
                    }
                }

                data = Strings.RemoveInvalidCharacters(data);

                Text = data;
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
