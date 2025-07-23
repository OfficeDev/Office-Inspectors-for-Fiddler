using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.2 Property Values
    /// [MS-OXNSPI] 2.2.2.5 LongArray_r Structure
    /// A class indicates the LongArray_r structure.
    /// </summary>
    public class LongArray_r : Block
    {
        /// <summary>
        /// The number of 32-bit integers represented in structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The 32-bit integer values.
        /// </summary>
        public BlockT<int>[] Lpl;

        /// <summary>
        /// Parse the LongArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tempList = new List<BlockT<int>>();
            for (int i = 0; i < CValues; i++)
            {
                tempList.Add(ParseT<int>());
            }

            Lpl = tempList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "LongArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(Lpl, "Lpl");
        }
    }
}
