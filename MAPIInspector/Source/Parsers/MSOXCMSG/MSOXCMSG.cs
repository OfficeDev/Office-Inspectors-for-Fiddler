namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    using System.Collections.Generic;
    using System.IO;

    #region Enums
    /// <summary>
    /// The enum value of OpenModeFlags that contains flags that control the access to the message. 
    /// </summary>
    public enum OpenMessageModeFlags : byte
    {
        /// <summary>
        /// Message will be opened as read-only
        /// </summary>
        ReadOnly = 0x00,

        /// <summary>
        /// Message will be opened for both reading and writing
        /// </summary>
        ReadWrite = 0x01,

        /// <summary>
        /// Open for read/write if the user has write permissions for the folder, read-only if not.
        /// </summary>
        BestAccess = 0x03,

        /// <summary>
        /// Open a soft deleted Message object if available
        /// </summary>
        OpenSoftDeleted = 0x04
    }

    /// <summary>
    /// An enumeration that specifies the flag of RecipientType.
    /// </summary>
    public enum RecipientTypeFlag : byte
    {
        /// <summary>
        /// This flag indicates that this recipient (1) did not successfully receive the message on the previous attempt
        /// </summary>
        FailToReceiveTheMessageOnThePreviousAttempt = 0x01,

        /// <summary>
        /// This flag indicates that this recipient (1) did successfully receive the message on the previous attempt
        /// </summary>
        SuccessfullyToReceiveTheMessageOnThePreviousAttempt = 0x08
    }

    /// <summary>
    /// An enumeration that specifies the type of RecipientType.
    /// </summary>
    public enum RecipientTypeType : byte
    {
        /// <summary>
        /// Primary recipient
        /// </summary>
        PrimaryRecipient = 0x01,

        /// <summary>
        /// Carbon copy recipient
        /// </summary>
        CcRecipient = 0x02,

        /// <summary>
        /// Blind carbon copy recipient
        /// </summary>
        BccRecipient = 0x03
    }

    /// <summary>
    /// The enum value of SaveFlags that contains flags that specify how the save operation behaves.
    /// </summary>
    public enum SaveFlags : byte
    {
        /// <summary>
        /// Keeps the Message object open with read-only access
        /// </summary>
        KeepOpenReadOnly = 0x01,

        /// <summary>
        /// Keeps the Message object open with read/write access
        /// </summary>
        KeepOpenReadWrite = 0x02,

        /// <summary>
        /// Keeps the Message object open with read/write access. The ecObjectModified error code is not valid when this flag is set; the server overwrites any changes instead
        /// </summary>
        ForceSave = 0x04
    }

    /// <summary>
    /// The enum value of GetAttachmentTableFlags that contains flags that control the type of table..
    /// </summary>
    public enum GetAttachmentTableFlags : byte
    {
        /// <summary>
        /// Open the table.
        /// </summary>
        Standard = 0x00,

        /// <summary>
        /// Open the table. Also requests that the columns containing string data be returned in Unicode format.
        /// </summary>
        Unicode = 0x40
    }

    /// <summary>
    /// The enum specifies the status of a message in a contents table. 
    /// </summary>
    [Flags]
    public enum MessageStatusFlag : uint
    {
        /// <summary>
        /// The message has been marked for downloading from the remote message store to the local client
        /// </summary>
        msRemoteDownload = 0x00001000,

        /// <summary>
        /// This is a conflict resolve message
        /// </summary>
        msInConflict = 0x00000800,

        /// <summary>
        /// The message has been marked for deletion at the remote message store without downloading to the local client
        /// </summary>
        msRemoteDelete = 0x00002000
    }

    /// <summary>
    /// The enum specifies the flags to set. 
    /// </summary>
    [Flags]
    public enum ReadFlags : byte
    {
        /// <summary>
        /// The server sets the read flag and sends the receipt.
        /// </summary>
        rfDefault = 0x00,

        /// <summary>
        /// The user requests that any pending read receipt be canceled; the server sets the mfRead bit
        /// </summary>
        rfSuppressReceipt = 0x01,

        /// <summary>
        /// Ignored by the server
        /// </summary>
        rfReserved = 0x0A,

        /// <summary>
        /// Server clears the mfRead bit; the client MUST include the rfSuppressReceipt bit with this flag
        /// </summary>
        rfClearReadFlag = 0x04,

        /// <summary>
        /// The server sends a read receipt if one is pending, but does not change the mfRead bit
        /// </summary>
        rfGenerateReceiptOnly = 0x10,

        /// <summary>
        /// The server clears the mfNotifyRead bit but does not send a read receipt
        /// </summary>
        rfClearNotifyRead = 0x20,

        /// <summary>
        /// The server clears the mfNotifyUnread bit but does not send a nonread receipt
        /// </summary>
        rfClearNotifyUnread = 0x40
    }

    /// <summary>
    /// The enum specifies the flags for opening attachments.
    /// </summary>
    public enum OpenAttachmentFlags : byte
    {
        /// <summary>
        /// Attachment will be opened as read-only
        /// </summary>
        ReadOnly = 0x00,

        /// <summary>
        /// Attachment will be opened for both reading and writing
        /// </summary>
        ReadWrite = 0x01,

        /// <summary>
        /// Attachment will be opened for read/write if the user has write permissions for the attachment; opened for read-only if not
        /// </summary>
        BestAccess = 0x03
    }
    #endregion

    #region 2.2.3.1	RopOpenMessage
    /// <summary>
    ///  A class indicates the RopOpenMessage ROP Request Buffer.
    /// </summary>
    public class RopOpenMessageRequest : BaseStructure
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
        /// An identifier that specifies which code page will be used for string values associated with the message.
        /// </summary>
        public short CodePageId;

        /// <summary>
        /// An identifier that identifies the parent folder of the message to be opened.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A flags structure that contains flags that control the access to the message. 
        /// </summary>
        public OpenMessageModeFlags OpenModeFlags;

        /// <summary>
        /// An identifier that identifies the message to be opened.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopOpenMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CodePageId = this.ReadINT16();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.OpenModeFlags = (OpenMessageModeFlags)this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }
    }

    /// <summary>
    ///  A class indicates the RopOpenMessage ROP response Buffer.
    /// </summary>
    public class RopOpenMessageResponse : BaseStructure
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
        /// A Boolean that specifies whether the message has named properties.
        /// </summary>
        public bool? HasNamedProperties;

        /// <summary>
        /// A TypedString structure that specifies the subject prefix of the message. 
        /// </summary>
        public TypedString SubjectPrefix;

        /// <summary>
        /// A TypedString structure that specifies the normalized subject of the message. 
        /// </summary>
        public TypedString NormalizedSubject;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (1) on the message.
        /// </summary>
        public ushort? RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public ushort? ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included in each row that is specified in the RecipientRows field. 
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientRows field.
        /// </summary>
        public byte? RowCount;

        /// <summary>
        /// A list of OpenRecipientRow structures. 
        /// </summary>
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.HasNamedProperties = this.ReadBoolean();
                this.SubjectPrefix = new TypedString();
                this.SubjectPrefix.Parse(s);
                this.NormalizedSubject = new TypedString();
                this.NormalizedSubject.Parse(s);
                this.RecipientCount = this.ReadUshort();
                this.ColumnCount = this.ReadUshort();
                List<PropertyTag> propertyTags = new List<PropertyTag>();

                for (int i = 0; i < this.ColumnCount; i++)
                {
                    PropertyTag propertyTag = Block.Parse<PropertyTag>(s);
                    propertyTags.Add(propertyTag);
                }

                this.RecipientColumns = propertyTags.ToArray();
                this.RowCount = this.ReadByte();
                List<OpenRecipientRow> openRecipientRows = new List<OpenRecipientRow>();

                for (int i = 0; i < this.RowCount; i++)
                {
                    OpenRecipientRow openRecipientRow = new OpenRecipientRow(this.RecipientColumns);
                    openRecipientRow.Parse(s);
                    openRecipientRows.Add(openRecipientRow);
                }

                this.RecipientRows = openRecipientRows.ToArray();
            }
        }
    }

    /// <summary>
    /// A class indicates the OpenRecipientRow structure.
    /// </summary>
    public class OpenRecipientRow : BaseStructure
    {
        /// <summary>
        /// An enumeration that specifies the type of recipient (2). 
        /// </summary>
        public RecipientType RecipientType;

        /// <summary>
        /// An identifier that specifies the code page for the recipient (2).
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// Reserved. The server MUST set this field to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public ushort RecipientRowSize;

        /// <summary>
        /// A RecipientRow structure. 
        /// </summary>
        public RecipientRow RecipientRow;
        
        /// <summary>
        /// Array of PropertyTag used to initialize the class.
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the OpenRecipientRow class.
        /// </summary>
        /// <param name="propTags">Array of PropertyTag used to initialize the class.</param>
        public OpenRecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the OpenRecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing OpenRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RecipientType = new RecipientType();
            this.RecipientType.Parse(s);
            this.CodePageId = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.RecipientRowSize = this.ReadUshort();
            this.RecipientRow = new RecipientRow(this.propTags);
            this.RecipientRow.Parse(s);
        }
    }
    #endregion

    #region 2.2.6.2	RopCreateMessage

    /// <summary>
    /// A class indicates the RopCreateMessage ROP request Buffer.
    /// </summary>
    public class RopCreateMessageRequest : BaseStructure
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
        /// An identifier that specifies the code page for the message.
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// An identifier that specifies the parent folder.
        /// </summary>
        public FolderID FolderId;

        /// <summary>
        /// A Boolean that specifies whether the message is an FAI message.
        /// </summary>
        public bool AssociatedFlag;

        /// <summary>
        /// Parse the RopCreateMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CodePageId = this.ReadUshort();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.AssociatedFlag = this.ReadBoolean();
        }
    }

    /// <summary>
    /// A class indicates the RopCreateMessage ROP response Buffer.
    /// </summary>
    public class RopCreateMessageResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex specified in field the request.
        /// </summary>
        public byte OutputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the MessageId field is present.
        /// </summary>
        public bool? HasMessageId;

        /// <summary>
        /// An identifier that is present if HasMessageId is nonzero and is not present if it is zero.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopCreateMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.HasMessageId = this.ReadBoolean();
                if ((bool)this.HasMessageId)
                {
                    this.MessageId = new MessageID();
                    this.MessageId.Parse(s);
                }
            }
        }
    }

    #endregion

    #region 2.2.6.3	RopSaveChangesMessage ROP

    /// <summary>
    /// A class indicates the RopSaveChangesMessage ROP request Buffer.
    /// </summary>
    public class RopSaveChangesMessageRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        ///  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that specify how the save operation behaves.
        /// </summary>
        public SaveFlags SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.SaveFlags = (SaveFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopSaveChangesMessage ROP response Buffer.
    /// </summary>
    public class RopSaveChangesMessageResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte? InputHandleIndex;

        /// <summary>
        /// An identifier that specifies the ID of the message saved.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSaveChangesMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesMessageResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.InputHandleIndex = this.ReadByte();
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
            }
        }
    }
    #endregion

    #region 2.2.6.4	RopRemoveAllRecipients ROP

    /// <summary>
    /// A class indicates the RopRemoveAllRecipients ROP request Buffer.
    /// </summary>
    public class RopRemoveAllRecipientsRequest : BaseStructure
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
        /// Reserved. The client SHOULD set this field to 0x00000000. 
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// Parse the RopRemoveAllRecipientsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopRemoveAllRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Reserved = this.ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopRemoveAllRecipients ROP response Buffer.
    /// </summary>
    public class RopRemoveAllRecipientsResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        ///  An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopRemoveAllRecipientsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopRemoveAllRecipientsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.6.5	RopModifyRecipients ROP

    /// <summary>
    /// A class indicates the RopModifyRecipients ROP request Buffer.
    /// </summary>
    public class RopModifyRecipientsRequest : BaseStructure
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
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public ushort ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included for each recipient (1).
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public ushort RowCount;

        /// <summary>
        /// A list of ModifyRecipientRow structures.
        /// </summary>
        public ModifyRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopModifyRecipientsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopModifyRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ColumnCount = this.ReadUshort();
            List<PropertyTag> propertyTags = new List<PropertyTag>();

            for (int i = 0; i < this.ColumnCount; i++)
            {
                PropertyTag propertyTag = Block.Parse<PropertyTag>(s);
                propertyTags.Add(propertyTag);
            }

            this.RecipientColumns = propertyTags.ToArray();
            this.RowCount = this.ReadUshort();
            List<ModifyRecipientRow> modifyRecipientRows = new List<ModifyRecipientRow>();

            for (int i = 0; i < this.RowCount; i++)
            {
                ModifyRecipientRow modifyRecipientRow = new ModifyRecipientRow(this.RecipientColumns);
                modifyRecipientRow.Parse(s);
                modifyRecipientRows.Add(modifyRecipientRow);
            }

            this.RecipientRows = modifyRecipientRows.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the ModifyRecipientRow structure.
    /// </summary>
    public class ModifyRecipientRow : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the ID of the recipient (1).
        /// </summary>
        public uint RowId;

        /// <summary>
        /// An enumeration that specifies the type of recipient (1).
        /// </summary>
        public byte RecipientType;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public ushort RecipientRowSize;

        /// <summary>
        /// A RecipientRow structure.
        /// </summary>
        public RecipientRow RecipientRow;

        /// <summary>
        /// A parameter for construct function
        /// </summary>
        private PropertyTag[] propTags;

        /// <summary>
        /// Initializes a new instance of the ModifyRecipientRow class.
        /// </summary>
        /// <param name="propTags">The initialized parameter</param>
        public ModifyRecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the ModifyRecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing ModifyRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RowId = this.ReadUint();
            this.RecipientType = this.ReadByte();
            this.RecipientRowSize = this.ReadUshort();

            if (this.RecipientRowSize > 0)
            {
                this.RecipientRow = new RecipientRow(this.propTags);
                this.RecipientRow.Parse(s);
            }
        }
    }

    /// <summary>
    /// A class indicates the RopModifyRecipients ROP response Buffer.
    /// </summary>
    public class RopModifyRecipientsResponse : BaseStructure
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
        /// Parse the RopModifyRecipientsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopModifyRecipientsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.6.6	RopReadRecipients ROP
    /// <summary>
    /// A class indicates the RopReadRecipients ROP request Buffer.
    /// </summary>
    public class RopReadRecipientsRequest : BaseStructure
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
        /// An unsigned integer that specifies the starting index for the recipients (2) to be retrieved.
        /// </summary>
        public uint RowId;

        /// <summary>
        /// Reserved. This field MUST be set to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Parse the RopReadRecipientsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.RowId = this.ReadUint();
            this.Reserved = this.ReadUshort();
        }
    }

    /// <summary>
    /// A class indicates the RopReadRecipients ROP response Buffer.
    /// </summary>
    public class RopReadRecipientsResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of structures in the RecipientRows field.
        /// </summary>
        public byte? RowCount;

        /// <summary>
        /// A list of ReadRecipientRow structures. 
        /// </summary>
        public ReadRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopReadRecipientsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReadRecipientsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = this.ReadByte();
                List<ReadRecipientRow> readRecipientRows = new List<ReadRecipientRow>();

                for (int i = 0; i < this.RowCount; i++)
                {
                    ReadRecipientRow readRecipientRow = new ReadRecipientRow();
                    readRecipientRow.Parse(s);
                    readRecipientRows.Add(readRecipientRow);
                }

                this.RecipientRows = readRecipientRows.ToArray();
            }
        }
    }

    /// <summary>
    /// A class indicates the ReadRecipientRow structure.
    /// </summary>
    public class ReadRecipientRow : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the row ID of the recipient (2).
        /// </summary>
        public uint RowId;

        /// <summary>
        /// An enumeration that specifies the type of recipient (2).
        /// </summary>
        public byte RecipientType;

        /// <summary>
        /// An identifier that specifies the code page for the recipient (2).
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// Reserved. The server MUST set this field to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size of the RecipientRow field.
        /// </summary>
        public ushort RecipientRowSize;

        /// <summary>
        /// A RecipientRow structure. //TODO: put the raw bytes here temporarily and we need to refine it later once we get the key which is required by RecipientRow.
        /// </summary>
        public byte[] RecipientRow;

        /// <summary>
        /// Parse the ReadRecipientRow structure.
        /// </summary>
        /// <param name="s">A stream containing ReadRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RowId = this.ReadUint();
            this.RecipientType = this.ReadByte();
            this.CodePageId = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.RecipientRowSize = this.ReadUshort();
            this.RecipientRow = this.ReadBytes(this.RecipientRowSize);
        }
    }
    #endregion

    #region 2.2.6.7	RopReloadCachedInformation ROP

    /// <summary>
    /// A class indicates the RopReloadCachedInformation ROP request Buffer.
    /// </summary>
    public class RopReloadCachedInformationRequest : BaseStructure
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
        /// Reserved. This field MUST be set to 0x0000.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Parse the RopReloadCachedInformationRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopReloadCachedInformationRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Reserved = this.ReadUshort();
        }
    }

    /// <summary>
    /// A class indicates the RopReloadCachedInformation ROP response Buffer.
    /// </summary>
    public class RopReloadCachedInformationResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex specified field in the request.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the message has named properties.
        /// </summary>
        public bool? HasNamedProperties;

        /// <summary>
        /// A TypedString structure that specifies the subject prefix of the message.
        /// </summary>
        public TypedString SubjectPrefix;

        /// <summary>
        /// A TypedString structure that specifies the normalized subject of the message.
        /// </summary>
        public TypedString NormalizedSubject;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public ushort? RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public ushort? ColumnCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that can be included for each recipient (2).
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public byte? RowCount;

        /// <summary>
        /// A list of OpenRecipientRow structures.
        /// </summary>
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopReloadCachedInformationResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopReloadCachedInformationResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.HasNamedProperties = this.ReadBoolean();
                this.SubjectPrefix = new TypedString();
                this.SubjectPrefix.Parse(s);
                this.NormalizedSubject = new TypedString();
                this.NormalizedSubject.Parse(s);
                this.RecipientCount = this.ReadUshort();
                this.ColumnCount = this.ReadUshort();
                List<PropertyTag> propertyTags = new List<PropertyTag>();

                for (int i = 0; i < this.ColumnCount; i++)
                {
                    PropertyTag propertyTag = Block.Parse<PropertyTag>(s);
                    propertyTags.Add(propertyTag);
                }

                this.RecipientColumns = propertyTags.ToArray();
                this.RowCount = this.ReadByte();
                List<OpenRecipientRow> openRecipientRows = new List<OpenRecipientRow>();

                for (int i = 0; i < this.RowCount; i++)
                {
                    OpenRecipientRow openRecipientRow = new OpenRecipientRow(this.RecipientColumns);
                    openRecipientRow.Parse(s);
                    openRecipientRows.Add(openRecipientRow);
                }

                this.RecipientRows = openRecipientRows.ToArray();
            }
        }
    }
    #endregion

    #region 2.2.6.8	RopSetMessageStatus ROP
    /// <summary>
    /// A class indicates the RopSetMessageStatus ROP request Buffer.
    /// </summary>
    public class RopSetMessageStatusRequest : BaseStructure
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
        /// An identifier that specifies the message for which the status will be changed.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// A flags structure that contains status flags to set on the message.
        /// </summary>
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// A bitmask that specifies which bits in the MessageStatusFlags field are to be changed.
        /// </summary>
        public uint MessageStatusMask;

        /// <summary>
        /// Parse the RopSetMessageStatusRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageStatusRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
            this.MessageStatusFlags = (MessageStatusFlag)this.ReadUint();
            this.MessageStatusMask = this.ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopSetMessageStatus ROP response Buffer.
    /// </summary>
    public class RopSetMessageStatusResponse : BaseStructure
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
        /// A flags structure that contains the status flags that were set on the message before this operation.
        /// </summary>
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// Parse the RopSetMessageStatusResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageStatusResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.MessageStatusFlags = (MessageStatusFlag)this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.6.9	RopGetMessageStatus ROP
    /// <summary>
    /// A class indicates the RopGetMessageStatus ROP request Buffer.
    /// </summary>
    public class RopGetMessageStatusRequest : BaseStructure
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
        /// An identifier that specifies the message for which the status will be returned.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopGetMessageStatusRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetMessageStatusRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }
    }

    /// <summary>
    /// A class indicates the RopGetMessageStatus ROP response Buffer.
    /// </summary>
    public class RopGetMessageStatusResponse : BaseStructure
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
        /// A flags structure that contains the status flags that were set on the message before this operation.
        /// </summary>
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// Parse the RopGetMessageStatusResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetMessageStatusResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.MessageStatusFlags = (MessageStatusFlag)this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.6.10 RopSetReadFlags ROP
    /// <summary>
    /// A class indicates the RopSetReadFlags ROP request Buffer.
    /// </summary>
    public class RopSetReadFlagsRequest : BaseStructure
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
        /// A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        /// </summary>
        public bool WantAsynchronous;

        /// <summary>
        /// A flags structure that contains flags that specify the flags to set.
        /// </summary>
        public ReadFlags ReadFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of identifiers in the MessageIds field.
        /// </summary>
        public ushort MessageIdCount;

        /// <summary>
        /// An array of 64-bit identifiers that specify the messages that are to have their read flags changed.
        /// </summary>
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopSetReadFlagsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetReadFlagsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.WantAsynchronous = this.ReadBoolean();
            this.ReadFlags = (ReadFlags)this.ReadByte();
            this.MessageIdCount = this.ReadUshort();
            List<MessageID> messageIDs = new List<MessageID>();

            for (int i = 0; i < this.MessageIdCount; i++)
            {
                MessageID messageID = new MessageID();
                messageID.Parse(s);
                messageIDs.Add(messageID);
            }

            this.MessageIds = messageIDs.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the RopSetReadFlags ROP response Buffer.
    /// </summary>
    public class RopSetReadFlagsResponse : BaseStructure
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
        /// A Boolean that indicates whether the operation was only partially completed. 
        /// </summary>
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopSetReadFlagsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetReadFlagsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
            this.PartialCompletion = this.ReadBoolean();
        }
    }

    #endregion

    #region 2.2.6.11 RopSetMessageReadFlag ROP
    /// <summary>
    /// A class indicates the RopSetMessageReadFlag ROP request Buffer.
    /// </summary>
    public class RopSetMessageReadFlagRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response. 
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        /// </summary>
        public ReadFlags ReadFlags;

        /// <summary>
        /// An array of bytes that is present when the RopLogon associated with LogonId was created with the Private flag
        /// </summary>
        public byte?[] ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageReadFlagRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReadFlags = (ReadFlags)this.ReadByte();
            if(!MapiInspector.MAPIParser.IsFromFiddlerCore(MapiInspector.MAPIParser.ParsingSession))
            {
                if (((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIParser.ParsingSession.id][this.LogonId] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private)
                {
                    this.ClientData = this.ConvertArray(this.ReadBytes(24));
                }
            }
            else
            {
                if (((byte)DecodingContext.SessionLogonFlagMapLogId[int.Parse(MapiInspector.MAPIParser.ParsingSession["VirtualID"])][this.LogonId] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private)
                {
                    this.ClientData = this.ConvertArray(this.ReadBytes(24));
                }
            }
        }
    }

    /// <summary>
    /// A class indicates the RopSetMessageReadFlag ROP response Buffer.
    /// </summary>
    public class RopSetMessageReadFlagResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the read status of a public folder's message has changed.
        /// </summary>
        public bool? ReadStatusChanged;

        /// <summary>
        /// An unsigned integer index that is present when the value in the ReadStatusChanged field is nonzero and is not present
        /// </summary>
        public byte? LogonId;

        /// <summary>
        /// An array of bytes that is present when the value in the ReadStatusChanged field is nonzero and is not present 
        /// </summary>
        public byte?[] ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetMessageReadFlagResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.ReadStatusChanged = this.ReadBoolean();

                if ((bool)this.ReadStatusChanged)
                {
                    this.LogonId = this.ReadByte();
                    this.ClientData = this.ConvertArray(this.ReadBytes(24));
                }
            }
        }
    }
    #endregion

    #region 2.2.6.12 RopOpenAttachment ROP
    /// <summary>
    /// A class indicates the RopOpenAttachment ROP request Buffer.
    /// </summary>
    public class RopOpenAttachmentRequest : BaseStructure
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
        /// A flags structure that contains flags for opening attachments.
        /// </summary>
        public OpenAttachmentFlags OpenAttachmentFlags;

        /// <summary>
        /// An unsigned integer index that identifies the attachment to be opened. 
        /// </summary>
        public uint AttachmentID;

        /// <summary>
        /// Parse the RopOpenAttachmentRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.OpenAttachmentFlags = (OpenAttachmentFlags)this.ReadByte();
            this.AttachmentID = this.ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopOpenAttachment ROP response Buffer.
    /// </summary>
    public class RopOpenAttachmentResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopOpenAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.6.13 RopCreateAttachment ROP
    /// <summary>
    /// A class indicates the RopCreateAttachment ROP request Buffer.
    /// </summary>
    public class RopCreateAttachmentRequest : BaseStructure
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
        /// Parse the RopCreateAttachmentRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateAttachmentRequest structure</param>
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
    /// A class indicates the RopCreateAttachment ROP response Buffer.
    /// </summary>
    public class RopCreateAttachmentResponse : BaseStructure
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
        /// An unsigned integer identifier that refers to the attachment created.
        /// </summary>
        public uint? AttachmentID;

        /// <summary>
        /// Parse the RopCreateAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.AttachmentID = this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.6.14 RopDeleteAttachment ROP
    /// <summary>
    /// A class indicates the RopDeleteAttachment ROP request Buffer.
    /// </summary>
    public class RopDeleteAttachmentRequest : BaseStructure
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
        /// An unsigned integer that identifies the attachment to be deleted. 
        /// </summary>
        public uint AttachmentID;

        /// <summary>
        /// Parse the RopDeleteAttachmentRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.AttachmentID = this.ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopDeleteAttachment ROP response Buffer.
    /// </summary>
    public class RopDeleteAttachmentResponse : BaseStructure
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
        /// Parse the RopDeleteAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopDeleteAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.6.15 RopSaveChangesAttachment ROP
    /// <summary>
    /// A class indicates the RopSaveChangesAttachment ROP request Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentRequest : BaseStructure
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
        /// An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response. 
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        /// </summary>
        public SaveFlags SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesAttachmentRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.SaveFlags = (SaveFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopSaveChangesAttachment ROP response Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        /// </summary>
        public byte ResponseHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP. 
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSaveChangesAttachmentResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSaveChangesAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.ResponseHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.6.16 RopOpenEmbeddedMessage ROP
    /// <summary>
    /// A class indicates the RopOpenEmbeddedMessage ROP request Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageRequest : BaseStructure
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
        /// An identifier that specifies which code page is used for string values associated with the message.
        /// </summary>
        public ushort CodePageId;

        /// <summary>
        /// A flags structure that contains flags that control the access to the message.
        /// </summary>
        public OpenMessageModeFlags OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenEmbeddedMessageRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.CodePageId = this.ReadUshort();
            this.OpenModeFlags = (OpenMessageModeFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopOpenEmbeddedMessage ROP response Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageResponse : BaseStructure
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
        /// Reserved. This field MUST be set to 0x00.
        /// </summary>
        public byte? Reserved;

        /// <summary>
        /// An identifier that specifies the ID of the Embedded Message object.
        /// </summary>
        public MessageID MessageId;

        /// <summary>
        /// A Boolean that specifies whether the message has named properties.
        /// </summary>
        public bool? HasNamedProperties;

        /// <summary>
        /// A TypedString structure that specifies the subject prefix of the message.
        /// </summary>
        public TypedString SubjectPrefix;

        /// <summary>
        /// A TypedString structure that specifies the normalized subject of the message.
        /// </summary>
        public TypedString NormalizedSubject;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public ushort? RecipientCount;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RecipientColumns field.
        /// </summary>
        public ushort? ColumnCount;

        /// <summary>
        /// An unsigned integer that specifies the number of recipients (2) on the message.
        /// </summary>
        public PropertyTag[] RecipientColumns;

        /// <summary>
        /// An unsigned integer that specifies the number of rows in the RecipientRows field.
        /// </summary>
        public byte? RowCount;

        /// <summary>
        /// A list of OpenRecipientRow structures.
        /// </summary>
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopOpenEmbeddedMessageResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.Reserved = this.ReadByte();
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
                this.HasNamedProperties = this.ReadBoolean();
                this.SubjectPrefix = new TypedString();
                this.SubjectPrefix.Parse(s);
                this.NormalizedSubject = new TypedString();
                this.NormalizedSubject.Parse(s);
                this.RecipientCount = this.ReadUshort();
                this.ColumnCount = this.ReadUshort();
                List<PropertyTag> propertyTags = new List<PropertyTag>();

                for (int i = 0; i < this.ColumnCount; i++)
                {
                    PropertyTag propertyTag = Block.Parse<PropertyTag>(s);
                    propertyTags.Add(propertyTag);
                }

                this.RecipientColumns = propertyTags.ToArray();
                this.RowCount = this.ReadByte();
                List<OpenRecipientRow> openRecipientRows = new List<OpenRecipientRow>();

                for (int i = 0; i < this.RowCount; i++)
                {
                    OpenRecipientRow openRecipientRow = new OpenRecipientRow(this.RecipientColumns);
                    openRecipientRow.Parse(s);
                    openRecipientRows.Add(openRecipientRow);
                }

                this.RecipientRows = openRecipientRows.ToArray();
            }
        }
    }

    #endregion

    #region 2.2.6.17 RopGetAttachmentTable ROP
    /// <summary>
    /// A class indicates the RopGetAttachmentTable ROP request Buffer.
    /// </summary>
    public class RopGetAttachmentTableRequest : BaseStructure
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
        public GetAttachmentTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetAttachmentTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAttachmentTableRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.TableFlags = (GetAttachmentTableFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopGetAttachmentTable ROP response Buffer.
    /// </summary>
    public class RopGetAttachmentTableResponse : BaseStructure
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
        /// Parse the RopGetAttachmentTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetAttachmentTableResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }

    #endregion

    #region 2.2.6.18 RopGetValidAttachments ROP
    /// <summary>
    /// A class indicates the RopGetValidAttachments ROP request Buffer.
    /// </summary>
    public class RopGetValidAttachmentsRequest : BaseStructure
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
        /// Parse the RopGetValidAttachmentsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetValidAttachmentsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopGetValidAttachments ROP response Buffer.
    /// </summary>
    public class RopGetValidAttachmentsResponse : BaseStructure
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
        /// An unsigned integer that specifies the number of integers in the AttachmentIdArray field.
        /// </summary>
        public ushort? AttachmentIdCount;

        /// <summary>
        /// An array of 32-bit integers that represent the valid attachment identifiers of the message. 
        /// </summary>
        public int?[] AttachmentIdArray;

        /// <summary>
        /// Parse the RopGetValidAttachmentsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetValidAttachmentsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)this.ReadByte();
            this.OutputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.AttachmentIdCount = this.ReadUshort();
                List<int> attachmentIdArrays = new List<int>();

                for (int i = 0; i < this.AttachmentIdCount; i++)
                {
                    attachmentIdArrays.Add(this.ReadINT32());
                }

                this.AttachmentIdArray = this.ConvertArray(attachmentIdArrays.ToArray());
            }
        }
    }
    #endregion

    /// <summary>
    /// An enumeration that specifies the type of recipient (2).
    /// </summary>
    public class RecipientType : BaseStructure
    {
        /// <summary>
        /// RecipientType flag
        /// </summary>
        [BitAttribute(4)]
        public RecipientTypeFlag Flag;

        /// <summary>
        /// RecipientType type
        /// </summary>
        [BitAttribute(4)]
        public RecipientTypeType Type;

        /// <summary>
        /// Parse RecipientType structure
        /// </summary>
        /// <param name="s">A stream containing RecipientType structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte bitWise = this.ReadByte();
            this.Flag = (RecipientTypeFlag)(bitWise & 0xF0);
            this.Type = (RecipientTypeType)(bitWise & 0x0F);
        }
    }
}
