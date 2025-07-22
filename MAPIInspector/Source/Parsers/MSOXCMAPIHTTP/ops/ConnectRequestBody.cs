using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Connect request type.
    /// [MS-OXCMAPIHTTP] 2.2.4 Request Types for Mailbox Server Endpoint
    /// [MS-OXCMAPIHTTP] 2.2.4.1 Connect
    /// </summary>
    public class ConnectRequestBody : Block
    {
        /// <summary>
        /// A null-terminated ASCII string that specifies the DN of the user who is requesting the connection.
        /// </summary>
        public BlockString UserDn;

        /// <summary>
        /// A set of flags that designate the type of connection being requested.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// An unsigned integer that specifies the code page that the server is being requested to use for string values of properties.
        /// </summary>
        public BlockT<uint> DefaultCodePage;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for sorting.
        /// </summary>
        public BlockT<uint> LcidSort;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for everything other than sorting.
        /// </summary>
        public BlockT<uint> LcidString;

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
            UserDn = ParseStringA();
            Flags = ParseT<uint>();
            DefaultCodePage = ParseT<uint>();
            LcidSort = ParseT<uint>();
            LcidString = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "ConnectRequestBody";
            AddChildString(UserDn, "UserDn");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(DefaultCodePage, "DefaultCodePage");
            AddChildBlockT(LcidSort, "LcidSort");
            AddChildBlockT(LcidString, "LcidString");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
