using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1.2.4 OP_FORWARD and OP_DELEGATE ActionData Structure
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.4 OP_FORWARD and OP_DELEGATE ActionData Structure
    /// </summary>
    public class OP_FORWARD_and_OP_DELEGATE_ActionData : Block
    {
        /// <summary>
        /// An integer that specifies the number of RecipientBlockData structures, as specified in section 2.2.5.1.2.4.1, contained in the RecipientBlocks field.
        /// </summary>
        public BlockT<ushort> RecipientCount;

        /// <summary>
        /// An array of RecipientBlockData structures, each of which specifies information about one recipient (2).
        /// </summary>
        public RecipientBlockData[] RecipientBlocks;

        /// <summary>
        /// Parse the OP_FORWARD_and_OP_DELEGATE_ActionData structure.
        /// </summary>
        protected override void Parse()
        {
            RecipientCount = ParseT<ushort>();
            var recipientBlocks = new List<RecipientBlockData>();
            for (int i = 0; i < RecipientCount; i++)
            {
                var recipientBlock = new RecipientBlockData();
                recipientBlock.Parse(parser);
                recipientBlocks.Add(recipientBlock);
            }

            RecipientBlocks = recipientBlocks.ToArray();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(RecipientCount, "RecipientCount");
            AddLabeledChildren(RecipientBlocks, "RecipientBlocks");
        }
    }
}
