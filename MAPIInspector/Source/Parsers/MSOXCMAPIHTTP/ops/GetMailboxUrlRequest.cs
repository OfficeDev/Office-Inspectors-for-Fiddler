using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetMailboxUrlRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.18 GetMailboxUrl
    /// </summary>
    public class GetMailboxUrlRequest : Block
    {
        /// <summary>
        /// Not used. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A null-terminated Unicode string that specifies the distinguished name (DN) of the mailbox server for which to look up the URL.
        /// </summary>
        public BlockString ServerDn;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMailboxUrlRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            ServerDn = ParseStringW();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "GetMailboxUrlRequest";
            AddChildBlockT(Flags, "Flags");
            AddChild(ServerDn, "ServerDn");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
