using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.6.2 WStringsArray_r
    /// A class indicates the WStringsArray_r structure.
    /// </summary>
    public class WStringsArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of character strings structures in aggregation. The value MUST NOT exceed 100,000.
        /// </summary>
        public uint Count;

        /// <summary>
        /// The list of wchar_t type strings in aggregation. The strings in list are NULL-terminated.
        /// </summary>
        public MAPIString[] Strings;

        /// <summary>
        /// Parse the WStringsArray_r payload of session.
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

            Count = ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < Count; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.Unicode);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }

            Strings = temBytes.ToArray();
        }
    }
}
