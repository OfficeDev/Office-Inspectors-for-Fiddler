using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MAPIInspector.Parsers
{
    #region 2.2.2.1	Table ROP Constants
    /// <summary>
    /// 2.2.2.1.1	Predefined Bookmarks
    /// </summary>
    public enum Bookmarks : byte
    {
        BOOKMARK_BEGINNING = 0x00,
        BOOKMARK_CURRENT = 0x01,
        BOOKMARK_END = 0x02,
        BOOKMARK_CUSTOM = 0x03
    }

    /// <summary>
    ///2.2.2.1.3	TableStatus
    /// </summary>
    public enum TableStatus : byte
    {
        TBLSTAT_COMPLETE = 0x00,
        TBLSTAT_SORTING = 0x09,
        TBLSTAT_SORT_ERROR = 0x0A,
        TBLSTAT_SETTING_COLS = 0x0B,
        TBLSTAT_SETCOL_ERROR = 0x0D,
        TBLSTAT_RESTRICTING = 0x0E,
        TBLSTAT_RESTRICT_ERROR = 0x0F
    }

    /// <summary>
    /// 2.2.2.1.4	Asynchronous Flags
    /// </summary>
    public enum AsynchronousFlags : byte
    {
        TBL_SYNC = 0x00,
        TBL_ASYNC = 0x01
    }
    #endregion

    #region 2.2.2.2	RopSetColumns ROP
    /// <summary>
    /// The RopSetColumns ROP ([MS-OXCROPS] section 2.2.5.1) sets the properties that the client requests to be included in the table. 
    /// </summary>
    public class RopSetColumnsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure that contains flags that control this operation. 
        public AsynchronousFlags SetColumnsFlags;

        // An unsigned integer that specifies the number of tags present in the PropertyTags field.
        public ushort PropertyTagCount;

        // An array of PropertyTag structures that specifies the property values that are visible in table rows. 
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSetColumnsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetColumnsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.SetColumnsFlags = (AsynchronousFlags)ReadByte();
            this.PropertyTagCount = ReadUshort();
            List<PropertyTag> tempPropertyTags = new List<PropertyTag>();
            for (int i = 0; i < this.PropertyTagCount; i++)
            {
                PropertyTag tempPropertyTag = new PropertyTag();
                tempPropertyTag.Parse(s);
                tempPropertyTags.Add(tempPropertyTag);
            }
            this.PropertyTags = tempPropertyTags.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the RopSetColumns ROP Response Buffer.
    /// </summary>
    public class RopSetColumnsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies the status of the table. 
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopSetColumns structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetColumns structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.3	RopSortTable ROP
    /// <summary>
    /// The RopSortTable ROP ([MS-OXCROPS] section 2.2.5.2) orders the rows of a contents table based on sort criteria. 
    /// </summary>
    public class RopSortTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A flags structure that contains flags that control this operation. 
        public AsynchronousFlags SortTableFlags;

        // An unsigned integer that specifies how many SortOrder structures are present in the SortOrders field. 
        public ushort SortOrderCount;

        // An unsigned integer that specifies the number of category SortOrder structures in the SortOrders field. 
        public ushort CategoryCount;

        // An unsigned integer that specifies the number of expanded categories in the SortOrders field.
        public ushort ExpandedCount;

        // An array of SortOrder structures that specifies the sort order for the rows in the table. T
        public SortOrder[] SortOrders;

        /// <summary>
        /// Parse the RopSortTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSortTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.SortTableFlags = (AsynchronousFlags)ReadByte();
            this.SortOrderCount = ReadUshort();
            this.CategoryCount = ReadUshort();
            this.ExpandedCount = ReadUshort();
            List<SortOrder> tempSortOrders = new List<SortOrder>();
            for (int i = 0; i < this.SortOrderCount; i++)
            {
                SortOrder tempSortOrder = new SortOrder();
                tempSortOrder.Parse(s);
                tempSortOrders.Add(tempSortOrder);
            }
            this.SortOrders = tempSortOrders.ToArray();
        }
    }

    /// <summary>
    /// A class indicates the RopSortTable ROP Response Buffer.
    /// </summary>
    public class RopSortTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies the status of the table. 
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopSortTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSortTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.4	RopRestrict ROP
    /// <summary>
    /// The RopRestrict ROP ([MS-OXCROPS] section 2.2.5.3) establishes a restriction on a table. 
    /// </summary>
    public class RopRestrictRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        public byte InputHandleIndex;

        // A flags structure that contains flags that control this operation. 
        public AsynchronousFlags RestrictFlags;

        // An unsigned integer that specifies the length of the RestrictionData field.
        public ushort RestrictionDataSize;

        // A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this table The size of this field is specified by the RestrictionDataSize field.
        public RestrictionType RestrictionData;

        /// <summary>
        /// Parse the RopRestrictRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopRestrictRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.RestrictFlags = (AsynchronousFlags)ReadByte();
            this.RestrictionDataSize = ReadUshort();
            if (this.RestrictionDataSize > 0)
            {
                RestrictionType restriction = new RestrictionType();
                this.RestrictionData = restriction;
                this.RestrictionData.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the RopRestrict ROP Response Buffer.
    /// </summary>
    public class RopRestrictResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies the status of the table. 
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopRestrictResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopRestrictResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.5	RopQueryRows ROP
    /// <summary>
    /// The RopQueryRows ROP ([MS-OXCROPS] section 2.2.5.4) returns zero or more rows from a table, beginning from the current table cursor position.
    /// </summary>
    public class RopQueryRowsRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A flags structure that contains flags that control this operation. 
        public QueryRowsFlags QueryRowsFlags;

        // A Boolean that specifies the direction to read rows.
        public bool ForwardRead;

        // An unsigned integer that specifies the number of requested rows.
        public ushort RowCount;

        /// <summary>
        /// Parse the RopQueryRowsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryRowsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.QueryRowsFlags = (QueryRowsFlags)ReadByte();
            this.ForwardRead = ReadBoolean();
            this.RowCount = ReadUshort();
        }
    }

    /// <summary>
    ///  A class indicates the RopQueryRows ROP Response Buffer.
    /// </summary>
    public class RopQueryRowsResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies current location of the cursor. 
        public Bookmarks? Origin;

        // An unsigned integer that specifies the number of structures in the RowData field.
        public ushort? RowCount;

        // A list of PropertyRow structures. 
        public PropertyRow[] RowData;

        // Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// The construe function for RopQueryRowsResponse
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopQueryRowsResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopQueryRows structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryRows structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.Origin = (Bookmarks)ReadByte();
                this.RowCount = ReadUshort();
                if (this.RowCount != 0)
                {
                    List<PropertyRow> tempPropertyRows = new List<PropertyRow>();
                    for (int i = 0; i < RowCount; i++)
                    {
                        PropertyRow tempPropertyRow = new PropertyRow(this.propertiesBySetColum);
                        tempPropertyRow.Parse(s);
                        tempPropertyRows.Add(tempPropertyRow);
                    }
                    this.RowData = tempPropertyRows.ToArray();
                }
            }
        }
    }

    /// <summary>
    /// The enum value of QueryRowsFlags
    /// </summary>
    [Flags]
    public enum QueryRowsFlags : byte
    {
        Advance = 0x00,
        NoAdvance = 0x01,
        EnablePackedBuffers = 0x02
    }

    #endregion

    #region 2.2.2.6	RopAbort ROP
    /// <summary>
    /// The RopAbort ROP ([MS-OXCROPS] section 2.2.5.5) attempts to stop any asynchronous table operations that are currently in progress
    /// </summary>
    public class RopAbortRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopAbortRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopAbortRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopAbort ROP Response Buffer.
    /// </summary>
    public class RopAbortResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        //An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies the status of the table. 
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopAbortResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopAbortResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.7	RopGetStatus ROP
    /// <summary>
    /// The RopGetStatus ROP ([MS-OXCROPS] section 2.2.5.6) retrieves information about the current status of asynchronous operations being performed on the table.
    /// </summary>
    public class RopGetStatusRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopGetStatusRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetStatusRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetStatus ROP Response Buffer.
    /// </summary>
    public class RopGetStatusResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An enumeration that specifies the status of the table. 
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopGetStatusResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetStatusResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.8	RopQueryPosition ROP
    /// <summary>
    /// The RopQueryPosition ROP ([MS-OXCROPS] section 2.2.5.7) returns the location of the cursor in the table. 
    /// </summary>
    public class RopQueryPositionRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopQueryPositionRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryPositionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the  RopQueryPosition ROP Response Buffer.
    /// </summary>
    public class RopQueryPositionResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that indicates the index (0-based) of the current row. 
        public uint? Numerator;

        // An unsigned integer that indicates the total number of rows in the table. 
        public uint? Denominator;

        /// <summary>
        /// Parse the RopQueryPositionResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryPositionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.Numerator = ReadUint();
                this.Denominator = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.9	RopSeekRow ROP
    /// <summary>
    /// The RopSeekRow ROP ([MS-OXCROPS] section 2.2.5.8) moves the table cursor to a specific location in the table. 
    /// </summary>
    public class RopSeekRowRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An enumeration that specifies the origin of this seek operation. 
        public Bookmarks Origin;

        // A signed integer that specifies the direction and the number of rows to seek.
        public int RowCount;

        // A Boolean that specifies whether the server returns the actual number of rows moved in the response.
        public bool WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Origin = (Bookmarks)ReadByte();
            this.RowCount = ReadINT32();
            this.WantRowMovedCount = ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekRow ROP Response Buffer.
    /// </summary>
    public class RopSeekRowResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        public bool? HasSoughtLess;

        // A signed integer that specifies the direction and number of rows sought.
        public int? RowsSought;

        /// <summary>
        /// Parse the RopSeekRowResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.HasSoughtLess = ReadBoolean();
                this.RowsSought = ReadINT32();
            }
        }
    }
    #endregion

    #region 2.2.2.10 RopSeekRowBookmark ROP
    /// <summary>
    /// The RopSeekRowBookmark ROP ([MS-OXCROPS] section 2.2.5.9) moves the table cursor to a specific location in the table. 
    /// </summary>
    public class RopSeekRowBookmarkRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the Bookmark field.
        public ushort BookmarkSize;

        // An array of bytes that specifies the origin for the seek operation. 
        public byte[] Bookmark;

        // A signed integer that specifies the direction and the number of rows to seek.
        public int RowCount;

        // A Boolean that specifies whether the server returns the actual number of rows sought in the response.
        public bool WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowBookmarkRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekRowBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.BookmarkSize = ReadUshort();
            this.Bookmark = ReadBytes(BookmarkSize);
            this.RowCount = ReadINT32();
            this.WantRowMovedCount = ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekRowBookmark ROP Response Buffer.
    /// </summary>
    public class RopSeekRowBookmarkResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the bookmark target is no longer visible.
        public bool? RowNoLongerVisible;

        // A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        public bool? HasSoughtLess;

        // An unsigned integer that specifies the direction and number of rows sought.
        public uint? RowsSought;

        /// <summary>
        /// Parse the RopSeekRowBookmarkResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekRowBookmarkResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowNoLongerVisible = ReadBoolean();
                this.HasSoughtLess = ReadBoolean();
                this.RowsSought = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.11 RopSeekRowFractional ROP
    /// <summary>
    /// The RopSeekRowFractional ROP ([MS-OXCROPS] section 2.2.5.10) moves the table cursor to an approximate position in the table.
    /// </summary>
    public class RopSeekRowFractionalRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that represents the numerator of the fraction identifying the table position to seek to.
        public uint Numerator;

        // An unsigned integer that represents the denominator of the fraction identifying the table position to seek to.
        public uint Denominator;

        /// <summary>
        /// Parse the RopSeekRowFractionalRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekRowFractionalRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.Numerator = ReadUint();
            this.Denominator = ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekRowFractional ROP Response Buffer.
    /// </summary>
    public class RopSeekRowFractionalResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;
        //An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopSeekRowFractionalResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSeekRowFractionalResponse structure.</param>
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

    #region 2.2.2.12 RopCreateBookmark ROP
    /// <summary>
    /// The RopCreateBookmark ROP ([MS-OXCROPS] section 2.2.5.11) creates a new bookmark at the current cursor position in the table. 
    /// </summary>
    public class RopCreateBookmarkRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopCreateBookmarkRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopCreateBookmark ROP Response Buffer.
    /// </summary>
    public class RopCreateBookmarkResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the size of the Bookmark field.
        public ushort? BookmarkSize;

        // An array of bytes that specifies the bookmark created. The size of this field, in bytes, is specified by the BookmarkSize field.
        public byte?[] Bookmark;

        /// <summary>
        /// Parse the RopCreateBookmarkResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCreateBookmarkResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.BookmarkSize = ReadUshort();
                this.Bookmark = ConvertArray(ReadBytes((int)BookmarkSize));
            }
        }
    }
    #endregion

    #region 2.2.2.13 RopQueryColumnsAll ROP
    /// <summary>
    /// The RopQueryColumnsAll ROP ([MS-OXCROPS] section 2.2.5.12) returns a complete list of all columns for the table. 
    /// </summary>
    public class RopQueryColumnsAllRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopQueryColumnsAllRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryColumnsAllRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopQueryColumnsAll ROP Response Buffer.
    /// </summary>
    public class RopQueryColumnsAllResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies how many tags are present in the PropertyTags field.
        public ushort? PropertyTagCount;

        // An array of PropertyTag structures that specifies the columns of the table. 
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopQueryColumnsAllResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopQueryColumnsAllResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.PropertyTagCount = ReadUshort();
                List<PropertyTag> tempPropertyTags = new List<PropertyTag>();
                for (int i = 0; i < this.PropertyTagCount; i++)
                {
                    PropertyTag tempPropertyTag = new PropertyTag();
                    tempPropertyTag.Parse(s);
                    tempPropertyTags.Add(tempPropertyTag);
                }
                this.PropertyTags = tempPropertyTags.ToArray();
            }
        }
    }
    #endregion

    #region 2.2.2.14 RopFindRow ROP
    /// <summary>
    /// The RopFindRow ROP ([MS-OXCROPS] section 2.2.5.13) returns the next row in a table that matches the search criteria and moves the cursor to that row. 
    /// </summary>
    public class RopFindRowRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // A flags structure that contains flags that control this operation. 
        public FindRowFlags FindRowFlags;

        // An unsigned integer that specifies the length of the RestrictionData field.
        public ushort RestrictionDataSize;

        // A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this operation. 
        public RestrictionType RestrictionData;

        // An enumeration that specifies where this operation begins its search. 
        public Bookmarks Origin;

        // An unsigned integer that specifies the size of the Bookmark field.
        public ushort BookmarkSize;

        // An array of bytes that specifies the bookmark to use as the origin. The 
        public byte[] Bookmark;

        /// <summary>
        /// Parse the RopFindRow structure.
        /// </summary>
        /// <param name="s">An stream containing RopFindRow structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.FindRowFlags = (FindRowFlags)ReadByte();
            this.RestrictionDataSize = ReadUshort();
            if (RestrictionDataSize > 0)
            {
                RestrictionType tempRestriction = new RestrictionType();
                this.RestrictionData = tempRestriction;
                this.RestrictionData.Parse(s);
            }
            this.Origin = (Bookmarks)ReadByte();
            this.BookmarkSize = ReadUshort();
            this.Bookmark = ReadBytes(BookmarkSize);
        }
    }

    /// <summary>
    /// A class indicates the RopFindRow ROP Response Buffer.
    /// </summary>
    public class RopFindRowResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // A Boolean that specifies whether the bookmark target is no longer visible.
        public bool? RowNoLongerVisible;

        // A Boolean that indicates whether the RowData field is present.
        public bool? HasRowData;

        // A Boolean that indicates whether the RowData field is present.
        public PropertyRow RowData;

        // Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// The construe function for RopFindRowsResponse
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopFindRowResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopFindRowResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFindRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.RowNoLongerVisible = ReadBoolean();
                this.HasRowData = ReadBoolean();
                if ((bool)HasRowData)
                {
                    PropertyRow tempPropertyRow = new PropertyRow(propertiesBySetColum);
                    this.RowData = tempPropertyRow;
                    this.RowData.Parse(s);
                }
            }
        }
    }

    /// <summary>
    /// The enum structure that contains an OR'ed combination. 
    /// </summary>
    public enum FindRowFlags : byte
    {
        Forwards = 0x00,
        Backwards = 0x01
    }
    #endregion

    #region 2.2.2.15 RopFreeBookmark ROP
    /// <summary>
    /// The RopFreeBookmark ROP ([MS-OXCROPS] section 2.2.5.14) frees the memory associated with a bookmark that was returned by a previous RopCreateBookmark ROP request ([MS-OXCROPS] section 2.2.5.11). 
    /// </summary>
    public class RopFreeBookmarkRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the Bookmark field.
        public ushort BookmarkSize;

        // An array of bytes that specifies the origin for the seek operation. 
        public byte[] Bookmark;

        /// <summary>
        /// Parse the RopFreeBookmarkRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopFreeBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.BookmarkSize = ReadUshort();
            this.Bookmark = ReadBytes(BookmarkSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopFreeBookmark ROP Response Buffer.
    /// </summary>
    public class RopFreeBookmarkResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopFreeBookmarkResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopFreeBookmarkResponse structure.</param>
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

    #region 2.2.2.16 RopResetTable ROP
    /// <summary>
    /// The RopResetTable ROP ([MS-OXCROPS] section 2.2.5.15).
    /// </summary>
    public class RopResetTableRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        /// <summary>
        /// Parse the RopResetTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopResetTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopResetTable ROP Response Buffer.
    /// </summary>
    public class RopResetTableResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        /// <summary>
        /// Parse the RopResetTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopResetTableResponse structure.</param>
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

    #region 2.2.2.17 RopExpandRow ROP
    /// <summary>
    /// The RopExpandRow ROP ([MS-OXCROPS] section 2.2.5.16) expands a collapsed category of a table and returns the rows that belong in the newly expanded category. 
    /// </summary>
    public class RopExpandRowRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the maximum number of expanded rows to return data for.
        public ushort MaxRowCount;

        // An identifier that specifies the category to be expanded.
        public long CategoryId;

        /// <summary>
        /// Parse the RopExpandRowRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopExpandRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.MaxRowCount = ReadUshort();
            this.CategoryId = ReadINT64();
        }
    }

    /// <summary>
    ///  A class indicates the RopExpandRow ROP Response Buffer.
    /// </summary>
    public class RopExpandRowResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the total number of rows that are in the expanded category.
        public uint? ExpandedRowCount;

        // An unsigned integer that specifies the number of PropertyRow structures.
        public ushort? RowCount;

        // A list of PropertyRow structures. The number of structures contained in this field is specified by the RowCount field.
        public PropertyRow[] RowData;

        // Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// The construe function for RopExpandRowsResponse
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopExpandRowResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopExpandRowResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopExpandRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.ExpandedRowCount = ReadUint();
                this.RowCount = ReadUshort();
                List<PropertyRow> tempPropertyRows = new List<PropertyRow>();
                for (int i = 0; i < RowCount; i++)
                {
                    PropertyRow tempPropertyRow = new PropertyRow(this.propertiesBySetColum);
                    tempPropertyRow.Parse(s);
                    tempPropertyRows.Add(tempPropertyRow);
                }
                this.RowData = tempPropertyRows.ToArray();
            }
        }
    }
    #endregion

    #region 2.2.2.18 RopCollapseRow ROP
    /// <summary>
    /// The RopCollapseRow ROP ([MS-OXCROPS] section 2.2.5.17) collapses an expanded category. 
    /// </summary>
    public class RopCollapseRowRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An identifier that specifies the category to be collapsed.
        public long CategoryId;

        /// <summary>
        /// Parse the RopCollapseRowRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopCollapseRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.CategoryId = ReadINT64();
        }
    }

    /// <summary>
    ///  A class indicates the RopCollapseRow ROP Response Buffer.
    /// </summary>
    public class RopCollapseRowResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the total number of rows in the collapsed category.
        public uint? CollapsedRowCount;

        /// <summary>
        /// Parse the RopCollapseRowResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopCollapseRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.CollapsedRowCount = ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.19 RopGetCollapseState ROP
    /// <summary>
    /// The RopGetCollapseState ROP ([MS-OXCROPS] section 2.2.5.18) returns the data necessary to rebuild the current expanded/collapsed state of the table. 
    /// </summary>
    public class RopGetCollapseStateRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An identifier that specifies the row to be preserved as the cursor. 
        public long RowId;

        // An unsigned integer that specifies the instance number of the row that is to be preserved as the cursor.
        public uint RowInstanceNumber;

        /// <summary>
        /// Parse the RopGetCollapseStateRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetCollapseStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.RowId = ReadINT64();
            this.RowInstanceNumber = ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopGetCollapseStateResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the size of the CollapseState field.
        public ushort? CollapseStateSize;

        // An array of bytes that specifies a collapse state for a categorized table. 
        public byte?[] CollapseState;

        /// <summary>
        /// Parse the RopGetCollapseStateResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopGetCollapseStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.CollapseStateSize = ReadUshort();
                this.CollapseState = ConvertArray(ReadBytes((int)CollapseStateSize));
            }
        }
    }
    #endregion

    #region 2.2.2.20 RopSetCollapseState ROP
    /// <summary>
    /// The following descriptions define valid fields for the RopSetCollapseState ROP request buffer ([MS-OXCROPS] section 2.2.5.19.1).
    /// </summary>
    public class RopSetCollapseStateRequest : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer that specifies the ID that the client requests to have associated with the created logon.
        public byte LogonId;

        // An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the size of the CollapseState field.
        public ushort CollapseStateSize;

        //  An array of bytes that specifies a collapse state for a categorized table. The size of this field, in bytes, is specified by the CollapseStateSize field.
        public byte[] CollapseState;

        /// <summary>
        /// Parse the RopSetCollapseStateRequest structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetCollapseStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.LogonId = ReadByte();
            this.InputHandleIndex = ReadByte();
            this.CollapseStateSize = ReadUshort();
            this.CollapseState = ReadBytes(CollapseStateSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopSetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopSetCollapseStateResponse : BaseStructure
    {
        // An unsigned integer that specifies the type of ROP.
        public RopIdType RopId;

        // An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        public byte InputHandleIndex;

        // An unsigned integer that specifies the status of the ROP.
        public object ReturnValue;

        // An unsigned integer that specifies the size of the Bookmark field.
        public ushort? BookmarkSize;

        // An array of bytes that specifies the origin for the seek operation. 
        public byte?[] Bookmark;

        /// <summary>
        /// Parse the RopSetCollapseStateResponse structure.
        /// </summary>
        /// <param name="s">An stream containing RopSetCollapseStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)ReadByte();
            this.InputHandleIndex = ReadByte();
            HelpMethod help = new HelpMethod();
            this.ReturnValue = help.FormatErrorCode(ReadUint());

            if ((ErrorCodes)ReturnValue == ErrorCodes.Success)
            {
                this.BookmarkSize = ReadUshort();
                this.Bookmark = ConvertArray(ReadBytes((int)BookmarkSize));
            }
        }
    }
    #endregion
}
