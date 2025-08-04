using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetAddressBookUrlRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.19 GetAddressBookUrl
    /// </summary>
    public class GetAddressBookUrlRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A null-terminated Unicode string that specifies the distinguished name (DN) of the user's mailbox.
        /// </summary>
        public BlockString UserDn;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetAddressBookUrlRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            UserDn = ParseStringW();
            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "GetAddressBookUrlRequest";
            AddChildBlockT(Flags, "Flags");
            AddChildString(UserDn, "UserDn");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
