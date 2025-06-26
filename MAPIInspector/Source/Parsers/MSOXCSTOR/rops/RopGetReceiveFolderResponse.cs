using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  2.2.1.2 RopGetReceiveFolder
    ///  A class indicates the RopGetReceiveFolder ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderResponse : Block
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
        /// An identifier that specifies the Receive folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class that is actually configured for delivery to the folder.
        /// </summary>
        public BlockString ExplicitMessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            InputHandleIndex = ParseT<byte>();
            ReturnValue = ParseT<ErrorCodes>();
            if (ReturnValue.Data == ErrorCodes.Success)
            {
                FolderId = Parse<FolderID>();
                ExplicitMessageClass = ParseStringA();
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetReceiveFolderResponse");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            if (ReturnValue != null) AddChild(ReturnValue, $"ReturnValue:{ReturnValue.Data.FormatErrorCode()}");
            AddChild(FolderId, "FolderId");
            AddChildString(ExplicitMessageClass, "ExplicitMessageClass");
        }
    }
}
