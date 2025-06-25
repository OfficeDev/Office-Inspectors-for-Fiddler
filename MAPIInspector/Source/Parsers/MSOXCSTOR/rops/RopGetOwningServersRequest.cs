namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  2.2.1.6 RopGetOwningServers
    ///  A class indicates the RopGetOwningServers ROP Request Buffer.
    /// </summary>
    public class RopGetOwningServersRequest : BaseStructure
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
        /// An identifier that specifies the folder for which to get owning servers.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopGetOwningServersRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetOwningServersRequest structure.</param>
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
