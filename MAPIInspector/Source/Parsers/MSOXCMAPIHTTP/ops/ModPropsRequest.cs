using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ModPropsRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.11 ModProps
    /// </summary>
    public class ModPropsRequest : Block
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
        /// A LargePropertyTagArray structure that specifies the properties to be removed.
        /// </summary>
        public LargePropertyTagArray PropertiesTags;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyValues field is present.
        /// </summary>
        public BlockT<bool> HasPropertyValues;

        /// <summary>
        /// An AddressBookPropertyValueList structure that specifies the values of the properties to be modified.
        /// </summary>
        public AddressBookPropertyValueList PropertyValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModPropsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            HasPropertyTags = ParseAs<byte, bool>();
            if (HasPropertyTags) PropertiesTags = Parse<LargePropertyTagArray>();
            HasPropertyValues = ParseAs<byte, bool>();
            if (HasPropertyValues) PropertyValues = Parse<AddressBookPropertyValueList>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "ModPropsRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasPropertyTags, "HasPropertyTags");
            AddChild(PropertiesTags, "PropertiesTags");
            AddChildBlockT(HasPropertyValues, "HasPropertyValues");
            AddChild(PropertyValues, "PropertyValues");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
