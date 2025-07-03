namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyMessages operation.
    /// 2.2.3.1.1.3.1 RopFastTransferSourceCopyMessages ROP Request Buffer
    /// </summary>
    public enum CopyFlags_CopyMessages : byte
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

        /// <summary>
        /// Identify whether the output message body is in the original format or compressed RTF format.
        /// </summary>
        BestBody = 0x10,

        /// <summary>
        /// This bit flag indicates whether message change information is included in the FastTransfer stream
        /// </summary>
        SendEntryId = 0x20,
    }
}
