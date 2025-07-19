using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1 RopModifyRules ROP
    /// The RopModifyRules ROP ([MS-OXCROPS] section 2.2.11.1) creates, modifies, or deletes rules (2) in a folder.
    /// </summary>
    public class RopModifyRulesRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this operation.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A bitmask that specifies how the rules (2) included in this structure are created on the server.
        /// </summary>
        public ModifyRulesFlags ModifyRulesFlags;

        /// <summary>
        /// An integer that specifies the number of RuleData structures present in the RulesData field.
        /// </summary>
        public BlockT<ushort> RulesCount;

        /// <summary>
        /// An array of RuleData structures, each of which specifies details about a standard rule.
        /// </summary>
        public RuleData[] RulesData;

        /// <summary>
        /// Parse the RopModifyRulesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>();
            LogonId = ParseT<byte>();
            InputHandleIndex = ParseT<byte>();
            ModifyRulesFlags = Parse<ModifyRulesFlags>();
            RulesCount = ParseT<ushort>();
            var tempRulesDatas = new List<RuleData>();
            for (int i = 0; i < RulesCount; i++)
            {
                tempRulesDatas.Add(Parse<RuleData>());
            }

            RulesData = tempRulesDatas.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RopModifyRulesRequest";
            AddChildBlockT(RopId, "RopId");
            AddChildBlockT(LogonId, "LogonId");
            AddChildBlockT(InputHandleIndex, "InputHandleIndex");
            AddChild(ModifyRulesFlags, "ModifyRulesFlags");
            AddChildBlockT(RulesCount, "RulesCount");
            AddLabeledChildren(RulesData, "RulesData");
        }
    }
}
