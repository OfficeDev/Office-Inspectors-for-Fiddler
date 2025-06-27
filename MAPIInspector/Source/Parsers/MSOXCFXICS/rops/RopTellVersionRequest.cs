using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopTellVersion ROP Request Buffer.
    /// 2.2.3.1.1.6.1 RopTellVersion ROP Request Buffer
    /// </summary>
    public class RopTellVersionRequest : Block
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
        /// An array of three unsigned 16-bit integers that contains the version information for the other server. 
        /// </summary>
        public BlockBytes Version;

        /// <summary>
        /// Parse the RopTellVersionRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Version = ParseBytes(6);
        }

        protected override void ParseBlocks()
        {
            SetText("RopTellVersionRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBytes(Version, "Version");
        }
    }
}
