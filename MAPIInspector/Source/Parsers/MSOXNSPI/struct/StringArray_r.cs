using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.6 StringArray_r
    /// A class indicates the StringArray_r structure.
    /// </summary>
    public class StringArray_r : Block
    {
        /// <summary>
        /// The number of 8-bit character string references represented in the StringArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The 8-bit character string references. The strings referred to are NULL-terminated.
        /// </summary>
        public BlockString[] LppszA; // ascii

        /// <summary>
        /// Parse the StringArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tmpList = new List<BlockString>();
            for (ulong i = 0; i < CValues; i++)
            {
                tmpList.Add(ParseStringA());
            }

            LppszA = tmpList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "StringArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(LppszA, "LppszA");
        }
    }
}
