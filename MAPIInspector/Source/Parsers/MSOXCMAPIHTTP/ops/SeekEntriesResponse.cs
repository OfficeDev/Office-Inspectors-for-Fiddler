using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the SeekEntriesResponse structure.
    /// 2.2.5.16 SeekEntries
    /// </summary>
    public class SeekEntriesResponse : Block
    {
        /// <summary>
        /// A string array that informs the client as to the state of processing a request on the server.
        /// </summary>
        public BlockString[] MetaTags;

        /// <summary>
        /// A string array that specifies additional header information.
        /// </summary>
        public BlockString[] AdditionalHeaders;

        /// <summary>
        /// An unsigned integer that specifies the status of the request.
        /// </summary>
        public BlockT<uint> StatusCode;

        /// <summary>
        /// An unsigned integer that specifies the return status of the operation.
        /// </summary>
        public BlockT<ErrorCodes> ErrorCode;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public BlockT<bool> HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        /// </summary>
        public BlockT<bool> HasColsAndRows;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the columns used for the rows returned.
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the RowData field.
        /// </summary>
        public BlockT<uint> RowCount;

        /// <summary>
        /// An array of AddressBookPropertyRow structures, each of which specifies the row data for the entries queried.
        /// </summary>
        public AddressBookPropertyRow[] RowData;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the SeekEntriesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                HasState = ParseAs<byte, bool>();

                if (HasState)
                {
                    State = Parse<STAT>();
                    HasColsAndRows = ParseAs<byte, bool>();

                    if (HasColsAndRows)
                    {
                        Columns = Parse<LargePropertyTagArray>();
                        RowCount = ParseT<uint>();
                        var addressBookPropRow = new List<AddressBookPropertyRow>();
                        for (int i = 0; i < RowCount; i++)
                        {
                            var addressPropRow = new AddressBookPropertyRow(Columns);
                            addressPropRow.Parse(parser);
                            addressBookPropRow.Add(addressPropRow);
                        }

                        RowData = addressBookPropRow.ToArray();
                    }
                }
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("SeekEntriesResponse");
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            if (ErrorCode != null) AddChild(ErrorCode, $"ErrorCode:{ErrorCode.Data.FormatErrorCode()}");
            AddChildBlockT(HasState, "HasState");
            AddChild(State, "State");
            AddChildBlockT(HasColsAndRows, "HasColsAndRows");
            AddChild(Columns, "Columns");
            AddChildBlockT(RowCount, "RowCount");
            AddLabeledChildren(RowData, "RowData");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }

    }
}