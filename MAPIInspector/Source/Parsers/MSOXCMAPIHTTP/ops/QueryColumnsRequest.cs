using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the QueryColumnsRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5.13 QueryColumns
    /// </summary>
    public class QueryColumnsRequest : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A set of bit flags that specify options to the server.
        /// </summary>
        public BlockT<uint> MapiFlags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryColumnsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            MapiFlags = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "QueryColumnsRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(MapiFlags, "MapiFlags");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }

    }
}
