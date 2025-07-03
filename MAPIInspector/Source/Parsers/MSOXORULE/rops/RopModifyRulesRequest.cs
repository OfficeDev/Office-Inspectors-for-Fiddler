using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1 RopModifyRules ROP
    /// The RopModifyRules ROP ([MS-OXCROPS] section 2.2.11.1) creates, modifies, or deletes rules (2) in a folder.
    /// </summary>
    public class RopModifyRulesRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A bitmask that specifies how the rules (2) included in this structure are created on the server.
        /// </summary>
        public ModifyRulesFlags ModifyRulesFlags;

        /// <summary>
        /// An integer that specifies the number of RuleData structures present in the RulesData field.
        /// </summary>
        public ushort RulesCount;

        /// <summary>
        /// An array of RuleData structures, each of which specifies details about a standard rule. 
        /// </summary>
        public RuleData[] RulesData;

        /// <summary>
        /// Parse the RopModifyRulesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing the RopModifyRulesRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopId = (RopIdType)ReadByte();
            LogonId = ReadByte();
            InputHandleIndex = ReadByte();
            ModifyRulesFlags = new ModifyRulesFlags();
            ModifyRulesFlags.Parse(s);
            RulesCount = ReadUshort();
            List<RuleData> tempRulesDatas = new List<RuleData>();
            for (int i = 0; i < RulesCount; i++)
            {
                RuleData tempRuleData = new RuleData();
                tempRuleData.Parse(s);
                tempRulesDatas.Add(tempRuleData);
            }

            RulesData = tempRulesDatas.ToArray();
        }
    }
}
