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
                // Find null-terminated length
                cchChar = 0;
                var addr = parser.GetAddress();
                var size = parser.GetSize();
                while (cchChar < size && addr[cchChar] != 0)
                    cchChar++;
            }

            if (cchChar > 0 && parser.CheckSize(cchChar))
            {
                var bytes = parser.ReadBytes(cchChar);
                data = strings.RemoveInvalidCharacters(Encoding.ASCII.GetString(bytes, 0, cchChar));
                SetText(data);
                parsed = true;
            }
        }

        public static BlockStringA EmptySA() => new BlockStringA();
    }
}
