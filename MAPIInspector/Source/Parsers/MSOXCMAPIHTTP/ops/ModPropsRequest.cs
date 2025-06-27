using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ModPropsRequest structure.
    /// 2.2.5.11 ModProps
    /// </summary>
    public class ModPropsRequest : BaseStructure
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.  
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags field is present.
        /// </summary>
        public bool HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties to be removed. 
        /// </summary>
        public LargePropertyTagArray PropertiesTags;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyValues field is present.
        /// </summary>
        public bool HasPropertyValues;

        /// <summary>
        /// An AddressBookPropertyValueList structure that specifies the values of the properties to be modified. 
        /// </summary>
        public AddressBookPropertyValueList PropertyValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModPropsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ModPropsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Reserved = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                State = new STAT();
                State.Parse(s);
            }

            HasPropertyTags = ReadBoolean();

            if (HasPropertyTags)
            {
                PropertiesTags = new LargePropertyTagArray();
                PropertiesTags.Parse(s);
            }

            HasPropertyValues = ReadBoolean();

            if (HasPropertyValues)
            {
                PropertyValues = new AddressBookPropertyValueList();
                PropertyValues.Parse(s);
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}