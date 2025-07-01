using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.8 FlatUIDArray_r
    /// A class indicates the FlatUIDArray_r structure.
    /// </summary>
    public class FlatUIDArray_r : BaseStructure
    {
        /// <summary>
        /// The number of FlatUID_r structures represented in the FlatUIDArray_r structure. value MUST NOT exceed 100,000.
        /// </summary>
        public uint CValues;

        /// <summary>
        /// The FlatUID_r data structures.
        /// </summary>
        public FlatUID_r[] Lpguid;

        /// <summary>
        /// Parse the FlatUIDArray_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            CValues = ReadUint();
            List<FlatUID_r> temBytes = new List<FlatUID_r>();
            for (ulong i = 0; i < CValues; i++)
            {
                FlatUID_r br = new FlatUID_r();
                br.Parse(s);
                temBytes.Add(br);
            }

            Lpguid = temBytes.ToArray();
        }
    }
}
