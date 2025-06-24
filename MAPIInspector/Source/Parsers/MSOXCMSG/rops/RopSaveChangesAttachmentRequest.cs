namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.15 RopSaveChangesAttachment ROP
    /// A class indicates the RopSaveChangesAttachment ROP request Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response. 
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        /// </summary>
        public SaveFlags SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesAttachmentRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.SaveFlags = (SaveFlags)this.ReadByte();
        }
    }
}
