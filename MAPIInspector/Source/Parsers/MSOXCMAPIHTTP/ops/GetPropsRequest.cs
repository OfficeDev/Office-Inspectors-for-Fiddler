namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the GetPropsRequest structure.
    ///  2.2.5.7 GetProps
    /// </summary>
    public class GetPropsRequest : BaseStructure
    {
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags field is present
        /// </summary>
        public bool HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that contains the property tags of the properties that the client is requesting. 
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetPropsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Flags = ReadUint();
            HasState = ReadBoolean();

            if (HasState)
            {
                State = new STAT();
                State.Parse(s);
            }

            HasPropertyTags = ReadBoolean();

            if (HasPropertyTags)
            {
                PropertyTags = new LargePropertyTagArray();
                PropertyTags.Parse(s);
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