using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageMove ROP Request Buffer.
    ///  2.2.3.2.4.4.1 RopSynchronizationImportMessageMove ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportMessageMoveRequest : Block
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
        /// An unsigned integer that specifies the size of the SourceFolderId field.
        /// </summary>
        public BlockT<uint> SourceFolderIdSize;

        /// <summary>
        ///  An array of bytes that identifies the parent folder of the source message.
        /// </summary>
        public BlockBytes SourceFolderId;

        /// <summary>
        /// An unsigned integer that specifies the size of the SourceMessageId field.
        /// </summary>
        public BlockT<uint> SourceMessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the source message.
        /// </summary>
        public BlockBytes SourceMessageId;

        /// <summary>
        /// An unsigned integer that specifies the size of the PredecessorChangeList field.
        /// </summary>
        public BlockT<uint> PredecessorChangeListSize;

        /// <summary>
        /// An array of bytes. The size of field, in bytes, is specified by the PredecessorChangeListSize field.
        /// </summary>
        public BlockBytes PredecessorChangeList;

        /// <summary>
        /// An unsigned integer that specifies the size of the DestinationMessageId field.
        /// </summary>
        public BlockT<uint> DestinationMessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the destination message. 
        /// </summary>
        public BlockBytes DestinationMessageId;

        /// <summary>
        /// An unsigned integer that specifies the size of the ChangeNumber field.
        /// </summary>
        public BlockT<uint> ChangeNumberSize;

        /// <summary>
        /// An array of bytes that specifies the change number of the message. 
        /// </summary>
        public BlockBytes ChangeNumber;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            SourceFolderIdSize = ParseT<uint>();
            SourceFolderId = ParseBytes((int)SourceFolderIdSize.Data);
            SourceMessageIdSize = ParseT<uint>();
            SourceMessageId = ParseBytes((int)SourceMessageIdSize.Data);
            PredecessorChangeListSize = ParseT<uint>();
            PredecessorChangeList = ParseBytes((int)PredecessorChangeListSize.Data);
            DestinationMessageIdSize = ParseT<uint>();
            DestinationMessageId = ParseBytes((int)DestinationMessageIdSize.Data);
            ChangeNumberSize = ParseT<uint>();
            ChangeNumber = ParseBytes((int)ChangeNumberSize.Data);
        }

        protected override void ParseBlocks()
        {
            SetText("RopSynchronizationImportMessageMoveRequest");
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChildBlockT(SourceFolderIdSize, "SourceFolderIdSize");
            if (SourceFolderId != null) AddChild(SourceFolderId, $"SourceFolderId: {SourceFolderId.ToHexString(false)}");
            AddChildBlockT(SourceMessageIdSize, "SourceMessageIdSize");
            if (SourceMessageId != null) AddChild(SourceMessageId, $"SourceMessageId: {SourceMessageId.ToHexString(false)}");
            AddChildBlockT(PredecessorChangeListSize, "PredecessorChangeListSize");
            if (PredecessorChangeList != null) AddChild(PredecessorChangeList, $"PredecessorChangeList: {PredecessorChangeList.ToHexString(false)}");
            AddChildBlockT(DestinationMessageIdSize, "DestinationMessageIdSize");
            if (DestinationMessageId != null) AddChild(DestinationMessageId, $"DestinationMessageId: {DestinationMessageId.ToHexString(false)}");
            AddChildBlockT(ChangeNumberSize, "ChangeNumberSize");
            if (ChangeNumber != null) AddChild(ChangeNumber, $"ChangeNumber: {ChangeNumber.ToHexString(false)}");
        }
    }
}
