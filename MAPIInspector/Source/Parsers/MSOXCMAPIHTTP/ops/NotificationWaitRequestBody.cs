using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the NotificationWait request type response body.
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.4 NotificationWait
    /// </summary>
    public class NotificationWaitRequestBody : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse method
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("NotificationWaitRequestBody");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}