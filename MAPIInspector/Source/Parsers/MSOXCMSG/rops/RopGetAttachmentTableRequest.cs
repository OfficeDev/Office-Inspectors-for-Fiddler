namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Net.Mail;

    /// <summary>
    /// 2.2.6.17 RopGetAttachmentTable ROP
    /// A class indicates the RopGetAttachmentTable ROP request Buffer.
    /// </summary>
    public class RopGetAttachmentTableRequest : Block
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
        /// A flags structure that contains flags that control the type of table.
        /// </summary>
        public BlockT<GetAttachmentTableFlags> TableFlags;

        /// <summary>
        /// Parse the RopGetAttachmentTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            OutputHandleIndex = ParseT<byte>();
            TableFlags = ParseT<GetAttachmentTableFlags>();
        }

        protected override void ParseBlocks()
        {
            SetText("RopGetAttachmentTableRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(TableFlags, "TableFlags");
        }
    }
}
