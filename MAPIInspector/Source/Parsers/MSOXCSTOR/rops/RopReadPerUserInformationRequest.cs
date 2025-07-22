using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.12 RopReadPerUserInformation
    /// A class indicates the RopReadPerUserInformation ROP Request Buffer.
    /// </summary>
    public class RopReadPerUserInformationRequest : Block
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
        /// A LongTermID structure that specifies the folder for which to get per-user information.
        /// </summary>
        public LongTermID FolderId;

        /// <summary>
        /// Reserved field.
        /// </summary>
        public BlockT<byte> Reserved;

        /// <summary>
        /// An unsigned integer that specifies the location at which to start reading within the per-user information to be retrieved.
        /// </summary>
        public BlockT<uint> DataOffset;

        /// <summary>
        /// An unsigned integer that specifies the maximum number of bytes of per-user information to be retrieved.
        /// </summary>
        public BlockT<ushort> MaxDataSize;

        /// <summary>
        /// Parse the RopReadPerUserInformationRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            FolderId = Parse<LongTermID>();
            Reserved = ParseT<byte>();
            DataOffset = ParseT<uint>();
            MaxDataSize = ParseT<ushort>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopReadPerUserInformationRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(FolderId, "FolderId");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(DataOffset, "DataOffset");
            AddChildBlockT(MaxDataSize, "MaxDataSize");
        }
    }
}
