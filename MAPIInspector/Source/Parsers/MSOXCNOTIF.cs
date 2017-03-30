using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region The class or enum define related to rops.

    /// <summary>
    /// The enum value of Notification type.
    /// </summary>
    public enum NotificationTypesEnum : ushort
    {
        NewMail = 0x0002,
        ObjectCreated = 0x0004,
        ObjectDeleted = 0x0008,
        ObjectModified = 0x0010,
        ObjectMoved = 0x0020,
        ObjectCopied = 0x0040,
        SearchComplete = 0x0080,
        TableModified = 0x0100,
        Extended = 0x0400,
        NULL = 0x0000
    }
    /// <summary>
    /// The enum value of NotificationData Availability.
    /// </summary>
    [Flags]
    public enum NotificationDataAvailabilityEnum : ushort
    {
        T = 0x1000,
        U = 0x2000,
        S = 0x4000,
        M = 0x8000,
        NULL = 0x0000
    }

    /// <summary>
    /// A class indicates the NotificationFlagsT.
    /// </summary>
    public class NotificationFlagsT : BaseStructure
    {
        // The Notification type.
        [BitAttribute(12)]
        public NotificationTypesEnum NotificationType;

        // The NotificationData Avaliablity.
        [BitAttribute(4)]
        public NotificationDataAvailabilityEnum NotificationDataAvailability;

        /// <summary>
        /// Parse the NotificationFlagsT structure.
        /// </summary>
        /// <param name="s">An stream containing NotificationFlagsT structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            ushort flag = ReadUshort();
            this.NotificationDataAvailability = (NotificationDataAvailabilityEnum)(flag & 0xf000);
            this.NotificationType = (NotificationTypesEnum)(flag & 0x0fff);
        }
    }

    /// <summary>
    /// A class indicates the NotificationFlags.
    /// </summary>
    public class NotificationFlags : BaseStructure
    {
        public NotificationFlagsT Value;
        /// <summary>
        /// Parse the NotificationFlags structure.
        /// </summary>
        /// <param name="s">An stream containing NotificationFlags structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Value = new NotificationFlagsT();
            this.Value.Parse(s);
        }
    }

    /// <summary>
    /// The enum value of TableEvent type.
    /// </summary>
    public enum TableEventTypeEnum : ushort
    {
        TableChanged = 0x0001,
        TableRowAdded = 0x0003,
        TableRowDeleted = 0x0004,
        TableRowModified = 0x0005,
        TableRestrictionChanged = 0x0007
    }

    #endregion

    #region 2.2.1.2	Subscription Management
    #endregion

    #region 2.2.1.2.1	RopRegisterNotification ROP
    /// <summary>
    /// The RopRegisterNotification ROP ([MS-OXCROPS] section 2.2.14.1) creates a subscription for specified notifications on the server and returns a handle of the subscription to the client. 
    /// </summary>
    public class RopRegisterNotificationRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x29.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // A flags structure that contains flags that specify the types of events to register for. 
        public NotificationTypesEnum NotificationTypes;

        // A flags structure. 
        public byte? Reserved;

        // A Boolean that specifies whether the notification is scoped to the mailbox instead of a specific folder or message.
        public bool WantWholeStore;

        // This value specifies the folder to register notifications for
        public FolderID FolderId;

        // This value specifies the message to register notifications for.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopRegisterNotificationRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopRegisterNotificationRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.NotificationTypes = (NotificationTypesEnum)ReadUshort();
            if (this.NotificationTypes == NotificationTypesEnum.Extended)
            {
                this.Reserved = ReadByte();
            }
            this.WantWholeStore = ReadBoolean();
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
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x29.
        public RopIdType RopId;

        //  An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request. 
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopRegisterNotificationResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopRegisterNotificationResponse structure.</param>
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

    #region 2.2.1.3.4	RopPending ROP
    /// <summary>
    /// A class indicates the RopPending ROP Response Buffer.
    /// </summary>
    public class RopPendingResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x6E.
        public RopIdType RopId;

        // An unsigned integer index that specifies which session has pending notifications.
        public ushort SessionIndex;

        /// <summary>
        /// Parse the RopPendingResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopPendingResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.SessionIndex = ReadUshort();
        }
    }
    #endregion

    #region 2.2.1.4.1	RopNotify ROP
    /// <summary>
    /// A class indicates the RopNotify ROP Response Buffer.
    /// </summary>
    public class RopNotifyResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. For this operation this field is set to 0x2A.
        public RopIdType RopId;

        // A Server object handle that specifies the notification Server object associated with this notification event.
        public uint NotificationHandle;

        // An unsigned integer that specifies the logon associated with this notification event.
        public byte LogonId;

        // Various structures. The 
        public NotificationData NotificationData;

        /// <summary>
        /// Parse the RopNotifyResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopNotifyResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.NotificationHandle = ReadUint();
            this.LogonId = ReadByte();
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
        // A combination of an enumeration and flags that describe the type of the notification and the availability of the notification data fields.
        public NotificationFlags NotificationFlags;

        // A subtype of the notification for a TableModified event.
        public TableEventTypeEnum? TableEventType;

        // The value of the Folder ID structure, as specified in [MS-OXCDATA] section 2.2.1.1, of the item triggering the notification. 
        public FolderID TableRowFolderID;

        // The value of the Message ID structure, as specified in [MS-OXCDATA] section 2.2.1.2, of the item triggering the notification. 
        public MessageID TableRowMessageID;

        // An identifier of the instance of the previous row in the table. 
        public uint? TableRowInstance;

        // The old value of the Folder ID structure of the item triggering the notification. 
        public FolderID InsertAfterTableRowFolderID;

        // The old value of the Message ID structure of the item triggering the notification. 
        public MessageID InsertAfterTableRowID;

        // An unsigned 32-bit identifier of the instance of the row where the modified row is inserted. 
        public uint? InsertAfterTableRowInstance;

        // An unsigned 16-bit integer that indicates the length of the table row data. 
        public ushort? TableRowDataSize;

        // The table row data, which contains a list of property values
        public PropertyRow TableRowData;

        // The Folder ID structure of the item triggering the event. 
        public FolderID FolderId;

        // The Message ID structure, as specified in [MS-OXCDATA] section 2.2.1.2, of the item triggering the event. 
        public MessageID MessageId;

        // The Folder ID structure of the parent folder of the item triggering the event. 
        public FolderID ParentFolderId;

        // The old Folder ID structure of the item triggering the event. 
        public FolderID OldFolderId;

        // The old Message ID structure of the item triggering the event. 
        public MessageID OldMessageId;

        // The old parent Folder ID structure of the item triggering the event. 
        public MessageID OldParentFolderId;

        // An unsigned 16-bit integer that specifies the number of property tags in the Tags field. 
        public ushort? TagCount;

        // An array of unsigned 32-bit integers that identifies the IDs of properties that have changed. 
        public PropertyTag[] Tags;

        // An unsigned 32-bit integer that specifies the total number of items in the folder triggering this event. 
        public uint? TotalMessageCount;

        // An unsigned 32-bit integer that specifies the number of unread items in a folder triggering this event. 
        public uint? UnreadMessageCount;

        // An unsigned 32-bit integer that specifies the message flags of new mail that has been received
        public uint? MessageFlags;

        // : A value of TRUE (0x01) indicates the value of the MessageClass field is in Unicode
        public byte? UnicodeFlag;

        // A null-terminated string containing the message class of the new mail. 
        public MAPIString MessageClass; //  A null-terminated string containing the message class of the new mail. The string is in Unicode if the UnicodeFlag field is set to TRUE (0x01). The string is in ASCII if UnicodeFlag is set to FALSE (0x00). 

        // A Server object handle that specifies the notification Server object associated with this notification event.
        private uint NotificationHandle;

        // Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// The construe function for NotificationData
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public NotificationData(uint NotificationHandle)
        {
            this.NotificationHandle = NotificationHandle;
        }

        /// <summary>
        /// Parse the NotificationData structure.
        /// </summary>
        /// <param name="s">An stream containing NotificationData structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.NotificationFlags = new NotificationFlags();
            this.NotificationFlags.Parse(s);
            if (this.NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified)
            {
                this.TableEventType = (TableEventTypeEnum)ReadUshort();
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowDeleted || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowFolderID = new FolderID();
                this.TableRowFolderID.Parse(s);
            }
            if ((((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowDeleted || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowMessageID = new MessageID();
                this.TableRowMessageID.Parse(s);
            }
            if ((((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowDeleted || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowInstance = ReadUint();
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.InsertAfterTableRowFolderID = new FolderID();
                this.InsertAfterTableRowFolderID.Parse(s);
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.InsertAfterTableRowID = new MessageID();
                this.InsertAfterTableRowID.Parse(s);
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0) && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.InsertAfterTableRowInstance = ReadUint();
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                this.TableRowDataSize = ReadUshort();
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.TableModified && (TableEventType == TableEventTypeEnum.TableRowAdded || TableEventType == TableEventTypeEnum.TableRowModified))
            {
                if (DecodingContext.PropertyTagsForNotify != null && DecodingContext.PropertyTagsForNotify.ContainsKey(this.NotificationHandle))
                {
                    propertiesBySetColum = DecodingContext.PropertyTagsForNotify[this.NotificationHandle];
                }
                else
                {
                    throw new MissingInformationException("Missing PropertyTags information for RopNotifyResponse", (ushort)RopIdType.RopNotify, new uint[] {0,NotificationHandle });
                }
                this.TableRowData = new PropertyRow(propertiesBySetColum);
                this.TableRowData.Parse(s);
            }
            if (NotificationFlags.Value.NotificationType != NotificationTypesEnum.TableModified && NotificationFlags.Value.NotificationType != NotificationTypesEnum.Extended)
            {

                this.FolderId = new FolderID();
                this.FolderId.Parse(s);
            }
            if (NotificationFlags.Value.NotificationType != NotificationTypesEnum.TableModified && NotificationFlags.Value.NotificationType != NotificationTypesEnum.Extended && (((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0))
            {
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }
            if ((NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCreated || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectDeleted || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied) && ((((int)NotificationFlags.Value.NotificationDataAvailability) & 0xC000) == 0xC000 || (((int)NotificationFlags.Value.NotificationDataAvailability) & 0xC000) == 0))
            {
                this.ParentFolderId = new FolderID();
                this.ParentFolderId.Parse(s);
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied)
            {

                this.OldFolderId = new FolderID();
                this.OldFolderId.Parse(s);
            }
            if ((NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied) && (((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) != 0))
            {
                this.OldMessageId = new MessageID();
                this.OldMessageId.Parse(s);
            }
            if ((NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectMoved || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCopied) && (((int)NotificationFlags.Value.NotificationDataAvailability & 0x8000) == 0))
            {
                this.OldParentFolderId = new MessageID();
                this.OldParentFolderId.Parse(s);
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectCreated || NotificationFlags.Value.NotificationType == NotificationTypesEnum.ObjectModified)
            {
                this.TagCount = ReadUshort();
                if (TagCount != 0x0000 && TagCount != 0xFFFF)
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
            if (((int)NotificationFlags.Value.NotificationDataAvailability & 0x1000) != 0)
            {
                this.TotalMessageCount = ReadUint();
            }
            if (((int)NotificationFlags.Value.NotificationDataAvailability & 0x2000) != 0)
            {
                this.UnreadMessageCount = ReadUint();
            }
            if (NotificationFlags.Value.NotificationType == NotificationTypesEnum.NewMail)
            {
                this.MessageFlags = ReadUint();
                this.UnicodeFlag = ReadByte();
                this.MessageClass = new MAPIString(Encoding.ASCII);
                this.MessageClass.Parse(s);
            }
        }
    }
    #endregion

}
