using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Execute request type response body.
    /// 2.2.2.2 Common Response Format
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.2.2 Execute Request Type Success Response Body
    /// 2.2.4.2.3 Execute Request Type Failure Response Body
    /// </summary>
    public class ExecuteResponseBody : Block
    {
        /// <summary>
        /// A string array that informs the client as to the state of processing a request on the server
        /// </summary>
        public BlockString[] MetaTags;

        /// <summary>
        /// A string array that specifies additional header information.
        /// </summary>
        public BlockString[] AdditionalHeaders;

        /// <summary>
        /// An unsigned integer that specifies the status of the request.
        /// </summary>
        public BlockT<uint> StatusCode;

        /// <summary>
        /// An unsigned integer that specifies the return status of the operation.
        /// </summary>
        public BlockT<ErrorCodes> ErrorCode;

        /// <summary>
        /// The reserved flag. The server MUST set this field to 0x00000000 and the client MUST ignore this field.
        /// </summary>
        public BlockT<uint> Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        /// </summary>
        public BlockT<uint> RopBufferSize;

        /// <summary>
        /// A structure of bytes that constitute the ROP responses payload.
        /// </summary>
        public RgbOutputBufferPack RopBuffer;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                Flags = ParseT<uint>();
                RopBufferSize = ParseT<uint>();
                RopBuffer = new RgbOutputBufferPack(RopBufferSize);
                RopBuffer.Parse(parser);
            }

            if (parser.RemainingBytes >= sizeof(uint))
            {
                AuxiliaryBufferSize = ParseT<uint>();

                if (AuxiliaryBufferSize > 0)
                {
                    AuxiliaryBuffer = Parse<ExtendedBuffer>();
                }
            }
        }

        protected override void ParseBlocks()
        {
            SetText("ExecuteResponseBody");
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            if (ErrorCode != null) AddChild(ErrorCode, $"ErrorCode: {ErrorCode.Data.FormatErrorCode()}");
            AddChildBlockT(Flags, "Flags");
            AddChildBlockT(RopBufferSize, "RopBufferSize");
            AddChild(RopBuffer, "RopBuffer");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}