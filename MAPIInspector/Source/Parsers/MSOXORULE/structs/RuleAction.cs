using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5 RuleAction Structure
    /// </summary>
    public class RuleAction : Block
    {
        /// <summary>
        /// Specifies the number of structures that are contained in the ActionBlocks field. For extended rules, the size of the NoOfActions field is 4 bytes instead of 2 bytes.
        /// </summary>
        private Block _noOfActions;
        public uint NoOfActions;

        /// <summary>
        /// An array of ActionBlock structures, each of which specifies an action (2) of the rule (2), as specified in section 2.2.5.1.
        /// </summary>
        public ActionBlock[] ActionBlocks;

        /// <summary>
        /// The wide size of NoOfActions.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the RuleAction class
        /// </summary>
        /// <param name="wide">The wide size of NoOfActions.</param>
        public RuleAction(CountWideEnum wide = CountWideEnum.twoBytes)
        {
            countWide = wide;
        }

        /// <summary>
        /// Parse the RuleAction structure.
        /// </summary>
        protected override void Parse()
        {
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    _noOfActions = ParseT<ushort>();
                    NoOfActions = (_noOfActions as BlockT<ushort>);
                    break;
                default:
                case CountWideEnum.fourBytes:
                    _noOfActions = ParseT<uint>();
                    NoOfActions = (_noOfActions as BlockT<uint>);
                    break;
            }
            var tempActionBlocks = new List<ActionBlock>();
            for (int i = 0; i < NoOfActions; i++)
            {
                var tempActionBlock = new ActionBlock(CountWideEnum.twoBytes);
                tempActionBlock.Parse(parser);
                tempActionBlocks.Add(tempActionBlock);
            }

            ActionBlocks = tempActionBlocks.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RuleAction");
            AddChild(_noOfActions, $"NoOfActions:{NoOfActions}");
            AddLabeledChildren(ActionBlocks, "ActionBlocks");
        }
    }
}
