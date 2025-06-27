using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.2 RopGetReceiveFolder
    /// A class indicates the RopGetReceiveFolder ROP Request Buffer.
    /// </summary>
    public class RopGetReceiveFolderRequest : Block
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
        /// A null-terminated ASCII string that specifies the message class to find the Receive folder for.
        /// </summary>
        public BlockString MessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            MessageClass = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetReceiveFolderRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildString(MessageClass, "MessageClass");
        }
    }
}
