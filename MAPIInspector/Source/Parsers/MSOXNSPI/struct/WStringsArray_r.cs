using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.6.2 WStringsArray_r
    /// A class indicates the WStringsArray_r structure.
    /// </summary>
    public class WStringsArray_r : Block
    {
        /// <summary>
        /// The number of character strings structures in aggregation. The value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// The list of wchar_t type strings in aggregation. The strings in list are NULL-terminated.
        /// </summary>
        public BlockString[] Strings; // unicode

        /// <summary>
        /// Parse the WStringsArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            Count = ParseT<uint>();
            var tmpList = new List<BlockString>();
            for (ulong i = 0; i < Count; i++)
            {
                tmpList.Add(ParseStringW());
            }

            Strings = tmpList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "WStringsArray_r";
            AddChildBlockT(Count, "CValues");
            AddLabeledChildren(Strings, "Strings");
        }
    }
}
