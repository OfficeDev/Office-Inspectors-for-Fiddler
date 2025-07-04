using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Disconnect request type.
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.3 Disconnect
    /// </summary>
    public class DisconnectRequestBody : Block
    {
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
            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("DisconnectRequestBody");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}