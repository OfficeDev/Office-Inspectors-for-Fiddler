namespace MAPIInspector.Parsers
{
    using System.IO;
    using System.Text;

    /// <summary>
    ///  2.2.1.2 RopGetReceiveFolder
    ///  A class indicates the RopGetReceiveFolder ROP Response Buffer.
    /// </summary>
    public class RopGetReceiveFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier that specifies the Receive folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A null-terminated ASCII string that specifies the message class that is actually configured for delivery to the folder.
        /// </summary>
        public MAPIString ExplicitMessageClass;

        /// <summary>
        /// Parse the RopGetReceiveFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetReceiveFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RopId = (RopIdType)ReadByte();
            InputHandleIndex = ReadByte();
            ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                FolderId = new FolderID();
                FolderId.Parse(s);
                ExplicitMessageClass = new MAPIString(Encoding.ASCII);
                ExplicitMessageClass.Parse(s);
            }
        }
    }
}
