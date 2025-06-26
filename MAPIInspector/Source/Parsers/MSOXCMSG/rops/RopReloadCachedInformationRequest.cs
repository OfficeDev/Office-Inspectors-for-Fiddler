using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.7 RopReloadCachedInformation ROP
    /// A class indicates the RopReloadCachedInformation ROP request Buffer.
    /// </summary>
    public class RopReloadCachedInformationRequest : Block
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
        /// Reserved. This field MUST be set to 0x0000.
        /// </summary>
        public BlockT<ushort> Reserved;

        /// <summary>
        /// Parse the RopReloadCachedInformationRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Reserved = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopReloadCachedInformationRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(Reserved, "Reserved");
        }
    }
}
