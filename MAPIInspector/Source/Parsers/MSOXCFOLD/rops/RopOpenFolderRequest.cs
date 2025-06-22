namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.1.1 RopOpenFolder ROP Request Buffer
    /// The RopOpenFolder ROP ([MS-OXCROPS] section 2.2.4.1) opens an existing folder.
    /// 2.2.4.1.1 RopOpenFolder ROP Request Buffer
    /// </summary>
    public class RopOpenFolderRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A 64-bit identifier that specifies the folder to be opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// An 8-bit flags structure that contains flags that are used to control how the folder is opened.
        /// </summary>
        public OpenModeFlagsMSOXCFOLD OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            FolderId = new FolderID();
            FolderId.Parse(s);
            OpenModeFlags = (OpenModeFlagsMSOXCFOLD)ReadByte();
        }
    }
}