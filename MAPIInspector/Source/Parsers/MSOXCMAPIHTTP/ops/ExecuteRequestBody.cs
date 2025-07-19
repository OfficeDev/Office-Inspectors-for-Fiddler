using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Execute request type.
    /// 2.2.2.1 Common Request Format
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.2.1 Execute Request Type Request Body
    /// </summary>
    public class ExecuteRequestBody : Block
    {
        /// <summary>
        /// An unsigned integer that specify to the server how to build the ROP responses in the RopBuffer field of the Execute request type success response body.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        /// </summary>
        public BlockT<uint> RopBufferSize;

        /// <summary>
        /// An structure of bytes that constitute the ROP request payload.
        /// </summary>
        public RgbInputBuffer RopBuffer;

        /// <summary>
        /// An unsigned integer that specifies the maximum size for the RopBuffer field of the Execute request type success response body.
        /// </summary>
        public BlockT<uint> MaxRopOut;

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
            Flags = ParseT<uint>();
            RopBufferSize = ParseT<uint>();
            RopBuffer = new RgbInputBuffer(RopBufferSize);
            RopBuffer.Parse(parser);
            MaxRopOut = ParseT<uint>();
            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = Parse<ExtendedBuffer>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "ExecuteRequestBody";
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(RopBufferSize, "RopBufferSize");
            AddChild(RopBuffer, "RopBuffer");
            AddChildBlockT(MaxRopOut, "MaxRopOut");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}