using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2 RopGetRulesTable ROP
    /// The RopGetRulesTable ROP ([MS-OXCROPS] section 2.2.11.2) creates a Table object through which the client can access the standard rules in a folder using table operations as specified in [MS-OXCTABL].
    /// </summary>
    public class RopGetRulesTableRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of table.
        /// </summary>
        public BlockT<TableFlags> TableFlags;

        /// <summary>
        /// Parse the RopGetRulesTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetRulesTableRequest structure.</param>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            TableFlags = ParseT<TableFlags>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetRulesTableRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(TableFlags, "TableFlags");
        }
    }
}
