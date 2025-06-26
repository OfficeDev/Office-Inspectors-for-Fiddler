using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.1.4.1.2 NotificationData Structure
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
        public BlockString MessageClass;

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
                    MessageClass = ParseStringA();
                }
                else if (UnicodeFlag.Data == 0x01)
                {
                    MessageClass = ParseStringW();
                }
            }
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(NotificationFlags, "NotificationFlags");
            AddChildBlockT(TableEventType, "TableEventType");
            AddLabeledChild(TableRowFolderID, "TableRowFolderID");
            AddLabeledChild(TableRowMessageID, "TableRowMessageID");
            AddChildBlockT(TableRowInstance, "TableRowInstance");
            AddLabeledChild(InsertAfterTableRowFolderID, "InsertAfterTableRowFolderID");
            AddLabeledChild(InsertAfterTableRowID, "InsertAfterTableRowID");
            AddChildBlockT(InsertAfterTableRowInstance, "InsertAfterTableRowInstance");
            AddChildBlockT(TableRowDataSize, "TableRowDataSize");
            AddLabeledChild(TableRowData, "TableRowData");
            AddLabeledChild(FolderId, "FolderId");
            AddLabeledChild(MessageId, "MessageId");
            AddLabeledChild(ParentFolderId, "ParentFolderId");
            AddLabeledChild(OldFolderId, "OldFolderId");
            AddLabeledChild(OldMessageId, "OldMessageId");
            AddLabeledChild(OldParentFolderId, "OldParentFolderId");
            AddChildBlockT(TagCount, "TagCount");
            AddLabeledChildren(Tags, "Tags");
            AddChildBlockT(TotalMessageCount, "TotalMessageCount");
            AddChildBlockT(UnreadMessageCount, "UnreadMessageCount");
            AddChildBlockT(MessageFlags, "MessageFlags");
            AddChildBlockT(UnicodeFlag, "UnicodeFlag");
            AddChildString(MessageClass, "MessageClass");
        }
    }
}
