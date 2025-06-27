using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetTemplateInfoResponse structure.
    /// 2.2.5.9 GetTemplateInfo
    /// </summary>
    public class GetTemplateInfoResponse : BaseStructure
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
        /// A Boolean value that specifies whether the Row field is present.
        /// </summary>
        public bool HasRow;

        /// <summary>
        /// A AddressBookPropertyValueList structure (section 2.2.1.3) that specifies the information that the client requested. 
        /// </summary>
        public AddressBookPropertyValueList Row;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetTemplateInfoResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetTemplateInfoResponse structure.</param>
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
                HasRow = ReadBoolean();

                if (HasRow)
                {
                    Row = new AddressBookPropertyValueList();
                    Row.Parse(s);
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