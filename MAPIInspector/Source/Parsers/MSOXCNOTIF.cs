namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    #region The class or enum define related to ROPs.

    /// <summary>
    /// The enum value of Notification type.
    /// </summary>
    public enum NotificationTypesEnum : ushort
    {
        /// <summary>
        /// A new email message has been received by the server
        /// </summary>
        NewMail = 0x0002,

        /// <summary>
        /// A new object has been created on the server.
        /// </summary>
        ObjectCreated = 0x0004,

        /// <summary>
        /// An existing object has been deleted from the server
        /// </summary>
        ObjectDeleted = 0x0008,

        /// <summary>
        /// An existing object has been modified on the server
        /// </summary>
        ObjectModified = 0x0010,

        /// <summary>
        /// An existing object has been moved to another location on the server
        /// </summary>
        ObjectMoved = 0x0020,

        /// <summary>
        /// An existing object has been copied on the server.
        /// </summary>
        ObjectCopied = 0x0040,

        /// <summary>
        /// A search operation has been completed on the server
        /// </summary>
        SearchComplete = 0x0080,

        /// <summary>
        /// A table has been modified on the server
        /// </summary>
        TableModified = 0x0100,

        /// <summary>
        /// Extended one
        /// </summary>
        Extended = 0x0400,

        /// <summary>
        /// Other event
        /// </summary>
        NULL = 0x0000
    }

    /// <summary>
    /// The enum value of NotificationData Availability.
    /// </summary>
    [Flags]
    public enum NotificationDataAvailabilityEnum : ushort
    {
        /// <summary>
        /// The notification contains information about a change in the total number of messages in a folder triggering the event
        /// </summary>
        T = 0x1000,

        /// <summary>
        /// The notification contains information about a change in the number of unread messages in a folder triggering the event
        /// </summary>
        U = 0x2000,

        /// <summary>
        /// The notification is caused by an event in a search folder
        /// </summary>
        S = 0x4000,

        /// <summary>
        /// The notification is caused by an event on a message
        /// </summary>
        M = 0x8000,

        /// <summary>
        /// Other value
        /// </summary>
        NULL = 0x0000
    }

    /// <summary>
    /// The enum value of TableEvent type.
    /// </summary>
    public enum TableEventTypeEnum : ushort
    {
        /// <summary>
        /// The notification is for TableChanged events
        /// </summary>
        TableChanged = 0x0001,

        /// <summary>
        /// The notification is for TableRowAdded events.
        /// </summary>
        TableRowAdded = 0x0003,

        /// <summary>
        /// The notification is for TableRowDeleted events.
        /// </summary>
        TableRowDeleted = 0x0004,

        /// <summary>
        /// The notification is for TableRowModified events.
        /// </summary>
        TableRowModified = 0x0005,

        /// <summary>
        /// The notification is for TableRestrictionChanged events
        /// </summary>
        TableRestrictionChanged = 0x0007
    }

    /// <summary>
    /// A class indicates the NotificationFlagsT.
    /// </summary>
    public class NotificationFlagsT : BaseStructure
    {
        /// <summary>
        /// The Notification type.
        /// </summary>
        [BitAttribute(12)]
        public NotificationTypesEnum NotificationType;

        /// <summary>
        /// The NotificationData Availability.
        /// </summary>
        [BitAttribute(4)]
        public NotificationDataAvailabilityEnum NotificationDataAvailability;

        /// <summary>
        /// Parse the NotificationFlagsT structure.
        /// </summary>
        /// <param name="s">A stream containing NotificationFlagsT structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ushort flag = this.ReadUshort();
            this.NotificationDataAvailability = (NotificationDataAvailabilityEnum)(flag & 0xf000);
            this.NotificationType = (NotificationTypesEnum)(flag & 0x0fff);
        }
    }

    /// <summary>
    /// A class indicates the NotificationFlags.
    /// </summary>
    public class NotificationFlags : BaseStructure
    {
        /// <summary>
        /// Notification flag
        /// </summary>
        public NotificationFlagsT Value;

        /// <summary>
        /// Parse the NotificationFlags structure.
        /// </summary>
        /// <param name="s">A stream containing NotificationFlags structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = new NotificationFlagsT();
            this.Value.Parse(s);
        }
    }
    #endregion

    #region 2.2.1.2.1	RopRegisterNotification ROP
    /// <summary>
    /// The RopRegisterNotification ROP ([MS-OXCROPS] section 2.2.14.1) creates a subscription for specified notifications on the server and returns a handle of the subscription to the client. 
    /// </summary>
    public class RopRegisterNotificationRequest : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x29.
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
        /// A flags structure that contains flags that specify the types of events to register for.
        /// </summary>
        public NotificationTypesEnum NotificationTypes;

        /// <summary>
        /// A flags structure. 
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// A Boolean that specifies whether the notification is scoped to the mailbox instead of a specific folder or message.
        /// </summary>
        public bool WantWholeStore;

        /// <summary>
        /// This value specifies the folder to register notifications for
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// This value specifies the message to register notifications for.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopRegisterNotificationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopRegisterNotificationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.NotificationTypes = (NotificationTypesEnum)this.ReadUshort();

            if (this.NotificationTypes == NotificationTypesEnum.Extended)
            {
                this.Reserved = this.ReadByte();
            }

            this.WantWholeStore = this.ReadBoolean();

            if (!this.WantWholeStore)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }
        }
    }

    /// <summary>
    /// A class indicates the RopRegisterNotification ROP Response Buffer.
    /// </summary>
    public class RopRegisterNotificationResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x29.
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
        /// Parse the RopRegisterNotificationResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopRegisterNotificationResponse structure.</param>
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

    #region 2.2.1.3.4	RopPending ROP
    /// <summary>
    /// A class indicates the RopPending ROP Response Buffer.
    /// </summary>
    public class RopPendingResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x6E.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that specifies which session has pending notifications.
        /// </summary>
        public ushort SessionIndex;

        /// <summary>
        /// Parse the RopPendingResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopPendingResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.SessionIndex = this.ReadUshort();
        }
    }
    #endregion

    #region 2.2.1.4.1	RopNotify ROP
    /// <summary>
    /// A class indicates the RopNotify ROP Response Buffer.
    /// </summary>
    public class RopNotifyResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x2A.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// A Server object handle that specifies the notification Server object associated with this notification event.
        /// </summary>
        public uint NotificationHandle;

        /// <summary>
        /// An unsigned integer that specifies the RopLogon associated with this notification event.
        /// </summary>
        public byte LogonId;

        /// <summary>
        /// Various structures
        /// </summary>
        public NotificationData NotificationData;

        /// <summary>
        /// Parse the RopNotifyResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopNotifyResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.NotificationHandle = this.ReadUint();
            this.LogonId = this.ReadByte();
            this.NotificationData = new NotificationData(this.NotificationHandle);
            this.NotificationData.Parse(s);
        }
    }
    #endregion

    #region 2.2.1.4.1.2	NotificationData Structure
    /// <summary>
    /// A class indicates the NotificationData
    /// </summary>
    public class NotificationData : BaseStructure
    {
        /// <summary>
        /// A combination of an enumeration and flags that describe the type of the notification and the availability of the notification data fields.
        /// </summary>
        public NotificationFlags NotificationFlags;

        /// <summary>
        /// A subtype of the notification for a TableModified event.
        /// </summary>
        public TableEventTypeEnum? TableEventType;

        /// <summary>
        /// The value of the Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, of the item triggering the notification. 
        /// </summary>
        public FolderID TableRowFolderID;

        /// <summary>
        /// The value of the Message ID structure, as specified in [MS-OXCDATA] section 2.2.1.2, of the item triggering the notification.
        /// </summary>
        public MessageID TableRowMessageID;

        /// <summary>
        /// An identifier of the instance of the previous row in the table. 
        /// </summary>
        public uint? TableRowInstance;

        /// <summary>
        /// The old value of the Folder ID structure of the item triggering the notification. 
        /// </summary>
        public FolderID InsertAfterTableRowFolderID;

        /// <summary>
        /// The old value of the Message ID structure of the item triggering the notification. 
        /// </summary>
        public MessageID InsertAfterTableRowID;

        /// <summary>
        /// An unsigned 32-bit identifier of the instance of the row where the modified row is inserted. 
        /// </summary>
        public uint? InsertAfterTableRowInstance;

        /// <summary>
        /// An unsigned 16-bit integer that indicates the length of the table row data. 
        /// </summary>
        public ushort? TableRowDataSize;

        /// <summary>
        /// The table row data, which contains a list of property values
        /// </summary>
        public PropertyRow TableRowData;

        /// <summary>
        /// The Folder ID structure of the item triggering the event. 
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// The Message ID structure, as specified in [MS-OXCDATA] section 2.2.1.2, of the item triggering the event. 
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// The Folder ID structure of the parent folder of the item triggering the event.
        /// </summary>
        public FolderID ParentFolderId;

        /// <summary>
        /// The old Folder ID structure of the item triggering the event. 
        /// </summary>
        public FolderID OldFolderId;

        /// <summary>
        /// The old Message ID structure of the item triggering the event. 
        /// </summary>
        public MessageID OldMessageId;

        /// <summary>
        /// The old parent Folder ID structure of the item triggering the event. 
        /// </summary>
        public MessageID OldParentFolderId;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the number of property tags in the Tags field. 
        /// </summary>
        public ushort? TagCount;

        /// <summary>
        /// An array of unsigned 32-bit integers that identifies the IDs of properties that have changed. 
        /// </summary>
        public PropertyTag[] Tags;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the total number of items in the folder triggering this event.
        /// </summary>
        public uint? TotalMessageCount;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the number of unread items in a folder triggering this event. 
        /// </summary>
        public uint? UnreadMessageCount;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the message flags of new mail that has been received
        /// </summary>
        public uint? MessageFlags;

        /// <summary>
        /// A value of TRUE (0x01) indicates the value of the MessageClass field is in Unicode
        /// </summary>
        public byte? UnicodeFlag;

        /// <summary>
        /// A null-terminated string containing the message class of the new mail. 
        /// </summary>
        public MAPIString MessageClass;

        /// <summary>
        /// A Server object handle that specifies the notification Server object associated with this notification event.
        /// </summary>
        private uint notificationHandle;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the NotificationData class
        /// </summary>
        /// <param name="notificationHandle">The object handle in notify operation</param>
        public NotificationData(uint notificationHandle)
        {
            this.notificationHandle = notificationHandle;
        }

        /// <summary>
        /// Get whether the messageID is empty 
        /// </summary>
        /// <param name="messageId">Message ID</param>
        /// <returns>Indicates if messageID is empty</returns>
        public bool IsEmptyMessageID(MessageID messageId)
        {
            if (messageId.ReplicaId != 0)
            {
                return false;
            }

            foreach (var item in messageId.GlobalCounter)
            {
                if (item != 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Parse the NotificationData structure.
        /// </summary>
        /// <param name="s">A stream containing NotificationData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.NotificationFlags = new NotificationFlags();
            this.NotificationFlags.Parse(s);

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified)
            {
                this.TableEventType = (TableEventTypeEnum)this.ReadUshort();
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowDeleted || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowFolderID = new FolderID();
                this.TableRowFolderID.Parse(s);
            }

            if ((((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowDeleted || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowMessageID = new MessageID();
                this.TableRowMessageID.Parse(s);
            }

            if ((((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowDeleted || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowInstance = this.ReadUint();
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.InsertAfterTableRowFolderID = new FolderID();
                this.InsertAfterTableRowFolderID.Parse(s);
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.InsertAfterTableRowID = new MessageID();
                this.InsertAfterTableRowID.Parse(s);
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.InsertAfterTableRowInstance = this.ReadUint();
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowDataSize = this.ReadUshort();
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (this.TableEventType == TableEventTypeEnum.TableRowAdded || this.TableEventType == TableEventTypeEnum.TableRowModified))
            {
                int parsingSessionID = MapiInspector.MAPIInspector.ParsingSession.id;
                if (MapiInspector.MAPIInspector.IsFromFiddlerCore(MapiInspector.MAPIInspector.ParsingSession))
                {
                    parsingSessionID = int.Parse(MapiInspector.MAPIInspector.ParsingSession["VirtualID"]);
                }
                if (!(DecodingContext.Notify_handlePropertyTags.Count > 0 && DecodingContext.Notify_handlePropertyTags.ContainsKey(this.notificationHandle) && DecodingContext.Notify_handlePropertyTags[this.notificationHandle].ContainsKey(parsingSessionID)
                    && DecodingContext.Notify_handlePropertyTags[this.notificationHandle][parsingSessionID].Item1 == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders.RequestPath
                    && DecodingContext.Notify_handlePropertyTags[this.notificationHandle][parsingSessionID].Item2 == MapiInspector.MAPIInspector.ParsingSession.LocalProcess
                    && DecodingContext.Notify_handlePropertyTags[this.notificationHandle][parsingSessionID].Item3 == MapiInspector.MAPIInspector.ParsingSession.RequestHeaders["X-ClientInfo"]))
                {
                    throw new MissingInformationException("Missing PropertyTags information for RopNotifyResponse", (ushort)RopIdType.RopNotify, new uint[] { this.IsEmptyMessageID(this.TableRowMessageID) ? (uint)0 : (uint)1, this.notificationHandle });
                }
                else
                {
                    this.propertiesBySetColum = DecodingContext.Notify_handlePropertyTags[this.notificationHandle][parsingSessionID].Item4;
                }

                this.TableRowData = new PropertyRow(this.propertiesBySetColum);
                this.TableRowData.Parse(s);
            }

            if (this.NotificationFlags.Value.NotificationType != NotificationTypesEnum.TableModified && this.NotificationFlags.Value.NotificationType != NotificationTypesEnum.Extended)
            {
                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
            }

            if (this.NotificationFlags.Value.NotificationType != NotificationTypesEnum.TableModified && this.NotificationFlags.Value.NotificationType != NotificationTypesEnum.Extended && (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0))
            {
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }

            if ((this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCreated || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectDeleted || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied) && ((((int)this.NotificationFlags.Value.NotificationDataAvailability) & 0xC000) == 0xC000 || (((int)this.NotificationFlags.Value.NotificationDataAvailability) & 0xC000) == 0))
            {
                this.ParentFolderId = new FolderID();
                this.ParentFolderId.Parse(s);
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied)
            {
                this.OldFolderId = new FolderID();
                this.OldFolderId.Parse(s);
            }

            if ((this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied) && (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0))
            {
                this.OldMessageId = new MessageID();
                this.OldMessageId.Parse(s);
            }

            if ((this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied) && (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x8000) == 0))
            {
                this.OldParentFolderId = new MessageID();
                this.OldParentFolderId.Parse(s);
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCreated || this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectModified)
            {
                this.TagCount = this.ReadUshort();

                if (this.TagCount != 0x0000 && this.TagCount != 0xFFFF)
                {
                    List<PropertyTag> listTags = new List<PropertyTag>();

                    for (int i = 0; i < this.TagCount; i++)
                    {
                        PropertyTag tempTag = new PropertyTag();
                        tempTag.Parse(s);
                        listTags.Add(tempTag);
                    }

                    this.Tags = listTags.ToArray();
                }
            }

            if (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x1000) != 0)
            {
                this.TotalMessageCount = this.ReadUint();
            }

            if (((int)this.NotificationFlags.Value.NotificationDataAvailability & 0x2000) != 0)
            {
                this.UnreadMessageCount = this.ReadUint();
            }

            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.NewMail)
            {
                this.MessageFlags = this.ReadUint();
                this.UnicodeFlag = this.ReadByte();
                this.MessageClass = new MAPIString(Encoding.ASCII);
                this.MessageClass.Parse(s);
            }
        }
    }

    #endregion
}
