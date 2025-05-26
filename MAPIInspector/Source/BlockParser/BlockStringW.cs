using System.Text;

namespace Parser
{
    // Mimics the C++ blockStringW class
    public class BlockStringW : Block
    {
        private string data = string.Empty;
        private int cchChar = -1;

        public BlockStringW() { }

        // Mimic std::wstring conversion
        public static implicit operator string(BlockStringW block) => block.data;

        public string Data => data;
        public int Length => data.Length;
        public bool Empty => string.IsNullOrEmpty(data);

        public string CStr() => data;

        public static BlockStringW Parse(string data, int size, int offset)
        {
            var ret = new BlockStringW();
            ret.parsed = true;
            ret.enableJunk = false;
            ret.data = data;
            ret.SetText(data);
            ret.Size = size;
            ret.Offset = offset;
            return ret;
        }

        public static BlockStringW Parse(BinaryParser parser, int cchChar = -1)
        {
            var ret = new BlockStringW();
            ret.parser = parser;
            ret.enableJunk = false;
            ret.cchChar = cchChar;
            ret.EnsureParsed();
            return ret;
        }

        protected override void Parse()
        {
            parsed = false;
            if (cchChar == -1)
            {
                // we want to walk through the data in parser (GetAddress(0)) looking for double null terminator
                // and set cchChar to the length of the string (including null terminator)
                if (parser != null)
                {
                    var bytes = parser.GetAddress();
                    int maxChars = parser.RemainingBytes / 2;
                    int charCount = 0;
                    for (int i = 0; i < maxChars; i++)
                    {
                        // Each char is 2 bytes (UTF-16LE)
                        int byteIndex = i * 2;
                        if (byteIndex + 2 > bytes.Length)
                            break;
                        ushort ch = (ushort)(bytes[byteIndex] | (bytes[byteIndex + 1] << 8));
                        ushort nextCh = 0;
                        if (byteIndex + 4 <= bytes.Length)
                            nextCh = (ushort)(bytes[byteIndex + 2] | (bytes[byteIndex + 3] << 8));
                        if (ch == 0 && nextCh == 0)
                        {
                            // Found double null terminator
                            charCount = i + 1; // include the first null
                            break;
                        }
                    }
                    if (charCount == 0)
                        charCount = maxChars;
                    cchChar = charCount;
                }
            }

            if (cchChar > 0 && parser.CheckSize(2 * cchChar))
            {
                // Read the bytes and convert to string
                var bytes = parser.ReadBytes(2 * cchChar);
                data = strings.RemoveInvalidCharacters(Encoding.Unicode.GetString(bytes).TrimEnd('\0'));
                SetText(data);
                parsed = true;
            }
        }

        public static BlockStringW EmptySW() => new BlockStringW();
    }
}
