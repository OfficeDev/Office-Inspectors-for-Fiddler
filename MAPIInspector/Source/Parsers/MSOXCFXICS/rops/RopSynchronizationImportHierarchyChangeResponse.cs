using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the RopSynchronizationImportHierarchyChange ROP Response Buffer.
    /// 2.2.3.2.4.3.2 RopSynchronizationImportHierarchyChange ROP Response Buffer
    /// </summary>
    public class RopSynchronizationImportHierarchyChangeResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<ErrorCodes> ReturnValue;

        /// <summary>
        /// An identifier.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();

            if (ReturnValue == ErrorCodes.Success)
            {
                FolderId = Parse<FolderID>();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "RopSynchronizationImportHierarchyChangeResponse";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            this.AddError(ReturnValue, "ReturnValue");
            AddChild(FolderId, "FolderId");
        }
    }
}
