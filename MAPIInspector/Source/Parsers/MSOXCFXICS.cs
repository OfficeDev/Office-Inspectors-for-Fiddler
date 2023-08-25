namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    #region Enum

    /// <summary>
    /// Code page property types are used to transmit string properties using the code page format of the string as stored on the server,
    /// </summary>
    public enum CodePageType : ushort
    {
        /// <summary>
        /// PtypCodePage Unicode 51
        /// </summary>
        PtypCodePageUnicode = 0x84B0,

        /// <summary>
        /// PtypCodePage Unicode big end
        /// </summary>
        PtypCodePageUnicodeBigendian = 0x84B1,

        /// <summary>
        /// PtypCodePage western European
        /// </summary>
        PtypCodePageWesternEuropean = 0x84E4,

        /// <summary>
        /// ptypCodePag eUnicode 52
        /// </summary>
        ptypCodePageUnicode52 = 0x94B0
    }

    /// <summary>
    /// Represents the type of FastTransfer stream.
    /// </summary>
    public enum FastTransferStreamType
    {
        /// <summary>
        /// contentsSync type
        /// </summary>
        contentsSync = 1,

        /// <summary>
        /// hierarchySync type
        /// </summary>
        hierarchySync = 2,

        /// <summary>
        /// state type
        /// </summary>
        state = 3,

        /// <summary>
        /// folderContent type
        /// </summary>
        folderContent = 4,

        /// <summary>
        /// Message Content
        /// </summary>
        MessageContent = 5,

        /// <summary>
        /// attachment Content
        /// </summary>
        attachmentContent = 6,

        /// <summary>
        /// The MessageList element
        /// </summary>
        MessageList = 7,

        /// <summary>
        /// The TopFolder element
        /// </summary>
        TopFolder = 8
    }

    /// <summary>
    ///  Object handles type. 
    /// </summary>
    public enum ObjectHandlesType : byte
    {
        /// <summary>
        /// Handles for handle
        /// </summary>
        FolderHandles = 0x01,

        /// <summary>
        /// Message for handle
        /// </summary>
        MessageHandles = 0x02,

        /// <summary>
        /// Attachment for handle
        /// </summary>
        AttachmentHandles = 0x03,
    }

    /// <summary>
    /// Syntactical markers
    /// </summary>
    public enum Markers : uint
    {
        /// <summary>
        /// StartTopFld marker 
        /// </summary>
        StartTopFld = 0x40090003,

        /// <summary>
        /// EndFolder marker
        /// </summary>
        EndFolder = 0x400B0003,

        /// <summary>
        /// StartSubFld marker
        /// </summary>
        StartSubFld = 0x400A0003,

        /// <summary>
        /// StartMessage marker
        /// </summary>
        StartMessage = 0x400C0003,

        /// <summary>
        /// EndMessage marker 
        /// </summary>
        EndMessage = 0x400D0003,

        /// <summary>
        /// StartFAIMsg marker
        /// </summary>
        StartFAIMsg = 0x40100003,

        /// <summary>
        /// StartEmbed marker
        /// </summary>
        StartEmbed = 0x40010003,

        /// <summary>
        /// EndEmbed marker
        /// </summary>
        EndEmbed = 0x40020003,

        /// <summary>
        /// StartRecip marker
        /// </summary>
        StartRecip = 0x40030003,

        /// <summary>
        /// EndToRecip marker
        /// </summary>
        EndToRecip = 0x40040003,

        /// <summary>
        /// NewAttach marker
        /// </summary>
        NewAttach = 0x40000003,

        /// <summary>
        /// EndAttach marker
        /// </summary>
        EndAttach = 0x400E0003,

        /// <summary>
        /// IncrSyncChg marker
        /// </summary>
        IncrSyncChg = 0x40120003,

        /// <summary>
        /// IncrSyncChgPartial marker
        /// </summary>
        IncrSyncChgPartial = 0x407D0003,

        /// <summary>
        /// IncrSyncDel marker
        /// </summary>
        IncrSyncDel = 0x40130003,

        /// <summary>
        /// IncrSyncEnd marker
        /// </summary>
        IncrSyncEnd = 0x40140003,

        /// <summary>
        /// IncrSyncRead marker
        /// </summary>
        IncrSyncRead = 0x402F0003,

        /// <summary>
        /// IncrSyncStateBegin marker
        /// </summary>
        IncrSyncStateBegin = 0x403A0003,

        /// <summary>
        /// IncrSyncStateEnd marker
        /// </summary>
        IncrSyncStateEnd = 0x403B0003,

        /// <summary>
        /// IncrSyncProgressMode marker
        /// </summary>
        IncrSyncProgressMode = 0x4074000B,

        /// <summary>
        /// IncrSyncProgressPerMsg marker
        /// </summary>
        IncrSyncProgressPerMsg = 0x4075000B,

        /// <summary>
        /// IncrSyncMessage marker
        /// </summary>
        IncrSyncMessage = 0x40150003,

        /// <summary>
        /// IncrSyncGroupInfo marker
        /// </summary>
        IncrSyncGroupInfo = 0x407B0102,

        /// <summary>
        /// FXErrorInfo marker
        /// </summary>
        FXErrorInfo = 0x40180003,
    }

    /// <summary>
    /// Meta properties
    /// </summary>
    public enum MetaProperties : uint
    {
        /// <summary>
        /// The MetaTagEcWarning meta-property contains a warning that occurred when producing output for an element in context
        /// </summary>
        MetaTagEcWarning = 0x400f0003,

        /// <summary>
        /// The MetaTagNewFXFolder meta-property provides information about alternative replicas (1) for a public folder in context
        /// </summary>
        MetaTagNewFXFolder = 0x40110102,

        /// <summary>
        /// The MetaTagFXDelProp meta-property represents a directive to a client to delete specific subobjects of the object in context
        /// </summary>
        MetaTagFXDelProp = 0x40160003,

        /// <summary>
        /// The MetaTagIncrSyncGroupId meta-property specifies an identifier of a property group mapping
        /// </summary>
        MetaTagIncrSyncGroupId = 0x407c0003,

        /// <summary>
        /// The MetaTagIncrementalSyncMessagePartial meta-property specifies an index of a property group within a property group mapping currently in context
        /// </summary>
        MetaTagIncrementalSyncMessagePartial = 0x407a0003,

        /// <summary>
        /// The MetaTagDnPrefix meta-property MUST be ignored when received
        /// </summary>
        MetaTagDnPrefix = 0x4008001E
    }

    /// <summary>
    /// An enumeration that specifies the current status of the transfer. 
    /// </summary>
    public enum TransferStatus : ushort
    {
        /// <summary>
        /// The download stopped because a nonrecoverable error has occurred when producing a FastTransfer stream.
        /// </summary>
        Error = 0x0000,

        /// <summary>
        /// The FastTransfer stream was split, and more data is available.
        /// </summary>
        Partial = 0x0001,

        /// <summary>
        /// The FastTransfer stream was split, more data is available, and the value of the TransferBuffer field contains incomplete data
        /// </summary>
        NoRoom = 0x0002,

        /// <summary>
        /// This was the last portion of the FastTransfer stream.
        /// </summary>
        Done = 0x0003,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyFolder operation. 
    /// </summary>
    public enum CopyFlags_CopyFolder : byte
    {
        /// <summary>
        /// This bit flag indicates whether the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x01,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused1 = 0x02,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused2 = 0x04,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused3 = 0x08,

        /// <summary>
        /// The subfolders of the folder specified in the InputServerObject field are recursively included in the scope
        /// </summary>
        CopySubfolders = 0x10,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyMessages operation. 
    /// </summary>
    public enum CopyFlags_CopyMessages : byte
    {
        /// <summary>
        /// This bit flag indicates whether the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x01,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused1 = 0x02,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused2 = 0x04,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused3 = 0x08,

        /// <summary>
        /// Identify whether the output message body is in the original format or compressed RTF format.
        /// </summary>
        BestBody = 0x10,

        /// <summary>
        /// This bit flag indicates whether message change information is included in the FastTransfer stream
        /// </summary>
        SendEntryId = 0x20,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyProperties operation. 
    /// </summary>
    public enum CopyFlags_CopyProperties : byte
    {
        /// <summary>
        /// This bit flag indicates whether the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x01,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused1 = 0x02,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused2 = 0x04,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused3 = 0x08,
    }

    /// <summary>
    /// An enumeration that specifies flags control the type of RopFastTransferSourceCopyTo operation. 
    /// </summary>
    public enum CopyFlags_CopyTo : uint
    {
        /// <summary>
        /// This bit flag indicates whether the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x00000001,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused1 = 0x00000002,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused2 = 0x00000004,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused3 = 0x00000008,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused4 = 0x00000200,

        /// <summary>
        /// Unused flag
        /// </summary>
        Unused5 = 0x00000400,

        /// <summary>
        /// This flag MUST only be passed if the value of the InputServerObject field is a Message object.
        /// </summary>
        BestBody = 0x0002000,
    }

    /// <summary>
    /// An enumeration that specifies flags control the behavior of RopFastTransferSourceCopy operations. 
    /// </summary>
    [Flags]
    public enum SendOptions : byte
    {
        /// <summary>
        /// This flag indicates whether string properties are output in Unicode or in the code page set on the current connection
        /// </summary>
        Unicode = 0x01,

        /// <summary>
        /// This flag indicates support for code page property types
        /// </summary>
        UseCpid = 0x02,

        /// <summary>
        /// This flag is the combination of the Unicode and UseCpid flags.
        /// </summary>
        ForUpload = 0x03,

        /// <summary>
        /// This flag indicates that the client supports recovery mode
        /// </summary>
        RecoverMode = 0x04,

        /// <summary>
        /// This flag indicates whether string properties are output in Unicode.
        /// </summary>
        ForceUnicode = 0x08,

        /// <summary>
        /// This flag MUST only be set for content synchronization download operations.
        /// </summary>
        PartialItem = 0x10,

        /// <summary>
        /// Reserved flag
        /// </summary>
        Reserved1 = 0x20,

        /// <summary>
        /// Reserved flag
        /// </summary>
        Reserved2 = 0x40,
    }

    /// <summary>
    /// An enumeration that defines the type of synchronization requested. 
    /// </summary>
    public enum SynchronizationType : byte
    {
        /// <summary>
        /// Indicates a content synchronization operation.
        /// </summary>
        Contents = 0x01,

        /// <summary>
        /// Indicates a hierarchy synchronization operation
        /// </summary>
        Hierarchy = 0x02,
    }

    /// <summary>
    /// A flags structure that contains flags that control the behavior of the synchronization.
    /// </summary>
    [Flags]
    public enum SynchronizationFlags : ushort
    {
        /// <summary>
        /// Indicates whether the client supports Unicode
        /// </summary>
        Unicode = 0x0001,

        /// <summary>
        /// Indicates how the server downloads information about item deletions
        /// </summary>
        NoDeletions = 0x0002,

        /// <summary>
        /// Indicates whether the server downloads information about messages that went out of scope
        /// </summary>
        IgnoreNoLongerInScope = 0x0004,

        /// <summary>
        /// Indicates whether the server downloads information about changes to the read state of messages
        /// </summary>
        ReadState = 0x0008,

        /// <summary>
        /// Indicates whether the server downloads information about changes to FAI messages
        /// </summary>
        FAI = 0x0010,

        /// <summary>
        /// Indicates whether the server downloads information about changes to normal messages
        /// </summary>
        Normal = 0x0020,

        /// <summary>
        /// Indicates whether the server limits or excludes properties and subobjects output to the properties listed in PropertyTags
        /// </summary>
        OnlySpecifiedProperties = 0x0080,

        /// <summary>
        /// Identifies whether the server ignores any persisted values for the PidTagSourceKey property (section 2.2.1.2.5) and PidTagParentSourceKey property (section 2.2.1.2.6) when producing output for folder and message changes.
        /// </summary>
        NoForeignIdentifies = 0x0100,

        /// <summary>
        /// This flag MUST be set to 0 when sending.
        /// </summary>
        Reserved = 0x1000,

        /// <summary>
        /// Identifies whether the server outputs message bodies in their original format or in RTF
        /// </summary>
        BesBody = 0x2000,

        /// <summary>
        /// Indicates whether the server outputs properties and subobjects of FAI messages
        /// </summary>
        IgnoreSpecifiedOnFAI = 0x4000,

        /// <summary>
        /// Indicates whether the server injects progress information into the output FastTransfer stream
        /// </summary>
        Progress = 0x8000,
    }

    /// <summary>
    /// A flags structure that contains flags control the additional behavior of the synchronization.
    /// </summary>
    public enum SynchronizationExtraFlags : uint
    {
        /// <summary>
        /// Indicates whether the server includes the PidTagFolderId or PidTagMid properties in the folder change or message change header
        /// </summary>
        Eid = 0x00000001,

        /// <summary>
        /// Indicates whether the server includes the PidTagMessageSize property in the message change header. 
        /// </summary>
        MessageSize = 0x00000002,

        /// <summary>
        /// Indicates whether the server includes the PidTagChangeNumber property in the message change header
        /// </summary>
        CN = 0x00000004,

        /// <summary>
        /// Indicates whether the server sorts messages by their delivery time
        /// </summary>
        OrderByDeliveryTime = 0x00000008,
    }

    /// <summary>
    /// This enumeration is used to specify the type of data in a FastTransfer stream that is uploaded by using the RopFastTransferDestinationPutBuffer ROP.
    /// </summary>
    public enum SourceOperation : byte
    {
        /// <summary>
        /// The value of the InputServerObject field can be any Message,folder or attachment object.
        /// </summary>
        CopyTo = 0x01,

        /// <summary>
        /// The value of the InputServerObject field can be any Message,folder or attachment object.
        /// </summary>
        CopyProperties = 0x02,

        /// <summary>
        /// The value of the InputServerObject field is a message object.
        /// </summary>
        CopyMessages = 0x03,

        /// <summary>
        /// The value of the InputServerObject field is a folder object.
        /// </summary>
        CopyFolder = 0x04,
    }

    /// <summary>
    /// This enumeration is used to specify CopyFlags for destination configure.
    /// </summary>
    public enum CopyFlags_DestinationConfigure : byte
    {
        /// <summary>
        /// If this flag is set, the FastTransfer operation is being configured as a logical part of a larger object move operation
        /// </summary>
        Move = 0x01,
    }

    /// <summary>
    /// An flag structure that defines the parameters of the import operation.
    /// </summary>
    public enum ImportDeleteFlags : byte
    {
        /// <summary>
        /// If this flag is set, folder deletions are being imported.
        /// </summary>
        Hierarchy = 0x01,

        /// <summary>
        /// If this flag is set, hard deletions are being imported
        /// </summary>
        HardDelete = 0x02,
    }

    /// <summary>
    /// An flag structure that defines the parameters of the import operation.
    /// </summary>
    public enum ImportFlag : byte
    {
        /// <summary>
        /// If this flag is set, the message being imported is an FAI message
        /// </summary>
        Associated = 0x10,

        /// <summary>
        /// If this flag is set, the server accepts conflicting versions of a particular message
        /// </summary>
        FailOnConflict = 0x40,
    }
    #endregion
    #region 2.2.2.1 CN
    /// <summary>
    /// Represents CN structure contains a change number that identifies a version of a messaging object. 
    /// </summary>
    public class CN : BaseStructure
    {
        /// <summary>
        /// A 16-bit unsigned integer identifying the server replica in which the messaging object was last changed.
        /// </summary>
        public ushort ReplicaId;

        /// <summary>
        /// An unsigned 48-bit integer identifying the change to the messaging object.
        /// </summary>
        [BytesAttribute(6)]
        public ulong GlobalCounter;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains CN.</param>
        public void Parse(FastTransferStream stream)
        {
            this.ReplicaId = stream.ReadUInt16();
            this.GlobalCounter = BitConverter.ToUInt64(stream.ReadBlock(6), 0);
        }
    }
    #endregion

    #region 2.2.2.2 XID
    /// <summary>
    /// Represents an external identifier for an entity within a data store.
    /// </summary>
    public class XID : BaseStructure
    {
        /// <summary>
        /// A GUID that identifies the nameSpace that the identifier specified by LocalId belongs to
        /// </summary>
        public Guid NamespaceGuid;

        /// <summary>
        /// A variable binary value that contains the ID of the entity in the nameSpace specified by NamespaceGuid.
        /// </summary>
        public byte[] LocalId;

        /// <summary>
        /// A unsigned int value specifies the length of the LocalId.
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the XID class.
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
            this.NamespaceGuid = stream.ReadGuid();
            this.LocalId = stream.ReadBlock((int)this.length - 16);
        }
    }
    #endregion

    #region 2.2.2.3 PredecessorChangeList
    /// <summary>
    /// Contains a set of XIDs that represent change numbers of messaging objects in different replicas. 
    /// </summary>
    public class PredecessorChangeList : BaseStructure
    {
        /// <summary>
        /// A SizedXid list.
        /// </summary>
        public SizedXid[] SizedXidList;

        /// <summary>
        /// A unsigned int value specifies the length in bytes of the SizedXidList.
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the PredecessorChangeList class.
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
            for (int i = 0; i < this.length;)
            {
                int position = (int)stream.Position;
                SizedXid tmpSizedXid = new SizedXid();
                tmpSizedXid.Parse(stream);
                interSizeXid.Add(tmpSizedXid);

                i += (int)stream.Position - position;
            }

            this.SizedXidList = interSizeXid.ToArray();
        }
    }

    /// <summary>
    /// SizedXid structure.
    /// </summary>
    public class SizedXid : BaseStructure
    {
        /// <summary>
        /// An unsigned 8-bit integer.
        /// </summary>
        public byte XidSize;

        /// <summary>
        /// A structure of type XID that contains the value of the internal identifier of an object, or internal or external identifier of a change number. 
        /// </summary>
        public XID Xid;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains SizedXid.</param>
        public void Parse(FastTransferStream stream)
        {
            this.XidSize = stream.ReadByte();
            this.Xid = new XID((int)this.XidSize);
            this.Xid.Parse(stream);
        }
    }
    #endregion

    #region 2.2.2.4 IDSET Structure
    /// <summary>
    /// Represents a REPLID and GLOBSET structure pair. 
    /// </summary>
    public class IDSET_REPLID : BaseStructure
    {
        /// <summary>
        /// A unsigned short which combined with all GLOBCNT structures contained in the GLOBSET field, produces a set of IDs.
        /// </summary>
        public ushort REPLID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
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
        /// <summary>
        /// A GUID that identifies a REPLGUID structure. 
        /// </summary>
        public Guid REPLGUID;

        /// <summary>
        /// A serialized GLOBSET structure.
        /// </summary>
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
        /// <summary>
        /// Commands composed a GLOBCNT range, which indicates a GLOBSET structure.
        /// </summary>
        public Command[] Commands;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains GLOBSET.</param>
        public void Parse(FastTransferStream stream)
        {
            // A UInt value indicates the bytes length in common stacks.
            uint commonStackLength = 0;

            // A UInt list indicates the pushed or popped count of bytes in common stack.
            List<uint> commonStackCollection = new List<uint>();

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
                        Command pushCommand = new PushCommand();
                        pushCommand.Parse(stream);
                        commands.Add(pushCommand);
                        if ((commonStackLength + (uint)(pushCommand as PushCommand).Command) < 6)
                        {
                            commonStackCollection.Add((pushCommand as PushCommand).Command);
                            commonStackLength += (uint)(pushCommand as PushCommand).Command;
                        }

                        break;
                    case 0x50:
                        Command popCommand = new PopCommand();
                        popCommand.Parse(stream);
                        commands.Add(popCommand);
                        commonStackLength -= commonStackCollection[commonStackCollection.Count - 1];
                        commonStackCollection.RemoveAt(commonStackCollection.Count - 1);
                        break;
                    case 0x42:
                        Command bitmaskCommand = new BitmaskCommand();
                        bitmaskCommand.Parse(stream);
                        commands.Add(bitmaskCommand);
                        break;
                    case 0x52:
                        Command rangeCommand = new RangeCommand(6 - commonStackLength);
                        rangeCommand.Parse(stream);
                        commands.Add(rangeCommand);
                        break;
                    default:
                        break;
                }

                tmp = stream.ReadByte();
                stream.Position -= 1;
            }

            Command endCommand = new EndCommand();
            endCommand.Parse(stream);
            commands.Add(endCommand);
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
        /// <summary>
        /// An integer that specifies the number of high-order bytes that the GLOBCNT structures
        /// </summary>
        public byte Command;

        /// <summary>
        /// A byte array that contains the bytes shared by the GLOBCNT structures
        /// </summary>
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
        /// <summary>
        /// The Command for pop
        /// </summary>
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
        /// <summary>
        /// Bitmask Command.
        /// </summary>
        public byte Command;

        /// <summary>
        /// The low-order byte of the low value of the first GLOBCNT range.
        /// </summary>
        public byte StartValue;

        /// <summary>
        /// One bit set for each value within a range, excluding the low value of the first GLOBCNT range.
        /// </summary>
        public byte Bitmask;

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains BitmaskCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.StartValue = stream.ReadByte();
            this.Bitmask = stream.ReadByte();
        }
    }

    /// <summary>
    /// Represent a range command.
    /// </summary>
    public class RangeCommand : Command
    {
        /// <summary>
        /// Bitmask Command.
        /// </summary>
        public byte Command;

        /// <summary>
        /// The low value of the range.
        /// </summary>
        public byte[] LowValue;

        /// <summary>
        /// The high value of the range.
        /// </summary>
        public byte[] HighValue;

        /// <summary>
        /// The length of the LowValue and hignValue.
        /// </summary>
        private uint length;

        /// <summary>
        /// Initializes a new instance of the RangeCommand class.
        /// </summary>
        /// <param name="length">The length of the LowValue and hignValue.</param>
        public RangeCommand(uint length)
        {
            this.length = length;
        }

        /// <summary>
        /// Parse from a stream.
        /// </summary>
        /// <param name="stream">A stream contains RangeCommand.</param>
        public override void Parse(FastTransferStream stream)
        {
            this.Command = stream.ReadByte();
            this.LowValue = stream.ReadBlock((int)this.length);
            this.HighValue = stream.ReadBlock((int)this.length);
        }
    }

    /// <summary>
    /// Represent an end command.
    /// </summary>
    public class EndCommand : Command
    {
        /// <summary>
        /// The Command for end
        /// </summary>
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
        /// <summary>
        /// An unsigned 16-bit value that contains a number that identifies the binary structure of the data that follows.
        /// </summary>
        public ushort Version;

        /// <summary>
        ///  The padding.
        /// </summary>
        public ushort Padding1;

        /// <summary>
        /// An unsigned 32-bit integer value that contains the total number of changes to FAI messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public uint FAIMessageCount;

        /// <summary>
        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to FAI messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public ulong FAIMessageTotalSize;

        /// <summary>
        /// An unsigned 32-bit integer value that contains the total number of changes to normal messages that are scheduled for download during the current synchronization operation.
        /// </summary>
        public uint NormalMessageCount;

        /// <summary>
        /// The padding.
        /// </summary>
        public uint Padding2;

        /// <summary>
        /// An unsigned 64-bit integer value that contains the size in bytes of all changes to normal messages  that are scheduled for download during the current synchronization operation.
        /// </summary>
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
    /// <summary>
    /// The PropertyGroupInfo class
    /// </summary>
    public class PropertyGroupInfo : BaseStructure
    {
        /// <summary>
        /// An unsigned 32-bit integer value that identifies a property mapping within the current synchronization download context.
        /// </summary>
        public uint GroupId;

        /// <summary>
        /// A reserved field
        /// </summary>
        public uint Reserved;

        /// <summary>
        ///  An unsigned 32-bit integer value that specifies how many PropertyGroup structures are present in the Groups field. 
        /// </summary>
        public uint GroupCount;

        /// <summary>
        /// An array of PropertyGroup structures,
        /// </summary>
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
                this.Groups[i] = tmpPropertyGroup;
            }
        }
    }

    #region 2.2.2.8.1 PropertyGroup
    /// <summary>
    /// The PropertyGroup.
    /// </summary>
    public class PropertyGroup : BaseStructure
    {
        /// <summary>
        /// An unsigned 32-bit integer value that specifies how many PropertyTag structures are present in the PropertyTags field. 
        /// </summary>
        public uint PropertyTagCount;

        /// <summary>
        /// An array of PropertyTagWithGroupPropertyName structures.
        /// </summary>
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
                this.PropertyTags[i] = tmpName;
            }
        }
    }

    /// <summary>
    /// This structure is a PropertyTag Structure (MS-OXCDATA section 2.9) which is special for named properties 
    /// </summary>
    public class PropertyTagWithGroupPropertyName : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value, as specified by the table in section 2.11.1.
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public ushort PropertyId;

        /// <summary>
        /// A GroupPropertyName structure.
        /// </summary>
        public GroupPropertyName GroupPropertyName;

        /// <summary>
        /// Parse the PropertyTagWithGroupPropertyName structure.
        /// </summary>
        /// <param name="stream">A stream containing the PropertyTagWithGroupPropertyName structure</param>
        public void Parse(FastTransferStream stream)
        {
            this.PropertyType = (PropertyDataType)stream.ReadUInt16();
            this.PropertyId = stream.ReadUInt16();
            if (this.PropertyId >= 0x8000)
            {
                this.GroupPropertyName = new GroupPropertyName();
                this.GroupPropertyName.Parse(stream);
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
        /// <summary>
        /// The GUID that identifies the property set for the named property.
        /// </summary>
        public Guid Guid;

        /// <summary>
        /// A value that identifies the type of property. 
        /// </summary>
        public uint Kind;

        /// <summary>
        ///  A value that identifies the named property within its property set. 
        /// </summary>
        public uint? Lid;

        /// <summary>
        /// A value that specifies the length of the Name field, in bytes. 
        /// </summary>
        public uint? NameSize;

        /// <summary>
        /// A Unicode (UTF-16) string that identifies the property within the property set. 
        /// </summary>
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
                this.Name = new MAPIString(Encoding.Unicode, string.Empty, (int)this.NameSize / 2);
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
        /// <summary>
        /// A UInt value.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A UInt value.
        /// </summary>
        public uint Depth;

        /// <summary>
        /// A LongTermID structure. Contains the LongTermID of a folder, for which server replica information is being described.
        /// </summary>
        public LongTermId FolderLongTermId;

        /// <summary>
        /// An unsigned 32-bit integer value that determines how many elements exist in ServerDNArray. 
        /// </summary>
        public uint ServerDNCount;

        /// <summary>
        /// An unsigned 32-bit integer value that determines how many of the leading elements in ServerDNArray have the same,lowest, network access cost.
        /// </summary>
        public uint CheapServerDNCount;

        /// <summary>
        /// An array of ASCII-encoded NULL-terminated strings. 
        /// </summary>
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// An unsigned integer that specifies whether descendant subobjects are copied
        /// </summary>
        public byte Level;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public CopyFlags_CopyProperties CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyPropertiesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyPropertiesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.Level = this.ReadByte();
            this.CopyFlags = (CopyFlags_CopyProperties)this.ReadByte();
            this.SendOptions = (SendOptions)this.ReadByte();
            this.PropertyTagCount = this.ReadUshort();
            PropertyTag[] interTag = new PropertyTag[(int)this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                interTag[i] = new PropertyTag();
                interTag[i].Parse(s);
            }

            this.PropertyTags = interTag;
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyProperties ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyPropertiesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyPropertiesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyPropertiesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.2 RopFastTransferSourceCopyTo
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyTo ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyToRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// An unsigned integer that specifies whether descendant subobjects are copied
        /// </summary>
        public byte Level;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public CopyFlags_CopyTo CopyFlags;

        /// <summary>
        ///  A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyToRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyToRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.Level = this.ReadByte();
            this.CopyFlags = (CopyFlags_CopyTo)this.ReadUint();
            this.SendOptions = (SendOptions)this.ReadByte();
            this.PropertyTagCount = this.ReadUshort();
            PropertyTag[] interTag = new PropertyTag[(int)this.PropertyTagCount];
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                interTag[i] = new PropertyTag();
                interTag[i].Parse(s);
            }

            this.PropertyTags = interTag;
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyTo ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyToResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyToResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyToResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.3 RopFastTransferSourceCopyMessages
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyMessages ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyMessagesRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specifies the messages to copy. 
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// A flags structure that contains flags that control the type of operation. 
        /// </summary>
        public CopyFlags_CopyMessages CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyMessagesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyMessagesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.MessageIdCount = this.ReadUshort();

            List<MessageID> messageIdList = new List<MessageID>();
            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID messageId = new MessageID();
                messageId.Parse(s);
                messageIdList.Add(messageId);
            }

            this.MessageIds = messageIdList.ToArray();
            this.CopyFlags = (CopyFlags_CopyMessages)ReadByte();
            this.SendOptions = (SendOptions)ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyMessages ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyMessagesResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyMessagesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyMessagesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.4 RopFastTransferSourceCopyFolder
    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyFolder ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyFolderRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// A flags structure that contains flags that control the type of operation.
        /// </summary>
        public CopyFlags_CopyFolder CopyFlags;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation. 
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyFolderRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyFolderRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CopyFlags = (CopyFlags_CopyFolder)this.ReadByte();
            this.SendOptions = (SendOptions)this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceCopyFolder ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceCopyFolderResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferSourceCopyFolderResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceCopyFolderResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.1.5 RopFastTransferSourceGetBuffer
    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Request Buffer.
    /// </summary>
    public class RopFastTransferSourceGetBufferRequest : BaseStructure
    {
        /// <summary>
        /// A byte that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// A byte that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// A byte that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An UShort that specifies the buffer size requested.
        /// </summary>
        public ushort BufferSize;

        /// <summary>
        /// An UShort that is present when the BufferSize field is set to 0xBABE.
        /// </summary>
        public ushort? MaximumBufferSize;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceGetBufferRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.BufferSize = this.ReadUshort();
            if (this.BufferSize == 0xBABE)
            {
                this.MaximumBufferSize = this.ReadUshort();
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferSourceGetBuffer ROP Response Buffer.
    /// </summary>
    public class RopFastTransferSourceGetBufferResponse : BaseStructure
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
        /// An enumeration that specifies the current status of the transfer. 
        /// </summary>
        public TransferStatus? TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public ushort? InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate number of steps to be completed in the current operation.
        /// </summary>
        public ushort? TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the TransferBuffer field.
        /// </summary>
        public ushort? TransferBufferSize;

        /// <summary>
        ///  An array of bytes that specifies FastTransferStream.
        /// </summary>
        public object TransferBuffer;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds for the client to wait before trying this operation again
        /// </summary>
        public uint? BackoffTime;

        /// <summary>
        /// Parse the RopFastTransferSourceGetBufferResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferSourceGetBufferResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.TransferStatus = (TransferStatus)this.ReadUshort();
                this.InProgressCount = this.ReadUshort();
                this.TotalStepCount = this.ReadUshort();
                this.Reserved = this.ReadByte();
                this.TransferBufferSize = this.ReadUshort();
                byte[] buffer = ReadBytes((int)this.TransferBufferSize);
                FastTransferStream transferStream = new FastTransferStream(new byte[0], true);
                long sposition = 0;

                if (this.TransferStatus.Value == Parsers.TransferStatus.Partial)
                {
                    transferStream = new FastTransferStream(buffer, true);
                    List<TransferGetBufferElement> transferBufferList = new List<TransferGetBufferElement>();

                    while (!transferStream.IsEndOfStream)
                    {
                        sposition = transferStream.Position;
                        TransferGetBufferElement element = new TransferGetBufferElement(transferStream);

                        if (sposition == transferStream.Position)
                        {
                            throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                        }
                        else
                        {
                            transferBufferList.Add(element);
                        }
                    }

                    this.TransferBuffer = transferBufferList.ToArray();
                }
                else
                {
                    transferStream = new FastTransferStream(buffer, true);
                    List<TransferGetBufferElement> transferBufferList = new List<TransferGetBufferElement>();

                    while (transferStream.Position < transferStream.Length)
                    {
                        sposition = transferStream.Position;

                        TransferGetBufferElement element = new TransferGetBufferElement(transferStream);
                        if (sposition == transferStream.Position)
                        {
                            throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                        }
                        else
                        {
                            transferBufferList.Add(element);
                        }
                    }

                    this.TransferBuffer = transferBufferList.ToArray();
                }
            }

            if ((AdditionalErrorCodes)this.ReturnValue == AdditionalErrorCodes.ServerBusy)
            {
                this.BackoffTime = this.ReadUint();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An array of three unsigned 16-bit integers that contains the version information for the other server. 
        /// </summary>
        public byte[] Version;

        /// <summary>
        /// Parse the RopTellVersionRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopTellVersionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Version = this.ReadBytes(6);
        }
    }

    /// <summary>
    ///  A class indicates the RopTellVersion ROP Response Buffer.
    /// </summary>
    public class RopTellVersionResponse : BaseStructure
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
        /// Parse the RopTellVersionResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopTellVersionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.2.1 RopFastTransferDestinationConfigure
    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Request Buffer.
    /// </summary>
    public class RopFastTransferDestinationConfigureRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// An enumeration that indicates how the data stream was created on the source.
        /// </summary>
        public SourceOperation SourceOperation;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the transfer operation.
        /// </summary>
        public CopyFlags_DestinationConfigure CopyFlags;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationConfigureRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.SourceOperation = (SourceOperation)this.ReadByte();
            this.CopyFlags = (CopyFlags_DestinationConfigure)this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationConfigure ROP Response Buffer.
    /// </summary>
    public class RopFastTransferDestinationConfigureResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFastTransferDestinationConfigureResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationConfigureResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.1.2.2 RopFastTransferDestinationPutBuffer
    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBuffer ROP Request Buffer.
    /// </summary>
    public class RopFastTransferDestinationPutBufferRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the TransferData field. 
        /// </summary>
        public ushort TransferDataSize;

        /// <summary>
        /// An array of bytes that contains the data to be uploaded to the destination fast transfer object.
        /// </summary>
        public object TransferData;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationPutBufferRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.TransferDataSize = this.ReadUshort();
            byte[] buffer = ReadBytes((int)this.TransferDataSize);
            FastTransferStream transferStream = new FastTransferStream(buffer, true);

            List<TransferPutBufferElement> transferBufferList = new List<TransferPutBufferElement>();
            long sposition = 0;

            while (!transferStream.IsEndOfStream)
            {
                sposition = transferStream.Position;
                TransferPutBufferElement element = new TransferPutBufferElement(transferStream);

                if (sposition == transferStream.Position)
                {
                    throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                }
                else
                {
                    transferBufferList.Add(element);
                }
            }

            this.TransferData = transferBufferList.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBuffer ROP Response Buffer.
    /// </summary>
    public class RopFastTransferDestinationPutBufferResponse : BaseStructure
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
        /// The current status of the transfer.
        /// </summary>
        public ushort TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public ushort InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate total number of steps to be completed in the current operation.
        /// </summary>
        public ushort TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An unsigned integer that specifies the buffer size that was used.
        /// </summary>
        public ushort BufferUsedSize;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationPutBufferResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            this.TransferStatus = this.ReadUshort();
            this.InProgressCount = this.ReadUshort();
            this.TotalStepCount = this.ReadUshort();
            this.Reserved = this.ReadByte();
            this.BufferUsedSize = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.3.1.2.3 RopFastTransferDestinationPutBufferExtended
    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBufferExtended ROP Request Buffer.
    /// </summary>
    public class RopFastTransferDestinationPutBufferExtendedRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the TransferData field. 
        /// </summary>
        public ushort TransferDataSize;

        /// <summary>
        /// An array of bytes that contains the data to be uploaded to the destination fast transfer object.
        /// </summary>
        public object TransferData;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferExtendedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationPutBufferExtendedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.TransferDataSize = this.ReadUshort();

            byte[] buffer = ReadBytes((int)this.TransferDataSize);
            FastTransferStream transferStream = new FastTransferStream(buffer, true);
            List<TransferPutBufferExtendElement> transferBufferList = new List<TransferPutBufferExtendElement>();
            long sposition = 0;

            while (!transferStream.IsEndOfStream)
            {
                sposition = transferStream.Position;
                TransferPutBufferExtendElement element = new TransferPutBufferExtendElement(transferStream);
                if (sposition == transferStream.Position)
                {
                    throw new Exception(string.Format("Error occurred in the {0} TransferElement", transferBufferList.Count));
                }
                else
                {
                    transferBufferList.Add(element);
                }
            }

            this.TransferData = transferBufferList.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopFastTransferDestinationPutBufferExtended ROP Response Buffer.
    /// </summary>
    public class RopFastTransferDestinationPutBufferExtendedResponse : BaseStructure
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
        /// The current status of the transfer.
        /// </summary>
        public ushort TransferStatus;

        /// <summary>
        /// An unsigned integer that specifies the number of steps that have been completed in the current operation.
        /// </summary>
        public uint InProgressCount;

        /// <summary>
        /// An unsigned integer that specifies the approximate total number of steps to be completed in the current operation.
        /// </summary>
        public uint TotalStepCount;

        /// <summary>
        /// A reserved field
        /// </summary>
        public byte Reserved;

        /// <summary>
        /// An unsigned integer that specifies the buffer size that was used.
        /// </summary>
        public ushort BufferUsedSize;

        /// <summary>
        /// Parse the RopFastTransferDestinationPutBufferExtendedResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFastTransferDestinationPutBufferExtendedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
            this.TransferStatus = this.ReadUshort();
            this.InProgressCount = this.ReadUint();
            this.TotalStepCount = this.ReadUint();
            this.Reserved = this.ReadByte();
            this.BufferUsedSize = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.3.2.1.1 RopSynchronizationConfigure
    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationConfigureRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// An enumeration that controls the type of synchronization.
        /// </summary>
        public SynchronizationType SynchronizationType;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the operation.
        /// </summary>
        public SendOptions SendOptions;

        /// <summary>
        /// A flags structure that contains flags that control the behavior of the synchronization.
        /// </summary>
        public SynchronizationFlags SynchronizationFlags;

        /// <summary>
        /// An unsigned integer that specifies the length, in bytes, of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet,that specifies the filter for this synchronization object.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// A flags structure that contains flags control the additional behavior of the synchronization. 
        /// </summary>
        public SynchronizationExtraFlags SynchronizationExtraFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the PropertyTags field.
        /// </summary>
        public ushort PropertyTagCount;

        /// <summary>
        ///  An array of PropertyTag structures that specifies the properties to exclude during the copy.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSynchronizationConfigureRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationConfigureRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.SynchronizationType = (SynchronizationType)this.ReadByte();
            this.SendOptions = (SendOptions)this.ReadByte();
            this.SynchronizationFlags = (SynchronizationFlags)this.ReadUshort();
            this.RestrictionDataSize = this.ReadUshort();

            if (this.RestrictionDataSize > 0)
            {
                this.RestrictionData = new RestrictionType();
                this.RestrictionData.Parse(s);
            }

            this.SynchronizationExtraFlags = (SynchronizationExtraFlags)this.ReadUint();
            this.PropertyTagCount = this.ReadUshort();
            PropertyTag[] interTag = new PropertyTag[(int)this.PropertyTagCount];

            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                interTag[i] = new PropertyTag();
                interTag[i].Parse(s);
            }

            this.PropertyTags = interTag;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationConfigure ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationConfigureResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationConfigureResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationConfigureResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.2.1 RopSynchronizationUploadStateStreamBegin
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamBegin ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamBeginRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A PropertyTag structure.
        /// </summary>
        public uint StateProperty;

        /// <summary>
        /// An unsigned integer that specifies the size of the stream to be uploaded.
        /// </summary>
        public uint TransferBufferSize;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamBeginRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamBeginRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.StateProperty = this.ReadUint();
            this.TransferBufferSize = this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamBegin ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamBeginResponse : BaseStructure
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
        /// Parse the RopSynchronizationUploadStateStreamBeginResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamBeginResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.2.2 RopSynchronizationUploadStateStreamContinue
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamContinue ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the StreamData field.
        /// </summary>
        public uint StreamDataSize;

        /// <summary>
        /// An array of bytes that contains the state stream data to be uploaded.
        /// </summary>
        public byte[] StreamData;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamContinueRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamContinueRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.StreamDataSize = this.ReadUint();
            this.StreamData = this.ReadBytes((int)this.StreamDataSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamContinue ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamContinueResponse : BaseStructure
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
        /// Parse the RopSynchronizationUploadStateStreamContinueResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamContinueResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.2.3 RopSynchronizationUploadStateStreamEnd
    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamEnd ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamEndRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopSynchronizationUploadStateStreamEndRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamEndRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationUploadStateStreamEnd ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationUploadStateStreamEndResponse : BaseStructure
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
        /// Parse the RopSynchronizationUploadStateStreamEndResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationUploadStateStreamEndResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.3.1 RopSynchronizationGetTransferState
    /// <summary>
    ///  A class indicates the RopSynchronizationGetTransferState ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationGetTransferStateRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// Parse the RopSynchronizationGetTransferStateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationGetTransferStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationGetTransferState ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationGetTransferStateResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationGetTransferStateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationGetTransferStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.4.1 RopSynchronizationOpenCollector
    /// <summary>
    ///  A class indicates the RopSynchronizationOpenCollector ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationOpenCollectorRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon
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
        /// A Boolean that specifies whether this synchronization upload context is for contents or for hierarchy.
        /// </summary>
        public bool IsContentsCollector;
        
        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationOpenCollectorRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.IsContentsCollector = this.ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationOpenCollector ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationOpenCollectorResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSynchronizationOpenCollectorResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationOpenCollectorResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.4.2 RopSynchronizationImportMessageChange
    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageChange ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageChangeRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
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
        /// A flags structure that contains flags that control the behavior of the synchronization.
        /// </summary>
        public ImportFlag ImportFlag;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify extra properties on the message.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageChangeRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportMessageChangeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ImportFlag = (ImportFlag)this.ReadByte();
            this.PropertyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue(CountWideEnum.twoBytes);
                interValue[i].Parse(s);
            }

            this.PropertyValues = interValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageChange ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageChangeResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An identifier.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageChangeResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportMessageChangeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the HierarchyValues field.
        /// </summary>
        public ushort HierarchyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify hierarchy-related properties of the folder.
        /// </summary>
        public TaggedPropertyValue[] HierarchyValues;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportHierarchyChangeRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.HierarchyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interHierarchyValues = new TaggedPropertyValue[(int)this.HierarchyValueCount];

            for (int i = 0; i < this.HierarchyValueCount; i++)
            {
                interHierarchyValues[i] = new TaggedPropertyValue();
                interHierarchyValues[i].Parse(s);
            }

            this.HierarchyValues = interHierarchyValues;
            this.PropertyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue();
                interValue[i].Parse(s);
            }

            this.PropertyValues = interValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportHierarchyChange ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportHierarchyChangeResponse : BaseStructure
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
        /// An identifier.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// Parse the RopSynchronizationImportHierarchyChangeResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportHierarchyChangeResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of the SourceFolderId field.
        /// </summary>
        public uint SourceFolderIdSize;

        /// <summary>
        ///  An array of bytes that identifies the parent folder of the source message.
        /// </summary>
        public byte[] SourceFolderId;

        /// <summary>
        /// An unsigned integer that specifies the size of the SourceMessageId field.
        /// </summary>
        public uint SourceMessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the source message.
        /// </summary>
        public byte[] SourceMessageId;

        /// <summary>
        /// An unsigned integer that specifies the size of the PredecessorChangeList field.
        /// </summary>
        public uint PredecessorChangeListSize;

        /// <summary>
        /// An array of bytes. The size of this field, in bytes, is specified by the PredecessorChangeListSize field.
        /// </summary>
        public byte[] PredecessorChangeList;

        /// <summary>
        /// An unsigned integer that specifies the size of the DestinationMessageId field.
        /// </summary>
        public uint DestinationMessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the destination message. 
        /// </summary>
        public byte[] DestinationMessageId;

        /// <summary>
        /// An unsigned integer that specifies the size of the ChangeNumber field.
        /// </summary>
        public uint ChangeNumberSize;

        /// <summary>
        /// An array of bytes that specifies the change number of the message. 
        /// </summary>
        public byte[] ChangeNumber;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportMessageMoveRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.SourceFolderIdSize = this.ReadUint();
            this.SourceFolderId = this.ReadBytes((int)this.SourceFolderIdSize);
            this.SourceMessageIdSize = this.ReadUint();
            this.SourceMessageId = this.ReadBytes((int)this.SourceMessageIdSize);
            this.PredecessorChangeListSize = this.ReadUint();
            this.PredecessorChangeList = this.ReadBytes((int)this.PredecessorChangeListSize);
            this.DestinationMessageIdSize = this.ReadUint();
            this.DestinationMessageId = this.ReadBytes((int)this.DestinationMessageIdSize);
            this.ChangeNumberSize = this.ReadUint();
            this.ChangeNumber = this.ReadBytes((int)this.ChangeNumberSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportMessageMove ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportMessageMoveResponse : BaseStructure
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
        /// An identifier.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSynchronizationImportMessageMoveResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportMessageMoveResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that specify options for the imported deletions.
        /// </summary>
        public ImportDeleteFlags ImportDeleteFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the PropertyValues field.
        /// </summary>
        public ushort PropertyValueCount;

        /// <summary>
        /// An array of TaggedPropertyValue structures that specify the folders or messages to delete.
        /// </summary>
        public TaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the RopSynchronizationImportDeletesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportDeletesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ImportDeleteFlags = (ImportDeleteFlags)this.ReadByte();
            this.PropertyValueCount = this.ReadUshort();
            TaggedPropertyValue[] interValue = new TaggedPropertyValue[(int)this.PropertyValueCount];

            for (int i = 0; i < this.PropertyValueCount; i++)
            {
                interValue[i] = new TaggedPropertyValue();
                interValue[i].Parse(s);
            }

            this.PropertyValues = interValue;
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportDeletes ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportDeletesResponse : BaseStructure
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
        /// Parse the RopSynchronizationImportDeletesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportDeletesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.3.2.4.6 RopSynchronizationImportReadStateChanges
    /// <summary>
    ///  A class indicates the RopSynchronizationImportReadStateChanges ROP Request Buffer.
    /// </summary>
    public class RopSynchronizationImportReadStateChangesRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the MessageReadStates field.
        /// </summary>
        public ushort MessageReadStatesSize;

        /// <summary>
        /// A list of MessageReadState structures that specify the messages and associated read states to be changed.
        /// </summary>
        public MessageReadState[] MessageReadStates;

        /// <summary>
        /// Parse the RopSynchronizationImportReadStateChangesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportReadStateChangesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageReadStatesSize = this.ReadUshort();
            List<MessageReadState> interValue = new List<MessageReadState>();
            int size = this.MessageReadStatesSize;

            while (size > 0)
            {
                MessageReadState interValueI = new MessageReadState();
                interValueI.Parse(s);
                interValue.Add(interValueI);
                size -= interValueI.MessageId.Length + 1 + 2;
            }

            this.MessageReadStates = interValue.ToArray();
        }
    }

    /// <summary>
    ///  A class indicates the RopSynchronizationImportReadStateChanges ROP Response Buffer.
    /// </summary>
    public class RopSynchronizationImportReadStateChangesResponse : BaseStructure
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
        /// Parse the RopSynchronizationImportReadStateChangesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSynchronizationImportReadStateChangesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }

    /// <summary>
    ///  A class indicates the MessageReadState structure.
    /// </summary>
    public class MessageReadState : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the size of the MessageId field.
        /// </summary>
        public ushort MessageIdSize;

        /// <summary>
        /// An array of bytes that identifies the message to be marked as read or unread.
        /// </summary>
        public byte[] MessageId;

        /// <summary>
        /// A Boolean that specifies whether to mark the message as read or not.
        /// </summary>
        public bool MarkAsRead;

        /// <summary>
        /// Parse the MessageReadState structure.
        /// </summary>
        /// <param name="s">A stream containing MessageReadState structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.MessageIdSize = this.ReadUshort();
            this.MessageId = this.ReadBytes(this.MessageIdSize);
            this.MarkAsRead = this.ReadBoolean();
        }
    }
    #endregion

    #region 2.2.3.2.4.7 RopGetLocalReplicaIds
    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Request Buffer.
    /// </summary>
    public class RopGetLocalReplicaIdsRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the number of IDs to reserve.
        /// </summary>
        public uint IdCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetLocalReplicaIdsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.IdCount = this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetLocalReplicaIds ROP Response Buffer.
    /// </summary>
    public class RopGetLocalReplicaIdsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// This field contains the replica GUID that is shared by the IDs.
        /// </summary>
        public Guid? ReplGuid;

        /// <summary>
        /// An array of bytes that specifies the first value in the reserved range.
        /// </summary>
        public byte?[] GlobalCount;

        /// <summary>
        /// Parse the RopGetLocalReplicaIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetLocalReplicaIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.ReplGuid = this.ReadGuid();
                this.GlobalCount = this.ConvertArray(this.ReadBytes(6));
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the size of both the LongTermIdRangeCount and LongTermIdRanges fields.
        /// </summary>
        public ushort DataSize;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the LongTermIdRanges field.
        /// </summary>
        public uint LongTermIdRangeCount;

        /// <summary>
        /// An array of LongTermIdRange structures that specify the ranges of message identifiers that have been deleted.
        /// </summary>
        public LongTermIdRange[] LongTermIdRanges;

        /// <summary>
        /// Parse the RopSetLocalReplicaMidsetDeletedRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetLocalReplicaMidsetDeletedRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.DataSize = this.ReadUshort();
            this.LongTermIdRangeCount = this.ReadUint();
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
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSetLocalReplicaMidsetDeletedResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetLocalReplicaMidsetDeletedResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(this.ReadUint());
        }
    }

    /// <summary>
    ///  A class indicates the LongTermIdRange structure.
    /// </summary>
    public class LongTermIdRange : BaseStructure
    {
        /// <summary>
        /// A LongTermId structure that specifies the beginning of a range. 
        /// </summary>
        public LongTermID MinLongTermId;

        /// <summary>
        /// A LongTermId structure that specifies the end of a range.
        /// </summary>
        public LongTermID MaxLongTermId;

        /// <summary>
        /// Parse the LongTermIdRange structure.
        /// </summary>
        /// <param name="s">A stream containing LongTermIdRange structure.</param>
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
        /// <summary>
        /// The length of a GUID structure.
        /// </summary>
        public static int GuidLength = Guid.Empty.ToByteArray().Length;

        /// <summary>
        /// The length of a MetaTag property.
        /// </summary>
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
        /// Read a UInt value from stream,and advance the position within the stream by 4
        /// </summary>
        /// <returns>The UInt value.</returns>
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
            for (i = 0; i < totalSize; i++)
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
                i += 1;
                list.Add(tmp);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Read a list of blocks and advance the position for partial.
        /// </summary>
        /// <param name="totalSize">The total number of bytes to read</param>
        /// <param name="blockSize">The size of each block</param>
        /// <param name="type">The data type to read</param>
        /// <param name="isGetbuffer">Check whether it's RopGetBuffer</param>
        /// <param name="isPutBuffer">Check whether it's RopPutBuffer</param>
        /// <returns>A list of blocks</returns>
        public byte[][] ReadBlocksPartial(int totalSize, int blockSize, ushort type, bool isGetbuffer, bool isPutBuffer)
        {
            int i;
            List<byte[]> l = new List<byte[]>();

            for (i = 0; i < totalSize; i++)
            {
                int remainLength = totalSize - i;

                if (isGetbuffer)
                {
                    if (this.IsEndOfStream)
                    {
                        MapiInspector.MAPIInspector.PartialGetType = type;
                        MapiInspector.MAPIInspector.PartialGetRemainSize = remainLength;
                        MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
						MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                        break;
                    }
                }
                else if (isPutBuffer)
                {
                    if (this.IsEndOfStream)
                    {
                        MapiInspector.MAPIInspector.PartialPutType = type;
                        MapiInspector.MAPIInspector.PartialPutRemainSize = remainLength;
                        MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
						MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                        break;
                    }
                }
                else
                {
                    if (this.IsEndOfStream)
                    {
                        MapiInspector.MAPIInspector.PartialPutExtendType = type;
                        MapiInspector.MAPIInspector.PartialPutExtendRemainSize = remainLength;
                        MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
						MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                        break;
                    }
                }

                // fixedSizeValue is a split atom, so the blockSize will be read without split 
                l.Add(this.ReadBlock(blockSize));
            }

            return l.ToArray();
        }

        /// <summary>
        /// Read LengthOfBlock and advance the position.
        /// </summary>
        /// <param name="length">The length to read</param>
        /// <param name="type">The data type parsing</param>
        /// <param name="isGetbuffer">Check whether it's RopGetBuffer</param>
        /// <param name="isPutBuffer">Check whether it's RopPutBuffer</param>
        /// <returns>A LengthOfBlock specifies the length of the bytes array</returns>
        public LengthOfBlock ReadLengthBlockPartial(int length, ushort type, bool isGetbuffer, bool isPutBuffer)
        {
            int tmp = 0;

            if (isGetbuffer)
            {
                if (this.IsEndOfStream)
                {
                    MapiInspector.MAPIInspector.PartialGetType = type;
                    MapiInspector.MAPIInspector.PartialGetRemainSize = length;
                    MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                    MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                }
                else
                {
                    if (MapiInspector.MAPIInspector.PartialGetSubRemainSize != -1 && !this.IsEndOfStream
                        && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                        && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        tmp = MapiInspector.MAPIInspector.PartialGetSubRemainSize;
                        MapiInspector.MAPIInspector.PartialGetSubRemainSize = -1;
                        MapiInspector.MAPIInspector.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetClientInfo = string.Empty;
                    }
                    else
                    {
                        tmp = this.ReadInt32();
                    }

                    if (this.Length - this.Position < tmp)
                    {
                        MapiInspector.MAPIInspector.PartialGetType = type;
                        MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                        MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                        MapiInspector.MAPIInspector.PartialGetSubRemainSize = tmp - (int)(this.Length - this.Position);
                        MapiInspector.MAPIInspector.PartialGetRemainSize = length;
                        tmp = (int)(this.Length - this.Position);
                    }
                }
            }
            else if (isPutBuffer)
            {
                if (this.IsEndOfStream)
                {
                    MapiInspector.MAPIInspector.PartialPutType = type;
                    MapiInspector.MAPIInspector.PartialPutRemainSize = length;
                    MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                    MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                }
                else
                {
                    if (MapiInspector.MAPIInspector.PartialPutSubRemainSize != -1 && !this.IsEndOfStream && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                        && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        tmp = MapiInspector.MAPIInspector.PartialPutSubRemainSize;
                        MapiInspector.MAPIInspector.PartialPutSubRemainSize = -1;
                        MapiInspector.MAPIInspector.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutClientInfo = string.Empty;
                    }
                    else
                    {
                        tmp = this.ReadInt32();
                    }

                    if (this.Length - this.Position < tmp)
                    {
                        MapiInspector.MAPIInspector.PartialPutType = type;
                        MapiInspector.MAPIInspector.PartialPutSubRemainSize = tmp - (int)(this.Length - this.Position);
                        tmp = (int)(this.Length - this.Position);
                        MapiInspector.MAPIInspector.PartialPutRemainSize = length;
                        MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                        MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                    }
                }
            }
            else
            {
                if (this.IsEndOfStream)
                {
                    MapiInspector.MAPIInspector.PartialPutExtendType = type;
                    MapiInspector.MAPIInspector.PartialPutExtendRemainSize = length;
                    MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                    MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                }
                else
                {
                    if (MapiInspector.MAPIInspector.PartialPutExtendSubRemainSize != -1 && !this.IsEndOfStream && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                        && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        tmp = MapiInspector.MAPIInspector.PartialPutExtendSubRemainSize;
                        MapiInspector.MAPIInspector.PartialPutExtendSubRemainSize = -1;
                        MapiInspector.MAPIInspector.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendClientInfo = string.Empty;
                    }
                    else
                    {
                        tmp = this.ReadInt32();
                    }

                    if (this.Length - this.Position < tmp)
                    {
                        MapiInspector.MAPIInspector.PartialPutExtendType = type;
                        MapiInspector.MAPIInspector.PartialPutExtendSubRemainSize = tmp - (int)(this.Length - this.Position);
                        tmp = (int)(this.Length - this.Position);
                        MapiInspector.MAPIInspector.PartialPutExtendRemainSize = length;
                        MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                        MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                        MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                    }
                }
            }

            byte[] buffer = this.ReadBlock(tmp);
            return new LengthOfBlock(tmp, buffer);
        }

        /// <summary>
        /// Read a list of LengthOfBlock and advance the position.
        /// </summary>
        /// <param name="totalLength">The number of bytes to read</param>
        /// <param name="type">The data type parsing</param>
        /// <param name="isGetbuffer">Check whether it's RopGetBuffer</param>
        /// <param name="isPutBuffer">Check whether it's RopPutBuffer</param>
        /// <returns>A list of LengthOfBlock</returns>
        public LengthOfBlock[] ReadLengthBlocksPartial(int totalLength, ushort type, bool isGetbuffer, bool isPutBuffer)
        {
            int i = 0;
            List<LengthOfBlock> list = new List<LengthOfBlock>();

            while (i < totalLength)
            {
                int remainLength = totalLength - i;
                LengthOfBlock tmp = this.ReadLengthBlockPartial(remainLength, type, isGetbuffer, isPutBuffer);
                i += 1;
                list.Add(tmp);

                if (isGetbuffer)
                {
                    if ((MapiInspector.MAPIInspector.PartialGetSubRemainSize != -1 || MapiInspector.MAPIInspector.PartialGetRemainSize != -1)
                        && (MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                        && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"]))
                    {
                        break;
                    }
                }
                else if (isPutBuffer)
                {
                    if ((MapiInspector.MAPIInspector.PartialPutSubRemainSize != -1 || MapiInspector.MAPIInspector.PartialPutRemainSize != -1)
                        && (MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                        && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"]))
                    {
                        break;
                    }
                }
                else
                {
                    if ((MapiInspector.MAPIInspector.PartialPutExtendSubRemainSize != -1 || MapiInspector.MAPIInspector.PartialPutExtendRemainSize != -1)
                        && (MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                        && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                        && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"]))
                    {
                        break;
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Get a UInt value and do not advance the position.
        /// </summary>
        /// <returns>A UInt value </returns>
        public uint VerifyUInt32()
        {
            try
            {
                return BitConverter.ToUInt32(
                    this.GetBuffer(),
                    (int)this.Position);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
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
        /// Indicate the Markers at the current position plus an offset equals a specified Markers
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
        /// Indicate the UInt value at the position equals a specified UInt value.
        /// </summary>
        /// <param name="val">A UInt value.</param>
        /// <returns>True if the UInt at the position equals the specified uint.else false.</returns>
        public bool Verify(uint val)
        {
            return !this.IsEndOfStream && BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position) == val;
        }

        /// <summary>
        /// Indicate the UInt value at the position plus an offset equals a specified UInt value.
        /// </summary>
        /// <param name="val">A UInt value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the UInt at the position plus an offset equals the specified UInt,else false.</returns>
        public bool Verify(uint val, int offset)
        {
            return !this.IsEndOfStream && BitConverter.ToUInt32(
                this.GetBuffer(),
                (int)this.Position + offset) == val;
        }

        /// <summary>
        /// Indicate the byte value at the position plus an offset equals a specified byte
        /// </summary>
        /// <param name="val">A UInt value</param>
        /// <param name="offset">An int value</param>
        /// <returns>True if the byte at the position plus an offset equals the specified byte, else false.</returns>
        public bool Verify(byte val, int offset)
        {
            byte[] tmp = this.GetBuffer();
            return !this.IsEndOfStream && tmp[(int)this.Position + offset] == val;
        }
    }
    #endregion

    #region 2.2.4.1 FastTransfer stream lexical structure
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

        public override string ToString() => string.Empty;
    }

    /// <summary>
    /// The PropValue represents identification information and the value of the property.
    /// </summary>
    public class PropValue : LexicalBase
    {
        /// <summary>
        /// The propType.
        /// </summary>
        public PropertyDataType? PropType;

        /// <summary>
        /// The PropInfo.
        /// </summary>
        public PropInfo PropInfo;

        /// <summary>
        /// The propType for partial split
        /// </summary>
        protected ushort ptype;

        /// <summary>
        /// The PropId for partial split
        /// </summary>
        protected PidTagPropertyEnum pid;

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
            if ((MapiInspector.MAPIInspector.IsPut == true && (MapiInspector.MAPIInspector.PartialPutType == 0 || (MapiInspector.MAPIInspector.PartialPutType != 0 && !(MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])))) ||
                (MapiInspector.MAPIInspector.IsGet == true && (MapiInspector.MAPIInspector.PartialGetType == 0 || (MapiInspector.MAPIInspector.PartialGetType != 0 && !(MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])))) ||
                (MapiInspector.MAPIInspector.IsPutExtend == true && (MapiInspector.MAPIInspector.PartialPutExtendType == 0 || (MapiInspector.MAPIInspector.PartialPutType != 0 && !(MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])))))
            {
                this.PropType = (PropertyDataType)stream.ReadUInt16();
                this.PropInfo = PropInfo.ParseFrom(stream) as PropInfo;
            }
        }
    }

    /// <summary>
    /// The PropInfo class.
    /// </summary>
    public class PropInfo : LexicalBase
    {
        /// <summary>
        /// The property id.
        /// </summary>
        public PidTagPropertyEnum PropID;

        /// <summary>
        /// The namedPropInfo in lexical definition.
        /// </summary>
        public NamedPropInfo NamedPropInfo;

        /// <summary>
        /// Initializes a new instance of the PropInfo class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public PropInfo(FastTransferStream stream)
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
            this.PropID = (PidTagPropertyEnum)stream.ReadUInt16();

            if ((ushort)this.PropID >= 0x8000)
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
        /// <summary>
        /// A fixed value.
        /// </summary>
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
                    if ((ushort)this.PropInfo.PropID == 0x67A4)
                    {
                        CN tmpCN = new CN();
                        tmpCN.Parse(stream);
                        this.FixedValue = tmpCN;
                    }
                    else
                    {
                        this.FixedValue = stream.ReadInt32();
                    }

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
                    if ((ushort)this.PropInfo.PropID == 0x6714)
                    {
                        CN tmpCN = new CN();
                        tmpCN.Parse(stream);
                        this.FixedValue = tmpCN;
                    }
                    else if ((ushort)base.PropInfo.PropID == 0x674A)
                    {
                        MessageID tmpMID = new MessageID();
                        tmpMID.Parse(stream);
                        this.FixedValue = tmpMID;
                    }
                    else if ((ushort)base.PropInfo.PropID == 0x6748)
                    {
                        FolderID tmpFID = new FolderID();
                        tmpFID.Parse(stream);
                        this.FixedValue = tmpFID;
                    }
                    else
                    {
                        this.FixedValue = stream.ReadInt64();
                    }

                    break;
                case PropertyDataType.PtypTime:
                    PtypTime tempPropertyValue = new PtypTime();
                    tempPropertyValue.Parse(stream);
                    this.FixedValue = tempPropertyValue;
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
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public int Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
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

            if (LexicalTypeHelper.IsCodePageType((ushort)this.PropType))
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
                        if ((ushort)this.PropInfo.PropID == 0x65E0 || (ushort)this.PropInfo.PropID == 0x65E1 || (ushort)this.PropInfo.PropID == 0x65E2)
                        {
                            if (this.Length != 0)
                            {
                                XID tmpXID = new XID(this.Length);
                                tmpXID.Parse(stream);
                                this.ValueArray = tmpXID;
                            }
                        }
                        else if ((ushort)this.PropInfo.PropID == 0x65E3) // PidTagPredecessorChangeList 
                        {
                            PredecessorChangeList tmpPredecessorChangeList = new PredecessorChangeList(this.Length);
                            tmpPredecessorChangeList.Parse(stream);
                            this.ValueArray = tmpPredecessorChangeList;
                        }
                        else if ((ushort)this.PropInfo.PropID == 0x402D || (ushort)this.PropInfo.PropID == 0x402E || (ushort)this.PropInfo.PropID == 0x67E5 || (ushort)this.PropInfo.PropID == 0x4021 || (ushort)this.PropInfo.PropID == 0x6793)
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
                        else if ((ushort)this.PropInfo.PropID == 0x4017 || (ushort)this.PropInfo.PropID == 0x6796 || (ushort)this.PropInfo.PropID == 0x67DA || (ushort)this.PropInfo.PropID == 0x67D2)
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
                        PtypServerId pserverId = new PtypServerId(CountWideEnum.fourBytes);

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
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public int Length;

        /// <summary>
        /// A list of fixed size values.
        /// </summary>
        public byte[][] FixedSizeValueList;

        /// <summary>
        /// A list of LengthOfBlock.
        /// </summary>
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
            this.Length = stream.ReadInt32();

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
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValueGetPartial : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public object FixedValue;

        /// <summary>
        /// Initializes a new instance of the FixedPropTypePropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public FixedPropTypePropValueGetPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);
            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialGetType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialGetId = (ushort)this.PropInfo.PropID;
                MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialGetType != 0 && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialGetType;
                    this.pid = (PidTagPropertyEnum)MapiInspector.MAPIInspector.PartialGetId;

                    // clear
                    MapiInspector.MAPIInspector.PartialGetType = 0;
                    MapiInspector.MAPIInspector.PartialGetId = 0;
                    MapiInspector.MAPIInspector.PartialGetServerUrl = string.Empty;
                    MapiInspector.MAPIInspector.PartialGetProcessName = string.Empty;
                    MapiInspector.MAPIInspector.PartialGetClientInfo = string.Empty;
                }

                ushort typeValue;
                ushort identifyValue;
                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if (this.PropInfo != null)
                {
                    identifyValue = (ushort)this.PropInfo.PropID;
                }
                else
                {
                    identifyValue = (ushort)this.pid;
                }

                switch ((PropertyDataType)typeValue)
                {
                    case PropertyDataType.PtypInteger16:
                        this.FixedValue = stream.ReadInt16();
                        break;
                    case PropertyDataType.PtypInteger32:
                        if (identifyValue == 0x67A4)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else
                        {
                            this.FixedValue = stream.ReadInt32();
                        }

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
                        if (identifyValue == 0x6714)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else if (identifyValue == 0x674A)
                        {
                            MessageID tmpMID = new MessageID();
                            tmpMID.Parse(stream);
                            this.FixedValue = tmpMID;
                        }
                        else if (identifyValue == 0x6748)
                        {
                            FolderID tmpFID = new FolderID();
                            tmpFID.Parse(stream);
                            this.FixedValue = tmpFID;
                        }
                        else
                        {
                            this.FixedValue = stream.ReadInt64();
                        }

                        break;
                    case PropertyDataType.PtypTime:
                        PtypTime tempPropertyValue = new PtypTime();
                        tempPropertyValue.Parse(stream);
                        this.FixedValue = tempPropertyValue;
                        break;
                    case PropertyDataType.PtypGuid:
                        this.FixedValue = stream.ReadGuid();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValueGetPartial : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public int? Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public object ValueArray;

        /// <summary>
        /// The length value used for partial split
        /// </summary>
        protected int plength;

        /// <summary>
        /// Initializes a new instance of the VarPropTypePropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValueGetPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialGetType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialGetType != 0 && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialGetType;

                    if (MapiInspector.MAPIInspector.PartialGetRemainSize != -1)
                    {
                        this.plength = MapiInspector.MAPIInspector.PartialGetRemainSize;
                        if (this.plength % 2 != 0 && (this.ptype == (ushort)PropertyDataType.PtypString || this.ptype == (ushort)CodePageType.PtypCodePageUnicode || this.ptype == (ushort)CodePageType.ptypCodePageUnicode52))
                        {
                            MapiInspector.MAPIInspector.IsOneMoreByteToRead = true;
                        }

                        MapiInspector.MAPIInspector.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIInspector.PartialGetType = 0;
                    MapiInspector.MAPIInspector.PartialGetId = 0;

                    if (MapiInspector.MAPIInspector.PartialGetRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if ((stream.Length - stream.Position) < lengthValue)
                {
                    MapiInspector.MAPIInspector.PartialGetType = typeValue;
                    MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                    MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            PtypString pstring = new PtypString();

                            if (stream.Length - stream.Position < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (MapiInspector.MAPIInspector.IsOneMoreByteToRead)
                                    {
                                        stream.Position += 1;
                                        MapiInspector.MAPIInspector.IsOneMoreByteToRead = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (MapiInspector.MAPIInspector.IsOneMoreByteToRead)
                                {
                                    stream.Position += 1;
                                    MapiInspector.MAPIInspector.IsOneMoreByteToRead = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case CodePageType.ptypCodePageUnicode52:
                            {
                                PtypString pstringII = new PtypString();

                                if (this.Length != null)
                                {
                                    this.Length = stream.ReadInt32();
                                    lengthValue = (int)this.Length;
                                }

                                if (stream.Length - stream.Position < lengthValue)
                                {
                                    MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                    this.plength = (int)(stream.Length - stream.Position);
                                    lengthValue = this.plength;

                                    if (lengthValue != 0)
                                    {
                                        if (MapiInspector.MAPIInspector.IsOneMoreByteToRead)
                                        {
                                            stream.Position += 1;
                                            MapiInspector.MAPIInspector.IsOneMoreByteToRead = false;
                                        }

                                        if ((lengthValue / 2) != 0)
                                        {
                                            pstringII = new PtypString(lengthValue / 2);
                                            pstringII.Parse(stream);
                                        }
                                        else
                                        {
                                            pstringII = null;
                                        }

                                        if (lengthValue % 2 != 0)
                                        {
                                            stream.Position += 1;
                                        }
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }
                                else
                                {
                                    if (MapiInspector.MAPIInspector.IsOneMoreByteToRead)
                                    {
                                        stream.Position += 1;
                                        MapiInspector.MAPIInspector.IsOneMoreByteToRead = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstringII = new PtypString(lengthValue / 2);
                                        pstringII.Parse(stream);
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }

                                this.ValueArray = pstringII;
                                break;
                            }

                        case CodePageType.PtypCodePageUnicodeBigendian:
                        case CodePageType.PtypCodePageWesternEuropean:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pdstring8 = new PtypString8(lengthValue);
                            pdstring8.Parse(stream);
                            this.ValueArray = pdstring8;
                            break;
                    }
                }
                else
                {
                    switch ((PropertyDataType)typeValue)
                    {
                        case PropertyDataType.PtypString:
                            PtypString pstring = new PtypString();

                            if (stream.Length - stream.Position < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (MapiInspector.MAPIInspector.IsOneMoreByteToRead)
                                    {
                                        stream.Position += 1;
                                        MapiInspector.MAPIInspector.IsOneMoreByteToRead = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (MapiInspector.MAPIInspector.IsOneMoreByteToRead)
                                {
                                    stream.Position += 1;
                                    MapiInspector.MAPIInspector.IsOneMoreByteToRead = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case PropertyDataType.PtypString8:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        case PropertyDataType.PtypBinary:
                        case PropertyDataType.PtypServerId:
                        case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialGetRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValueGetPartial : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public int? Length;

        /// <summary>
        /// A list of fixed size values.
        /// </summary>
        public byte[][] FixedSizeValueList;

        /// <summary>
        /// A list of LengthOfBlock.
        /// </summary>
        public LengthOfBlock[] VarSizeValueList;

        /// <summary>
        /// Length value for partial split
        /// </summary>
        private int Plength;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValueGetPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialGetType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialGetType != 0 && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialGetType;

                    if (MapiInspector.MAPIInspector.PartialGetRemainSize != -1)
                    {
                        this.Plength = MapiInspector.MAPIInspector.PartialGetRemainSize;
                        MapiInspector.MAPIInspector.PartialGetRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIInspector.PartialGetType = 0;
                    if (MapiInspector.MAPIInspector.PartialGetRemainSize == -1 && MapiInspector.MAPIInspector.PartialGetSubRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.Plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                switch ((PropertyDataType)typeValue)
                {
                    case PropertyDataType.PtypMultipleInteger16:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 2, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, Guid.Empty.ToByteArray().Length, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, true, false);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, true, false);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public object FixedValue;

        /// <summary>
        /// Initializes a new instance of the FixedPropTypePropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public FixedPropTypePropValuePutPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutId = (ushort)this.PropInfo.PropID;
                MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutType != 0 && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialPutType;
                    this.pid = (PidTagPropertyEnum)MapiInspector.MAPIInspector.PartialPutId;

                    // clear
                    MapiInspector.MAPIInspector.PartialPutType = 0;
                    MapiInspector.MAPIInspector.PartialPutId = 0;
                    MapiInspector.MAPIInspector.PartialPutServerUrl = string.Empty;
                    MapiInspector.MAPIInspector.PartialPutProcessName = string.Empty;
                    MapiInspector.MAPIInspector.PartialPutClientInfo = string.Empty;
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if (this.PropInfo != null)
                {
                    identifyValue = (ushort)this.PropInfo.PropID;
                }
                else
                {
                    identifyValue = (ushort)this.pid;
                }

                switch ((PropertyDataType)typeValue)
                {
                    case PropertyDataType.PtypInteger16:
                        this.FixedValue = stream.ReadInt16();
                        break;
                    case PropertyDataType.PtypInteger32:
                        if (identifyValue == 0x67A4)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else
                        {
                            this.FixedValue = stream.ReadInt32();
                        }

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
                        if (identifyValue == 0x6714)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else if (identifyValue == 0x674A)
                        {
                            MessageID tmpMID = new MessageID();
                            tmpMID.Parse(stream);
                            this.FixedValue = tmpMID;
                        }
                        else if (identifyValue == 0x6748)
                        {
                            FolderID tmpFID = new FolderID();
                            tmpFID.Parse(stream);
                            this.FixedValue = tmpFID;
                        }
                        else
                        {
                            this.FixedValue = stream.ReadInt64();
                        }

                        break;
                    case PropertyDataType.PtypTime:
                        PtypTime tempPropertyValue = new PtypTime();
                        tempPropertyValue.Parse(stream);
                        this.FixedValue = tempPropertyValue;
                        break;
                    case PropertyDataType.PtypGuid:
                        this.FixedValue = stream.ReadGuid();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public int? Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public object ValueArray;

        /// <summary>
        /// The length value used for partial split
        /// </summary>
        protected int plength;

        /// <summary>
        /// Boolean value used to record whether ptypString value is split to two bytes which parse in different buffer
        /// </summary>
        protected bool splitpreviousOne = false;

        /// <summary>
        /// Initializes a new instance of the VarPropTypePropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValuePutPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutType != 0 && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialPutType;

                    if (MapiInspector.MAPIInspector.PartialPutRemainSize != -1)
                    {
                        this.plength = MapiInspector.MAPIInspector.PartialPutRemainSize;
                        MapiInspector.MAPIInspector.PartialPutRemainSize = -1;

                        if (this.plength % 2 != 0 && (this.ptype == (ushort)PropertyDataType.PtypString || this.ptype == (ushort)CodePageType.PtypCodePageUnicode || this.ptype == (ushort)CodePageType.ptypCodePageUnicode52))
                        {
                            this.splitpreviousOne = true;
                        }
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }
                    
                    // clear
                    MapiInspector.MAPIInspector.PartialPutType = 0;

                    if (MapiInspector.MAPIInspector.PartialPutRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if ((stream.Length - stream.Position) < lengthValue)
                {
                    MapiInspector.MAPIInspector.PartialPutType = typeValue;
                    MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                    MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            PtypString pstring = new PtypString();

                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    stream.Position += 1;
                                    splitpreviousOne = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case CodePageType.ptypCodePageUnicode52:
                            {
                                PtypString pstringII = new PtypString();

                                if (this.Length != null)
                                {
                                    this.Length = stream.ReadInt32();
                                    lengthValue = (int)this.Length;
                                }

                                if (stream.Length - stream.Position < lengthValue)
                                {
                                    MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                    this.plength = (int)(stream.Length - stream.Position);
                                    lengthValue = this.plength;

                                    if (lengthValue != 0)
                                    {
                                        if (this.splitpreviousOne)
                                        {
                                            stream.Position += 1;
                                            this.splitpreviousOne = false;
                                        }

                                        if ((lengthValue / 2) != 0)
                                        {
                                            pstringII = new PtypString(lengthValue / 2);
                                            pstringII.Parse(stream);
                                        }
                                        else
                                        {
                                            pstringII = null;
                                        }

                                        if (lengthValue % 2 != 0)
                                        {
                                            stream.Position += 1;
                                        }
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }
                                else
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstringII = new PtypString(lengthValue / 2);
                                        pstringII.Parse(stream);
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }

                                this.ValueArray = pstringII;

                                break;
                            }

                        case CodePageType.PtypCodePageUnicodeBigendian:
                        case CodePageType.PtypCodePageWesternEuropean:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pdstring8 = new PtypString8(lengthValue);
                            pdstring8.Parse(stream);
                            this.ValueArray = pdstring8;
                            break;
                    }
                }
                else
                {
                    switch ((PropertyDataType)typeValue)
                    {
                        case PropertyDataType.PtypString:
                            PtypString pstring = new PtypString();
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                                if (lengthValue != 0)
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    stream.Position += 1;
                                    splitpreviousOne = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case PropertyDataType.PtypString8:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        case PropertyDataType.PtypBinary:
                        case PropertyDataType.PtypServerId:
                        case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValuePutPartial : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public int? Length;

        /// <summary>
        /// A list of fixed size values.
        /// </summary>
        public byte[][] FixedSizeValueList;

        /// <summary>
        /// A list of LengthOfBlock.
        /// </summary>
        public LengthOfBlock[] VarSizeValueList;

        /// <summary>
        /// Length for partial
        /// </summary>
        private int Plength;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValuePutPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutType != 0 && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialPutType;

                    if (MapiInspector.MAPIInspector.PartialPutRemainSize != -1)
                    {
                        this.Plength = MapiInspector.MAPIInspector.PartialPutRemainSize;
                        MapiInspector.MAPIInspector.PartialPutRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIInspector.PartialPutType = 0;
                    MapiInspector.MAPIInspector.PartialPutId = 0;

                    if (MapiInspector.MAPIInspector.PartialPutRemainSize == -1 && MapiInspector.MAPIInspector.PartialPutSubRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.Plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }
                
                switch ((PropertyDataType)this.PropType)
                {
                    case PropertyDataType.PtypMultipleInteger16:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 2, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, Guid.Empty.ToByteArray().Length, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, true);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, true);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Represent a fixedPropType PropValue.
    /// </summary>
    public class FixedPropTypePropValuePutExtendPartial : PropValue
    {
        /// <summary>
        /// A fixed value.
        /// </summary>
        public object FixedValue;

        /// <summary>
        /// Initializes a new instance of the FixedPropTypePropValuePutExtendPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public FixedPropTypePropValuePutExtendPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutExtendType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutExtendId = (ushort)this.PropInfo.PropID;
                MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutExtendType != 0 && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialPutExtendType;
                    this.pid = (PidTagPropertyEnum)MapiInspector.MAPIInspector.PartialPutExtendId;
                    
                    // clear
                    MapiInspector.MAPIInspector.PartialPutExtendType = 0;
                    MapiInspector.MAPIInspector.PartialPutExtendId = 0;
                    MapiInspector.MAPIInspector.PartialPutExtendServerUrl = string.Empty;
                    MapiInspector.MAPIInspector.PartialPutExtendProcessName = string.Empty;
                    MapiInspector.MAPIInspector.PartialPutExtendClientInfo = string.Empty;
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if (this.PropInfo != null)
                {
                    identifyValue = (ushort)this.PropInfo.PropID;
                }
                else
                {
                    identifyValue = (ushort)this.pid;
                }

                switch ((PropertyDataType)typeValue)
                {
                    case PropertyDataType.PtypInteger16:
                        this.FixedValue = stream.ReadInt16();
                        break;
                    case PropertyDataType.PtypInteger32:
                        if (identifyValue == 0x67A4)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else
                        {
                            this.FixedValue = stream.ReadInt32();
                        }

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
                        if (identifyValue == 0x6714)
                        {
                            CN tmpCN = new CN();
                            tmpCN.Parse(stream);
                            this.FixedValue = tmpCN;
                        }
                        else if (identifyValue == 0x674A)
                        {
                            MessageID tmpMID = new MessageID();
                            tmpMID.Parse(stream);
                            this.FixedValue = tmpMID;
                        }
                        else if (identifyValue == 0x6748)
                        {
                            FolderID tmpFID = new FolderID();
                            tmpFID.Parse(stream);
                            this.FixedValue = tmpFID;
                        }
                        else
                        {
                            this.FixedValue = stream.ReadInt64();
                        }

                        break;
                    case PropertyDataType.PtypTime:
                        PtypTime tempPropertyValue = new PtypTime();
                        tempPropertyValue.Parse(stream);
                        this.FixedValue = tempPropertyValue;
                        break;
                    case PropertyDataType.PtypGuid:
                        this.FixedValue = stream.ReadGuid();
                        break;
                }
            }
        }
    }

    /// <summary>
    /// The VarPropTypePropValue class.
    /// </summary>
    public class VarPropTypePropValuePutExtendPartial : PropValue
    {
        /// <summary>
        /// The length of a variate type value.
        /// </summary>
        public int? Length;

        /// <summary>
        /// The valueArray.
        /// </summary>
        public object ValueArray;

        /// <summary>
        /// The length value used for partial split
        /// </summary>
        protected int plength;

        /// <summary>
        /// Boolean value used to record whether ptypString value is split to two bytes which parse in different buffer
        /// </summary>
        protected bool splitpreviousOne = false;

        /// <summary>
        /// Initializes a new instance of the VarPropTypePropValuePutExtendPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public VarPropTypePropValuePutExtendPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutExtendType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutExtendType != 0 && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialPutExtendType;

                    if (MapiInspector.MAPIInspector.PartialPutExtendRemainSize != -1)
                    {
                        this.plength = MapiInspector.MAPIInspector.PartialPutExtendRemainSize;
                        MapiInspector.MAPIInspector.PartialPutExtendRemainSize = -1;

                        if (this.plength % 2 != 0 && (this.ptype == (ushort)PropertyDataType.PtypString || this.ptype == (ushort)CodePageType.PtypCodePageUnicode || this.ptype == (ushort)CodePageType.ptypCodePageUnicode52))
                        {
                            this.splitpreviousOne = true;
                        }
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIInspector.PartialPutExtendType = 0;

                    if (MapiInspector.MAPIInspector.PartialPutExtendRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                if ((stream.Length - stream.Position) < lengthValue)
                {
                    MapiInspector.MAPIInspector.PartialPutExtendType = typeValue;
                    MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                    MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                    MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
                }

                if (LexicalTypeHelper.IsCodePageType(typeValue))
                {
                    switch ((CodePageType)typeValue)
                    {
                        case CodePageType.PtypCodePageUnicode:
                            PtypString pstring = new PtypString();

                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    stream.Position += 1;
                                    splitpreviousOne = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case CodePageType.ptypCodePageUnicode52:
                            {
                                PtypString pstringII = new PtypString();

                                if (this.Length != null)
                                {
                                    this.Length = stream.ReadInt32();
                                    lengthValue = (int)this.Length;
                                }

                                if (stream.Length - stream.Position < lengthValue)
                                {
                                    MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                    this.plength = (int)(stream.Length - stream.Position);
                                    lengthValue = this.plength;

                                    if (lengthValue != 0)
                                    {
                                        if (this.splitpreviousOne)
                                        {
                                            stream.Position += 1;
                                            this.splitpreviousOne = false;
                                        }

                                        if ((lengthValue / 2) != 0)
                                        {
                                            pstringII = new PtypString(lengthValue / 2);
                                            pstringII.Parse(stream);
                                        }
                                        else
                                        {
                                            pstringII = null;
                                        }

                                        if (lengthValue % 2 != 0)
                                        {
                                            stream.Position += 1;
                                        }
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }
                                else
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstringII = new PtypString(lengthValue / 2);
                                        pstringII.Parse(stream);
                                    }
                                    else
                                    {
                                        pstringII = null;
                                    }
                                }

                                this.ValueArray = pstringII;
                                break;
                            }

                        case CodePageType.PtypCodePageUnicodeBigendian:
                        case CodePageType.PtypCodePageWesternEuropean:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pdstring8 = new PtypString8(lengthValue);
                            pdstring8.Parse(stream);
                            this.ValueArray = pdstring8;
                            break;
                    }
                }
                else
                {
                    switch ((PropertyDataType)typeValue)
                    {
                        case PropertyDataType.PtypString:
                            PtypString pstring = new PtypString();

                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;

                                if (lengthValue != 0)
                                {
                                    if (this.splitpreviousOne)
                                    {
                                        stream.Position += 1;
                                        this.splitpreviousOne = false;
                                    }

                                    if ((lengthValue / 2) != 0)
                                    {
                                        pstring = new PtypString(lengthValue / 2);
                                        pstring.Parse(stream);
                                    }
                                    else
                                    {
                                        pstring = null;
                                    }

                                    if (lengthValue % 2 != 0)
                                    {
                                        stream.Position += 1;
                                    }
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }
                            else
                            {
                                if (splitpreviousOne)
                                {
                                    stream.Position += 1;
                                    splitpreviousOne = false;
                                }

                                if ((lengthValue / 2) != 0)
                                {
                                    pstring = new PtypString(lengthValue / 2);
                                    pstring.Parse(stream);
                                }
                                else
                                {
                                    pstring = null;
                                }
                            }

                            this.ValueArray = pstring;
                            break;
                        case PropertyDataType.PtypString8:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            PtypString8 pstring8 = new PtypString8(lengthValue);
                            pstring8.Parse(stream);
                            this.ValueArray = pstring8;
                            break;
                        case PropertyDataType.PtypBinary:
                        case PropertyDataType.PtypServerId:
                        case PropertyDataType.PtypObject_Or_PtypEmbeddedTable:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                        default:
                            if ((stream.Length - stream.Position) < lengthValue)
                            {
                                MapiInspector.MAPIInspector.PartialPutExtendRemainSize = lengthValue - (int)(stream.Length - stream.Position);
                                this.plength = (int)(stream.Length - stream.Position);
                                lengthValue = this.plength;
                            }

                            this.ValueArray = stream.ReadBlock(lengthValue);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// multi-valued property type PropValue
    /// </summary>
    public class MvPropTypePropValuePutExtendPartial : PropValue
    {
        /// <summary>
        /// This represent the length variable.
        /// </summary>
        public int? Length;

        /// <summary>
        /// A list of fixed size values.
        /// </summary>
        public byte[][] FixedSizeValueList;

        /// <summary>
        /// A list of LengthOfBlock.
        /// </summary>
        public LengthOfBlock[] VarSizeValueList;

        /// <summary>
        /// Length for partial
        /// </summary>
        private int Plength;

        /// <summary>
        /// Initializes a new instance of the MvPropTypePropValuePutExtendPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public MvPropTypePropValuePutExtendPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse next object from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        public override void Parse(FastTransferStream stream)
        {
            base.Parse(stream);

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutExtendType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutExtendType != 0 && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.ptype = MapiInspector.MAPIInspector.PartialPutExtendType;

                    if (MapiInspector.MAPIInspector.PartialPutExtendRemainSize != -1)
                    {
                        this.Plength = MapiInspector.MAPIInspector.PartialPutExtendRemainSize;
                        MapiInspector.MAPIInspector.PartialPutExtendRemainSize = -1;
                    }
                    else
                    {
                        this.Length = stream.ReadInt32();
                    }

                    // clear
                    MapiInspector.MAPIInspector.PartialPutExtendType = 0;
                    MapiInspector.MAPIInspector.PartialPutExtendId = 0;

                    if (MapiInspector.MAPIInspector.PartialPutExtendRemainSize == -1 && MapiInspector.MAPIInspector.PartialPutExtendSubRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendClientInfo = string.Empty;
                    }
                }
                else
                {
                    this.Length = stream.ReadInt32();
                }

                int lengthValue;
                ushort typeValue;

                if (this.Length != null)
                {
                    lengthValue = (int)this.Length;
                }
                else
                {
                    lengthValue = this.Plength;
                }

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.ptype;
                }

                switch ((PropertyDataType)this.PropType)
                {
                    case PropertyDataType.PtypMultipleInteger16:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 2, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating32:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 4, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleFloating64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleCurrency:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleFloatingTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleInteger64:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleTime:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, 8, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleGuid:
                        this.FixedSizeValueList = stream.ReadBlocksPartial(lengthValue, Guid.Empty.ToByteArray().Length, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleBinary:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleString:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, false);
                        break;
                    case PropertyDataType.PtypMultipleString8:
                        this.VarSizeValueList = stream.ReadLengthBlocksPartial(lengthValue, typeValue, false, false);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// The NamedPropInfo class.
    /// </summary>
    public class NamedPropInfo : LexicalBase
    {
        /// <summary>
        /// The PropertySet item in lexical definition.
        /// </summary>
        public AnnotatedGuid PropertySet;

        /// <summary>
        /// The flag variable.
        /// </summary>
        public byte Flag;

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
            this.PropertySet = new AnnotatedGuid(stream);
            int tmp = stream.ReadByte();
            if (tmp > 0)
            {
                this.Flag = (byte)tmp;
            }
        }
    }

    /// <summary>
    /// Represents a NamedPropInfo has a Dispid.
    /// </summary>
    public class DispidNamedPropInfo : NamedPropInfo
    {
        /// <summary>
        /// The Dispid in lexical definition.
        /// </summary>
        public AnnotatedUint Dispid;

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
            this.Dispid = new AnnotatedUint(stream);
            var namedProp = NamedProperty.Lookup(this.PropertySet.value, Dispid.value);
            if (namedProp != null)
                Dispid.ParsedValue = $"{namedProp.Name} = 0x{Dispid.value:X4}";
            else
                Dispid.ParsedValue = $"0x{Dispid.value:X4}";
        }
    }

    /// <summary>
    /// The NameNamedPropInfo class.
    /// </summary>
    public class NameNamedPropInfo : NamedPropInfo
    {
        /// <summary>
        /// The name of the NamedPropInfo.
        /// </summary>
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

    #region 2.2.4.2 FastTransfer stream syntactical structure
    /// <summary>
    /// Base class for all syntactical object.
    /// </summary>
    public abstract class SyntacticalBase
    {
        /// <summary>
        /// The size of an MetaTag value.
        /// </summary>
        protected const int MetaLength = 4;

        /// <summary>
        /// Previous position.
        /// </summary>
        private long previousPosition;

        /// <summary>
        /// Initializes a new instance of the SyntacticalBase class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        protected SyntacticalBase(FastTransferStream stream)
        {
            this.previousPosition = stream.Position;

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
        /// <summary>
        /// A list of PropValue objects.
        /// </summary>
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
        /// Verify that a stream's current position contains a serialized PropList.
        /// </summary>
        /// <param name="stream">A FastTransferStream</param>
        /// <returns>If the stream's current position contains a serialized PropList, return true, else false.</returns>
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
            List<PropValue> propValuesList = new List<PropValue>();

            while (PropValue.Verify(stream))
            {
                propValuesList.Add(PropValue.ParseFrom(stream) as PropValue);
            }

            this.PropValues = propValuesList.ToArray();
        }
    }

    /// <summary>
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValue : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public ushort PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public ushort PropID;

        /// <summary>
        /// The property value.
        /// </summary>
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

            if (this.PropID != 0x4011 && this.PropID != 0x4008)
            {
                this.PropValue = stream.ReadUInt32();
            }
            else
            {
                if (this.PropID != 0x4011)
                {
                    FolderReplicaInfo folderReplicaInfo = new FolderReplicaInfo();
                    folderReplicaInfo.Parse(stream);
                    this.PropValue = folderReplicaInfo;
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
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValueGetPartial : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public PropertyDataType? PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public PidTagPropertyEnum? PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public object PropValue;

        /// <summary>
        /// The property type for partial split.
        /// </summary>
        private ushort propertyType;

        /// <summary>
        /// The property id for partial split.
        /// </summary>
        private ushort propertyID;

        /// <summary>
        /// The length value is for ptypBinary
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the MetaPropValueGetPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValueGetPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIInspector.PartialGetType == 0 || (MapiInspector.MAPIInspector.PartialGetType != 0 && !(MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                this.PropType = (PropertyDataType)stream.ReadUInt16();
                this.PropID = (PidTagPropertyEnum)stream.ReadUInt16();
            }

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialGetType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialGetId = (ushort)this.PropID;
                MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialGetType != 0 && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.propertyType = MapiInspector.MAPIInspector.PartialGetType;
                    this.propertyID = MapiInspector.MAPIInspector.PartialGetId;

                    // clear
                    MapiInspector.MAPIInspector.PartialGetType = 0;
                    MapiInspector.MAPIInspector.PartialGetId = 0;

                    if (MapiInspector.MAPIInspector.PartialGetRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialGetServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialGetClientInfo = string.Empty;
                    }
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.propertyType;
                }

                if (this.PropID != null)
                {
                    identifyValue = (ushort)this.PropID;
                }
                else
                {
                    identifyValue = this.propertyID;
                }

                if (identifyValue != 0x4011 && identifyValue != 0x4008)
                {
                    this.PropValue = stream.ReadUInt32();
                }
                else if (identifyValue == 0x4011)
                {
                    PtypBinary ptypeBinary = new PtypBinary(CountWideEnum.fourBytes);

                    if (!stream.IsEndOfStream)
                    {
                        long spositon = stream.Position;

                        if (MapiInspector.MAPIInspector.PartialGetRemainSize != -1 && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                            && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            this.length = MapiInspector.MAPIInspector.PartialGetRemainSize;

                            // clear
                            MapiInspector.MAPIInspector.PartialGetRemainSize = -1;
                            MapiInspector.MAPIInspector.PartialGetServerUrl = string.Empty;
                            MapiInspector.MAPIInspector.PartialGetProcessName = string.Empty;
                            MapiInspector.MAPIInspector.PartialGetClientInfo = string.Empty;
                        }
                        else
                        {
                            this.length = stream.ReadInt32();
                        }

                        if ((stream.Length - stream.Position) < this.length)
                        {
                            MapiInspector.MAPIInspector.PartialGetType = typeValue;
                            MapiInspector.MAPIInspector.PartialGetId = identifyValue;
                            MapiInspector.MAPIInspector.PartialGetRemainSize = this.length - (int)(stream.Length - stream.Position);
                            MapiInspector.MAPIInspector.PartialGetServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIInspector.PartialGetProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                            MapiInspector.MAPIInspector.PartialGetClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];

                            if (spositon != stream.Position)
                            {
                                // the length value is from the previous RopBuffer
                                ptypeBinary.Count = (int)(stream.Length - stream.Position);
                            }

                            ptypeBinary.Value = stream.ReadBlock(this.length);
                        }
                        else
                        {
                            stream.Position -= 4;
                            ptypeBinary.Parse(stream);
                        }

                        this.PropValue = ptypeBinary;
                    }
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
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValuePutPartial : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public ushort? PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public ushort? PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public object PropValue;

        /// <summary>
        /// The property type for partial split.
        /// </summary>
        private ushort propertyType;

        /// <summary>
        /// The property id for partial split.
        /// </summary>
        private ushort propertyID;

        /// <summary>
        /// The length value is for ptypBinary
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the MetaPropValuePutPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValuePutPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIInspector.PartialPutType == 0 || (MapiInspector.MAPIInspector.PartialPutType != 0 && !(MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                this.PropType = stream.ReadUInt16();
                this.PropID = stream.ReadUInt16();
            }

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutId = (ushort)this.PropID;
                MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutType != 0 && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.propertyType = MapiInspector.MAPIInspector.PartialPutType;
                    this.propertyID = MapiInspector.MAPIInspector.PartialPutId;

                    // clear
                    MapiInspector.MAPIInspector.PartialPutType = 0;
                    MapiInspector.MAPIInspector.PartialPutId = 0;

                    if (MapiInspector.MAPIInspector.PartialPutRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialPutServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutClientInfo = string.Empty;
                    }
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.propertyType;
                }

                if (this.PropID != null)
                {
                    identifyValue = (ushort)this.PropID;
                }
                else
                {
                    identifyValue = this.propertyID;
                }

                if (identifyValue != 0x4011 && identifyValue != 0x4008)
                {
                    this.PropValue = stream.ReadUInt32();
                }
                else if (identifyValue == 0x4011)
                {
                    PtypBinary ptypeBinary = new PtypBinary(CountWideEnum.fourBytes);

                    if (!stream.IsEndOfStream)
                    {
                        long spositon = stream.Position;

                        if (MapiInspector.MAPIInspector.PartialPutRemainSize != -1 && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                            && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            this.length = MapiInspector.MAPIInspector.PartialPutRemainSize;

                            // clear
                            MapiInspector.MAPIInspector.PartialPutRemainSize = -1;
                            MapiInspector.MAPIInspector.PartialPutServerUrl = string.Empty;
                            MapiInspector.MAPIInspector.PartialPutProcessName = string.Empty;
                            MapiInspector.MAPIInspector.PartialPutClientInfo = string.Empty;
                        }
                        else
                        {
                            this.length = stream.ReadInt32();
                        }

                        if ((stream.Length - stream.Position) < this.length)
                        {
                            MapiInspector.MAPIInspector.PartialPutType = typeValue;
                            MapiInspector.MAPIInspector.PartialPutId = identifyValue;
                            MapiInspector.MAPIInspector.PartialPutRemainSize = this.length - (int)(stream.Length - stream.Position);
                            MapiInspector.MAPIInspector.PartialPutServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIInspector.PartialPutProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                            MapiInspector.MAPIInspector.PartialPutClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];

                            if (spositon != stream.Position)
                            {
                                ptypeBinary.Count = (int)(stream.Length - stream.Position);
                            }

                            ptypeBinary.Value = stream.ReadBlock(this.length);
                        }
                        else
                        {
                            stream.Position -= 4;
                            ptypeBinary.Parse(stream);
                        }

                        this.PropValue = ptypeBinary;
                    }
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
    /// The MetaPropValue represents identification information and the value of the Meta property.
    /// </summary>
    public class MetaPropValuePutExtendPartial : SyntacticalBase
    {
        /// <summary>
        /// The property type.
        /// </summary>
        public ushort? PropType;

        /// <summary>
        /// The property id.
        /// </summary>
        public ushort? PropID;

        /// <summary>
        /// The property value.
        /// </summary>
        public object PropValue;

        /// <summary>
        /// The property type for partial split.
        /// </summary>
        private ushort propertyType;

        /// <summary>
        /// The property id for partial split.
        /// </summary>
        private ushort propertyID;

        /// <summary>
        /// The length value is for ptypBinary
        /// </summary>
        private int length;

        /// <summary>
        /// Initializes a new instance of the MetaPropValuePutExtendPartial class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public MetaPropValuePutExtendPartial(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse MetaPropValue from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIInspector.PartialPutExtendType == 0 || (MapiInspector.MAPIInspector.PartialPutType != 0 && !(MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])))
            {
                this.PropType = stream.ReadUInt16();
                this.PropID = stream.ReadUInt16();
            }

            if (stream.IsEndOfStream)
            {
                MapiInspector.MAPIInspector.PartialPutExtendType = (ushort)this.PropType;
                MapiInspector.MAPIInspector.PartialPutExtendId = (ushort)this.PropID;
                MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];
            }
            else
            {
                if (MapiInspector.MAPIInspector.PartialPutExtendType != 0 && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                {
                    this.propertyType = MapiInspector.MAPIInspector.PartialPutExtendType;
                    this.propertyID = MapiInspector.MAPIInspector.PartialPutExtendId;

                    // clear
                    MapiInspector.MAPIInspector.PartialPutExtendType = 0;
                    MapiInspector.MAPIInspector.PartialPutExtendId = 0;

                    if (MapiInspector.MAPIInspector.PartialPutExtendRemainSize == -1)
                    {
                        MapiInspector.MAPIInspector.PartialPutExtendServerUrl = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendProcessName = string.Empty;
                        MapiInspector.MAPIInspector.PartialPutExtendClientInfo = string.Empty;
                    }
                }

                ushort typeValue;
                ushort identifyValue;

                if (this.PropType != null)
                {
                    typeValue = (ushort)this.PropType;
                }
                else
                {
                    typeValue = this.propertyType;
                }

                if (this.PropID != null)
                {
                    identifyValue = (ushort)this.PropID;
                }
                else
                {
                    identifyValue = this.propertyID;
                }

                if (identifyValue != 0x4011 && identifyValue != 0x4008)
                {
                    this.PropValue = stream.ReadUInt32();
                }
                else if (identifyValue == 0x4011)
                {
                    PtypBinary ptypeBinary = new PtypBinary(CountWideEnum.fourBytes);

                    if (!stream.IsEndOfStream)
                    {
                        long spositon = stream.Position;

                        if (MapiInspector.MAPIInspector.PartialPutExtendRemainSize != -1 && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                            && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
                        {
                            this.length = MapiInspector.MAPIInspector.PartialPutExtendRemainSize;

                            // clear
                            MapiInspector.MAPIInspector.PartialPutExtendRemainSize = -1;
                            MapiInspector.MAPIInspector.PartialPutExtendServerUrl = string.Empty;
                            MapiInspector.MAPIInspector.PartialPutExtendProcessName = string.Empty;
                            MapiInspector.MAPIInspector.PartialPutExtendClientInfo = string.Empty;
                        }
                        else
                        {
                            this.length = stream.ReadInt32();
                        }

                        if ((stream.Length - stream.Position) < this.length)
                        {
                            MapiInspector.MAPIInspector.PartialGetType = typeValue;
                            MapiInspector.MAPIInspector.PartialGetId = identifyValue;
                            MapiInspector.MAPIInspector.PartialPutExtendRemainSize = this.length - (int)(stream.Length - stream.Position);
                            MapiInspector.MAPIInspector.PartialPutExtendServerUrl = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath;
                            MapiInspector.MAPIInspector.PartialPutExtendProcessName = MapiInspector.MAPIInspector.ParsingSession.LocalProcess;
                            MapiInspector.MAPIInspector.PartialPutExtendClientInfo = MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"];

                            if (spositon != stream.Position)
                            {
                                ptypeBinary.Count = (int)(stream.Length - stream.Position);
                            }

                            ptypeBinary.Value = stream.ReadBlock(this.length);
                        }
                        else
                        {
                            stream.Position -= 4;
                            ptypeBinary.Parse(stream);
                        }

                        this.PropValue = ptypeBinary;
                    }
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
    public class TransferGetBufferElement : SyntacticalBase
    {
        /// <summary>
        /// MetaTagDnPrefix field
        /// </summary>
        public MetaPropValueGetPartial MetaValue;

        /// <summary>
        /// PropValue field
        /// </summary>
        public PropValue PropValue;

        /// <summary>
        /// Marker field
        /// </summary>
        public object Marker;

        /// <summary>
        /// Initializes a new instance of the TransferGetBufferElement class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TransferGetBufferElement(FastTransferStream stream)
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
            return !stream.IsEndOfStream;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIInspector.PartialGetType != 0 && MapiInspector.MAPIInspector.PartialGetServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialGetProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                && MapiInspector.MAPIInspector.PartialGetClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIInspector.PartialGetId))
                {
                    this.MetaValue = new MetaPropValueGetPartial(stream);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIInspector.PartialGetType) && MapiInspector.MAPIInspector.PartialGetRemainSize == -1)
                    {
                        if (MapiInspector.MAPIInspector.PartialGetType == (ushort)PropertyDataType.PtypInteger32 && MapiInspector.MAPIInspector.PartialGetId == 0x4017)
                        {
                            this.PropValue = new VarPropTypePropValueGetPartial(stream);
                        }
                        else
                        {
                            this.PropValue = new FixedPropTypePropValueGetPartial(stream);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIInspector.PartialGetType)
                    || LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIInspector.PartialGetType) ||
                    (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIInspector.PartialGetType) && MapiInspector.MAPIInspector.PartialGetRemainSize != -1))
                    {
                        this.PropValue = new VarPropTypePropValueGetPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIInspector.PartialGetType))
                    {
                        this.PropValue = new MvPropTypePropValueGetPartial(stream);
                    }
                }
            }
            else
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (MarkersHelper.IsMetaTag(stream.VerifyUInt32()))
                {
                    this.MetaValue = new MetaPropValueGetPartial(stream);
                }
                else
                {
                    long streamPosition = stream.Position;
                    PropValue propertyValue = new PropValue(stream);
                    stream.Position = streamPosition;

                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)propertyValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new FixedPropTypePropValueGetPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)propertyValue.PropType) || PropValue.IsMetaTagIdsetGiven(stream)
                    || LexicalTypeHelper.IsCodePageType((ushort)propertyValue.PropType))
                    {
                        this.PropValue = new VarPropTypePropValueGetPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)propertyValue.PropType))
                    {
                        this.PropValue = new MvPropTypePropValueGetPartial(stream);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferPutBufferElement : SyntacticalBase
    {
        /// <summary>
        /// MetaTagDnPrefix field
        /// </summary>
        public MetaPropValuePutPartial MetaValue;

        /// <summary>
        /// PropValue  field
        /// </summary>
        public PropValue PropValue;

        /// <summary>
        /// Marker field
        /// </summary>
        public object Marker;

        /// <summary>
        /// Initializes a new instance of the TransferPutBufferElement class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TransferPutBufferElement(FastTransferStream stream)
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
            return !stream.IsEndOfStream;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIInspector.PartialPutType != 0 && MapiInspector.MAPIInspector.PartialPutServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                && MapiInspector.MAPIInspector.PartialPutClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIInspector.PartialPutId))
                {
                    this.MetaValue = new MetaPropValuePutPartial(stream);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIInspector.PartialPutType))
                    {
                        if (MapiInspector.MAPIInspector.PartialPutType == (ushort)PropertyDataType.PtypInteger32 && MapiInspector.MAPIInspector.PartialPutId == 0x4017)
                        {
                            this.PropValue = new VarPropTypePropValuePutPartial(stream);
                        }
                        else
                        {
                            this.PropValue = new FixedPropTypePropValuePutPartial(stream);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIInspector.PartialPutType)
                    || LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIInspector.PartialPutType))
                    {
                        this.PropValue = new VarPropTypePropValuePutPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIInspector.PartialPutType))
                    {
                        this.PropValue = new MvPropTypePropValuePutPartial(stream);
                    }
                }
            }
            else
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (MarkersHelper.IsMetaTag(stream.VerifyUInt32()))
                {
                    this.MetaValue = new MetaPropValuePutPartial(stream);
                }
                else
                {
                    long streamPosition = stream.Position;
                    PropValue propValue = new PropValue(stream);
                    stream.Position = streamPosition;

                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new FixedPropTypePropValuePutPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)propValue.PropType) || PropValue.IsMetaTagIdsetGiven(stream)
                    || LexicalTypeHelper.IsCodePageType((ushort)propValue.PropType))
                    {
                        this.PropValue = new VarPropTypePropValuePutPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new MvPropTypePropValuePutPartial(stream);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TransferPutBufferExtendElement : SyntacticalBase
    {
        /// <summary>
        /// MetaTagDnPrefix field
        /// </summary>
        public MetaPropValuePutExtendPartial MetaValue;

        /// <summary>
        /// PropValue field
        /// </summary>
        public PropValue PropValue;

        /// <summary>
        /// Marker field
        /// </summary>
        public object Marker;

        /// <summary>
        /// Initializes a new instance of the TransferPutBufferExtendElement class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public TransferPutBufferExtendElement(FastTransferStream stream)
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
            return !stream.IsEndOfStream;
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (MapiInspector.MAPIInspector.PartialPutExtendType != 0 && MapiInspector.MAPIInspector.PartialPutExtendServerUrl == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath && MapiInspector.MAPIInspector.PartialPutExtendProcessName == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                && MapiInspector.MAPIInspector.PartialPutExtendClientInfo == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"])
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (LexicalTypeHelper.IsMetaPropertyID(MapiInspector.MAPIInspector.PartialPutExtendId))
                {
                    this.MetaValue = new MetaPropValuePutExtendPartial(stream);
                }
                else
                {
                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)MapiInspector.MAPIInspector.PartialPutExtendType))
                    {
                        if (MapiInspector.MAPIInspector.PartialPutExtendType == (ushort)PropertyDataType.PtypInteger32 && MapiInspector.MAPIInspector.PartialPutExtendId == 0x4017)
                        {
                            this.PropValue = new VarPropTypePropValuePutExtendPartial(stream);
                        }
                        else
                        {
                            this.PropValue = new FixedPropTypePropValuePutExtendPartial(stream);
                        }
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)MapiInspector.MAPIInspector.PartialPutExtendType)
                    || LexicalTypeHelper.IsCodePageType(MapiInspector.MAPIInspector.PartialPutExtendType))
                    {
                        this.PropValue = new VarPropTypePropValuePutExtendPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)MapiInspector.MAPIInspector.PartialPutExtendType))
                    {
                        this.PropValue = new MvPropTypePropValuePutExtendPartial(stream);
                    }
                }
            }
            else
            {
                if (MarkersHelper.IsMarker(stream.VerifyUInt32()))
                {
                    this.Marker = stream.ReadMarker();
                }
                else if (MarkersHelper.IsMetaTag(stream.VerifyUInt32()))
                {
                    this.MetaValue = new MetaPropValuePutExtendPartial(stream);
                }
                else
                {
                    long streamPosition = stream.Position;
                    PropValue propValue = new PropValue(stream);
                    stream.Position = streamPosition;

                    if (LexicalTypeHelper.IsFixedType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new FixedPropTypePropValuePutExtendPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsVarType((PropertyDataType)propValue.PropType) || PropValue.IsMetaTagIdsetGiven(stream)
                    || LexicalTypeHelper.IsCodePageType((ushort)propValue.PropType))
                    {
                        this.PropValue = new VarPropTypePropValuePutExtendPartial(stream);
                    }
                    else if (LexicalTypeHelper.IsMVType((PropertyDataType)propValue.PropType) && !PropValue.IsMetaTagIdsetGiven(stream))
                    {
                        this.PropValue = new MvPropTypePropValuePutExtendPartial(stream);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class TopFolder : SyntacticalBase
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// The start marker of TopFolder.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A FolderContentNoDelProps value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContentNoDelProps FolderContentNoDelProps;

        /// <summary>
        /// The end marker of TopFolder.
        /// </summary>
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
            return stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || stream.VerifyMarker(Markers.StartTopFld);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

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
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// Contains the properties of the Folder object, which are possibly affected by property filters.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MetaTagNewFXFolder property.
        /// </summary>
        public MetaPropValue MetaTagNewFXFolder;

        /// <summary>
        /// The folderMessages element contains the messages contained in a folder.
        /// </summary>
        public FolderMessages FolderMessages;

        /// <summary>
        /// A MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue MetaTagFXDelProp;

        /// <summary>
        /// The subFolders element contains subFolders of a folder.
        /// </summary>
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
            return !stream.IsEndOfStream && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || PropList.Verify(stream));
        }

        /// <summary>
        ///  Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

            this.PropList = new PropList(stream);

            if (!stream.IsEndOfStream)
            {
                List<SubFolder> interSubFolders = new List<SubFolder>();

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
                        interSubFolders.Add(new SubFolder(stream));
                    }

                    this.SubFolders = interSubFolders.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// The folderContentNoDelProps element contains the content of a folder: its properties, messages, and subFolders.
    /// </summary>
    public class FolderContentNoDelProps : SyntacticalBase
    {
        /// <summary>
        /// Contains the properties of the Folder object, which are possibly affected by property filters.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MetaTagNewFXFolder property.
        /// </summary>
        public MetaPropValue MetaTagNewFXFolder;

        /// <summary>
        /// The FolderMessagesNoDelProps element contains the messages contained in a folder.
        /// </summary>
        public FolderMessagesNoDelProps FolderMessagesNoDelProps;

        /// <summary>
        /// A MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue MetaTagFXDelProp;

        /// <summary>
        /// The subFolders element contains subFolders of a folder.
        /// </summary>
        public SubFolderNoDelProps[] SubFolderNoDelPropList;

        /// <summary>
        /// Initializes a new instance of the FolderContentNoDelProps class.
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
            return !stream.IsEndOfStream && PropList.Verify(stream);
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
                List<SubFolderNoDelProps> interSubFolders = new List<SubFolderNoDelProps>();

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
                        interSubFolders.Add(new SubFolderNoDelProps(stream));
                    }

                    this.SubFolderNoDelPropList = interSubFolders.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// Contains a folderContent.
    /// </summary>
    public class SubFolder : SyntacticalBase
    {
        /// <summary>
        /// The start marker of SubFolder.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A folderContent value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContent FolderContent;

        /// <summary>
        /// The end marker of SubFolder.
        /// </summary>
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
        /// <summary>
        /// The start marker of SubFolder.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A folderContentNoDelProps value contains the content of a folder: its properties, messages, and subFolders.
        /// </summary>
        public FolderContentNoDelProps FolderContentNoDelProps;

        /// <summary>
        /// The end marker of SubFolder.
        /// </summary>
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
                this.FolderContentNoDelProps = new FolderContentNoDelProps(stream);

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
        /// <summary>
        /// A list of MetaTagFxDelPropMessageList.
        /// </summary>
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
            List<MetaTagFxDelPropMessageList> interMessageLists = new List<MetaTagFxDelPropMessageList>();

            while (!stream.IsEndOfStream && count < 2)
            {
                if (MetaTagFxDelPropMessageList.Verify(stream))
                {
                    interMessageLists.Add(new MetaTagFxDelPropMessageList(stream));
                }
                else
                {
                    break;
                }

                count++;
            }

            this.MetaTagFxDelPropMessageLists = interMessageLists.ToArray();
        }
    }

    /// <summary>
    /// The MetaTagFxDelPropMessageList is used to parse folderMessages class.
    /// </summary>
    public class MetaTagFxDelPropMessageList : SyntacticalBase
    {
        /// <summary>
        /// A MetaTagFXDelProp property. 
        /// </summary>
        public MetaPropValue MetaTagFXDelProp;

        /// <summary>
        /// A list of messageList.
        /// </summary>
        public MessageList MessageLists;

        /// <summary>
        /// Initializes a new instance of the MetaTagFxDelPropMessageList class.
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
        /// <summary>
        /// A list of MessageList.
        /// </summary>
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
            List<MessageList> interMessageLists = new List<MessageList>();

            while (!stream.IsEndOfStream && count < 2)
            {
                if (MessageList.Verify(stream))
                {
                    interMessageLists.Add(new MessageList(stream));
                }
                else
                {
                    break;
                }

                count++;
            }

            this.MessageLists = interMessageLists.ToArray();
        }
    }

    /// <summary>
    /// The message element represents a Message object.
    /// </summary>
    public class Message : SyntacticalBase
    {
        /// <summary>
        /// The start marker of message.
        /// </summary>
        public Markers? StartMarker1;

        /// <summary>
        /// The start marker of message.
        /// </summary>
        public Markers? StartMarker2;

        /// <summary>
        /// A MessageContent value.Represents the content of a message: its properties, the recipients, and the attachments.
        /// </summary>
        public MessageContent Content;

        /// <summary>
        /// The end marker of message.
        /// </summary>
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
                {
                    this.StartMarker1 = Markers.StartMessage;
                }
                else
                {
                    this.StartMarker2 = Markers.StartFAIMsg;
                }

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
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Represents children of the Message objects: Recipient and Attachment objects.
        /// </summary>
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
            return !stream.IsEndOfStream && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || PropList.Verify(stream));
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

            this.PropList = new PropList(stream);
            this.MessageChildren = new MessageChildren(stream);
        }
    }

    /// <summary>
    /// The MessageChildren element represents children of the Message objects: Recipient and Attachment objects.
    /// </summary>
    public class MessageChildren : SyntacticalBase
    {
        /// <summary>
        /// A MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue FxdelPropsBeforeRecipient;

        /// <summary>
        /// A list of recipients.
        /// </summary>
        public Recipient[] Recipients;

        /// <summary>
        /// Another MetaTagFXDelProp property.
        /// </summary>
        public MetaPropValue FxdelPropsBeforeAttachment;

        /// <summary>
        /// A list of attachments.
        /// </summary>
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
            List<Attachment> interAttachments = new List<Attachment>();
            List<Recipient> interRecipients = new List<Recipient>();

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
            {
                this.FxdelPropsBeforeRecipient = new MetaPropValue(stream);
            }

            if (Recipient.Verify(stream))
            {
                interRecipients = new List<Recipient>();

                while (Recipient.Verify(stream))
                {
                    interRecipients.Add(new Recipient(stream));
                }
            }

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagFXDelProp))
            {
                this.FxdelPropsBeforeAttachment = new MetaPropValue(stream);
            }

            while (Attachment.Verify(stream))
            {
                interAttachments.Add(new Attachment(stream));
            }

            this.Attachments = interAttachments.ToArray();
            this.Recipients = interRecipients.ToArray();
        }
    }

    /// <summary>
    /// The Recipient element represents a Recipient object, which is a subobject of the Message object.
    /// </summary>
    public class Recipient : SyntacticalBase
    {
        /// <summary>
        /// The start marker of Recipient.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// The end marker of Recipient.
        /// </summary>
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
        /// <summary>
        /// The  start marker of an attachment object.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PidTagAttachNumber property.
        /// </summary>
        public FixedPropTypePropValue PidTagAttachNumber;

        /// <summary>
        /// Attachment content.
        /// </summary>
        public AttachmentContent AttachmentContent;

        /// <summary>
        /// The end marker of an attachment object.
        /// </summary>
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
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// An EmbeddedMessage value.
        /// </summary>
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
            return !stream.IsEndOfStream && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix || PropList.Verify(stream));
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

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
        /// <summary>
        /// The start marker of the EmbeddedMessage.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A MessageContent value represents the content of a message: its properties, the recipients, and the attachments.
        /// </summary>
        public MessageContent MessageContent;

        /// <summary>
        /// The end marker of the EmbeddedMessage.
        /// </summary>
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
        /// <summary>
        /// A list of MetaTagMessage objects.
        /// </summary>
        public MetaTagMessage[] MetaTagMessages;

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
            return MetaTagMessage.Verify(stream);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<MetaTagMessage> interMessageList = new List<MetaTagMessage>();

            while (Verify(stream))
            {
                interMessageList.Add(new MetaTagMessage(stream));
            }

            this.MetaTagMessages = interMessageList.ToArray();
        }
    }

    /// <summary>
    /// The MetaTagEcWaringMessage is used to parse MessageList class.
    /// </summary>
    public class MetaTagMessage : SyntacticalBase
    {
        /// <summary>
        /// The MetaTagDnPrefix
        /// </summary>
        public MetaPropValue MetaTagDnPrefix;

        /// <summary>
        /// MetaTagEcWaring indicates a MetaTagEcWaring property.
        /// </summary>
        public MetaPropValue MetaTagEcWaring;

        /// <summary>
        /// Message indicates a Message object.
        /// </summary>
        public Message Message;

        /// <summary>
        /// Initializes a new instance of the MetaTagMessage class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public MetaTagMessage(FastTransferStream stream)
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
                && (stream.VerifyUInt32() == (uint)MetaProperties.MetaTagDnPrefix
                || stream.VerifyUInt32() == (uint)MetaProperties.MetaTagEcWarning
                || Message.Verify(stream));
        }

        /// <summary>
        /// Parse MetaTagEcWaringMessage from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            if (stream.VerifyMetaProperty(MetaProperties.MetaTagDnPrefix))
            {
                this.MetaTagDnPrefix = new MetaPropValue(stream);
            }

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
        /// <summary>
        /// The start marker of Deletions.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
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
        /// <summary>
        /// The start marker of FolderChange.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
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
        /// <summary>
        /// The start marker of GroupInfo.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// The propertyTag for ProgressInformation.
        /// </summary>
        public uint PropertiesTag;

        /// <summary>
        /// The count of the PropList.
        /// </summary>
        public uint PropertiesLength;

        /// <summary>
        /// A PropList value.
        /// </summary>
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
                this.PropertiesTag = stream.ReadUInt32();
                this.PropertiesLength = stream.ReadUInt32();
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
        /// <summary>
        /// The start marker of ProgressPerMessage.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
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
        /// <summary>
        /// The start marker of progressTotal.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// The propertyTag for ProgressInformation.
        /// </summary>
        public uint PropertiesTag;

        /// <summary>
        /// The count of the PropList.
        /// </summary>
        public uint PropertiesLength;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public ProgressInformation PropList;

        /// <summary>
        /// Initializes a new instance of the ProgressTotal class.
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
                this.PropertiesTag = stream.ReadUInt32();
                this.PropertiesLength = stream.ReadUInt32();
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
        /// <summary>
        /// The start marker of ReadStateChange.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// Initializes a new instance of the ReadStateChanges class.
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
        /// <summary>
        /// The start marker of ReadStateChange.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// The end marker of ReadStateChange.
        /// </summary>
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
        /// <summary>
        /// A ProgressTotal value
        /// </summary>
        public ProgressTotal ProgressTotal;

        /// <summary>
        /// A list of ProgressPerMessageChange value
        /// </summary>
        public ProgressPerMessageChange[] ProgressPerMessageChanges;

        /// <summary>
        /// A Deletions value
        /// </summary>
        public Deletions Deletions;

        /// <summary>
        /// A readStateChanges value.
        /// </summary>
        public ReadStateChanges ReadStateChanges;

        /// <summary>
        /// A state value.
        /// </summary>
        public State State;

        /// <summary>
        /// A end marker of ContentSync.
        /// </summary>
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
                && stream.VerifyMarker(Markers.IncrSyncEnd, (int)stream.Length - 4 - (int)stream.Position);
        }

        /// <summary>
        /// Parse fields from a FastTransferStream.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public override void Parse(FastTransferStream stream)
        {
            List<ProgressPerMessageChange> interProgressPerMessageChanges = new List<ProgressPerMessageChange>();

            if (ProgressTotal.Verify(stream))
            {
                this.ProgressTotal = new ProgressTotal(stream);
            }

            while (ProgressPerMessageChange.Verify(stream))
            {
                interProgressPerMessageChanges.Add(new ProgressPerMessageChange(stream));
            }

            this.ProgressPerMessageChanges = interProgressPerMessageChanges.ToArray();

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
    /// The ProgressPerMessageChange is used to parse ContentSync class.
    /// </summary>
    public class ProgressPerMessageChange : SyntacticalBase
    {
        /// <summary>
        /// A ProgressPerMessage value.
        /// </summary>
        public ProgressPerMessage ProgressPerMessage;

        /// <summary>
        /// A MessageChange value.
        /// </summary>
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
        /// <summary>
        /// A list of FolderChange value.
        /// </summary>
        public FolderChange[] FolderChanges;

        /// <summary>
        /// A Deletions value.
        /// </summary>
        public Deletions Deletions;

        /// <summary>
        /// The State value.
        /// </summary>
        public State State;

        /// <summary>
        /// The end marker of hierarchySync.
        /// </summary>
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
            List<FolderChange> interFolderChanges = new List<FolderChange>();

            while (FolderChange.Verify(stream))
            {
                interFolderChanges.Add(new FolderChange(stream));
            }

            this.FolderChanges = interFolderChanges.ToArray();

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
    /// The MessageChange element contains information for the changed messages.
    /// </summary>
    public class MessageChange : SyntacticalBase
    {
        /// <summary>
        /// A MessageChangeFull value.
        /// </summary>
        public MessageChangeFull MessageChangeFull;

        /// <summary>
        /// A MessageChangePartial value.
        /// </summary>
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
        /// <summary>
        /// A start marker for MessageChangeFull.
        /// </summary>
        public Markers StartMarker;

        /// <summary>
        /// A MessageChangeHeader value.
        /// </summary>
        public PropList MessageChangeHeader;

        /// <summary>
        /// A second marker for MessageChangeFull.
        /// </summary>
        public Markers SecondMarker;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

        /// <summary>
        /// A MessageChildren value.
        /// </summary>
        public MessageChildren MessageChildren;

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
                this.MessageChangeHeader = new PropList(stream);

                if (stream.ReadMarker() == Markers.IncrSyncMessage)
                {
                    this.SecondMarker = Markers.IncrSyncMessage;
                    this.PropList = new PropList(stream);
                    this.MessageChildren = new MessageChildren(stream);
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
        /// <summary>
        /// A groupInfo value.
        /// </summary>
        public GroupInfo GroupInfo;

        /// <summary>
        /// A MetaTagIncrSyncGroupId property.
        /// </summary>
        public MetaPropValue MetaTagIncrSyncGroupId;

        /// <summary>
        /// The MessageChangePartial marker.
        /// </summary>
        public Markers Marker;

        /// <summary>
        /// A MessageChangeHeader value.
        /// </summary>
        public PropList MessageChangeHeader;

        /// <summary>
        /// A list of SyncMessagePartialPropList values.
        /// </summary>
        public SyncMessagePartialPropList[] SyncMessagePartialPropList;

        /// <summary>
        /// A MessageChildren field.
        /// </summary>
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
            List<SyncMessagePartialPropList> interMessagePartialList = new List<SyncMessagePartialPropList>();
            this.GroupInfo = new GroupInfo(stream);

            if (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrSyncGroupId))
            {
                this.MetaTagIncrSyncGroupId = new MetaPropValue(stream);
            }

            if (stream.ReadMarker() == Markers.IncrSyncChgPartial)
            {
                this.Marker = Markers.IncrSyncChgPartial;
                this.MessageChangeHeader = new PropList(stream);

                while (stream.VerifyMetaProperty(MetaProperties.MetaTagIncrementalSyncMessagePartial))
                {
                    interMessagePartialList.Add(new SyncMessagePartialPropList(stream));
                }

                this.SyncMessagePartialPropList = interMessagePartialList.ToArray();
                this.MessageChildren = new MessageChildren(stream);
            }
            else
            {
                throw new Exception("The MessageChangePartial cannot be parsed successfully. The IncrSyncChgPartial Marker is missed.");
            }
        }
    }

    /// <summary>
    /// The SyncMessagePartialPropList is used to parse MessageChangePartial element.
    /// </summary>
    public class SyncMessagePartialPropList : SyntacticalBase
    {
        /// <summary>
        /// A MetaTagIncrementalSyncMessagePartial property.
        /// </summary>
        public MetaPropValue MetaSyncMessagePartial;

        /// <summary>
        /// A PropList value.
        /// </summary>
        public PropList PropList;

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
                this.MetaSyncMessagePartial = new MetaPropValue(stream);
            }

            this.PropList = new PropList(stream);
        }
    }
    #endregion

    #region FastTransfer help
    /// <summary>
    /// Supply help functions for manipulate Markers.
    /// </summary>
    public class MarkersHelper
    {
        /// <summary>
        /// Indicate whether a UInt is a Marker.
        /// </summary>
        /// <param name="marker">The UInts value.</param>
        /// <returns>If is a Marker, return true, else false.</returns>
        public static bool IsMarker(uint marker)
        {
            foreach (Markers ma in Enum.GetValues(typeof(Markers)))
            {
                if ((uint)ma == marker)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Indicate whether a UInt is a MetaProperties.
        /// </summary>
        /// <param name="metaTag">The UInts value.</param>
        /// <returns>If is a MetaProperties, return true, else false.</returns>
        public static bool IsMetaTag(uint metaTag)
        {
            foreach (MetaProperties me in Enum.GetValues(typeof(MetaProperties)))
            {
                if (metaTag == (uint)me)
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
        /// <summary>
        /// Contains fixedPropTypes.
        /// </summary>
        private static List<PropertyDataType> fixedTypes;

        /// <summary>
        /// Contains varPropTypes.
        /// </summary>
        private static List<PropertyDataType> varTypes;

        /// <summary>
        /// Contains mvPropTypes.
        /// </summary>
        private static List<PropertyDataType> mVTypes;

        /// <summary>
        /// Contains CodePageTypes.
        /// </summary>
        private static List<CodePageType> codePageTypes;

        /// <summary>
        /// Contains MetaProperty Ids.
        /// </summary>
        private static List<ushort> metaPropIds;

        /// <summary>
        /// Initializes static members of the LexicalTypeHelper class.
        /// </summary>
        static LexicalTypeHelper()
        {
            fixedTypes = new List<PropertyDataType>
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

            varTypes = new List<PropertyDataType>
            {
                PropertyDataType.PtypString,
                PropertyDataType.PtypString8,
                PropertyDataType.PtypBinary,
                PropertyDataType.PtypServerId,
                PropertyDataType.PtypObject_Or_PtypEmbeddedTable
            };

            mVTypes = new List<PropertyDataType>
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

            codePageTypes = new List<CodePageType>
            {
                CodePageType.PtypCodePageUnicode,
                CodePageType.PtypCodePageUnicodeBigendian,
                CodePageType.PtypCodePageWesternEuropean
            };

            metaPropIds = new List<ushort>
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
            return mVTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a PropertyDataType is either PtypString, PtypString8 or PtypBinary, PtypServerId, or PtypObject. 
        /// </summary>
        /// <param name="type">A PropertyDataType.</param>
        /// <returns>If the PropertyDataType is a either PtypString, PtypString8 or PtypBinary, PtypServerId, or PtypObject return true, else false.</returns>
        public static bool IsVarType(PropertyDataType type)
        {
            return varTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a property type value of any type that has a fixed length.
        /// </summary>
        /// <param name="type">A property type.</param>
        /// <returns>If a property type value of any type that has a fixed length, return true , else return false.</returns>
        public static bool IsFixedType(PropertyDataType type)
        {
            return fixedTypes.Contains(type);
        }

        /// <summary>
        /// Indicate whether a PropertyID is a Meta property ID.
        /// </summary>
        /// <param name="id">A UShort value.</param>
        /// <returns>If a PropertyID is a Meta property ID, return true, else return false.</returns>
        public static bool IsMetaPropertyID(ushort id)
        {
            return metaPropIds.Contains(id);
        }

        /// <summary>
        /// Indicate whether a UShort value is a codePage property type. 
        /// </summary>
        /// <param name="type">A UShort value.</param>
        /// <returns>If the UShort is a either PtypCodePageUnicode, PtypCodePageUnicodeBigendian or PtypCodePageWesternEuropean return true, else false.</returns>
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
        /// <summary>
        /// Specifies the number of blocks
        /// </summary>
        public int TotalSize;

        /// <summary>
        /// Specifies block length
        /// </summary>
        public byte[] BlockSize;

        /// <summary>
        /// Initializes a new instance of the LengthOfBlock class
        /// </summary>
        /// <param name="totalSize">The total size</param>
        /// <param name="blockSize">The block size</param>
        public LengthOfBlock(int totalSize, byte[] blockSize)
        {
            this.TotalSize = totalSize;
            this.BlockSize = blockSize;
        }
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
        /// An UShort.
        /// </summary>
        public ushort Pad;

        /// <summary>
        /// Initializes a new instance of the LongTermId class.
        /// </summary>
        /// <param name="stream">A FastTransferStream.</param>
        public LongTermId(FastTransferStream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Parse the LongTermId structure
        /// </summary>
        /// <param name="stream">The stream to parse</param>
        public override void Parse(FastTransferStream stream)
        {
            this.DatabaseGuid = stream.ReadGuid();
            this.GlobalCounter = BitConverter.ToUInt64(stream.ReadBlock(6), 0);
            this.Pad = stream.ReadUInt16();
        }
    }
    #endregion
}
