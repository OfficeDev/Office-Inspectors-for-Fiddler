using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Text;
using Be.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region 2.2.2.1 CN
    /// <summary>
    /// Represents CN structure contains a change number that identifies a version of a messaging object. 
    /// </summary>
    public class CN : BaseStructure
    {
        // A 16-bit unsigned integer identifying the server replica in which the messaging object was last changed.
        public ushort replicaId;

        // An unsigned 48-bit integer identifying the change to the messaging object.
        [BytesAttribute(6)]
        public ulong globalCounter;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains CN.</param>
        public void Parse(FastTransferStream stream)
        {
            this.replicaId = stream.ReadUInt16();
            this.globalCounter = BitConverter.ToUInt64(stream.ReadBlock(6), 0);
        }
    }
    #endregion

    #region 2.2.2.2 XID
    /// <summary>
    /// Represents an external identifier for an entity within a data store.
    /// </summary>
    public class XID : BaseStructure
    {
        // A GUID that identifies the namespace that the identifier specified by LocalId belongs to
        public Guid namespaceGuid;

        // A variable binary value that contains the ID of the entity in the namespace specified by NamespaceGuid.
        public byte[] localId;

        // A unsigned int value specifies the length of the LocalId.
        private int length;

        /// <summary>
        /// Initializes a new instance of the XID structure.
        /// </summary>
        /// <param name="length">the length of the LocalId.</param>
        public XID(int length)
        {
            this.length = length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains XID.</param>
        public void Parse(FastTransferStream stream)
        {
            this.namespaceGuid = stream.ReadGuid();
            this.localId = stream.ReadBlock((int)this.length - 16);
        }
    }
    #endregion

    #region 2.2.2.3 PredecessorChangeList
    /// <summary>
    /// Contains a set of XIDs that represent change numbers of messaging objects in different replicas. 
    /// </summary>
    public class PredecessorChangeList : BaseStructure
    {
        // A SizedXid list.
        public SizedXid[] sizedXidList;

        // A unsigned int value specifies the length in bytes of the sizedXidList.
        private int length;

        /// <summary>
        /// Initializes a new instance of the PredecessorChangeList structure.
        /// </summary>
        /// <param name="length">The length of the sizedXid structure.</param>
        public PredecessorChangeList(int length)
        {
            this.length = length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PredecessorChangeList.</param>
        public void Parse(FastTransferStream stream)
        {
            List<SizedXid> interSizeXid = new List<SizedXid>();
            for (int i = 0; i < this.length; )
            {
                int position = (int)stream.Position;
                SizedXid tmpSizedXid = new SizedXid();
                tmpSizedXid.Parse(stream);
                interSizeXid.Add(tmpSizedXid);

                i += ((int)stream.Position - position);
            }
            this.sizedXidList = interSizeXid.ToArray();
        }
    }

    /// <summary>
    /// SizedXid structure.
    /// </summary>
    public class SizedXid : BaseStructure
    {
        // An unsigned 8-bit integer.
        public byte xidSize;

        // A structure of type XID that contains the value of the internal identifier of an object, or internal or external identifier of a change number. 
        public XID xid;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains SizedXid.</param>
        public void Parse(FastTransferStream stream)
        {
            this.xidSize = stream.ReadByte();
            this.xid = new XID((int)this.xidSize);
            this.xid.Parse(stream);
        }
    }
    #endregion

    #region 2.2.2.4 IDSET Structure
    /// <summary>
    /// Represents a REPLID and GLOBSET structure pair. 
    /// </summary>
    public class IDSET_REPLID : BaseStructure
    {
        // A unsigned short which combined with all GLOBCNT structures contained in the GLOBSET field, produces a set of IDs.
        public ushort REPLID;

        // A serialized GLOBSET structure.
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains IDSET_REPLID.</param>
        public void Parse(FastTransferStream stream)
        {
            this.REPLID = stream.ReadUInt16();
            this.GLOBSET = new GLOBSET();
            this.GLOBSET.Parse(stream);
        }
    }

    /// <summary>
    /// Represents a REPLGUID and GLOBSET structure pair. 
    /// </summary>
    public class IDSET_REPLGUID : BaseStructure
    {
        // A GUID that identifies a REPLGUID structure. 
        public Guid REPLGUID;

        // A serialized GLOBSET structure.
        public GLOBSET GLOBSET;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains IDSET_REPLGUID.</param>
        public void Parse(FastTransferStream stream)
        {
            this.REPLGUID = stream.ReadGuid();
            this.GLOBSET = new GLOBSET();
            this.GLOBSET.Parse(stream);
        }
    }
    #endregion

    #region 2.2.2.6 GLOBSET Structure
    /// <summary>
    /// Represents GLOBSET structure is a set of GLOBCNT structures, that are reduced to one or more GLOBCNT ranges. A GLOBCNT range is created using any of the commands  
    /// </summary>
    public class GLOBSET : BaseStructure
    {
        // Commands composed a GLOBCNT range, which indicates a GLOBSET structure.
        public Command[] Commands;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains GLOBSET.</param>
        public void Parse(FastTransferStream stream)
        {
            // A unsigned interger value indicates the bytes length in common stacks.
            uint CommonStackLength = 0;

            // A uint list indicats the pushed or poped count of bytes in common stack.
            List<uint> CommonStackCollection = new List<uint>();

            byte tmp = stream.ReadByte();
            stream.Position -= 1;

            List<Command> commands = new List<Command>();
            while (tmp != 0X00)
            {
                switch (tmp)
                {
                    case 0x01:
                    case 0x02:
                    case 0x03:
                    case 0x04:
                    case 0x05:
                    case 0x06:
                        Command PushCommand = new PushCommand();
                        PushCommand.Parse(stream);
                        commands.Add(PushCommand);
                        if ((CommonStackLength + (uint)(PushCommand as PushCommand).Command) < 6)
                        {
                            CommonStackCollection.Add((PushCommand as PushCommand).Command);
                            CommonStackLength += (uint)(PushCommand as PushCommand).Command;
                        }
                        break;
                    case 0x50:
                        Command PopCommand = new PopCommand();
                        PopCommand.Parse(stream);
                        commands.Add(PopCommand);
                        CommonStackLength -= CommonStackCollection[CommonStackCollection.Count - 1];
                        CommonStackCollection.RemoveAt(CommonStackCollection.Count - 1);
                        break;
                    case 0x42:
                        Command BitmaskCommand = new BitmaskCommand();
                        BitmaskCommand.Parse(stream);
                        commands.Add(BitmaskCommand);
                        break;
                    case 0x52:
                        Command RangeCommand = new RangeCommand(6 - CommonStackLength);
                        RangeCommand.Parse(stream);
                        commands.Add(RangeCommand);
                        break;
                    default:
                        break;
                }
                tmp = stream.ReadByte();
                stream.Position -= 1;
            }
            Command EndCommand = new EndCommand();
            EndCommand.Parse(stream);
            commands.Add(EndCommand);
            this.Commands = commands.ToArray();
        }
    }

    /// <summary>
    /// Represents a command in GLOBSET.
    /// </summary>
    public class Command : BaseStructure
    {
        /// <summary>
        /// Parse from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public virtual void Parse(FastTransferStream stream)
        {
        }
    }

    /// <summary>
    /// Represent a push command.
    /// </summary>
    public class PushCommand : Command
    {
        // An integer that specifies the number of high-order bytes that the GLOBCNT structures
        public byte Command;

        // A byte array that contains the bytes shared by the GLOBCNT structures
        public byte[] CommonBytes;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PushCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.CommonBytes = stream.ReadBlock(this.Command);
        }
    }

    /// <summary>
    /// Represent a pop command.
    /// </summary>
    public class PopCommand : Command
    {
        // Command.
        public byte Command;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PopCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
        }
    }

    /// <summary>
    /// Represent a bitmask command.
    /// </summary>
    public class BitmaskCommand : Command
    {
        // Bitmask Command.
        public byte Command;

        // The low-order byte of the low value of the first GLOBCNT range.
        public byte startValue;

        // One bit set for each value within a range, excluding the low value of the first GLOBCNT range.
        public byte bitmask;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains BitmaskCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.startValue = stream.ReadByte();
            this.bitmask = stream.ReadByte();
        }
    }

    /// <summary>
    /// Represent a range command.
    /// </summary>
    public class RangeCommand : Command
    {
        // Bitmask Command.
        public byte Command;

        // The low value of the range.
        public byte[] lowValue;

        // The high value of the range.
        public byte[] highValue;

        // the length of the LowValue and hignValue.
        private uint Length;

        /// <summary>
        /// Initializes a new instance of the RangeCommand structure.
        /// </summary>
        /// <param name="Length">The length of the LowValue and hignValue.</param>
        public RangeCommand(uint Length)
        {
            this.Length = Length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains RangeCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.lowValue = stream.ReadBlock((int)this.Length);
            this.highValue = stream.ReadBlock((int)this.Length);
        }
    }

    /// <summary>
    /// Represent an end command.
    /// </summary>
    public class EndCommand : Command
    {
        // Command.
        public byte Command;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains EndCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
        }
    }
    #endregion

    #region 2.2.2.7 ProgressInformation
    /// <summary>
    /// The ProgressInformation.
    /// </summary>
    public class ProgressInformation : BaseStructure
    {
        // An unsigned 16-bit value that contains a number that identifies the binary structure of the data that follows.
        public ushort Version;

        // The padding.
        public ushort Padding1;

        // An unsigned 32-bit integer value that contains the total number of changes to FAI messages that are scheduled for download during the current synchronization operation.
        public uint FAIMessageCount;

        // An unsigned 64-bit integer value that contains the size in bytes of all changes to FAI messages that are scheduled for download during the current synchronization operation.
        public ulong FAIMessageTotalSize;

        // An unsigned 32-bit integer value that contains the total number of changes to normal messages that are scheduled for download during the current synchronization operation.
        public uint NormalMessageCount;

        // The padding.
        public uint Padding2;

        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to normal messages  that are scheduled for download during the current synchronization operation.
        public ulong NormalMessageTotalSize;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains ProgressInformation.</param>
        public void Parse(FastTransferStream stream)
        {
            this.Version = stream.ReadUInt16();
            this.Padding1 = stream.ReadUInt16();
            this.FAIMessageCount = stream.ReadUInt32();
            this.FAIMessageTotalSize = stream.ReadUInt64();
            this.NormalMessageCount = stream.ReadUInt32();
            this.Padding2 = stream.ReadUInt32();
            this.NormalMessageTotalSize = stream.ReadUInt64();
        }
    }
    #endregion

    #region 2.2.2.8 PropertyGroupInfo
    public class PropertyGroupInfo : BaseStructure
    {
        // An unsigned 32-bit integer value that identifies a property mapping within the current synchronization download context.
        public uint GroupId;

        // Reserved.
        public uint Reserved;

        // An unsigned 32-bit integer value that specifies how many PropertyGroup structures are present in the Groups field. 
        public uint GroupCount;

        // An array of PropertyGroup structures,
        public PropertyGroup[] Groups;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PropertyGroupInfo.</param>
        public void Parse(FastTransferStream stream)
        {
            this.GroupId = stream.ReadUInt32();
            this.Reserved = stream.ReadUInt32();
            this.GroupCount = stream.ReadUInt32();
            this.Groups = new PropertyGroup[this.GroupCount];
            for (int i = 0; i < this.GroupCount; i++)
            {
                PropertyGroup tmpPropertyGroup = new PropertyGroup();
                tmpPropertyGroup.Parse(stream);
                Groups[i] = tmpPropertyGroup;
            }
        }
    }

    #region 2.2.2.8.1 PropertyGroup
    /// <summary>
    /// The PropertyGroup.
    /// </summary>
    public class PropertyGroup : BaseStructure
    {
        // An unsigned 32-bit integer value that specifies how many PropertyTag structures are present in the PropertyTags field. 
        public uint PropertyTagCount;

        // An array of PropertyTagWithGroupPropertyName structures.
        public PropertyTagWithGroupPropertyName[] PropertyTags;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains PropertyGroup.</param>
        public void Parse(FastTransferStream stream)
        {
            this.PropertyTagCount = stream.ReadUInt32();
            this.PropertyTags = new PropertyTagWithGroupPropertyName[this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTagWithGroupPropertyName tmpName = new PropertyTagWithGroupPropertyName();
                tmpName.Parse(stream);
                PropertyTags[i] = tmpName;
            }
        }
    }

    /// <summary>
    /// This structure is a PropertyTag Structure (MS-OXCDATA section 2.9) which is special for named properties 
    /// </summary>
    public class PropertyTagWithGroupPropertyName : BaseStructure
    {
        // An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        public PropertyDataType PropertyType;

        // An unsigned integer that identifies the property.
        public ushort PropertyId;

        // A GroupPropertyName structure.
        public GroupPropertyName groupPropertyName;

        /// <summary>
        /// Parse the PropertyTagWithGroupPropertyName structure.
        /// </summary>
        /// <param name="s">A stream containing the PropertyTagWithGroupPropertyName structure</param>
        public void Parse(FastTransferStream stream)
        {
            this.PropertyType = (PropertyDataType)stream.ReadUInt16();
            this.PropertyId = stream.ReadUInt16();
            if (this.PropertyId >= 0x8000)
            {
                this.groupPropertyName = new GroupPropertyName();
                this.groupPropertyName.Parse(stream);
            }
        }
    }
    #endregion

    #region 2.2.2.8.1.1 GroupPropertyName
    /// <summary>
    /// The GroupPropertyName.
    /// </summary>
    public class GroupPropertyName : BaseStructure
    {
        // The GUID that identifies the property set for the named property.
        public Guid Guid;

        // A value that identifies the type of property. 
        public uint Kind;

        // A value that identifies the named property within its property set. 
        public uint? Lid;

        // A value that specifies the length of the Name field, in bytes. 
        public uint? NameSize;

        // A Unicode (UTF-16) string that identifies the property within the property set. 
        public MAPIString Name;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains GroupPropertyName.</param>
        public void Parse(FastTransferStream stream)
        {
            this.Guid = stream.ReadGuid();
            this.Kind = stream.ReadUInt32();

            if (this.Kind == 0x00000000)
            {
                this.Lid = stream.ReadUInt32();
            }
            else if (this.Kind == 0x00000001)
            {
                this.NameSize = stream.ReadUInt32();
                this.Name = new MAPIString(Encoding.Unicode, "", (int)this.NameSize / 2);
                this.Name.Parse(stream);
            }
        }
    }
    #endregion
    #endregion

    #region 2.2.2.9 FolderReplicaInfo
    /// <summary>
    /// The FolderReplicaInfo structure contains information about server replicas of a public folder.
    /// </summary>
    public class FolderReplicaInfo : BaseStructure
    {
        // A uint value.
        public uint Flags;

        // A uint value.
        public uint Depth;

        // A LongTermID structure. Contains the LongTermID of a folder, for which server replica information is being described.
        public LongTermId FolderLongTermId;

        // An unsigned 32-bit integer value that determines how many elements exist in ServerDNArray. 
        public uint ServerDNCount;

        // An unsigned 32-bit integer value that determines how many of the leading elements in ServerDNArray have the same,lowest, network access cost.
        public uint CheapServerDNCount;

        // An array of ASCII-encoded NULL-terminated strings. 
        public MAPIString[] ServerDNArray;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains FolderReplicaInfo.</param>
        public void Parse(FastTransferStream stream)
        {
            this.Flags = stream.ReadUInt32();
            this.Depth = stream.ReadUInt32();
            this.FolderLongTermId = new LongTermId(stream);
            this.ServerDNCount = stream.ReadUInt32();
            this.CheapServerDNCount = stream.ReadUInt32();
            this.ServerDNArray = new MAPIString[this.ServerDNCount];

            for (int i = 0; i < this.ServerDNCount; i++)
            {
                this.ServerDNArray[i] = new MAPIString(Encoding.ASCII);
                this.ServerDNArray[i].Parse(stream);
            }
        }
    }
    #endregion

    #region 2.2.3.1.1.1 RopFastTransferSourceCopyProperties
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyProperties ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyPropertiesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies whether descendant subobjects are copied
        public byte Level;

        // A flags structure that contains flags that control the type of operation. 
        public CopyFlags_CopyProperties CopyFlags;

        // A flags structure that contains flags that control the behavior of the operation. 
        public SendOptions SendOptions;

        // An unsigned integer that specifies the number of structures in the PropertyTags field.
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the properties to exclude during the copy.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyPropertiesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.Level = ReadByte();
            this.CopyFlags = (CopyFlags_CopyProperties)ReadByte();
            this.SendOptions = (SendOptions)ReadByte();
            this.PropertyTagCount = ReadUshort();
            PropertyTag[] InterTag = new PropertyTag[(int)this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                InterTag[i] = new PropertyTag();
                InterTag[i].Parse(s);
            }
            this.PropertyTags = InterTag;
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyProperties ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyPropertiesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyPropertiesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.2 RopFastTransferSourceCopyTo
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyTo ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyToRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies whether descendant subobjects are copied
        public byte Level;

        // A flags structure that contains flags that control the type of operation. 
        public CopyFlags_CopyTo CopyFlags;

        // A flags structure that contains flags that control the behavior of the operation. 
        public SendOptions SendOptions;

        // An unsigned integer that specifies the number of structures in the PropertyTags field.
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the properties to exclude during the copy.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyToRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyToRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.Level = ReadByte();
            this.CopyFlags = (CopyFlags_CopyTo)ReadUint();
            this.SendOptions = (SendOptions)ReadByte();
            this.PropertyTagCount = ReadUshort();
            PropertyTag[] InterTag = new PropertyTag[(int)this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                InterTag[i] = new PropertyTag();
                InterTag[i].Parse(s);
            }
            this.PropertyTags = InterTag;
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyTo ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyToResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyToResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyToResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.3 RopFastTransferSourceCopyMessages
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyMessages ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyMessagesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the number of identifiers in the MessageIds field.
        public ushort MessageIdCount;

        // An array of 64-bit identifiers that specifies the messages to copy. 
        public MessageID[] MessageIds;

        // A flags structure that contains flags that control the type of operation. 
        public CopyFlags_CopyMessages CopyFlags;

        // A flags structure that contains flags that control the behavior of the operation. 
        public SendOptions SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyMessagesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.MessageIdCount = ReadUshort();

            List<MessageID> MessageIdList = new List<MessageID>();
            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID MessageId = new MessageID();
                MessageId.Parse(s);
                MessageIdList.Add(MessageId);
            }

            this.MessageIds = MessageIdList.ToArray();
            this.CopyFlags = (CopyFlags_CopyMessages)ReadByte();
            this.SendOptions = (SendOptions)ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyMessages ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyMessagesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyMessagesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.4 RopFastTransferSourceCopyFolder
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyFolder ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyFolderRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // A flags structure that contains flags that control the type of operation. 
        public CopyFlags_CopyFolder CopyFlags;

        // A flags structure that contains flags that control the behavior of the operation. 
        public SendOptions SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyFolderRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.CopyFlags = (CopyFlags_CopyFolder)ReadByte();
            this.SendOptions = (SendOptions)ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyFolder ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyFolderResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyFolderResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceCopyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.5 RopFastTransferSourceGetBuffer
    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceGetBufferRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the buffer size requested.
        public ushort BufferSize;

        // An unsigned integer that is present when the BufferSize field is set to 0xBABE.
        public ushort? MaximumBufferSize;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceGetBufferRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.BufferSize = ReadUshort();
            if (this.BufferSize == 0xBABE)
            {
                this.MaximumBufferSize = ReadUshort();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceGetBufferResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies the current status of the transfer. 
        public TransferStatus? TransferStatus;

        // An unsigned integer that specifies the number of steps that have been completed in the current operation.
        public ushort? InProgressCount;

        // An unsigned integer that specifies the approximate number of steps to be completed in the current operation.
        public ushort? TotalStepCount;

        // Reserved.
        public byte? Reserved;

        // An unsigned integer that specifies the size of the TransferBuffer field.
        public ushort? TransferBufferSize;

        // An array of bytes that specifies FastTransferStream.
        public SyntacticalBase TransferBuffer;

        // An unsigned integer that specifies the number of milliseconds for the client to wait before trying this operation again
        public uint? BackoffTime;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferSourceGetBufferResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TransferStatus = (TransferStatus)ReadUshort();
                this.InProgressCount = ReadUshort();
                this.TotalStepCount = ReadUshort();
                this.Reserved = ReadByte();
                this.TransferBufferSize = ReadUshort();
                byte[] Buffer = ReadBytes((int)this.TransferBufferSize);
                FastTransferStream TransferStream = new FastTransferStream(Buffer, true);

                switch (DecodingContext.StreamType_Getbuffer)
                {
                    case FastTransferStreamType.TopFolder:
                        this.TransferBuffer = new TopFolder(TransferStream);
                        break;
                    case FastTransferStreamType.contentsSync:
                        this.TransferBuffer = new ContentsSync(TransferStream);
                        break;
                    case FastTransferStreamType.hierarchySync:
                        this.TransferBuffer = new HierarchySync(TransferStream);
                        break;
                    case FastTransferStreamType.state:
                        this.TransferBuffer = new State(TransferStream);
                        break;
                    case FastTransferStreamType.folderContent:
                        this.TransferBuffer = new FolderContent(TransferStream);
                        break;
                    case FastTransferStreamType.MessageContent:
                        this.TransferBuffer = new MessageContent(TransferStream);
                        break;
                    case FastTransferStreamType.attachmentContent:
                        this.TransferBuffer = new AttachmentContent(TransferStream);
                        break;
                    case FastTransferStreamType.MessageList:
                        this.TransferBuffer = new MessageList(TransferStream);
                        break;
                    default:
                        throw new Exception("The transferStream type is not right");
                }
            }

            if ((AdditionalErrorCodes)ReturnValue == AdditionalErrorCodes.ServerBusy)
            {
                this.BackoffTime = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.3.1.1.6 RopTellVersion
    /// <summary>
    ///  A class indicates the RopTellVersion ROP Request Buffer.
    /// </summary>
    public class RopTellVersionRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An array of three unsigned 16-bit integers that contains the version information for the other server. 
        public byte[] Version;

        /// <summary>
        /// Parse the RopTellVersionRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopTellVersionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Version = ReadBytes(6);
        }
    }

    /// <summary>
    ///  A class indicates the RopTellVersion ROP Response Buffer.
    /// </summary>
    public class RopTellVersionResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopTellVersionResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopTellVersionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.2.1 RopFastTransferDestinationConfigure
    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Request Buffer.
    /// </summary>
    public class RopFastTransferDestinationConfigureRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An enumeration that indicates how the data stream was created on the source.
        public SourceOperation SourceOperation;

        // A flags structure that contains flags that control the behavior of the transfer operation.
        public CopyFlags_DestinationConfigure CopyFlags;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferDestinationConfigureRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.SourceOperation = (SourceOperation)ReadByte();
            this.CopyFlags = (CopyFlags_DestinationConfigure)ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Response Buffer.
    /// </summary>
    public class RopFastTransferDestinationConfigureResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferDestinationConfigureResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.2.2 RopFastTransferDestinationPutBuffer
    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBuffer ROP Request Buffer.
    /// </summary>
    public class RopFastTransferDestinationPutBufferRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the TransferData field. 
        public ushort TransferDataSize;

        // An array of bytes that contains the data to be uploaded to the destination fast transfer object.
        public SyntacticalBase TransferData;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferDestinationPutBufferRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.TransferDataSize = ReadUshort();

            byte[] Buffer = ReadBytes((int)this.TransferDataSize);
            FastTransferStream TransferStream = new FastTransferStream(Buffer, true);

            switch (DecodingContext.StreamType_Putbuffer)
            {
                case FastTransferStreamType.TopFolder:
                    this.TransferData = new TopFolder(TransferStream);
                    break;
                case FastTransferStreamType.folderContent:
                    this.TransferData = new FolderContent(TransferStream);
                    break;
                case FastTransferStreamType.MessageContent:
                    this.TransferData = new MessageContent(TransferStream);
                    break;
                case FastTransferStreamType.attachmentContent:
                    this.TransferData = new AttachmentContent(TransferStream);
                    break;
                case FastTransferStreamType.MessageList:
                    this.TransferData = new MessageList(TransferStream);
                    break;
                default:
                    throw new Exception("The transferStream type is not right");
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBuffer ROP Response Buffer.
    /// </summary>
    public class RopFastTransferDestinationPutBufferResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // the current status of the transfer.
        public ushort? TransferStatus;

        // An unsigned integer that specifies the number of steps that have been completed in the current operation.
        public ushort? InProgressCount;

        // An unsigned integer that specifies the approximate total number of steps to be completed in the current operation.
        public ushort? TotalStepCount;

        // Reserved.
        public byte? Reserved;

        // An unsigned integer that specifies the buffer size that was used.
        public ushort? BufferUsedSize;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFastTransferDestinationPutBufferResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TransferStatus = ReadUshort();
                this.InProgressCount = ReadUshort();
                this.TotalStepCount = ReadUshort();
                this.Reserved = ReadByte();
                this.BufferUsedSize = ReadUshort();
            }
        }
    }
    #endregion

    #region 2.2.3.2.1.1 RopSynchronizationConfigure
    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationConfigureRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An enumeration that controls the type of synchronization.
        public SynchronizationType SynchronizationType;

        // A flags structure that contains flags that control the behavior of the operation. 
        public SendOptions SendOptions;

        // A flags structure that contains flags that control the behavior of the synchronization.
        public SynchronizationFlags SynchronizationFlags;

        // An unsigned integer that specifies the length, in bytes, of the RestrictionData field.
        public ushort RestrictionDataSize;

        // A restriction packet,that specifies the filter for this synchronization object.
        public RestrictionType RestrictionData;

        // A flags structure that contains flags control the additional behavior of the synchronization. 
        public SynchronizationExtraFlags SynchronizationExtraFlags;

        // An unsigned integer that specifies the number of structures in the PropertyTags field.
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the properties to exclude during the copy.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSynchronizationConfigureRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationConfigureRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.SynchronizationType = (SynchronizationType)ReadByte();
            this.SendOptions = (SendOptions)ReadByte();
            this.SynchronizationFlags = (SynchronizationFlags)ReadUshort();
            this.RestrictionDataSize = ReadUshort();

            if (RestrictionDataSize > 0)
            {
                this.RestrictionData = new RestrictionType();
                this.RestrictionData.Parse(s);
            }
            this.SynchronizationExtraFlags = (SynchronizationExtraFlags)ReadUint();
            this.PropertyTagCount = ReadUshort();
            PropertyTag[] InterTag = new PropertyTag[(int)this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                InterTag[i] = new PropertyTag();
                InterTag[i].Parse(s);
            }
            this.PropertyTags = InterTag;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationConfigureResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationConfigureResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationConfigureResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.2.1 RopSynchronizationUploadStateStreamBegin
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamBegin ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamBeginRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A PropertyTag structure.
        public uint StateProperty;

        // An unsigned integer that specifies the size of the stream to be uploaded.
        public uint TransferBufferSize;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamBeginRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationUploadStateStreamBeginRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.StateProperty = ReadUint();
            this.TransferBufferSize = ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamBegin ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamBeginResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamBeginResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationUploadStateStreamBeginResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.2.2 RopSynchronizationUploadStateStreamContinue
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamContinue ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size, in bytes, of the StreamData field.
        public uint StreamDataSize;

        // An array of bytes that contains the state stream data to be uploaded.
        public byte[] StreamData;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamContinueRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationUploadStateStreamContinueRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.StreamDataSize = ReadUint();
            this.StreamData = ReadBytes((int)this.StreamDataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamContinue ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamContinueResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationUploadStateStreamContinueResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.2.3 RopSynchronizationUploadStateStreamEnd
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamEnd ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamEndRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamEndRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationUploadStateStreamEndRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamEnd ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamEndResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamEndResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationUploadStateStreamEndResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.3.1 RopSynchronizationGetTransferState
    /// <summary>
    ///  A class indicates the RopSynchronizationGetTransferState ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationGetTransferStateRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        /// <summary>
        /// Parse the RopSynchronizationGetTransferStateRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationGetTransferStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationGetTransferState ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationGetTransferStateResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationGetTransferStateResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationGetTransferStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.4.1 RopSynchronizationOpenCollector
    /// <summary>
    ///  A class indicates the RopSynchronizationOpenCollector ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationOpenCollectorRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // A Boolean that specifies whether this synchronization upload context is for contents or for hierarchy.
        public bool IsContentsCollector;
        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationOpenCollectorRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.IsContentsCollector = ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationOpenCollector ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationOpenCollectorResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationOpenCollectorResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.4.2 RopSynchronizationImportMessageChange
    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageChange ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageChangeRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // A flags structure that contains flags that control the behavior of the synchronization.
        public ImportFlag ImportFlag;

        // An unsigned integer that specifies the number of structures present in the PropertyValues field.
        public ushort PropertyValueCount;

        // An array of TaggedPropertyValue structures that specify extra properties on the message.
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageChangeRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportMessageChangeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.ImportFlag = (ImportFlag)ReadByte();
            this.PropertyValueCount = ReadUshort();
            TaggedPropertyValue[] InterValue = new TaggedPropertyValue[(int)this.PropertyValueCount];
            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                InterValue[i] = new TaggedPropertyValue();
                InterValue[i].Parse(s);
            }
            this.PropertyValues = InterValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageChange ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageChangeResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageChangeResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportMessageChangeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.3.2.4.3 RopSynchronizationImportHierarchyChange
    /// <summary>
    ///  A class indicates the RopSynchronizationImportHierarchyChange ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportHierarchyChangeRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of structures present in the HierarchyValues field.
        public ushort HierarchyValueCount;

        // An array of TaggedPropertyValue structures that specify hierarchy-related properties of the folder.
        public TaggedPropertyValue[] HierarchyValues;

        // An unsigned integer that specifies the number of structures present in the PropertyValues field.
        public ushort PropertyValueCount;

        // An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportHierarchyChangeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.HierarchyValueCount = ReadUshort();
            TaggedPropertyValue[] InterHierarchyValues = new TaggedPropertyValue[(int)this.HierarchyValueCount];
            for (int i = 0; i < this.HierarchyValueCount; i++)
            {
                InterHierarchyValues[i] = new TaggedPropertyValue();
                InterHierarchyValues[i].Parse(s);
            }
            this.HierarchyValues = InterHierarchyValues;

            this.PropertyValueCount = ReadUshort();
            TaggedPropertyValue[] InterValue = new TaggedPropertyValue[(int)this.PropertyValueCount];
            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                InterValue[i] = new TaggedPropertyValue();
                InterValue[i].Parse(s);
            }
            this.PropertyValues = InterValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportHierarchyChange ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportHierarchyChangeResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier.
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportHierarchyChangeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.3.2.4.4 RopSynchronizationImportMessageMove
    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageMove ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageMoveRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the SourceFolderId field.
        public uint SourceFolderIdSize;

        // An array of bytes that identifies the parent folder of the source message.
        public byte[] SourceFolderId;

        // An unsigned integer that specifies the size of the SourceMessageId field.
        public uint SourceMessageIdSize;

        // An array of bytes that identifies the source message.
        public byte[] SourceMessageId;

        // An unsigned integer that specifies the size of the PredecessorChangeList field.
        public uint PredecessorChangeListSize;

        // An array of bytes. The size of this field, in bytes, is specified by the PredecessorChangeListSize field.
        public byte[] PredecessorChangeList;

        // An unsigned integer that specifies the size of the DestinationMessageId field.
        public uint DestinationMessageIdSize;

        // An array of bytes that identifies the destination message. 
        public byte[] DestinationMessageId;

        // An unsigned integer that specifies the size of the ChangeNumber field.
        public uint ChangeNumberSize;

        // An array of bytes that specifies the change number of the message. 
        public byte[] ChangeNumber;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportMessageMoveRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.SourceFolderIdSize = ReadUint();
            this.SourceFolderId = ReadBytes((int)this.SourceFolderIdSize);
            this.SourceMessageIdSize = ReadUint();
            this.SourceMessageId = ReadBytes((int)this.SourceMessageIdSize);
            this.PredecessorChangeListSize = ReadUint();
            this.PredecessorChangeList = ReadBytes((int)this.PredecessorChangeListSize);
            this.DestinationMessageIdSize = ReadUint();
            this.DestinationMessageId = ReadBytes((int)this.DestinationMessageIdSize);
            this.ChangeNumberSize = ReadUint();
            this.ChangeNumber = ReadBytes((int)this.ChangeNumberSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageMove ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageMoveResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An identifier.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportMessageMoveResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.3.2.4.5 RopSynchronizationImportDeletes
    /// <summary>
    ///  A class indicates the RopSynchronizationImportDeletes ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportDeletesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure that contains flags that specify options for the imported deletions.
        public ImportDeleteFlags ImportDeleteFlags;

        // An unsigned integer that specifies the number of structures present in the PropertyValues field.
        public ushort PropertyValueCount;

        // An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportDeletesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportDeletesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ImportDeleteFlags = (ImportDeleteFlags)ReadByte();
            this.PropertyValueCount = ReadUshort();
            TaggedPropertyValue[] InterValue = new TaggedPropertyValue[(int)this.PropertyValueCount];
            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                InterValue[i] = new TaggedPropertyValue();
                InterValue[i].Parse(s);
            }
            this.PropertyValues = InterValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportDeletes ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportDeletesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationImportDeletesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportDeletesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.4.6 RopSynchronizationImportReadStateChanges
    /// <summary>
    ///  A class indicates the RopSynchronizationImportReadStateChanges ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportReadStateChangesRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size, in bytes, of the MessageReadStates field.
        public ushort MessageReadStatesSize;

        // A list of MessageReadState structures that specify the messages and associated read states to be changed.
        public MessageReadState[] MessageReadStates;

        /// <summary>
        /// Parse the RopSynchronizationImportReadStateChangesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportReadStateChangesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MessageReadStatesSize = ReadUshort();
            List<MessageReadState> InterValue = new List<MessageReadState>();
            int size = this.MessageReadStatesSize;
            while (size > 0)
            {
                MessageReadState InterValueI = new MessageReadState();
                InterValueI.Parse(s);
                InterValue.Add(InterValueI);
                size -= (InterValueI.MessageId.Length + 1 + 2);
            }
            this.MessageReadStates = InterValue.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportReadStateChanges ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportReadStateChangesResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationImportReadStateChangesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSynchronizationImportReadStateChangesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }

    /// <summary>
    ///  A class indicates the MessageReadState structure.
    /// </summary>
    public class MessageReadState : BaseStructure
    {
        // An unsigned integer that specifies the size of the MessageId field.
        public ushort MessageIdSize;

        // An array of bytes that identifies the message to be marked as read or unread.
        public byte[] MessageId;

        // A Boolean that specifies whether to mark the message as read or not.
        public bool MarkAsRead;

        /// <summary>
        /// Parse the MessageReadState structure.
        /// </summary>
        /// <param name="s">An stream containing MessageReadState structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.MessageIdSize = ReadUshort();
            this.MessageId = ReadBytes(this.MessageIdSize);
            this.MarkAsRead = ReadBoolean();
        }
    }
    #endregion

    #region 2.2.3.2.4.7 RopGetLocalReplicaIds
    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Request Buffer.
    /// </summary>
    public class RopGetLocalReplicaIdsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of IDs to reserve.
        public uint IdCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetLocalReplicaIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.IdCount = ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Response Buffer.
    /// </summary>
    public class RopGetLocalReplicaIdsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // This field contains the replica GUID that is shared by the IDs.
        public Guid? ReplGuid;

        // An array of bytes that specifies the first value in the reserved range.
        public byte?[] GlobalCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetLocalReplicaIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.ReplGuid = ReadGuid();
                this.GlobalCount = ConvertArray(ReadBytes(6));
            }
        }
    }
    #endregion

    #region 2.2.3.2.4.8 RopSetLocalReplicaMidsetDeleted
    /// <summary>
    ///  A class indicates the RopSetLocalReplicaMidsetDeleted ROP Request Buffer.
    /// </summary>
    public class RopSetLocalReplicaMidsetDeletedRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of both the LongTermIdRangeCount and LongTermIdRanges fields.
        public ushort DataSize;

        // An unsigned integer that specifies the number of structures in the LongTermIdRanges field.
        public uint LongTermIdRangeCount;

        // An array of LongTermIdRange structures that specify the ranges of message identifiers that have been deleted.
        public LongTermIdRange[] LongTermIdRanges;

        /// <summary>
        /// Parse the RopSetLocalReplicaMidsetDeletedRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetLocalReplicaMidsetDeletedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.DataSize = ReadUshort();
            this.LongTermIdRangeCount = ReadUint();

            LongTermIdRange[] interRangs = new LongTermIdRange[this.LongTermIdRangeCount];
            for (int i = 0; i < interRangs.Length; i++)
            {
                interRangs[i] = new LongTermIdRange();
                interRangs[i].Parse(s);
            }
            this.LongTermIdRanges = interRangs;
        }
    }

    /// <summary>
    ///  A class indicates the RopSetLocalReplicaMidsetDeleted ROP Response Buffer.
    /// </summary>
    public class RopSetLocalReplicaMidsetDeletedResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetLocalReplicaMidsetDeletedResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetLocalReplicaMidsetDeletedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }

    /// <summary>
    ///  A class indicates the LongTermIdRange structure.
    /// </summary>
    public class LongTermIdRange : BaseStructure
    {
        // A LongTermId structure that specifies the beginning of a range. 
        public LongTermID MinLongTermId;

        // A LongTermId structure that specifies the end of a range.
        public LongTermID MaxLongTermId;

        /// <summary>
        /// Parse the LongTermIdRange structure.
        /// </summary>
        /// <param name="s">An stream containing LongTermIdRange structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.MinLongTermId = new LongTermID();
            this.MinLongTermId.Parse(s);
            this.MaxLongTermId = new LongTermID();
            this.MaxLongTermId.Parse(s);
        }
    }
    #endregion

    #region 2.2.4 FastTransfer Stream
    /// <summary>
    /// Used for Parsing a fast transfer stream.
    /// </summary>
    public class FastTransferStream : MemoryStream
    {
        // The length of a GUID structure.
        public static int GuidLength = Guid.Empty.ToByteArray().Length;

        // The length of a MetaTag property.
        private const int MetaLength = 4;

        /// <summary>
        /// Initializes a new instance of the FastTransferStream class.
        /// </summary>
        /// <param name="buffer">A bytes array.</param>
        /// <param name="writable">Whether the stream supports writing.</param>
        public FastTransferStream(byte[] buffer, bool writable)
            : base(buffer, 0, buffer.Length, writable, true)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the stream position is at the end of this stream
        /// </summary>
        public bool IsEndOfStream
        {
            get
            {
                return this.Position == this.Length;
            }
        }

        /// <summary>
        /// Read a Markers value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The Markers value</returns>
        public Markers ReadMarker()
        {
            byte[] buffer = new byte[MetaLength];
            this.Read(buffer, 0, MetaLength);
            uint marker;
            marker = BitConverter.ToUInt32(buffer, 0);
            return (Markers)marker;
        }

        /// <summary>
        /// Read a byte value from stream and advance the position within the stream by 1
        /// </summary>
        /// <returns>A byte</returns>
        public new byte ReadByte()
        {
            int value = base.ReadByte();
            if (value == -1)
            {
                throw new Exception();
            }
            return (byte)value;
        }

        /// <summary>
        /// Read a uint value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The uint value.</returns>
        public uint ReadUInt32()
        {
            byte[] buffer = new byte[4];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Read an int value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The int value.</returns>
        public int ReadInt32()
        {
            byte[] buffer = new byte[4];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Read a unsigned short integer value from stream,and advance the position within the stream by 2
        /// </summary>
        /// <returns>The unsigned short integer value</returns>
        public ushort ReadUInt16()
        {
            byte[] buffer = new byte[2];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Read a short value from stream,and advance the position within the stream by 2
        /// </summary>
        /// <returns>The short value</returns>
        public short ReadInt16()
        {
            byte[] buffer = new byte[2];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Read a long value from stream,and advance the position within the stream by 8
        /// </summary>
        /// <returns>The long value</returns>
        public long ReadInt64()
        {
            byte[] buffer = new byte[8];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// Read the unsigned long integer value from stream, and advance the position within the stream by 8
        /// </summary>
        /// <returns>The unsigned long integer value</returns>
        public ulong ReadUInt64()
        {
            byte[] buffer = new byte[8];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Read a float value from stream, and advance the position within the stream by 4
        /// </summary>
        /// <returns>The float value</returns>
        public float ReadFloating32()
        {
            byte[] buffer = new byte[4];
            this.Read(buffer, 0, MetaLength);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <summary>
        /// Read a double value from stream,and advance the position within the stream by 8
        /// </summary>
        /// <returns>The double value</returns>
        public double ReadFloating64()
        {
            byte[] buffer = new byte[8];
            this.Read(buffer, 0, buffer.Length);
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <summary>
        /// Read a currency value from stream,and advance the position within the stream by 8
        /// </summary>
        /// <returns>The long value represents a currency value</returns>
        public long ReadCurrency()
        {
            return this.ReadInt64();
        }

        /// <summary>
        /// Read a FloatingTime value from stream, and advance the position within the stream by 8
        /// </summary>
        /// <returns>The double value represents a FloatingTime value</returns>
        public double ReadFloatingTime()
        {
            return this.ReadFloating64();
        }

        /// <summary>
        /// Read a Boolean value from stream, and advance the position within the stream by 2
        /// </summary>
        /// <returns>The unsigned short integer value represents a Boolean value</returns>
        public ushort ReadBoolean()
        {
            return this.ReadUInt16();
        }

        /// <summary>
        /// Read a Time value from stream,and advance the position within the stream by 2
        /// </summary>
        /// <returns>The unsigned long integer value represents a Time value</returns>
        public ulong ReadTime()
        {
            return this.ReadUInt64();
        }

        /// <summary>
        /// Read a GUID value from stream, and advance the position.
        /// </summary>
        /// <returns>The GUID value</returns>
        public Guid ReadGuid()
        {
            byte[] buffer = new byte[Guid.Empty.ToByteArray().Length];
            this.Read(buffer, 0, buffer.Length);
            return new Guid(buffer);
        }

        /// <summary>
        /// Read  bytes from stream, and advance the position.
        /// </summary>
        /// <param name="size">The size of bytes</param>
        /// <returns>The bytes array</returns>
        public byte[] ReadBlock(int size)
        {
            byte[] buffer = new byte[size];
            this.Read(buffer, 0, size);
            return buffer;
        }

        /// <summary>
        /// Read a list of blocks and advance the position.
        /// </summary>
        /// <param name="totalSize">The total number of bytes to read</param>
        /// <param name="blockSize">The size of each block</param>
        /// <returns>A list of blocks</returns>
        public byte[][] ReadBlocks(int totalSize, int blockSize)
        {
            int i;
            List<byte[]> l = new List<byte[]>();
            for (i = 0; i < totalSize; i += blockSize)
            {
                l.Add(this.ReadBlock(blockSize));
            }

            return l.ToArray();
        }

        /// <summary>
        /// Read LengthOfBlock and advance the position.
        /// </summary>
        /// <returns>A LengthOfBlock specifies the length of the bytes array</returns>
        public LengthOfBlock ReadLengthBlock()
        {
            int tmp = this.ReadInt32();
            byte[] buffer = this.ReadBlock(tmp);
            return new LengthOfBlock(tmp, buffer);
        }

        /// <summary>
        /// Read a list of LengthOfBlock and advance the position.
        /// </summary>
        /// <param name="totalLength">The number of bytes to read</param>
        /// <returns>A list of LengthOfBlock</returns>
        public LengthOfBlock[] ReadLengthBlocks(int totalLength)
        {
            int i = 0;
            List<LengthOfBlock> list = new List<LengthOfBlock>();
            while (i < totalLength)
            {
                LengthOfBlock tmp = this.ReadLengthBlock();
                i++;
                list.Add(tmp);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Get a uint value and do not advance the position.
        /// </summary>
        /// <returns>A uint value </returns>
        public uint VerifyUInt32()
        {
            return BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position);
        }

        /// <summary>
        /// Get an unsigned short integer value for current position plus an offset and does not advance the position.
        /// </summary>
        /// <returns>An unsigned short integer value</returns>
        public ushort VerifyUInt16()
        {
            return BitConverter.ToUInt16(
                this.GetBuffer(),
                (int)this.Position);
        }

        /// <summary>
        /// Get an unsigned short integer value for current position plus an offset and do not advance the position.
        /// </summary>
        /// <param name="offset">An int value</param>
        /// <returns>An unsigned short integer value</returns>
        public ushort VerifyUInt16(int offset)
        {
            return BitConverter.ToUInt16(
                this.GetBuffer(),
                (int)this.Position + offset);
        }

        /// <summary>
        /// Indicate the Markers at the position equals a specified Markers.
        /// </summary>
        /// <param name="marker">A Markers value</param>
        /// <returns>True if the Markers at the position equals to the specified Markers, else false.</returns>
        public bool VerifyMarker(Markers marker)
        {
            return this.Verify((uint)marker);
        }

        /// <summary>
        /// Indicate the Markers at the current position plus an offsetequals a specified Markers
        /// </summary>
        /// <param name="marker">A Markers to be verified</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the Markers at the current position plus an offset equals a specified Markers, else false.</returns>
        public bool VerifyMarker(Markers marker, int offset)
        {
            return this.Verify((uint)marker, offset);
        }

        /// <summary>
        /// Indicate the MetaProperties at the position equals a specified MetaProperties
        /// </summary>
        /// <param name="meta">A MetaProperties value</param>
        /// <returns>True if the MetaProperties at the position equals the specified MetaProperties, else false.</returns>
        public bool VerifyMetaProperty(MetaProperties meta)
        {
            return !this.IsEndOfStream && this.Verify((uint)meta, 0);
        }

        /// <summary>
        /// Indicate the uint value at the position equals a specified uint value.
        /// </summary>
        /// <param name="val">A uint value.</param>
        /// <returns>True if the uint at the position equals the specified uint.else false.</returns>
        public bool Verify(uint val)
        {
            return !this.IsEndOfStream && BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position) == val;
        }

        /// <summary>
        /// Indicate the uint value at the position plus an offset equals a specified uint value.
        /// </summary>
        /// <param name="val">A uint value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the uint at the position plus an offset equals the specified uint,else false.</returns>
        public bool Verify(uint val, int offset)
        {
            return !this.IsEndOfStream && BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position + offset) == val;
        }

        /// <summary>
        /// Indicate the byte value at the position plus an offset equals a specified byte
        /// </summary>
        /// <param name="val">A uint value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the byte at the position plus an offset equals the specified byte, else false.</returns>
        public bool Verify(byte val, int offset)
        {
            byte[] tmp = this.GetBuffer();
            return !this.IsEndOfStream && tmp[(int)this.Position + offset] == val;
        }
    }
    #endregion

    # region 2.2.4.1 FastTransfer stream lexical structure
    /// <summary>
    /// Base class for lexical objects
    /// </summary>
    public abstract class LexicalBase
    {
        /// <summary>
        /// Initializes a new instance of the LexicalBase class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        protected LexicalBase(FastTransferStream stream)
        {
            this.Parse(stream);
        }

        /// <summary>
        /// Parse from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public virtual void Parse(FastTransferStream stream)
        {
        }
    }

    /// <summary>
    /// The PropValue represents identification information and the value of the property.
    /// </summary>
    public class PropValue : LexicalBase
    {
        // The propType.
        public ushort PropType;

        // The PropInfo.
        public PropInfo PropInfo;

        /// <summary>
        /// Initializes a new instance of the PropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public PropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Indicate whether the stream's position is IsMetaTagIdsetGiven.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>True if the stream's position is IsMetaTagIdsetGiven,else false.</returns>
        public static bool IsMetaTagIdsetGiven(FastTransferStream stream)
        {
            ushort type = stream.VerifyUInt16();
            ushort id = stream.VerifyUInt16(2);
            return type == (ushort)PropertyDataType.PtypInteger32 && id == 0x4017;
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized PropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized PropValue, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream
                && (FixedPropTypePropValue.Verify(stream) || VarPropTypePropValue.Verify(stream) || MvPropTypePropValue.Verify(stream))
                && !MarkersHelper.IsMarker(stream.VerifyUInt32())
                && !MarkersHelper.IsMetaTag(stream.VerifyUInt32());
        }

        /// <summary>
        /// Parse a PropValue instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A PropValue instance.</returns>
        public static LexicalBase ParseFrom(FastTransferStream stream)
        {
            if (FixedPropTypePropValue.Verify(stream))
            {
                return FixedPropTypePropValue.ParseFrom(stream);
            }
            else if (VarPropTypePropValue.Verify(stream))
            {
                return VarPropTypePropValue.ParseFrom(stream);
            }
            else if (MvPropTypePropValue.Verify(stream))
            {
                return MvPropTypePropValue.ParseFrom(stream);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.PropType = stream.ReadUInt16();
            PropInfo = PropInfo.ParseFrom(stream) as PropInfo;
        }
    }

    /// <summary>
    /// The PropInfo class.
    /// </summary>
    public class PropInfo : LexicalBase
    {
        // The property id.
        public ushort PropID;

        // The namedPropInfo in lexical definition.
        public NamedPropInfo NamedPropInfo;

        /// <summary>
        /// Initializes a new instance of the PropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        protected PropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized PropInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized PropInfo, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream;
        }

        /// <summary>
        /// Parse a PropInfo instance from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A PropInfo instance.</returns>
        public static LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new PropInfo(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.PropID = stream.ReadUInt16();

            if (this.PropID >= 0x8000)
            {
                this.NamedPropInfo = NamedPropInfo.ParseFrom(stream) as NamedPropInfo;
            }
        }
    }

    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValue : PropValue
    {
        // A fixed value.
        public object FixedValue;

        /// <summary>
        /// Initializes a new instance of the FixedPropTypePropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public FixedPropTypePropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized FixedPropTypePropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized FixedPropTypePropValue, return true, else false</returns>
        public static new bool Verify(FastTransferStream stream)
        {
            ushort tmp = stream.VerifyUInt16();
            return LexicalTypeHelper.IsFixedType((PropertyDataType)tmp)
                && !PropValue.IsMetaTagIdsetGiven(stream);
        }

        /// <summary>
        /// Parse a DispidNamedPropInfo instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A DispidNamedPropInfo instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new FixedPropTypePropValue(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            PropertyDataType type = (PropertyDataType)this.PropType;

            switch (type)
            {
                case PropertyDataType.PtypInteger16:
                    this.FixedValue = stream.ReadInt16();
                    break;
                case PropertyDataType.PtypInteger32:
                    if (this.PropInfo.PropID == 0x67A4)
                    {
                        CN tmpCN = new CN();
                        tmpCN.Parse(stream);
                        this.FixedValue = tmpCN;
                    }
                    this.FixedValue = stream.ReadInt32();
                    break;
                case PropertyDataType.PtypFloating32:
                    this.FixedValue = stream.ReadFloating32();
                    break;
                case PropertyDataType.PtypFloating64:
                    this.FixedValue = stream.ReadFloating64();
                    break;
                case PropertyDataType.PtypCurrency:
                    this.FixedValue = stream.ReadCurrency();
                    break;
                case PropertyDataType.PtypFloatingTime:
                    this.FixedValue = stream.ReadFloatingTime();
                    break;
                case PropertyDataType.PtypBoolean:
                    this.FixedValue = stream.ReadBoolean();
                    break;
                case PropertyDataType.PtypInteger64:
                    if (base.PropInfo.PropID == 0x6714)
                    {
                        CN tmpCN = new CN();
                        tmpCN.Parse(stream);
                        this.FixedValue = tmpCN;
                    }
                    this.FixedValue = stream.ReadInt64();
                    break;
                case PropertyDataType.PtypTime:
                    this.FixedValue = stream.ReadTime();
                    break;
                case PropertyDataType.PtypGuid:
                    this.FixedValue = stream.ReadGuid();
                    break;
            }
        }
    }

    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValue : PropValue
    {
        // The length of a variate type value.
        public int Length;

        // The valueArray.
        public object ValueArray;

        /// <summary>
        /// Initializes a new instance of the VarPropTypePropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized VarPropTypePropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized VarPropTypePropValue, return true, else false</returns>
        public static new bool Verify(FastTransferStream stream)
        {
            ushort tmp = stream.VerifyUInt16();
            return LexicalTypeHelper.IsVarType((PropertyDataType)tmp)
                || PropValue.IsMetaTagIdsetGiven(stream)
                || LexicalTypeHelper.IsCodePageType(tmp);
        }

        /// <summary>
        /// Parse a VarPropTypePropValue instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A VarPropTypePropValue instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new VarPropTypePropValue(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.Length = stream.ReadInt32();

            if (LexicalTypeHelper.IsCodePageType(this.PropType))
            {
                CodePageType type = (CodePageType)this.PropType;
                switch (type)
                {
                    case CodePageType.PtypCodePageUnicode:
                        PtypString pstring = new PtypString();
                        pstring.Parse(stream);
                        this.ValueArray = pstring;
                        break;
                    case CodePageType.PtypCodePageUnicodeBigendian:
                    case CodePageType.PtypCodePageWesternEuropean:
                        PtypString8 pstring8 = new PtypString8();
                        pstring8.Parse(stream);
                        this.ValueArray = pstring8;
                        break;
                    default:
                        PtypString8 defaultstring8 = new PtypString8();
                        defaultstring8.Parse(stream);
                        break;
                }
            }
            else
            {
                PropertyDataType type = (PropertyDataType)this.PropType;
                switch (type)
                {
                    case PropertyDataType.PtypInteger32:
                    case PropertyDataType.PtypBinary:
                        // PidTagParentSourceKey, PidTagParentSourceKey, PidTagChangeKey
                        if (this.PropInfo.PropID == 0x65E0 || this.PropInfo.PropID == 0x65E1 || this.PropInfo.PropID == 0x65E2)
                        {
                            if (this.Length != 0)
                            {
                                XID tmpXID = new XID(this.Length);
                                tmpXID.Parse(stream);
                                this.ValueArray = tmpXID;
                            }
                        }
                        else if (this.PropInfo.PropID == 0x65E3) // PidTagPredecessorChangeList 
                        {
                            PredecessorChangeList tmpPredecessorChangeList = new PredecessorChangeList(this.Length);
                            tmpPredecessorChangeList.Parse(stream);
                            this.ValueArray = tmpPredecessorChangeList;
                        }
                        else if (this.PropInfo.PropID == 0x402D || this.PropInfo.PropID == 0x402E || this.PropInfo.PropID == 0x67E5 || this.PropInfo.PropID == 0x4021 || this.PropInfo.PropID == 0x6793)
                        {
                            if (this.Length != 0)
                            {
                                int begionPosition = (int)stream.Position;
                                int EveLength = this.Length;
                                List<IDSET_REPLID> InterIDSET_REPLID = new List<IDSET_REPLID>();
                                while (EveLength > 0)
                                {
                                    IDSET_REPLID tmpIDSET_REPLID = new IDSET_REPLID();
                                    tmpIDSET_REPLID.Parse(stream);
                                    InterIDSET_REPLID.Add(tmpIDSET_REPLID);
                                    EveLength -= ((int)stream.Position - begionPosition);
                                }
                                this.ValueArray = InterIDSET_REPLID.ToArray();
                            }
                        }
                        else if (this.PropInfo.PropID == 0x4017 || this.PropInfo.PropID == 0x6796 || this.PropInfo.PropID == 0x67DA || this.PropInfo.PropID == 0x67D2)
                        {
                            if (this.Length != 0)
                            {
                                int begionPosition = (int)stream.Position;
                                int EveLength = this.Length;
                                List<IDSET_REPLGUID> InterIDSET_REPLGUID = new List<IDSET_REPLGUID>();
                                while (EveLength > 0)
                                {
                                    IDSET_REPLGUID tmpIDSET_REPLGUID = new IDSET_REPLGUID();
                                    tmpIDSET_REPLGUID.Parse(stream);
                                    InterIDSET_REPLGUID.Add(tmpIDSET_REPLGUID);
                                    EveLength -= ((int)stream.Position - begionPosition);
                                }
                                this.ValueArray = InterIDSET_REPLGUID.ToArray();
                            }
                        }
                        else
                        {
                            this.ValueArray = stream.ReadBlock(this.Length);
                        }
                        break;
                    case PropertyDataType.PtypString:
                        PtypString pstring = new PtypString();
                        pstring.Parse(stream);
                        this.ValueArray = pstring;
                        break;
                    case PropertyDataType.PtypString8:
                        PtypString8 pstring8 = new PtypString8();
                        pstring8.Parse(stream);
                        this.ValueArray = pstring8;
                        break;
                    case PropertyDataType.PtypServerId:
                        PtypServerId pserverId = new PtypServerId();
                        // PtypServerId in MSOXCFXICS does not contain Length element
                        stream.Position -= 4;
                        pserverId.Parse(stream);
                        this.ValueArray = pserverId;
                        break;
                    case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                        this.ValueArray = stream.ReadBlock(this.Length);
                        break;
                    default:
                        this.ValueArray = stream.ReadBlock(this.Length);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValue : PropValue
    {
        // This represent the length variable.
        public int Length;

        // A list of fixed size values.
        public byte[][] FixedSizeValueList;

        // A list of LengthOfBlock.
        public LengthOfBlock[] VarSizeValueList;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MvPropTypePropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>I the stream's current position contains a serialized MvPropTypePropValue, return true, else false</returns>
        public static new bool Verify(FastTransferStream stream)
        {
            ushort tmp = stream.VerifyUInt16();
            return LexicalTypeHelper.IsMVType((PropertyDataType)tmp) && !PropValue.IsMetaTagIdsetGiven(stream);
        }

        /// <summary>
        /// Parse a MvPropTypePropValue instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>A MvPropTypePropValue instance </returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new MvPropTypePropValue(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            PropertyDataType type = (PropertyDataType)this.PropType;
            this.Length=stream.ReadInt32();
            switch (type)
            {
                case PropertyDataType.PtypMultipleInteger16:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 2);
                    break;
                case PropertyDataType.PtypMultipleInteger32:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 4);
                    break;
                case PropertyDataType.PtypMultipleFloating32:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 4);
                    break;
                case PropertyDataType.PtypMultipleFloating64:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleCurrency:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleFloatingTime:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleInteger64:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleTime:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, 8);
                    break;
                case PropertyDataType.PtypMultipleGuid:
                    this.FixedSizeValueList = stream.ReadBlocks(this.Length, Guid.Empty.ToByteArray().Length);
                    break;
                case PropertyDataType.PtypMultipleBinary:
                    this.VarSizeValueList = stream.ReadLengthBlocks(this.Length);
                    break;
                case PropertyDataType.PtypMultipleString:
                    this.VarSizeValueList = stream.ReadLengthBlocks(this.Length);
                    break;
                case PropertyDataType.PtypMultipleString8:
                    this.VarSizeValueList = stream.ReadLengthBlocks(this.Length);
                    break;
            }
        }
    }

    /// <summary>
    /// The NamedPropInfo class.
    /// </summary>
    public class NamedPropInfo : LexicalBase
    {
        // The propertySet item in lexical definition.
        public Guid propertySet;

        // The flag variable.
        public byte flag;

        /// <summary>
        /// Initializes a new instance of the NamedPropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public NamedPropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse a NamedPropInfo instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A NamedPropInfo instance.</returns>
        public static LexicalBase ParseFrom(FastTransferStream stream)
        {
            if (DispidNamedPropInfo.Verify(stream))
            {
                return new DispidNamedPropInfo(stream);
            }
            else if (NameNamedPropInfo.Verify(stream))
            {
                return new NameNamedPropInfo(stream);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            byte[] buffer = new byte[Guid.Empty.ToByteArray().Length];
            stream.Read(buffer, 0, buffer.Length);
            this.propertySet = new Guid(buffer);
            int tmp = stream.ReadByte();
            if (tmp > 0)
            {
                this.flag = (byte)tmp;
            }
        }
    }

    /// <summary>
    /// Represents a NamedPropInfo has a dispid.
    /// </summary>
    public class DispidNamedPropInfo : NamedPropInfo
    {
        // The dispid in lexical definition.
        public int Dispid;

        /// <summary>
        /// Initializes a new instance of the DispidNamedPropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public DispidNamedPropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized DispidNamedPropInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>If the stream's current position contains a serialized DispidNamedPropInfo, return true, else false</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.Verify(0x00, Guid.Empty.ToByteArray().Length);
        }

        /// <summary>
        /// Parse a DispidNamedPropInfo instance from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A DispidNamedPropInfo instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new DispidNamedPropInfo(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.Dispid = stream.ReadInt32();
        }
    }

    /// <summary>
    /// The NameNamedPropInfo class.
    /// </summary>
    public class NameNamedPropInfo : NamedPropInfo
    {
        // The name of the NamedPropInfo.
        public MAPIString Name;

        /// <summary>
        /// Initializes a new instance of the NameNamedPropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public NameNamedPropInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized NameNamedPropInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>If the stream's current position contains a serialized NameNamedPropInfo, return true, else false</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.Verify(0x01, Guid.Empty.ToByteArray().Length);
        }

        /// <summary>
        /// Parse a NameNamedPropInfo instance from a FastTransferStream
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>A NameNamedPropInfo instance.</returns>
        public static new LexicalBase ParseFrom(FastTransferStream stream)
        {
            return new NameNamedPropInfo(stream);
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            this.Name = new MAPIString(Encoding.Unicode);
            this.Name.Parse(stream);
        }
    }
    #endregion

    # region 2.2.4.2 FastTransfer stream syntactical structure
    /// <summary>
    /// Base class for all syntactical object.
    /// </summary>
    public abstract class SyntacticalBase
    {
        // The size of an MetaTag value.
        protected const int MetaLength = 4;

        // Previous position.
        private long PreviousPosition;

        /// <summary>
        /// Initializes a new instance of the SyntacticalBase class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        protected SyntacticalBase(FastTransferStream stream)
        {
            this.PreviousPosition = stream.Position;
            if (stream != null && stream.Length > 0)
            {
                this.Parse(stream);
            }
        }

        /// <summary>
        /// Parse object from memory stream,
        /// </summary>
        /// <param name="stream">Stream contains the serialized object</param>
        public abstract void Parse(FastTransferStream stream);
    }

    /// <summary>
    /// Contains a list of propValues.
    /// </summary>
    public class PropList : SyntacticalBase
    {
        // A list of PropValue objects.
        public PropValue[] PropValues;

        /// <summary>
        /// Initializes a new instance of the PropList class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public PropList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized propList.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized propList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return PropValue.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<PropValue> PropValuesList = new List<PropValue>();
            while (PropValue.Verify(stream))
            {
                PropValuesList.Add(PropValue.ParseFrom(stream) as PropValue);
            }
            this.PropValues = PropValuesList.ToArray();
        }
    }

    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Metaproperty.
    /// </summary>
    public class MetaPropValue : SyntacticalBase
    {
        // The property type.
        public ushort PropType;

        // The property id.
        public ushort PropID;

        // The property value.
        public object PropValue;

        /// <summary>
        /// Initializes a new instance of the MetaPropValue class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValue(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaPropValue.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MetaPropValue, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            ushort tmpType = stream.VerifyUInt16();
            ushort tmpId = stream.VerifyUInt16();
            return !stream.IsEndOfStream && LexicalTypeHelper.IsMetaPropertyID(tmpId);
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropType = stream.ReadUInt16();
            this.PropID = stream.ReadUInt16();
            if (PropID != 0x4011 && PropID != 0x4008)
            {
                this.PropValue = stream.ReadUInt32();
            }
            else
            {
                if (PropID != 0x4011)
                {
                    FolderReplicaInfo FolderReplicaInfo = new FolderReplicaInfo();
                    FolderReplicaInfo.Parse(stream);
                    this.PropValue = FolderReplicaInfo;
                }
                else
                {
                    PtypString8 pstring8 = new PtypString8();
                    pstring8.Parse(stream);
                    this.PropValue = pstring8;
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TopFolder : SyntacticalBase
    {
        // The start marker of TopFolder.
        public Markers StartMarker;

        // A FolderContentNoDelProps value contains the content of a folder: its properties, messages, and subfolders.
        public FolderContentNoDelProps FolderContentNoDelProps;

        // The end marker of TopFolder.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the TopFolder class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TopFolder(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify a stream's current position contains a serialized TopFolder.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized TopFolder, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartTopFld);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartTopFld)
            {
                this.StartMarker = Markers.StartTopFld;
                this.FolderContentNoDelProps = new FolderContentNoDelProps(stream);
                if (stream.ReadMarker() == Markers.EndFolder)
                {
                    this.EndMarker = Markers.EndFolder;
                }
            }
        }
    }

    /// <summary>
    /// The folderContent element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContent : SyntacticalBase
    {
        // Contains the properties of the Folder object, which are possibly affected by property filters.
        public PropList PropList;

        // A MetaTagNewFXFolder property.
        public MetaPropValue MetaTagNewFXFolder;

        // The folderMessages element contains the messages contained in a folder.
        public FolderMessages FolderMessages;

        // A MetaTagFXDelProp property.
        public MetaPropValue MetaTagFXDelProp;

        // The subFolders element contains subFolders of a folder.
        public SubFolder[] SubFolders;

        /// <summary>
        /// Initializes a new instance of the FolderContent class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderContent(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized folderContent.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized folderContent, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return PropList.Verify(stream);
        }

        /// <summary>
        ///  Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropList = new PropList(stream);
            if (!stream.IsEndOfStream)
            {
                List<SubFolder> InterSubFolders = new List<SubFolder>();
                if (stream.VerifyMetaProperty(MetaProperties.MetaTagNewFXFolder))
                {
                    this.MetaTagNewFXFolder = new MetaPropValue(stream);
                }
                else
                {
                    this.FolderMessages = new FolderMessages(stream);
                }

                if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
                {
                    this.MetaTagFXDelProp = new MetaPropValue(stream);
                }

                if (!stream.IsEndOfStream)
                {
                    while (SubFolder.Verify(stream))
                    {
                        InterSubFolders.Add(new SubFolder(stream));
                    }
                    this.SubFolders = InterSubFolders.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// The folderContentNoDelProps element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContentNoDelProps : SyntacticalBase
    {
        // Contains the properties of the Folder object, which are possibly affected by property filters.
        public PropList PropList;

        // A MetaTagNewFXFolder property.
        public MetaPropValue MetaTagNewFXFolder;

        // The FolderMessagesNoDelProps element contains the messages contained in a folder.
        public FolderMessagesNoDelProps FolderMessagesNoDelProps;

        // A MetaTagFXDelProp property.
        public MetaPropValue MetaTagFXDelProp;

        // The subFolders element contains subFolders of a folder.
        public SubFolderNoDelProps[] SubFolderNoDelPropList;

        /// <summary>
        /// Initializes a new instance of the folderContentNoDelProps class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderContentNoDelProps(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized folderContentNoDelProps.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized folderContentNoDelProps, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return PropList.Verify(stream);
        }

        /// <summary>
        ///  Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropList = new PropList(stream);
            if (!stream.IsEndOfStream)
            {
                List<SubFolderNoDelProps> InterSubFolders = new List<SubFolderNoDelProps>();
                if (stream.VerifyMetaProperty(MetaProperties.MetaTagNewFXFolder))
                {
                    this.MetaTagNewFXFolder = new MetaPropValue(stream);
                }
                else
                {
                    this.FolderMessagesNoDelProps = new FolderMessagesNoDelProps(stream);
                }

                if (!stream.IsEndOfStream)
                {
                    while (SubFolderNoDelProps.Verify(stream))
                    {
                        InterSubFolders.Add(new SubFolderNoDelProps(stream));
                    }
                    this.SubFolderNoDelPropList = InterSubFolders.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class SubFolder : SyntacticalBase
    {
        // The start marker of SubFolder.
        public Markers StartMarker;

        // A folderContent value contains the content of a folder: its properties, messages, and subfolders.
        public FolderContent FolderContent;

        // The end marker of SubFolder.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the SubFolder class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public SubFolder(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized SubFolder.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized SubFolder, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartSubFld);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartSubFld)
            {
                this.StartMarker = Markers.StartSubFld;
                this.FolderContent = new FolderContent(stream);
                if (stream.ReadMarker() == Markers.EndFolder)
                {
                    this.EndMarker = Markers.EndFolder;
                }
                else
                {
                    throw new Exception("The SubFolder cannot be parsed successfully. The EndFolder Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContentNoDelProps.
    /// </summary>
    public class SubFolderNoDelProps : SyntacticalBase
    {
        // The start marker of SubFolder.
        public Markers StartMarker;

        // A folderContentNoDelProps value contains the content of a folder: its properties, messages, and subfolders.
        public FolderContentNoDelProps folderContentNoDelProps;

        // The end marker of SubFolder.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the SubFolderNoDelProps class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public SubFolderNoDelProps(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized SubFolderNoDelProps.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized SubFolderNoDelProps, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartSubFld);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartSubFld)
            {
                this.StartMarker = Markers.StartSubFld;
                this.folderContentNoDelProps = new FolderContentNoDelProps(stream);
                if (stream.ReadMarker() == Markers.EndFolder)
                {
                    this.EndMarker = Markers.EndFolder;
                }
                else
                {
                    throw new Exception("The SubFolderNoDelProps cannot be parsed successfully. The EndFolder Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// The folderMessages element contains the messages contained in a folder.
    /// </summary>
    public class FolderMessages : SyntacticalBase
    {
        // A list of MetaTagFxDelPropMessageList.
        public MetaTagFxDelPropMessageList[] MetaTagFxDelPropMessageLists;

        /// <summary>
        /// Initializes a new instance of the FolderMessages class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderMessages(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized folderMessages
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized folderMessages, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && MetaTagFxDelPropMessageList.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            int count = 0;
            List<MetaTagFxDelPropMessageList> InterMessageLists = new List<MetaTagFxDelPropMessageList>();
            while (!stream.IsEndOfStream && count < 2)
            {
                if (MetaTagFxDelPropMessageList.Verify(stream))
                {
                    InterMessageLists.Add(new MetaTagFxDelPropMessageList(stream));
                }
                else
                {
                    break;
                }
                count++;
            }
            this.MetaTagFxDelPropMessageLists = InterMessageLists.ToArray();
        }
    }

    /// <summary>
    /// The MetaTagFxDelPropMessageList is defined to help Parsering folderMessages class.
    /// </summary>
    public class MetaTagFxDelPropMessageList : SyntacticalBase
    {
        // A MetaTagFXDelProp property. 
        public MetaPropValue MetaTagFXDelProp;

        // A list of messageList.
        public MessageList MessageLists;

        /// <summary>
        /// Initializes a new instance of the FolderMessages class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaTagFxDelPropMessageList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaTagFxDelPropMessageList
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MetaTagFxDelPropMessageList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream && stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.MetaTagFXDelProp = new MetaPropValue(stream);
            this.MessageLists = new MessageList(stream);
        }
    }

    /// <summary>
    /// The FolderMessagesNoDelProps element contains the messages contained in a folder.
    /// </summary>
    public class FolderMessagesNoDelProps : SyntacticalBase
    {
        // A list of MessageList.
        public MessageList[] MessageLists;

        /// <summary>
        /// Initializes a new instance of the FolderMessagesNoDelProps class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public FolderMessagesNoDelProps(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized FolderMessagesNoDelProps
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized FolderMessagesNoDelProps, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream
                && MessageList.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            int count = 0;
            List<MessageList> InterMessageLists = new List<MessageList>();
            while (!stream.IsEndOfStream && count < 2)
            {
                if (MessageList.Verify(stream))
                {
                    InterMessageLists.Add(new MessageList(stream));
                }
                else
                {
                    break;
                }
                count++;
            }
            this.MessageLists = InterMessageLists.ToArray();

        }
    }

    /// <summary>
    /// The message element represents a Message object.
    /// </summary>
    public class Message : SyntacticalBase
    {
        // The start marker of message.
        public Markers? StartMarker1;

        // The start marker of message.
        public Markers? StartMarker2;

        // A MessageContent value.Represents the content of a message: its properties, the recipients, and the attachments.
        public MessageContent Content;

        // The end marker of message.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the Message class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public Message(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized message.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized message, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartMessage) ||
                stream.VerifyMarker(Markers.StartFAIMsg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            Markers marker = stream.ReadMarker();
            if (marker == Markers.StartMessage || marker == Markers.StartFAIMsg)
            {
                if (marker == Markers.StartMessage)
                { this.StartMarker1 = Markers.StartMessage; }
                else
                { this.StartMarker2 = Markers.StartFAIMsg; }

                this.Content = new MessageContent(stream);
                if (stream.ReadMarker() == Markers.EndMessage)
                {
                    this.EndMarker = Markers.EndMessage;
                }
                else
                {
                    throw new Exception("The Message cannot be parsed successfully. The EndMessage Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// The MessageContent element represents the content of a message: its properties, the recipients, and the attachments.
    /// </summary>
    public class MessageContent : SyntacticalBase
    {
        // A propList value.
        public PropList PropList;

        // Represents children of the Message objects: Recipient and Attachment objects.
        public MessageChildren MessageChildren;

        /// <summary>
        /// Initializes a new instance of the MessageContent class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MessageContent(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageContent.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageContent, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return PropList.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropList = new PropList(stream);
            this.MessageChildren = new MessageChildren(stream);
        }
    }

    /// <summary>
    /// The MessageChildren element represents children of the Message objects: Recipient and Attachment objects.
    /// </summary>
    public class MessageChildren : SyntacticalBase
    {
        // A MetaTagFXDelProp property.
        public MetaPropValue FxdelPropsBeforeRecipient;

        // A list of recipients.
        public Recipient[] Recipients;

        // Another MetaTagFXDelProp property.
        public MetaPropValue FxdelPropsBeforeAttachment;

        // A list of attachments.
        public Attachment[] Attachments;

        /// <summary>
        /// Initializes a new instance of the MessageChildren class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MessageChildren(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<Attachment> InterAttachments = new List<Attachment>();
            List<Recipient> InterRecipients = new List<Recipient>();
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
            {
                this.FxdelPropsBeforeRecipient = new MetaPropValue(stream);
            }

            if (Recipient.Verify(stream))
            {
                InterRecipients = new List<Recipient>();
                while (Recipient.Verify(stream))
                {
                    InterRecipients.Add(new Recipient(stream));
                }
            }

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
            {
                this.FxdelPropsBeforeAttachment = new MetaPropValue(stream);
            }

            while (Attachment.Verify(stream))
            {
                InterAttachments.Add(new Attachment(stream));
            }

            this.Attachments = InterAttachments.ToArray();
            this.Recipients = InterRecipients.ToArray();
        }
    }

    /// <summary>
    /// The Recipient element represents a Recipient object, which is a subobject of the Message object.
    /// </summary>
    public class Recipient : SyntacticalBase
    {
        // The start marker of Recipient.
        public Markers StartMarker;

        // A propList value.
        public PropList PropList;

        // The end marker of Recipient.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the Recipient class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public Recipient(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized Recipient.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized Recipient, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartRecip);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartRecip)
            {
                this.StartMarker = Markers.StartRecip;
                this.PropList = new PropList(stream);
                if (stream.ReadMarker() == Markers.EndToRecip)
                {
                    this.EndMarker = Markers.EndToRecip;
                }
                else
                {
                    throw new Exception("The Recipient cannot be parsed successfully. The EndToRecip Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// Contains an attachmentContent.
    /// </summary>
    public class Attachment : SyntacticalBase
    {
        // The  start marker of an attachment object.
        public Markers StartMarker;

        // A PidTagAttachNumber property.
        public FixedPropTypePropValue PidTagAttachNumber;

        // Attachment content.
        public AttachmentContent AttachmentContent;

        // The end marker of an attachment object.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the Attachment class.
        /// </summary>
        /// <param name="stream">a FastTransferStream</param>
        public Attachment(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized attachment.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized attachment, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.NewAttach);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.NewAttach)
            {
                this.StartMarker = Markers.NewAttach;
                this.PidTagAttachNumber = new FixedPropTypePropValue(stream);
                this.AttachmentContent = new AttachmentContent(stream);
                if (stream.ReadMarker() == Markers.EndAttach)
                {
                    this.EndMarker = Markers.EndAttach;
                }
                else
                {
                    throw new Exception("The Attachment cannot be parsed successfully. The EndAttach Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// The attachmentContent element contains the properties and the embedded message of an Attachment object. If present,
    /// </summary>
    public class AttachmentContent : SyntacticalBase
    {
        // A propList value.
        public PropList PropList;

        // An EmbeddedMessage value.
        public EmbeddedMessage EmbeddedMessage;

        /// <summary>
        /// Initializes a new instance of the AttachmentContent class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public AttachmentContent(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized attachmentContent.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized attachmentContent, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return PropList.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.PropList = new PropList(stream);
            if (EmbeddedMessage.Verify(stream))
            {
                this.EmbeddedMessage = new EmbeddedMessage(stream);
            }
        }
    }

    /// <summary>
    /// Contain a MessageContent.
    /// </summary>
    public class EmbeddedMessage : SyntacticalBase
    {
        // The start marker of the EmbeddedMessage.
        public Markers StartMarker;

        // A MessageContent value represents the content of a message: its properties, the recipients, and the attachments.
        public MessageContent MessageContent;

        // The end marker of the EmbeddedMessage.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the EmbeddedMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public EmbeddedMessage(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized EmbeddedMessage.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized EmbeddedMessage, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.StartEmbed);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.StartEmbed)
            {
                this.StartMarker = Markers.NewAttach;
                this.MessageContent = new MessageContent(stream);
                if (stream.ReadMarker() == Markers.EndEmbed)
                {
                    this.EndMarker = Markers.EndEmbed;
                }
                else
                {
                    throw new Exception("The EmbeddedMessage cannot be parsed successfully. The EndEmbed Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// The MessageList element contains a list of messages, which is determined by the scope of the operation.
    /// </summary>
    public class MessageList : SyntacticalBase
    {
        // A list of MetaTagEcWaringMessage objects.
        public MetaTagEcWaringMessage[] MetaTagEcWaringMessages;

        /// <summary>
        /// Initializes a new instance of the MessageList class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MessageList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageList.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return MetaTagEcWaringMessage.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<MetaTagEcWaringMessage> InterMessageList = new List<MetaTagEcWaringMessage>();

            while (Verify(stream))
            {
                InterMessageList.Add(new MetaTagEcWaringMessage(stream));
            }

            this.MetaTagEcWaringMessages = InterMessageList.ToArray();
        }
    }

    /// <summary>
    /// The MetaTagEcWaringMessage is defined to help Parsering MessageList class.
    /// </summary>
    public class MetaTagEcWaringMessage : SyntacticalBase
    {
        // MetaTagEcWaring indicates a MetaTagEcWaring property.
        public MetaPropValue MetaTagEcWaring;

        // Message indicates a Message object.
        public Message Message;

        /// <summary>
        /// Initializes a new instance of the MetaTagEcWaringMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MetaTagEcWaringMessage(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MetaTagEcWaringMessage.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MetaTagEcWaringMessage, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return !stream.IsEndOfStream
                && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagEcWarning)
                || Message.Verify(stream);
        }

        /// <summary>
        /// Parse MetaTagEcWaringMessage from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagEcWarning))
            {
                this.MetaTagEcWaring = new MetaPropValue(stream);
            }

            if (Message.Verify(stream))
            {
                this.Message = new Message(stream);
            }
        }
    }

    /// <summary>
    /// The Deletions element contains information of messages that have been deleted expired or moved out of the sync scope.
    /// </summary>
    public class Deletions : SyntacticalBase
    {
        // The start marker of Deletions.
        public Markers StartMarker;

        // A propList value.
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the Deletions class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public Deletions(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized Deletions.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized Deletions, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncDel);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncDel)
            {
                this.StartMarker = Markers.IncrSyncDel;
                this.PropList = new PropList(stream);
            }
        }
    }

    /// <summary>
    /// The FolderChange element contains a new or changed folder in the hierarchy sync.
    /// </summary>
    public class FolderChange : SyntacticalBase
    {
        // The start marker of FolderChange.
        public Markers StartMarker;

        // A propList value.
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the FolderChange class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public FolderChange(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized FolderChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized FolderChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncChg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncChg)
            {
                this.StartMarker = Markers.IncrSyncChg;
                this.PropList = new PropList(stream);
            }
        }
    }

    /// <summary>
    /// The GroupInfo element provides a definition for the property group mapping.
    /// </summary>
    public class GroupInfo : SyntacticalBase
    {
        // The start marker of GroupInfo.
        public Markers StartMarker;

        // The propertyTag for ProgressInformation.
        public uint propertiesTag;

        // The count of the PropList.
        public uint propertiesLength;

        // A propList value.
        public PropertyGroupInfo PropList;

        /// <summary>
        /// Initializes a new instance of the GroupInfo class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public GroupInfo(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized GroupInfo.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized GroupInfo, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncGroupInfo);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncGroupInfo)
            {
                this.StartMarker = Markers.IncrSyncGroupInfo;
                this.propertiesTag = stream.ReadUInt32();
                this.propertiesLength = stream.ReadUInt32();
                PropertyGroupInfo tmpGroupInfo = new PropertyGroupInfo();
                tmpGroupInfo.Parse(stream);
                this.PropList = tmpGroupInfo;
            }
        }
    }

    /// <summary>
    /// The ProgressPerMessage element contains data that describes the approximate size of message change data that follows.
    /// </summary>
    public class ProgressPerMessage : SyntacticalBase
    {
        // The start marker of ProgressPerMessage.
        public Markers StartMarker;

        // A propList value.
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the ProgressPerMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ProgressPerMessage(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized ProgressPerMessage.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized ProgressPerMessage, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncProgressPerMsg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncProgressPerMsg)
            {
                this.StartMarker = Markers.IncrSyncProgressPerMsg;
                this.PropList = new PropList(stream);
            }
        }
    }

    /// <summary>
    /// The progressTotal element contains data that describes the approximate size of all the messageChange elements.
    /// </summary>
    public class ProgressTotal : SyntacticalBase
    {
        // The start marker of progressTotal.
        public Markers StartMarker;

        // The propertyTag for ProgressInformation.
        public uint propertiesTag;

        // The count of the PropList.
        public uint propertiesLength;

        // A propList value.
        public ProgressInformation PropList;

        /// <summary>
        /// Initializes a new instance of the progressTotal class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ProgressTotal(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized progressTotal.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized progressTotal, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncProgressMode);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncProgressMode)
            {
                this.StartMarker = Markers.IncrSyncProgressMode;
                this.propertiesTag = stream.ReadUInt32();
                this.propertiesLength = stream.ReadUInt32();
                ProgressInformation tmpProgressInfo = new ProgressInformation();
                tmpProgressInfo.Parse(stream);
                this.PropList = tmpProgressInfo;
            }
        }
    }

    /// <summary>
    /// The readStateChanges element contains information of Message objects that had their read state changed
    /// </summary>
    public class ReadStateChanges : SyntacticalBase
    {
        // The start marker of ReadStateChange.
        public Markers StartMarker;

        // A propList value.
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the ReadStateChange class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ReadStateChanges(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized ReadStateChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized ReadStateChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncRead);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncRead)
            {
                this.StartMarker = Markers.IncrSyncRead;
                this.PropList = new PropList(stream);
            }
        }
    }

    /// <summary>
    /// The state element contains the final ICS state of the synchronization download operation. 
    /// </summary>
    public class State : SyntacticalBase
    {
        // The start marker of ReadStateChange.
        public Markers StartMarker;

        // A propList value.
        public PropList PropList;

        // The end marker of ReadStateChange.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the State class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public State(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized State.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized State, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncStateBegin);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncStateBegin)
            {
                this.StartMarker = Markers.IncrSyncStateBegin;
                this.PropList = new PropList(stream);
                if (stream.ReadMarker() == Markers.IncrSyncStateEnd)
                {
                    this.EndMarker = Markers.IncrSyncStateEnd;
                }
                else
                {
                    throw new Exception("The State cannot be parsed successfully. The IncrSyncStateEnd Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// The ContentsSync element contains the result of the contents synchronization download operation.
    /// </summary>
    public class ContentsSync : SyntacticalBase
    {
        // A ProgressTotal value
        public ProgressTotal ProgressTotal;

        // A list of ProgressPerMessageChange value
        public ProgressPerMessageChange[] ProgressPerMessageChanges;

        // A Deletions value
        public Deletions Deletions;

        // A readStateChanges value.
        public ReadStateChanges ReadStateChanges;

        // A state value.
        public State State;

        // A end marker of ContentSync.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the ContentsSync class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public ContentsSync(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized contentsSync.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized contentsSync, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return (ProgressTotal.Verify(stream)
                || ProgressPerMessageChange.Verify(stream)
                || Deletions.Verify(stream)
                || ReadStateChanges.Verify(stream)
                || State.Verify(stream))
                && stream.VerifyMarker(Markers.IncrSyncEnd, (int)stream.Length - 4 - (int)stream.Position); ;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<ProgressPerMessageChange> InterProgressPerMessageChanges = new List<ProgressPerMessageChange>();
            if (ProgressTotal.Verify(stream))
            {
                this.ProgressTotal = new ProgressTotal(stream);
            }

            while (ProgressPerMessageChange.Verify(stream))
            {
                InterProgressPerMessageChanges.Add(new ProgressPerMessageChange(stream));
            }
            this.ProgressPerMessageChanges = InterProgressPerMessageChanges.ToArray();

            if (Deletions.Verify(stream))
            {
                this.Deletions = new Deletions(stream);
            }

            if (ReadStateChanges.Verify(stream))
            {
                this.ReadStateChanges = new ReadStateChanges(stream);
            }

            this.State = new State(stream);
            if (stream.ReadMarker() == Markers.IncrSyncEnd)
            {
                this.EndMarker = Markers.IncrSyncEnd;
            }
            else
            {
               throw new Exception("The ContentsSync cannot be parsed successfully. The IncrSyncEnd Marker is missed.");
            }
        }
    }

    /// <summary>
    /// The ProgressPerMessageChange is defined to help Parsering ContentSync class.
    /// </summary>
    public class ProgressPerMessageChange : SyntacticalBase
    {
        // A ProgressPerMessage value.
        public ProgressPerMessage ProgressPerMessage;

        // A MessageChange value.
        public MessageChange MessageChange;

        /// <summary>
        /// Initializes a new instance of the ProgressPerMessageChange class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ProgressPerMessageChange(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized ProgressPerMessageChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized ProgressPerMessageChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return ProgressPerMessage.Verify(stream) || MessageChange.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (ProgressPerMessage.Verify(stream))
            {
                this.ProgressPerMessage = new ProgressPerMessage(stream);
            }

            this.MessageChange = new MessageChange(stream);
        }
    }

    /// <summary>
    /// The hierarchySync element contains the result of the hierarchy synchronization download operation.
    /// </summary>
    public class HierarchySync : SyntacticalBase
    {
        // A list of FolderChange value.
        public FolderChange[] FolderChanges;

        // A Deletions value.
        public Deletions Deletions;

        // The State value.
        public State State;

        // The end marker of hierarchySync.
        public Markers EndMarker;

        /// <summary>
        /// Initializes a new instance of the HierarchySync class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public HierarchySync(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized hierarchySync.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized hierarchySync, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return (FolderChange.Verify(stream)
                || Deletions.Verify(stream)
                || State.Verify(stream))
                && stream.VerifyMarker(Markers.IncrSyncEnd, (int)stream.Length - 4 - (int)stream.Position);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<FolderChange> InterFolderChanges = new List<FolderChange>();
            while (FolderChange.Verify(stream))
            {
                InterFolderChanges.Add(new FolderChange(stream));
            }
            this.FolderChanges = InterFolderChanges.ToArray();

            if (Deletions.Verify(stream))
            {
                this.Deletions = new Deletions(stream);
            }

            this.State = new State(stream);
            if (stream.ReadMarker() == Markers.IncrSyncEnd)
            {
                this.EndMarker = Markers.IncrSyncEnd;
            }
            else
            {
                throw new Exception("The HierarchySync cannot be parsed successfully. The IncrSyncEnd Marker is missed.");
            }
        }
    }

    /// <summary>
    /// The Messagechange element contains information for the changed messages.
    /// </summary>
    public class MessageChange : SyntacticalBase
    {
        // A MessageChangeFull value.
        public MessageChangeFull MessageChangeFull;

        // A MessageChangePartial value.
        public MessageChangePartial MesageChangePartial;

        /// <summary>
        /// Initializes a new instance of the MessageChange class.
        /// </summary>
        /// <param name="stream">A FastTransferStream object.</param>
        public MessageChange(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageChange.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageChange, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return MessageChangeFull.Verify(stream) || MessageChangePartial.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MessageChangeFull.Verify(stream))
            {
                this.MessageChangeFull = new MessageChangeFull(stream);
            }
            else
            {
                this.MesageChangePartial = new MessageChangePartial(stream);
            }
        }
    }

    /// <summary>
    /// The messageChangeFull element contains the complete content of a new or changed message: the message properties, the recipients,and the attachments.
    /// </summary>
    public class MessageChangeFull : SyntacticalBase
    {
        // A start marker for MessageChangeFull.
        public Markers StartMarker;

        // A messageChangeHeader value.
        public PropList messageChangeHeader;

        // A second marker for MessageChangeFull.
        public Markers SecondMarker;

        // A propList value.
        public PropList propList;

        // A MessageChildren value.
        public MessageChildren messageChildren;

        /// <summary>
        /// Initializes a new instance of the MessageChangeFull class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MessageChangeFull(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized messageChangeFull.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains 
        /// a serialized messageChangeFull, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyMarker(Markers.IncrSyncChg);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.ReadMarker() == Markers.IncrSyncChg)
            {
                this.StartMarker = Markers.IncrSyncChg;
                this.messageChangeHeader = new PropList(stream);

                if (stream.ReadMarker() == Markers.IncrSyncMessage)
                {
                    this.SecondMarker = Markers.IncrSyncMessage;
                    this.propList = new PropList(stream);
                    this.messageChildren = new MessageChildren(stream);
                }
                else
                {
                    throw new Exception("The MessageChangeFull cannot be parsed successfully. The IncrSyncMessage Marker is missed.");
                }
            }
        }
    }

    /// <summary>
    /// The MessageChangePartial element represents the difference in message content since the last download, as identified by the initial ICS state.
    /// </summary>
    public class MessageChangePartial : SyntacticalBase
    {
        // A groupInfo value.
        public GroupInfo groupInfo;

        // A MetaTagIncrSyncGroupId property.
        public MetaPropValue MetaTagIncrSyncGroupId;

        // the MessageChangePartial marker.
        public Markers Marker;

        // A messageChangeHeader value.
        public PropList messageChangeHeader;

        // A list of SyncMessagePartialPropList values.
        public SyncMessagePartialPropList[] SyncMessagePartialPropList;

        // A MessageChildren field.
        public MessageChildren MessageChildren;

        /// <summary>
        /// Initializes a new instance of the MessageChangePartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream object.</param>
        public MessageChangePartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized MessageChangePartial.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized MessageChangePartial, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return GroupInfo.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<SyncMessagePartialPropList> InterMessagePartialList = new List<SyncMessagePartialPropList>();
            this.groupInfo = new GroupInfo(stream);
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrSyncGroupId))
            {
                this.MetaTagIncrSyncGroupId = new MetaPropValue(stream);
            }

            if (stream.ReadMarker() == Markers.IncrSyncChgPartial)
            {
                this.Marker = Markers.IncrSyncChgPartial;
                this.messageChangeHeader = new PropList(stream);

                while (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrementalSyncMessagePartial))
                {
                    InterMessagePartialList.Add(new SyncMessagePartialPropList(stream));
                }
                this.SyncMessagePartialPropList = InterMessagePartialList.ToArray();
                this.MessageChildren = new MessageChildren(stream);
            }
            else
            {
                throw new Exception("The MessageChangePartial cannot be parsed successfully. The IncrSyncChgPartial Marker is missed.");
            }
        }
    }

    /// <summary>
    /// The SyncMessagePartialPropList is defined to help Parsering MessageChangePartial element.
    /// </summary>
    public class SyncMessagePartialPropList : SyntacticalBase
    {
        // A MetaTagIncrementalSyncMessagePartial property.
        public MetaPropValue Meta_SyncMessagePartial;

        // A PropList value.
        PropList PropList;

        /// <summary>
        /// Initializes a new instance of the SyncMessagePartialPropList class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public SyncMessagePartialPropList(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Verify that a stream's current position contains a serialized SyncMessagePartialPropList.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        /// <returns>If the stream's current position contains a serialized SyncMessagePartialPropList, return true, else false.</returns>
        public static bool Verify(FastTransferStream stream)
        {
            return stream.VerifyUInt32() == (uint)MetaProperties.MetaTagIncrementalSyncMessagePartial;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrementalSyncMessagePartial))
            {
                this.Meta_SyncMessagePartial = new MetaPropValue(stream);
            }
            this.PropList = new PropList(stream);
        }
    }
    # endregion

    #region FastTransfer help
    /// <summary>
    /// Supply help functions for manipulate Markers.
    /// </summary>
    public class MarkersHelper
    {
        /// <summary>
        /// Indicate whether a uint is a Marker.
        /// </summary>
        /// <param name="marker">The uints value.</param>
        /// <returns>If is a Marker, return true, else false.</returns>
        public static bool IsMarker(uint Marker)
        {
            foreach (Markers ma in Enum.GetValues(typeof(Markers)))
            {
                if ((uint)ma == Marker)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicate whether a uint is a MetaProperties.
        /// </summary>
        /// <param name="marker">The uints value.</param>
        /// <returns>If is a MetaProperties, return true, else false.</returns>
        public static bool IsMetaTag(uint MetaTag)
        {
            foreach (MetaProperties me in Enum.GetValues(typeof(MetaProperties)))
            {
                if (MetaTag == (uint)me)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Supply help functions for lexical enumerations.
    /// </summary>
    public class LexicalTypeHelper
    {
        // Contains fixedPropTypes.
        private static List<PropertyDataType> FixedTypes;

        // Contains varPropTypes.
        private static List<PropertyDataType> VarTypes;

        // Contains mvPropTypes.
        private static List<PropertyDataType> MVTypes;

        // Contains CodePageTypes.
        private static List<CodePageType> CodePageTypes;

        // Contains MetaProperty Ids.
        private static List<ushort> MetaPropIds;

        /// <summary>
        /// Initializes static members of the LexicalTypeHelper class.
        /// </summary>
        static LexicalTypeHelper()
        {
            FixedTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypInteger16,
                PropertyDataType.PtypInteger32,
                PropertyDataType.PtypFloating32,
                PropertyDataType.PtypFloating64,
                PropertyDataType.PtypCurrency,
                PropertyDataType.PtypFloatingTime,
                PropertyDataType.PtypErrorCode,
                PropertyDataType.PtypBoolean,
                PropertyDataType.PtypInteger64,
                PropertyDataType.PtypTime,
                PropertyDataType.PtypGuid
            };

            VarTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypString,
                PropertyDataType.PtypString8,
                PropertyDataType.PtypBinary,
                PropertyDataType.PtypServerId,
                PropertyDataType.PtypObject_Or_PtypEmbeddedTable
            };

            MVTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypMultipleInteger16,
                PropertyDataType.PtypMultipleInteger32,
                PropertyDataType.PtypMultipleFloating32,
                PropertyDataType.PtypMultipleFloating64,
                PropertyDataType.PtypMultipleCurrency,
                PropertyDataType.PtypMultipleFloatingTime,
                PropertyDataType.PtypMultipleInteger64,
                PropertyDataType.PtypMultipleString,
                PropertyDataType.PtypMultipleString8,
                PropertyDataType.PtypMultipleTime,
                PropertyDataType.PtypMultipleGuid,
                PropertyDataType.PtypMultipleBinary
            };

            CodePageTypes = new List<CodePageType>
            {
                CodePageType.PtypCodePageUnicode,
                CodePageType.PtypCodePageUnicodeBigendian,
                CodePageType.PtypCodePageWesternEuropean
            };

            MetaPropIds = new List<ushort> 
            { 
                0x4016,
                0x400f,
                0x4011,
                0x407c,
                0x407a,
                0x4008
            };
        }

        /// <summary>
        /// Indicate whether a PropertyDataType is a multi-valued property type.
        /// </summary>
        /// <param name="type">A PropertyDataType.</param>
        /// <returns>If the PropertyDataType is a multi-value type return true, else false.</returns>
        public static bool IsMVType(PropertyDataType type)
        {
            return MVTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a PropertyDataType is either PtypString, PtypString8 or PtypBinary, PtypServerId, or PtypObject. 
        /// </summary>
        /// <param name="type">A PropertyDataType.</param>
        /// <returns>If the PropertyDataType is a either PtypString, PtypString8 or PtypBinary, PtypServerId, or PtypObject return true, else false.</returns>
        public static bool IsVarType(PropertyDataType type)
        {
            return VarTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a property type value of any type that has a fixed length.
        /// </summary>
        /// <param name="type">A property type.</param>
        /// <returns>If a property type value of any type that has a fixed length, return true , else return false.</returns>
        public static bool IsFixedType(PropertyDataType type)
        {
            return FixedTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a PropertyID is a Meta property ID.
        /// </summary>
        /// <param name="id">A ushort value.</param>
        /// <returns>If a PropertyID is a Meta property ID, return true, else return false.</returns>
        public static bool IsMetaPropertyID(ushort id)
        {
            return MetaPropIds.Contains(id);
        }

        /// <summary>
        /// Indicate whether a ushort value is a codePage property type. 
        /// </summary>
        /// <param name="type">A ushort value.</param>
        /// <returns>If the ushort is a either PtypCodePageUnicode, PtypCodePageUnicodeBigendian or PtypCodePageWesternEuropean return true, else false.</returns>
        public static bool IsCodePageType(ushort type)
        {
            foreach (CodePageType t in Enum.GetValues(typeof(CodePageType)))
            {
                if (type == (uint)t)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// This class contains int value and byte array block.
    /// </summary>
    public class LengthOfBlock
    {
        private int totalSize;
        private byte[] BlockSize;
        public LengthOfBlock(int totalSize, byte[] BlockSize)
        {
            this.totalSize = totalSize;
            this.BlockSize = BlockSize;
        }
    }
    #endregion

    #region Enum

    /// <summary>
    /// Code page property types are used to transmit string properties using the code page format of the string as stored on the server,
    /// </summary>
    public enum CodePageType : ushort
    {
        PtypCodePageUnicode = 0x84B0,
        PtypCodePageUnicodeBigendian = 0x84B1,
        PtypCodePageWesternEuropean = 0x84E4
    }

    /// <summary>
    /// Represents the type of FastTransfer stream.
    /// </summary>
    public enum FastTransferStreamType
    {
        contentsSync = 1,
        hierarchySync = 2,
        state = 3,
        folderContent = 4,
        MessageContent = 5,
        attachmentContent = 6,
        MessageList = 7,
        TopFolder = 8
    }

    /// <summary>
    ///  Object handles type. 
    /// </summary>
    public enum ObjectHandlesType : byte
    {
        FolderHandles = 0x01,
        MessageHandles = 0x02,
        AttachmentHandles = 0x03,
    }

    /// <summary>
    /// Syntactical markers
    /// </summary>
    public enum Markers : uint
    {
        StartTopFld = 0x40090003,
        EndFolder = 0x400B0003,
        StartSubFld = 0x400A0003,
        StartMessage = 0x400C0003,
        EndMessage = 0x400D0003,
        StartFAIMsg = 0x40100003,
        StartEmbed = 0x40010003,
        EndEmbed = 0x40020003,
        StartRecip = 0x40030003,
        EndToRecip = 0x40040003,
        NewAttach = 0x40000003,
        EndAttach = 0x400E0003,
        IncrSyncChg = 0x40120003,
        IncrSyncChgPartial = 0x407D0003,
        IncrSyncDel = 0x40130003,
        IncrSyncEnd = 0x40140003,
        IncrSyncRead = 0x402F0003,
        IncrSyncStateBegin = 0x403A0003,
        IncrSyncStateEnd = 0x403B0003,
        IncrSyncProgressMode = 0x4074000B,
        IncrSyncProgressPerMsg = 0x4075000B,
        IncrSyncMessage = 0x40150003,
        IncrSyncGroupInfo = 0x407B0102,
        FXErrorInfo = 0x40180003,
    }

    /// <summary>
    /// Meta properties
    /// </summary>
    public enum MetaProperties : uint
    {
        MetaTagEcWarning = 0x400f0003,
        MetaTagNewFXFolder = 0x40110102,
        MetaTagFXDelProp = 0x40160003,
        MetaTagIncrSyncGroupId = 0x407c0003,
        MetaTagIncrementalSyncMessagePartial = 0x407a0003,
        MetaTagDnPrefix = 0x4008001E
    }

    /// <summary>
    /// An enumeration that specifies the current status of the transfer. 
    /// </summary>
    public enum TransferStatus : ushort
    {
        Error = 0x0000,
        Partial = 0x0001,
        NoRoom = 0x0002,
        Done = 0x0003,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyFolder operation. 
    /// </summary>
    public enum CopyFlags_CopyFolder : byte
    {
        Move = 0x01,
        Unused1 = 0x02,
        Unused2 = 0x04,
        Unused3 = 0x08,
        CopySubfolders = 0x10,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyMessages operation. 
    /// </summary>
    public enum CopyFlags_CopyMessages : byte
    {
        Move = 0x01,
        Unused1 = 0x02,
        Unused2 = 0x04,
        Unused3 = 0x08,
        BestBody = 0x10,
        SendEntryId = 0x20,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyProperties operation. 
    /// </summary>
    public enum CopyFlags_CopyProperties : byte
    {
        Move = 0x01,
        Unused1 = 0x02,
        Unused2 = 0x04,
        Unused3 = 0x08,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyTo operation. 
    /// </summary>
    public enum CopyFlags_CopyTo : uint
    {
        Move = 0x00000001,
        Unused1 = 0x00000002,
        Unused2 = 0x00000004,
        Unused3 = 0x00000008,
        Unused4 = 0x00000200,
        Unused5 = 0x00000400,
        BestBody = 0x0002000,
    }

    /// <summary>
    /// An enumeration that specifies flags control the behavior of RopFastTransferSourceCopy operations. 
    /// </summary>
    [Flags]
    public enum SendOptions : byte
    {
        Unicode = 0x01,
        UseCpid = 0x02,
        ForUpload = 0x03,
        RecoverMode = 0x04,
        ForceUnicode = 0x08,
        PartialItem = 0x10,
        Reserved1 = 0x20,
        Reserved2 = 0x40,
    }

    /// <summary>
    /// An enumeration that defines the type of synchronization requested. 
    /// </summary>
    public enum SynchronizationType : byte
    {
        Contents = 0x01,
        Hierarchy = 0x02,
    }

    /// <summary>
    /// A flags structure that contains flags that control the behavior of the synchronization.
    /// </summary>
    [Flags]
    public enum SynchronizationFlags : ushort
    {
        Unicode = 0x0001,
        NoDeletions = 0x0002,
        IgnoreNoLongerInScope = 0x0004,
        ReadState = 0x0008,
        FAI = 0x0010,
        Normal = 0x0020,
        OnlySpecifiedProperties = 0x0080,
        NoForeignIdentifies = 0x0100,
        Reserved = 0x1000,
        BesBody = 0x2000,
        IgnoreSpecifiedOnFAI = 0x4000,
        Progress = 0x8000,
    }

    /// <summary>
    /// A flags structure that contains flags control the additional behavior of the synchronization.
    /// </summary>
    public enum SynchronizationExtraFlags : uint
    {
        Eid = 0x00000001,
        MessageSize = 0x00000002,
        CN = 0x00000004,
        OrderByDeliveryTime = 0x00000008,
    }

    /// <summary>
    /// This enumeration is used to specify the type of data in a FastTransfer stream that is uploaded by using the RopFastTransferDestinationPutBuffer ROP.
    /// </summary>
    public enum SourceOperation : byte
    {
        CopyTo = 0x01,
        CopyProperties = 0x02,
        CopyMessages = 0x03,
        CopyFolder = 0x04,
    }

    /// <summary>
    /// This enumeration is used to specify CopyFlags for destination configure.
    /// </summary>
    public enum CopyFlags_DestinationConfigure : byte
    {
        Move = 0x01,
    }

    /// <summary>
    /// An flag structure that defines the parameters of the import operation.
    /// </summary>
    public enum ImportDeleteFlags : byte
    {
        Hierarchy = 0x01,
        HardDelete = 0x02,
    }

    /// <summary>
    /// An flag structure that defines the parameters of the import operation.
    /// </summary>
    public enum ImportFlag : byte
    {
        Associated = 0x10,
        FailOnConflict = 0x40,
    }
    #endregion

    #region Structures used in FastTransfer stream (defined in MS-OXCDATA)
    /// <summary>
    /// The structure of LongTermId
    /// </summary>
    public class LongTermId : SyntacticalBase
    {
        /// <summary>
        /// A 128-bit unsigned integer identifying a Store object.
        /// </summary>
        public Guid DatabaseGuid;

        /// <summary>
        /// An unsigned 48-bit integer identifying the folder within its Store object.
        /// </summary>
        [BytesAttribute(6)]
        public ulong GlobalCounter;

        /// <summary>
        /// An unshort.
        /// </summary>
        public ushort pad;

        /// <summary>
        /// Initializes a new instance of the LongTermId structure.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public LongTermId(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse the ROP response buffer.
        /// </summary>
        /// <param name="ropBytes">ROPs bytes in response.</param>
        /// <param name="startIndex">The start index of this ROP.</param>
        /// <returns>The size of response buffer structure.</returns>
        public override void Parse(FastTransferStream stream)
        {
            this.DatabaseGuid = stream.ReadGuid();
            this.GlobalCounter = BitConverter.ToUInt64(stream.ReadBlock(6), 0);
            this.pad = stream.ReadUInt16();
        }
    }
    #endregion
}
