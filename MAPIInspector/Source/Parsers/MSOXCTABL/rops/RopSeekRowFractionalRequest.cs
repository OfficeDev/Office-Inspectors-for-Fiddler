using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.2.11 RopSeekRowFractional ROP
    /// The RopSeekRowFractional ROP ([MS-OXCROPS] section 2.2.5.10) moves the table cursor to an approximate position in the table.
    /// </summary>
    public class RopSeekRowFractionalRequest : Block
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
        /// An unsigned integer that represents the numerator of the fraction identifying the table position to seek to.
        /// </summary>
        BlockT<uint> Numerator;

        /// <summary>
        /// An unsigned integer that represents the denominator of the fraction identifying the table position to seek to.
        /// </summary>
        BlockT<uint> Denominator;

        /// <summary>
        /// Parse the RopSeekRowFractionalRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            Numerator = ParseT<uint>();
            Denominator = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSeekRowFractionalRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(Numerator, "Numerator");
            AddChildBlockT(Denominator, "Denominator");
        }
    }
}
