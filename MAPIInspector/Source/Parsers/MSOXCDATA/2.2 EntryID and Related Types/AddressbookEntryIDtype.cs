namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.2 Address Book EntryID Structure
    /// The enum of AddressbookEntryID type.
    /// </summary>
    public enum AddressbookEntryIDtype : uint
    {
        /// <summary>
        /// Local mail user
        /// </summary>
        Localmailuser = 0x00000000,

        /// <summary>
        /// Distribution list
        /// </summary>
        Distributionlist = 0x00000001,

        /// <summary>
        /// Bulletin board or public folder
        /// </summary>
        Bulletinboardorpublicfolder = 0x00000002,

        /// <summary>
        /// Automated mailbox
        /// </summary>
        Automatedmailbox = 0x00000003,

        /// <summary>
        /// Organizational mailbox
        /// </summary>
        Organizationalmailbox = 0x00000004,

        /// <summary>
        /// Private distribution list
        /// </summary>
        Privatedistributionlist = 0x00000005,

        /// <summary>
        /// Remote mail user
        /// </summary>
        Remotemailuser = 0x00000006,

        /// <summary>
        /// A Container
        /// </summary>
        Container = 0x00000100,

        /// <summary>
        /// A Template
        /// </summary>
        Template = 0x00000101,

        /// <summary>
        /// One-off user
        /// </summary>
        Oneoffuser = 0x00000102,

        /// <summary>
        /// A Search
        /// </summary>
        Search = 0x00000200
    }
}
