using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.1.6 RopGetOwningServers
    ///  A class indicates the RopGetOwningServers ROP Request Buffer.
    /// </summary>
    public class RopGetOwningServersRequest : Block
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
        /// An identifier that specifies the folder for which to get owning servers.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopGetOwningServersRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            FolderId = Parse<FolderID>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetOwningServersRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(FolderId, "FolderId");
        }
    }
}
