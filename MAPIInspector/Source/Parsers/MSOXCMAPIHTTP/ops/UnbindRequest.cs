using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the UnbindRequest structure.
    /// [MS-OXCMAPIHTTP] 2.2.5 Request Types for Address Book Server Endpoint
    /// [MS-OXCMAPIHTTP] 2.2.5.2 Unbind
    /// </summary>
    public class UnbindRequest : Block
    {
        /// <summary>
        /// The reserved field
        /// </summary>
        public BlockT<uint> Reserved;

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
            Reserved = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "UnbindRequest";
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}
