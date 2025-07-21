using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.5.1 ActionBlock Structure
    /// </summary>
    public class ActionBlock : Block
    {
        /// <summary>
        /// An integer that specifies the cumulative length, in bytes, of the subsequent fields in this ActionBlock structure. For extended rules, the size of the ActionLength field is 4 bytes instead of 2 bytes.
        /// </summary>
        public BlockT<uint> ActionLength;

        /// <summary>
        /// An integer that specifies the type of action (2).
        /// </summary>
        public BlockT<ActionType> _actionType;

        /// <summary>
        /// The flags that are associated with a particular type of action (2).
        /// </summary>
        public Block ActionFlavor;

        /// <summary>
        /// Client-defined flags. The ActionFlags field is used solely by the client
        /// </summary>
        public BlockT<uint> ActionFlags;

        /// <summary>
        /// An ActionData structure, as specified in section 2.2.5.1.2, that specifies data related to the particular action (2).
        /// </summary>
        public Block ActionData;

        /// <summary>
        /// The wide size of NoOfActions.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the ActionBlock class.
        /// </summary>
        /// <param name="wide">The wide size of ActionLength.</param>
        public ActionBlock(CountWideEnum wide = CountWideEnum.twoBytes)
        {
            countWide = wide;
        }

        /// <summary>
        /// Parse the ActionBlock structure.
        /// </summary>
        protected override void Parse()
        {
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    ActionLength = ParseAs<ushort, uint>();
                    break;
                default:
                case CountWideEnum.fourBytes:
                    ActionLength = ParseT<uint>();
                    break;
            }

            _actionType = ParseT<ActionType>();
            switch (_actionType.Data)
            {
                case ActionType.OP_REPLY:
                    {
                        ActionFlavor = new ActionFlavor_Reply();
                        ActionFlavor.Parse(parser);
                        break;
                    }

                case ActionType.OP_OOF_REPLY:
                    {
                        ActionFlavor = new ActionFlavor_Reply();
                        ActionFlavor.Parse(parser);
                        break;
                    }

                case ActionType.OP_FORWARD:
                    {
                        ActionFlavor = new ActionFlavor_Forward();
                        ActionFlavor.Parse(parser);
                        break;
                    }

                default:
                    {
                        ActionFlavor = new ActionFlavor_Reserved();
                        ActionFlavor.Parse(parser);
                        break;
                    }
            }

            ActionFlags = ParseT<uint>();
            if (ActionLength > 9)
            {
                if ((ActionType.OP_MOVE == _actionType || ActionType.OP_COPY == _actionType) && countWide.Equals(CountWideEnum.twoBytes))
                {
                    ActionData = new OP_MOVE_and_OP_COPY_ActionData_forStandard();
                    ActionData.Parse(parser);
                }
                else if ((ActionType.OP_MOVE == _actionType || ActionType.OP_COPY == _actionType) && countWide.Equals(CountWideEnum.fourBytes))
                {
                    ActionData = new OP_MOVE_and_OP_COPY_ActionData_forExtended();
                    ActionData.Parse(parser);
                }
                else if ((ActionType.OP_REPLY == _actionType || ActionType.OP_OOF_REPLY == _actionType) && countWide.Equals(CountWideEnum.twoBytes))
                {
                    ActionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard();
                    ActionData.Parse(parser);
                }
                else if ((ActionType.OP_REPLY == _actionType || ActionType.OP_OOF_REPLY == _actionType) && countWide.Equals(CountWideEnum.fourBytes))
                {
                    ActionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended();
                    ActionData.Parse(parser);
                }
                else if (ActionType.OP_FORWARD == _actionType || ActionType.OP_DELEGATE == _actionType)
                {
                    ActionData = new OP_FORWARD_and_OP_DELEGATE_ActionData();
                    ActionData.Parse(parser);
                }
                else if (ActionType.OP_BOUNCE == _actionType)
                {
                    ActionData = new OP_BOUNCE_ActionData();
                    ActionData.Parse(parser);
                }
                else if (ActionType.OP_TAG == _actionType)
                {
                    ActionData = new TaggedPropertyValue(CountWideEnum.twoBytes);
                    ActionData.Parse(parser);
                }
                else if (ActionType.OP_DEFER_ACTION == _actionType)
                {
                    ActionData = new OP_DEFER_ACTION((int)ActionLength.Data);
                    ActionData.Parse(parser);
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = "ActionBlock";
            AddChildBlockT(ActionLength, "ActionLength");
            AddChildBlockT(_actionType, "ActionType");
            AddChild(ActionFlavor, "ActionFlavor");
            AddChildBlockT(ActionFlags, "ActionFlags");
            AddChild(ActionData, "ActionData");
        }
    }
}
