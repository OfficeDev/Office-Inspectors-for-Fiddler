namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.8.3.2 RecipientRow Structure
    /// The enum value of DisplayType.
    /// </summary>
    public enum DisplayType : byte
    {
        /// <summary>
        /// A messaging user
        /// </summary>
        MessagingUser = 0x00,

        /// <summary>
        /// A distribution list
        /// </summary>
        DistributionList = 0x01,

        /// <summary>
        /// A forum, such as a bulletin board service or a public or shared folder
        /// </summary>
        Forum = 0x02,

        /// <summary>
        /// An automated agent
        /// </summary>
        AutomatedAgent = 0x03,

        /// <summary>
        /// An Address Book object defined for a large group, such as helpdesk, accounting, coordinator, or department
        /// </summary>
        AddressBookforLargeGroup = 0x04,

        /// <summary>
        /// A private, personally administered distribution list
        /// </summary>
        Private = 0x05,

        /// <summary>
        /// An Address Book object known to be from a foreign or remote messaging system
        /// </summary>
        AddressBookfromMessagingSystem = 0x06
    }
}
