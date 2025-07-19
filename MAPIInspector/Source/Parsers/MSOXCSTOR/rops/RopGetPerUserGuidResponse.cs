using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.11 RopGetPerUserGuid
    /// A class indicates the RopGetPerUserGuid ROP Response Buffer.
    /// </summary>
    public class RopGetPerUserGuidResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// A GUID that specifies the database for which per-user information was obtained.
        /// </summary>
        public BlockGuid DatabaseGuid;

        /// <summary>
        /// Parse the RopGetPerUserGuidResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue == ErrorCodes.Success)
            {
                DatabaseGuid = Parse<BlockGuid>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopGetPerUserGuidResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            this.AddChildGuid(DatabaseGuid, "DatabaseGuid");
        }
    }
}
