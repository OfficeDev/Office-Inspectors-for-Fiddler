using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.2 Property Values
    /// [MS-OXNSPI] 2.2.2.4 ShortArray_r Structure
    /// A class indicates the ShortArray_r structure.
    /// </summary>
    public class ShortArray_r : Block
    {
        /// <summary>
        /// The number of 16-bit integer values represented in the ShortArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The 16-bit integer values.
        /// </summary>
        public BlockT<short>[] Lpi;

        /// <summary>
        /// Parse the ShortArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tempList = new List<BlockT<short>>();
            for (ulong i = 0; i < CValues; i++)
            {
                tempList.Add(ParseT<short>());
            }

            Lpi = tempList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "ShortArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(Lpi, "Lpi");
        }
    }
}
