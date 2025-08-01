namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.8.3.1 RecipientFlags Field
    /// The enumeration specifies the type of address.
    /// </summary>
    public enum AddressTypeEnum : int
    {
        /// <summary>
        /// There is no type
        /// </summary>
        NoType = 0x0,

        /// <summary>
        /// X500DN type
        /// </summary>
        X500DN = 0x1,

        /// <summary>
        /// MsMail type
        /// </summary>
        MsMail = 0x2,

        /// <summary>
        /// SMTP type
        /// </summary>
        SMTP = 0x3,

        /// <summary>
        /// Fax type
        /// </summary>
        Fax = 0x4,

        /// <summary>
        /// ProfessionalOfficeSystem type
        /// </summary>
        ProfessionalOfficeSystem = 0x5,

        /// <summary>
        /// PersonalDistributionList1 type
        /// </summary>
        PersonalDistributionList1 = 0x6,

        /// <summary>
        /// PersonalDistributionList2 type
        /// </summary>
        PersonalDistributionList2 = 0x7
    }
}
