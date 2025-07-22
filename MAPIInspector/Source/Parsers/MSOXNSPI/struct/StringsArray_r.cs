using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.6.1 StringsArray_r
    /// A class indicates the StringsArray_r structure.
    /// </summary>
    public class StringsArray_r : Block
    {
        /// <summary>
        /// The number of character string structures in aggregation. The value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// The list of character type strings in aggregation. The strings in list are NULL-terminated.
        /// </summary>
        public BlockString[] Strings; //ascii

        /// <summary>
        /// Parse the StringsArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            Count = ParseT<uint>();
            var tmpList = new List<BlockString>();
            for (ulong i = 0; i < Count; i++)
            {
                tmpList.Add(ParseStringA());
            }

            Strings = tmpList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "StringsArray_r";
            AddChildBlockT(Count, "CValues");
            AddLabeledChildren(Strings, "Strings");
        }
    }
}
