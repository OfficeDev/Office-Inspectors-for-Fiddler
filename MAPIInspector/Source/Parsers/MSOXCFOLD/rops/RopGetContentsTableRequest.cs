using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCFOLD] 2.2.1.14 RopGetContentsTable ROP
    /// The RopGetContentsTable ROP ([MS-OXCROPS] section 2.2.4.14) is used to retrieve the contents table for a folder.
    /// </summary>
    public class RopGetContentsTableRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// These flags control the type of table.
        /// </summary>
        public BlockT<HierarchyTableFlags> TableFlags;

        /// <summary>
        /// Parse the RopGetContentsTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            TableFlags = ParseT<HierarchyTableFlags>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetContentsTableRequest";
            AddChildBlockT(RopId, "RopId");
            if (LogonId != null) AddChild(LogonId, $"LogonId:0x{LogonId.Data:X2}");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(TableFlags, "TableFlags");
        }
    }
}
