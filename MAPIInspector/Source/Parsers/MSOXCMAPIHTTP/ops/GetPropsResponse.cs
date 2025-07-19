using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the GetPropsResponse structure.
    /// 2.2.5.7 GetProps
    /// </summary>
    public class GetPropsResponse : Block
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
        /// An unsigned integer that specifies the code page that the server used to express string properties.
        /// </summary>
        public BlockT<uint> CodePage;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyValues field is present.
        /// </summary>
        public BlockT<bool> HasPropertyValues;

        /// <summary>
        /// An AddressBookPropertyValueList structure (section 2.2.1.3) that contains the values of the properties requested.
        /// </summary>
        public AddressBookPropertyValueList PropertyValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public BlockT<uint> AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropsResponse structure.
        /// </summary>
        protected override void Parse()
        {
            ParseMAPIMethod.ParseAdditionalHeader(parser, out MetaTags, out AdditionalHeaders);
            StatusCode = ParseT<uint>();

            if (StatusCode == 0)
            {
                ErrorCode = ParseT<ErrorCodes>();
                CodePage = ParseT<uint>();
                HasPropertyValues = ParseAs<byte, bool>();

                if (HasPropertyValues)
                {
                    PropertyValues = Parse<AddressBookPropertyValueList>();
                }
            }

            AuxiliaryBufferSize = ParseT<uint>();

            if (AuxiliaryBufferSize > 0) AuxiliaryBuffer = Parse<ExtendedBuffer>();
        }

        protected override void ParseBlocks()
        {
            Text = "GetPropsResponse";
            AddLabeledChildren(MetaTags, "MetaTags");
            AddLabeledChildren(AdditionalHeaders, "AdditionalHeaders");
            AddChildBlockT(StatusCode, "StatusCode");
            this.AddError(ErrorCode, "ErrorCode ");
            AddChildBlockT(CodePage, "CodePage");
            AddChildBlockT(HasPropertyValues, "HasPropertyValues");
            AddChild(PropertyValues, "PropertyValues");
            AddChildBlockT(AuxiliaryBufferSize, "AuxiliaryBufferSize");
            AddChild(AuxiliaryBuffer, "AuxiliaryBuffer");
        }
    }
}