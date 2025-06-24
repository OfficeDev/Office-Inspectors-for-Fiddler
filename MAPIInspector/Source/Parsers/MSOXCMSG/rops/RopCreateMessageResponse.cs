namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.2 RopCreateMessage
    /// A class indicates the RopCreateMessage ROP response Buffer.
    /// </summary>
    public class RopCreateMessageResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex specified in field the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the MessageId field is present.
        /// </summary>
        public bool? HasMessageId;

        /// <summary>
        /// An identifier that is present if HasMessageId is nonzero and is not present if it is zero.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopCreateMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.HasMessageId = this.ReadBoolean();
                if ((bool)this.HasMessageId)
                {
                    this.MessageId = new MessageID();
                    this.MessageId.Parse(s);
                }
            }
        }
    }
}
