namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCMSG] 2.2.3.17.1 RopGetAttachmentTable ROP Request Buffer
    /// The enum value of GetAttachmentTableFlags that contains flags that control the type of table..
    /// </summary>
    public enum GetAttachmentTableFlags : byte
    {
        /// <summary>
        /// Open the table.
        /// </summary>
        Standard = 0x00,

        /// <summary>
        /// Open the table. Also requests that the columns containing string data be returned in Unicode format.
        /// </summary>
        Unicode = 0x40
    }
}
