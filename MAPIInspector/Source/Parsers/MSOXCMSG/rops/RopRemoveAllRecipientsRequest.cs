using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.4 RopRemoveAllRecipients ROP
    /// A class indicates the RopRemoveAllRecipients ROP request Buffer.
    /// </summary>
    public class RopRemoveAllRecipientsRequest : Block
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
        /// Reserved. The client SHOULD set this field to 0x00000000.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// Parse the RopRemoveAllRecipientsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Reserved = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopRemoveAllRecipientsRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(Reserved, "Reserved");
        }
    }
}
