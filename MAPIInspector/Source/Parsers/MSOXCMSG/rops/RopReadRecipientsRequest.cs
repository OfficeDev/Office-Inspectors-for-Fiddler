using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.6 RopReadRecipients ROP
    /// A class indicates the RopReadRecipients ROP request Buffer.
    /// </summary>
    public class RopReadRecipientsRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the starting index for the recipients (2) to be retrieved.
        /// </summary>
        public BlockT<uint> RowId;

        /// <summary>
        /// Reserved. This field MUST be set to 0x0000.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// Parse the RopReadRecipientsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            RowId = ParseT<uint>();
            Reserved = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopReadRecipientsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(RowId, "RowId");
            AddChildBlockT(Reserved, "Reserved");
        }
    }
}
