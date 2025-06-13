namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageMove ROP Request Buffer.
    ///  2.2.3.2.4.4.1 RopSynchronizationImportMessageMove ROP Request Buffer
    /// </summary>
    public class RopSynchronizationImportMessageMoveRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the SourceFolderId field.
        /// </summary>
        public uint SourceFolderIdSize;

        /// <summary>
        ///  An array of bytes that identifies the parent folder of the source message.
        /// </summary>
        public byte[] SourceFolderId;

        /// <summary>
        /// An unsigned integer that specifies the size of the SourceMessageId field.
        /// </summary>
        public uint SourceMessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the source message.
        /// </summary>
        public byte[] SourceMessageId;

        /// <summary>
        /// An unsigned integer that specifies the size of the PredecessorChangeList field.
        /// </summary>
        public uint PredecessorChangeListSize;

        /// <summary>
        /// An array of bytes. The size of this field, in bytes, is specified by the PredecessorChangeListSize field.
        /// </summary>
        public byte[] PredecessorChangeList;

        /// <summary>
        /// An unsigned integer that specifies the size of the DestinationMessageId field.
        /// </summary>
        public uint DestinationMessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the destination message. 
        /// </summary>
        public byte[] DestinationMessageId;

        /// <summary>
        /// An unsigned integer that specifies the size of the ChangeNumber field.
        /// </summary>
        public uint ChangeNumberSize;

        /// <summary>
        /// An array of bytes that specifies the change number of the message. 
        /// </summary>
        public byte[] ChangeNumber;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportMessageMoveRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.SourceFolderIdSize = this.ReadUint();
            this.SourceFolderId = this.ReadBytes((int)this.SourceFolderIdSize);
            this.SourceMessageIdSize = this.ReadUint();
            this.SourceMessageId = this.ReadBytes((int)this.SourceMessageIdSize);
            this.PredecessorChangeListSize = this.ReadUint();
            this.PredecessorChangeList = this.ReadBytes((int)this.PredecessorChangeListSize);
            this.DestinationMessageIdSize = this.ReadUint();
            this.DestinationMessageId = this.ReadBytes((int)this.DestinationMessageIdSize);
            this.ChangeNumberSize = this.ReadUint();
            this.ChangeNumber = this.ReadBytes((int)this.ChangeNumberSize);
        }
    }
}
