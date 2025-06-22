namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.1.9 RopEmptyFolder ROP
    /// The RopEmptyFolder ROP ([MS-OXCROPS] section 2.2.4.9) is used to soft delete messages and sub-folders from a folder without deleting the folder itself. 
    /// </summary>
    public class RopEmptyFolderRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A Boolean that specifies whether the operation also deletes folder associated information (FAI) messages.
        /// </summary>
        public bool WantDeleteAssociated;

        /// <summary>
        /// Parse the RopEmptyFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopEmptyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            WantAsynchronous = ReadBoolean();
            WantDeleteAssociated = ReadBoolean();
        }
    }
}