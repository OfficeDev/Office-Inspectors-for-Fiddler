namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System;
    using System.Collections.Generic;
    using System.IO;

    #region 2.2.2.1	Table ROP Constants
    /// <summary>
    /// 2.2.2.1.1 Predefined Bookmarks
    /// </summary>
    public enum Bookmarks : byte
    {
        /// <summary>
        /// Points to the beginning position of the table, or the first row.
        /// </summary>
        BOOKMARK_BEGINNING = 0x00,

        /// <summary>
        /// Points to the current position of the table, or the current row.
        /// </summary>
        BOOKMARK_CURRENT = 0x01,

        /// <summary>
        /// Points to the ending position of the table, or the location after the last row
        /// </summary>
        BOOKMARK_END = 0x02,

        /// <summary>
        /// Points to the custom position in the table. Used with the BookmarkSize and Bookmark fields.
        /// </summary>
        BOOKMARK_CUSTOM = 0x03
    }

    /// <summary>
    /// The enum value of QueryRowsFlags
    /// </summary>
    [Flags]
    public enum QueryRowsFlags : byte
    {
        /// <summary>
        /// Advance the table cursor.
        /// </summary>
        Advance = 0x00,

        /// <summary>
        /// Do not advance the table cursor.
        /// </summary>
        NoAdvance = 0x01,

        /// <summary>
        /// Enable packed buffers for the response. 
        /// </summary>
        EnablePackedBuffers = 0x02
    }

    /// <summary>
    /// 2.2.2.1.3 TableStatus
    /// </summary>
    public enum TableStatus : byte
    {
        /// <summary>
        /// No operations are in progress.
        /// </summary>
        TBLSTAT_COMPLETE = 0x00,

        /// <summary>
        /// A RopSortTable ROP is in progress.
        /// </summary>
        TBLSTAT_SORTING = 0x09,

        /// <summary>
        /// An error occurred during a RopSortTable ROP
        /// </summary>
        TBLSTAT_SORT_ERROR = 0x0A,

        /// <summary>
        /// A RopSetColumns ROP is in progress.
        /// </summary>
        TBLSTAT_SETTING_COLS = 0x0B,

        /// <summary>
        /// An error occurred during a RopSetColumns ROP
        /// </summary>
        TBLSTAT_SETCOL_ERROR = 0x0D,

        /// <summary>
        /// A RopRestrict ROP is in progress.
        /// </summary>
        TBLSTAT_RESTRICTING = 0x0E,

        /// <summary>
        /// An error occurred during a RopRestrict ROP.
        /// </summary>
        TBLSTAT_RESTRICT_ERROR = 0x0F
    }

    /// <summary>
    /// 2.2.2.1.4 Asynchronous Flags
    /// </summary>
    public enum AsynchronousFlags : byte
    {
        /// <summary>
        /// The server will perform the ROP asynchronously.
        /// </summary>
        TBL_SYNC = 0x00,

        /// <summary>
        /// The server will perform the operation synchronously
        /// </summary>
        TBL_ASYNC = 0x01
    }

    /// <summary>
    /// The enum structure that contains an OR'ed combination. 
    /// </summary>
    public enum FindRowFlags : byte
    {
        /// <summary>
        /// Perform the find forwards.
        /// </summary>
        Forwards = 0x00,

        /// <summary>
        /// Perform the find backwards
        /// </summary>
        Backwards = 0x01
    }

    #endregion

    #region 2.2.2.2	RopSetColumns ROP
    /// <summary>
    /// The RopSetColumns ROP ([MS-OXCROPS] section 2.2.5.1) sets the properties that the client requests to be included in the table. 
    /// </summary>
    public class RopSetColumnsRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control this operation. 
        /// </summary>
        public BlockT<AsynchronousFlags> SetColumnsFlags;

        /// <summary>
        /// An unsigned integer that specifies the number of tags present in the PropertyTags field.
        /// </summary>
        public BlockT<ushort> PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the property values that are visible in table rows. 
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopSetColumnsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetColumnsRequest structure.</param>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>(parser);
            LogonId = ParseT<byte>(parser);
            InputHandleIndex = ParseT<byte>(parser);
            SetColumnsFlags = ParseT<AsynchronousFlags>(parser);
            PropertyTagCount = ParseT<ushort>(parser);

            List<PropertyTag> tempPropertyTags = new List<PropertyTag>();
            for (int i = 0; i < this.PropertyTagCount.Data; i++)
            {
                PropertyTag tempPropertyTag = new PropertyTag();
                tempPropertyTag.Parse(parser);
                tempPropertyTags.Add(tempPropertyTag);
            }

            this.PropertyTags = tempPropertyTags.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetColumnsRequest");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(LogonId, "LogonId:0x{0:X2}", LogonId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            AddChild(SetColumnsFlags, "SetColumnsFlags:{0}", SetColumnsFlags.Data);
            AddChild(PropertyTagCount, "PropertyTagCount:{0}", PropertyTagCount.Data);
            AddLabeledChildren(PropertyTags, "PropertyTags");
        }
    }

    /// <summary>
    /// A class indicates the RopSetColumns ROP Response Buffer.
    /// </summary>
    public class RopSetColumnsResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request.
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<uint> ReturnValue;

        /// <summary>
        /// An enumeration that specifies the status of the table.
        /// </summary>
        public BlockT<TableStatus> TableStatus;

        protected override void Parse()
        {
            RopId = ParseT<RopIdType>(parser);
            InputHandleIndex = ParseT<byte>(parser);
            ReturnValue = ParseT<uint>(parser);

            if (ReturnValue.Data == (uint)ErrorCodes.Success)
            {
                TableStatus = ParseT<TableStatus>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSetColumnsResponse");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            var returnValue = HelpMethod.FormatErrorCode(ReturnValue.Data);
            AddChild(ReturnValue, "ReturnValue:{0}", returnValue);
            if (TableStatus.Parsed)
            {
                AddChild(TableStatus, "TableStatus:{0}", TableStatus.Data);
            }
        }
    }
    #endregion

    #region 2.2.2.3 RopSortTable ROP
    /// <summary>
    /// The RopSortTable ROP ([MS-OXCROPS] section 2.2.5.2) orders the rows of a contents table based on sort criteria. 
    /// </summary>
    public class RopSortTableRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control this operation.
        /// </summary>
        public BlockT<AsynchronousFlags> SortTableFlags;

        /// <summary>
        /// An unsigned integer that specifies how many SortOrder structures are present in the SortOrders field.
        /// </summary>
        public BlockT<ushort> SortOrderCount;

        /// <summary>
        /// An unsigned integer that specifies the number of category SortOrder structures in the SortOrders field.
        /// </summary>
        public BlockT<ushort> CategoryCount;

        /// <summary>
        /// An unsigned integer that specifies the number of expanded categories in the SortOrders field.
        /// </summary>
        public BlockT<ushort> ExpandedCount;

        /// <summary>
        /// An array of SortOrder structures that specifies the sort order for the rows in the table. 
        /// </summary>
        public SortOrder[] SortOrders;

        /// <summary>
        /// Parse the RopSortTableRequest structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>(parser);
            LogonId = ParseT<byte>(parser);
            InputHandleIndex = ParseT<byte>(parser);
            SortTableFlags = ParseT<AsynchronousFlags>(parser);
            SortOrderCount = ParseT<ushort>(parser);
            CategoryCount = ParseT<ushort>(parser);
            ExpandedCount = ParseT<ushort>(parser);
            var tempSortOrders = new List<SortOrder>();
            for (int i = 0; i < SortOrderCount.Data; i++)
            {
                tempSortOrders.Add(Parse<SortOrder>(parser));
            }

            SortOrders = tempSortOrders.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RopSortTableRequest");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(LogonId, "LogonId:{0}", LogonId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            AddChild(SortTableFlags, "SortTableFlags:{0}", SortTableFlags.Data);
            AddChild(SortOrderCount, "SortOrderCount:{0}", SortOrderCount.Data);
            AddChild(SortOrderCount, "CategoryCount:{0}", CategoryCount.Data);
            AddChild(ExpandedCount, "ExpandedCount :{0}", ExpandedCount.Data);
            AddLabeledChildren(SortOrders, "SortOrders");
        }
    }

    /// <summary>
    /// A class indicates the RopSortTable ROP Response Buffer.
    /// </summary>
    public class RopSortTableResponse : BaseStructure
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
        /// An enumeration that specifies the status of the table. 
        /// </summary>
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopSortTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSortTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)this.ReadByte();
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
        /// A flags structure that contains flags that control this operation. 
        /// </summary>
        public AsynchronousFlags RestrictFlags;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this table The size of this field is specified by the RestrictionDataSize field.
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// Parse the RopRestrictRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopRestrictRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.RestrictFlags = (AsynchronousFlags)this.ReadByte();
            this.RestrictionDataSize = this.ReadUshort();
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
        /// An enumeration that specifies the status of the table. 
        /// </summary>
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopRestrictResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopRestrictResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)this.ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.5	RopQueryRows ROP
    /// <summary>
    /// The RopQueryRows ROP ([MS-OXCROPS] section 2.2.5.4) returns zero or more rows from a table, beginning from the current table cursor position.
    /// </summary>
    public class RopQueryRowsRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// A flags structure that contains flags that control this operation.
        /// </summary>
        public BlockT<QueryRowsFlags> QueryRowsFlags;

        /// <summary>
        /// A Boolean that specifies the direction to read rows.
        /// </summary>
        public BlockT<bool> ForwardRead;

        /// <summary>
        /// An unsigned integer that specifies the number of requested rows.
        /// </summary>
        public BlockT<ushort> RowCount;

        /// <summary>
        /// Parse the RopQueryRowsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryRowsRequest structure.</param>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>(parser);
            LogonId = ParseT<byte>(parser);
            InputHandleIndex = ParseT<byte>(parser);
            QueryRowsFlags = ParseT<QueryRowsFlags>(parser);
            ForwardRead = ParseAs<byte, bool>(parser);
            RowCount = ParseT<ushort>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("RopSeekRowRequest");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(LogonId, "LogonId:0x{0:X2}", LogonId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            AddChild(QueryRowsFlags, "QueryRowsFlags:{0}", QueryRowsFlags.Data);
            AddChild(ForwardRead, "ForwardRead:{0}", ForwardRead.Data);
            AddChild(RowCount, "RowCount:{0}", RowCount.Data);
        }
    }

    /// <summary>
    ///  A class indicates the RopQueryRows ROP Response Buffer.
    /// </summary>
    public class RopQueryRowsResponse : BaseStructure
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
        /// An enumeration that specifies current location of the cursor. 
        /// </summary>
        public Bookmarks? Origin;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RowData field.
        /// </summary>
        public ushort? RowCount;

        /// <summary>
        /// A list of PropertyRow structures. 
        /// </summary>
        public PropertyRow[] RowData;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the RopQueryRowsResponse class.
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopQueryRowsResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopQueryRows structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryRows structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.Origin = (Bookmarks)this.ReadByte();
                this.RowCount = this.ReadUshort();
                if (this.RowCount != 0)
                {
                    List<PropertyRow> tempPropertyRows = new List<PropertyRow>();
                    for (int i = 0; i < this.RowCount; i++)
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

    #endregion

    #region 2.2.2.6 RopAbort ROP
    /// <summary>
    /// The RopAbort ROP ([MS-OXCROPS] section 2.2.5.5) attempts to stop any asynchronous table operations that are currently in progress
    /// </summary>
    public class RopAbortRequest : BaseStructure
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
        /// Parse the RopAbortRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopAbort ROP Response Buffer.
    /// </summary>
    public class RopAbortResponse : BaseStructure
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
        /// An enumeration that specifies the status of the table. 
        /// </summary>
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopAbortResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopAbortResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)this.ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.7 RopGetStatus ROP
    /// <summary>
    /// The RopGetStatus ROP ([MS-OXCROPS] section 2.2.5.6) retrieves information about the current status of asynchronous operations being performed on the table.
    /// </summary>
    public class RopGetStatusRequest : BaseStructure
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
        /// Parse the RopGetStatusRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStatusRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetStatus ROP Response Buffer.
    /// </summary>
    public class RopGetStatusResponse : BaseStructure
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
        /// An enumeration that specifies the status of the table. 
        /// </summary>
        public TableStatus? TableStatus;

        /// <summary>
        /// Parse the RopGetStatusResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetStatusResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.TableStatus = (TableStatus)this.ReadByte();
            }
        }
    }
    #endregion

    #region 2.2.2.8 RopQueryPosition ROP
    /// <summary>
    /// The RopQueryPosition ROP ([MS-OXCROPS] section 2.2.5.7) returns the location of the cursor in the table. 
    /// </summary>
    public class RopQueryPositionRequest : BaseStructure
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
        /// Parse the RopQueryPositionRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryPositionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the  RopQueryPosition ROP Response Buffer.
    /// </summary>
    public class RopQueryPositionResponse : BaseStructure
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
        /// An unsigned integer that indicates the index (0-based) of the current row. 
        /// </summary>
        public uint? Numerator;

        /// <summary>
        /// An unsigned integer that indicates the total number of rows in the table. 
        /// </summary>
        public uint? Denominator;

        /// <summary>
        /// Parse the RopQueryPositionResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryPositionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.Numerator = this.ReadUint();
                this.Denominator = this.ReadUint();
            }
        }
    }
    #endregion

    #region 2.2.2.9	RopSeekRow ROP
    /// <summary>
    /// The RopSeekRow ROP ([MS-OXCROPS] section 2.2.5.8) moves the table cursor to a specific location in the table. 
    /// </summary>
    public class RopSeekRowRequest : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer that specifies the ID that the client requests to have associated with the created RopLogon.
        /// </summary>
        public BlockT<byte> LogonId;

        /// <summary>
        /// An unsigned integer index that specifies the location in the Server object handle table where the handle for the input Server object is stored. 
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An enumeration that specifies the origin of this seek operation. 
        /// </summary>
        public BlockT<Bookmarks> Origin;

        /// <summary>
        /// A signed integer that specifies the direction and the number of rows to seek.
        /// </summary>
        public BlockT<int> RowCount;

        /// <summary>
        /// A Boolean that specifies whether the server returns the actual number of rows moved in the response.
        /// </summary>
        public BlockT<bool> WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowRequest structure.</param>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>(parser);
            LogonId = ParseT<byte>(parser);
            InputHandleIndex = ParseT<byte>(parser);
            Origin = ParseT<Bookmarks>(parser);
            RowCount = ParseT<int>(parser);
            WantRowMovedCount = ParseAs<byte, bool>(parser);
        }

        protected override void ParseBlocks()
        {
            SetText("RopSeekRowRequest");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(LogonId, "LogonId:0x{0:X2}", LogonId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            AddChild(Origin, "Origin:{0}", Origin.Data);
            AddChild(RowCount, "RowCount:{0}", RowCount.Data);
            AddChild(WantRowMovedCount, "WantRowMovedCount:{0}", WantRowMovedCount.Data);
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekRow ROP Response Buffer.
    /// </summary>
    public class RopSeekRowResponse : Block
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public BlockT<RopIdType> RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. 
        /// </summary>
        public BlockT<byte> InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public BlockT<uint> ReturnValue;

        /// <summary>
        /// A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        /// </summary>
        public BlockT<bool> HasSoughtLess;

        /// <summary>
        /// A signed integer that specifies the direction and number of rows sought.
        /// </summary>
        public BlockT<int> RowsSought;

        /// <summary>
        /// Parse the RopSeekRowResponse structure.
        /// </summary>
        protected override void Parse()
        {
            RopId = ParseT<RopIdType>(parser);
            InputHandleIndex = ParseT<byte>(parser);
            ReturnValue = ParseT<uint>(parser);
            if (ReturnValue.Data == (uint)ErrorCodes.Success)
            {
                HasSoughtLess = ParseAs<byte, bool>(parser);
                RowsSought = ParseT<int>(parser);
            }
        }

        protected override void ParseBlocks()
        {
            SetText("RopSeekRowResponse");
            AddChild(RopId, "RopId:{0}", RopId.Data);
            AddChild(InputHandleIndex, "InputHandleIndex:{0}", InputHandleIndex.Data);
            var returnValue = HelpMethod.FormatErrorCode(ReturnValue.Data);
            AddChild(ReturnValue, "ReturnValue:{0}", returnValue);
            if (HasSoughtLess != null) AddChild(HasSoughtLess, "HasSoughtLess:{0}", HasSoughtLess.Data);
            if (RowsSought != null) AddChild(RowsSought, "RowsSought:{0}", RowsSought.Data);
        }
    }
    #endregion

    #region 2.2.2.10 RopSeekRowBookmark ROP
    /// <summary>
    /// The RopSeekRowBookmark ROP ([MS-OXCROPS] section 2.2.5.9) moves the table cursor to a specific location in the table. 
    /// </summary>
    public class RopSeekRowBookmarkRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the origin for the seek operation. 
        /// </summary>
        public byte[] Bookmark;

        /// <summary>
        /// A signed integer that specifies the direction and the number of rows to seek.
        /// </summary>
        public int RowCount;

        /// <summary>
        /// A Boolean that specifies whether the server returns the actual number of rows sought in the response.
        /// </summary>
        public bool WantRowMovedCount;

        /// <summary>
        /// Parse the RopSeekRowBookmarkRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.BookmarkSize = this.ReadUshort();
            this.Bookmark = this.ReadBytes(this.BookmarkSize);
            this.RowCount = this.ReadINT32();
            this.WantRowMovedCount = this.ReadBoolean();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekRowBookmark ROP Response Buffer.
    /// </summary>
    public class RopSeekRowBookmarkResponse : BaseStructure
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
        /// A Boolean that specifies whether the bookmark target is no longer visible.
        /// </summary>
        public bool? RowNoLongerVisible;

        /// <summary>
        /// A Boolean that specifies whether the full number of rows sought past was less than the number that was requested.
        /// </summary>
        public bool? HasSoughtLess;

        /// <summary>
        /// An unsigned integer that specifies the direction and number of rows sought.
        /// </summary>
        public uint? RowsSought;

        /// <summary>
        /// Parse the RopSeekRowBookmarkResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowBookmarkResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowNoLongerVisible = this.ReadBoolean();
                this.HasSoughtLess = this.ReadBoolean();
                this.RowsSought = this.ReadUint();
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
        /// An unsigned integer that represents the numerator of the fraction identifying the table position to seek to.
        /// </summary>
        public uint Numerator;

        /// <summary>
        /// An unsigned integer that represents the denominator of the fraction identifying the table position to seek to.
        /// </summary>
        public uint Denominator;

        /// <summary>
        /// Parse the RopSeekRowFractionalRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowFractionalRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.Numerator = this.ReadUint();
            this.Denominator = this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopSeekRowFractional ROP Response Buffer.
    /// </summary>
    public class RopSeekRowFractionalResponse : BaseStructure
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
        /// Parse the RopSeekRowFractionalResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSeekRowFractionalResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.12 RopCreateBookmark ROP
    /// <summary>
    /// The RopCreateBookmark ROP ([MS-OXCROPS] section 2.2.5.11) creates a new bookmark at the current cursor position in the table. 
    /// </summary>
    public class RopCreateBookmarkRequest : BaseStructure
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
        /// Parse the RopCreateBookmarkRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopCreateBookmark ROP Response Buffer.
    /// </summary>
    public class RopCreateBookmarkResponse : BaseStructure
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort? BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the bookmark created. The size of this field, in bytes, is specified by the BookmarkSize field.
        /// </summary>
        public byte?[] Bookmark;

        /// <summary>
        /// Parse the RopCreateBookmarkResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCreateBookmarkResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.BookmarkSize = this.ReadUshort();
                this.Bookmark = this.ConvertArray(this.ReadBytes((int)this.BookmarkSize));
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
        /// Parse the RopQueryColumnsAllRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryColumnsAllRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopQueryColumnsAll ROP Response Buffer.
    /// </summary>
    public class RopQueryColumnsAllResponse : BaseStructure
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
        /// An unsigned integer that specifies how many tags are present in the PropertyTags field.
        /// </summary>
        public ushort? PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures that specifies the columns of the table. 
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the RopQueryColumnsAllResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopQueryColumnsAllResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.PropertyTagCount = this.ReadUshort();
                List<PropertyTag> tempPropertyTags = new List<PropertyTag>();
                for (int i = 0; i < this.PropertyTagCount; i++)
                {
                    PropertyTag tempPropertyTag = Block.Parse<PropertyTag>(s);
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
        /// A flags structure that contains flags that control this operation. 
        /// </summary>
        public FindRowFlags FindRowFlags;

        /// <summary>
        /// An unsigned integer that specifies the length of the RestrictionData field.
        /// </summary>
        public ushort RestrictionDataSize;

        /// <summary>
        /// A restriction packet, as specified in [MS-OXCDATA] section 2.12, that specifies the filter for this operation. 
        /// </summary>
        public RestrictionType RestrictionData;

        /// <summary>
        /// An enumeration that specifies where this operation begins its search. 
        /// </summary>
        public Bookmarks Origin;

        /// <summary>
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the bookmark to use as the origin.
        /// </summary>
        public byte[] Bookmark;

        /// <summary>
        /// Parse the RopFindRow structure.
        /// </summary>
        /// <param name="s">A stream containing RopFindRow structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.FindRowFlags = (FindRowFlags)this.ReadByte();
            this.RestrictionDataSize = this.ReadUshort();
            if (this.RestrictionDataSize > 0)
            {
                RestrictionType tempRestriction = new RestrictionType();
                this.RestrictionData = tempRestriction;
                this.RestrictionData.Parse(s);
            }

            this.Origin = (Bookmarks)this.ReadByte();
            this.BookmarkSize = this.ReadUshort();
            this.Bookmark = this.ReadBytes(this.BookmarkSize);
        }
    }

    /// <summary>
    /// A class indicates the RopFindRow ROP Response Buffer.
    /// </summary>
    public class RopFindRowResponse : BaseStructure
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
        /// A Boolean that specifies whether the bookmark target is no longer visible.
        /// </summary>
        public bool? RowNoLongerVisible;

        /// <summary>
        /// A Boolean that indicates whether the RowData field is present.
        /// </summary>
        public bool? HasRowData;

        /// <summary>
        /// A Boolean that indicates whether the RowData field is present.
        /// </summary>
        public PropertyRow RowData;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the RopFindRowResponse class.
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopFindRowResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopFindRowResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFindRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.RowNoLongerVisible = this.ReadBoolean();
                this.HasRowData = this.ReadBoolean();
                if ((bool)this.HasRowData)
                {
                    PropertyRow tempPropertyRow = new PropertyRow(this.propertiesBySetColum);
                    this.RowData = tempPropertyRow;
                    this.RowData.Parse(s);
                }
            }
        }
    }

    #endregion

    #region 2.2.2.15 RopFreeBookmark ROP
    /// <summary>
    /// The RopFreeBookmark ROP ([MS-OXCROPS] section 2.2.5.14) frees the memory associated with a bookmark that was returned by a previous RopCreateBookmark ROP request ([MS-OXCROPS] section 2.2.5.11). 
    /// </summary>
    public class RopFreeBookmarkRequest : BaseStructure
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the origin for the seek operation. 
        /// </summary>
        public byte[] Bookmark;

        /// <summary>
        /// Parse the RopFreeBookmarkRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopFreeBookmarkRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.BookmarkSize = this.ReadUshort();
            this.Bookmark = this.ReadBytes(this.BookmarkSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopFreeBookmark ROP Response Buffer.
    /// </summary>
    public class RopFreeBookmarkResponse : BaseStructure
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
        /// Parse the RopFreeBookmarkResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopFreeBookmarkResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.16 RopResetTable ROP
    /// <summary>
    /// The RopResetTable ROP ([MS-OXCROPS] section 2.2.5.15).
    /// </summary>
    public class RopResetTableRequest : BaseStructure
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
        /// Parse the RopResetTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopResetTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
        }
    }

    /// <summary>
    ///  A class indicates the RopResetTable ROP Response Buffer.
    /// </summary>
    public class RopResetTableResponse : BaseStructure
    {
        /// <summary>
        /// An unsigned integer that specifies the type of ROP.
        /// </summary>
        public RopIdType RopId;

        /// <summary>
        /// An unsigned integer index that MUST be set to the value specified in the InputHandleIndex field in the request. c
        /// </summary>
        public byte InputHandleIndex;

        /// <summary>
        /// An unsigned integer that specifies the status of the ROP.
        /// </summary>
        public object ReturnValue;

        /// <summary>
        /// Parse the RopResetTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopResetTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());
        }
    }
    #endregion

    #region 2.2.2.17 RopExpandRow ROP
    /// <summary>
    /// The RopExpandRow ROP ([MS-OXCROPS] section 2.2.5.16) expands a collapsed category of a table and returns the rows that belong in the newly expanded category. 
    /// </summary>
    public class RopExpandRowRequest : BaseStructure
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
        /// An unsigned integer that specifies the maximum number of expanded rows to return data for.
        /// </summary>
        public ushort MaxRowCount;

        /// <summary>
        /// An identifier that specifies the category to be expanded.
        /// </summary>
        public long CategoryId;

        /// <summary>
        /// Parse the RopExpandRowRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopExpandRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.MaxRowCount = this.ReadUshort();
            this.CategoryId = this.ReadINT64();
        }
    }

    /// <summary>
    ///  A class indicates the RopExpandRow ROP Response Buffer.
    /// </summary>
    public class RopExpandRowResponse : BaseStructure
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
        /// An unsigned integer that specifies the total number of rows that are in the expanded category.
        /// </summary>
        public uint? ExpandedRowCount;

        /// <summary>
        /// An unsigned integer that specifies the number of PropertyRow structures.
        /// </summary>
        public ushort? RowCount;

        /// <summary>
        /// A list of PropertyRow structures. The number of structures contained in this field is specified by the RowCount field.
        /// </summary>
        public PropertyRow[] RowData;

        /// <summary>
        /// Each row MUST have the same columns and ordering of columns as specified in the last RopSetColumns ROP request ([MS-OXCROPS] section 2.2.5.1). 
        /// </summary>
        private PropertyTag[] propertiesBySetColum;

        /// <summary>
        /// Initializes a new instance of the RopExpandRowResponse class.
        /// </summary>
        /// <param name="propertiesBySetColum">Property Tags got from RopSetColumn</param>
        public RopExpandRowResponse(PropertyTag[] propertiesBySetColum)
        {
            this.propertiesBySetColum = propertiesBySetColum;
        }

        /// <summary>
        /// Parse the RopExpandRowResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopExpandRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.ExpandedRowCount = this.ReadUint();
                this.RowCount = this.ReadUshort();
                List<PropertyRow> tempPropertyRows = new List<PropertyRow>();
                for (int i = 0; i < this.RowCount; i++)
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
        /// An identifier that specifies the category to be collapsed.
        /// </summary>
        public long CategoryId;

        /// <summary>
        /// Parse the RopCollapseRowRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopCollapseRowRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.CategoryId = this.ReadINT64();
        }
    }

    /// <summary>
    ///  A class indicates the RopCollapseRow ROP Response Buffer.
    /// </summary>
    public class RopCollapseRowResponse : BaseStructure
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
        /// An unsigned integer that specifies the total number of rows in the collapsed category.
        /// </summary>
        public uint? CollapsedRowCount;

        /// <summary>
        /// Parse the RopCollapseRowResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopCollapseRowResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.CollapsedRowCount = this.ReadUint();
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
        /// An identifier that specifies the row to be preserved as the cursor. 
        /// </summary>
        public long RowId;

        /// <summary>
        /// An unsigned integer that specifies the instance number of the row that is to be preserved as the cursor.
        /// </summary>
        public uint RowInstanceNumber;

        /// <summary>
        /// Parse the RopGetCollapseStateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetCollapseStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.RowId = this.ReadINT64();
            this.RowInstanceNumber = this.ReadUint();
        }
    }

    /// <summary>
    ///  A class indicates the RopGetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopGetCollapseStateResponse : BaseStructure
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
        /// An unsigned integer that specifies the size of the CollapseState field.
        /// </summary>
        public ushort? CollapseStateSize;

        /// <summary>
        /// An array of bytes that specifies a collapse state for a categorized table. 
        /// </summary>
        public byte?[] CollapseState;

        /// <summary>
        /// Parse the RopGetCollapseStateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopGetCollapseStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.CollapseStateSize = this.ReadUshort();
                this.CollapseState = this.ConvertArray(this.ReadBytes((int)this.CollapseStateSize));
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
        /// An unsigned integer that specifies the size of the CollapseState field.
        /// </summary>
        public ushort CollapseStateSize;

        /// <summary>
        /// An array of bytes that specifies a collapse state for a categorized table. The size of this field, in bytes, is specified by the CollapseStateSize field.
        /// </summary>
        public byte[] CollapseState;

        /// <summary>
        /// Parse the RopSetCollapseStateRequest structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetCollapseStateRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.LogonId = this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.CollapseStateSize = this.ReadUshort();
            this.CollapseState = this.ReadBytes(this.CollapseStateSize);
        }
    }

    /// <summary>
    ///  A class indicates the RopSetCollapseState ROP Response Buffer.
    /// </summary>
    public class RopSetCollapseStateResponse : BaseStructure
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
        /// An unsigned integer that specifies the size of the Bookmark field.
        /// </summary>
        public ushort? BookmarkSize;

        /// <summary>
        /// An array of bytes that specifies the origin for the seek operation. 
        /// </summary>
        public byte?[] Bookmark;

        /// <summary>
        /// Parse the RopSetCollapseStateResponse structure.
        /// </summary>
        /// <param name="s">A stream containing RopSetCollapseStateResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RopId = (RopIdType)this.ReadByte();
            this.InputHandleIndex = this.ReadByte();
            this.ReturnValue = HelpMethod.FormatErrorCode(this.ReadUint());

            if ((ErrorCodes)this.ReturnValue == ErrorCodes.Success)
            {
                this.BookmarkSize = this.ReadUshort();
                this.Bookmark = this.ConvertArray(this.ReadBytes((int)this.BookmarkSize));
            }
        }
    }
    #endregion
}
