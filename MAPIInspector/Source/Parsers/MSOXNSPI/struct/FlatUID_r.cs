using System;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.1 FlatUID_r
    /// A class indicates the FlatUID_r structure.
    /// </summary>
    public class FlatUID_r : BaseStructure
    {
        /// <summary>
        /// Encodes the ordered bytes of the FlatUID data structure.
        /// </summary>
        public Guid Ab;

        /// <summary>
        /// Parse the FlatUID_r payload of session.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Ab = ReadGuid();
        }
    }
}
