using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.9 WStringArray_r
    /// A class indicates the WStringArray_r structure.
    /// </summary>
    public class WStringArray_r : BaseStructure
    {
        /// <summary>
        /// A variable value
        /// </summary>
        public byte? HasValue;

        /// <summary>
        /// The number of Unicode character string references represented in the WStringArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The Unicode character string references. The strings referred to are NULL-terminated.
        /// </summary>
        public MAPIString[] LppszW;

        /// <summary>
        /// Parse the WStringArray_r payload of session.
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

            CValues = ReadUint();
            List<MAPIString> temBytes = new List<MAPIString>();
            for (ulong i = 0; i < CValues; i++)
            {
                MAPIString tempByte = new MAPIString(Encoding.Unicode);
                tempByte.Parse(s);
                temBytes.Add(tempByte);
            }

            LppszW = temBytes.ToArray();
        }
    }
}
