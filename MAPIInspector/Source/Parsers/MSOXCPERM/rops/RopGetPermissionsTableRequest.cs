using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCPERM] 2.2.1 RopGetPermissionsTable ROP
    /// The RopGetPermissionsTable ROP ([MS-OXCROPS] section 2.2.10.2) retrieves a Server object handle to a Table object, which is then used in other ROP requests to retrieve the current permissions list on a folder.
    /// </summary>
    public class RopGetPermissionsTableRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x3E.
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of table.
        /// </summary>
        public BlockT<TableFlagsRopGetPermissionsTable> TableFlags;

        /// <summary>
        /// Parse the RopGetPermissionsTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            TableFlags = ParseT<TableFlagsRopGetPermissionsTable>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPermissionsTableRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(TableFlags, "TableFlags");
        }
    }
}
