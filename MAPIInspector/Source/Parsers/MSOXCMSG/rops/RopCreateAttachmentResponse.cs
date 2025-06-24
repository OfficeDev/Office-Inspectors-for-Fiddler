namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.13 RopCreateAttachment ROP
    /// A class indicates the RopCreateAttachment ROP response Buffer.
    /// </summary>
    public class RopCreateAttachmentResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer identifier that refers to the attachment created.
        /// </summary>
        public uint? AttachmentID;

        /// <summary>
        /// Parse the RopCreateAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.AttachmentID = this.ReadUint();
            }
        }
    }
}
