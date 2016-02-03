using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Be.Windows.Forms;

namespace MAPIInspector.Parsers
{
    # region 2.2.4	Request Types for Mailbox Server Endpoint
    #region 2.2.4.1	Connect

    /// <summary>
    ///  A class indicates the Connect request type.
    /// </summary>
    public class ConnectRequestBody : BaseStructure
    {
        // A null-terminated ASCII string that specifies the DN of the user who is requesting the connection. 
        public MAPIString UserDn;

        // A set of flags that designate the type of connection being requested. 
        public uint Flags;

        // An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        public uint DefaultCodePage;

        // An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for sorting. 
        public uint LcidSort;

        // An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for everything other than sorting. 
        public uint LcidString;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.UserDn = new MAPIString(Encoding.ASCII);
            this.UserDn.Parse(s);
            this.Flags = ReadUint();
            this.DefaultCodePage = ReadUint();
            this.LcidSort = ReadUint();
            this.LcidString = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    /// <summary>
    /// A class indicates the Connect request type response body.
    /// </summary>
    public class ConnectResponseBody : BaseStructure
    {

        // A string array that informs the client as to the state of processing a request on the server

        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.

        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the number of milliseconds for the maximum polling interval.
        public uint PollsMax;

        // An unsigned integer that specifies the number of times to retry request types.
        public uint RetryCount;

        // An unsigned integer that specifies the number of milliseconds for the client to wait before retrying a failed request type. 
        public uint RetryDelay;

        //A null-terminated ASCII string that specifies the DN prefix to be used for building message recipients. 

        public MAPIString DnPrefix;

        //A null-terminated Unicode string that specifies the display name of the user who is specified in the UserDn field of the Connect request type request body. 

        public MAPIString DisplayName;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.PollsMax = ReadUint();
                this.RetryCount = ReadUint();
                this.RetryDelay = ReadUint();
                this.DnPrefix = new MAPIString(Encoding.ASCII);
                this.DnPrefix.Parse(s);
                this.DisplayName = new MAPIString(Encoding.Unicode);
                this.DisplayName.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    #endregion

    #region 2.2.4.2	Execute

    /// <summary>
    ///  A class indicates the Execute request type.
    /// </summary>
    public class ExecuteRequestBody : BaseStructure
    {
        // An unsigned integer that specify to the server how to build the ROP responses in the RopBuffer field of the Execute request type success response body.
        public uint Flags;

        // An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        public uint RopBufferSize;

        // An structure of bytes that constitute the ROP request payload. 
        public rgbInputBuffer RopBuffer;

        // An unsigned integer that specifies the maximum size for the RopBuffer field of the Execute request type success response body.
        public uint MaxRopOut;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = ReadUint();
            this.RopBufferSize = ReadUint();
            this.RopBuffer = new rgbInputBuffer();
            this.RopBuffer.Parse(s);
            this.MaxRopOut = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    /// <summary>
    /// A class indicates the Execute request type response body.
    /// </summary>
    public class ExecuteResponseBody : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server

        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.

        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // The reserved flag. The server MUST set this field to 0x00000000 and the client MUST ignore this field.
        public uint Flags;

        // An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        public uint RopBufferSize;

        // A structure of bytes that constitute the ROP responses payload. 
        public rgbOutputBufferPack RopBuffer;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.Flags = ReadUint();
                this.RopBufferSize = ReadUint();
                this.RopBuffer = new rgbOutputBufferPack(this.RopBufferSize);
                this.RopBuffer.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    #endregion

    #region 2.2.4.3	Disconnect

    /// <summary>
    ///  A class indicates the Disconnect request type.
    /// </summary>
    public class DisconnectRequestBody : BaseStructure
    {
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    /// <summary>
    /// A class indicates the Disconnect request type response body.
    /// </summary>
    public class DisconnectResponseBody : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server

        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.

        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    #endregion

    #region 2.2.4.4	NotificationWait

    /// <summary>
    /// A class indicates the NotificationWait request type response body.
    /// </summary>
    public class NotificationWaitRequestBody : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Flags;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    /// <summary>
    /// A class indicates the NotificationWait request type response body.
    /// </summary>
    public class NotificationWaitResponseBody : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server

        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.

        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        //An unsigned integer that indicates whether an event is pending on the Session Context. 
        public uint EventPending;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.EventPending = ReadUint();
            }
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    #endregion
    #endregion

    #region 2.2.5	Request Types for Address Book Server Endpoint

    #region 2.2.5.1 Bind
    /// <summary>
    ///  A class indicates the Bind request type request body.
    /// </summary>
    public class BindRequest : BaseStructure
    {
        // An unsigned integer that specify the authentication type for the connection.
        public uint Flags;

        // A Boolean value that specifies whether the State field is present.
        public byte HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.HasState = ReadByte();
            if (HasState != 0)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            else
            {
                this.State = null;
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    /// <summary>
    /// A class indicates the Bind request type response body.
    /// </summary>
    class BindResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.

        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.

        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A GUID that is associated with a specific address book server.
        public Guid ServerGuid;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.  
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.ServerGuid = ReadGuid();
            }
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }
    #endregion

    #region 2.2.5.2 Unbind

    /// <summary>
    /// A class indicates the UnbindRequest structure.
    /// </summary>
    public class UnbindRequest : BaseStructure
    {
        // Reserved.
        public uint Reserved;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }

    }

