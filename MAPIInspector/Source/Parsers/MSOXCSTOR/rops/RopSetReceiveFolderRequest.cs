namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.3 RopSetReceiveFolder
    ///  A class indicates the RopSetReceiveFolder ROP Request Buffer.
    /// </summary>
    public class RopSetReceiveFolderRequest : BaseStructure
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
        /// An identifier that specifies the Receive folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies which message class to set the Receive folder for.
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// Parse the RopSetReceiveFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetReceiveFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            FolderId = new FolderID();
            FolderId.Parse(s);
            MessageClass = new MAPIString(Encoding.ASCII);
            MessageClass.Parse(s);
        }
    }
}
