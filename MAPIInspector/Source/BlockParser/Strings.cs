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

        public static string Join(System.Collections.Generic.IEnumerable<string> values, string separator)
        {
            if (values == null)
                return string.Empty;

            using (var enumerator = values.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return string.Empty;

                var sb = new System.Text.StringBuilder();
                sb.Append(enumerator.Current);

                while (enumerator.MoveNext())
                {
                    sb.Append(separator);
                    sb.Append(enumerator.Current);
                }

                return sb.ToString();
            }
        }

        public static string EmptyString => string.Empty;

        public static string FormatMessage(string format, params object[] args)
        {
            if (string.IsNullOrEmpty(format))
                return string.Empty;
            return string.Format(format, args);
        }
    }
}
