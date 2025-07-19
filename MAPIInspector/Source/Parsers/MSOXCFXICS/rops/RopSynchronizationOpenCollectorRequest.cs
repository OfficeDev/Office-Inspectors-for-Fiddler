using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationOpenCollector ROP Request Buffer.
    /// 2.2.3.2.4.1.1 RopSynchronizationOpenCollector ROP Request Buffer
    /// </summary>
    public class RopSynchronizationOpenCollectorRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether synchronization upload context is for contents or for hierarchy.
        /// </summary>
        public BlockT<bool> IsContentsCollector;

        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            IsContentsCollector = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopSynchronizationOpenCollectorRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(IsContentsCollector, "IsContentsCollector");
        }
    }
}
