using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Connect request type response body.
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.1 Connect
    /// </summary>
    public class ConnectResponseBody : BaseStructure
    {
        /// <summary>
        /// A string array that informs the client as to the state of processing a request on the server
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
        /// An unsigned integer that specifies the number of milliseconds for the maximum polling interval.
        /// </summary>
        public uint PollsMax;

        /// <summary>
        /// An unsigned integer that specifies the number of times to retry request types.
        /// </summary>
        public uint RetryCount;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds for the client to wait before retrying a failed request type. 
        /// </summary>
        public uint RetryDelay;

        /// <summary>
        /// A null-terminated ASCII string that specifies the DN prefix to be used for building message recipients. 
        /// </summary>
        public MAPIString DnPrefix;

        /// <summary>
        /// A null-terminated Unicode string that specifies the display name of the user who is specified in the UserDn field of the Connect request type request body.
        /// </summary>
        public MAPIString DisplayName;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
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
                PollsMax = ReadUint();
                RetryCount = ReadUint();
                RetryDelay = ReadUint();
                DnPrefix = new MAPIString(Encoding.ASCII);
                DnPrefix.Parse(s);
                DisplayName = new MAPIString(Encoding.Unicode);
                DisplayName.Parse(s);
            }

            AuxiliaryBufferSize = ReadUint();

            if (AuxiliaryBufferSize > 0)
            {
                AuxiliaryBuffer = new ExtendedBuffer();
                AuxiliaryBuffer.Parse(s);
            }
            else
            {
                AuxiliaryBuffer = null;
            }
        }
    }
}