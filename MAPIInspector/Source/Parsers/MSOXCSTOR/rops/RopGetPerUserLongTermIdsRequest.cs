using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.10 RopGetPerUserLongTermIds
    /// A class indicates the RopGetPerUserLongTermIds ROP Request Buffer.
    /// </summary>
    public class RopGetPerUserLongTermIdsRequest : Block
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
        /// A GUID that specifies which database the client is querying data for
        /// </summary>
        public BlockGuid DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserLongTermIdsRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            DatabaseGuid = Parse<BlockGuid>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetPerUserLongTermIdsRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddChildGuid(DatabaseGuid, "DatabaseGuid");
        }
    }
}
