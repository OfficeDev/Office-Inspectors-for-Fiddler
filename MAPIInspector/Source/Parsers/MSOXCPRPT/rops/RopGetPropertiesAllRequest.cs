using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.3 RopGetPropertiesAll
    /// A class indicates the RopGetPropertiesAll ROP Request Buffer.
    /// </summary>
    public class RopGetPropertiesAllRequest : Block
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
        /// An unsigned integer that specifies the maximum size allowed for a property value returned.
        /// </summary>
        public BlockT<ushort> PropertySizeLimit;

        /// <summary>
        /// A Boolean that specifies whether to return string properties in multibyte Unicode.
        /// </summary>
        public BlockT<bool> WantUnicode;

        /// <summary>
        /// Parse the RopGetPropertiesAllRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            PropertySizeLimit = ParseT<ushort>();
            WantUnicode = ParseAs<ushort, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPropertiesAllRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(PropertySizeLimit, "PropertySizeLimit");
            AddChildBlockT(WantUnicode, "WantUnicode");
        }
    }
}
