namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.6 RopReadRecipients ROP
    /// A class indicates the RopReadRecipients ROP request Buffer.
    /// </summary>
    public class RopReadRecipientsRequest : BaseStructure
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
        /// An unsigned integer that specifies the starting index for the recipients (2) to be retrieved.
        /// </summary>
        public uint RowId;

        /// <summary>
        /// Reserved. This field MUST be set to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Parse the RopReadRecipientsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            RowId = ReadUint();
            Reserved = ReadUshort();
        }
    }
}
