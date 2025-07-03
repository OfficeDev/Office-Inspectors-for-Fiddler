using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetSpecialTableResponse structure.
    /// 2.2.5.8 GetSpecialTable
    /// </summary>
    public class GetSpecialTableResponse : BaseStructure
    {
        /// <summary>
        /// A string array that informs the client as to the state of processing a request on the server.
        /// </summary>
        public MAPIString[] MetaTags;

        /// <summary>
        /// A string array that specifies additional header information.
        /// </summary>
        public MAPIString[] AdditionalHeaders;

        /// <summary>
        /// An unsigned integer that specifies the status of the request.
        /// </summary>
        public uint StatusCode;

        /// <summary>
        /// An unsigned integer that specifies the return status of the operation.
        /// </summary>
        public uint ErrorCode;

        /// <summary>
        /// An unsigned integer that specifies the code page the server used to express string properties.
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// A Boolean value that specifies whether the Version field is present.
        /// </summary>
        public bool HasVersion;

        /// <summary>
        /// An unsigned integer that specifies the version number of the address book hierarchy table that the server has.
        /// </summary>
        public uint Version;

        /// <summary>
        /// A Boolean value that specifies whether the RowCount and Rows fields are present.
        /// </summary>
        public bool HasRows;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the Rows field.
        /// </summary>
        public uint RowsCount;

        /// <summary>
        /// An array of AddressBookPropertyValueList structures, each of which contains a row of the table that the client requested.
        /// </summary>
        public AddressBookPropertyValueList[] Rows;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetSpecialTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            MetaTags = metaTags.ToArray();
            AdditionalHeaders = additionalHeaders.ToArray();
            StatusCode = ReadUint();

            if (StatusCode == 0)
            {
                ErrorCode = ReadUint();
                CodePage = ReadUint();
                HasVersion = ReadBoolean();

                if (HasVersion)
                {
                    Version = ReadUint();
                }

                HasRows = ReadBoolean();

                if (HasRows)
                {
                    RowsCount = ReadUint();
                    List<AddressBookPropertyValueList> listAddressValue = new List<AddressBookPropertyValueList>();

                    for (int i = 0; i < RowsCount; i++)
                    {
                        AddressBookPropertyValueList addressValueList = new AddressBookPropertyValueList();
                        addressValueList.Parse(s);
                        listAddressValue.Add(addressValueList);
                    }

                    Rows = listAddressValue.ToArray();
                }
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
        }
    }
}