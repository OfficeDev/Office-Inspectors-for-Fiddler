using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.10 DateTimeArray_r
    /// A class indicates the DateTimeArray_r structure.
    /// </summary>
    public class DateTimeArray_r : BaseStructure
    {
        /// <summary>
        /// The number of FILETIME data structures represented in the DateTimeArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The FILETIME data structures.
        /// </summary>
        public PtypTime[] Lpft;

        /// <summary>
        /// Parse the DateTimeArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            CValues = ReadUint();
            List<PtypTime> temBytes = new List<PtypTime>();
            for (ulong i = 0; i < CValues; i++)
            {
                PtypTime pt = new PtypTime();
                pt.Parse(s);
                temBytes.Add(pt);
            }

            Lpft = temBytes.ToArray();
        }
    }
}
