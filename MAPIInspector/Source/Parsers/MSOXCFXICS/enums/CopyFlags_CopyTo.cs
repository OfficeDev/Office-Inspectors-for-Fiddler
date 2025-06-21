namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyTo operation. 
    /// 2.2.3.1.1.1.1 RopFastTransferSourceCopyTo ROP Request Buffer
    /// </summary>
    public enum CopyFlags_CopyTo : uint
    {
        /// <summary>
        /// This bit flag indicates whether the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x00000001,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused1 = 0x00000002,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused2 = 0x00000004,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused3 = 0x00000008,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused4 = 0x00000200,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused5 = 0x00000400,

        /// <summary>
        /// This flag MUST only be passed if the value of the InputServerObject field is a Message object.
        /// </summary>
        BestBody = 0x0002000,
    }
}
