namespace MAPIInspector.Parsers
{
    using System.IO;
    #region 2.2.1.2 RopCreateFolder ROP

    #endregion

    /// <summary>
    /// 2.2.1.3 RopDeleteFolder ROP
    /// The RopDeleteFolder ROP ([MS-OXCROPS] section 2.2.4.3) removes a folder. 
    /// </summary>
    public class RopDeleteFolderRequest : BaseStructure
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
        /// A flags structure that contains flags that control how to delete the folder. 
        /// </summary>
        public DeleteFolderFlags DeleteFolderFlags;

        /// <summary>
        /// An identifier that specifies the folder to be deleted.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopDeleteFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            DeleteFolderFlags = (DeleteFolderFlags)ReadByte();
            FolderId = new FolderID();
            FolderId.Parse(s);
        }
    }
}