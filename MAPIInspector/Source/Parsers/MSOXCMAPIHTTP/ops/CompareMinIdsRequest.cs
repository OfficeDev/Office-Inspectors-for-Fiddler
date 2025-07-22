using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the CompareMinIdsRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5 Request Types for Address Book Server Endpoint
    /// [MS-OXCMAPIHTTP] 2.2.5.3 CompareMinIds
    /// </summary>
    public class CompareMinIdsRequest : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<byte> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A MinimalEntryID structure ([MS-OXNSPI] section 2.2.9.1) that specifies the Minimal Entry ID of the first object.
        /// </summary>
        public MinimalEntryID MinimalId1;

        /// <summary>
        /// A MinimalEntryID structure that specifies the Minimal Entry ID of the second object.
        /// </summary>
        public MinimalEntryID MinimalId2;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseT<byte>();

            if (HasState != 0) State = Parse<STAT>();

            MinimalId1 = Parse<MinimalEntryID>();
            MinimalId2 = Parse<MinimalEntryID>();
            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "CompareMinIdsRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChild(MinimalId1, "MinimalId1");
            AddChild(MinimalId2, "MinimalId2");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
