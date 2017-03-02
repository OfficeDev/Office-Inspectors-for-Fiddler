using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region 2.2.3.1	RopOpenMessage
    /// <summary>
    ///  A class indicates the RopOpenMessage ROP Request Buffer.
    /// </summary>
    public class RopOpenMessageRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // An identifier that specifies which code page will be used for string values associated with the message.
        public short CodePageId;

        // An identifier that identifies the parent folder of the message to be opened.
        public FolderID FolderId;

        // A flags structure that contains flags that control the access to the message. 
        public OpenMessageModeFlags OpenModeFlags;

        //  An identifier that identifies the message to be opened.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopOpenMessageRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.CodePageId = ReadINT16();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.OpenModeFlags = (OpenMessageModeFlags)ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }
    }


    /// <summary>
    ///  A class indicates the RopOpenMessage ROP response Buffer.
    /// </summary>
    public class RopOpenMessageResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        //  A Boolean that specifies whether the message has named properties.
        public bool? HasNamedProperties;

        // A TypedString structure that specifies the subject prefix of the message. 
        public TypedString SubjectPrefix;

        // A TypedString structure that specifies the normalized subject of the message. 
        public TypedString NormalizedSubject;

        // An unsigned integer that specifies the number of recipients (1) on the message.
        public ushort? RecipientCount;

        // An unsigned integer that specifies the number of structures in the RecipientColumns field.
        public ushort? ColumnCount;

        // An array of PropertyTag structures that specifies the property values that can be included in each row that is specified in the RecipientRows field. 
        public PropertyTag[] RecipientColumns;

        // An unsigned integer that specifies the number of structures in the RecipientRows field.
        public byte? RowCount;

        // A list of OpenRecipientRow structures. 
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenMessageResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.HasNamedProperties = ReadBoolean();
                this.SubjectPrefix = new TypedString();
                this.SubjectPrefix.Parse(s);
                this.NormalizedSubject = new TypedString();
                this.NormalizedSubject.Parse(s);
                this.RecipientCount = ReadUshort();
                this.ColumnCount = ReadUshort();
                List<PropertyTag> PropertyTags = new List<PropertyTag>();
                for (int i = 0; i < this.ColumnCount; i++)
                {
                    PropertyTag propertyTag = new PropertyTag();
                    propertyTag.Parse(s);
                    PropertyTags.Add(propertyTag);
                }
                this.RecipientColumns = PropertyTags.ToArray();
                this.RowCount = ReadByte();
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
        // Array of PropertyTag used to initialize the class.
        private PropertyTag[] propTags;

        // An enumeration that specifies the type of recipient (2). 
        public RecipientType RecipientType;

        // An identifier that specifies the code page for the recipient (2).
        public ushort CodePageId;

        // Reserved. The server MUST set this field to 0x0000.
        public ushort Reserved;

        // An unsigned integer that specifies the size of the RecipientRow field.
        public ushort RecipientRowSize;

        //  A RecipientRow structure. 
        public RecipientRow RecipientRow;

        /// <summary>
        /// The OpenRecipientRow construct function
        /// </summary>
        /// <param name="propTags">Array of PropertyTag used to initialize the class.</param>
        public OpenRecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the OpenRecipientRow structure.
        /// </summary>
        /// <param name="s">An stream containing OpenRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RecipientType = new RecipientType();
            this.RecipientType.Parse(s);
            this.CodePageId = ReadUshort();
            this.Reserved = ReadUshort();
            this.RecipientRowSize = ReadUshort();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // An identifier that specifies the code page for the message.
        public ushort CodePageId;

        // An identifier that specifies the parent folder.
        public FolderID FolderId;

        // A Boolean that specifies whether the message is an FAI message.
        public bool AssociatedFlag;

        /// <summary>
        /// Parse the RopCreateMessageRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.CodePageId = ReadUshort();
            this.FolderId = new FolderID();
            this.FolderId.Parse(s);
            this.AssociatedFlag = ReadBoolean();
        }
    }

    /// <summary>
    /// A class indicates the RopCreateMessage ROP response Buffer.
    /// </summary>
    public class RopCreateMessageResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex specified in field the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the MessageId field is present.
        public bool? HasMessageId;

        // An identifier that is present if HasMessageId is nonzero and is not present if it is zero.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopCreateMessageResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateMessageResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.HasMessageId = ReadBoolean();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response.
        public byte ResponseHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A flags structure that contains flags that specify how the save operation behaves.
        public SaveFlags SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesMessageRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSaveChangesMessageRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.ResponseHandleIndex = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.SaveFlags = (SaveFlags)ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopSaveChangesMessage ROP response Buffer.
    /// </summary>
    public class RopSaveChangesMessageResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        public byte ResponseHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte? InputHandleIndex;

        // An identifier that specifies the ID of the message saved.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopSaveChangesMessageResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSaveChangesMessageResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.ResponseHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.InputHandleIndex = ReadByte();
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
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // Reserved. The client SHOULD set this field to 0x00000000. 
        public uint Reserved;

        /// <summary>
        /// Parse the RopRemoveAllRecipientsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopRemoveAllRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Reserved = ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopRemoveAllRecipients ROP response Buffer.
    /// </summary>
    public class RopRemoveAllRecipientsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopRemoveAllRecipientsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopRemoveAllRecipientsResponse structure</param>
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

    #region 2.2.6.5	RopModifyRecipients ROP

    /// <summary>
    /// A class indicates the RopModifyRecipients ROP request Buffer.
    /// </summary>
    public class RopModifyRecipientsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the number of structures in the RecipientColumns field.
        public ushort ColumnCount;

        // An array of PropertyTag structures that specifies the property values that can be included for each recipient (1).
        public PropertyTag[] RecipientColumns;

        // An unsigned integer that specifies the number of rows in the RecipientRows field.
        public ushort RowCount;

        // A list of ModifyRecipientRow structures.
        public ModifyRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopModifyRecipientsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopModifyRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ColumnCount = ReadUshort();
            List<PropertyTag> propertyTags = new List<PropertyTag>();
            for (int i = 0; i < ColumnCount; i++)
            {
                PropertyTag propertyTag = new PropertyTag();
                propertyTag.Parse(s);
                propertyTags.Add(propertyTag);
            }
            this.RecipientColumns = propertyTags.ToArray();
            this.RowCount = ReadUshort();
            List<ModifyRecipientRow> modifyRecipientRows = new List<ModifyRecipientRow>();
            for (int i = 0; i < RowCount; i++)
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
        // An unsigned integer that specifies the ID of the recipient (1).
        public uint RowId;

        // An enumeration that specifies the type of recipient (1).
        public byte RecipientType;

        // An unsigned integer that specifies the size of the RecipientRow field.
        public ushort RecipientRowSize;

        // A RecipientRow structure.
        public RecipientRow RecipientRow;

        // A parameter for construct function
        private PropertyTag[] propTags;

        /// <summary>
        /// The construct function for ModifyRecipientRow
        /// </summary>
        /// <param name="propTags">The initialized parameter</param>
        public ModifyRecipientRow(PropertyTag[] propTags)
        {
            this.propTags = propTags;
        }

        /// <summary>
        /// Parse the ModifyRecipientRow structure.
        /// </summary>
        /// <param name="s">An stream containing ModifyRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RowId = ReadUint();
            this.RecipientType = ReadByte();
            this.RecipientRowSize = ReadUshort();
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
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopModifyRecipientsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopModifyRecipientsResponse structure</param>
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

    #region 2.2.6.6	RopReadRecipients ROP
    /// <summary>
    /// A class indicates the RopReadRecipients ROP request Buffer.
    /// </summary>
    public class RopReadRecipientsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the starting index for the recipients (2) to be retrieved.
        public uint RowId;

        // Reserved. This field MUST be set to 0x0000.
        public ushort Reserved;

        /// <summary>
        /// Parse the RopReadRecipientsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopReadRecipientsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.RowId = ReadUint();
            this.Reserved = ReadUshort();
        }
    }

    /// <summary>
    /// A class indicates the RopReadRecipients ROP response Buffer.
    /// </summary>
    public class RopReadRecipientsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the number of structures in the RecipientRows field.
        public byte? RowCount;

        // A list of ReadRecipientRow structures. 
        public ReadRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopReadRecipientsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopReadRecipientsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowCount = ReadByte();
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
        // An unsigned integer that specifies the row ID of the recipient (2).
        public uint RowId;

        // An enumeration that specifies the type of recipient (2).
        public byte RecipientType;

        // An identifier that specifies the code page for the recipient (2).
        public ushort CodePageId;

        // Reserved. The server MUST set this field to 0x0000.
        public ushort Reserved;

        // An unsigned integer that specifies the size of the RecipientRow field.
        public ushort RecipientRowSize;

        // A RecipientRow structure. //TODO: put the raw bytes here temporarily and we need to refine it later once we get the key which is required by RecipientRow.
        public byte[] RecipientRow;

        /// <summary>
        /// Parse the ReadRecipientRow structure.
        /// </summary>
        /// <param name="s">An stream containing ReadRecipientRow structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RowId = ReadUint();
            this.RecipientType = ReadByte();
            this.CodePageId = ReadUshort();
            this.Reserved = ReadUshort();
            this.RecipientRowSize = ReadUshort();
            this.RecipientRow = ReadBytes(this.RecipientRowSize);
        }
    }
    #endregion

    #region 2.2.6.7	RopReloadCachedInformation ROP

    /// <summary>
    /// A class indicates the RopReloadCachedInformation ROP request Buffer.
    /// </summary>
    public class RopReloadCachedInformationRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // Reserved. This field MUST be set to 0x0000.
        public ushort Reserved;

        /// <summary>
        /// Parse the RopReloadCachedInformationRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopReloadCachedInformationRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Reserved = ReadUshort();
        }
    }

    /// <summary>
    /// A class indicates the RopReloadCachedInformation ROP response Buffer.
    /// </summary>
    public class RopReloadCachedInformationResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex specified field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the message has named properties.
        public bool? HasNamedProperties;

        // A TypedString structure that specifies the subject prefix of the message.
        public TypedString SubjectPrefix;

        // A TypedString structure that specifies the normalized subject of the message.
        public TypedString NormalizedSubject;

        // An unsigned integer that specifies the number of recipients (2) on the message.
        public ushort? RecipientCount;

        // An unsigned integer that specifies the number of structures in the RecipientColumns field.
        public ushort? ColumnCount;

        // An array of PropertyTag structures that specifies the property values that can be included for each recipient (2).
        public PropertyTag[] RecipientColumns;

        // An unsigned integer that specifies the number of rows in the RecipientRows field.
        public byte? RowCount;

        // A list of OpenRecipientRow structures.
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopReloadCachedInformationResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopReloadCachedInformationResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.HasNamedProperties = ReadBoolean();
                this.SubjectPrefix = new TypedString();
                this.SubjectPrefix.Parse(s);
                this.NormalizedSubject = new TypedString();
                this.NormalizedSubject.Parse(s);
                this.RecipientCount = ReadUshort();
                this.ColumnCount = ReadUshort();
                List<PropertyTag> propertyTags = new List<PropertyTag>();
                for (int i = 0; i < this.ColumnCount; i++)
                {
                    PropertyTag propertyTag = new PropertyTag();
                    propertyTag.Parse(s);
                    propertyTags.Add(propertyTag);
                }
                this.RecipientColumns = propertyTags.ToArray();
                this.RowCount = ReadByte();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        //  An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An identifier that specifies the message for which the status will be changed.
        public MessageID MessageId;

        // A flags structure that contains status flags to set on the message.
        public MessageStatusFlag MessageStatusFlags;

        // A bitmask that specifies which bits in the MessageStatusFlags field are to be changed.
        public uint MessageStatusMask;

        /// <summary>
        /// Parse the RopSetMessageStatusRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetMessageStatusRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
            this.MessageStatusFlags = (MessageStatusFlag)ReadUint();
            this.MessageStatusMask = ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopSetMessageStatus ROP response Buffer.
    /// </summary>
    public class RopSetMessageStatusResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A flags structure that contains the status flags that were set on the message before this operation.
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// Parse the RopSetMessageStatusResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetMessageStatusResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.MessageStatusFlags = (MessageStatusFlag)ReadUint();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        //  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An identifier that specifies the message for which the status will be returned.
        public MessageID MessageId;

        /// <summary>
        /// Parse the RopGetMessageStatusRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetMessageStatusRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MessageId = new MessageID();
            this.MessageId.Parse(s);
        }
    }

    /// <summary>
    /// A class indicates the RopGetMessageStatus ROP response Buffer.
    /// </summary>
    public class RopGetMessageStatusResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A flags structure that contains the status flags that were set on the message before this operation.
        public MessageStatusFlag MessageStatusFlags;

        /// <summary>
        /// Parse the RopGetMessageStatusResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetMessageStatusResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.MessageStatusFlags = (MessageStatusFlag)ReadUint();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        //  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A Boolean that specifies whether the operation is to be processed asynchronously with status reported via the RopProgress ROP (section 2.2.8.13).
        public bool WantAsynchronous;

        // A flags structure that contains flags that specify the flags to set.
        public ReadFlags ReadFlags;

        // An unsigned integer that specifies the number of identifiers in the MessageIds field.
        public ushort MessageIdCount;

        // An array of 64-bit identifiers that specify the messages that are to have their read flags changed.
        public MessageID[] MessageIds;

        /// <summary>
        /// Parse the RopSetReadFlagsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetReadFlagsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.WantAsynchronous = ReadBoolean();
            this.ReadFlags = (ReadFlags)ReadByte();
            this.MessageIdCount = ReadUshort();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that indicates whether the operation was only partially completed. 
        public bool PartialCompletion;

        /// <summary>
        /// Parse the RopSetReadFlagsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetReadFlagsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            this.PartialCompletion = ReadBoolean();
        }

    }
    #endregion

    #region 2.2.6.11 RopSetMessageReadFlag ROP
    /// <summary>
    /// A class indicates the RopSetMessageReadFlag ROP request Buffer.
    /// </summary>
    public class RopSetMessageReadFlagRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response. 
        public byte ResponseHandleIndex;

        //  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        public ReadFlags ReadFlags;

        // An array of bytes that is present when the logon associated with LogonId was created with the Private flag
        public byte?[] ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetMessageReadFlagRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.ResponseHandleIndex = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.ReadFlags = (ReadFlags)this.ReadByte();
            if ((((byte)DecodingContext.SessionLogonFlagMapLogId[MapiInspector.MAPIInspector.currentParsingSessionID][this.LogonId] & (byte)LogonFlags.Private) != (byte)LogonFlags.Private))
            {
                // Both public mode and private mode don't contain ClientData element
                // this.ClientData = ConvertArray(ReadBytes(24));
            }
        }
    }

    /// <summary>
    /// A class indicates the RopSetMessageReadFlag ROP response Buffer.
    /// </summary>
    public class RopSetMessageReadFlagResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        public byte ResponseHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        // A Boolean that specifies whether the read status of a public folder's message has changed.
        public bool? ReadStatusChanged;

        // An unsigned integer index that is present when the value in the ReadStatusChanged field is nonzero and is not present
        public byte? LogonId;

        // An array of bytes that is present when the value in the ReadStatusChanged field is nonzero and is not present 
        public byte?[] ClientData;

        /// <summary>
        /// Parse the RopSetMessageReadFlagResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetMessageReadFlagResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.ResponseHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.ReadStatusChanged = ReadBoolean();
                if ((bool)this.ReadStatusChanged)
                {
                    this.LogonId = ReadByte();
                    this.ClientData = ConvertArray(ReadBytes(24));
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        //  An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        // A flags structure that contains flags for opening attachments.
        public OpenAttachmentFlags OpenAttachmentFlags;

        // An unsigned integer index that identifies the attachment to be opened. 
        public uint AttachmentID;

        /// <summary>
        /// Parse the RopOpenAttachmentRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.OpenAttachmentFlags = (OpenAttachmentFlags)ReadByte();
            this.AttachmentID = ReadUint();
        }

    }

    /// <summary>
    /// A class indicates the RopOpenAttachment ROP response Buffer.
    /// </summary>
    public class RopOpenAttachmentResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        public byte ResponseHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        /// <summary>
        /// Parse the RopOpenAttachmentResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.ResponseHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.6.13 RopCreateAttachment ROP
    /// <summary>
    /// A class indicates the RopCreateAttachment ROP request Buffer.
    /// </summary>
    public class RopCreateAttachmentRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        //  An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored. 
        public byte OutputHandleIndex;

        /// <summary>
        /// Parse the RopCreateAttachmentRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateAttachmentRequest structure</param>
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
    /// A class indicates the RopCreateAttachment ROP response Buffer.
    /// </summary>
    public class RopCreateAttachmentResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        // An unsigned integer identifier that refers to the attachment created.
        public uint? AttachmentID;

        /// <summary>
        /// Parse the RopCreateAttachmentResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.AttachmentID = ReadUint();
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
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        //  An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // An unsigned integer that identifies the attachment to be deleted. 
        public uint AttachmentID;

        /// <summary>
        /// Parse the RopDeleteAttachmentRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeleteAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.AttachmentID = ReadUint();
        }
    }

    /// <summary>
    /// A class indicates the RopDeleteAttachment ROP response Buffer.
    /// </summary>
    public class RopDeleteAttachmentResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;


        /// <summary>
        /// Parse the RopDeleteAttachmentResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopDeleteAttachmentResponse structure</param>
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

    #region 2.2.6.15 RopSaveChangesAttachment ROP
    /// <summary>
    /// A class indicates the RopSaveChangesAttachment ROP request Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table that is referenced in the response. 
        public byte ResponseHandleIndex;

        //  An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure. The possible values for these flags are specified in [MS-OXCMSG] section 2.2.3.11.1.
        public SaveFlags SaveFlags;

        /// <summary>
        /// Parse the RopSaveChangesAttachmentRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSaveChangesAttachmentRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.ResponseHandleIndex = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.SaveFlags = (SaveFlags)this.ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopSaveChangesAttachment ROP response Buffer.
    /// </summary>
    public class RopSaveChangesAttachmentResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the ResponseHandleIndex field in the request.
        public byte ResponseHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSaveChangesAttachmentResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSaveChangesAttachmentResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.ResponseHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
        }
    }
    #endregion

    #region 2.2.6.16 RopOpenEmbeddedMessage ROP
    /// <summary>
    /// A class indicates the RopOpenEmbeddedMessage ROP request Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // An identifier that specifies which code page is used for string values associated with the message.
        public ushort CodePageId;

        // A flags structure that contains flags that control the access to the message.
        public OpenMessageModeFlags OpenModeFlags;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenEmbeddedMessageRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.CodePageId = ReadUshort();
            this.OpenModeFlags = (OpenMessageModeFlags)ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopOpenEmbeddedMessage ROP response Buffer.
    /// </summary>
    public class RopOpenEmbeddedMessageResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        // Reserved. This field MUST be set to 0x00.
        public byte? Reserved;

        // An identifier that specifies the ID of the Embedded Message object.
        public MessageID MessageId;

        // A Boolean that specifies whether the message has named properties.
        public bool? HasNamedProperties;

        // A TypedString structure that specifies the subject prefix of the message.
        public TypedString SubjectPrefix;

        // A TypedString structure that specifies the normalized subject of the message.
        public TypedString NormalizedSubject;

        // An unsigned integer that specifies the number of recipients (2) on the message.
        public ushort? RecipientCount;

        // An unsigned integer that specifies the number of structures in the RecipientColumns field.
        public ushort? ColumnCount;

        // An unsigned integer that specifies the number of recipients (2) on the message.
        public PropertyTag[] RecipientColumns;

        // An unsigned integer that specifies the number of rows in the RecipientRows field.
        public byte? RowCount;

        // A list of OpenRecipientRow structures.
        public OpenRecipientRow[] RecipientRows;

        /// <summary>
        /// Parse the RopOpenEmbeddedMessageResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopOpenEmbeddedMessageResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.Reserved = ReadByte();
                this.MessageId = new MessageID();
                this.MessageId.Parse(s);
                this.HasNamedProperties = ReadBoolean();
                this.SubjectPrefix = new TypedString();
                this.SubjectPrefix.Parse(s);
                this.NormalizedSubject = new TypedString();
                this.NormalizedSubject.Parse(s);
                this.RecipientCount = ReadUshort();
                this.ColumnCount = ReadUshort();
                List<PropertyTag> propertyTags = new List<PropertyTag>();
                for (int i = 0; i < this.ColumnCount; i++)
                {
                    PropertyTag propertyTag = new PropertyTag();
                    propertyTag.Parse(s);
                    propertyTags.Add(propertyTag);
                }
                this.RecipientColumns = propertyTags.ToArray();
                this.RowCount = ReadByte();
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
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the output Server object will be stored.
        public byte OutputHandleIndex;

        // A flags structure that contains flags that control the type of table. 
        public GetAttachmentTableFlags TableFlags;

        /// <summary>
        /// Parse the RopGetAttachmentTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetAttachmentTableRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.OutputHandleIndex = ReadByte();
            this.TableFlags = (GetAttachmentTableFlags)ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopGetAttachmentTable ROP response Buffer.
    /// </summary>
    public class RopGetAttachmentTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        /// <summary>
        /// Parse the RopGetAttachmentTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetAttachmentTableResponse structure</param>
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

    #region 2.2.6.18 RopGetValidAttachments ROP
    /// <summary>
    /// A class indicates the RopGetValidAttachments ROP reqeust Buffer.
    /// </summary>
    public class RopGetValidAttachmentsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP. 
        public RopIdType RopId;

        // An unsigned integer that specifies the logon associated with this operation.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetValidAttachmentsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetValidAttachmentsRequest structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    /// A class indicates the RopGetValidAttachments ROP response Buffer.
    /// </summary>
    public class RopGetValidAttachmentsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the OutputHandleIndex field in the request.
        public byte OutputHandleIndex;

        // An unsigned integer that specifies the status of the ROP. 
        public object ReturnValue;

        // An unsigned integer that specifies the number of integers in the AttachmentIdArray field.
        public ushort? AttachmentIdCount;

        // An array of 32-bit integers that represent the valid attachment identifiers of the message. 
        public int?[] AttachmentIdArray;

        /// <summary>
        /// Parse the RopGetValidAttachmentsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetValidAttachmentsResponse structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RopId = (RopIdType)ReadByte();
            this.OutputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());
            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.AttachmentIdCount = ReadUshort();
                List<int> attachmentIdArrays = new List<int>();
                for (int i = 0; i < this.AttachmentIdCount; i++)
                {
                    attachmentIdArrays.Add(ReadINT32());
                }
                this.AttachmentIdArray = ConvertArray(attachmentIdArrays.ToArray());
            }
        }
    }
    #endregion

    #region Enums
    /// <summary>
    /// The enum value of OpenModeFlags that contains flags that control the access to the message. 
    /// </summary>
    public enum OpenMessageModeFlags : byte
    {
        ReadOnly = 0x00,
        ReadWrite = 0x01,
        BestAccess = 0x03,
        OpenSoftDeleted = 0x04
    };

    /// <summary>
    /// An enumeration that specifies the type of recipient (2).
    /// </summary>
    public class RecipientType : BaseStructure
    {
        [BitAttribute(4)]
        public RecipientTypeFlag Flag;
        [BitAttribute(4)]
        public RecipientTypeType Type;

        public override void Parse(Stream s)
        {
            base.Parse(s);
            byte bitWise = ReadByte();
            this.Flag = (RecipientTypeFlag)(bitWise & 0xF0);
            this.Type = (RecipientTypeType)(bitWise & 0x0F);
        }
    }

    /// <summary>
    /// An enumeration that specifies the flag of RecipientType.
    /// </summary>
    public enum RecipientTypeFlag : byte
    {
        FailToReceiveTheMessageOnThePreviousAttempt = 0x01,
        SuccessfullyToReceiveTheMessageOnThePreviousAttempt = 0x08
    }

    /// <summary>
    /// An enumeration that specifies the type of RecipientType.
    /// </summary>
    public enum RecipientTypeType : byte
    {
        PrimaryRecipient = 0x01,
        CcRecipient = 0x02,
        BccRecipient = 0x03
    }

    /// <summary>
    /// The enum value of SaveFlags that contains flags that specify how the save operation behaves.
    /// </summary>
    public enum SaveFlags : byte
    {
        KeepOpenReadOnly = 0x01,
        KeepOpenReadWrite = 0x02,
        ForceSave = 0x04
    };

    /// <summary>
    /// The enum value of OpenEmbedMsgModeFlags that contains flags that control the access to the message.
    /// </summary>
    public enum OpenEmbedMsgModeFlags : byte
    {
        ReadOnly = 0x00,
        ReadWrite = 0x01,
        Create = 0x02
    };

    /// <summary>
    /// The enum value of GetAttachmentTableFlags that contains flags that control the type of table..
    /// </summary>
    public enum GetAttachmentTableFlags : byte
    {
        Standard = 0x00,
        Unicode = 0x40
    };

    /// <summary>
    /// The enum specifies the status of a message in a contents table. 
    /// </summary>
    [Flags]
    public enum MessageStatusFlag : uint
    {
        msRemoteDownload = 0x00001000,
        msInConflict = 0x00000800,
        msRemoteDelete = 0x00002000
    };

    /// <summary>
    /// The enum specifies the flags to set. 
    /// </summary>
    [Flags]
    public enum ReadFlags : byte
    {
        rfDefault = 0x00,
        rfSuppressReceipt = 0x01,
        rfReserved = 0x0A,
        rfClearReadFlag = 0x04,
        rfGenerateReceiptOnly = 0x10,
        rfClearNotifyRead = 0x20,
        rfClearNotifyUnread = 0x40
    };

    /// <summary>
    /// The enum specifies the flags for opening attachments.
    /// </summary>
    public enum OpenAttachmentFlags : byte
    {
        ReadOnly = 0x00,
        ReadWrite = 0x01,
        BestAccess = 0x03
    };
    #endregion
}
