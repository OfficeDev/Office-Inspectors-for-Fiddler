using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the UpdateStatRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.17 UpdateStat
    /// </summary>
    public class UpdateStatRequest : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the client is requesting a value to be returned in the Delta field of the response.
        /// </summary>
        public BlockT<bool> DeltaRequested;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the UpdateStatRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            DeltaRequested = ParseAs<byte, bool>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "UpdateStatRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(DeltaRequested, "DeltaRequested");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
