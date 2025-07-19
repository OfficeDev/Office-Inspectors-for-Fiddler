using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ResortRestrictionResponse structure.
    /// 2.2.5.15 ResortRestriction
    /// </summary>
    public class ResortRestrictionResponse : Block
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
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public BlockT<bool> HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the MinimalIds field.
        /// </summary>
        public BlockT<uint> MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that compose a restricted address book container.
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionResponse structure.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                HasState = ParseAs<byte, bool>();

                if (HasState) State = Parse<STAT>();

                HasMinimalIds = ParseAs<byte, bool>();

                if (HasMinimalIds)
                {
                    MinimalIdCount = ParseT<uint>();
                    var miniEIDList = new List<MinimalEntryID>();
                    for (int i = 0; i < MinimalIdCount; i++)
                    {
                        miniEIDList.Add(Parse<MinimalEntryID>());
                    }

                    MinimalIds = miniEIDList.ToArray();
                }

                AuxiliaryBufferSize = ParseT<uint>();
                if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "ResortRestrictionResponse";
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            this.AddError(ErrorCode, "ErrorCode ");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasMinimalIds, "HasMinimalIds");
            AddChildBlockT(MinimalIdCount, "MinimalIdCount");
            AddLabeledChildren(MinimalIds, "MinimalIds");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}