namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.2.6.8 RopSetMessageStatus ROP
    /// A class indicates the RopSetMessageStatus ROP response Buffer.
    /// </summary>
    public class RopSetMessageStatusResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A flags structure that contains the status flags that were set on the message before this operation.
        /// </summary>
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// Parse the RopSetMessageStatusResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageStatusResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.MessageStatusFlags = (MessageStatusFlag)this.ReadUint();
            }
        }
    }
}
