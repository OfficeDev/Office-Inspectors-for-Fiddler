namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.15 RopSaveChangesAttachment ROP
    /// A class indicates the RopSaveChangesAttachment ROP response Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSaveChangesAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }
}
