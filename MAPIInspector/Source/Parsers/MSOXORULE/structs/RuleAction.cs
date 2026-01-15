using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5 RuleAction Structure
    /// </summary>
    public class RuleAction : Block
    {
        /// <summary>
        /// Specifies the number of structures that are contained in the ActionBlocks field. For extended rules, the size of the NoOfActions field is 4 bytes instead of 2 bytes.
        /// </summary>
        public BlockT<uint> NoOfActions;

        /// <summary>
        /// An array of ActionBlock structures, each of which specifies an action (2) of the rule (2), as specified in section 2.2.5.1.
        /// </summary>
        public ActionBlock[] ActionBlocks;

        /// <summary>
        /// The parsing context that determines count field widths.
        /// </summary>
        private readonly PropertyCountContext context;

        /// <summary>
        /// Initializes a new instance of the RuleAction class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths.</param>
        public RuleAction(PropertyCountContext countContext = PropertyCountContext.RopBuffers)
        {
            context = countContext;
        }

        /// <summary>
        /// Parse the RuleAction structure.
        /// </summary>
        protected override void Parse()
        {
            switch (context)
            {
                case PropertyCountContext.RopBuffers:
                    NoOfActions = ParseAs<ushort, uint>();
                    break;
                default:
                case PropertyCountContext.ExtendedRules:
                case PropertyCountContext.MapiHttp:
                case PropertyCountContext.AddressBook:
                    NoOfActions = ParseT<uint>();
                    break;
            }
            var tempActionBlocks = new List<ActionBlock>();
            for (int i = 0; i < NoOfActions; i++)
            {
                var tempActionBlock = new ActionBlock(context);
                tempActionBlock.Parse(parser);
                tempActionBlocks.Add(tempActionBlock);
            }

            ActionBlocks = tempActionBlocks.ToArray();
        }

        protected override void ParseBlocks()
        {
            Text = "RuleAction";
            AddChildBlockT(NoOfActions, "NoOfActions");
            AddLabeledChildren(ActionBlocks, "ActionBlocks");
        }
    }
}
