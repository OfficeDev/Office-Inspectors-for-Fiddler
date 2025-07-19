using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the NotificationWait request type response body.
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.4 NotificationWait
    /// </summary>
    public class NotificationWaitResponseBody : Block
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
        /// An unsigned integer that indicates whether an event is pending on the Session Context.
        /// </summary>
        public BlockT<uint> EventPending;

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
                EventPending = ParseT<uint>();
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("NotificationWaitResponseBody");
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            if (ErrorCode != null) AddChild(ErrorCode, $"ErrorCode: {ErrorCode.Data.FormatErrorCode()}");
            AddChildBlockT(EventPending, "EventPending");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }

    }
}