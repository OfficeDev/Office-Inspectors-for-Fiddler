using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the UpdateStatResponse structure.
    /// 2.2.5.17 UpdateStat
    /// </summary>
    public class UpdateStatResponse : Block
    {
        /// <summary>
        /// A string array that informs the client as to the state of processing a request on the server.
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
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Delta field is present.
        /// </summary>
        public BlockT<bool> HasDelta;

        /// <summary>
        /// A signed integer that specifies the movement within the address book container that was specified in the State field of the request.
        /// </summary>
        public BlockT<int> Delta;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the UpdateStatResponse structure.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                HasState = ParseAs<byte, bool>();
                if (HasState)
                {
                    State = Parse<STAT>();
                    HasDelta = ParseAs<byte, bool>();
                    if (HasDelta) Delta = ParseT<int>();
                }
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("UpdateStatResponse");
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            if (ErrorCode != null) AddChild(ErrorCode, $"ErrorCode: {ErrorCode.Data.FormatErrorCode()}");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasDelta, "HasDelta");
            AddChild(Delta, "Delta");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }

    }
}