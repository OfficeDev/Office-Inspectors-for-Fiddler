namespace MAPIInspector.Parsers
{
    using System;

    /// <summary>
    /// The enum value of Modify Flags
    /// </summary>
    [Flags]
    public enum ModifyFlags : byte
    {
        /// <summary>
        /// If this flag is set, the server MUST replace all existing entries except the default user entry in the current permissions list with the ones contained in the PermissionsData field
        /// </summary>
        ReplaceRows = 0x01,

        /// <summary>
        /// If this flag is set, the server MUST apply the settings of the FreeBusySimple and FreeBusyDetailed flags of the PidTagMemberRights property when modifying the permissions of the Calendar folder
        /// </summary>
        IncludeFreeBusy = 0x02
    }
}
