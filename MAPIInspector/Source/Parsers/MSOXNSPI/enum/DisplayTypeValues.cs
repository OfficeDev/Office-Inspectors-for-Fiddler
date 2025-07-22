namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The DisplayTypeValues enum type
    /// 2.2.3 Display Type Values
    /// </summary>
    public enum DisplayTypeValues : uint
    {
        /// <summary>
        /// A typical messaging user.
        /// </summary>
        DT_MAILUSER = 0x00000000,

        /// <summary>
        /// A distribution list.
        /// </summary>
        DT_DISTLIST = 0x00000001,

        /// <summary>
        /// A forum, such as a bulletin board service or a public or shared folder.
        /// </summary>
        DT_FORUM = 0x00000002,

        /// <summary>
        /// An automated agent, such as Quote-Of-The-Day or a weather chart display
        /// </summary>
        DT_AGENT = 0x00000003,

        /// <summary>
        /// An Address Book object defined for a large group
        /// </summary>
        DT_ORGANIZATION = 0x00000004,

        /// <summary>
        /// A private, personally administered distribution list.
        /// </summary>
        DT_PRIVATE_DISTLIST = 0x00000005,

        /// <summary>
        /// An Address Book object known to be from a foreign or remote messaging system
        /// </summary>
        DT_REMOTE_MAILUSER = 0x00000006,

        /// <summary>
        /// An address book hierarchy table container.
        /// </summary>
        DT_CONTAINER = 0x00000100,

        /// <summary>
        /// A display template object. An Exchange NSPI server MUST NOT return this display type.
        /// </summary>
        DT_TEMPLATE = 0x00000101,

        /// <summary>
        /// An address creation template.
        /// </summary>
        DT_ADDRESS_TEMPLATE = 0x00000102,

        /// <summary>
        /// A search template
        /// </summary>
        DT_SEARCH = 0x00000200
    }
}
