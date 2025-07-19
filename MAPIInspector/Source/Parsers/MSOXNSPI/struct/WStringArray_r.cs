using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.9 WStringArray_r
    /// A class indicates the WStringArray_r structure.
    /// </summary>
    public class WStringArray_r : Block
    {
        /// <summary>
        /// The number of Unicode character string references represented in the WStringArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The Unicode character string references. The strings referred to are NULL-terminated.
        /// </summary>
        public BlockString[] LppszW; // unicode

        /// <summary>
        /// Parse the WStringArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tmpList = new List<BlockString>();
            for (ulong i = 0; i < CValues; i++)
            {
                tmpList.Add(ParseStringW());
            }

            LppszW = tmpList.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "WStringArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(LppszW, "LppszW");
        }
    }
}
