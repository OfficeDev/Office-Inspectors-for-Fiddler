using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetSpecialTableRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.8 GetSpecialTable
    /// </summary>
    public class GetSpecialTableRequest : Block
    {
        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Version field is present.
        /// </summary>
        public BlockT<bool> HasVersion;

        /// <summary>
        /// An unsigned integer that specifies the version number of the address book hierarchy table that the client has.
        /// </summary>
        public BlockT<uint> Version;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            HasVersion = ParseAs<byte, bool>();
            if (HasVersion) Version = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "GetSpecialTableRequest";
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasVersion, "HasVersion");
            AddChild(Version, "Version");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
