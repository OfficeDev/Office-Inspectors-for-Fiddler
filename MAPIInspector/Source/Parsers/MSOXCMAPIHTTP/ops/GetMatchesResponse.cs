namespace MAPIInspector.Parsers
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///  A class indicates the GetMatchesResponse structure.
    ///  2.2.5.5 GetMatches
    /// </summary>
    public class GetMatchesResponse : BaseStructure
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
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public bool HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the MinimalIds field. 
        /// </summary>
        public uint MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures 
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        /// </summary>
        public bool HasColsAndRows;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns used for each row returned. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RowData field. 
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// An array of AddressBookPropertyRow structures (section 2.2.1.7), each of which specifies the row data for the entries requested. 
        /// </summary>
        public AddressBookPropertyRow[] RowData;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMatchesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetMatchesResponse structure.</param>
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
                HasState = ReadBoolean();

                if (HasState)
                {
                    State = new STAT();
                    State.Parse(s);
                }

                HasMinimalIds = ReadBoolean();

                if (HasMinimalIds)
                {
                    MinimalIdCount = ReadUint();
                    List<MinimalEntryID> listMinimalEID = new List<MinimalEntryID>();

                    for (int i = 0; i < MinimalIdCount; i++)
                    {
                        MinimalEntryID minialEID = new MinimalEntryID();
                        minialEID.Parse(s);
                        listMinimalEID.Add(minialEID);
                    }

                    MinimalIds = listMinimalEID.ToArray();
                }

                HasColsAndRows = ReadBoolean();

                if (HasColsAndRows)
                {
                    Columns = new LargePropertyTagArray();
                    Columns.Parse(s);
                    RowCount = ReadUint();
                    List<AddressBookPropertyRow> addressBookPropRow = new List<AddressBookPropertyRow>();

                    for (int i = 0; i < RowCount; i++)
                    {
                        AddressBookPropertyRow addressPropRow = new AddressBookPropertyRow(Columns);
                        addressPropRow.Parse(s);
                        addressBookPropRow.Add(addressPropRow);
                    }

                    RowData = addressBookPropRow.ToArray();
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