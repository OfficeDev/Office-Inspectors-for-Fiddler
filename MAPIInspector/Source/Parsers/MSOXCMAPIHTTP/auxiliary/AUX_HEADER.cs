using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The AUX_HEADER structure provides information about the auxiliary block structures that follow it. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// Section 2.2.2.2 AUX_HEADER Structure
    /// </summary>
    public class AUX_HEADER : BaseStructure
    {
        /// <summary>
        /// The size of the AUX_HEADER structure plus any additional payload data.
        /// </summary>
        public ushort _Size;

        /// <summary>
        /// The version information of the payload data.
        /// </summary>
        public PayloadDataVersion Version;

        /// <summary>
        /// The type of auxiliary block data structure. The Type should be AuxiliaryBlockType_1 or AuxiliaryBlockType_2.
        /// </summary>
        public object Type;

        /// <summary>
        /// Parse the AUX_HEADER structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_HEADER structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            _Size = ReadUshort();
            Version = (PayloadDataVersion)ReadByte();

            if (Version == PayloadDataVersion.AUX_VERSION_1)
            {
                Type = (AuxiliaryBlockType_1)ReadByte();
            }
            else
            {
                Type = (AuxiliaryBlockType_2)ReadByte();
            }
        }
    }
}