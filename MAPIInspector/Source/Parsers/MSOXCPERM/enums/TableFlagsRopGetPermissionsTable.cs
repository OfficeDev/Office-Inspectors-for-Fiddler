using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The enum value of RopGetPermissionsTable TableFlags
    /// </summary>
    [Flags]
    public enum TableFlagsRopGetPermissionsTable : byte
    {
        /// <summary>
        /// If this flag is set, the server MUST include the values of the FreeBusySimple and FreeBusyDetailed flags of the PidTagMemberRights property in the returned permissions list
        /// </summary>
        IncludeFreeBusy = 0x02
    }
}
