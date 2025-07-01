using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.4 ShortArray_r
    /// A class indicates the ShortArray_r structure.
    /// </summary>
    public class ShortArray_r : BaseStructure
    {
        /// <summary>
        /// The number of 16-bit integer values represented in the ShortArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The 16-bit integer values.
        /// </summary>
        public short[] Lpi;

        /// <summary>
        /// Parse the ShortArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            CValues = ReadUint();
            List<short> tempList = new List<short>();
            for (ulong i = 0; i < CValues; i++)
            {
                tempList.Add(ReadINT16());
            }

            Lpi = tempList.ToArray();
        }
    }
}
