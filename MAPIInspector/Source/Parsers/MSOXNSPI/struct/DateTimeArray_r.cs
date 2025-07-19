using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.10 DateTimeArray_r
    /// A class indicates the DateTimeArray_r structure.
    /// </summary>
    public class DateTimeArray_r : Block
    {
        /// <summary>
        /// The number of FILETIME data structures represented in the DateTimeArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public BlockT<uint> CValues;

        /// <summary>
        /// The FILETIME data structures.
        /// </summary>
        public PtypTime[] Lpft;

        /// <summary>
        /// Parse the DateTimeArray_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            CValues = ParseT<uint>();
            var tmpTime = new List<PtypTime>();
            for (ulong i = 0; i < CValues; i++)
            {
                tmpTime.Add(Parse<PtypTime>());
            }

            Lpft = tmpTime.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "DateTimeArray_r";
            AddChildBlockT(CValues, "CValues");
            AddLabeledChildren(Lpft, "Lpft");
        }
    }
}
