namespace MAPIInspector.Parsers
{
    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyFolder operation. 
    /// </summary>
    public enum CopyFlags_CopyFolder : byte
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
        /// The subfolders of the folder specified in the InputServerObject field are recursively included in the scope
        /// </summary>
        CopySubfolders = 0x10,
    }
}
