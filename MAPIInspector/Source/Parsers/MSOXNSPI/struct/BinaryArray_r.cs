using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.7 BinaryArray_r
    /// A class indicates the BinaryArray_r structure.
    /// </summary>
    public class BinaryArray_r : Block
    {
        /// <summary>
        /// The number of Binary_r data structures represented in the BinaryArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The Binary_r data structures.
        /// </summary>
        public Binary_r[] Lpbin;

        /// <summary>
        /// Parse the BinaryArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tmpBin = new List<Binary_r>();
            for (ulong i = 0; i < CValues; i++)
            {
                tmpBin.Add(Parse<Binary_r>());
            }

            Lpbin = tmpBin.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "BinaryArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(Lpbin, "Lpbin");
        }
    }
}
