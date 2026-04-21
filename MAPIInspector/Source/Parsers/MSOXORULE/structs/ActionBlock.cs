using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXORULE] 2.2.5.1 ActionBlock Structure
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
        /// The parsing context that determines count field widths.
        /// </summary>
        private readonly PropertyCountContext context;

        /// <summary>
        /// Initializes a new instance of the ActionBlock class
        /// </summary>
        /// <param name="countContext">The parsing context that determines count field widths.</param>
        public ActionBlock(PropertyCountContext countContext)
        {
            context = countContext;
        }

        /// <summary>
        /// Parse the ActionBlock structure.
        /// </summary>
        protected override void Parse()
        {
            switch (context)
            {
                case PropertyCountContext.RopBuffers:
                    ActionLength = ParseAs<ushort, uint>();
                    break;
                default:
                case PropertyCountContext.ExtendedRules:
                case PropertyCountContext.MapiHttp:
                case PropertyCountContext.AddressBook:
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
                bool isExtended = (context != PropertyCountContext.RopBuffers);
                
                if ((ActionType.OP_MOVE == _actionType || ActionType.OP_COPY == _actionType) && !isExtended)
                {
                    ActionData = new OP_MOVE_and_OP_COPY_ActionData_forStandard();
                    ActionData.Parse(parser);
                }
                else if ((ActionType.OP_MOVE == _actionType || ActionType.OP_COPY == _actionType) && isExtended)
                {
                    ActionData = new OP_MOVE_and_OP_COPY_ActionData_forExtended();
                    ActionData.Parse(parser);
                }
                else if ((ActionType.OP_REPLY == _actionType || ActionType.OP_OOF_REPLY == _actionType) && !isExtended)
                {
                    ActionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard();
                    ActionData.Parse(parser);
                }
                else if ((ActionType.OP_REPLY == _actionType || ActionType.OP_OOF_REPLY == _actionType) && isExtended)
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
                    ActionData = new TaggedPropertyValue(context);
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
