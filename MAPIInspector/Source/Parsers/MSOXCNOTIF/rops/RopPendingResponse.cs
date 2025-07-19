using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.14.3.1 RopPending ROP Response Buffer
    /// A class indicates the RopPending ROP Response Buffer.
    /// </summary>
    public class RopPendingResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x6E.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies which session has pending notifications.
        /// </summary>
        public BlockT<ushort> SessionIndex;

        /// <summary>
        /// Parse the RopPendingResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            SessionIndex = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopPendingResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(SessionIndex, "SessionIndex");
        }
    }
}
