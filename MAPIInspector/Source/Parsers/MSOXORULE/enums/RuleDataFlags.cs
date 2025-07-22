using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.1.3 RuleData
    /// The enum value of RuleDataFlags
    /// </summary>
    [Flags]
    public enum RuleDataFlags : byte
    {
        /// <summary>
        /// Adds the data in the rule buffer to the rule set as a new rule
        /// </summary>
        ROW_ADD = 0x01,

        /// <summary>
        /// Modifies the existing rule identified by the value of the PidTagRuleId property
        /// </summary>
        ROW_MODIFY = 0x02,

        /// <summary>
        /// Removes from the rule set the rule that has the same value of the PidTagRuleId property
        /// </summary>
        ROW_REMOVE = 0x04
    }
}
