using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.4 RopRestrict ROP
    /// The RopRestrict ROP ([MS-OXCROPS] section 2.2.5.3) establishes a restriction on a table.
    /// </summary>
    public class RopRestrictRequest : Block
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
        /// A flags structure that contains flags that control this operation.
        /// </summary>
        public BlockT<AsynchronousFlags> RestrictFlags;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        BlockT<ushort> RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this table The size of this field is specified by the RestrictionDataSize field.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// Parse the RopRestrictRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            RestrictFlags = ParseT<AsynchronousFlags>();
            RestrictionDataSize = ParseT<ushort>();
            if (RestrictionDataSize > 0)
            {
                RestrictionData = new RestrictionType();
                RestrictionData.Parse(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopRestrictRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(RestrictFlags, "RestrictFlags");
            AddChildBlockT(RestrictionDataSize, "RestrictionDataSize");
            AddChild(RestrictionData, "RestrictionData");
        }
    }
}
