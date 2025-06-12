namespace MAPIInspector.Parsers
{
    /// <summary>
    /// This enumeration is used to specify the type of data in a FastTransfer stream that is uploaded by using the RopFastTransferDestinationPutBuffer ROP.
    /// 2.2.3.1.2.1.1 RopFastTransferDestinationConfigure ROP Request Buffer
    /// </summary>
    public enum SourceOperation : byte
    {
        /// <summary>
        /// The value of the InputServerObject field can be any Message,folder or attachment object.
        /// </summary>
        CopyTo = 0x01,

        /// <summary>
        /// The value of the InputServerObject field can be any Message,folder or attachment object.
        /// </summary>
        CopyProperties = 0x02,

        /// <summary>
        /// The value of the InputServerObject field is a message object.
        /// </summary>
        CopyMessages = 0x03,

        /// <summary>
        /// The value of the InputServerObject field is a folder object.
        /// </summary>
        CopyFolder = 0x04,
    }
}
