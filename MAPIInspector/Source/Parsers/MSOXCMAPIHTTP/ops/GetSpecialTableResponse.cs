using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetSpecialTableResponse structure.
    /// 2.2.5.8 GetSpecialTable
    /// </summary>
    public class GetSpecialTableResponse : Block
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
        /// An unsigned integer that specifies the code page the server used to express string properties.
        /// </summary>
        public BlockT<uint> CodePage;

        /// <summary>
        /// A Boolean value that specifies whether the Version field is present.
        /// </summary>
        public BlockT<bool> HasVersion;

        /// <summary>
        /// An unsigned integer that specifies the version number of the address book hierarchy table that the server has.
        /// </summary>
        public BlockT<uint> Version;

        /// <summary>
        /// A Boolean value that specifies whether the RowCount and Rows fields are present.
        /// </summary>
        public BlockT<bool> HasRows;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the Rows field.
        /// </summary>
        public BlockT<uint> RowsCount;

        /// <summary>
        /// An array of AddressBookPropertyValueList structures, each of which contains a row of the table that the client requested.
        /// </summary>
        public AddressBookPropertyValueList[] Rows;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableResponse structure.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                CodePage = ParseT<uint>();
                HasVersion = ParseAs<byte, bool>();
                if (HasVersion) Version = ParseT<uint>();
                HasRows = ParseAs<byte, bool>();
                if (HasRows)
                {
                    RowsCount = ParseT<uint>();
                    var listAddressValue = new List<AddressBookPropertyValueList>();

                    for (int i = 0; i < RowsCount; i++)
                    {
                        listAddressValue.Add(Parse<AddressBookPropertyValueList>());
                    }

                    Rows = listAddressValue.ToArray();
                }
            }

            AuxiliaryBufferSize = ParseT<uint>();
            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            SetText("GetSpecialTableResponse");
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            this.AddError(ErrorCode, "ErrorCode ");
            AddChildBlockT(CodePage, "CodePage");
            AddChildBlockT(HasVersion, "HasVersion");
            AddChild(Version, "Version");
            AddChildBlockT(HasRows, "HasRows");
            AddChildBlockT(RowsCount, "RowsCount");
            AddLabeledChildren(Rows, "Rows");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}