using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetMatchesRequest structure.
    /// 2.2.5.5 GetMatches
    /// </summary>
    public class GetMatchesRequest : Block
    {
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public BlockT<bool> HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the MinimalIds field.
        /// </summary>
        public BlockT<uint> MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that constitute an Explicit Table.
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public BlockT<uint> InterfaceOptionFlags;

        /// <summary>
        /// A Boolean value that specifies whether the Filter field is present.
        /// </summary>
        public BlockT<bool> HasFilter;

        /// <summary>
        /// A restriction, as specified in [MS-OXCDATA] section 2.12, that is to be applied to the rows in the address book container.
        /// </summary>
        public RestrictionType Filter;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyNameGuid and PropertyNameId fields are present.
        /// </summary>
        public BlockT<bool> HasPropertyName;

        /// <summary>
        /// The GUID of the property to be opened.
        /// </summary>
        public BlockGuid PropertyNameGuid;

        /// <summary>
        /// A 4-byte value that specifies the ID of the property to be opened.
        /// </summary>
        public BlockT<uint> PropertyNameId;

        /// <summary>
        /// An unsigned integer that specifies the number of rows the client is requesting.
        /// </summary>
        public BlockT<uint> RowCount;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public BlockT<bool> HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting.
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMatchesRequest structure.
        /// </summary>
        protected override void Parse()
        {
            Reserved = ParseT<uint>();
            HasState = ParseAs<byte, bool>();
            if (HasState) State = Parse<STAT>();
            HasMinimalIds = ParseAs<byte, bool>();
            if (HasMinimalIds)
            {
                MinimalIdCount = ParseT<uint>();
                var me = new List<MinimalEntryID>();

                for (int i = 0; i < MinimalIdCount; i++)
                {
                    me.Add(Parse<MinimalEntryID>());
                }

                MinimalIds = me.ToArray();
            }

            InterfaceOptionFlags = ParseT<uint>();
            HasFilter = ParseAs<byte, bool>();

            if (HasFilter)
            {
                var restriction = new RestrictionType(CountWideEnum.fourBytes);
                restriction.Parse(parser);
                Filter = restriction;
            }

            HasPropertyName = ParseAs<byte, bool>();

            if (HasPropertyName)
            {
                PropertyNameGuid = Parse<BlockGuid>();
                PropertyNameId = ParseT<uint>();
            }

            RowCount = ParseT<uint>();
            HasColumns = ParseAs<byte, bool>();
            if (HasColumns) Columns = Parse<LargePropertyTagArray>();
            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("GetMatchesRequest");
            AddChildBlockT(Reserved, "Reserved");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasMinimalIds, "HasMinimalIds");
            AddChildBlockT(MinimalIdCount, "MinimalIdCount");
            AddLabeledChildren(MinimalIds, "MinimalIds");
            AddChildBlockT(InterfaceOptionFlags, "InterfaceOptionFlags");
            AddChildBlockT(HasFilter, "HasFilter");
            AddChild(Filter, "Filter");
            AddChildBlockT(HasPropertyName, "HasPropertyName");
            this.AddChildGuid(PropertyNameGuid, "PropertyNameGuid");
            AddChildBlockT(PropertyNameId, "PropertyNameId");
            AddChildBlockT(RowCount, "RowCount");
            AddChildBlockT(HasColumns, "HasColumns");
            AddChild(Columns, "Columns");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}