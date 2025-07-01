using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.11 PROP_VAL_UNION
    /// A class indicates the String_r structure.
    /// </summary>
    public class String_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// A single 8-bit character string value. value is NULL-terminated.
        /// </summary>
        public MAPIString Value;

        /// <summary>
        /// Parse the String_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte temp = ReadByte();
            if (temp == 0xFF)
            {
                HasValue = temp;
            }
            else
            {
                s.Position -= 1;
            }

            Value = new MAPIString(Encoding.ASCII);
            Value.Parse(s);
        }
    }
}
