using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MapiInspector;
using System.Reflection;
using Fiddler;

namespace MAPIInspector.Parsers
{
    #region 2.2.1	RopModifyRules ROP
    /// <summary>
    /// The RopModifyRules ROP ([MS-OXCROPS] section 2.2.11.1) creates, modifies, or deletes rules (2) in a folder.
    /// </summary>
    public class RopModifyRulesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A bitmask that specifies how the rules (2) included in this structure are created on the server.
        public ModifyRulesFlags ModifyRulesFlags;

        // An integer that specifies the number of RuleData structures present in the RulesData field.
        public ushort RulesCount;

        // An array of RuleData structures, each of which specifies details about a standard rule. 
        public RuleData[] RulesData;

        /// <summary>
        /// Parse the RopModifyRulesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing the RopModifyRulesRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ModifyRulesFlags = new ModifyRulesFlags();
            this.ModifyRulesFlags.Parse(s);
            this.RulesCount = ReadUshort();
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

    ///  <summary>
    /// A class indicates the ModifyRulesFlags ROP Response Buffer.
    /// </summary>
    public class ModifyRulesFlags : BaseStructure
    {
        //Unused. This bit MUST be set to zero (0) when sent.
        [BitAttribute(7)]
        public byte X;

        // If this bit is set, the rules (2) in this request are to replace the existing set of rules (2) in the folde.
        [BitAttribute(1)]
        public byte R;

