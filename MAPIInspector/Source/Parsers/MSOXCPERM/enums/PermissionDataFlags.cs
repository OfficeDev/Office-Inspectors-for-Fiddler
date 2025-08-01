using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of Permission Data Flags
    /// </summary>
    [Flags]
    public enum PermissionDataFlags : byte
    {
        /// <summary>
        /// The user that is specified by the PidTagEntryId property (section 2.2.4) is added to the permissions list
        /// </summary>
        AddRow = 0x01,

        /// <summary>
        /// The existing permissions for the user that is identified by the PidTagMemberId property are modified
        /// </summary>
        ModifyRow = 0x02,

        /// <summary>
        /// The user that is identified by the PidTagMemberId property is deleted from the permissions list
        /// </summary>
        RemoveRow = 0x04
    }
}
