namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.14 RopDeleteAttachment ROP
    /// A class indicates the RopDeleteAttachment ROP request Buffer.
    /// </summary>
    public class RopDeleteAttachmentRequest : BaseStructure
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
        /// An unsigned integer that identifies the attachment to be deleted. 
        /// </summary>
        public uint AttachmentID;

        /// <summary>
        /// Parse the RopDeleteAttachmentRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            AttachmentID = ReadUint();
        }
    }
}
