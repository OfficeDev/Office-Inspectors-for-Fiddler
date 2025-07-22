using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.15.3 RopRelease
    /// A class indicates the RopRelease ROP Request Buffer.
    /// </summary>
    public class RopReleaseRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x01.
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
        /// Parse the RopReleaseRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopReleaseRequest";
            AddChildBlockT(RopId, "RopId");
            if (LogonId != null) AddChild(LogonId, $"LogonId:0x{LogonId:X2}");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
        }
    }
}
