using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.6 RopReadRecipients ROP
    /// A class indicates the RopReadRecipients ROP response Buffer.
    /// </summary>
    public class RopReadRecipientsResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientRows field.
        /// </summary>
        public BlockT<byte> RowCount;

        /// <summary>
        /// A list of ReadRecipientRow structures.
        /// </summary>
        public ReadRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopReadRecipientsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                RowCount = ParseT<byte>();
                var readRecipientRows = new List<ReadRecipientRow>();

                for (int i = 0; i < RowCount; i++)
                {
                    readRecipientRows.Add(Parse<ReadRecipientRow>());
                }

                RecipientRows = readRecipientRows.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopReadRecipientsResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChildBlockT(RowCount, "RowCount");
            AddLabeledChildren(RecipientRows, "RecipientRows");
        }
    }
}
