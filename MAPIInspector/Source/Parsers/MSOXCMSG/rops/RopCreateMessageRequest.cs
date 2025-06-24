namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.2 RopCreateMessage
    /// A class indicates the RopCreateMessage ROP request Buffer.
    /// </summary>
    public class RopCreateMessageRequest : BaseStructure
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
        /// An identifier that specifies the code page for the message.
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// An identifier that specifies the parent folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A Boolean that specifies whether the message is an FAI message.
        /// </summary>
        public bool AssociatedFlag;

        /// <summary>
        /// Parse the RopCreateMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            CodePageId = ReadUshort();
            FolderId = new FolderID();
            FolderId.Parse(s);
            AssociatedFlag = ReadBoolean();
        }
    }
}
