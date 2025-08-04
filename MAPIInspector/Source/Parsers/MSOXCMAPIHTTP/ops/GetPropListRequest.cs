using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetPropListRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.6 GetPropList
    /// </summary>
    public class GetPropListRequest : Block
    {
        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// A MinimalEntryID structure that specifies the object for which to return properties.
        /// </summary>
        public MinimalEntryID MinimalId;

        /// <summary>
        /// An unsigned integer that specifies the code page that the server is being requested to use for string values of properties.
        /// </summary>
        public BlockT<uint> CodePage;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropListRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Flags = ParseT<uint>();
            MinimalId = Parse<MinimalEntryID>();
            CodePage = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "GetPropListRequest";
            AddChildBlockT(Flags, "Flags");
            AddChild(MinimalId, "MinimalId");
            AddChildBlockT(CodePage, "CodePage");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }

    }
}
