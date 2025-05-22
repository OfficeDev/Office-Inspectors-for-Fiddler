
using System.IO;
using System.Text;

namespace MAPIInspector.strings
{
    internal class Strings
    {
        internal static bool InvalidCharacter(uint chr, bool bMultiLine)
        {
            // Remove range of control characters
            if (chr >= 0x80 && chr <= 0x9F) return true;
            // Any printable Unicode character gets mapped directly
            if (chr >= 0x20)
            {
                return false;
            }
            // If we allow multiple lines, we accept tab, LF and CR
            else if (
                bMultiLine && (chr == 9 || // Tab
                               chr == 10 || // Line Feed
                               chr == 13)) // Carriage Return
            {
                return false;
            }

            return true;
        }

        internal static string RemoveInvalidCharacters(string szString, bool bMultiLine)
        {
            if (string.IsNullOrEmpty(szString)) return szString;
            var chars = szString.ToCharArray();
            bool nullTerminated = chars.Length > 0 && chars[chars.Length - 1] == '\0';

            for (int i = 0; i < chars.Length; i++)
            {
                if (InvalidCharacter(chars[i], bMultiLine))
                {
                    chars[i] = '.';
                }
            }

            if (nullTerminated)
            {
                chars[chars.Length - 1] = '\0';
            }

            return new string(chars);
        }

        // Converts binary data from a Stream to a string, assuming source string was Unicode
        internal static string BinToTextString(Stream stream, bool bMultiLine)
        {
            if (stream == null || stream.Length == 0 || stream.Length % sizeof(char) != 0)
                return string.Empty;

            long originalPosition = stream.Position;
            try
            {
                int charCount = (int)(stream.Length / sizeof(char));
                var buffer = new char[charCount];
                using (var reader = new BinaryReader(stream, System.Text.Encoding.Unicode, leaveOpen: true))
                {
                    for (int i = 0; i < charCount; i++)
                    {
                        buffer[i] = reader.ReadChar();
                    }
                }
                var szBin = new string(buffer);
                return RemoveInvalidCharacters(szBin, bMultiLine);
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        internal static string StripCharacter(string szString, char character)
        {
            if (string.IsNullOrEmpty(szString)) return szString;
            var sb = new StringBuilder(szString.Length);
            foreach (var chr in szString)
            {
                if (chr != character)
                    sb.Append(chr);
            }

            return sb.ToString();
        }

        internal static string BinToHexString(Stream stream, bool bMultiLine)
        {
            if (stream == null || stream.Length == 0) return string.Empty;
            long originalPosition = stream.Position;
            try
            {
                var sb = new StringBuilder();
                using (var reader = new BinaryReader(stream, System.Text.Encoding.Default, leaveOpen: true))
                {
                    int byteCount = (int)stream.Length;
                    for (int i = 0; i < byteCount; i++)
                    {
                        byte b = reader.ReadByte();
                        sb.Append(b.ToString("X2"));
                        if (bMultiLine && (i + 1) % 16 == 0)
                            sb.AppendLine();
                    }
                }
                return sb.ToString();
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }
    }
}
