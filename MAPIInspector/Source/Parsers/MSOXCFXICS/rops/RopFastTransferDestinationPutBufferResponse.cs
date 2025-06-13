namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBuffer ROP Response Buffer.
    ///  2.2.3.1.2.2.2 RopFastTransferDestinationPutBuffer ROP Response Buffer
    /// </summary>
    public class RopFastTransferDestinationPutBufferResponse : BaseStructure
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
        /// The current status of the transfer.
        /// </summary>
        public ushort TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public ushort InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate total number of steps to be completed in the current operation.
        /// </summary>
        public ushort TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An unsigned integer that specifies the buffer size that was used.
        /// </summary>
        public ushort BufferUsedSize;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationPutBufferResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.TransferStatus = this.ReadUshort();
            this.InProgressCount = this.ReadUshort();
            this.TotalStepCount = this.ReadUshort();
            this.Reserved = this.ReadByte();
            this.BufferUsedSize = this.ReadUshort();
        }
    }
}
