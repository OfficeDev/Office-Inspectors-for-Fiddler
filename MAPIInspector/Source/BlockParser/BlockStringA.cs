using System.Text;

namespace Parser
{
    // Assuming 'block' is a base class in your project
    public class BlockStringA : Block
    {
        private string data = string.Empty;
        private int cchChar = -1;

        private BlockStringA() { }

        public string Data => data;
        public string CStr() => data;
        public int Length => data.Length;
        public bool Empty => string.IsNullOrEmpty(data);

        public static BlockStringA Parse(BinaryParser parser, int cchChar = -1)
        {
            var ret = new BlockStringA();
            ret.parser = parser;
            ret.EnableJunk = false;
            ret.cchChar = cchChar;
            ret.EnsureParsed();
            return ret;
        }

        protected override void Parse()
        {
            Parsed = false;
            var fixedLength = cchChar != -1;
            var oldOffset = parser.Offset;
            var size = parser.RemainingBytes;
            if (size <= 0)
                return;

            var bytes = parser.ReadBytes(size);
            parser.Offset = oldOffset;
            int length = cchChar;

            if (length == -1)
            {
                length = 0;
                while (length < size && bytes[length] != 0)
                    length++;
            }

            if (length > 0)
            {
                data = strings.RemoveInvalidCharacters(Encoding.ASCII.GetString(bytes, 0, length));
                SetText(data);
                parser.Advance(Length);
                if (!fixedLength) parser.Advance(1);
                Parsed = true;
            }
        }

        public static BlockStringA EmptySA() => new BlockStringA();
    }
}
