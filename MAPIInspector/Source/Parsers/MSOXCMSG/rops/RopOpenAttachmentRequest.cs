using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.6.12 RopOpenAttachment ROP
    /// A class indicates the RopOpenAttachment ROP request Buffer.
    /// </summary>
    public class RopOpenAttachmentRequest : Block
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        /// </summary>
        public BlockT<byte> OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags for opening attachments.
        /// </summary>
        public BlockT<OpenAttachmentFlags> OpenAttachmentFlags;

        /// <summary>
        /// An unsigned integer index that identifies the attachment to be opened.
        /// </summary>
        public BlockT<uint> AttachmentID;

        /// <summary>
        /// Parse the RopOpenAttachmentRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            OpenAttachmentFlags = ParseT<OpenAttachmentFlags>();
            AttachmentID = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            Text = "RopOpenAttachmentRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(OutputHandleIndex, "OutputHandleIndex");
            AddChildBlockT(OpenAttachmentFlags, "OpenAttachmentFlags");
            AddChildBlockT(AttachmentID, "AttachmentID");
        }
    }
}
