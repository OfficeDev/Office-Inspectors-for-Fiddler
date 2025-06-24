namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.12 RopOpenAttachment ROP
    /// A class indicates the RopOpenAttachment ROP response Buffer.
    /// </summary>
    public class RopOpenAttachmentResponse : BaseStructure
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
        /// Parse the RopOpenAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
        }
    }
}
