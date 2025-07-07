using System;
using System.Text;

namespace BlockParser
{
    public class BlockStringA : BlockString
    {
        public bool LineMode { get; set; } = false;
        protected override void Parse()
        {
            Parsed = false;
            var size = parser.RemainingBytes;
            if (size <= 0)
                return;

            // We don't want to skip a null because we stopped at the line ending
            var fixedLength = cchChar != -1 || LineMode;
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
                data = Encoding.ASCII.GetString(bytes, 0, length);
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
                        length = lfIndex + lineEndingLength;
                    }
                }

                data = Strings.RemoveInvalidCharacters(data);

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
