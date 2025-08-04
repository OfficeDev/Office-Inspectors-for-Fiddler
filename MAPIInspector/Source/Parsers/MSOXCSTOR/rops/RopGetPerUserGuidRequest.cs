using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.3.11.1 RopGetPerUserGuid ROP Request Buffer
    /// A class indicates the RopGetPerUserGuid ROP Request Buffer.
    /// </summary>
    public class RopGetPerUserGuidRequest : Block
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
        /// A LongTermID structure that specifies the public folder.
        /// </summary>
        public LongTermID LongTermId;

        /// <summary>
        /// Parse the RopGetPerUserGuidRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            LongTermId = Parse<LongTermID>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPerUserGuidRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(LongTermId, "LongTermId");
        }
    }
}
