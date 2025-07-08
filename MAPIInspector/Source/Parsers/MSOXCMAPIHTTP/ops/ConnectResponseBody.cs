using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Connect request type response body.
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.1 Connect
    /// </summary>
    public class ConnectResponseBody : Block
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
        /// An unsigned integer that specifies the number of milliseconds for the maximum polling interval.
        /// </summary>
        public BlockT<uint> PollsMax;

        /// <summary>
        /// An unsigned integer that specifies the number of times to retry request types.
        /// </summary>
        public BlockT<uint> RetryCount;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds for the client to wait before retrying a failed request type.
        /// </summary>
        public BlockT<uint> RetryDelay;

        /// <summary>
        /// A null-terminated ASCII string that specifies the DN prefix to be used for building message recipients.
        /// </summary>
        public BlockString DnPrefix;

        /// <summary>
        /// A null-terminated Unicode string that specifies the display name of the user who is specified in the UserDn field of the Connect request type request body.
        /// </summary>
        public BlockString DisplayName;

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
                PollsMax = ParseT<uint>();
                RetryCount = ParseT<uint>();
                RetryDelay = ParseT<uint>();
                DnPrefix = ParseStringA();
                DisplayName = ParseStringW();
            }

            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("ConnectResponseBody");
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            AddChildBlockT(ErrorCode, "ErrorCode");
            AddChildBlockT(PollsMax, "PollsMax");
            AddChildBlockT(RetryCount, "RetryCount");
            AddChildBlockT(RetryDelay, "RetryDelay");
            AddChildString(DnPrefix, "DnPrefix");
            AddChildString(DisplayName, "DisplayName");
        }
    }
}