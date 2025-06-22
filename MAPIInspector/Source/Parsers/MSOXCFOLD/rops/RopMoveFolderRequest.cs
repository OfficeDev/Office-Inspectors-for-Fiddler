namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.1.7 RopMoveFolder ROP
    /// The RopMoveFolder ROP ([MS-OXCROPS] section 2.2.4.7) moves a folder from one parent folder to another parent folder.
    /// </summary>
    public class RopMoveFolderRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the source Server object is stored. 
        /// </summary>
        public byte SourceHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the destination Server object is stored. 
        /// </summary>
        public byte DestHandleIndex;

        /// <summary>
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the NewFolderName field contains Unicode characters.
        /// </summary>
        public bool UseUnicode;

        /// <summary>
        /// An identifier that specifies the folder to be moved.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated string that specifies the name for the new moved folder. 
        /// </summary>
        public MAPIString NewFolderName;

        /// <summary>
        /// Parse the RopMoveFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopMoveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            SourceHandleIndex = ReadByte();
            DestHandleIndex = ReadByte();
            WantAsynchronous = ReadBoolean();
            UseUnicode = ReadBoolean();
            FolderId = new FolderID();
            FolderId.Parse(s);
            if (UseUnicode)
            {
                NewFolderName = new MAPIString(Encoding.Unicode);
                NewFolderName.Parse(s);
            }
            else
            {
                NewFolderName = new MAPIString(Encoding.ASCII);
                NewFolderName.Parse(s);
            }
        }
    }
}