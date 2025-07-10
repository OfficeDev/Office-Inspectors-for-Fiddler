using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// A class indicates the Execute request type response body.
    /// 2.2.4 Request Types for Mailbox Server Endpoint
    /// 2.2.4.2.2 Execute Request Type Success Response Body
    /// 2.2.4.2.3 Execute Request Type Failure Response Body
    /// </summary>
    public class ExecuteResponseBody : BaseStructure
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
        /// The reserved flag. The server MUST set this field to 0x00000000 and the client MUST ignore this field.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        /// </summary>
        public uint RopBufferSize;

        /// <summary>
        /// A structure of bytes that constitute the ROP responses payload.
        /// </summary>
        public RgbOutputBufferPack RopBuffer;

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
                Flags = ReadUint();
                RopBufferSize = ReadUint();
                RopBuffer = new RgbOutputBufferPack(RopBufferSize);
                RopBuffer.Parse(s);
            }

            if (RemainingBytes() >= 4)
            {
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
}