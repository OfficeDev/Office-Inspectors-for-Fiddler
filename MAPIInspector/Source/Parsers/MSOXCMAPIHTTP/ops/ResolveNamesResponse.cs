using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the ResolveNamesResponse structure.
    /// 2.2.5.14 ResolveNames
    /// </summary>
    public class ResolveNamesResponse : Block
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
        /// An unsigned integer that specifies the code page the server used to express string values of properties.
        /// </summary>
        public BlockT<uint> CodePage;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public BlockT<bool> HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the MinimalIds field.
        /// </summary>
        public BlockT<uint> MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures, each of which specifies a Minimal Entry ID matching a name requested by the client.
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags, RowCount, and RowData fields are present.
        /// </summary>
        public BlockT<bool> HasRowsAndCols;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties returned for the rows in the RowData field.
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RowData field.
        /// </summary>
        public BlockT<uint> RowCount;

        /// <summary>
        /// An array of AddressBookPropertyRow structures (section 2.2.1.7), each of which specifies the row data requested.
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
        /// Parse the ResolveNamesResponse structure.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                CodePage = ParseT<uint>();
                HasMinimalIds = ParseAs<byte, bool>();

                if (HasMinimalIds)
                {
                    MinimalIdCount = ParseT<uint>();
                    var miniEIDList = new List<MinimalEntryID>();
                    for (int i = 0; i < MinimalIdCount; i++)
                    {
                        miniEIDList.Add(Parse<MinimalEntryID>());
                    }

                    MinimalIds = miniEIDList.ToArray();
                }

                HasRowsAndCols = ParseAs<byte, bool>();
                if (HasRowsAndCols)
                {
                    PropertyTags = Parse<LargePropertyTagArray>();
                    RowCount = ParseT<uint>();
                    var addressPRList = new List<AddressBookPropertyRow>();
                    for (int i = 0; i < RowCount; i++)
                    {
                        var addressPR = new AddressBookPropertyRow(PropertyTags);
                        addressPR.Parse(parser);
                        addressPRList.Add(addressPR);
                    }

                    RowData = addressPRList.ToArray();
                }
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "ResolveNamesResponse";
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            this.AddError(ErrorCode, "ErrorCode ");
            AddChildBlockT(CodePage, "CodePage");
            AddChildBlockT(HasMinimalIds, "HasMinimalIds");
            AddChildBlockT(MinimalIdCount, "MinimalIdCount");
            AddLabeledChildren(MinimalIds, "MinimalIds");
            AddChildBlockT(HasRowsAndCols, "HasRowsAndCols");
            AddChild(PropertyTags, "PropertyTags");
            AddChildBlockT(RowCount, "RowCount");
            AddLabeledChildren(RowData, "RowData");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}