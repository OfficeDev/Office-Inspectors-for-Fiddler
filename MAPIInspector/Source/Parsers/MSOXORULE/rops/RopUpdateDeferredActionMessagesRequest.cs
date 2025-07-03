namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3 RopUpdateDeferredActionMessages ROP
    /// The RopUpdateDeferredActionMessages ROP ([MS-OXCROPS] section 2.2.11.3) instructs the server to update the PidTagDamOriginalEntryId property (section 2.2.6.3) on one or more DAMs.
    /// </summary>
    public class RopUpdateDeferredActionMessagesRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the ServerEntryId field.
        /// </summary>
        public ushort ServerEntryIdSize;

        /// <summary>
        /// An array of bytes that specifies the ID of the message on the server. 
        /// </summary>
        public byte[] ServerEntryId;

        /// <summary>
        /// An unsigned integer that specifies the size of the ClientEntryId field.
        /// </summary>
        public ushort ClientEntryIdSize;

        /// <summary>
        /// An array of bytes that specifies the ID of the downloaded message on the client. 
        /// </summary>
        public byte[] ClientEntryId;

        /// <summary>
        /// Parse the RopUpdateDeferredActionMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopUpdateDeferredActionMessagesRequest structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            ServerEntryIdSize = ReadUshort();
            ServerEntryId = ReadBytes((int)ServerEntryIdSize);
            ClientEntryIdSize = ReadUshort();
            ClientEntryId = ReadBytes((int)ClientEntryIdSize);
        }
    }
}
