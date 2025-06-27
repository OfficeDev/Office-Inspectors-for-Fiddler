using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.8 RopLongTermIdFromId
    /// A class indicates the RopLongTermIdFromId ROP Request Buffer.
    /// </summary>
    public class RopLongTermIdFromIdRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the short-term ID to be converted to a long-term ID.
        /// </summary>
        public BlockBytes ObjectId; // 8 bytes

        /// <summary>
        /// Parse the RopLongTermIdFromIdRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ObjectId = ParseBytes(8);
        }

        protected override void ParseBlocks()
        {
            SetText("RopLongTermIdFromIdRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBytes(ObjectId, "ObjectId");
        }
    }
}
