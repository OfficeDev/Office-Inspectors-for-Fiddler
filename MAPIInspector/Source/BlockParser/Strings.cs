using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public static class strings
    {
        public static string TrimWhitespace(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            char[] whitespace = { '\0', ' ', '\r', '\n', '\t' };

            // Find first non-whitespace
            int first = 0;
            while (first < input.Length && whitespace.Contains(input[first]))
                first++;

            // Find last non-whitespace
            int last = input.Length - 1;
            while (last >= 0 && whitespace.Contains(input[last]))
                last--;

            if (first > last)
                return string.Empty;

            return input.Substring(first, last - first + 1);
        }

        public static string StripCharacter(string input, char character)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var sb = new System.Text.StringBuilder(input.Length);
            foreach (var chr in input)
            {
                if (chr != character)
                    sb.Append(chr);
            }
            return sb.ToString();
        }

        // Determines if a character is invalid based on Unicode and multiline rules
        public static bool InvalidCharacter(uint chr, bool multiLine)
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
                multiLine && (chr == 9 || // Tab
                              chr == 10 || // Line Feed
                              chr == 13)) // Carriage Return
            {
                return false;
            }

            return true;
        }

        // Converts binary data to a string, assuming source string was unicode
        public static string BinToTextStringW(List<byte> bin, bool multiLine)
        {
            if (bin == null || bin.Count == 0 || bin.Count % sizeof(char) != 0)
                return string.Empty;

            // Convert List<byte> to byte[]
            byte[] byteArray = bin.ToArray();

            // Convert byte array to string (Unicode/UTF-16LE)
            string text = System.Text.Encoding.Unicode.GetString(byteArray);

            // Remove invalid characters using the InvalidCharacter method
            var sb = new System.Text.StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (!InvalidCharacter(c, multiLine))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        // Converts binary data to a string, assuming each byte is a single character (ASCII/Latin1)
        public static string BinToTextStringA(List<byte> bin, bool multiLine)
        {
            if (bin == null || bin.Count == 0)
                return string.Empty;

            // Convert List<byte> to byte[]
            byte[] byteArray = bin.ToArray();

            // Use ASCII encoding (or Encoding.Latin1 for extended characters)
            string text = System.Text.Encoding.ASCII.GetString(byteArray);

            // Remove invalid characters using the InvalidCharacter method
            var sb = new System.Text.StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (!InvalidCharacter(c, multiLine))
                    sb.Append(c);
            }
            return sb.ToString();
        }

        public static string BinToHexString(List<byte> bin, bool prependCb = false)
        {
            var sb = new System.Text.StringBuilder();

            if (prependCb)
            {
                sb.AppendFormat("cb: {0} lpb: ", bin.Count);
            }

            if (bin == null || bin.Count == 0)
            {
                sb.Append("NULL");
            }
            else
            {
                for (int i = 0; i < bin.Count; i++)
                {
                    byte b = bin[i];
                    char high = (char)((b >> 4) <= 0x9 ? '0' + (b >> 4) : 'A' + (b >> 4) - 0xA);
                    char low = (char)((b & 0xF) <= 0x9 ? '0' + (b & 0xF) : 'A' + (b & 0xF) - 0xA);
                    sb.Append(high);
                    sb.Append(low);
                }
            }

            return sb.ToString();
        }
        public static string RemoveInvalidCharacters(string input, bool multiLine = true)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var chars = input.ToCharArray();
            bool nullTerminated = chars[chars.Length - 1] == '\0';

            for (int i = 0; i < chars.Length; i++)
            {
                if (InvalidCharacter((uint)(chars[i] & 0xFF), multiLine))
                    chars[i] = '.';
            }

            if (nullTerminated)
                chars[chars.Length - 1] = '\0';

            return new string(chars);
        }

    }
}
