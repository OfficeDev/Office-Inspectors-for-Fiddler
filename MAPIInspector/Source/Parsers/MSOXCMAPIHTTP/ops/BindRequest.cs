using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Bind request type request body.
    /// 2.2.5 Request Types for Address Book Server Endpoint
    /// 2.2.5.1 Bind
    /// </summary>
    public class BindRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<byte> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

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
            Flags = ParseT<uint>();
            HasState = ParseT<byte>();
            if (HasState != 0) State = Parse<STAT>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "BindRequest";
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}