        /// <summary>
        /// Parse the ModifyRulesFlags structure.
        /// </summary>
        /// <param name="s">An stream containing ModifyRulesFlags structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s); // TODO: need to modify the AddTreeNode method about the pos and length.  
            Byte tempByte = ReadByte();
            int index = 0;
            this.X = GetBits(tempByte, index, 7);
            index = index + 7;
            this.R = GetBits(tempByte, index, 1);
        }
    }

    ///  <summary>
    /// A class indicates the RopModifyRules ROP Response Buffer.
    /// </summary>
    public class RopModifyRulesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopModifyRulesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing the RopModifyRulesResponse structure</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }

    #region 2.2.1.3	RuleData
    /// <summary>
    /// The enum vlaue of RuleDataFlags
    /// </summary>
    [Flags]
    public enum RuleDataFlags : byte
    {
        ROW_ADD = 0x01,
        ROW_MODIFY = 0x02,
        ROW_REMOVE = 0x04
    }

    ///  <summary>
    /// The RuleData structure contains properties and flags that provide details about a standard rule. 
    /// </summary>
    public class RuleData : BaseStructure
    {
        // A value that contains flags specifying whether the rule (2) is to be added, modified, or deleted. 
        public RuleDataFlags RuleDataFlags;

        // An integer that specifies the number of properties that are specified in the PropertyValues field. 
        public ushort PropertyValueCount;

        // An array of TaggedPropertyValue structures, as specified in [MS-OXCDATA] section 2.11.4, each of which contains one property of a standard rule. 
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RuleData structure.
        /// </summary>
        /// <param name="s">A stream containing the RuleData structure</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RuleDataFlags = (RuleDataFlags)ReadByte();
            this.PropertyValueCount = ReadUshort();
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
    /// The enum value of TableFlags.
    /// </summary>
    public enum TableFlags : byte
    {

        U_0x40 = 0x40,
        U_0x00 = 0x00

    }

    ///  <summary>
    /// The RopGetRulesTable ROP ([MS-OXCROPS] section 2.2.11.2) creates a Table object through which the client can access the standard rules in a folder using table operations as specified in [MS-OXCTABL]. 
    /// </summary>
    public class RopGetRulesTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        //  A flags structure that contains flags that control the type of table. 
        public TableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetRulesTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetRulesTableRequest structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.TableFlags = (TableFlags)ReadByte();
        }
    }

    ///  <summary>
    /// A class indicates the RopGetRulesTable ROP Response Buffer.
    /// </summary>
    public class RopGetRulesTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopGetRulesTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetRulesTableResponse structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3	RopUpdateDeferredActionMessages ROP
    /// <summary>
    /// The RopUpdateDeferredActionMessages ROP ([MS-OXCROPS] section 2.2.11.3) instructs the server to update the PidTagDamOriginalEntryId property (section 2.2.6.3) on one or more DAMs.
    /// </summary>
    public class RopUpdateDeferredActionMessagesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the ServerEntryId field.
        public ushort ServerEntryIdSize;

        // An array of bytes that specifies the ID of the message on the server. 
        public byte[] ServerEntryId;

        // An unsigned integer that specifies the size of the ClientEntryId field.
        public ushort ClientEntryIdSize;

        // An array of bytes that specifies the ID of the downloaded message on the client. 
        public byte[] ClientEntryId;

        /// <summary>
        /// Parse the RopUpdateDeferredActionMessagesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopUpdateDeferredActionMessagesRequest structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ServerEntryIdSize = ReadUshort();
            this.ServerEntryId = ReadBytes((int)ServerEntryIdSize);
            this.ClientEntryIdSize = ReadUshort();
            this.ClientEntryId = ReadBytes((int)ClientEntryIdSize);
        }
    }

    /// <summary>
    /// A class indicates the RopUpdateDeferredActionMessages ROP Response Buffer.
    /// </summary>
    public class RopUpdateDeferredActionMessagesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopUpdateDeferredActionMessagesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopUpdateDeferredActionMessagesResponse structure.</param>
        public override void Parse(System.IO.Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.5	RuleAction Structure
    /// <summary>
    /// 2.2.5	RuleAction Structure
    /// </summary>
    public class RuleAction : BaseStructure
    {
        //Specifies the number of structures that are contained in the ActionBlocks field. For extended rules, the size of the NoOfActions field is 4 bytes instead of 2 bytes.
        public object NoOfActions;

        // An array of ActionBlock structures, each of which specifies an action (2) of the rule (2), as specified in section 2.2.5.1.
        public ActionBlock[] ActionBlocks;

        // The wide size of NoOfActions.
        private CountWideEnum countWide;

        /// <summary>
        ///  The Constructor to set the NoOfActions wide size.
        /// </summary>
        /// <param name="wide">The wide size of NoOfActions.</param>
        public RuleAction(CountWideEnum wide = CountWideEnum.twoBytes)
        {
            countWide = wide;
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
            for (int i = 0; i < NoOfActions.GetHashCode(); i++)
            {
                ActionBlock tempActionBlock = new ActionBlock(CountWideEnum.twoBytes);
                tempActionBlock.Parse(s);
                tempActionBlocks.Add(tempActionBlock);
            }
            this.ActionBlocks = tempActionBlocks.ToArray();
        }
    }

    /// <summary>
    /// 2.2.5.1	ActionBlock Structure
    /// </summary>
    public class ActionBlock : BaseStructure
    {
        // An integer that specifies the cumulative length, in bytes, of the subsequent fields in this ActionBlock structure. For extended rules, the size of the ActionLength field is 4 bytes instead of 2 bytes.
        public object ActionLength;

        // An integer that specifies the type of action (2). 
        public ActionType ActionType;

        // The flags that are associated with a particular type of action (2). 
        public object ActionFlavor;

        // Client-defined flags. The ActionFlags field is used solely by the client
        public uint ActionFlags;

        // An ActionData structure, as specified in section 2.2.5.1.2, that specifies data related to the particular action (2).
        public object ActionData;

        // The wide size of NoOfActions.
        private CountWideEnum countWide;

        /// <summary>
        ///  The Constructor to set the ActionLength wide size.
        /// </summary>
        /// <param name="wide">The wide size of ActionLength.</param>
        public ActionBlock(CountWideEnum wide = CountWideEnum.twoBytes)
        {
            countWide = wide;
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
            switch (ActionType)
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

            this.ActionFlags = ReadUint();
            if ((ActionLength.GetHashCode() > 9))
            {
                if ((ActionType.OP_MOVE == ActionType || ActionType.OP_COPY == ActionType) && countWide.Equals(CountWideEnum.twoBytes))
                {
                    OP_MOVE_and_OP_COPY_ActionData_forStandard actionData = new OP_MOVE_and_OP_COPY_ActionData_forStandard();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if ((ActionType.OP_MOVE == ActionType || ActionType.OP_COPY == ActionType) && countWide.Equals(CountWideEnum.fourBytes))
                {
                    OP_MOVE_and_OP_COPY_ActionData_forExtended actionData = new OP_MOVE_and_OP_COPY_ActionData_forExtended();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if ((ActionType.OP_REPLY == ActionType || ActionType.OP_OOF_REPLY == ActionType) && countWide.Equals(CountWideEnum.twoBytes))
                {
                    OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard actionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if ((ActionType.OP_REPLY == ActionType || ActionType.OP_OOF_REPLY == ActionType) && countWide.Equals(CountWideEnum.fourBytes))
                {
                    OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended actionData = new OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_FORWARD == ActionType || ActionType.OP_DELEGATE == ActionType)
                {
                    OP_FORWARD_and_OP_DELEGATE_ActionData actionData = new OP_FORWARD_and_OP_DELEGATE_ActionData();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_BOUNCE == ActionType)
                {
                    OP_BOUNCE_ActionData actionData = new OP_BOUNCE_ActionData();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_TAG == ActionType)
                {
                    OP_TAG_ActionData actionData = new OP_TAG_ActionData();
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
                else if (ActionType.OP_DEFER_ACTION == ActionType)
                {
                    OP_DEFER_ACTION actionData = new OP_DEFER_ACTION(ActionLength.GetHashCode());
                    actionData.Parse(s);
                    this.ActionData = actionData;
                }
            }
        }

    }

    /// <summary>
    /// The enum value of ActionType.
    /// </summary>
    public enum ActionType : byte
    {
        OP_MOVE = 0x01,
        OP_COPY = 0x02,
        OP_REPLY = 0x03,
        OP_OOF_REPLY = 0x04,
        OP_DEFER_ACTION = 0x05,
        OP_BOUNCE = 0x06,
        OP_FORWARD = 0x07,
        OP_DELEGATE = 0x08,
        OP_TAG = 0x09,
        OP_DELETE = 0x0A,
        OP_MARK_AS_READ = 0x0B
    }

    #region 2.2.5.1.1	Action Flavors
    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is relate to FORWARD
    /// </summary>
    public class ActionFlavor_Forward : BaseStructure
    {
        // The reserved bit.
        [BitAttribute(4)]
        public int Reserved_bits_0;

        // Indicates that the message SHOULD<5> be forwarded as a Short Message Service (SMS) text message. 
        [BitAttribute(1)]
        public int TM;

        // Forwards the message as an attachment. This value MUST NOT be combined with other ActionFlavor flags.
        [BitAttribute(1)]
        public int AT;

        // Forwards the message without making any changes to the message. 
        [BitAttribute(1)]
        public int NC;

        // Preserves the sender information and indicates that the message was autoforwarded. 
        [BitAttribute(1)]
        public int PR;

        // The reserved bit.3 bytes.
        public byte[] Reserved_bits_1;

        /// <summary>
        /// Parse the ActionFlavor_Forward structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionFlavor_Forward structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte tempbyte = ReadByte();
            int index = 0;
            this.Reserved_bits_0 = GetBits(tempbyte, index, 4);
            index += 4;
            this.TM = GetBits(tempbyte, index, 1);
            index += 1;
            this.AT = GetBits(tempbyte, index, 1);
            index += 1;
            this.NC = GetBits(tempbyte, index, 1);
            index += 1;
            this.PR = GetBits(tempbyte, index, 1);

            this.Reserved_bits_1 = ReadBytes(3);
        }
    }

    /// <summary>
    ///  This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is relate to REPLY
    /// </summary>
    public class ActionFlavor_Reply : BaseStructure
    {
        // The reserved bit.
        [BitAttribute(6)]
        public int Reserved_bits_0;

        // Server will use fixed, server-defined text in the reply message and ignore the text in the reply template. 
        [BitAttribute(1)]
        public int ST;

        // The server SHOULD<6> not send the message to the message sender (the reply template MUST contain recipients (2) in this case).
        [BitAttribute(1)]
        public int NS;

        // The reserved bit.3 bytes
        public byte[] Reserved_bits_1;

        /// <summary>
        /// Parse the ActionFlavor_Reply structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionFlavor_Reply structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte tempbyte = ReadByte();
            int index = 0;
            this.Reserved_bits_0 = GetBits(tempbyte, index, 6);
            index += 6;
            this.ST = GetBits(tempbyte, index, 1);
            index += 1;
            this.NS = GetBits(tempbyte, index, 1);
            this.Reserved_bits_1 = ReadBytes(3);
        }
    }

    /// <summary>
    ///  This type is specified in MS-OXORULE section 2.2.5.1.1 ActionFlavor structure when ActionType is not related to REPLY or FORWARD 
    /// </summary>
    public class ActionFlavor_Reserved : BaseStructure
    {
        // The reserved bits.
        public int Reserved_bits;

        /// <summary>
        /// Parse the ActionFlavor_Reserved structure.
        /// </summary>
        /// <param name="s">A stream containing the ActionFlavor_Reserved structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved_bits = ReadINT32();
        }
    }
    #endregion

    #region 2.2.5.1.2	ActionData Structure
    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure for Standard Rules
    /// </summary>
    public class OP_MOVE_and_OP_COPY_ActionData_forStandard : BaseStructure
    {
        // A Boolean value that indicates whether the folder is in the user's mailbox or a different mailbox.
        public bool FolderInThisStore;

        // An integer that specifies the size, in bytes, of the StoreEID field.
        public ushort StoreEIDSize;

        // A Store Object EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.3, that identifies the message store. 
        public object StoreEID;

        // An integer that specifies the size, in bytes, of the FolderEID field.
        public ushort FolderEIDSize;

        // A structure that identifies the destination folder.
        public object FolderEID;

        /// <summary>
        /// Parse the OP_MOVE_and_OP_COPY_ActionData_forStandard structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_MOVE_and_OP_COPY_ActionData_forStandard structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.FolderInThisStore = ReadBoolean();
            this.StoreEIDSize = ReadUshort();
            if (FolderInThisStore)
            {
                MAPIString storeEID = new MAPIString(Encoding.ASCII, "", StoreEIDSize);
                storeEID.Parse(s);
                this.StoreEID = storeEID;
            }
            else
            {
                StoreObjectEntryID storeEID = new StoreObjectEntryID();
                storeEID.Parse(s);
                this.StoreEID = storeEID;
            }
            this.FolderEIDSize = ReadUshort();
            if (FolderInThisStore)
            {
                ServerEid folderEID = new ServerEid();
                folderEID.Parse(s);
                this.FolderEID = folderEID;
            }
            else
            {
                this.FolderEID = ReadBytes(FolderEIDSize);
            }

        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.1 OP_MOVE and OP_COPY ActionData Structure for Extended Rules
    /// </summary>
    public class OP_MOVE_and_OP_COPY_ActionData_forExtended : BaseStructure
    {
        // An integer that specifies the size, in bytes, of the StoreEID field.
        public uint StoreEIDSize;

        // This field is not used and can be set to any non-null value by the client and the server. 
        public MAPIString StoreEID;

        // An integer that specifies the size, in bytes, of the FolderEID field.
        public uint FolderEIDSize;

        // A Folder EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.1, that identifies the destination folder. 
        public FolderEntryID FolderEID;

        /// <summary>
        /// Parse the OP_MOVE_and_OP_COPY_ActionData_forExtended structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_MOVE_and_OP_COPY_ActionData_forExtended structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.StoreEIDSize = ReadUint();
            this.StoreEID = new MAPIString(Encoding.ASCII, "", (int)StoreEIDSize);
            this.StoreEID.Parse(s);
            this.FolderEIDSize = ReadUint();
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
        // The value 0x01 indicates that the remaining bytes conform to this structure; 
        public byte Ours;

        // A Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the destination folder.
        public FolderID FolderId;

        // This field is not used and MUST be set to all zeros.
        public ulong MessageId;

        // This field is not used and MUST be set to all zeros.
        public int Instance;

        /// <summary>
        /// Parse the ServerEid structure.
        /// </summary>
        /// <param name="s">A stream containing the ServerEid structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Ours = ReadByte();
            FolderID folderId = new FolderID();
            this.FolderId = folderId;
            this.FolderId.Parse(s);
            this.MessageId = ReadUlong();
            this.Instance = ReadINT32();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.2 OP_REPLY and OP_OOF_REPLY ActionData Structure for Standard Rules
    /// </summary>
    public class OP_REPLY_and_OP_OOF_REPLY_ActionData_forStandard : BaseStructure
    {
        // A Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, that identifies the folder that contains the reply template.
        public FolderID ReplyTemplateFID;

        // A Message ID structure, as specified in [MS-OXCDATA] section 2.2.1.2, that identifies the FAI message being used as the reply template.
        public MessageID ReplyTemplateMID;

        // A GUID that is generated by the client in the process of creating a reply template. 
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
            this.ReplyTemplateGUID = ReadGuid();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.2 OP_REPLY and OP_OOF_REPLY ActionData Structure for Extended Rules
    /// </summary>
    public class OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended : BaseStructure
    {
        // An integer that specifies the size, in bytes, of the ReplyTemplateMessageEID field.
        public uint MessageEIDSize;

        // A Message EntryID structure, as specified in [MS-OXCDATA] section 2.2.4.2, that contains the entry ID of the reply template.
        public MessageEntryID ReplyTemplateMessageEID;

        // A GUID that is generated by the client in the process of creating a reply template. 
        public Guid ReplyTemplateGUID;

        /// <summary>
        /// Parse the OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_REPLY_and_OP_OOF_REPLY_ActionData_forExtended structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.MessageEIDSize = ReadUint();
            MessageEntryID replyTemplateMessageEID = new MessageEntryID();
            this.ReplyTemplateMessageEID = replyTemplateMessageEID;
            this.ReplyTemplateMessageEID.Parse(s);
            this.ReplyTemplateGUID = ReadGuid();
        }
    }

    /// <summary>
    /// This type is specified in MS-OXORULE section 2.2.5.1.2.4 OP_FORWARD and OP_DELEGATE ActionData Structure
    /// </summary>
    public class OP_FORWARD_and_OP_DELEGATE_ActionData : BaseStructure
    {
        // An integer that specifies the number of RecipientBlockData structures, as specified in section 2.2.5.1.2.4.1, contained in the RecipientBlocks field.
        public ushort RecipientCount;

        // An array of RecipientBlockData structures, each of which specifies information about one recipient (2). 
        public RecipientBlockData[] RecipientBlocks;

        /// <summary>
        /// Parse the OP_FORWARD_and_OP_DELEGATE_ActionData structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_FORWARD_and_OP_DELEGATE_ActionData structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RecipientCount = ReadUshort();
            List<RecipientBlockData> recipientBlocks = new List<RecipientBlockData>();
            for (int i = 0; i < RecipientCount; i++)
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
        // This value is implementation-specific and not required for interoperability
        public byte Reserved;

        // An integer that specifies the number of structures present in the PropertyValues field. This number MUST be greater than zero.
        public ushort NoOfProperties;

        // An array of TaggedPropertyValue structures, each of which contains a property that provides some information about the recipient (2). 
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RecipientBlockData structure.
        /// </summary>
        /// <param name="s">A stream containing the RecipientBlockData structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadByte();
            this.NoOfProperties = ReadUshort();
            List<TaggedPropertyValue> propertyValues = new List<TaggedPropertyValue>();
            for (int i = 0; i < NoOfProperties; i++)
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
        // An integer that specifies a bounce code.
        public BounceCodeEnum BounceCode;

        /// <summary>
        /// Parse the OP_BOUNCE_ActionData structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_BOUNCE_ActionData structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.BounceCode = (BounceCodeEnum)ReadUint();
        }
    }

    /// <summary>
    /// The enum value of Bounce Code.
    /// </summary>
    public enum BounceCodeEnum : uint
    {
        RejectedMessageTooLarge = 0x0000000D,
        RejectedMessageNotDisplayed = 0x0000001F,
        DeliveryMessageDenied = 0x00000026

    }

    ///  <summary>
    ///  This type is specified in MS-OXORULE section 2.2.5.1.2.3 OP_DEFER_ACTION ActionData Structure
    /// </summary>
    public class OP_DEFER_ACTION : BaseStructure
    {
        // The length of DeferActionData
        private int length;

        // The defer Action data.
        public byte[] DeferActionData;

        /// <summary>
        /// The Constructor to set the DeferActionData length.
        /// </summary>
        /// <param name="size">The size.</param>
        public OP_DEFER_ACTION(int size)
        {
            length = size - 9;
        }
        /// <summary>
        /// Parse the OP_DEFER_ACTION structure.
        /// </summary>
        /// <param name="s">A stream containing the OP_DEFER_ACTION structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.DeferActionData = ReadBytes(length);
        }
    }

    /// <summary>
    /// An OP_TAG ActionData structure is a TaggedPropertyValue structure, packaged as specified in [MS-OXCDATA] section 2.11.4.
    /// </summary>
    public class OP_TAG_ActionData : TaggedPropertyValue
    {
        //None, class OP_TAG_ActionData is same as TaggedPropertyValue.
    }

    #endregion
    #endregion
}
