namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.3.1.2 RopOpenMessage ROP Response Buffer
    /// An enumeration that specifies the type of RecipientType.
    /// </summary>
    public enum RecipientTypeType : byte
    {
        /// <summary>
        /// Primary recipient
        /// </summary>
        PrimaryRecipient = 0x01,

        /// <summary>
        /// Carbon copy recipient
        /// </summary>
        CcRecipient = 0x02,

        /// <summary>
        /// Blind carbon copy recipient
        /// </summary>
        BccRecipient = 0x03
    }
}
