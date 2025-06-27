using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetSpecialTableRequest structure.
    /// 2.2.5.8 GetSpecialTable
    /// </summary>
    public class GetSpecialTableRequest : BaseStructure
    {
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Version field is present.
        /// </summary>
        public bool HasVersion;

        /// <summary>
        /// An unsigned integer that specifies the version number of the address book hierarchy table that the client has. 
        /// </summary>
        public uint Version;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetSpecialTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                State = new STAT();
                State.Parse(s);
            }

            HasVersion = ReadBoolean();

            if (HasVersion)
            {
                Version = ReadUint();
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}