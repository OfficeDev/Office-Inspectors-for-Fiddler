namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// 2.2.1.2 RopCreateFolder ROP
    /// The RopCreateFolder ROP ([MS-OXCROPS] section 2.2.4.2) creates a new folder
    /// </summary>
    public class RopCreateFolderRequest : BaseStructure
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
        /// An enumeration that specifies what type of folder to create. 
        /// </summary>
        public FolderType FolderType;

        /// <summary>
        /// A Boolean that specifies whether DisplayName and Comment fields are formated in Unicode.
        /// </summary>
        public bool UseUnicodeStrings;

        /// <summary>
        /// Boolean that specifies whether this operation opens a Folder object or fails when the Folder object already exists.
        /// </summary>
        public bool OpenExisting;

        /// <summary>
        /// Reserved. This field MUST be set to 0x00.
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// A null-terminated string that specifies the name of the created folder. 
        /// </summary>
        public MAPIString DisplayName;

        /// <summary>
        /// A null-terminated folder string that specifies the folder comment that is associated with the created folder. 
        /// </summary>
        public MAPIString Comment;

        /// <summary>
        /// Parse the RopCreateFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            OutputHandleIndex = ReadByte();
            FolderType = (FolderType)ReadByte();
            UseUnicodeStrings = ReadBoolean();
            OpenExisting = ReadBoolean();
            Reserved = ReadByte();
            if (UseUnicodeStrings)
            {
                DisplayName = new MAPIString(Encoding.Unicode);
                DisplayName.Parse(s);
                Comment = new MAPIString(Encoding.Unicode);
                Comment.Parse(s);
            }
            else
            {
                DisplayName = new MAPIString(Encoding.ASCII);
                DisplayName.Parse(s);
                Comment = new MAPIString(Encoding.ASCII);
                Comment.Parse(s);
            }
        }
    }
}