namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.7 RopPublicFolderIsGhosted
    ///  A class indicates the RopPublicFolderIsGhosted ROP Request Buffer.
    /// </summary>
    public class RopPublicFolderIsGhostedRequest : BaseStructure
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
        /// An identifier that specifies the folder to check.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopPublicFolderIsGhostedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopPublicFolderIsGhostedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            FolderId = new FolderID();
            FolderId.Parse(s);
        }
    }
}
