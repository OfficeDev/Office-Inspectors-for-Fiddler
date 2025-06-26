using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.3 RopSaveChangesMessage ROP
    /// A class indicates the RopSaveChangesMessage ROP request Buffer.
    /// </summary>
    public class RopSaveChangesMessageRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response.
        /// </summary>
        public BlockT<byte> ResponseHandleIndex;

        /// <summary>
        ///  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that specify how the save operation behaves.
        /// </summary>
        public BlockT<SaveFlags> SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesMessageRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            ResponseHandleIndex = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            SaveFlags = ParseT<SaveFlags>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSaveChangesMessageRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(SaveFlags, "SaveFlags");
        }
    }
}
