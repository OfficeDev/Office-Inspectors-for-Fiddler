using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCROPS] 2.2.6.14 RopDeleteAttachment ROP
    /// A class indicates the RopDeleteAttachment ROP request Buffer.
    /// </summary>
    public class RopDeleteAttachmentRequest : Block
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
        /// An unsigned integer that identifies the attachment to be deleted.
        /// </summary>
        public BlockT<uint> AttachmentID;

        /// <summary>
        /// Parse the RopDeleteAttachmentRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            AttachmentID = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopDeleteAttachmentRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(AttachmentID, "AttachmentID");
        }
    }
}
