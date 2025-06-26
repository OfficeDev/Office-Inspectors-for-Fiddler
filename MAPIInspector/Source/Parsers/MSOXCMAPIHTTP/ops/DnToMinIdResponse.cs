using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    ///  A class indicates the DnToMinIdResponse structure.
    ///  2.2.5 Request Types for Address Book Server Endpoint
    ///  2.2.5.4 DnToMinId
    /// </summary>
    public class DnToMinIdResponse : BaseStructure
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
        /// A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        /// </summary>
        public bool HasMinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the MinimalIds field.
        /// </summary>
        public uint MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1)
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the DnToMinIdResponse structure.
        /// </summary>
        /// <param name="s">A stream containing DnToMinIdResponse structure.</param>
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
                HasMinimalIds = ReadBoolean();
                MinimalIdCount = ReadUint();
                List<MinimalEntryID> lm = new List<MinimalEntryID>();

                for (int i = 0; i < MinimalIdCount; i++)
                {
                    MinimalEntryID me = new MinimalEntryID();
                    me.Parse(s);
                    lm.Add(me);
                }

                MinimalIds = lm.ToArray();
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