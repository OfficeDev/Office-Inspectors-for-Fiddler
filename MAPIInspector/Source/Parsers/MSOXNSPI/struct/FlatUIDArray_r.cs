using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.2 Property Values
    /// [MS-OXNSPI] 2.2.2.8 FlatUIDArray_r Structure
    /// A class indicates the FlatUIDArray_r structure.
    /// </summary>
    public class FlatUIDArray_r : Block
    {
        /// <summary>
        /// The number of FlatUID_r structures represented in the FlatUIDArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The FlatUID_r data structures.
        /// </summary>
        public FlatUID_r[] Lpguid;

        /// <summary>
        /// Parse the FlatUIDArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tmpFlat = new List<FlatUID_r>();
            for (ulong i = 0; i < CValues; i++)
            {
                tmpFlat.Add(Parse<FlatUID_r>());
            }

            Lpguid = tmpFlat.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "FlatUIDArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(Lpguid, "Lpguid");
        }
    }
}
