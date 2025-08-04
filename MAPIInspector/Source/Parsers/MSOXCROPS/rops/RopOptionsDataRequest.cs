using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.7.9 RopOptionsData
    /// A class indicates the RopOptionsData ROP Request Buffer.
    /// </summary>
    public class RopOptionsDataRequest : Block
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
        /// A null-terminated ASCII string that specifies the address type that options are to be returned for.
        /// </summary>
        public BlockString AddressType;

        /// <summary>
        /// A boolean that specifies whether the help file data is to be returned in a format that is suited for 32-bit machines.
        /// </summary>
        public BlockT<byte> WantWin32;

        /// <summary>
        /// Parse the RopOptionsDataRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            AddressType = ParseStringA();
            WantWin32 = ParseT<byte>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopOptionsDataRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildString(AddressType, "AddressType");
            AddChildBlockT(WantWin32, "WantWin32");
        }
    }
}
