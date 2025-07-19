using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.18 RopGetValidAttachments ROP
    /// A class indicates the RopGetValidAttachments ROP response Buffer.
    /// </summary>
    public class RopGetValidAttachmentsResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of integers in the AttachmentIdArray field.
        /// </summary>
        public BlockT<ushort> AttachmentIdCount;

        /// <summary>
        /// An array of 32-bit integers that represent the valid attachment identifiers of the message.
        /// </summary>
        public BlockT<int>[] AttachmentIdArray;

        /// <summary>
        /// Parse the RopGetValidAttachmentsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            OutputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                AttachmentIdCount = ParseT<ushort>();
                var attachmentIdArrays = new List<BlockT<int>>();

                for (int i = 0; i < AttachmentIdCount; i++)
                {
                    attachmentIdArrays.Add(ParseT<int>());
                }

                AttachmentIdArray = attachmentIdArrays.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetValidAttachmentsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue: {ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(AttachmentIdCount, "AttachmentIdCount");
            AddLabeledChildren(AttachmentIdArray, "AttachmentIdArray");
        }
    }
}
