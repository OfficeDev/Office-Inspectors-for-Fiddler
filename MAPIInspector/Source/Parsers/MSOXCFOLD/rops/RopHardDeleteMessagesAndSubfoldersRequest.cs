using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.10 RopHardDeleteMessagesAndSubfolders ROP
    /// The RopHardDeleteMessagesAndSubfolders ROP ([MS-OXCROPS] section 2.2.4.10) is used to hard delete all messages and sub-folders from a folder without deleting the folder itself.
    /// </summary>
    public class RopHardDeleteMessagesAndSubfoldersRequest : Block
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
        ///  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public BlockT<bool> WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation also deletes folder associated information (FAI) messages.
        /// </summary>
        public BlockT<bool> WantDeleteAssociated;

        /// <summary>
        /// Parse the RopHardDeleteMessagesAndSubfoldersRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            WantAsynchronous = ParseAs<byte, bool>();
            WantDeleteAssociated = ParseAs<byte, bool>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopHardDeleteMessagesAndSubfoldersRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(WantAsynchronous, "WantAsynchronous");
            AddChildBlockT(WantDeleteAssociated, "WantDeleteAssociated");
        }
    }
}