    /// <summary>
    /// A class indicates the UnbindResponse structure.
    /// </summary>
    public class UnbindResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.

        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.

        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.  
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
            }
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }

    }
    #endregion

    #region 2.2.5.3	CompareMinIds

    /// <summary>
    ///  A class indicates the CompareMinIdsRequest structure.
    /// </summary>
    public class CompareMinIdsRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field. 
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public byte HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A MinimalEntryID structure ([MS-OXNSPI] section 2.2.9.1) that specifies the Minimal Entry ID of the first object.
        public MinimalEntryID MinimalId1;

        // A MinimalEntryID structure that specifies the Minimal Entry ID of the second object.
        public MinimalEntryID MinimalId2;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadByte();
            if (this.HasState != 0)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            else
            {
                this.State = null;
            }

            this.MinimalId1 = new MinimalEntryID();
            this.MinimalId1.Parse(s);
            this.MinimalId2 = new MinimalEntryID();
            this.MinimalId2.Parse(s);

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }

        }
    }

    /// <summary>
    ///  A class indicates the CompareMinIdsResponse structure.
    /// </summary>
    public class CompareMinIdsResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A signed integer that specifies the result of the comparison. 
        public int Result;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the CompareMinIdsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing CompareMinIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.Result = ReadINT32();
            }

            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }
    #endregion 2.2.5.3

    #region 2.2.5.4 DnToMinId
    /// <summary>
    ///  A class indicates the DnToMinIdRequest structure.
    /// </summary>
    public class DnToMinIdRequest : BaseStructure
    {
        // Reserved. 
        public uint Reserved;

        // A Boolean value that specifies whether the NameCount and NameValues fields are present.
        public bool HasNames;

        // An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field. 
        public uint NameCount;

        // An array of null-terminated ASCII strings which are distinguished names (DNs) to be mapped to Minimal Entry IDs. 
        public MAPIString[] NameValues;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the DnToMinIdRequest structure.
        /// </summary>
        /// <param name="s">An stream containing DnToMinIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasNames = ReadBoolean();
            this.NameCount = ReadUint();
            List<MAPIString> nameValues = new List<MAPIString>();
            for(int i=0; i< this.NameCount; i++)
            {
                MAPIString mapiString = new MAPIString(Encoding.ASCII);
                mapiString.Parse(s);
                nameValues.Add(mapiString);
            }
            this.NameValues = nameValues.ToArray();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }

    }

    /// <summary>
    ///  A class indicates the DnToMinIdResponse structure.
    /// </summary>
    public class DnToMinIdResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        public bool HasMinimalIds;

        // An unsigned integer that specifies the number of structures in the MinimalIds field.
        public uint MinimalIdCount;

        // An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1)
        public MinimalEntryID[] MinimalIds;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the DnToMinIdResponse structure.
        /// </summary>
        /// <param name="s">An stream containing DnToMinIdResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasMinimalIds = ReadBoolean();
                this.MinimalIdCount = ReadUint();
                List<MinimalEntryID> lm = new List<MinimalEntryID>();
                for (int i = 0; i < MinimalIdCount; i++)
                {
                    MinimalEntryID me = new MinimalEntryID();
                    me.Parse(s);
                    lm.Add(me);
                }
                this.MinimalIds = lm.ToArray();
            }

            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }

    }

    #endregion 2.2.5.4

    #region 2.2.5.5	GetMatches
    /// <summary>
    ///  A class indicates the GetMatchesRequest structure.
    /// </summary>
    public class GetMatchesRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        public bool HasMinimalIds;

        // An unsigned integer that specifies the number of structures present in the MinimalIds field. 
        public uint MinimalIdCount;

        // An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that constitute an Explicit Table. 
        public MinimalEntryID[] MinimalIds;

        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint InterfaceOptionFlags;

        // A Boolean value that specifies whether the Filter field is present.
        public bool HasFilter;

        // A restriction, as specified in [MS-OXCDATA] section 2.12, that is to be applied to the rows in the address book container. 
        public RestrictionType Filter;

        // A Boolean value that specifies whether the PropertyNameGuid and PropertyNameId fields are present.
        public bool HasPropertyName;

        // The GUID of the property to be opened. 
        public Guid PropertyNameGuid;

        // A 4-byte value that specifies the ID of the property to be opened. 
        public uint PropertyNameId;

        // An unsigned integer that specifies the number of rows the client is requesting.
        public uint RowCount;

        // A Boolean value that specifies whether the Columns field is present.
        public bool HasColumns;

        // A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMatchesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetMatchesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                STAT stat = new STAT();
                stat.Parse(s);
                this.State = stat;
            }

            this.HasMinimalIds = ReadBoolean();
            if (this.HasMinimalIds)
            {
                this.MinimalIdCount = ReadUint();
                List<MinimalEntryID> me = new List<MinimalEntryID>();
                for (int i = 0; i < this.MinimalIdCount; i++)
                {
                    MinimalEntryID mEntryId = new MinimalEntryID();
                    mEntryId.Parse(s);
                    me.Add(mEntryId);
                }
                this.MinimalIds = me.ToArray();
            }

            this.InterfaceOptionFlags = ReadUint();

            this.HasFilter = ReadBoolean();
            if (this.HasFilter)
            {
                RestrictionType restriction = new RestrictionType();
                restriction.Parse(s);
                this.Filter = restriction;
            }

            this.HasPropertyName = ReadBoolean();
            if (this.HasPropertyName)
            {
                this.PropertyNameGuid = ReadGuid();
                this.PropertyNameId = ReadUint();
            }

            this.RowCount = ReadUint();
            this.HasColumns = ReadBoolean();
            if (this.HasColumns)
            {
                LargePropertyTagArray largePTA = new LargePropertyTagArray();
                largePTA.Parse(s);
                this.Columns = largePTA;
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }

        }

    }

    /// <summary>
    ///  A class indicates the GetMatchesResponse structure.
    /// </summary>
    public class GetMatchesResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        public bool HasMinimalIds;

        // An unsigned integer that specifies the number of structures present in the MinimalIds field. 
        public uint MinimalIdCount;

        // An array of MinimalEntryID structures 
        public MinimalEntryID[] MinimalIds;

        // A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        public bool HasColsAndRows;

        // A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns used for each row returned. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the number of structures in the RowData field. 
        public uint RowCount;

        // An array of AddressBookPropertyRow structures (section 2.2.1.7), each of which specifies the row data for the entries requested. 
        public AddressBookPropertyRow[] RowData;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data returned from the server.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMatchesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetMatchesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasState = ReadBoolean();
                if (this.HasState)
                {
                    STAT stat = new STAT();
                    stat.Parse(s);
                }
                this.HasMinimalIds = ReadBoolean();
                if (this.HasMinimalIds)
                {
                    this.MinimalIdCount = ReadUint();
                    List<MinimalEntryID> listMinimalEID = new List<MinimalEntryID>();
                    for (int i = 0; i < MinimalIdCount; i++)
                    {
                        MinimalEntryID minialEID = new MinimalEntryID();
                        minialEID.Parse(s);
                        listMinimalEID.Add(minialEID);
                    }
                    this.MinimalIds = listMinimalEID.ToArray();
                }
                this.HasColsAndRows = ReadBoolean();
                if (this.HasColsAndRows)
                {
                    this.Columns = new LargePropertyTagArray();
                    this.Columns.Parse(s);
                    this.RowCount = ReadUint();
                    List<AddressBookPropertyRow> addressBookPropRow = new List<AddressBookPropertyRow>();
                    for (int i = 0; i < this.RowCount; i++)
                    {
                        AddressBookPropertyRow addressPropRow = new AddressBookPropertyRow(this.Columns);
                        addressPropRow.Parse(s);
                        addressBookPropRow.Add(addressPropRow);
                    }
                    this.RowData = addressBookPropRow.ToArray();
                }
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    #endregion 2.2.5.5

    #region 2.2.5.6	GetPropList
    /// <summary>
    ///  A class indicates the GetPropListRequest structure.
    /// </summary>
    public class GetPropListRequest : BaseStructure
    {
        // A set of bit flags that specify options to the server. 
        public uint Flags;

        // A MinimalEntryID structure that specifies the object for which to return properties.
        public MinimalEntryID MinimalId;

        // An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        public uint CodePage;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropListRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetPropListRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.MinimalId = new MinimalEntryID();
            this.MinimalId.Parse(s);
            this.CodePage = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the GetPropListResponse structure.
    /// </summary>
    public class GetPropListResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the PropertyTags field is present.
        public bool HasPropertyTags;

        // A LargePropertyTagArray structure (section 2.2.1.8) that contains the property tags of properties that have values on the requested object. 
        public LargePropertyTagArray PropertyTags;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropListResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetPropListResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasPropertyTags = ReadBoolean();
                if (this.HasPropertyTags)
                {
                    this.PropertyTags = new LargePropertyTagArray();
                    this.PropertyTags.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.6

    #region 2.2.5.7	GetProps
    /// <summary>
    ///  A class indicates the GetPropsRequest structure.
    /// </summary>
    public class GetPropsRequest : BaseStructure
    {
        // A set of bit flags that specify options to the server. 
        public uint Flags;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the PropertyTags field is present
        public bool HasPropertyTags;

        // A LargePropertyTagArray structure (section 2.2.1.8) that contains the property tags of the properties that the client is requesting. 
        public LargePropertyTagArray PropertyTags;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetPropsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.HasPropertyTags = ReadBoolean();
            if (this.HasPropertyTags)
            {
                this.PropertyTags = new LargePropertyTagArray();
                this.PropertyTags.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the GetPropsResponse structure.
    /// </summary>
    public class GetPropsResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the code page that the server used to express string properties. 
        public uint CodePage;

        // A Boolean value that specifies whether the PropertyValues field is present.
        public bool HasPropertyValues;

        // An AddressBookPropertyValueList structure (section 2.2.1.3) that contains the values of the properties requested. 
        public AddressBookPropertyValueList PropertyValues;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetPropsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.CodePage = ReadUint();
                this.HasPropertyValues = ReadBoolean();
                if (this.HasPropertyValues)
                {
                    this.PropertyValues = new AddressBookPropertyValueList();
                    this.PropertyValues.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.7

    #region 2.2.5.8	GetSpecialTable
    /// <summary>
    ///  A class indicates the GetSpecialTableRequest structure.
    /// </summary>
    public class GetSpecialTableRequest : BaseStructure
    {
        // A set of bit flags that specify options to the server. 
        public uint Flags;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.  
        public STAT State;

        // A Boolean value that specifies whether the Version field is present.
        public bool HasVersion;

        // An unsigned integer that specifies the version number of the address book hierarchy table that the client has. 
        public uint Version;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetSpecialTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.HasVersion = ReadBoolean();
            if (this.HasVersion)
            {
                this.Version = ReadUint();
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the GetSpecialTableResponse structure.
    /// </summary>
    public class GetSpecialTableResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the code page the server used to express string properties. 
        public uint CodePage;

        // A Boolean value that specifies whether the Version field is present.
        public bool HasVersion;

        // An unsigned integer that specifies the version number of the address book hierarchy table that the server has. 
        public uint Version;

        // A Boolean value that specifies whether the RowCount and Rows fields are present.
        public bool HasRows;

        // An unsigned integer that specifies the number of structures in the Rows field. 
        public uint RowsCount;

        // An array of AddressBookPropertyValueList structures, each of which contains a row of the table that the client requested. 
        public AddressBookPropertyValueList[] Rows;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetSpecialTableResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.CodePage = ReadUint();
                this.HasVersion = ReadBoolean();
                if (this.HasVersion)
                {
                    this.Version = ReadUint();
                }
                this.HasRows = ReadBoolean();
                if (this.HasRows)
                {
                    this.RowsCount = ReadUint();
                    List<AddressBookPropertyValueList> listAddressValue = new List<AddressBookPropertyValueList>();
                    for (int i = 0; i < this.RowsCount; i++)
                    {
                        AddressBookPropertyValueList addressValueList = new AddressBookPropertyValueList();
                        addressValueList.Parse(s);
                        listAddressValue.Add(addressValueList);
                    }
                    this.Rows = listAddressValue.ToArray();
                }
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }
    #endregion 2.2.5.8

    #region 2.2.5.9	GetTemplateInfo
    /// <summary>
    ///  A class indicates the GetTemplateInfoRequest structure.
    /// </summary>
    public class GetTemplateInfoRequest : BaseStructure
    {
        // A set of bit flags that specify options to the server. 
        public uint Flags;

        // An unsigned integer that specifies the display type of the template for which information is requested. 
        public uint DisplayType;

        // A Boolean value that specifies whether the TemplateDn field is present.
        public bool HasTemplateDn;

        // A null-terminated ASCII string that specifies the DN of the template requested. 
        public MAPIString TemplateDn;

        // An unsigned integer that specifies the code page of the template for which information is requested.
        public uint CodePage;

        // An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], of the template for which information is requested.
        public uint LocaleId;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetTemplateInfoRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetTemplateInfoRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.DisplayType = ReadUint();
            this.HasTemplateDn = ReadBoolean();
            if (this.HasTemplateDn)
            {
                this.TemplateDn = new MAPIString(Encoding.ASCII);
                this.TemplateDn.Parse(s);
            }
            this.CodePage = ReadUint();
            this.LocaleId = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the GetTemplateInfoResponse structure.
    /// </summary>
    public class GetTemplateInfoResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the code page the server used to express string values of properties.
        public uint CodePage;

        // A Boolean value that specifies whether the Row field is present.
        public bool HasRow;

        // A AddressBookPropertyValueList structure (section 2.2.1.3) that specifies the information that the client requested. 
        public AddressBookPropertyValueList Row;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetTemplateInfoResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetTemplateInfoResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.CodePage = ReadUint();
                this.HasRow = ReadBoolean();
                if (this.HasRow)
                {
                    this.Row = new AddressBookPropertyValueList();
                    this.Row.Parse(s);
                }
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }
    #endregion 2.2.5.9

    #region 2.2.5.10 ModLinkAtt
    /// <summary>
    ///  A class indicates the ModLinkAttRequest structure.
    /// </summary>
    public class ModLinkAttRequest : BaseStructure
    {
        // A set of bit flags that specify options to the server. 
        public uint Flags;

        // A PropertyTag structure that specifies the property to be modified.
        public PropertyTag PropertyTag;

        // A MinimalEntryID structure that specifies the Minimal Entry ID of the address book row to be modified.
        public MinimalEntryID MinimalId;

        // A Boolean value that specifies whether the EntryIdCount and EntryIds fields are present.
        public bool HasEntryIds;

        // An unsigned integer that specifies the count of structures in the EntryIds field. 
        public uint EntryIdCount;

        // An array of entry IDs, each of which is either an EphemeralEntryID structure or a PermanentEntryID structure. 
        public object[] EntryIds;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModLinkAttRequest structure.
        /// </summary>
        /// <param name="s">An stream containing ModLinkAttRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.MinimalId = new MinimalEntryID();
            this.MinimalId.Parse(s);
            this.HasEntryIds = ReadBoolean();
            if (this.HasEntryIds)
            {
                this.EntryIdCount = ReadUint();
                List<object> tempObj = new List<object>();
                for (int i = 0; i < this.EntryIdCount; i++)
                {
                    byte currentByte = ReadByte();
                    s.Position -= 1;
                    if (currentByte == 0x87)
                    {
                        EphemeralEntryID ephemeralEntryID = new EphemeralEntryID();
                        ephemeralEntryID.Parse(s);
                        tempObj.Add(ephemeralEntryID);
                    }
                    else if (currentByte == 0x00)
                    {
                        PermanentEntryID permanentEntryID = new PermanentEntryID();
                        permanentEntryID.Parse(s);
                        tempObj.Add(permanentEntryID);
                    }
                }
                this.EntryIds = tempObj.ToArray();
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the ModLinkAttResponse structure.
    /// </summary>
    public class ModLinkAttResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModLinkAttResponse structure.
        /// </summary>
        /// <param name="s">An stream containing ModLinkAttResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.10

    #region 2.2.5.11 ModProps

    /// <summary>
    ///  A class indicates the ModPropsRequest structure.
    /// </summary>
    public class ModPropsRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.  
        public STAT State;

        // A Boolean value that specifies whether the PropertyTags field is present.
        public bool HasPropertyTags;

        // A LargePropertyTagArray structure that specifies the properties to be removed. 
        public LargePropertyTagArray PropertiesTags;

        // A Boolean value that specifies whether the PropertyValues field is present.
        public bool HasPropertyValues;

        // An AddressBookPropertyValueList structure that specifies the values of the properties to be modified. 
        public AddressBookPropertyValueList PropertyValues;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModPropsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing ModPropsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.HasPropertyTags = ReadBoolean();
            if (this.HasPropertyTags)
            {
                this.PropertiesTags = new LargePropertyTagArray();
                this.PropertiesTags.Parse(s);
            }
            this.HasPropertyValues = ReadBoolean();
            if (this.HasPropertyValues)
            {
                this.PropertyValues = new AddressBookPropertyValueList();
                this.PropertyValues.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the ModPropsResponse structure.
    /// </summary>
    public class ModPropsResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModPropsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing ModPropsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }
    #endregion 2.2.5.11

    #region 2.2.5.12 QueryRows
    /// <summary>
    ///  A class indicates the QueryRowsRequest structure.
    /// </summary>
    public class QueryRowsRequest : BaseStructure
    {
        // An unsigned integer that specify the authentication type for the connection.
        public uint Flags;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // An unsigned integer that specifies the number of structures present in the ExplicitTable field. 
        public uint ExplicitTableCount;

        // An array of MinimalEntryID structures that constitute the Explicit Table.
        public MinimalEntryID[] ExplicitTable;

        // An unsigned integer that specifies the number of rows the client is requesting.
        public uint RowCount;

        // A Boolean value that specifies whether the Columns field is present.
        public bool HasColumns;

        // A LargePropertyTagArray structure that specifies the properties that the client requires for each row returned. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryRowsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing QueryRowsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.ExplicitTableCount = ReadUint();
            List<MinimalEntryID> miniEntryIDlist = new List<MinimalEntryID>();
            for (int i = 0; i < this.ExplicitTableCount; i++)
            {
                MinimalEntryID miniEntryID = new MinimalEntryID();
                miniEntryID.Parse(s);
                miniEntryIDlist.Add(miniEntryID);
            }
            this.ExplicitTable = miniEntryIDlist.ToArray();
            this.RowCount = ReadUint();
            this.HasColumns = ReadBoolean();
            if (this.HasColumns)
            {
                this.Columns = new LargePropertyTagArray();
                this.Columns.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }

    /// <summary>
    ///  A class indicates the QueryRowsResponse structure.
    /// </summary>
    public class QueryRowsResponse : BaseStructure
    {
        // A string array that informs the client as to the state of processing a request on the server.
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        public bool HasColsAndRows;

        // A LargePropertyTagArray structure that specifies the columns for the returned rows. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the number of structures in the RowData field. 
        public uint RowCount;

        // An array of AddressBookPropertyRow structures, each of which specifies the row data of the Explicit Table. 
        public AddressBookPropertyRow[] RowData;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryRowsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing QueryRowsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasState = ReadBoolean();
                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                }
                this.HasColsAndRows = ReadBoolean();
                if (this.HasColsAndRows)
                {
                    this.Columns = new LargePropertyTagArray();
                    this.Columns.Parse(s);
                    this.RowCount = ReadUint();
                    List<AddressBookPropertyRow> addressBookPRList = new List<AddressBookPropertyRow>();
                    for (int i = 0; i < this.RowCount; i++)
                    {
                        AddressBookPropertyRow addressBookPR = new AddressBookPropertyRow(this.Columns);
                        addressBookPR.Parse(s);
                        addressBookPRList.Add(addressBookPR);
                    }
                    this.RowData = addressBookPRList.ToArray();
                }
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.12

    #region 2.2.5.13 QueryColumns
    /// <summary>
    ///  A class indicates the QueryColumnsRequest structure.
    /// </summary>
    public class QueryColumnsRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A set of bit flags that specify options to the server. 
        public uint MapiFlags;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryColumnsRequest structure.
        /// </summary>
        /// <param name="s">An stream containing QueryColumnsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.MapiFlags = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the QueryColumnsResponse structure.
    /// </summary>
    public class QueryColumnsResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the Columns field is present.
        public bool HasColumns;

        // A LargePropertyTagArray structure that specifies the properties that exist on the address book. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryColumnsResponse structure.
        /// </summary>
        /// <param name="s">An stream containing QueryColumnsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasColumns = ReadBoolean();
                if (this.HasColumns)
                {
                    this.Columns = new LargePropertyTagArray();
                    this.Columns.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.13

    #region 2.2.5.14 ResolveNames
    /// <summary>
    ///  A class indicates the ResolveNamesRequest structure.
    /// </summary>
    public class ResolveNamesRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the PropertyTags field is present.
        public bool HasPropertyTags;

        // A LargePropertyTagArray structure that specifies the properties that client requires for the rows returned. 
        public LargePropertyTagArray PropertyTags;

        // A Boolean value that specifies whether the NameCount and NameValues fields are present.
        public bool HasNames;

        // An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field. TODO:
        public uint NameCount;

        // An array of null-terminated Unicode strings. The number of strings is specified by the NameCount field. 
        public WStringArray_r Names;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResolveNamesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing ResolveNamesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.HasPropertyTags = ReadBoolean();
            if (this.HasPropertyTags)
            {
                this.PropertyTags = new LargePropertyTagArray();
                this.PropertyTags.Parse(s);
            }
            this.HasNames = ReadBoolean();
            if (this.HasNames)
            {
                this.Names = new WStringArray_r();
                this.Names.Parse(s);
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the ResolveNamesResponse structure.
    /// </summary>
    public class ResolveNamesResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // An unsigned integer that specifies the code page the server used to express string values of properties.
        public uint CodePage;

        // A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        public bool HasMinimalIds;

        // An unsigned integer that specifies the number of structures in the MinimalIds field. 
        public uint MinimalIdCount;

        // An array of MinimalEntryID structures, each of which specifies a Minimal Entry ID matching a name requested by the client. 
        public MinimalEntryID[] MinimalIds;

        // A Boolean value that specifies whether the PropertyTags, RowCount, and RowData fields are present.
        public bool HasRowsAndCols;

        // A LargePropertyTagArray structure that specifies the properties returned for the rows in the RowData field. 
        public LargePropertyTagArray PropertyTags;

        // An unsigned integer that specifies the number of structures in the RowData field. 
        public uint RowCount;

        // An array of AddressBookPropertyRow structures (section 2.2.1.7), each of which specifies the row data requested. 
        public AddressBookPropertyRow[] RowData;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResolveNamesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing ResolveNamesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.CodePage = ReadUint();
                this.HasMinimalIds = ReadBoolean();
                if (this.HasMinimalIds)
                {
                    this.MinimalIdCount = ReadUint();
                    List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();
                    for (int i = 0; i < this.MinimalIdCount; i++)
                    {
                        MinimalEntryID miniEID = new MinimalEntryID();
                        miniEID.Parse(s);
                        miniEIDList.Add(miniEID);
                    }
                    this.MinimalIds = miniEIDList.ToArray();
                }
                this.HasRowsAndCols = ReadBoolean();
                if (this.HasRowsAndCols)
                {
                    this.PropertyTags = new LargePropertyTagArray();
                    this.PropertyTags.Parse(s);
                    this.RowCount = ReadUint();
                    List<AddressBookPropertyRow> addressPRList = new List<AddressBookPropertyRow>();
                    for (int i = 0; i < this.RowCount; i++)
                    {
                        AddressBookPropertyRow addressPR = new AddressBookPropertyRow(this.PropertyTags);
                        addressPR.Parse(s);
                        addressPRList.Add(addressPR);
                    }
                    this.RowData = addressPRList.ToArray();
                }
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }
    #endregion 2.2.5.14

    #region 2.2.5.15 ResortRestriction
    /// <summary>
    ///  A class indicates the ResortRestrictionRequest structure.
    /// </summary>
    public class ResortRestrictionRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        public bool HasMinimalIds;

        // An unsigned integer that specifies the number of structures in the MinimalIds field. 
        public uint MinimalIdCount;

        // An array of MinimalEntryID structures that compose a restricted address book container. 
        public MinimalEntryID[] MinimalIds;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionRequest structure.
        /// </summary>
        /// <param name="s">An stream containing ResortRestrictionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.HasMinimalIds = ReadBoolean();
            if (this.HasMinimalIds)
            {
                this.MinimalIdCount = ReadUint();
                List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();
                for (int i = 0; i < this.MinimalIdCount; i++)
                {
                    MinimalEntryID miniEID = new MinimalEntryID();
                    miniEID.Parse(s);
                    miniEIDList.Add(miniEID);
                }
                this.MinimalIds = miniEIDList.ToArray();
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the ResortRestrictionResponse structure.
    /// </summary>
    public class ResortRestrictionResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the MinimalIdCount and MinimalIds fields are present.
        public bool HasMinimalIds;

        // An unsigned integer that specifies the number of structures present in the Minimalids field. 
        public uint MinimalIdCount;

        // An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that compose a restricted address book container. 
        public MinimalEntryID[] MinimalIds;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionResponse structure.
        /// </summary>
        /// <param name="s">An stream containing ResortRestrictionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasState = ReadBoolean();
                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                }
                this.HasMinimalIds = ReadBoolean();
                if (this.HasMinimalIds)
                {
                    this.MinimalIdCount = ReadUint();
                    List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();
                    for (int i = 0; i < this.MinimalIdCount; i++)
                    {
                        MinimalEntryID miniEID = new MinimalEntryID();
                        miniEID.Parse(s);
                        miniEIDList.Add(miniEID);
                    }
                    this.MinimalIds = miniEIDList.ToArray();
                }
                this.AuxiliaryBufferSize = ReadUint();
                if (this.AuxiliaryBufferSize > 0)
                {
                    this.AuxiliaryBuffer = new ExtendedBuffer();
                    this.AuxiliaryBuffer.Parse(s);
                }
            }
        }
    }
    #endregion 2.2.5.15

    #region 2.2.5.16 SeekEntries
    /// <summary>
    ///  A class indicates the SeekEntriesRequest structure.
    /// </summary>
    public class SeekEntriesRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the Target field is present.
        public bool HasTarget;

        // An AddressBookTaggedPropertyValue structure that specifies the property value being sought. 
        public AddressBookTaggedPropertyValue Target;

        // A Boolean value that specifies whether the ExplicitTableCount and ExplicitTable fields are present.
        public bool HasExplicitTable;

        // An unsigned integer that specifies the number of structures present in the ExplicitTable field. 
        public uint ExplicitTableCount;

        // An array of MinimalEntryID structures that constitute an Explicit Table. 
        public MinimalEntryID[] ExplicitTable;

        // A Boolean value that specifies whether the Columns field is present.
        public bool HasColumns;

        // A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the SeekEntriesRequest structure.
        /// </summary>
        /// <param name="s">An stream containing SeekEntriesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.HasTarget = ReadBoolean();
            if (this.HasTarget)
            {
                this.Target = new AddressBookTaggedPropertyValue();
                this.Target.Parse(s);
            }
            this.HasExplicitTable = ReadBoolean();
            if (this.HasExplicitTable)
            {
                this.ExplicitTableCount = ReadUint();
                List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();
                for (int i = 0; i < this.ExplicitTableCount; i++)
                {
                    MinimalEntryID miniEID = new MinimalEntryID();
                    miniEID.Parse(s);
                    miniEIDList.Add(miniEID);
                }
                this.ExplicitTable = miniEIDList.ToArray();
            }
            this.HasColumns = ReadBoolean();
            if (this.HasColumns)
            {
                this.Columns = new LargePropertyTagArray();
                this.Columns.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the SeekEntriesResponse structure.
    /// </summary>
    public class SeekEntriesResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        public bool HasColsAndRows;

        // A LargePropertyTagArray structure that specifies the columns used for the rows returned. 
        public LargePropertyTagArray Columns;

        // An unsigned integer that specifies the number of structures contained in the RowData field. 
        public uint RowCount;

        // An array of AddressBookPropertyRow structures, each of which specifies the row data for the entries queried. 
        public AddressBookPropertyRow[] RowData;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the SeekEntriesResponse structure.
        /// </summary>
        /// <param name="s">An stream containing SeekEntriesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasState = ReadBoolean();
                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                    this.HasColsAndRows = ReadBoolean();
                    if (this.HasColsAndRows)
                    {
                        this.Columns = new LargePropertyTagArray();
                        this.Columns.Parse(s);
                        this.RowCount = ReadUint();
                        List<AddressBookPropertyRow> addressBookPropRow = new List<AddressBookPropertyRow>();
                        for (int i = 0; i < this.RowCount; i++)
                        {
                            AddressBookPropertyRow addressPropRow = new AddressBookPropertyRow(this.Columns);
                            addressPropRow.Parse(s);
                            addressBookPropRow.Add(addressPropRow);
                        }
                        this.RowData = addressBookPropRow.ToArray();
                    }
                }
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }
    #endregion 2.2.5.16

    #region 2.2.5.17 UpdateStat
    /// <summary>
    ///  A class indicates the UpdateStatRequest structure.
    /// </summary>
    public class UpdateStatRequest : BaseStructure
    {
        // Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Reserved;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the client is requesting a value to be returned in the Delta field of the response. 
        public bool DeltaRequested;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the UpdateStatRequest structure.
        /// </summary>
        /// <param name="s">An stream containing UpdateStatRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = ReadUint();
            this.HasState = ReadBoolean();
            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            this.DeltaRequested = ReadBoolean();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the UpdateStatResponse structure.
    /// </summary>
    public class UpdateStatResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A Boolean value that specifies whether the State field is present.
        public bool HasState;

        // A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        public STAT State;

        // A Boolean value that specifies whether the Delta field is present.
        public bool HasDelta;

        // A signed integer that specifies the movement within the address book container that was specified in the State field of the request. 
        public int Delta;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the UpdateStatResponse structure.
        /// </summary>
        /// <param name="s">An stream containing UpdateStatResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.HasState = ReadBoolean();
                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                    this.HasDelta = ReadBoolean();
                    if (this.HasDelta)
                    {
                        this.Delta = ReadINT32();
                    }
                }
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.17

    #region 2.2.5.18 GetMailboxUrl
    /// <summary>
    ///  A class indicates the GetMailboxUrlRequest structure.
    /// </summary>
    public class GetMailboxUrlRequest : BaseStructure
    {
        // Not used. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Flags;

        // A null-terminated Unicode string that specifies the distinguished name (DN) of the mailbox server for which to look up the URL.
        public MAPIString ServerDn;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMailboxUrlRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetMailboxUrlRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = ReadUint();
            this.ServerDn = new MAPIString(Encoding.Unicode);
            this.ServerDn.Parse(s);
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }

    /// <summary>
    ///  A class indicates the GetMailboxUrlResponse structure.
    /// </summary>
    public class GetMailboxUrlResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A null-terminated Unicode string that specifies URL of the EMSMDB server.
        public MAPIString ServerUrl;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMailboxUrlResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetMailboxUrlResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.ServerUrl = new MAPIString(Encoding.Unicode, "\0");
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }

    }
    #endregion 2.2.5.18

    #region 2.2.5.19 GetAddressBookUrl
    /// <summary>
    ///  A class indicates the GetAddressBookUrlRequest structure.
    /// </summary>
    public class GetAddressBookUrlRequest : BaseStructure
    {
        // An unsigned integer that specify the authentication type for the connection.
        public uint Flags;

        // A null-terminated Unicode string that specifies the distinguished name (DN) of the user's mailbox. 
        public MAPIString UserDn;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetAddressBookUrlRequest structure.
        /// </summary>
        /// <param name="s">An stream containing GetAddressBookUrlRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = ReadUint();
            this.UserDn = new MAPIString(Encoding.Unicode);
            this.UserDn.Parse(s);
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }

    /// <summary>
    ///  A class indicates the GetAddressBookUrlResponse structure.
    /// </summary>
    public class GetAddressBookUrlResponse : BaseStructure
    {
        public MAPIString[] MetaTags;

        // A string array that specifies additional header information.
        public MAPIString[] AdditionalHeaders;

        // An unsigned integer that specifies the status of the request. 
        public uint StatusCode;

        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;

        // A null-terminated Unicode string that specifies the URL of the NSPI server.
        public MAPIString ServerUrl;

        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        public uint AuxiliaryBufferSize;

        // An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetAddressBookUrlResponse structure.
        /// </summary>
        /// <param name="s">An stream containing GetAddressBookUrlResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.ServerUrl = new MAPIString(Encoding.Unicode);
                this.ServerUrl.Parse(s);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer();
                this.AuxiliaryBuffer.Parse(s);
            }
        }
    }
    #endregion 2.2.5.19

    #endregion

    #region 2.2.1	Common Data Types

    #region 2.2.1.1	AddressBookPropertyValue Structure
    /// <summary>
    ///  A class indicates the AddressBookPropertyValue structure.
    /// </summary>
    public class AddressBookPropertyValue : BaseStructure
    {
        // An unsigned integer when the PropertyType is known to be either PtypString, PtypString8, PtypBinary or PtypMultiple ([MS-OXCDATA] section 2.11.1). 
        public bool? HasValue;

        // A PropertyValue structure, unless HasValue is present with a value of FALSE (0x00). 
        public object PropertyValue;

        // A propertyDataType is used to initialized the AddressBookPropertyValue structure
        private PropertyDataType propertyDataType;

        // A CountWideEnum is used to initialized the AddressBookPropertyValue structure
        private CountWideEnum countWide;

        /// <summary>
        /// The constructed function for AddressBookPropertyValue.
        /// </summary>
        /// <param name="propertyDataType">The PropertyDataType for this structure</param>
        /// <param name="ptypMultiCountSize">The CountWideEnum for this structure</param>
        public AddressBookPropertyValue(PropertyDataType propertyDataType, CountWideEnum ptypMultiCountSize = CountWideEnum.fourBytes)
        {
            this.propertyDataType = propertyDataType;
            this.countWide = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the AddressBookPropertyValue structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            bool hasValue = (propertyDataType == PropertyDataType.PtypString) || (propertyDataType == PropertyDataType.PtypString8) ||
                            (propertyDataType == PropertyDataType.PtypBinary) || (propertyDataType == PropertyDataType.PtypMultipleInteger16) ||
                            (propertyDataType == PropertyDataType.PtypMultipleInteger32) || (propertyDataType == PropertyDataType.PtypMultipleFloating32) ||
                            (propertyDataType == PropertyDataType.PtypMultipleFloating64) || (propertyDataType == PropertyDataType.PtypMultipleCurrency) ||
                            (propertyDataType == PropertyDataType.PtypMultipleFloatingTime) || (propertyDataType == PropertyDataType.PtypMultipleInteger64) ||
                            (propertyDataType == PropertyDataType.PtypMultipleString) || (propertyDataType == PropertyDataType.PtypMultipleString8) ||
                            (propertyDataType == PropertyDataType.PtypMultipleTime) || (propertyDataType == PropertyDataType.PtypMultipleGuid) ||
                            (propertyDataType == PropertyDataType.PtypMultipleBinary);
            if (hasValue)
            {
                this.HasValue = ReadBoolean();
            }
            else
            {
                this.HasValue = null;
            }

            if ((HasValue == null) || ((HasValue != null) && (HasValue == true)))
            {

                PropertyValue propertyValue = new PropertyValue();
                this.PropertyValue = propertyValue.ReadPropertyValue(this.propertyDataType, s, this.countWide);
            }
        }
    }

    #endregion

    #region 2.2.1.2	AddressBookTaggedPropertyValue Structure
    /// <summary>
    ///  A class indicates the AddressBookTaggedPropertyValue structure.
    /// </summary>
    public class AddressBookTaggedPropertyValue : BaseStructure
    {
        // An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        public PropertyDataType PropertyType;

        // An unsigned integer that identifies the property.
        public ushort PropertyId;

        // An AddressBookPropertyValue structure
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookTaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookTaggedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)ReadUshort();
            this.PropertyId = ReadUshort();
            AddressBookPropertyValue addressBookValue = new AddressBookPropertyValue(this.PropertyType);
            addressBookValue.Parse(s);
            this.PropertyValue = addressBookValue;
        }
    }
    #endregion

    #region 2.2.1.3	AddressBookPropertyValueList Structure
    /// <summary>
    ///  A class indicates the AddressBookPropertyValueList structure.
    /// </summary>
    public class AddressBookPropertyValueList : BaseStructure
    {
        // An unsigned integer that specifies the number of structures contained in the PropertyValues field.
        public uint PropertyValueCount;

        // An array of AddressBookTaggedPropertyValue structures
        public AddressBookTaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the AddressBookPropertyValueList structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookPropertyValueList structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyValueCount = ReadUint();
            List<AddressBookTaggedPropertyValue> tempABTP = new List<AddressBookTaggedPropertyValue>();
            for (int i = 0; i < PropertyValueCount; i++)
            {
                AddressBookTaggedPropertyValue abtp = new AddressBookTaggedPropertyValue();
                abtp.Parse(s);
                tempABTP.Add(abtp);
            }
            this.PropertyValues = tempABTP.ToArray();
        }

    }

    #endregion

    #region 2.2.1.4	AddressBookTypedPropertyValue Structure
    /// <summary>
    ///  A class indicates the AddressBookTypedPropertyValue structure.
    /// </summary>
    public class AddressBookTypedPropertyValue : BaseStructure
    {
        // An unsigned integer that identifies the data type of the property value 
        public PropertyDataType PropertyType;

        // An AddressBookPropertyValue structure
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookTypedPropertyValue structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookTypedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)ReadUshort();
            AddressBookPropertyValue addressBookPropValue = new AddressBookPropertyValue(this.PropertyType);
            addressBookPropValue.Parse(s);
            this.PropertyValue = addressBookPropValue;
        }
    }
    #endregion

    #region 2.2.1.5	AddressBookFlaggedPropertyValue Structure
    /// <summary>
    ///  A class indicates the AddressBookFlaggedPropertyValue structure.
    /// </summary>
    public class AddressBookFlaggedPropertyValue : BaseStructure
    {
        // An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field. 
        public byte Flag;

        // An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless the Flag field is set to 0x1.
        public AddressBookPropertyValue PropertyValue;

        // A PropertyDataType used to initialize the constructed function
        private PropertyDataType propertyDataType;

        /// <summary>
        /// The constructed function for AddressBookFlaggedPropertyValue
        /// </summary>
        /// <param name="propertyDataType">The PropertyDataType parameter for AddressBookFlaggedPropertyValue</param>
        public AddressBookFlaggedPropertyValue(PropertyDataType propertyDataType)
        {
            this.propertyDataType = propertyDataType;
        }

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookFlaggedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flag = ReadByte();
            if (this.Flag != 0x01)
            {
                if (this.Flag == 0x00)
                {
                    AddressBookPropertyValue addressPropValue = new AddressBookPropertyValue(this.propertyDataType);
                    addressPropValue.Parse(s);
                    this.PropertyValue = addressPropValue;
                }
                else if (this.Flag == 0x0A)
                {
                    AddressBookPropertyValue addressPropValueForErrorCode = new AddressBookPropertyValue(PropertyDataType.PtypErrorCode);
                    addressPropValueForErrorCode.Parse(s);
                    this.PropertyValue = addressPropValueForErrorCode;
                }
            }
        }
    }
    #endregion

    #region 2.2.1.6	AddressBookFlaggedPropertyValueWithType Structure
    /// <summary>
    ///  A class indicates the AddressBookFlaggedPropertyValueWithType structure.
    /// </summary>
    public class AddressBookFlaggedPropertyValueWithType : BaseStructure
    {
        // An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        public PropertyDataType PropertyType;

        // An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field. 
        public byte Flag;

        // An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless Flag field is set to 0x01
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValueWithType structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookFlaggedPropertyValueWithType structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)ReadUshort();
            this.Flag = ReadByte();
            if (this.Flag != 0x01)
            {
                if (this.Flag == 0x00)
                {
                    AddressBookPropertyValue addressPropValue = new AddressBookPropertyValue(PropertyType);
                    addressPropValue.Parse(s);
                    this.PropertyValue = addressPropValue;
                }
                else if (this.Flag == 0x0A)
                {
                    AddressBookPropertyValue addressPropValueForErrorCode = new AddressBookPropertyValue(PropertyDataType.PtypErrorCode);
                    addressPropValueForErrorCode.Parse(s);
                    this.PropertyValue = addressPropValueForErrorCode;
                }
            }
        }
    }
    #endregion

    #region 2.2.1.7	AddressBookPropertyRow Structure
    /// <summary>
    ///  A class indicates the AddressBookPropertyRow structure.
    /// </summary>
    public class AddressBookPropertyRow : BaseStructure
    {
        // An unsigned integer that indicates whether all property values are present and without error in the ValueArray field. 
        public byte Flags;

        // An array of variable-sized structures.  
        public object[] ValueArray;

        // The LargePropertyTagArray type used to initialize the constructed function.
        private LargePropertyTagArray largePropTagArray;

        // The ptypMultiCountSize type used to initialize the constructed function.
        private CountWideEnum ptypMultiCountSize;

        /// <summary>
        /// The constructed function for AddressBookPropertyRow
        /// </summary>
        /// <param name="largePropTagArray">The LargePropertyTagArray value</param>
        /// <param name="ptypMultiCountSize">The ptypMultiCountSize value</param>
        public AddressBookPropertyRow(LargePropertyTagArray largePropTagArray, CountWideEnum ptypMultiCountSize = CountWideEnum.fourBytes)
        {
            this.largePropTagArray = largePropTagArray;
            this.ptypMultiCountSize = ptypMultiCountSize;
        }

        /// <summary>
        /// Parse the AddressBookPropertyRow structure.
        /// </summary>
        /// <param name="s">An stream containing AddressBookPropertyRow structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = ReadByte();

            List<object> result = new List<object>();

            if (largePropTagArray is LargePropertyTagArray)
            {
                foreach (var propTag in largePropTagArray.PropertyTags)
                {
                    object addrRowValue = null;
                    if (this.Flags == 0x00)
                    {
                        if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            AddressBookPropertyValue propValue = new AddressBookPropertyValue(propTag.PropertyType, this.ptypMultiCountSize);
                            propValue.Parse(s);
                            addrRowValue = propValue;
                        }
                        else
                        {
                            AddressBookTypedPropertyValue typePropValue = new AddressBookTypedPropertyValue();
                            typePropValue.Parse(s);
                            addrRowValue = typePropValue;
                        }
                    }
                    else if (this.Flags == 0x01)
                    {
                        if (propTag.PropertyType != PropertyDataType.PtypUnspecified)
                        {
                            AddressBookFlaggedPropertyValue flagPropValue = new AddressBookFlaggedPropertyValue(propTag.PropertyType);
                            flagPropValue.Parse(s);
                            addrRowValue = flagPropValue;
                        }
                        else
                        {
                            AddressBookFlaggedPropertyValueWithType flagPropValue = new AddressBookFlaggedPropertyValueWithType();
                            flagPropValue.Parse(s);
                            addrRowValue = flagPropValue;
                        }
                    }

                    result.Add(addrRowValue);
                }
            }

            this.ValueArray = result.ToArray();
        }
    }
    #endregion

    #region 2.2.1.8	LargePropertyTagArray Structure
    /// <summary>
    ///  A class indicates the LargePropertyTagArray structure.
    /// </summary>
    public class LargePropertyTagArray : BaseStructure
    {
        // An unsigned integer that specifies the number of structures contained in the PropertyTags field. 
        public uint PropertyTagCount;

        // An array of PropertyTag structures, each of which contains a property tag that specifies a property.
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the LargePropertyTagArray structure.
        /// </summary>
        /// <param name="s">An stream containing LargePropertyTagArray structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyTagCount = ReadUint();
            List<PropertyTag> tempPT = new List<PropertyTag>();
            for (int i = 0; i < PropertyTagCount; i++)
            {
                PropertyTag p = new PropertyTag();
                p.Parse(s);
                tempPT.Add(p);
            }
            this.PropertyTags = tempPT.ToArray();
        }
    }
    #endregion

    #endregion

    #region Extended Buffer
    /// <summary>
    /// The auxiliary blocks sent from the server to the client in the rgbAuxOut parameter auxiliary buffer on the EcDoConnectEx method. It is defined in section 3.1.4.1.1.1 of MS-OXCRPC.
    /// </summary>
    public class ExtendedBuffer : BaseStructure
    {
        // The RPC_HEADER_EXT structure provides information about the payload.
        public RPC_HEADER_EXT RPC_HEADER_EXT;

        // A structure of bytes that constitute the auxiliary payload data returned from the server. 
        public AuxiliaryBufferPayload[] Payload;

        /// <summary>
        /// Parse the ExtendedBuffer. 
        /// </summary>
        /// <param name="s">An stream of the extended buffers.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RPC_HEADER_EXT = new RPC_HEADER_EXT();
            this.RPC_HEADER_EXT.Parse(s);

            if (this.RPC_HEADER_EXT.Size > 0)
            {
                byte[] payloadBytes = ReadBytes((int)this.RPC_HEADER_EXT.Size);
                bool isCompressedXOR = false;
                if (((ushort)RPC_HEADER_EXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)RPC_HEADER_EXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)RPC_HEADER_EXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (isCompressedXOR)
                {
                    MapiInspector.MAPIInspector.auxPayLoadCompresssedXOR = payloadBytes;
                }
                Stream stream = new MemoryStream(payloadBytes);

                List<AuxiliaryBufferPayload> payload = new List<AuxiliaryBufferPayload>();
                for (int length = 0; length < RPC_HEADER_EXT.Size; )
                {
                    AuxiliaryBufferPayload buffer = new AuxiliaryBufferPayload();
                    buffer.Parse(stream);
                    payload.Add(buffer);
                    length += buffer.AUX_HEADER.Size;
                }
                this.Payload = payload.ToArray();
            }
        }
    }
    #endregion

    #region RPC_HEADER_EXT
    /// <summary>
    /// The RPC_HEADER_EXT structure provides information about the payload. It is defined in section 2.2.2.1 of MS-OXCRPC.
    /// </summary>
    public class RPC_HEADER_EXT : BaseStructure
    {
        // The version of the structure. This value MUST be set to 0x0000.
        public ushort Version;

        // The flags that specify how data that follows this header MUST be interpreted. 
        public RpcHeaderFlags Flags;

        // The total length of the payload data that follows the RPC_HEADER_EXT structure. 
        public ushort Size;

        // The length of the payload data after it has been uncompressed.
        public ushort SizeActual;

        /// <summary>
        /// Parse the RPC_HEADER_EXT. 
        /// </summary>
        /// <param name="s">An stream related to the RPC_HEADER_EXT.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Version = ReadUshort();
            this.Flags = (RpcHeaderFlags)ReadUshort();
            this.Size = ReadUshort();
            this.SizeActual = ReadUshort();
        }
    }

    /// <summary>
    /// The enum flags that specify how data that follows this header MUST be interpreted. It is defined in section 2.2.2.1 of MS-OXCRPC. 
    /// </summary>
    [Flags]
    public enum RpcHeaderFlags : ushort
    {
        //The data that follows the RPC_HEADER_EXT structure is compressed. 
        Compressed = 0x0001,

        //The data following the RPC_HEADER_EXT structure has been obfuscated. 
        XorMagic = 0x0002,

        //No other RPC_HEADER_EXT structure follows the data of the current RPC_HEADER_EXT structure. 
        Last = 0x0004
    }

    #endregion

    #region Auxiliary Buffer Payload
    /// <summary>
    ///  A class indicates the payload data contains auxiliary information. It is defined in section 3.1.4.1.2 of MS-OXCRPC.
    /// </summary>
    public class AuxiliaryBufferPayload : BaseStructure
    {
        // An AUX_HEADER structure that provides information about the auxiliary block structures that follow it. 
        public AUX_HEADER AUX_HEADER;

        // An object that constitute the auxiliary buffer payload data.
        public object AuxiliaryBlock;

        /// <summary>
        /// Parse the auxiliary buffer payload of session.
        /// </summary>
        /// <param name="s">An stream of auxiliary buffer payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AUX_HEADER = new AUX_HEADER();
            this.AUX_HEADER.Parse(s);
            AuxiliaryBlockType_1 type1;
            AuxiliaryBlockType_2 type2;
            if (this.AUX_HEADER.Version == PayloadDataVersion.AUX_VERSION_1)
            {
                type1 = (AuxiliaryBlockType_1)this.AUX_HEADER.Type;
                switch (type1)
                {
                    case AuxiliaryBlockType_1.AUX_TYPE_ENDPOINT_CAPABILITIES:
                        {
                            AUX_ENDPOINT_CAPABILITIES auxiliaryBlock = new AUX_ENDPOINT_CAPABILITIES();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_CLIENT_CONNECTION_INFO:
                        {
                            AUX_CLIENT_CONNECTION_INFO auxiliaryBlock = new AUX_CLIENT_CONNECTION_INFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_PROTOCOL_DEVICE_IDENTIFICATION:
                        {
                            AUX_PROTOCOL_DEVICE_IDENTIFICATION auxiliaryBlock = new AUX_PROTOCOL_DEVICE_IDENTIFICATION();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_SERVER_SESSION_INFO:
                        {
                            AUX_SERVER_SESSION_INFO auxiliaryBlock = new AUX_SERVER_SESSION_INFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_CLIENT_CONTROL:
                        {
                            AUX_CLIENT_CONTROL auxiliaryBlock = new AUX_CLIENT_CONTROL();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_EXORGINFO:
                        {
                            AUX_EXORGINFO auxiliaryBlock = new AUX_EXORGINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_OSVERSIONINFO:
                        {
                            AUX_OSVERSIONINFO auxiliaryBlock = new AUX_OSVERSIONINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_ACCOUNTINFO:
                        {
                            AUX_PERF_ACCOUNTINFO auxiliaryBlock = new AUX_PERF_ACCOUNTINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_DEFGC_SUCCESS:
                        {
                            AUX_PERF_DEFGC_SUCCESS auxiliaryBlock = new AUX_PERF_DEFGC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_DEFMDB_SUCCESS:
                        {
                            AUX_PERF_DEFMDB_SUCCESS auxiliaryBlock = new AUX_PERF_DEFMDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_FAILURE:
                        {
                            AUX_PERF_FAILURE auxiliaryBlock = new AUX_PERF_FAILURE();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS auxiliaryBlock = new AUX_PERF_GC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_BG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS auxiliaryBlock = new AUX_PERF_MDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_CLIENTINFO:
                        {
                            AUX_PERF_CLIENTINFO auxiliaryBlock = new AUX_PERF_CLIENTINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_DEFGC_SUCCESS:
                        {
                            AUX_PERF_DEFGC_SUCCESS auxiliaryBlock = new AUX_PERF_DEFGC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_DEFMDB_SUCCESS:
                        {
                            AUX_PERF_DEFMDB_SUCCESS auxiliaryBlock = new AUX_PERF_DEFMDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FAILURE:
                        {
                            AUX_PERF_FAILURE auxiliaryBlock = new AUX_PERF_FAILURE();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_DEFGC_SUCCESS:
                        {
                            AUX_PERF_DEFGC_SUCCESS auxiliaryBlock = new AUX_PERF_DEFGC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_DEFMDB_SUCCESS:
                        {
                            AUX_PERF_DEFMDB_SUCCESS auxiliaryBlock = new AUX_PERF_DEFMDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_FAILURE:
                        {
                            AUX_PERF_FAILURE auxiliaryBlock = new AUX_PERF_FAILURE();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS auxiliaryBlock = new AUX_PERF_GC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_FG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS auxiliaryBlock = new AUX_PERF_MDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS auxiliaryBlock = new AUX_PERF_GC_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS auxiliaryBlock = new AUX_PERF_MDB_SUCCESS();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_PROCESSINFO:
                        {
                            AUX_PERF_PROCESSINFO auxiliaryBlock = new AUX_PERF_PROCESSINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_REQUESTID:
                        {
                            AUX_PERF_REQUESTID auxiliaryBlock = new AUX_PERF_REQUESTID();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_SERVERINFO:
                        {
                            AUX_PERF_SERVERINFO auxiliaryBlock = new AUX_PERF_SERVERINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_1.AUX_TYPE_PERF_SESSIONINFO:
                        {
                            AUX_PERF_SESSIONINFO auxiliaryBlock = new AUX_PERF_SESSIONINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    default:
                        this.AuxiliaryBlock = ReadBytes((int)this.AUX_HEADER.Size - 4);
                        break;
                }

            }
            else if (this.AUX_HEADER.Version == PayloadDataVersion.AUX_VERSION_2)
            {
                type2 = (AuxiliaryBlockType_2)this.AUX_HEADER.Type;
                switch (type2)
                {
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_FAILURE:
                        {
                            AUX_PERF_FAILURE_V2 auxiliaryBlock = new AUX_PERF_FAILURE_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_GC_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_BG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_MDB_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FAILURE:
                        {
                            AUX_PERF_FAILURE_V2 auxiliaryBlock = new AUX_PERF_FAILURE_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_FAILURE:
                        {
                            AUX_PERF_FAILURE_V2 auxiliaryBlock = new AUX_PERF_FAILURE_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_GC_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_FG_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_MDB_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_GC_SUCCESS:
                        {
                            AUX_PERF_GC_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_GC_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_MDB_SUCCESS:
                        {
                            AUX_PERF_MDB_SUCCESS_V2 auxiliaryBlock = new AUX_PERF_MDB_SUCCESS_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_PROCESSINFO:
                        {
                            AUX_PERF_PROCESSINFO auxiliaryBlock = new AUX_PERF_PROCESSINFO();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    case AuxiliaryBlockType_2.AUX_TYPE_PERF_SESSIONINFO:
                        {
                            AUX_PERF_SESSIONINFO_V2 auxiliaryBlock = new AUX_PERF_SESSIONINFO_V2();
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                    default:
                        this.AuxiliaryBlock = ReadBytes((int)this.AUX_HEADER.Size - 4);
                        break;
                }
            }
            else
            {
                this.AuxiliaryBlock = ReadBytes((int)this.AUX_HEADER.Size - 4);
            }
        }
    }

    # region Section 2.2.2.2	AUX_HEADER Structure

    # region Section 2.2.2.2.1   AUX_PERF_REQUESTID Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_REQUESTID Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_REQUESTID : BaseStructure
    {
        // The session identification number. 
        public ushort SessionID;

        // The request identification number.
        public ushort RequestID;

        /// <summary>
        /// Parse the AUX_PERF_REQUESTID structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_REQUESTID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SessionID = ReadUshort();
            this.RequestID = ReadUshort();
        }
    }
    #endregion

    # region Section 2.2.2.2.2   AUX_PERF_SESSIONINFO Auxiliary Block Structure

    /// <summary>
    ///  A class indicates the AUX_PERF_SESSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO : BaseStructure
    {
        // The session identification number.
        public ushort SessionID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved;

        // The GUID representing the client session to associate with the session identification number in the SessionID field.
        public Guid SessionGuid;

        /// <summary>
        /// Parse the AUX_PERF_SESSIONINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SESSIONINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SessionID = ReadUshort();
            this.Reserved = ReadUshort();
            this.SessionGuid = ReadGuid();
        }
    }
    #endregion

    #region Section 2.2.2.2.3   AUX_PERF_SESSIONINFO_V2 Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_SESSIONINFO_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO_V2 : BaseStructure
    {
        // The session identification number.
        public ushort SessionID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved;

        // The GUID representing the client session to associate with the session identification number in the SessionID field.
        public Guid SessionGuid;

        // The connection identification number.
        public uint ConnectionID;

        /// <summary>
        /// Parse the AUX_PERF_SESSIONINFO_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SESSIONINFO_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SessionID = ReadUshort();
            this.Reserved = ReadUshort();
            this.SessionGuid = ReadGuid();
            this.ConnectionID = ReadUint();
        }
    }
    #endregion

    # region Section 2.2.2.2.4   AUX_PERF_CLIENTINFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_CLIENTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_CLIENTINFO : BaseStructure
    {
        // The speed of client computer's network adapter, in kilobits per second.
        public uint AdapterSpeed;

        // The client-assigned client identification number.
        public ushort ClientID;

        // The offset from the beginning of the AUX_HEADER structure to the MachineName field. 
        public ushort MachineNameOffset;

        // The offset from the beginning of the AUX_HEADER structure to the UserName field. 
        public ushort UserNameOffset;

        // The size of the client IP address referenced by the ClientIPOffset field. 
        public ushort ClientIPSize;

        // The offset from the beginning of the AUX_HEADER structure to the ClientIP field. 
        public ushort ClientIPOffset;

        // The size of the client IP subnet mask referenced by the ClientIPMaskOffset field. 
        public ushort ClientIPMaskSize;

        // The offset from the beginning of the AUX_HEADER structure to the ClientIPMask field. 
        public ushort ClientIPMaskOffset;

        // The offset from the beginning of the AUX_HEADER structure to the AdapterName field. 
        public ushort AdapterNameOffset;

        // The size of the network adapter Media Access Control (MAC) address referenced by the MacAddressOffset field. 
        public ushort MacAddressSize;

        // The offset from the beginning of the AUX_HEADER structure to the MacAddress field. 
        public ushort MacAddressOffset;

        // A flag that shows the mode in which the client is running. 
        public ClientModeFlag ClientMode;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved;

        // A null-terminated Unicode string that contains the client computer name. 
        public MAPIString MachineName;

        // A null-terminated Unicode string that contains the user's account name. 
        public MAPIString UserName;

        // The client's IP address. 
        public byte?[] ClientIP;

        // The client's IP subnet mask. 
        public byte?[] ClientIPMask;

        // A null-terminated Unicode string that contains the client network adapter name.
        public MAPIString AdapterName;

        // The client's network adapter MAC address. 
        public byte?[] MacAddress;

        /// <summary>
        /// Parse the AUX_PERF_CLIENTINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_CLIENTINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AdapterSpeed = ReadUint();
            this.ClientID = ReadUshort();
            this.MachineNameOffset = ReadUshort();
            this.UserNameOffset = ReadUshort();
            this.ClientIPSize = ReadUshort();
            this.ClientIPOffset = ReadUshort();
            this.ClientIPMaskSize = ReadUshort();
            this.ClientIPMaskOffset = ReadUshort();
            this.AdapterNameOffset = ReadUshort();
            this.MacAddressSize = ReadUshort();
            this.MacAddressOffset = ReadUshort();
            this.ClientMode = (ClientModeFlag)ReadUshort();
            this.Reserved = ReadUshort();
            if (this.MachineNameOffset != 0)
            {
                this.MachineName = new MAPIString(Encoding.Unicode);
                this.MachineName.Parse(s);
            }

            if (this.UserNameOffset != 0)
            {
                this.UserName = new MAPIString(Encoding.Unicode);
                this.UserName.Parse(s);
            }

            if (this.ClientIPSize > 0 && this.ClientIPOffset != 0)
            {
                this.ClientIP = ConvertArray(ReadBytes(this.ClientIPSize));
            }

            if (this.ClientIPMaskSize > 0 && this.ClientIPMaskOffset != 0)
            {
                this.ClientIPMask = ConvertArray(ReadBytes(this.ClientIPMaskSize));
            }

            if (this.AdapterNameOffset != 0)
            {
                this.AdapterName = new MAPIString(Encoding.Unicode);
                this.AdapterName.Parse(s);
            }

            if (this.MacAddressSize > 0 && this.MacAddressOffset != 0)
            {
                this.MacAddress = ConvertArray(ReadBytes(this.MacAddressSize));
            }

        }
    }

    /// <summary>
    /// A flag that shows the mode in which the client is running. 
    /// </summary>
    public enum ClientModeFlag : ushort
    {
        CLIENTMODE_UNKNOWN = 0x00,
        CLIENTMODE_CLASSIC = 0x01,
        CLIENTMODE_CACHED = 0x02
    };

    #endregion

    #region  Section 2.2.2.2.5   AUX_PERF_SERVERINFO Auxiliary Block Structure

    /// <summary>
    ///  A class indicates the AUX_PERF_SERVERINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SERVERINFO : BaseStructure
    {
        // The client-assigned server identification number.
        public ushort ServerID;

        // The server type assigned by client. 
        public ServerType ServerType;

        // The offset from the beginning of the AUX_HEADER structure to the ServerDN field. 
        public ushort ServerDNOffset;

        // The offset from the beginning of the AUX_HEADER structure to the ServerName field. 
        public ushort ServerNameOffset;

        // A null-terminated Unicode string that contains the DN of the server. 
        public MAPIString ServerDN;

        // A null-terminated Unicode string that contains the server name. 
        public MAPIString ServerName;

        /// <summary>
        /// Parse the AUX_PERF_SERVERINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SERVERINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ServerID = ReadUshort();
            this.ServerType = (ServerType)ReadUshort();
            this.ServerDNOffset = ReadUshort();
            this.ServerNameOffset = ReadUshort();
            if (this.ServerDNOffset != 0)
            {
                this.ServerDN = new MAPIString(Encoding.Unicode);
                this.ServerDN.Parse(s);
            }
            if (this.ServerNameOffset != 0)
            {
                this.ServerName = new MAPIString(Encoding.Unicode);
                this.ServerName.Parse(s);
            }
        }
    }

    /// <summary>
    /// The server type assigned by client. 
    /// </summary>
    public enum ServerType : ushort
    {
        SERVERTYPE_UNKNOWN = 0x00,
        SERVERTYPE_PRIVATE = 0x01,
        SERVERTYPE_PUBLIC = 0x02,
        SERVERTYPE_DIRECTORY = 0x03,
        SERVERTYPE_REFERRAL = 0x04
    }
    #endregion

    #region Section 2.2.2.2.6   AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_PROCESSINFO : BaseStructure
    {
        // The client-assigned process identification number.
        public ushort ProcessID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved_1;

        // The GUID representing the client process to associate with the process identification number in the ProcessID field.
        public Guid ProcessGuid;

        // The offset from the beginning of the AUX_HEADER structure to the ProcessName field. 
        public ushort ProcessNameOffset;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved_2;

        // A null-terminated Unicode string that contains the client process name. 
        public MAPIString ProcessName;

        /// <summary>
        /// Parse the AUX_PERF_PROCESSINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_PROCESSINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = ReadUshort();
            this.Reserved_1 = ReadUshort();
            this.ProcessGuid = ReadGuid();
            this.ProcessNameOffset = ReadUshort();
            this.Reserved_2 = ReadUshort();
            if (ProcessNameOffset != 0)
            {
                this.ProcessName = new MAPIString(Encoding.Unicode);
                this.ProcessName.Parse(s);
            }
        }
    }
    #endregion

    #region Section 2.2.2.2.7   AUX_PERF_DEFMDB_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_DEFMDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_DEFMDB_SUCCESS : BaseStructure
    {
        // The number of milliseconds since a successful request occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the successful request took to complete.
        public uint TimeToCompleteRequest;

        // The request identification number.
        public ushort RequestID;

        // Padding to enforce alignment of the data on a 4-byte field.
        public ushort Reserved;

        /// <summary>
        /// Parse the AUX_PERF_DEFMDB_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_DEFMDB_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.TimeSinceRequest = ReadUint();
            this.TimeToCompleteRequest = ReadUint();
            this.RequestID = ReadUshort();
            this.Reserved = ReadUshort();
        }
    }

    #endregion

    #region Section 2.2.2.2.8   AUX_PERF_DEFGC_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_DEFGC_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_DEFGC_SUCCESS : BaseStructure
    {
        // The server identification number.
        public ushort ServerID;

        // // The session identification number.
        public ushort SessionID;

        // The number of milliseconds since a successful request occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the successful request took to complete.
        public uint TimeToCompleteRequest;

        // The client-defined operation that was successful.
        public byte RequestOperation;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_DEFGC_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_DEFGC_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToCompleteRequest = ReadUint();
            this.RequestOperation = ReadByte();
            this.Reserved = ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.9   AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_MDB_SUCCESS : BaseStructure
    {
        // The client identification number.
        public ushort ClientID;

        // The server identification number.
        public ushort ServerID;

        // The session identification number.
        public ushort SessionID;

        // The request identification number.
        public ushort RequestID;

        // The number of milliseconds since a successful request occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the successful request took to complete.
        public uint TimeToCompleteRequest;

        /// <summary>
        /// Parse the AUX_PERF_MDB_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_MDB_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = ReadUshort();
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.RequestID = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToCompleteRequest = ReadUint();
        }
    }
    #endregion

    #region Section 2.2.2.2.10   AUX_PERF_MDB_SUCCESS_V2 Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_MDB_SUCCESS_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_MDB_SUCCESS_V2 : BaseStructure
    {
        // The process identification number.
        public ushort ProcessID;

        // The client identification number.
        public ushort ClientID;

        // The server identification number.
        public ushort ServerID;

        // The session identification number.
        public ushort SessionID;

        // The request identification number.
        public ushort RequestID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved;

        // The number of milliseconds since a successful request occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the successful request took to complete.
        public uint TimeToCompleteRequest;

        /// <summary>
        /// Parse the AUX_PERF_MDB_SUCCESS_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_MDB_SUCCESS_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = ReadUshort();
            this.ClientID = ReadUshort();
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.RequestID = ReadUshort();
            this.Reserved = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToCompleteRequest = ReadUint();
        }
    }
    #endregion

    #region Section 2.2.2.2.11   AUX_PERF_GC_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_GC_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_GC_SUCCESS : BaseStructure
    {
        // The client identification number.
        public ushort ClientID;

        // The server identification number.
        public ushort ServerID;

        // The session identification number.
        public ushort SessionID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved_1;

        // The number of milliseconds since a successful request occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the successful request took to complete.
        public uint TimeToCompleteRequest;

        // The client-defined operation that was successful.
        public byte RequestOperation;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public byte[] Reserved_2;

        /// <summary>
        /// Parse the AUX_PERF_GC_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_GC_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = ReadUshort();
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.Reserved_1 = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToCompleteRequest = ReadUint();
            this.RequestOperation = ReadByte();
            this.Reserved_2 = ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.12   AUX_PERF_GC_SUCCESS_V2 Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_GC_SUCCESS_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_GC_SUCCESS_V2 : BaseStructure
    {
        // The process identification number.
        public ushort ProcessID;

        // The client identification number. 
        public ushort ClientID;

        // The server identification number.
        public ushort ServerID;

        //  The session identification number.
        public ushort SessionID;

        // The number of milliseconds since a successful request occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the successful request took to complete.
        public uint TimeToCompleteRequest;

        // The client-defined operation that was successful.
        public byte RequestOperation;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_GC_SUCCESS_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_GC_SUCCESS_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = ReadUshort();
            this.ClientID = ReadUshort();
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToCompleteRequest = ReadUint();
            this.RequestOperation = ReadByte();
            this.Reserved = ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.13   AUX_PERF_FAILURE Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_FAILURE Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE : BaseStructure
    {
        // The client identification number.
        public ushort ClientID;

        // The server identification number.
        public ushort ServerID;

        // The session identification number.
        public ushort SessionID;

        // The request identification number.
        public ushort RequestID;

        // The number of milliseconds since a request failure occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the failed request took to complete.
        public uint TimeToFailRequest;

        // The error code returned for the failed request. 
        public uint ResultCode;

        // The client-defined operation that failed.
        public byte RequestOperation;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_FAILURE structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = ReadUshort();
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.RequestID = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToFailRequest = ReadUint();
            this.ResultCode = ReadUint();
            this.RequestOperation = ReadByte();
            this.Reserved = ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.14   AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE_V2 : BaseStructure
    {
        // The process identification number.
        public ushort ProcessID;

        // The client identification number.
        public ushort ClientID;

        // The server identification number.
        public ushort ServerID;

        // The session identification number.
        public ushort SessionID;

        // The request identification number.
        public ushort RequestID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved_1;

        // The number of milliseconds since a request failure occurred.
        public uint TimeSinceRequest;

        // The number of milliseconds the request failure took to complete.
        public uint TimeToFailRequest;

        // The error code returned for the failed request. 
        public uint ResultCode;

        // The client-defined operation that failed.
        public byte RequestOperation;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public byte[] Reserved_2;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_FAILURE_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = ReadUshort();
            this.ClientID = ReadUshort();
            this.ServerID = ReadUshort();
            this.SessionID = ReadUshort();
            this.RequestID = ReadUshort();
            this.Reserved_1 = ReadUshort();
            this.TimeSinceRequest = ReadUint();
            this.TimeToFailRequest = ReadUint();
            this.ResultCode = ReadUint();
            this.RequestOperation = ReadByte();
            this.Reserved_2 = ReadBytes(3);
        }
    }

    #endregion

    #region Section 2.2.2.2.15   AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONTROL : BaseStructure
    {
        // The flags that instruct the client to either enable or disable behavior. 
        public EnableFlags EnableFlags;

        // The number of milliseconds the client keeps unsent performance data before the data is expired. 
        public uint ExpiryTime;

        /// <summary>
        /// Parse the AUX_CLIENT_CONTROL structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_CLIENT_CONTROL structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.EnableFlags = (EnableFlags)ReadUint();
            this.ExpiryTime = ReadUint();
        }
    }

    public enum EnableFlags : uint
    {
        ENABLE_PERF_SENDTOSERVER = 0x00000001,
        ENABLE_COMPRESSION = 0x00000004,
        ENABLE_HTTP_TUNNELING = 0x00000008,
        ENABLE_PERF_SENDGCDATA = 0x00000010
    };
    #endregion

    #region Section 2.2.2.2.16   AUX_OSVERSIONINFO Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_OSVERSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_OSVERSIONINFO : BaseStructure
    {
        // The size of this AUX_OSVERSIONINFO structure.
        public uint OSVersionInfoSize;

        // The major version number of the operating system of the server.
        public uint MajorVersion;

        // The minor version number of the operating system of the server.
        public uint MinorVersion;

        // The build number of the operating system of the server.
        public uint BuildNumber;

        // Reserved and MUST be ignored when received. 
        public byte[] Reserved1;

        // The major version number of the latest operating system service pack that is installed on the server.
        public ushort ServicePackMajor;

        // The minor version number of the latest operating system service pack that is installed on the server.
        public ushort ServicePackMinor;

        // Reserved and MUST be ignored when received. 
        public uint Reserved2;

        /// <summary>
        /// Parse the AUX_OSVERSIONINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_OSVERSIONINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.OSVersionInfoSize = ReadUint();
            this.MajorVersion = ReadUint();
            this.MinorVersion = ReadUint();
            this.BuildNumber = ReadUint();
            this.Reserved1 = ReadBytes(132);
            this.ServicePackMajor = ReadUshort();
            this.ServicePackMinor = ReadUshort();
            this.Reserved2 = ReadUint();
        }
    }

    #endregion

    #region Section 2.2.2.2.17   AUX_EXORGINFO Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_EXORGINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_EXORGINFO : BaseStructure
    {
        public OrgFlags OrgFlags;

        /// <summary>
        /// Parse the AUX_EXORGINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_EXORGINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.OrgFlags = (OrgFlags)ReadUint();
        }
    }

    /// <summary>
    /// The OrgFlags enum
    /// </summary>
    public enum OrgFlags : uint
    {
        PUBLIC_FOLDERS_ENABLED = 0x00000001,
        USE_AUTODISCOVER_FOR_PUBLIC_FOLDER_CONFIGURATION = 0x0000002
    }

    #endregion

    #region Section 2.2.2.2.18   AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_ACCOUNTINFO : BaseStructure
    {
        // The client-assigned identification number. 
        public ushort ClientID;

        // Padding to enforce alignment of the data on a 4-byte field. 
        public ushort Reserved;

        // A GUID representing the client account information that relates to the client identification number in the ClientID field.
        public Guid Account;

        /// <summary>
        /// Parse the AUX_PERF_ACCOUNTINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_ACCOUNTINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = ReadUshort();
            this.Reserved = ReadUshort();
            this.Account = ReadGuid();
        }
    }

    #endregion

    #region Section 2.2.2.2.19  AUX_ENDPOINT_CAPABILITIES
    /// <summary>
    ///  A class indicates the AUX_ENDPOINT_CAPABILITIES Auxiliary Block Structure
    /// </summary>
    public class AUX_ENDPOINT_CAPABILITIES : BaseStructure
    {
        //A flag that indicates that the server combines capabilities on a single endpoint.
        public EndpointCapabilityFlag EndpointCapabilityFlag;

        /// <summary>
        /// Parse the AUX_ENDPOINT_CAPABILITIES structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_ENDPOINT_CAPABILITIES structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.EndpointCapabilityFlag = (EndpointCapabilityFlag)ReadUint();
        }
    }

    /// <summary>
    /// A flag that indicates that the server combines capabilities on a single endpoint. It is defined in section 2.2.2.2.19 of MS-OXCRPC.
    /// </summary>
    public enum EndpointCapabilityFlag : uint
    {
        ENDPOINT_CAPABILITIES_SINGLE_ENDPOINT = 0x00000001
    }
    #endregion

    #region Section 2.2.2.2.20   AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONNECTION_INFO : BaseStructure
    {
        // The GUID of the connection to the server.
        public Guid ConnectionGUID;

        // The offset from the beginning of the AUX_HEADER structure to the ConnectionContextInfo field.
        public ushort OffsetConnectionContextInfo;

        // Padding to enforce alignment of the data on a 4-byte field.
        public ushort Reserved;

        // The number of connection attempts.
        public uint ConnectionAttempts;

        // A flag designating the mode of operation.
        public ConnectionFlags ConnectionFlags;

        // A null-terminated Unicode string that contains opaque connection context information to be logged by the server.
        public MAPIString ConnectionContextInfo;

        /// <summary>
        /// Parse the AUX_CLIENT_CONNECTION_INFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_ENDPOINT_CAPABILITIES structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ConnectionGUID = ReadGuid();
            this.OffsetConnectionContextInfo = ReadUshort();
            this.Reserved = ReadUshort();
            this.ConnectionAttempts = ReadUint();
            this.ConnectionFlags = (ConnectionFlags)ReadUint();
            if (OffsetConnectionContextInfo != 0)
            {
                this.ConnectionContextInfo = new MAPIString(Encoding.Unicode);
                this.ConnectionContextInfo.Parse(s);
            }
        }
    }

    // ConnectionFlags designating the mode of operation.
    public enum ConnectionFlags : uint
    {
        Clientisrunningincachedmode = 0x0001,
        Clientisnotdesignatingamodeofoperation = 0x0000,
    }
    #endregion

    #region Section 2.2.2.2.21   AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_SERVER_SESSION_INFO : BaseStructure
    {
        // The offset from the beginning of the AUX_HEADER structure to the ServerSessionContextInfo field. 
        public ushort OffsetServerSessionContextInfo;

        // A null-terminated Unicode string that contains opaque server session context information to be logged by the client. 
        public MAPIString ServerSessionContextInfo;

        /// <summary>
        /// Parse the AUX_SERVER_SESSION_INFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_SERVER_SESSION_INFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.OffsetServerSessionContextInfo = ReadUshort();
            if (OffsetServerSessionContextInfo != 0)
            {
                this.ServerSessionContextInfo = new MAPIString(Encoding.Unicode);
                this.ServerSessionContextInfo.Parse(s);
            }
        }
    }
    #endregion

    #region Section 2.2.2.2.22   AUX_PROTOCOL_DEVICE_IDENTIFICATION Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PROTOCOL_DEVICE_IDENTIFICATION Auxiliary Block Structure
    /// </summary>
    public class AUX_PROTOCOL_DEVICE_IDENTIFICATION : BaseStructure
    {
        // The offset from the beginning of the AUX_HEADER structure, as specified in section 2.2.2.2, to the DeviceManufacturer field. 
        public ushort DeviceManufacturerOffset;

        // The offset from the beginning of the AUX_HEADER structure to the DeviceModel field. 
        public ushort DeviceModelOffset;

        // The offset from the beginning of the AUX_HEADER structure to the DeviceSerialNumber field. 
        public ushort DeviceSerialNumberOffset;

        // The offset from the beginning of the AUX_HEADER structure to the DeviceVersion field. 
        public ushort DeviceVersionOffset;

        // The offset from the beginning of the AUX_HEADER structure to the DeviceFirmwareVersion field. 
        public ushort DeviceFirmwareVersionOffset;

        // A null-terminated Unicode string that contains the name of the manufacturer of the device. 
        public MAPIString DeviceManufacturer;

        //  A null-terminated Unicode string that contains the model name of the device. 
        public MAPIString DeviceModel;

        // A null-terminated Unicode string that contains the serial number of the device. 
        public MAPIString DeviceSerialNumber;

        // A null-terminated Unicode string that contains the version number of the device. 
        public MAPIString DeviceVersion;

        // A null-terminated Unicode string that contains the firmware version of the device. 
        public MAPIString DeviceFirmwareVersion;

        /// <summary>
        /// Parse the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.DeviceManufacturerOffset = ReadUshort();
            this.DeviceModelOffset = ReadUshort();
            this.DeviceSerialNumberOffset = ReadUshort();
            this.DeviceVersionOffset = ReadUshort();
            this.DeviceFirmwareVersionOffset = ReadUshort();
            if (this.DeviceManufacturerOffset != 0)
            {
                this.DeviceManufacturer = new MAPIString(Encoding.Unicode);
                this.DeviceManufacturer.Parse(s);
            }

            if (this.DeviceModelOffset != 0)
            {
                this.DeviceModel = new MAPIString(Encoding.Unicode);
                this.DeviceModel.Parse(s);
            }

            if (this.DeviceSerialNumberOffset != 0)
            {
                this.DeviceSerialNumber = new MAPIString(Encoding.Unicode);
                this.DeviceSerialNumber.Parse(s);
            }

            if (this.DeviceVersionOffset != 0)
            {
                this.DeviceVersion = new MAPIString(Encoding.Unicode);
                this.DeviceVersion.Parse(s);
            }

            if (this.DeviceFirmwareVersionOffset != 0)
            {
                this.DeviceFirmwareVersion = new MAPIString(Encoding.Unicode);
                this.DeviceFirmwareVersion.Parse(s);
            }
        }
    }
    #endregion

    #endregion

    /// <summary>
    /// The AUX_HEADER structure provides information about the auxiliary block structures that follow it. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public class AUX_HEADER : BaseStructure
    {
        // The size of the AUX_HEADER structure plus any additional payload data.
        public ushort Size;

        // The version information of the payload data.
        public PayloadDataVersion Version;

        // The type of auxiliary block data structure. The Type should be AuxiliaryBlockType_1 or AuxiliaryBlockType_2.
        public object Type;

        /// <summary>
        /// Parse the AUX_HEADER structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_HEADER structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Size = ReadUshort();
            this.Version = (PayloadDataVersion)ReadByte();
            if (this.Version == PayloadDataVersion.AUX_VERSION_1)
            {
                this.Type = (AuxiliaryBlockType_1)ReadByte();
            }
            else
            {
                this.Type = (AuxiliaryBlockType_2)ReadByte();
            }
        }
    }

    /// <summary>
    /// The version information of the payload data. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum PayloadDataVersion : byte
    {
        AUX_VERSION_1 = 0x01,
        AUX_VERSION_2 = 0x02
    }

    /// <summary>
    /// The enum type corresponding auxiliary block structure that follows the AUX_HEADER structure when the Version field is AUX_VERSION_1. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum AuxiliaryBlockType_1 : byte
    {
        AUX_TYPE_PERF_REQUESTID = 0x01,
        AUX_TYPE_PERF_CLIENTINFO = 0x02,
        AUX_TYPE_PERF_SERVERINFO = 0x03,
        AUX_TYPE_PERF_SESSIONINFO = 0x04,
        AUX_TYPE_PERF_DEFMDB_SUCCESS = 0x05,
        AUX_TYPE_PERF_DEFGC_SUCCESS = 0x06,
        AUX_TYPE_PERF_MDB_SUCCESS = 0x07,
        AUX_TYPE_PERF_GC_SUCCESS = 0x08,
        AUX_TYPE_PERF_FAILURE = 0x09,
        AUX_TYPE_CLIENT_CONTROL = 0x0A,
        AUX_TYPE_PERF_PROCESSINFO = 0x0B,
        AUX_TYPE_PERF_BG_DEFMDB_SUCCESS = 0x0C,
        AUX_TYPE_PERF_BG_DEFGC_SUCCESS = 0x0D,
        AUX_TYPE_PERF_BG_MDB_SUCCESS = 0x0E,
        AUX_TYPE_PERF_BG_GC_SUCCESS = 0x0F,
        AUX_TYPE_PERF_BG_FAILURE = 0x10,
        AUX_TYPE_PERF_FG_DEFMDB_SUCCESS = 0x11,
        AUX_TYPE_PERF_FG_DEFGC_SUCCESS = 0x12,
        AUX_TYPE_PERF_FG_MDB_SUCCESS = 0x13,
        AUX_TYPE_PERF_FG_GC_SUCCESS = 0x14,
        AUX_TYPE_PERF_FG_FAILURE = 0x15,
        AUX_TYPE_OSVERSIONINFO = 0x16,
        AUX_TYPE_EXORGINFO = 0x17,
        AUX_TYPE_PERF_ACCOUNTINFO = 0x18,
        AUX_TYPE_ENDPOINT_CAPABILITIES = 0x48,
        AUX_CLIENT_CONNECTION_INFO = 0x4A,
        AUX_SERVER_SESSION_INFO = 0x4B,
        AUX_PROTOCOL_DEVICE_IDENTIFICATION = 0x4E
    }

    /// <summary>
    /// The enum type corresponding auxiliary block structure that follows the AUX_HEADER structure when the Version field is AUX_VERSION_2. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum AuxiliaryBlockType_2 : byte
    {
        AUX_TYPE_PERF_SESSIONINFO = 0x04,
        AUX_TYPE_PERF_MDB_SUCCESS = 0x07,
        AUX_TYPE_PERF_GC_SUCCESS = 0x08,
        AUX_TYPE_PERF_FAILURE = 0x09,
        AUX_TYPE_PERF_PROCESSINFO = 0x0B,
        AUX_TYPE_PERF_BG_MDB_SUCCESS = 0x0E,
        AUX_TYPE_PERF_BG_GC_SUCCESS = 0x0F,
        AUX_TYPE_PERF_BG_FAILURE = 0x10,
        AUX_TYPE_PERF_FG_MDB_SUCCESS = 0x13,
        AUX_TYPE_PERF_FG_GC_SUCCESS = 0x14,
        AUX_TYPE_PERF_FG_FAILURE = 0x15
    }
    #endregion

    #region rgbIn Input Buffer
    /// <summary>
    /// The rgbInputBuffer contains the ROP request payload. It is defined in section 3.1.4.2.1.1.1 of MS-OXCRPC.
    /// </summary>
    public class rgbInputBuffer : BaseStructure
    {
        // The RPC_HEADER_EXT structure provides information about the payload.
        public RPC_HEADER_EXT RPC_HEADER_EXT;
        // A structure of bytes that constitute the ROP request payload. 
        public ROPInputBuffer Payload;

        /// <summary>
        /// Parse the rgbInputBuffer. 
        /// </summary>
        /// <param name="s">An stream containing the rgbInputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RPC_HEADER_EXT = new RPC_HEADER_EXT();
            this.RPC_HEADER_EXT.Parse(s);
            if (this.RPC_HEADER_EXT.Size > 0)
            {
                byte[] payloadBytes = ReadBytes((int)this.RPC_HEADER_EXT.Size);
                bool isCompressedXOR = false;
                if (((ushort)RPC_HEADER_EXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)RPC_HEADER_EXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)RPC_HEADER_EXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (isCompressedXOR)
                {
                    MapiInspector.MAPIInspector.payLoadCompresssedXOR = payloadBytes;
                }
                Stream stream = new MemoryStream(payloadBytes);
                this.Payload = new ROPInputBuffer();
                this.Payload.Parse(stream);
            }

        }
    }
    #endregion

    #region rgbOut Output Buffer
    /// <summary>
    /// The rgbOutputBuffer contains the ROP request payload. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class rgbOutputBuffer : BaseStructure
    {
        // The RPC_HEADER_EXT structure provides information about the payload.
        public RPC_HEADER_EXT RPC_HEADER_EXT;
        // A structure of bytes that constitute the ROP responses payload. 
        public ROPOutputBuffer Payload;

        /// <summary>
        /// Parse the rgbOutputBuffer. 
        /// </summary>
        /// <param name="s">An stream containing the rgbOutputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RPC_HEADER_EXT = new RPC_HEADER_EXT();
            this.RPC_HEADER_EXT.Parse(s);
            if (this.RPC_HEADER_EXT.Size > 0)
            {
                byte[] payloadBytes = ReadBytes((int)this.RPC_HEADER_EXT.Size);
                bool isCompressedXOR = false;
                if (((ushort)RPC_HEADER_EXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)RPC_HEADER_EXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)RPC_HEADER_EXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (isCompressedXOR)
                {
                    MapiInspector.MAPIInspector.payLoadCompresssedXOR = payloadBytes;
                }
                Stream stream = new MemoryStream(payloadBytes);
                this.Payload = new ROPOutputBuffer();
                this.Payload.Parse(stream);
            }
        }
    }

    /// <summary>
    /// The rgbOutputBufferPack contains multiple rgbOutputBuffer structure. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class rgbOutputBufferPack : BaseStructure
    {
        // An unsigned int indicates the total size of the rgbOutputBuffers, this is a customized value.
        uint RopBufferSize;
        // rgbOutputBuffer packing.
        public rgbOutputBuffer[] rgbOutputBuffers;

        // Initializes a new instance of the rgbOutputBufferPack class.
        public rgbOutputBufferPack(uint RopBufferSize)
        {
            this.RopBufferSize = RopBufferSize;
        }
        /// <summary>
        /// Parse the rgbOutputBufferPack. 
        /// </summary>
        /// <param name="s">An stream containing the rgbOutputBufferPack.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<rgbOutputBuffer> rgbOutputBufferList = new List<rgbOutputBuffer>();
            long StartPosition = s.Position;
            while (s.Position - StartPosition < this.RopBufferSize)
            {
                rgbOutputBuffer buffer = new rgbOutputBuffer();
                buffer.Parse(s);
                rgbOutputBufferList.Add(buffer);
            }

            this.rgbOutputBuffers = rgbOutputBufferList.ToArray();
        }
    }
    #endregion

    #region Parse common message methods
    /// <summary>
    /// Parse the additional headers in Common Response Format
    /// </summary>
    public class ParseMAPIMethod : BaseStructure
    {
        public void ParseAddtionlHeader(Stream s, out List<MAPIString> metaTags, out List<MAPIString> additionalHeaders)
        {
            base.Parse(s);
            string str = null;
            List<MAPIString> tempmetaTags = new List<MAPIString>();
            List<MAPIString> tempadditionalHeaders = new List<MAPIString>();
            while (str != "")
            {
                str = ReadString(Encoding.ASCII, "\r\n");
                MAPIString tempString = new MAPIString(Encoding.ASCII, "\r\n");
                tempString.Value = str;
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        tempmetaTags.Add(tempString);
                        break;
                    default:
                        if (str != "")
                        {
                            tempadditionalHeaders.Add(tempString);
                            break;
                        }
                        else
                        {
                            tempString.Value = "";
                            tempadditionalHeaders.Add(tempString);
                            break;
                        }
                }
            }
            metaTags = tempmetaTags;
            additionalHeaders = tempadditionalHeaders;
        }
    }
    #endregion Parse common message methods

    #region Helper method for compression and obfuscation algorithm.
    /// <summary>
    ///  The DecodingContext is shared between some ROP request and response.
    /// </summary>
    public class CompressionAndObfuscationAlgorithm
    {
        /// <summary>
        /// Obfuscates payload in the stream by applying XOR to each byte of the data with the value 0xA5
        /// </summary>
        /// <param name="data">The bytes to be obfuscated.</param>
        /// <returns>The obfuscated bytes</returns>
        public static byte[] XOR(byte[] data)
        {

            if (data == null)
            {
                throw new ArgumentNullException("inputStream");
            }

            byte[] byteArray = data;
            for (int i = 0; i < data.Length; i++)
            {
                byteArray[i] ^= 0xA5;
            }

            return byteArray;
        }

        /// <summary>
        /// Decodes stream using Direct2 algorithm and decompresses using LZ77 algorithm.
        /// </summary>
        /// <param name="inputStream">The input stream needed to be decompressed.</param>
        /// <param name="actualSize">The expected size of the decompressed output stream.</param>
        /// <returns>Returns the decompressed stream.</returns>
        public static byte[] LZ77Decompress(byte[] inputStream, int actualLength)
        {
            byte? shareByteCache = null;
            int bitMaskIndex = 0;
            uint bitMask = 0x00000000;
            int inputPosition = 0;
            int outputPosition = 0;
            byte[] outputBuffer = new byte[actualLength];

            while (inputPosition < inputStream.Length)
            {
                // If the bitMaskIndex = 0, it represents the entire "bitMask" has been
                // consumed or we are just starting to do the decompress.
                if (bitMaskIndex == 0)
                {
                    bitMask = BitConverter.ToUInt32(inputStream, inputPosition);
                    inputPosition += 4;
                    bitMaskIndex = 32;
                    continue;
                }

                bool hasMetaData = (bitMask & 0x80000000) != 0;
                bitMask = bitMask << 1;
                bitMaskIndex--;

                // If it's data, just copy.
                if (!hasMetaData)
                {
                    outputBuffer[outputPosition] = inputStream[inputPosition];
                    outputPosition++;
                    inputPosition++;
                }
                // Otherwise copy the data specified by metadata (offset, length) pair
                else
                {
                    int offset = 0;
                    int length = 0;
                    GetMetaDataValue(inputStream, ref inputPosition, ref shareByteCache, out offset, out length);
                    while (length != 0)
                    {

                        outputBuffer[outputPosition] = outputBuffer[outputPosition - offset];
                        outputPosition++;
                        length--;
                    }
                }
            }
            return outputBuffer;
        }

        /// <summary>
        /// The function is used to get the metadata from raw request data
        /// </summary>
        /// <param name="encodedBuffer">The raw request data</param>
        /// <param name="decodingPosition">The decoding position for the raw request data</param>
        /// <param name="shareByteCache">The shared bytes stack</param>
        /// <param name="offset">The returned offset value</param>
        /// <param name="length">The returned length value</param>
        public static void GetMetaDataValue(byte[] encodedBuffer, ref int decodingPosition, ref byte? shareByteCache, out int offset, out int length)
        {
            // Initialize: To encode a length between 3 and 9, we use the 3 bits that are "in-line" in the 2-byte metadata.
            ushort inlineMetadata = 0;
            inlineMetadata = BitConverter.ToUInt16(encodedBuffer, decodingPosition);
            decodingPosition += 2;

            offset = inlineMetadata >> 3;
            offset++;
            length = inlineMetadata & 0x0007;

            // Add the minimum match - 3 bytes
            length += 3;

            // Every other time that the length is greater than 9, 
            // an additional byte follows the initial 2-byte metadata
            if (length > 9)
            {
                int additiveLength = 0;
                if (shareByteCache != null)
                {
                    additiveLength = (shareByteCache.Value >> 4) & 0x0f;
                    shareByteCache = null;
                }
                else
                {
                    shareByteCache = encodedBuffer[decodingPosition];
                    decodingPosition++;
                    additiveLength = shareByteCache.Value & 0x0f;
                }

                length += additiveLength;
            }

            // If the length is more than 24, the next byte is also used in the length calculation
            if (length > 24)
            {
                length += encodedBuffer[decodingPosition];
                decodingPosition++;
            }

            // For lengths that are equal to 280 or greater, the length is calculated only 
            // from these last 2 bytes and is not added to the previous length bits.
            if (length > 279)
            {
                length = BitConverter.ToInt16(encodedBuffer, decodingPosition) + 3;
                decodingPosition += 2;
            }
        }
    }
    #endregion
}