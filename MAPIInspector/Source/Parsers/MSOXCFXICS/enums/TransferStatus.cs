namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies the current status of the transfer.
    /// 2.2.3.1.1.5.2 RopFastTransferSourceGetBuffer ROP Response Buffer
    /// </summary>
    public enum TransferStatus : ushort
    {
        /// <summary>
        /// The download stopped because a nonrecoverable error has occurred when producing a FastTransfer stream.
        /// </summary>
        Error = 0x0000,

        /// <summary>
        /// The FastTransfer stream was split, and more data is available.
        /// </summary>
        Partial = 0x0001,

        /// <summary>
        /// The FastTransfer stream was split, more data is available, and the value of the TransferBuffer field contains incomplete data
        /// </summary>
        NoRoom = 0x0002,

        /// <summary>
        /// This was the last portion of the FastTransfer stream.
        /// </summary>
        Done = 0x0003,
    }
}
