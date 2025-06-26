using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the ResolveNamesRequest structure.
    ///  2.2.5.14 ResolveNames
    /// </summary>
    public class ResolveNamesRequest : BaseStructure
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
        /// A LargePropertyTagArray structure that specifies the properties that client requires for the rows returned. 
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// A Boolean value that specifies whether the NameCount and NameValues fields are present.
        /// </summary>
        public bool HasNames;

        /// <summary>
        /// An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field. TODO:
        /// </summary>
        public uint NameCount;

        /// <summary>
        /// An array of null-terminated Unicode strings. The number of strings is specified by the NameCount field. 
        /// </summary>
        public WStringArray_r Names;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResolveNamesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ResolveNamesRequest structure.</param>
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
                PropertyTags = new LargePropertyTagArray();
                PropertyTags.Parse(s);
            }

            HasNames = ReadBoolean();

            if (HasNames)
            {
                Names = new WStringArray_r();
                Names.Parse(s);
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