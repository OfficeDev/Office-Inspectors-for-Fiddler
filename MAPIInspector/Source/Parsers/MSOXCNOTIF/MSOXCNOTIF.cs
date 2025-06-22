namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    using System.Collections.Generic;
    using System.IO;

    #region The class or enum define related to ROPs.

    /// <summary>
    /// The enum value of Notification type.
    /// </summary>
    [Flags]
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
        SearchCompleted = 0x0080,

        /// <summary>
        /// A table has been modified on the server
        /// </summary>
        TableModified = 0x0100,

        /// <summary>
        /// Extended one
        /// </summary>
        Extended = 0x0400,

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
            this.ReturnValue = HelpMethod.FormatErrorCode((ErrorCodes)this.ReadUint());
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
    public class NotificationData : Block
    {
        /// <summary>
        /// A combination of an enumeration and flags that describe the type of the notification and the availability of the notification data fields.
        /// </summary>
        public BlockT<NotificationTypesEnum> NotificationFlags;

        /// <summary>
        /// A subtype of the notification for a TableModified event.
        /// </summary>
        public BlockT<TableEventTypeEnum> TableEventType;

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
        public BlockT<uint> TableRowInstance;

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
        public BlockT<uint> InsertAfterTableRowInstance;

        /// <summary>
        /// An unsigned 16-bit integer that indicates the length of the table row data.
        /// </summary>
        public BlockT<ushort> TableRowDataSize;

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
        public FolderID OldParentFolderId;

        /// <summary>
        /// An unsigned 16-bit integer that specifies the number of property tags in the Tags field.
        /// </summary>
        public BlockT<ushort> TagCount;

        /// <summary>
        /// An array of unsigned 32-bit integers that identifies the IDs of properties that have changed.
        /// </summary>
        public PropertyTag[] Tags;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the total number of items in the folder triggering this event.
        /// </summary>
        public BlockT<uint> TotalMessageCount;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the number of unread items in a folder triggering this event.
        /// </summary>
        public BlockT<uint> UnreadMessageCount;

        /// <summary>
        /// An unsigned 32-bit integer that specifies the message flags of new mail that has been received
        /// </summary>
        public BlockT<uint> MessageFlags;

        /// <summary>
        /// A value of TRUE (0x01) indicates the value of the MessageClass field is in Unicode
        /// </summary>
        public BlockT<byte> UnicodeFlag;

        /// <summary>
        /// A null-terminated string containing the message class of the new mail.
        /// </summary>
        public Block MessageClass;

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
        /// Parse the NotificationData structure.
        /// </summary>
        protected override void Parse()
        {
            NotificationFlags = ParseT<NotificationTypesEnum>();
            if (NotificationFlags.Data.HasFlag(NotificationTypesEnum.TableModified))
            {
                TableEventType = ParseT<TableEventTypeEnum>();
            }

            // bit 0x8000 is set in the NotificationFlags field
            var isMessage = NotificationFlags.Data.HasFlag(NotificationTypesEnum.M);
            // NotificationType value in the NotificationFlags field is not 0x0100 or 0x0400
            var notModifiedExtended = !NotificationFlags.Data.HasFlag(NotificationTypesEnum.TableModified) &&
            !NotificationFlags.Data.HasFlag(NotificationTypesEnum.Extended);
            // NotificationType in the NotificationFlags field is 0x0004, 0x0008, 0x0020, or 0x0040,
            var isCreateDeleteMovedCopied = NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectCreated) ||
                NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectDeleted) ||
                NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectMoved) ||
                NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectCopied);
            // a message in a search folder(both bit 0x4000 and bit 0x8000 are set in the NotificationFlags field)
            var isSearchFolderMessage = NotificationFlags.Data.HasFlag(NotificationTypesEnum.S) &&
                NotificationFlags.Data.HasFlag(NotificationTypesEnum.M);
            // a folder (both bit 0x4000 and bit 0x8000 are not set in the NotificationFlags field).
            var isFolder = !NotificationFlags.Data.HasFlag(NotificationTypesEnum.S) &&
                !NotificationFlags.Data.HasFlag(NotificationTypesEnum.M);
            // NotificationType value in the NotificationFlags field is 0x0020 or 0x0040
            var isMovedCopied = NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectMoved) ||
                NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectCopied);
            // NotificationType in the NotificationFlags field is 0x0004 or 0x0010
            var isCreateDelete = NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectCreated) ||
                NotificationFlags.Data.HasFlag(NotificationTypesEnum.ObjectDeleted);
            // bit 0x1000 is set in the NotificationFlags field
            var isTotalMessageCount = NotificationFlags.Data.HasFlag(NotificationTypesEnum.T);
            // bit 0x2000 is set in the NotificationFlags field
            var isUnreadMessageCount = NotificationFlags.Data.HasFlag(NotificationTypesEnum.U);
            // NotificationType in the NotificationFlags field is 0x0002
            var isNewMail = NotificationFlags.Data.HasFlag(NotificationTypesEnum.NewMail);

            if (TableEventType != null)
            {
                // TableEventType field is available and is 0x0003, 0x0004, or 0x0005
                var isADM = TableEventType.Data == TableEventTypeEnum.TableRowAdded ||
                    TableEventType.Data == TableEventTypeEnum.TableRowDeleted ||
                    TableEventType.Data == TableEventTypeEnum.TableRowModified;
                // TableEventType field is available and is 0x0003 or 0x0005
                var isAM = TableEventType.Data == TableEventTypeEnum.TableRowAdded ||
                    TableEventType.Data == TableEventTypeEnum.TableRowModified;

                if (isADM) TableRowFolderID = Parse<FolderID>();
                if (isMessage && isADM) TableRowMessageID = Parse<MessageID>();
                if (isMessage && isADM) TableRowInstance = ParseT<uint>();

                if (isMessage && isAM) InsertAfterTableRowFolderID = Parse<FolderID>();
                if (isMessage && isAM) InsertAfterTableRowID = Parse<MessageID>();
                if (isMessage && isAM) InsertAfterTableRowInstance = ParseT<uint>();

                if (isAM)
                {
                    TableRowDataSize = ParseT<ushort>();

                    int parsingSessionID = MapiInspector.MAPIParser.ParsingSession.id;
                    if (MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
                    {
                        parsingSessionID = int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"]);
                    }
                    if (DecodingContext.Notify_handlePropertyTags.Count > 0 && DecodingContext.Notify_handlePropertyTags.ContainsKey(notificationHandle) && DecodingContext.Notify_handlePropertyTags[notificationHandle].ContainsKey(parsingSessionID)
                        && DecodingContext.Notify_handlePropertyTags[notificationHandle][parsingSessionID].Item1 == MapiInspector.MAPIParser.ParsingSession.RequestHeaders.RequestPath
                        && DecodingContext.Notify_handlePropertyTags[notificationHandle][parsingSessionID].Item2 == MapiInspector.MAPIParser.ParsingSession.LocalProcess
                        && DecodingContext.Notify_handlePropertyTags[notificationHandle][parsingSessionID].Item3 == MapiInspector.MAPIParser.ParsingSession.RequestHeaders["X-ClientInfo"])
                    {
                        propertiesBySetColum = DecodingContext.Notify_handlePropertyTags[notificationHandle][parsingSessionID].Item4;
                    }

                    TableRowData = new PropertyRow(TableRowDataSize.Data, propertiesBySetColum);
                    TableRowData.Parse(parser);
                }
            }

            if (notModifiedExtended) FolderId = Parse<FolderID>();
            if (notModifiedExtended && isMessage) MessageId = Parse<MessageID>();
            if (isCreateDeleteMovedCopied && (isSearchFolderMessage || isFolder)) ParentFolderId = Parse<FolderID>();
            if (isMovedCopied) OldFolderId = Parse<FolderID>();
            if (isMovedCopied && isMessage) OldMessageId = Parse<MessageID>();
            if (isMovedCopied && isMessage) OldParentFolderId = Parse<FolderID>();

            if (isCreateDelete)
            {
                TagCount = ParseT<ushort>();

                if (TagCount.Data != 0x0000 && TagCount.Data != 0xFFFF)
                {
                    var listTags = new List<PropertyTag>();

                    for (int i = 0; i < TagCount.Data; i++)
                    {
                        listTags.Add(Parse<PropertyTag>());
                    }

                    Tags = listTags.ToArray();
                }
            }

            if (isTotalMessageCount) TotalMessageCount = ParseT<uint>();
            if (isUnreadMessageCount) UnreadMessageCount = ParseT<uint>();

            if (isNewMail)
            {
                MessageFlags = ParseT<uint>();
                UnicodeFlag = ParseT<byte>();
                if (UnicodeFlag.Data == 0x00)
                {
                    MessageClass = Parse<PtypString8>();
                }
                else if (UnicodeFlag.Data == 0x01)
                {
                    MessageClass = Parse<PtypString>();
                }
            }
        }

        protected override void ParseBlocks()
        {
            // Add NotificationFlags as a labeled child
            if (NotificationFlags != null)
            {
                AddChildBlockT(NotificationFlags, "NotificationFlags");
            }

            // Add TableEventType if present
            if (TableEventType != null)
            {
                AddChildBlockT(TableEventType, "TableEventType");
            }

            // Add TableRowFolderID, TableRowMessageID, TableRowInstance if present
            AddLabeledChild(TableRowFolderID, "TableRowFolderID");
            AddLabeledChild(TableRowMessageID, "TableRowMessageID");
            if (TableRowInstance != null)
            {
                AddChildBlockT(TableRowInstance, "TableRowInstance");
            }

            // Add InsertAfterTableRowFolderID, InsertAfterTableRowID, InsertAfterTableRowInstance if present
            AddLabeledChild(InsertAfterTableRowFolderID, "InsertAfterTableRowFolderID");
            AddLabeledChild(InsertAfterTableRowID, "InsertAfterTableRowID");
            if (InsertAfterTableRowInstance != null)
            {
                AddChildBlockT(InsertAfterTableRowInstance, "InsertAfterTableRowInstance");
            }

            // Add TableRowDataSize and TableRowData if present
            if (TableRowDataSize != null)
            {
                AddChildBlockT(TableRowDataSize, "TableRowDataSize");
            }

            AddLabeledChild(TableRowData, "TableRowData");

            // Add FolderId, MessageId, ParentFolderId, OldFolderId, OldMessageId, OldParentFolderId if present
            AddLabeledChild(FolderId, "FolderId");
            AddLabeledChild(MessageId, "MessageId");
            AddLabeledChild(ParentFolderId, "ParentFolderId");
            AddLabeledChild(OldFolderId, "OldFolderId");
            AddLabeledChild(OldMessageId, "OldMessageId");
            AddLabeledChild(OldParentFolderId, "OldParentFolderId");

            // Add TagCount and Tags if present
            if (TagCount != null)
            {
                AddChildBlockT(TagCount, "TagCount");
                AddLabeledChildren(Tags, "Tags");
            }

            // Add TotalMessageCount, UnreadMessageCount if present
            if (TotalMessageCount != null)
            {
                AddChildBlockT(TotalMessageCount, "TotalMessageCount");
            }
            if (UnreadMessageCount != null)
            {
                AddChildBlockT(UnreadMessageCount, "UnreadMessageCount");
            }

            // Add MessageFlags, UnicodeFlag, MessageClass if present
            if (MessageFlags != null)
            {
                AddChildBlockT(MessageFlags, "MessageFlags");
            }
            if (UnicodeFlag != null)
            {
                AddChildBlockT(UnicodeFlag, "UnicodeFlag");
            }

            AddLabeledChild(MessageClass, "MessageClass");
        }
    }

    #endregion
}
