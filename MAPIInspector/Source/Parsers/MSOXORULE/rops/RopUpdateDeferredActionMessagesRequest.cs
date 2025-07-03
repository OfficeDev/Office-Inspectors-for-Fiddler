using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3 RopUpdateDeferredActionMessages ROP
    /// The RopUpdateDeferredActionMessages ROP ([MS-OXCROPS] section 2.2.11.3) instructs the server to update the PidTagDamOriginalEntryId property (section 2.2.6.3) on one or more DAMs.
    /// </summary>
    public class RopUpdateDeferredActionMessagesRequest : Block
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
        /// An unsigned integer that specifies the size of the ServerEntryId field.
        /// </summary>
        public BlockT<ushort> ServerEntryIdSize;

        /// <summary>
        /// An array of bytes that specifies the ID of the message on the server. 
        /// </summary>
        public BlockBytes ServerEntryId;

        /// <summary>
        /// An unsigned integer that specifies the size of the ClientEntryId field.
        /// </summary>
        public BlockT<ushort> ClientEntryIdSize;

        /// <summary>
        /// An array of bytes that specifies the ID of the downloaded message on the client. 
        /// </summary>
        public BlockBytes ClientEntryId;

        /// <summary>
        /// Parse the RopUpdateDeferredActionMessagesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ServerEntryIdSize = ParseT<ushort>();
            ServerEntryId = ParseBytes(ServerEntryIdSize);
            ClientEntryIdSize = ParseT<ushort>();
            ClientEntryId = ParseBytes(ClientEntryIdSize);
        }

        protected override void ParseBlocks()
        {
            SetText("RopUpdateDeferredActionMessagesRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(ServerEntryIdSize, "ServerEntryIdSize");
            AddChildBytes(ServerEntryId, "ServerEntryId");
            AddChildBlockT(ClientEntryIdSize, "ClientEntryIdSize");
            AddChildBytes(ClientEntryId, "ClientEntryId");
        }
    }
}
