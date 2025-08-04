using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ResolveNamesRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.14 ResolveNames
    /// </summary>
    public class ResolveNamesRequest : Block
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
        /// A Boolean value that specifies whether the PropertyTags field is present.
        /// </summary>
        public BlockT<bool> HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties that client requires for the rows returned.
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// A Boolean value that specifies whether the NameCount and NameValues fields are present.
        /// </summary>
        public BlockT<bool> HasNames;

        /// <summary>
        /// An array of null-terminated Unicode strings. The number of strings is specified by the NameCount field.
        /// </summary>
        public WStringArray_r Names;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResolveNamesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            HasPropertyTags = ParseAs<byte, bool>();
            if (HasPropertyTags) PropertyTags = Parse<LargePropertyTagArray>();
            HasNames = ParseAs<byte, bool>();
            if (HasNames) Names = Parse<WStringArray_r>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "ResolveNamesRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasPropertyTags, "HasPropertyTags");
            AddChild(PropertyTags, "PropertyTags");
            AddChildBlockT(HasNames, "HasNames");
            AddChild(Names, "Names");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
