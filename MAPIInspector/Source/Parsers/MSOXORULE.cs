namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
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

    /// <summary>
    /// The enum value of TableFlags.
    /// </summary>
    public enum TableFlags : byte
    {
        /// <summary>
        /// This bit is set if the client is requesting that string values in the table be returned as Unicode strings
        /// </summary>
        U_0x40 = 0x40,

        /// <summary>
        /// These unused bits MUST be set to zero by the client
        /// </summary>
        U_0x00 = 0x00
    }

    /// <summary>
    /// The enum value of ActionType.
    /// </summary>
    public enum ActionType : byte
    {
        /// <summary>
        /// Moves the message to a folder. MUST NOT be used in a public folder rule
        /// </summary>
        OP_MOVE = 0x01,

        /// <summary>
        /// Copies the message to a folder. MUST NOT be used in a public folder rule
        /// </summary>
        OP_COPY = 0x02,

        /// <summary>
        /// Replies to the message
        /// </summary>
        OP_REPLY = 0x03,

        /// <summary>
        /// Sends an OOF reply to the message
        /// </summary>
        OP_OOF_REPLY = 0x04,

        /// <summary>
        /// Used for actions that cannot be executed by the server
        /// </summary>
        OP_DEFER_ACTION = 0x05,

        /// <summary>
        /// Rejects the message back to the sender.
        /// </summary>
        OP_BOUNCE = 0x06,

        /// <summary>
        /// Forwards the message to a recipient (2) address
        /// </summary>
        OP_FORWARD = 0x07,

        /// <summary>
        /// Resends the message to another recipient (2), who acts as a delegate
        /// </summary>
        OP_DELEGATE = 0x08,

        /// <summary>
        /// Adds or changes a property on the message
        /// </summary>
        OP_TAG = 0x09,

        /// <summary>
        /// Deletes the message.
        /// </summary>
        OP_DELETE = 0x0A,

        /// <summary>
        /// Sets the MSGFLAG_READ flag in the PidTagMessageFlags property ([MS-OXCMSG] section 2.2.1.6) on the message
        /// </summary>
        OP_MARK_AS_READ = 0x0B
    }

    /// <summary>
    /// The enum value of Bounce Code.
    /// </summary>
    public enum BounceCodeEnum : uint
    {
        /// <summary>
        /// The message was rejected because it was too large
        /// </summary>
        RejectedMessageTooLarge = 0x0000000D,

        /// <summary>
        /// The message was rejected because it cannot be displayed to the user
        /// </summary>
        RejectedMessageNotDisplayed = 0x0000001F,

        /// <summary>
        /// The message delivery was denied for other reasons
        /// </summary>
        DeliveryMessageDenied = 0x00000026
    }

    #region 2.2.1	RopModifyRules ROP
    /// <summary>
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
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ModifyRulesFlags = new ModifyRulesFlags();
            this.ModifyRulesFlags.Parse(s);
            this.RulesCount = this.ReadUshort();
            List<RuleData> tempRulesDatas = new List<RuleData>();
            for (int i = 0; i < this.RulesCount; i++)
            {
                RuleData tempRuleData = new RuleData();
                tempRuleData.Parse(s);
                tempRulesDatas.Add(tempRuleData);
            }

            this.RulesData = tempRulesDatas.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the ModifyRulesFlags ROP Response Buffer.
    /// </summary>
    public class ModifyRulesFlags : BaseStructure
    {
        /// <summary>
        /// Unused. This bit MUST be set to zero (0) when sent.
        /// </summary>
        [BitAttribute(7)]
        public byte X;

        /// <summary>
        /// If this bit is set, the rules (2) in this request are to replace the existing set of rules (2) in the folder.
        /// </summary>
        [BitAttribute(1)]
        public byte R;

        /// <summary>
        /// Parse the ModifyRulesFlags structure.
        /// </summary>
        /// <param name="s">A stream containing ModifyRulesFlags structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s); // TODO: need to modify the AddTreeNode method about the position and length.  
            byte tempByte = this.ReadByte();
            int index = 0;
            this.X = this.GetBits(tempByte, index, 7);
            index = index + 7;
            this.R = this.GetBits(tempByte, index, 1);
        }
    }

    /// <summary>
    /// A class indicates the RopModifyRules ROP Response Buffer.
    /// </summary>
    public class RopModifyRulesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopModifyRulesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing the RopModifyRulesResponse structure</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }

    #region 2.2.1.3	RuleData

    /// <summary>
    /// The RuleData structure contains properties and flags that provide details about a standard rule. 
    /// </summary>
    public class RuleData : BaseStructure
    {
        /// <summary>
        /// A value that contains flags specifying whether the rule (2) is to be added, modified, or deleted. 
        /// </summary>
        public RuleDataFlags RuleDataFlags;

        /// <summary>
        /// An integer that specifies the number of properties that are specified in the PropertyValues field. 
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures, as specified in [MS-OXCDATA] section 2.11.4, each of which contains one property of a standard rule. 
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RuleData structure.
        /// </summary>
        /// <param name="s">A stream containing the RuleData structure</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RuleDataFlags = (RuleDataFlags)this.ReadByte();
            this.PropertyValueCount = this.ReadUshort();
            List<TaggedPropertyValue> tempPropertyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                TaggedPropertyValue temptaggedPropertyValue = new TaggedPropertyValue(CountWideEnum.twoBytes);
                temptaggedPropertyValue.Parse(s);
                tempPropertyValues.Add(temptaggedPropertyValue);
            }

            this.PropertyValues = tempPropertyValues.ToArray();
        }
    }
    #endregion

    #endregion

    #region 2.2.2	RopGetRulesTable ROP

    /// <summary>
    /// The RopGetRulesTable ROP ([MS-OXCROPS] section 2.2.11.2) creates a Table object through which the client can access the standard rules in a folder using table operations as specified in [MS-OXCTABL]. 
    /// </summary>
    public class RopGetRulesTableRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control the type of table. 
        /// </summary>
        public TableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetRulesTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetRulesTableRequest structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.TableFlags = (TableFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopGetRulesTable ROP Response Buffer.
    /// </summary>
    public class RopGetRulesTableResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopGetRulesTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetRulesTableResponse structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3	RopUpdateDeferredActionMessages ROP
    /// <summary>
    /// The RopUpdateDeferredActionMessages ROP ([MS-OXCROPS] section 2.2.11.3) instructs the server to update the PidTagDamOriginalEntryId property (section 2.2.6.3) on one or more DAMs.
    /// </summary>
    public class RopUpdateDeferredActionMessagesRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the ServerEntryId field.
        /// </summary>
        public ushort ServerEntryIdSize;

        /// <summary>
        /// An array of bytes that specifies the ID of the message on the server. 
        /// </summary>
        public byte[] ServerEntryId;

        /// <summary>
        /// An unsigned integer that specifies the size of the ClientEntryId field.
        /// </summary>
        public ushort ClientEntryIdSize;

        /// <summary>
        /// An array of bytes that specifies the ID of the downloaded message on the client. 
        /// </summary>
        public byte[] ClientEntryId;

        /// <summary>
        /// Parse the RopUpdateDeferredActionMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopUpdateDeferredActionMessagesRequest structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ServerEntryIdSize = this.ReadUshort();
            this.ServerEntryId = this.ReadBytes((int)this.ServerEntryIdSize);
            this.ClientEntryIdSize = this.ReadUshort();
            this.ClientEntryId = this.ReadBytes((int)this.ClientEntryIdSize);
        }
    }

    /// <summary>
    /// A class indicates the RopUpdateDeferredActionMessages ROP Response Buffer.
    /// </summary>
    public class RopUpdateDeferredActionMessagesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopUpdateDeferredActionMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopUpdateDeferredActionMessagesResponse structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.5	RuleAction Structure
    /// <summary>
    /// 2.2.5 RuleAction Structure
    /// </summary>
    public class RuleAction : BaseStructure
    {
        /// <summary>
        /// Specifies the number of structures that are contained in the ActionBlocks field. For extended rules, the size of the NoOfActions field is 4 bytes instead of 2 bytes.
        /// </summary>
        public object NoOfActions;

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
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the RuleAction structure.
        /// </summary>
        /// <param name="s">A stream containing the RuleAction structure</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.NoOfActions = help.ReadCount(this.countWide, s);
            List<ActionBlock> tempActionBlocks = new List<ActionBlock>();
            for (int i = 0; i < this.NoOfActions.GetHashCode(); i++)
            {
                ActionBlock tempActionBlock = new ActionBlock(CountWideEnum.twoBytes);
                tempActionBlock.Parse(s);
                tempActionBlocks.Add(tempActionBlock);
            }

            this.ActionBlocks = tempActionBlocks.ToArray();
        }
    }

    /// <summary>
    /// 2.2.5.1 ActionBlock Structure
    /// </summary>
    public class ActionBlock : BaseStructure
    {
        /// <summary>
        /// An integer that specifies the cumulative length, in bytes, of the subsequent fields in this ActionBlock structure. For extended rules, the size of the ActionLength field is 4 bytes instead of 2 bytes.
        /// </summary>
        public object ActionLength;

        /// <summary>
        /// An integer that specifies the type of action (2). 
        /// </summary>
        public ActionType ActionType;

        /// <summary>
        /// The flags that are associated with a particular type of action (2). 
        /// </summary>
        public object ActionFlavor;

        /// <summary>
        /// Client-defined flags. The ActionFlags field is used solely by the client
        /// </summary>
        public uint ActionFlags;

        /// <summary>
        /// An ActionData structure, as specified in section 2.2.5.1.2, that specifies data related to the particular action (2).
        /// </summary>
        public object ActionData;

        /// <summary>
        /// The wide size of NoOfActions.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        ///  Initializes a new instance of the ActionBlock class.
        /// </summary>
        /// <param name="wide">The wide size of ActionLength.</param>
        public ActionBlock(CountWideEnum wide = CountWideEnum.twoBytes)
        {
            this.countWide = wide;
        }

        /// <summary>
        /// Parse the ActionBlock structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionBlock structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            this.ActionLength = help.ReadCount(this.countWide, s);
            this.ActionType = (ActionType)ReadByte();
            switch (this.ActionType)
            {
                case ActionType.OP_REPLY:
                    {
                        ActionFlavor_Reply action = new ActionFlavor_Reply();
                        action.Parse(s);
                        this.ActionFlavor = action;
                        break;
                    }

                case ActionType.OP_OOF_REPLY:
                    {
                        ActionFlavor_Reply action = new ActionFlavor_Reply();
                        action.Parse(s);
                        this.ActionFlavor = action;
                        break;
                    }

                case ActionType.OP_FORWARD:
                    {
                        ActionFlavor_Forward action = new ActionFlavor_Forward();
                        action.Parse(s);
                        this.ActionFlavor = action;
                        break;
                    }

                default:
                    {
                        ActionFlavor_Reserved action = new ActionFlavor_Reserved();
                        action.Parse(s);
                        this.ActionFlavor = action;
                        break;
                    }
            }

            this.ActionFlags = this.ReadUint();
            if (this.ActionLength.GetHashCode() > 9)
            {
                if ((ActionType.OP_MOVE == this.ActionType || ActionType.OP_COPY == this.ActionType) && this.countWide.Equals(CountWideEnum.twoBytes))
                {
                    OP_MOVE_and_OP_COPY_ActionData_forStandard actionData = new OP_MOVE_and_OP_COPY_ActionData_forStandard();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if ((ActionType.OP_MOVE == this.ActionType || ActionType.OP_COPY == this.ActionType) && this.countWide.Equals(CountWideEnum.fourBytes))
                {
                    OP_MOVE_and_OP_COPY_ActionData_forExtended actionData = new OP_MOVE_and_OP_COPY_ActionData_forExtended();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if ((ActionType.OP_REPLY == this.ActionType || ActionType.OP_OOF_REPLY == this.ActionType) && this.countWide.Equals(CountWideEnum.twoBytes))
                {
                    OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard actionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if ((ActionType.OP_REPLY == this.ActionType || ActionType.OP_OOF_REPLY == this.ActionType) && this.countWide.Equals(CountWideEnum.fourBytes))
                {
                    OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended actionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_FORWARD == this.ActionType || ActionType.OP_DELEGATE == this.ActionType)
                {
                    OP_FORWARD_and_OP_DELEGATE_ActionData actionData = new OP_FORWARD_and_OP_DELEGATE_ActionData();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_BOUNCE == this.ActionType)
                {
                    OP_BOUNCE_ActionData actionData = new OP_BOUNCE_ActionData();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_TAG == this.ActionType)
                {
                    OP_TAG_ActionData actionData = new OP_TAG_ActionData();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_DEFER_ACTION == this.ActionType)
                {
                    OP_DEFER_ACTION actionData = new OP_DEFER_ACTION(this.ActionLength.GetHashCode());
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
            }
        }
    }

    #region 2.2.5.1.1	Action Flavors
    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is relate to FORWARD
    /// </summary>
    public class ActionFlavor_Forward : BaseStructure
    {
        /// <summary>
        /// The reserved bit.
        /// </summary>
        [BitAttribute(4)]
        public int Reservedbits0;

        /// <summary>
        /// Indicates that the message SHOULD be forwarded as a Short Message Service (SMS) text message. 
        /// </summary>
        [BitAttribute(1)]
        public byte TM;

        /// <summary>
        /// Forwards the message as an attachment. This value MUST NOT be combined with other ActionFlavor flags.
        /// </summary>
        [BitAttribute(1)]
        public byte AT;

        /// <summary>
        /// Forwards the message without making any changes to the message. 
        /// </summary>
        [BitAttribute(1)]
        public byte NC;

        /// <summary>
        /// Preserves the sender information and indicates that the message was auto forwarded. 
        /// </summary>
        [BitAttribute(1)]
        public byte PR;

        /// <summary>
        /// The reserved bit.3 bytes.
        /// </summary>
        public byte[] Reservedbits1;

        /// <summary>
        /// Parse the ActionFlavor_Forward structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionFlavor_Forward structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte tempbyte = this.ReadByte();
            int index = 0;
            this.Reservedbits0 = this.GetBits(tempbyte, index, 4);
            index += 4;
            this.TM = this.GetBits(tempbyte, index, 1);
            index += 1;
            this.AT = this.GetBits(tempbyte, index, 1);
            index += 1;
            this.NC = this.GetBits(tempbyte, index, 1);
            index += 1;
            this.PR = this.GetBits(tempbyte, index, 1);

            this.Reservedbits1 = this.ReadBytes(3);
        }
    }

    /// <summary>
    ///  This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is relate to REPLY
    /// </summary>
    public class ActionFlavor_Reply : BaseStructure
    {
        /// <summary>
        /// The reserved bit.
        /// </summary>
        [BitAttribute(6)]
        public byte Reservedbits0;

        /// <summary>
        /// Server will use fixed, server-defined text in the reply message and ignore the text in the reply template. 
        /// </summary>
        [BitAttribute(1)]
        public byte ST;

        /// <summary>
        /// The server SHOULD not send the message to the message sender (the reply template MUST contain recipients (2) in this case).
        /// </summary>
        [BitAttribute(1)]
        public byte NS;

        /// <summary>
        /// The reserved bit.3 bytes
        /// </summary>
        public byte[] Reservedbits1;

        /// <summary>
        /// Parse the ActionFlavor_Reply structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionFlavor_Reply structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte tempbyte = this.ReadByte();
            int index = 0;
            this.Reservedbits0 = this.GetBits(tempbyte, index, 6);
            index += 6;
            this.ST = this.GetBits(tempbyte, index, 1);
            index += 1;
            this.NS = this.GetBits(tempbyte, index, 1);
            this.Reservedbits1 = this.ReadBytes(3);
        }
    }

    /// <summary>
    ///  This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is not related to REPLY or FORWARD 
    /// </summary>
    public class ActionFlavor_Reserved : BaseStructure
    {
        /// <summary>
        /// The reserved bits.
        /// </summary>
        public int Reservedbits;

        /// <summary>
        /// Parse the ActionFlavor_Reserved structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionFlavor_Reserved structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reservedbits = this.ReadINT32();
        }
    }
    #endregion

    #region 2.2.5.1.2	ActionData Structure
    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure for Standard Rules
    /// </summary>
    public class OP_MOVE_and_OP_COPY_ActionData_forStandard : BaseStructure
    {
        /// <summary>
        /// A Boolean value that indicates whether the folder is in the user's mailbox or a different mailbox.
        /// </summary>
        public bool FolderInThisStore;

        /// <summary>
        /// An integer that specifies the size, in bytes, of the StoreEID field.
        /// </summary>
        public ushort StoreEIDSize;

        /// <summary>
        /// A Store Object EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.3, that identifies the message store. 
        /// </summary>
        public object StoreEID;

        /// <summary>
        /// An integer that specifies the size, in bytes, of the FolderEID field.
        /// </summary>
        public ushort FolderEIDSize;

        /// <summary>
        /// A structure that identifies the destination folder.
        /// </summary>
        public object FolderEID;

        /// <summary>
        /// Parse the OP_MOVE_and_OP_COPY_ActionData_forStandard structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_MOVE_and_OP_COPY_ActionData_forStandard structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.FolderInThisStore = this.ReadBoolean();
            this.StoreEIDSize = this.ReadUshort();

            // 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure
            // No matter the value of FolderInThisStore, the server tends to set StoreEIDSize to 0x0001.
            // So instead of parsing it, we'll just read StoreEIDSize bytes.
            this.StoreEID = this.ReadBytes(this.StoreEIDSize);

            this.FolderEIDSize = this.ReadUshort();
            if (this.FolderInThisStore)
            {
                ServerEid folderEID = new ServerEid();
                folderEID.Parse(s);
                this.FolderEID = folderEID;
            }
            else
            {
                this.FolderEID = this.ReadBytes(this.FolderEIDSize);
            }
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure for Extended Rules
    /// </summary>
    public class OP_MOVE_and_OP_COPY_ActionData_forExtended : BaseStructure
    {
        /// <summary>
        /// An integer that specifies the size, in bytes, of the StoreEID field.
        /// </summary>
        public uint StoreEIDSize;

        /// <summary>
        /// This field is not used and can be set to any non-null value by the client and the server. 
        /// </summary>
        public MAPIString StoreEID;

        /// <summary>
        /// An integer that specifies the size, in bytes, of the FolderEID field.
        /// </summary>
        public uint FolderEIDSize;

        /// <summary>
        /// A Folder EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.1, that identifies the destination folder. 
        /// </summary>
        public FolderEntryID FolderEID;

        /// <summary>
        /// Parse the OP_MOVE_and_OP_COPY_ActionData_forExtended structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_MOVE_and_OP_COPY_ActionData_forExtended structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.StoreEIDSize = this.ReadUint();
            this.StoreEID = new MAPIString(Encoding.ASCII, string.Empty, (int)this.StoreEIDSize);
            this.StoreEID.Parse(s);
            this.FolderEIDSize = this.ReadUint();
            FolderEntryID folderEID = new FolderEntryID();
            this.FolderEID = folderEID;
            this.FolderEID.Parse(s);
        }
    }

    /// <summary>
    ///  This type is specified in MS-OXORULE Section 2.2.5.1.2.1.1 ServerEid Structure
    /// </summary>
    public class ServerEid : BaseStructure
    {
        /// <summary>
        /// The value 0x01 indicates that the remaining bytes conform to this structure; 
        /// </summary>
        public byte Ours;

        /// <summary>
        /// A Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the destination folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// This field is not used and MUST be set to all zeros.
        /// </summary>
        public ulong MessageId;

        /// <summary>
        /// This field is not used and MUST be set to all zeros.
        /// </summary>
        public int Instance;

        /// <summary>
        /// Parse the ServerEid structure.
        /// </summary>
        /// <param name="s">A stream containing the ServerEid structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Ours = this.ReadByte();
            FolderID folderId = new FolderID();
            this.FolderId = folderId;
            this.FolderId.Parse(s);
            this.MessageId = this.ReadUlong();
            this.Instance = this.ReadINT32();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.2 OP_REPLY and OP_OOF_REPLY ActionData Structure for Standard Rules
    /// </summary>
    public class OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard : BaseStructure
    {
        /// <summary>
        /// A Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the folder that contains the reply template.
        /// </summary>
        public FolderID ReplyTemplateFID;

        /// <summary>
        /// A Message ID structure, as specified in [MS-OXCDATA] section 2.2.1.2, that identifies the FAI message being used as the reply template.
        /// </summary>
        public MessageID ReplyTemplateMID;

        /// <summary>
        /// A GUID that is generated by the client in the process of creating a reply template. 
        /// </summary>
        public Guid ReplyTemplateGUID;

        /// <summary>
        /// Parse the OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            FolderID replyTemplateFID = new FolderID();
            this.ReplyTemplateFID = replyTemplateFID;
            this.ReplyTemplateFID.Parse(s);
            MessageID replyTemplateMID = new MessageID();
            this.ReplyTemplateMID = replyTemplateMID;
            this.ReplyTemplateMID.Parse(s);
            this.ReplyTemplateGUID = this.ReadGuid();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.2 OP_REPLY and OP_OOF_REPLY ActionData Structure for Extended Rules
    /// </summary>
    public class OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended : BaseStructure
    {
        /// <summary>
        /// An integer that specifies the size, in bytes, of the ReplyTemplateMessageEID field.
        /// </summary>
        public uint MessageEIDSize;

        /// <summary>
        /// A Message EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.2, that contains the entry ID of the reply template.
        /// </summary>
        public MessageEntryID ReplyTemplateMessageEID;

        /// <summary>
        /// A GUID that is generated by the client in the process of creating a reply template. 
        /// </summary>
        public Guid ReplyTemplateGUID;

        /// <summary>
        /// Parse the OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.MessageEIDSize = this.ReadUint();
            MessageEntryID replyTemplateMessageEID = new MessageEntryID();
            this.ReplyTemplateMessageEID = replyTemplateMessageEID;
            this.ReplyTemplateMessageEID.Parse(s);
            this.ReplyTemplateGUID = this.ReadGuid();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.4 OP_FORWARD and OP_DELEGATE ActionData Structure
    /// </summary>
    public class OP_FORWARD_and_OP_DELEGATE_ActionData : BaseStructure
    {
        /// <summary>
        /// An integer that specifies the number of RecipientBlockData structures, as specified in section 2.2.5.1.2.4.1, contained in the RecipientBlocks field.
        /// </summary>
        public ushort RecipientCount;

        /// <summary>
        /// An array of RecipientBlockData structures, each of which specifies information about one recipient (2). 
        /// </summary>
        public RecipientBlockData[] RecipientBlocks;

        /// <summary>
        /// Parse the OP_FORWARD_and_OP_DELEGATE_ActionData structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_FORWARD_and_OP_DELEGATE_ActionData structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RecipientCount = this.ReadUshort();
            List<RecipientBlockData> recipientBlocks = new List<RecipientBlockData>();
            for (int i = 0; i < this.RecipientCount; i++)
            {
                RecipientBlockData recipientBlock = new RecipientBlockData();
                recipientBlock.Parse(s);
                recipientBlocks.Add(recipientBlock);
            }

            this.RecipientBlocks = recipientBlocks.ToArray();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.4.1 RecipientBlockData Structure
    /// </summary>
    public class RecipientBlockData : BaseStructure
    {
        /// <summary>
        /// This value is implementation-specific and not required for interoperability
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An integer that specifies the number of structures present in the PropertyValues field. This number MUST be greater than zero.
        /// </summary>
        public ushort NoOfProperties;

        /// <summary>
        /// An array of TaggedPropertyValue structures, each of which contains a property that provides some information about the recipient (2). 
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RecipientBlockData structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientBlockData structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadByte();
            this.NoOfProperties = this.ReadUshort();
            List<TaggedPropertyValue> propertyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < this.NoOfProperties; i++)
            {
                TaggedPropertyValue propertyValue = new TaggedPropertyValue();
                propertyValue.Parse(s);
                propertyValues.Add(propertyValue);
            }

            this.PropertyValues = propertyValues.ToArray();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.5 OP_BOUNCE ActionData Structure
    /// </summary>
    public class OP_BOUNCE_ActionData : BaseStructure
    {
        /// <summary>
        /// An integer that specifies a bounce code.
        /// </summary>
        public BounceCodeEnum BounceCode;

        /// <summary>
        /// Parse the OP_BOUNCE_ActionData structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_BOUNCE_ActionData structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.BounceCode = (BounceCodeEnum)this.ReadUint();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.3 OP_DEFER_ACTION ActionData Structure
    /// </summary>
    public class OP_DEFER_ACTION : BaseStructure
    {
        /// <summary>
        /// The defer Action data.
        /// </summary>
        public byte[] DeferActionData;

        /// <summary>
        /// The length of DeferActionData
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the OP_DEFER_ACTION class.
        /// </summary>
        /// <param name="size">The size.</param>
        public OP_DEFER_ACTION(int size)
        {
            this.length = size - 9;
        }

        /// <summary>
        /// Parse the OP_DEFER_ACTION structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_DEFER_ACTION structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.DeferActionData = this.ReadBytes(this.length);
        }
    }

    /// <summary>
    /// An OP_TAG ActionData structure is a TaggedPropertyValue structure, packaged as specified in [MS-OXCDATA] section 2.11.4.
    /// </summary>
    public class OP_TAG_ActionData : TaggedPropertyValue
    {
        // None, class OP_TAG_ActionData is same as TaggedPropertyValue.
    }

    #endregion
    #endregion
}
