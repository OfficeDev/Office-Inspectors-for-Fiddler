namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.3.1 RopOpenMessage
    ///  A class indicates the RopOpenMessage ROP Request Buffer.
    /// </summary>
    public class RopOpenMessageRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An identifier that specifies which code page will be used for string values associated with the message.
        /// </summary>
        public short CodePageId;

        /// <summary>
        /// An identifier that identifies the parent folder of the message to be opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A flags structure that contains flags that control the access to the message. 
        /// </summary>
        public OpenMessageModeFlags OpenModeFlags;

        /// <summary>
        /// An identifier that identifies the message to be opened.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopOpenMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CodePageId = this.ReadINT16();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.OpenModeFlags = (OpenMessageModeFlags)this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }
    }
}
