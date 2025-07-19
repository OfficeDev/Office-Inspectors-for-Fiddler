using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockParser
{
    public static class Strings
    {
        /// <summary>
        /// Trims leading and trailing whitespace characters (including null, space, carriage return, line feed, and tab) from the input string.
        /// Returns an empty string if the input is null or contains only whitespace.
        /// </summary>
        /// <param name="input">The string to trim.</param>
        /// <returns>The trimmed string, or an empty string if input is null or whitespace.</returns>
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

        /// <summary>
        /// Removes all occurrences of a specified character from the input string.
        /// Returns an empty string if the input is null or empty.
        /// </summary>
        /// <param name="input">The string to process.</param>
        /// <param name="character">The character to remove.</param>
        /// <returns>The string with the specified character removed.</returns>
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

        /// <summary>
        /// Determines if a character is invalid based on Unicode and multiline rules.
        /// Control characters in the range 0x80-0x9F are considered invalid.
        /// Printable Unicode characters (>= 0x20) are valid.
        /// If multiLine is true, tab, line feed, and carriage return are also valid.
        /// </summary>
        /// <param name="chr">The character code to check.</param>
        /// <param name="multiLine">Whether multiline characters are allowed.</param>
        /// <returns>True if the character is invalid; otherwise, false.</returns>
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

        /// <summary>
        /// Converts a byte array to a string, assuming the source string was Unicode (UTF-16LE).
        /// Invalid characters are replaced with '.'.
        /// </summary>
        /// <param name="bin">The byte array to convert.</param>
        /// <param name="multiLine">Whether multiline characters are allowed.</param>
        /// <returns>The resulting string with invalid characters replaced.</returns>
        public static string BinToTextStringW(byte[] bin, bool multiLine)
        {
            if (bin == null || bin.Length == 0 || bin.Length % sizeof(char) != 0)
                return string.Empty;

            // Convert byte array to string (Unicode/UTF-16LE)
            string text = System.Text.Encoding.Unicode.GetString(bin);

            // Remove invalid characters using the InvalidCharacter method
            var sb = new System.Text.StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (!InvalidCharacter(c, multiLine))
                    sb.Append(c);
                else
                    sb.Append('.');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a byte array to a string, assuming each byte is a single character (ASCII/Latin1).
        /// Invalid characters are replaced with '.'.
        /// </summary>
        /// <param name="bin">The byte array to convert.</param>
        /// <param name="multiLine">Whether multiline characters are allowed.</param>
        /// <returns>The resulting string with invalid characters replaced.</returns>
        public static string BinToTextStringA(byte[] bin, bool multiLine)
        {
            if (bin == null || bin.Length == 0)
                return string.Empty;

            // Use ASCII encoding with custom encoder fallback to replace unknown chars with '.'
            var encoding = System.Text.Encoding.GetEncoding(
                "ASCII",
                new System.Text.EncoderReplacementFallback("."),
                new System.Text.DecoderReplacementFallback("."));

            string text = encoding.GetString(bin);

            // Remove invalid characters using the InvalidCharacter method
            var sb = new System.Text.StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (!InvalidCharacter(c, multiLine))
                    sb.Append(c);
                else
                    sb.Append('.');
            }
            return sb.ToString();
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string representation.
        /// If a limit is specified and the array is longer, the output is truncated and suffixed with "...".
        /// Returns "NULL" if the input is null or empty.
        /// </summary>
        /// <param name="bin">The byte array to convert.</param>
        /// <param name="limit">The maximum number of bytes to convert (0 for no limit).</param>
        /// <returns>The hexadecimal string representation of the byte array.</returns>
        public static string BinToHexString(byte[] bin, int limit = 128)
        {
            var sb = new System.Text.StringBuilder();

            if (bin == null || bin.Length == 0)
            {
                sb.Append("NULL");
            }
            else
            {
                if (limit < 0) limit = 0;
                int count = limit == 0 ? bin.Length : Math.Min(bin.Length, limit);
                for (int i = 0; i < count; i++)
                {
                    byte b = bin[i];
                    char high = (char)((b >> 4) <= 0x9 ? '0' + (b >> 4) : 'A' + (b >> 4) - 0xA);
                    char low = (char)((b & 0xF) <= 0x9 ? '0' + (b & 0xF) : 'A' + (b & 0xF) - 0xA);
                    sb.Append(high);
                    sb.Append(low);
                }

                if (limit != 0 && bin.Length > limit)
                {
                    sb.Append("...");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Replaces invalid characters in a string with '.'.
        /// The last character is preserved as null if the input was null-terminated.
        /// </summary>
        /// <param name="input">The string to process.</param>
        /// <returns>The string with invalid characters replaced by '.'.</returns>
        public static string RemoveInvalidCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var chars = input.ToCharArray();
            bool nullTerminated = chars[chars.Length - 1] == '\0';

            for (int i = 0; i < chars.Length; i++)
            {
                if (InvalidCharacter((uint)(chars[i] & 0xFF), true))
                    chars[i] = '.';
            }

            if (nullTerminated)
                chars[chars.Length - 1] = '\0';

            return new string(chars);
        }

        /// <summary>
        /// Prepends each string in the list with a tab character or a pipe and tab, depending on the usePipes flag.
        /// </summary>
        /// <param name="elems">The list of strings to process.</param>
        /// <param name="usePipes">If true, prepends "|\t" to each string; otherwise, prepends "\t".</param>
        /// <returns>A new list of strings with the specified prefix added to each element.</returns>
        public static List<string> TabStrings(List<string> elems, bool usePipes)
        {
            if (elems == null || elems.Count == 0) return new List<string>();
            var strings = new List<string>(elems.Count);
            foreach (var elem in elems)
            {
                if (usePipes)
                {
                    strings.Add("|\t" + elem);
                }
                else
                {
                    strings.Add("\t" + elem);
                }
            }

            return strings;
        }
    }
}
