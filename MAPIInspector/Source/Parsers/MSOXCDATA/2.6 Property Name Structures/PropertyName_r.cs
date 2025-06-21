namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    /// 2.6.2 PropertyName_r Structure
    /// </summary>
    public class PropertyName_r : BaseStructure
    {
        /// <summary>
        /// Encodes the GUID field of the PropertyName structure, as specified in section 2.6.1.
        /// </summary>
        public Guid GUID;

        /// <summary>
        /// All clients and servers MUST set this value to 0x00000000.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// This value encodes the LID field in the PropertyName structure, as specified in section 2.6.1.
        /// </summary>
        public uint LID;

        /// <summary>
        /// Parse the PropertyName_r structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyName_r structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            GUID = ReadGuid();
            Reserved = ReadUint();
            LID = ReadUint();
        }
    }
}
