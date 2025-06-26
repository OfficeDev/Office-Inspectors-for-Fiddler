using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the ResolveNamesResponse structure.
    ///  2.2.5.14 ResolveNames
    /// </summary>
    public class ResolveNamesResponse : BaseStructure
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
        /// An unsigned integer that specifies the code page the server used to express string values of properties.
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public bool HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the MinimalIds field. 
        /// </summary>
        public uint MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures, each of which specifies a Minimal Entry ID matching a name requested by the client. 
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags, RowCount, and RowData fields are present.
        /// </summary>
        public bool HasRowsAndCols;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties returned for the rows in the RowData field. 
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RowData field. 
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// An array of AddressBookPropertyRow structures (section 2.2.1.7), each of which specifies the row data requested. 
        /// </summary>
        public AddressBookPropertyRow[] RowData;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResolveNamesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing ResolveNamesResponse structure.</param>
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
                HasMinimalIds = ReadBoolean();

                if (HasMinimalIds)
                {
                    MinimalIdCount = ReadUint();
                    List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                    for (int i = 0; i < MinimalIdCount; i++)
                    {
                        MinimalEntryID miniEID = new MinimalEntryID();
                        miniEID.Parse(s);
                        miniEIDList.Add(miniEID);
                    }

                    MinimalIds = miniEIDList.ToArray();
                }

                HasRowsAndCols = ReadBoolean();

                if (HasRowsAndCols)
                {
                    PropertyTags = new LargePropertyTagArray();
                    PropertyTags.Parse(s);
                    RowCount = ReadUint();
                    List<AddressBookPropertyRow> addressPRList = new List<AddressBookPropertyRow>();

                    for (int i = 0; i < RowCount; i++)
                    {
                        AddressBookPropertyRow addressPR = new AddressBookPropertyRow(PropertyTags);
                        addressPR.Parse(s);
                        addressPRList.Add(addressPR);
                    }

                    RowData = addressPRList.ToArray();
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