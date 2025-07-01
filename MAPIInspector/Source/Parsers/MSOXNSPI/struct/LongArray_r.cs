using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.5 LongArray_r
    /// A class indicates the LongArray_r structure.
    /// </summary>
    public class LongArray_r : BaseStructure
    {
        /// <summary>
        /// The number of 32-bit integers represented in structure. value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The 32-bit integer values.
        /// </summary>
        public int[] Lpl;

        /// <summary>
        /// Parse the LongArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            CValues = ReadUint();
            List<int> tempList = new List<int>();
            for (int i = 0; i < CValues; i++)
            {
                tempList.Add(ReadINT32());
            }

            Lpl = tempList.ToArray();
        }
    }
}
