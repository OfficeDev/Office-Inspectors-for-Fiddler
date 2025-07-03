namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyProperties operation.
    /// 2.2.3.1.1.2.1 RopFastTransferSourceCopyProperties ROP Request Buffer
    /// </summary>
    public enum CopyFlags_CopyProperties : byte
    {
        /// <summary>
        /// This bit flag indicates whether the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x01,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused1 = 0x02,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused2 = 0x04,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused3 = 0x08,
    }
}
