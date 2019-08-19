namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The enum flags that specify how data that follows this header MUST be interpreted. It is defined in section 2.2.2.1 of MS-OXCRPC. 
    /// </summary>
    [Flags]
    public enum RpcHeaderFlags : ushort
    {
        /// <summary>
        /// The data that follows the RPC_HEADER_EXT structure is compressed. 
        /// </summary>
        Compressed = 0x0001,

        /// <summary>
        /// The data following the RPC_HEADER_EXT structure has been obfuscated. 
        /// </summary>
        XorMagic = 0x0002,

        /// <summary>
        /// No other RPC_HEADER_EXT structure follows the data of the current RPC_HEADER_EXT structure.
        /// </summary>
        Last = 0x0004
    }

    /// <summary>
    /// A flag that shows the mode in which the client is running. 
    /// </summary>
    public enum ClientModeFlag : ushort
    {
        /// <summary>
        /// CLIENTMODE_UNKNOWN flag
        /// </summary>
        CLIENTMODE_UNKNOWN = 0x00,

        /// <summary>
        /// CLIENTMODE_CLASSIC flag
        /// </summary>
        CLIENTMODE_CLASSIC = 0x01,

        /// <summary>
        /// CLIENTMODE_CACHED flag
        /// </summary>
        CLIENTMODE_CACHED = 0x02
    }

    /// <summary>
    /// The server type assigned by client. 
    /// </summary>
    public enum ServerType : ushort
    {
        /// <summary>
        /// SERVERTYPE_UNKNOWN flag
        /// </summary>
        SERVERTYPE_UNKNOWN = 0x00,

        /// <summary>
        /// SERVERTYPE_PRIVATE flag
        /// </summary>
        SERVERTYPE_PRIVATE = 0x01,

        /// <summary>
        /// SERVERTYPE_PUBLIC flag
        /// </summary>
        SERVERTYPE_PUBLIC = 0x02,

        /// <summary>
        /// SERVERTYPE_DIRECTORY flag
        /// </summary>
        SERVERTYPE_DIRECTORY = 0x03,

        /// <summary>
        /// SERVERTYPE_REFERRAL flag
        /// </summary>
        SERVERTYPE_REFERRAL = 0x04
    }

    /// <summary>
    /// The EnableFlags values
    /// </summary>
    public enum EnableFlags : uint
    {
        /// <summary>
        /// ENABLE_PERF_SENDTOSERVER flag
        /// </summary>
        ENABLE_PERF_SENDTOSERVER = 0x00000001,

        /// <summary>
        /// ENABLE_COMPRESSION flag
        /// </summary>
        ENABLE_COMPRESSION = 0x00000004,

        /// <summary>
        /// ENABLE_HTTP_TUNNELING flag
        /// </summary>
        ENABLE_HTTP_TUNNELING = 0x00000008,

        /// <summary>
        /// ENABLE_PERF_SENDGCDATA flag
        /// </summary>
        ENABLE_PERF_SENDGCDATA = 0x00000010
    }

    /// <summary>
    /// The OrgFlags enum
    /// </summary>
    public enum OrgFlags : uint
    {
        /// <summary>
        /// Public folder enable flag
        /// </summary>
        PUBLIC_FOLDERS_ENABLED = 0x00000001,

        /// <summary>
        /// Use auto-discover for public folder configuration
        /// </summary>
        USE_AUTODISCOVER_FOR_PUBLIC_FOLDER_CONFIGURATION = 0x0000002
    }

    /// <summary>
    /// A flag that indicates that the server combines capabilities on a single endpoint. It is defined in section 2.2.2.2.19 of MS-OXCRPC.
    /// </summary>
    public enum EndpointCapabilityFlag : uint
    {
        /// <summary>
        /// Endpoint capabilities single endpoint
        /// </summary>
        ENDPOINT_CAPABILITIES_SINGLE_ENDPOINT = 0x00000001
    }

    /// <summary>
    /// ConnectionFlags designating the mode of operation.
    /// </summary>
    public enum ConnectionFlags : uint
    {
        /// <summary>
        /// Client running cached mode
        /// </summary>
        Clientisrunningincachedmode = 0x0001,

        /// <summary>
        /// Client is not designating mode of operation
        /// </summary>
        Clientisnotdesignatingamodeofoperation = 0x0000,
    }

    /// <summary>
    /// The version information of the payload data. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum PayloadDataVersion : byte
    {
        /// <summary>
        /// AUX_VERSION_1 flag
        /// </summary>
        AUX_VERSION_1 = 0x01,

        /// <summary>
        /// AUX_VERSION_2 flag
        /// </summary>
        AUX_VERSION_2 = 0x02
    }

    /// <summary>
    /// The enum type corresponding auxiliary block structure that follows the AUX_HEADER structure when the Version field is AUX_VERSION_1. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum AuxiliaryBlockType_1 : byte
    {
        /// <summary>
        /// AUX_TYPE_PERF_REQUESTID type
        /// </summary>
        AUX_TYPE_PERF_REQUESTID = 0x01,

        /// <summary>
        /// AUX_TYPE_PERF_CLIENTINFO type
        /// </summary>
        AUX_TYPE_PERF_CLIENTINFO = 0x02,

        /// <summary>
        /// AUX_TYPE_PERF_SERVERINFO type
        /// </summary>
        AUX_TYPE_PERF_SERVERINFO = 0x03,

        /// <summary>
        /// AUX_TYPE_PERF_SESSIONINFO type
        /// </summary>
        AUX_TYPE_PERF_SESSIONINFO = 0x04,

        /// <summary>
        /// AUX_TYPE_PERF_DEFMDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_DEFMDB_SUCCESS = 0x05,

        /// <summary>
        /// AUX_TYPE_PERF_DEFGC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_DEFGC_SUCCESS = 0x06,

        /// <summary>
        /// AUX_TYPE_PERF_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_MDB_SUCCESS = 0x07,

        /// <summary>
        /// AUX_TYPE_PERF_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_GC_SUCCESS = 0x08,

        /// <summary>
        /// AUX_TYPE_PERF_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_FAILURE = 0x09,

        /// <summary>
        /// AUX_TYPE_CLIENT_CONTROL type
        /// </summary>
        AUX_TYPE_CLIENT_CONTROL = 0x0A,

        /// <summary>
        /// AUX_TYPE_PERF_PROCESSINFO type
        /// </summary>
        AUX_TYPE_PERF_PROCESSINFO = 0x0B,

        /// <summary>
        /// AUX_TYPE_PERF_BG_DEFMDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_DEFMDB_SUCCESS = 0x0C,

        /// <summary>
        /// AUX_TYPE_PERF_BG_DEFGC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_DEFGC_SUCCESS = 0x0D,

        /// <summary>
        ///  AUX_TYPE_PERF_BG_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_MDB_SUCCESS = 0x0E,

        /// <summary>
        /// AUX_TYPE_PERF_BG_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_GC_SUCCESS = 0x0F,

        /// <summary>
        /// AUX_TYPE_PERF_BG_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_BG_FAILURE = 0x10,

        /// <summary>
        /// AUX_TYPE_PERF_FG_DEFMDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_DEFMDB_SUCCESS = 0x11,

        /// <summary>
        /// AUX_TYPE_PERF_FG_DEFGC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_DEFGC_SUCCESS = 0x12,

        /// <summary>
        /// AUX_TYPE_PERF_FG_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_MDB_SUCCESS = 0x13,

        /// <summary>
        /// AUX_TYPE_PERF_FG_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_GC_SUCCESS = 0x14,

        /// <summary>
        /// AUX_TYPE_PERF_FG_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_FG_FAILURE = 0x15,

        /// <summary>
        /// AUX_TYPE_OSVERSIONINFO type
        /// </summary>
        AUX_TYPE_OSVERSIONINFO = 0x16,

        /// <summary>
        /// AUX_TYPE_EXORGINFO type
        /// </summary>
        AUX_TYPE_EXORGINFO = 0x17,

        /// <summary>
        /// AUX_TYPE_PERF_ACCOUNTINFO type
        /// </summary>
        AUX_TYPE_PERF_ACCOUNTINFO = 0x18,

        /// <summary>
        /// AUX_TYPE_ENDPOINT_CAPABILITIES type
        /// </summary>
        AUX_TYPE_ENDPOINT_CAPABILITIES = 0x48,

        /// <summary>
        /// AUX_CLIENT_CONNECTION_INFO type
        /// </summary>
        AUX_CLIENT_CONNECTION_INFO = 0x4A,

        /// <summary>
        /// AUX_SERVER_SESSION_INFO type
        /// </summary>
        AUX_SERVER_SESSION_INFO = 0x4B,

        /// <summary>
        /// AUX_PROTOCOL_DEVICE_IDENTIFICATION type
        /// </summary>
        AUX_PROTOCOL_DEVICE_IDENTIFICATION = 0x4E
    }

    /// <summary>
    /// The enum type corresponding auxiliary block structure that follows the AUX_HEADER structure when the Version field is AUX_VERSION_2. It is defined in section 2.2.2.2 of MS-OXCRPC.
    /// </summary>
    public enum AuxiliaryBlockType_2 : byte
    {
        /// <summary>
        /// AUX_TYPE_PERF_SESSIONINFO type
        /// </summary>
        AUX_TYPE_PERF_SESSIONINFO = 0x04,

        /// <summary>
        /// AUX_TYPE_PERF_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_MDB_SUCCESS = 0x07,

        /// <summary>
        /// AUX_TYPE_PERF_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_GC_SUCCESS = 0x08,

        /// <summary>
        /// AUX_TYPE_PERF_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_FAILURE = 0x09,

        /// <summary>
        /// AUX_TYPE_PERF_PROCESSINFO type
        /// </summary>
        AUX_TYPE_PERF_PROCESSINFO = 0x0B,

        /// <summary>
        /// AUX_TYPE_PERF_BG_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_MDB_SUCCESS = 0x0E,

        /// <summary>
        /// AUX_TYPE_PERF_BG_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_BG_GC_SUCCESS = 0x0F,

        /// <summary>
        /// AUX_TYPE_PERF_BG_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_BG_FAILURE = 0x10,

        /// <summary>
        /// AUX_TYPE_PERF_FG_MDB_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_MDB_SUCCESS = 0x13,

        /// <summary>
        /// AUX_TYPE_PERF_FG_GC_SUCCESS type
        /// </summary>
        AUX_TYPE_PERF_FG_GC_SUCCESS = 0x14,

        /// <summary>
        /// AUX_TYPE_PERF_FG_FAILURE type
        /// </summary>
        AUX_TYPE_PERF_FG_FAILURE = 0x15
    }

    #region 2.2.4 Request Types for Mailbox Server Endpoint
    #region 2.2.4.1 Connect

    /// <summary>
    ///  A class indicates the Connect request type.
    /// </summary>
    public class ConnectRequestBody : BaseStructure
    {
        /// <summary>
        /// A null-terminated ASCII string that specifies the DN of the user who is requesting the connection. 
        /// </summary>
        public MAPIString UserDn;

        /// <summary>
        /// A set of flags that designate the type of connection being requested. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        /// </summary>
        public uint DefaultCodePage;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for sorting. 
        /// </summary>
        public uint LcidSort;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for everything other than sorting. 
        /// </summary>
        public uint LcidString;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.UserDn = new MAPIString(Encoding.ASCII);
            this.UserDn.Parse(s);
            this.Flags = this.ReadUint();
            this.DefaultCodePage = this.ReadUint();
            this.LcidSort = this.ReadUint();
            this.LcidString = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.PollsMax = this.ReadUint();
                this.RetryCount = this.ReadUint();
                this.RetryDelay = this.ReadUint();
                this.DnPrefix = new MAPIString(Encoding.ASCII);
                this.DnPrefix.Parse(s);
                this.DisplayName = new MAPIString(Encoding.Unicode);
                this.DisplayName.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// An unsigned integer that specify to the server how to build the ROP responses in the RopBuffer field of the Execute request type success response body.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        /// </summary>
        public uint RopBufferSize;

        /// <summary>
        /// An structure of bytes that constitute the ROP request payload. 
        /// </summary>
        public RgbInputBuffer RopBuffer;

        /// <summary>
        /// An unsigned integer that specifies the maximum size for the RopBuffer field of the Execute request type success response body.
        /// </summary>
        public uint MaxRopOut;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = this.ReadUint();
            this.RopBufferSize = this.ReadUint();
            this.RopBuffer = new RgbInputBuffer(this.RopBufferSize);
            this.RopBuffer.Parse(s);
            this.MaxRopOut = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.Flags = this.ReadUint();
                this.RopBufferSize = this.ReadUint();
                this.RopBuffer = new RgbOutputBufferPack(this.RopBufferSize);
                this.RopBuffer.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse method
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();

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
        public uint? StatusCode;

        /// <summary>
        /// An unsigned integer that specifies the return status of the operation.
        /// </summary>
        public uint? ErrorCode;

        /// <summary>
        /// An unsigned integer that indicates whether an event is pending on the Session Context. 
        /// </summary>
        public uint? EventPending;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint? AuxiliaryBufferSize;

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();

            if (s.Position < s.Length - 1)
            {
                this.StatusCode = this.ReadUint();

                if (this.StatusCode == 0)
                {
                    this.ErrorCode = this.ReadUint();
                    this.EventPending = this.ReadUint();
                }

                this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public uint Flags;

        /// <summary>
        ///  A Boolean value that specifies whether the State field is present.
        /// </summary>
        public byte HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = this.ReadUint();
            this.HasState = this.ReadByte();

            if (this.HasState != 0)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }
            else
            {
                this.State = null;
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
    public class BindResponse : BaseStructure
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
        /// A GUID that is associated with a specific address book server.
        /// </summary>
        public Guid ServerGuid;

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.ServerGuid = this.ReadGuid();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// The reserved field
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client.
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();
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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field. 
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public byte HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A MinimalEntryID structure ([MS-OXNSPI] section 2.2.9.1) that specifies the Minimal Entry ID of the first object.
        /// </summary>
        public MinimalEntryID MinimalId1;

        /// <summary>
        /// A MinimalEntryID structure that specifies the Minimal Entry ID of the second object.
        /// </summary>
        public MinimalEntryID MinimalId2;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">A stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadByte();

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
            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A signed integer that specifies the result of the comparison. 
        /// </summary>
        public int Result;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the CompareMinIdsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing CompareMinIdsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.Result = this.ReadINT32();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// The reserved field
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the NameCount and NameValues fields are present.
        /// </summary>
        public bool HasNames;

        /// <summary>
        /// An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field. 
        /// </summary>
        public uint? NameCount;

        /// <summary>
        /// An array of null-terminated ASCII strings which are distinguished names (DNs) to be mapped to Minimal Entry IDs. 
        /// </summary>
        public MAPIString[] NameValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data returned from the server. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the DnToMinIdRequest structure.
        /// </summary>
        /// <param name="s">A stream containing DnToMinIdRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasNames = this.ReadBoolean();
            uint count = this.ReadUint();
            List<MAPIString> nameValues = new List<MAPIString>();

            if (count == 0)
            {
                s.Position -= 4;
            }
            else
            {
                this.NameCount = count;

                for (int i = 0; i < this.NameCount; i++)
                {
                    MAPIString mapiString = new MAPIString(Encoding.ASCII);
                    mapiString.Parse(s);
                    nameValues.Add(mapiString);
                }
            }

            this.NameValues = nameValues.ToArray();
            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasMinimalIds = this.ReadBoolean();
                this.MinimalIdCount = this.ReadUint();
                List<MinimalEntryID> lm = new List<MinimalEntryID>();

                for (int i = 0; i < this.MinimalIdCount; i++)
                {
                    MinimalEntryID me = new MinimalEntryID();
                    me.Parse(s);
                    lm.Add(me);
                }

                this.MinimalIds = lm.ToArray();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

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
        public uint? MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that constitute an Explicit Table. 
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint InterfaceOptionFlags;

        /// <summary>
        /// A Boolean value that specifies whether the Filter field is present.
        /// </summary>
        public bool HasFilter;

        /// <summary>
        /// A restriction, as specified in [MS-OXCDATA] section 2.12, that is to be applied to the rows in the address book container. 
        /// </summary>
        public RestrictionType Filter;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyNameGuid and PropertyNameId fields are present.
        /// </summary>
        public bool HasPropertyName;

        /// <summary>
        /// The GUID of the property to be opened. 
        /// </summary>
        public Guid? PropertyNameGuid;

        /// <summary>
        /// A 4-byte value that specifies the ID of the property to be opened. 
        /// </summary>
        public uint? PropertyNameId;

        /// <summary>
        /// An unsigned integer that specifies the number of rows the client is requesting.
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMatchesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetMatchesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                STAT stat = new STAT();
                stat.Parse(s);
                this.State = stat;
            }

            this.HasMinimalIds = this.ReadBoolean();

            if (this.HasMinimalIds)
            {
                this.MinimalIdCount = this.ReadUint();
                List<MinimalEntryID> me = new List<MinimalEntryID>();

                for (int i = 0; i < this.MinimalIdCount; i++)
                {
                    MinimalEntryID minimalEntryId = new MinimalEntryID();
                    minimalEntryId.Parse(s);
                    me.Add(minimalEntryId);
                }

                this.MinimalIds = me.ToArray();
            }

            this.InterfaceOptionFlags = this.ReadUint();
            this.HasFilter = this.ReadBoolean();

            if (this.HasFilter)
            {
                RestrictionType restriction = new RestrictionType(CountWideEnum.fourBytes);
                restriction.Parse(s);
                this.Filter = restriction;
            }

            this.HasPropertyName = this.ReadBoolean();

            if (this.HasPropertyName)
            {
                this.PropertyNameGuid = this.ReadGuid();
                this.PropertyNameId = this.ReadUint();
            }

            this.RowCount = this.ReadUint();
            this.HasColumns = this.ReadBoolean();

            if (this.HasColumns)
            {
                LargePropertyTagArray largePTA = new LargePropertyTagArray();
                largePTA.Parse(s);
                this.Columns = largePTA;
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasState = this.ReadBoolean();

                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                }

                this.HasMinimalIds = this.ReadBoolean();

                if (this.HasMinimalIds)
                {
                    this.MinimalIdCount = this.ReadUint();
                    List<MinimalEntryID> listMinimalEID = new List<MinimalEntryID>();

                    for (int i = 0; i < this.MinimalIdCount; i++)
                    {
                        MinimalEntryID minialEID = new MinimalEntryID();
                        minialEID.Parse(s);
                        listMinimalEID.Add(minialEID);
                    }

                    this.MinimalIds = listMinimalEID.ToArray();
                }

                this.HasColsAndRows = this.ReadBoolean();

                if (this.HasColsAndRows)
                {
                    this.Columns = new LargePropertyTagArray();
                    this.Columns.Parse(s);
                    this.RowCount = this.ReadUint();
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

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A MinimalEntryID structure that specifies the object for which to return properties.
        /// </summary>
        public MinimalEntryID MinimalId;

        /// <summary>
        /// An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropListRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetPropListRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.MinimalId = new MinimalEntryID();
            this.MinimalId.Parse(s);
            this.CodePage = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A Boolean value that specifies whether the PropertyTags field is present.
        /// </summary>
        public bool HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that contains the property tags of properties that have values on the requested object. 
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropListResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetPropListResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasPropertyTags = this.ReadBoolean();

                if (this.HasPropertyTags)
                {
                    this.PropertyTags = new LargePropertyTagArray();
                    this.PropertyTags.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags field is present
        /// </summary>
        public bool HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that contains the property tags of the properties that the client is requesting. 
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetPropsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.HasPropertyTags = this.ReadBoolean();

            if (this.HasPropertyTags)
            {
                this.PropertyTags = new LargePropertyTagArray();
                this.PropertyTags.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// An unsigned integer that specifies the code page that the server used to express string properties. 
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyValues field is present.
        /// </summary>
        public bool HasPropertyValues;

        /// <summary>
        /// An AddressBookPropertyValueList structure (section 2.2.1.3) that contains the values of the properties requested. 
        /// </summary>
        public AddressBookPropertyValueList PropertyValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetPropsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetPropsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.CodePage = this.ReadUint();
                this.HasPropertyValues = this.ReadBoolean();

                if (this.HasPropertyValues)
                {
                    this.PropertyValues = new AddressBookPropertyValueList();
                    this.PropertyValues.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Version field is present.
        /// </summary>
        public bool HasVersion;

        /// <summary>
        /// An unsigned integer that specifies the version number of the address book hierarchy table that the client has. 
        /// </summary>
        public uint Version;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetSpecialTableRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetSpecialTableRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.HasVersion = this.ReadBoolean();

            if (this.HasVersion)
            {
                this.Version = this.ReadUint();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.CodePage = this.ReadUint();
                this.HasVersion = this.ReadBoolean();

                if (this.HasVersion)
                {
                    this.Version = this.ReadUint();
                }

                this.HasRows = this.ReadBoolean();

                if (this.HasRows)
                {
                    this.RowsCount = this.ReadUint();
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

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// An unsigned integer that specifies the display type of the template for which information is requested. 
        /// </summary>
        public uint DisplayType;

        /// <summary>
        /// A Boolean value that specifies whether the TemplateDn field is present.
        /// </summary>
        public bool HasTemplateDn;

        /// <summary>
        /// A null-terminated ASCII string that specifies the DN of the template requested. 
        /// </summary>
        public MAPIString TemplateDn;

        /// <summary>
        /// An unsigned integer that specifies the code page of the template for which information is requested.
        /// </summary>
        public uint CodePage;

        /// <summary>
        /// An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], of the template for which information is requested.
        /// </summary>
        public uint LocaleId;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetTemplateInfoRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetTemplateInfoRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.DisplayType = this.ReadUint();
            this.HasTemplateDn = this.ReadBoolean();

            if (this.HasTemplateDn)
            {
                this.TemplateDn = new MAPIString(Encoding.ASCII);
                this.TemplateDn.Parse(s);
            }

            this.CodePage = this.ReadUint();
            this.LocaleId = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.CodePage = this.ReadUint();
                this.HasRow = this.ReadBoolean();

                if (this.HasRow)
                {
                    this.Row = new AddressBookPropertyValueList();
                    this.Row.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A PropertyTag structure that specifies the property to be modified.
        /// </summary>
        public PropertyTag PropertyTag;

        /// <summary>
        /// A MinimalEntryID structure that specifies the Minimal Entry ID of the address book row to be modified.
        /// </summary>
        public MinimalEntryID MinimalId;

        /// <summary>
        /// A Boolean value that specifies whether the EntryIdCount and EntryIds fields are present.
        /// </summary>
        public bool HasEntryIds;

        /// <summary>
        /// An unsigned integer that specifies the count of structures in the EntryIds field. 
        /// </summary>
        public uint? EntryIdCount;

        /// <summary>
        /// An array of entry IDs, each of which is either an EphemeralEntryID structure or a PermanentEntryID structure. 
        /// </summary>
        public object[] EntryIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModLinkAttRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ModLinkAttRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.PropertyTag = new PropertyTag();
            this.PropertyTag.Parse(s);
            this.MinimalId = new MinimalEntryID();
            this.MinimalId.Parse(s);
            this.HasEntryIds = this.ReadBoolean();

            if (this.HasEntryIds)
            {
                this.EntryIdCount = this.ReadUint();
                List<object> tempObj = new List<object>();

                for (int i = 0; i < this.EntryIdCount; i++)
                {
                    var cb = this.ReadUint(); //See details on MS-OXNSPI  3.1.4.1.15	NspiModLinkAtt (Opnum 14) and 2.2.2.3	Binary_r Structure
                    byte currentByte = this.ReadByte();
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
                    else
                    {
                        uint length = this.ReadUint();
                        byte[] byteleft = this.ReadBytes((int)length);
                    }
                }

                this.EntryIds = tempObj.ToArray();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModLinkAttResponse structure.
        /// </summary>
        /// <param name="s">A stream containing ModLinkAttResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.  
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags field is present.
        /// </summary>
        public bool HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties to be removed. 
        /// </summary>
        public LargePropertyTagArray PropertiesTags;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyValues field is present.
        /// </summary>
        public bool HasPropertyValues;

        /// <summary>
        /// An AddressBookPropertyValueList structure that specifies the values of the properties to be modified. 
        /// </summary>
        public AddressBookPropertyValueList PropertyValues;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModPropsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ModPropsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.HasPropertyTags = this.ReadBoolean();

            if (this.HasPropertyTags)
            {
                this.PropertiesTags = new LargePropertyTagArray();
                this.PropertiesTags.Parse(s);
            }

            this.HasPropertyValues = this.ReadBoolean();

            if (this.HasPropertyValues)
            {
                this.PropertyValues = new AddressBookPropertyValueList();
                this.PropertyValues.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ModPropsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing ModPropsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the ExplicitTable field. 
        /// </summary>
        public uint ExplicitTableCount;

        /// <summary>
        /// An array of MinimalEntryID structures that constitute the Explicit Table.
        /// </summary>
        public MinimalEntryID[] ExplicitTable;

        /// <summary>
        /// An unsigned integer that specifies the number of rows the client is requesting.
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties that the client requires for each row returned. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryRowsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing QueryRowsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.ExplicitTableCount = this.ReadUint();
            List<MinimalEntryID> miniEntryIDlist = new List<MinimalEntryID>();

            for (int i = 0; i < this.ExplicitTableCount; i++)
            {
                MinimalEntryID miniEntryID = new MinimalEntryID();
                miniEntryID.Parse(s);
                miniEntryIDlist.Add(miniEntryID);
            }

            this.ExplicitTable = miniEntryIDlist.ToArray();
            this.RowCount = this.ReadUint();
            this.HasColumns = this.ReadBoolean();

            if (this.HasColumns)
            {
                this.Columns = new LargePropertyTagArray();
                this.Columns.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        /// </summary>
        public bool HasColsAndRows;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the columns for the returned rows. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the number of structures in the RowData field. 
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// An array of AddressBookPropertyRow structures, each of which specifies the row data of the Explicit Table. 
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
        /// Parse the QueryRowsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing QueryRowsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasState = this.ReadBoolean();

                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                }

                this.HasColsAndRows = this.ReadBoolean();

                if (this.HasColsAndRows)
                {
                    this.Columns = new LargePropertyTagArray();
                    this.Columns.Parse(s);
                    this.RowCount = this.ReadUint();
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

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A set of bit flags that specify options to the server. 
        /// </summary>
        public uint MapiFlags;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryColumnsRequest structure.
        /// </summary>
        /// <param name="s">A stream containing QueryColumnsRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.MapiFlags = this.ReadUint();
            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties that exist on the address book. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the QueryColumnsResponse structure.
        /// </summary>
        /// <param name="s">A stream containing QueryColumnsResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasColumns = this.ReadBoolean();

                if (this.HasColumns)
                {
                    this.Columns = new LargePropertyTagArray();
                    this.Columns.Parse(s);
                }
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the PropertyTags field is present.
        /// </summary>
        public bool HasPropertyTags;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the properties that client requires for the rows returned. 
        /// </summary>
        public LargePropertyTagArray PropertyTags;

        /// <summary>
        /// A Boolean value that specifies whether the NameCount and NameValues fields are present.
        /// </summary>
        public bool HasNames;

        /// <summary>
        /// An unsigned integer that specifies the number of null-terminated Unicode strings in the NameValues field. TODO:
        /// </summary>
        public uint NameCount;

        /// <summary>
        /// An array of null-terminated Unicode strings. The number of strings is specified by the NameCount field. 
        /// </summary>
        public WStringArray_r Names;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResolveNamesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ResolveNamesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.HasPropertyTags = this.ReadBoolean();

            if (this.HasPropertyTags)
            {
                this.PropertyTags = new LargePropertyTagArray();
                this.PropertyTags.Parse(s);
            }

            this.HasNames = this.ReadBoolean();

            if (this.HasNames)
            {
                this.Names = new WStringArray_r();
                this.Names.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.CodePage = this.ReadUint();
                this.HasMinimalIds = this.ReadBoolean();

                if (this.HasMinimalIds)
                {
                    this.MinimalIdCount = this.ReadUint();
                    List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                    for (int i = 0; i < this.MinimalIdCount; i++)
                    {
                        MinimalEntryID miniEID = new MinimalEntryID();
                        miniEID.Parse(s);
                        miniEIDList.Add(miniEID);
                    }

                    this.MinimalIds = miniEIDList.ToArray();
                }

                this.HasRowsAndCols = this.ReadBoolean();

                if (this.HasRowsAndCols)
                {
                    this.PropertyTags = new LargePropertyTagArray();
                    this.PropertyTags.Parse(s);
                    this.RowCount = this.ReadUint();
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

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

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
        /// An unsigned integer that specifies the number of structures in the MinimalIds field. 
        /// </summary>
        public uint MinimalIdCount;

        /// <summary>
        /// An array of MinimalEntryID structures that compose a restricted address book container. 
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionRequest structure.
        /// </summary>
        /// <param name="s">A stream containing ResortRestrictionRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.HasMinimalIds = this.ReadBoolean();

            if (this.HasMinimalIds)
            {
                this.MinimalIdCount = this.ReadUint();
                List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                for (int i = 0; i < this.MinimalIdCount; i++)
                {
                    MinimalEntryID miniEID = new MinimalEntryID();
                    miniEID.Parse(s);
                    miniEIDList.Add(miniEID);
                }

                this.MinimalIds = miniEIDList.ToArray();
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// An array of MinimalEntryID structures ([MS-OXNSPI] section 2.2.9.1) that compose a restricted address book container. 
        /// </summary>
        public MinimalEntryID[] MinimalIds;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the ResortRestrictionResponse structure.
        /// </summary>
        /// <param name="s">A stream containing ResortRestrictionResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasState = this.ReadBoolean();

                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                }

                this.HasMinimalIds = this.ReadBoolean();

                if (this.HasMinimalIds)
                {
                    this.MinimalIdCount = this.ReadUint();
                    List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                    for (int i = 0; i < this.MinimalIdCount; i++)
                    {
                        MinimalEntryID miniEID = new MinimalEntryID();
                        miniEID.Parse(s);
                        miniEIDList.Add(miniEID);
                    }

                    this.MinimalIds = miniEIDList.ToArray();
                }

                this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Target field is present.
        /// </summary>
        public bool HasTarget;

        /// <summary>
        /// An AddressBookTaggedPropertyValue structure that specifies the property value being sought. 
        /// </summary>
        public AddressBookTaggedPropertyValue Target;

        /// <summary>
        /// A Boolean value that specifies whether the ExplicitTableCount and ExplicitTable fields are present.
        /// </summary>
        public bool HasExplicitTable;

        /// <summary>
        /// An unsigned integer that specifies the number of structures present in the ExplicitTable field. 
        /// </summary>
        public uint ExplicitTableCount;

        /// <summary>
        /// An array of MinimalEntryID structures that constitute an Explicit Table. 
        /// </summary>
        public MinimalEntryID[] ExplicitTable;

        /// <summary>
        /// A Boolean value that specifies whether the Columns field is present.
        /// </summary>
        public bool HasColumns;

        /// <summary>
        /// A LargePropertyTagArray structure (section 2.2.1.8) that specifies the columns that the client is requesting. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the SeekEntriesRequest structure.
        /// </summary>
        /// <param name="s">A stream containing SeekEntriesRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.HasTarget = this.ReadBoolean();

            if (this.HasTarget)
            {
                this.Target = new AddressBookTaggedPropertyValue();
                this.Target.Parse(s);
            }

            this.HasExplicitTable = this.ReadBoolean();

            if (this.HasExplicitTable)
            {
                this.ExplicitTableCount = this.ReadUint();
                List<MinimalEntryID> miniEIDList = new List<MinimalEntryID>();

                for (int i = 0; i < this.ExplicitTableCount; i++)
                {
                    MinimalEntryID miniEID = new MinimalEntryID();
                    miniEID.Parse(s);
                    miniEIDList.Add(miniEID);
                }

                this.ExplicitTable = miniEIDList.ToArray();
            }

            this.HasColumns = this.ReadBoolean();

            if (this.HasColumns)
            {
                this.Columns = new LargePropertyTagArray();
                this.Columns.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A Boolean value that specifies whether the Columns, RowCount, and RowData fields are present.
        /// </summary>
        public bool HasColsAndRows;

        /// <summary>
        /// A LargePropertyTagArray structure that specifies the columns used for the rows returned. 
        /// </summary>
        public LargePropertyTagArray Columns;

        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the RowData field. 
        /// </summary>
        public uint RowCount;

        /// <summary>
        /// An array of AddressBookPropertyRow structures, each of which specifies the row data for the entries queried. 
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
        /// Parse the SeekEntriesResponse structure.
        /// </summary>
        /// <param name="s">A stream containing SeekEntriesResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasState = this.ReadBoolean();

                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                    this.HasColsAndRows = this.ReadBoolean();

                    if (this.HasColsAndRows)
                    {
                        this.Columns = new LargePropertyTagArray();
                        this.Columns.Parse(s);
                        this.RowCount = this.ReadUint();
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

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Reserved;

        /// <summary>
        /// A Boolean value that specifies whether the State field is present.
        /// </summary>
        public bool HasState;

        /// <summary>
        /// A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container.
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the client is requesting a value to be returned in the Delta field of the response. 
        /// </summary>
        public bool DeltaRequested;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the UpdateStatRequest structure.
        /// </summary>
        /// <param name="s">A stream containing UpdateStatRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Reserved = this.ReadUint();
            this.HasState = this.ReadBoolean();

            if (this.HasState)
            {
                this.State = new STAT();
                this.State.Parse(s);
            }

            this.DeltaRequested = this.ReadBoolean();
            this.AuxiliaryBufferSize = this.ReadUint();

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
        ///  A STAT structure ([MS-OXNSPI] section 2.2.8) that specifies the state of a specific address book container. 
        /// </summary>
        public STAT State;

        /// <summary>
        /// A Boolean value that specifies whether the Delta field is present.
        /// </summary>
        public bool HasDelta;

        /// <summary>
        /// A signed integer that specifies the movement within the address book container that was specified in the State field of the request. 
        /// </summary>
        public int Delta;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the UpdateStatResponse structure.
        /// </summary>
        /// <param name="s">A stream containing UpdateStatResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.HasState = this.ReadBoolean();
                if (this.HasState)
                {
                    this.State = new STAT();
                    this.State.Parse(s);
                    this.HasDelta = this.ReadBoolean();
                    if (this.HasDelta)
                    {
                        this.Delta = this.ReadINT32();
                    }
                }
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// Not used. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A null-terminated Unicode string that specifies the distinguished name (DN) of the mailbox server for which to look up the URL.
        /// </summary>
        public MAPIString ServerDn;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMailboxUrlRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetMailboxUrlRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadUint();
            this.ServerDn = new MAPIString(Encoding.Unicode);
            this.ServerDn.Parse(s);
            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A null-terminated Unicode string that specifies URL of the EMSMDB server.
        /// </summary>
        public MAPIString ServerUrl;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetMailboxUrlResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetMailboxUrlResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.ServerUrl = new MAPIString(Encoding.Unicode, "\0");
                this.ServerUrl.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// An unsigned integer that specify the authentication type for the connection.
        /// </summary>
        public uint Flags;

        /// <summary>
        /// A null-terminated Unicode string that specifies the distinguished name (DN) of the user's mailbox. 
        /// </summary>
        public MAPIString UserDn;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetAddressBookUrlRequest structure.
        /// </summary>
        /// <param name="s">A stream containing GetAddressBookUrlRequest structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = this.ReadUint();
            this.UserDn = new MAPIString(Encoding.Unicode);
            this.UserDn.Parse(s);
            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// A null-terminated Unicode string that specifies the URL of the NSPI server.
        /// </summary>
        public MAPIString ServerUrl;

        /// <summary>
        /// An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.
        /// </summary>
        public uint AuxiliaryBufferSize;

        /// <summary>
        /// An array of bytes that constitute the auxiliary payload data sent from the client. 
        /// </summary>
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the GetAddressBookUrlResponse structure.
        /// </summary>
        /// <param name="s">A stream containing GetAddressBookUrlResponse structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            List<MAPIString> metaTags = new List<MAPIString>();
            List<MAPIString> additionalHeaders = new List<MAPIString>();
            ParseMAPIMethod parseMAPIMethod = new ParseMAPIMethod();
            parseMAPIMethod.ParseAddtionlHeader(s, out metaTags, out additionalHeaders);
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = this.ReadUint();

            if (this.StatusCode == 0)
            {
                this.ErrorCode = this.ReadUint();
                this.ServerUrl = new MAPIString(Encoding.Unicode);
                this.ServerUrl.Parse(s);
            }

            this.AuxiliaryBufferSize = this.ReadUint();

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
        /// <summary>
        /// An unsigned integer when the PropertyType is known to be either PtypString, PtypString8, PtypBinary or PtypMultiple ([MS-OXCDATA] section 2.11.1). 
        /// </summary>
        public bool? HasValue;

        /// <summary>
        /// A PropertyValue structure, unless HasValue is present with a value of FALSE (0x00).
        /// </summary>
        public object PropertyValue;

        /// <summary>
        /// A propertyDataType is used to initialized the AddressBookPropertyValue structure
        /// </summary>
        private PropertyDataType propertyDataType;

        /// <summary>
        /// A CountWideEnum is used to initialized the AddressBookPropertyValue structure
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the AddressBookPropertyValue class.
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
        /// <param name="s">A stream containing AddressBookPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            bool hasValue = (this.propertyDataType == PropertyDataType.PtypString) || (this.propertyDataType == PropertyDataType.PtypString8) ||
                            (this.propertyDataType == PropertyDataType.PtypBinary) || (this.propertyDataType == PropertyDataType.PtypMultipleInteger16) ||
                            (this.propertyDataType == PropertyDataType.PtypMultipleInteger32) || (this.propertyDataType == PropertyDataType.PtypMultipleFloating32) ||
                            (this.propertyDataType == PropertyDataType.PtypMultipleFloating64) || (this.propertyDataType == PropertyDataType.PtypMultipleCurrency) ||
                            (this.propertyDataType == PropertyDataType.PtypMultipleFloatingTime) || (this.propertyDataType == PropertyDataType.PtypMultipleInteger64) ||
                            (this.propertyDataType == PropertyDataType.PtypMultipleString) || (this.propertyDataType == PropertyDataType.PtypMultipleString8) ||
                            (this.propertyDataType == PropertyDataType.PtypMultipleTime) || (this.propertyDataType == PropertyDataType.PtypMultipleGuid) ||
                            (this.propertyDataType == PropertyDataType.PtypMultipleBinary);

            if (hasValue)
            {
                this.HasValue = this.ReadBoolean();
            }
            else
            {
                this.HasValue = null;
            }

            if ((this.HasValue == null) || ((this.HasValue != null) && (this.HasValue == true)))
            {
                PropertyValue propertyValue = new PropertyValue(true);
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
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer that identifies the property.
        /// </summary>
        public ushort PropertyId;

        /// <summary>
        /// An AddressBookPropertyValue structure
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookTaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookTaggedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)this.ReadUshort();
            this.PropertyId = this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the PropertyValues field.
        /// </summary>
        public uint PropertyValueCount;

        /// <summary>
        /// An array of AddressBookTaggedPropertyValue structures
        /// </summary>
        public AddressBookTaggedPropertyValue[] PropertyValues;

        /// <summary>
        /// Parse the AddressBookPropertyValueList structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookPropertyValueList structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyValueCount = this.ReadUint();
            List<AddressBookTaggedPropertyValue> tempABTP = new List<AddressBookTaggedPropertyValue>();

            for (int i = 0; i < this.PropertyValueCount; i++)
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
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value 
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An AddressBookPropertyValue structure
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookTypedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookTypedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)this.ReadUshort();
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
        /// <summary>
        /// An unsigned integer. This value of this flag determines what is conveyed in the PropertyValue field. 
        /// </summary>
        public byte Flag;

        /// <summary>
        /// An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless the Flag field is set to 0x1.
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// A PropertyDataType used to initialize the constructed function
        /// </summary>
        private PropertyDataType propertyDataType;

        /// <summary>
        /// Initializes a new instance of the AddressBookFlaggedPropertyValue class.
        /// </summary>
        /// <param name="propertyDataType">The PropertyDataType parameter for AddressBookFlaggedPropertyValue</param>
        public AddressBookFlaggedPropertyValue(PropertyDataType propertyDataType)
        {
            this.propertyDataType = propertyDataType;
        }

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValue structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookFlaggedPropertyValue structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flag = this.ReadByte();

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
        /// <summary>
        /// An unsigned integer that identifies the data type of the property value ([MS-OXCDATA] section 2.11.1).
        /// </summary>
        public PropertyDataType PropertyType;

        /// <summary>
        /// An unsigned integer. This flag MUST be set one of three possible values: 0x0, 0x1, or 0xA, which determines what is conveyed in the PropertyValue field. 
        /// </summary>
        public byte Flag;

        /// <summary>
        /// An AddressBookPropertyValue structure, as specified in section 2.2.1.1, unless Flag field is set to 0x01
        /// </summary>
        public AddressBookPropertyValue PropertyValue;

        /// <summary>
        /// Parse the AddressBookFlaggedPropertyValueWithType structure.
        /// </summary>
        /// <param name="s">A stream containing AddressBookFlaggedPropertyValueWithType structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyType = (PropertyDataType)this.ReadUshort();
            this.Flag = this.ReadByte();

            if (this.Flag != 0x01)
            {
                if (this.Flag == 0x00)
                {
                    AddressBookPropertyValue addressPropValue = new AddressBookPropertyValue(this.PropertyType);
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
        /// <summary>
        /// An unsigned integer that indicates whether all property values are present and without error in the ValueArray field. 
        /// </summary>
        public byte Flags;

        /// <summary>
        /// An array of variable-sized structures.  
        /// </summary>
        public object[] ValueArray;

        /// <summary>
        /// The LargePropertyTagArray type used to initialize the constructed function.
        /// </summary>
        private LargePropertyTagArray largePropTagArray;

        /// <summary>
        /// The ptypMultiCountSize type used to initialize the constructed function.
        /// </summary>
        private CountWideEnum ptypMultiCountSize;

        /// <summary>
        /// Initializes a new instance of the AddressBookPropertyRow class.
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
        /// <param name="s">A stream containing AddressBookPropertyRow structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Flags = this.ReadByte();
            List<object> result = new List<object>();

            if (this.largePropTagArray is LargePropertyTagArray)
            {
                foreach (var propTag in this.largePropTagArray.PropertyTags)
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
        /// <summary>
        /// An unsigned integer that specifies the number of structures contained in the PropertyTags field. 
        /// </summary>
        public uint PropertyTagCount;

        /// <summary>
        /// An array of PropertyTag structures, each of which contains a property tag that specifies a property.
        /// </summary>
        public PropertyTag[] PropertyTags;

        /// <summary>
        /// Parse the LargePropertyTagArray structure.
        /// </summary>
        /// <param name="s">A stream containing LargePropertyTagArray structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.PropertyTagCount = this.ReadUint();
            List<PropertyTag> tempPT = new List<PropertyTag>();

            for (int i = 0; i < this.PropertyTagCount; i++)
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
        /// <summary>
        /// The RPC_HEADER_EXT structure provides information about the payload.
        /// </summary>
        public RPC_HEADER_EXT RPCHEADEREXT;

        /// <summary>
        /// A structure of bytes that constitute the auxiliary payload data returned from the server. 
        /// </summary>
        public AuxiliaryBufferPayload[] Payload;

        /// <summary>
        /// Parse the ExtendedBuffer. 
        /// </summary>
        /// <param name="s">A stream of the extended buffers.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RPCHEADEREXT = new RPC_HEADER_EXT();
            this.RPCHEADEREXT.Parse(s);

            if (this.RPCHEADEREXT.Size > 0)
            {
                byte[] payloadBytes = this.ReadBytes((int)this.RPCHEADEREXT.Size);
                bool isCompressedXOR = false;

                if (((ushort)this.RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)this.RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)this.RPCHEADEREXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (isCompressedXOR)
                {
                    MapiInspector.MAPIInspector.AuxPayLoadCompressedXOR = payloadBytes;
                }

                Stream stream = new MemoryStream(payloadBytes);
                List<AuxiliaryBufferPayload> payload = new List<AuxiliaryBufferPayload>();

                for (int length = 0; length < this.RPCHEADEREXT.Size;)
                {
                    AuxiliaryBufferPayload buffer = new AuxiliaryBufferPayload();
                    buffer.Parse(stream);
                    payload.Add(buffer);
                    length += buffer.AUXHEADER.Size;
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
        /// <summary>
        /// The version of the structure. This value MUST be set to 0x0000.
        /// </summary>
        public ushort Version;

        /// <summary>
        /// The flags that specify how data that follows this header MUST be interpreted. 
        /// </summary>
        public RpcHeaderFlags Flags;

        /// <summary>
        /// The total length of the payload data that follows the RPC_HEADER_EXT structure. 
        /// </summary>
        public ushort Size;

        /// <summary>
        /// The length of the payload data after it has been uncompressed.
        /// </summary>
        public ushort SizeActual;

        /// <summary>
        /// Parse the RPC_HEADER_EXT. 
        /// </summary>
        /// <param name="s">A stream related to the RPC_HEADER_EXT.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Version = this.ReadUshort();
            this.Flags = (RpcHeaderFlags)this.ReadUshort();
            this.Size = this.ReadUshort();
            this.SizeActual = this.ReadUshort();
        }
    }

    #endregion

    #region Auxiliary Buffer Payload
    /// <summary>
    ///  A class indicates the payload data contains auxiliary information. It is defined in section 3.1.4.1.2 of MS-OXCRPC.
    /// </summary>
    public class AuxiliaryBufferPayload : BaseStructure
    {
        /// <summary>
        /// An AUX_HEADER structure that provides information about the auxiliary block structures that follow it. 
        /// </summary>
        public AUX_HEADER AUXHEADER;

        /// <summary>
        /// An object that constitute the auxiliary buffer payload data.
        /// </summary>
        public object AuxiliaryBlock;

        /// <summary>
        /// Parse the auxiliary buffer payload of session.
        /// </summary>
        /// <param name="s">A stream of auxiliary buffer payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AUXHEADER = new AUX_HEADER();
            this.AUXHEADER.Parse(s);
            AuxiliaryBlockType_1 type1;
            AuxiliaryBlockType_2 type2;

            if (this.AUXHEADER.Version == PayloadDataVersion.AUX_VERSION_1)
            {
                type1 = (AuxiliaryBlockType_1)this.AUXHEADER.Type;

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
                        {
                            AnnotatedBytes auxiliaryBlock = new AnnotatedBytes(this.AUXHEADER.Size - 4);
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                }
            }
            else if (this.AUXHEADER.Version == PayloadDataVersion.AUX_VERSION_2)
            {
                type2 = (AuxiliaryBlockType_2)this.AUXHEADER.Type;
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
                        {
                            AnnotatedBytes auxiliaryBlock = new AnnotatedBytes(this.AUXHEADER.Size - 4);
                            auxiliaryBlock.Parse(s);
                            this.AuxiliaryBlock = auxiliaryBlock;
                            break;
                        }
                }
            }
            else
            {
                AnnotatedBytes auxiliaryBlock = new AnnotatedBytes(this.AUXHEADER.Size - 4);
                auxiliaryBlock.Parse(s);
                this.AuxiliaryBlock = auxiliaryBlock;
            }
        }
    }

    #region Section 2.2.2.2	AUX_HEADER Structure

    #region Section 2.2.2.2.1   AUX_PERF_REQUESTID Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_REQUESTID Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_REQUESTID : BaseStructure
    {
        /// <summary>
        /// The session identification number. 
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// Parse the AUX_PERF_REQUESTID structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_REQUESTID structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SessionID = this.ReadUshort();
            this.RequestID = this.ReadUshort();
        }
    }
    #endregion

    #region Section 2.2.2.2.2   AUX_PERF_SESSIONINFO Auxiliary Block Structure

    /// <summary>
    ///  A class indicates the AUX_PERF_SESSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO : BaseStructure
    {
        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The GUID representing the client session to associate with the session identification number in the SessionID field.
        /// </summary>
        public Guid SessionGuid;

        /// <summary>
        /// Parse the AUX_PERF_SESSIONINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SESSIONINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SessionID = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.SessionGuid = this.ReadGuid();
        }
    }
    #endregion

    #region Section 2.2.2.2.3   AUX_PERF_SESSIONINFO_V2 Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_SESSIONINFO_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SESSIONINFO_V2 : BaseStructure
    {
        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The GUID representing the client session to associate with the session identification number in the SessionID field.
        /// </summary>
        public Guid SessionGuid;

        /// <summary>
        /// The connection identification number.
        /// </summary>
        public uint ConnectionID;

        /// <summary>
        /// Parse the AUX_PERF_SESSIONINFO_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SESSIONINFO_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.SessionID = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.SessionGuid = this.ReadGuid();
            this.ConnectionID = this.ReadUint();
        }
    }
    #endregion

    #region Section 2.2.2.2.4   AUX_PERF_CLIENTINFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_CLIENTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_CLIENTINFO : BaseStructure
    {
        /// <summary>
        /// The speed of client computer's network adapter, in kilobits per second.
        /// </summary>
        public uint AdapterSpeed;

        /// <summary>
        /// The client-assigned client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the MachineName field. 
        /// </summary>
        public ushort MachineNameOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the UserName field. 
        /// </summary>
        public ushort UserNameOffset;

        /// <summary>
        /// The size of the client IP address referenced by the ClientIPOffset field. 
        /// </summary>
        public ushort ClientIPSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ClientIP field. 
        /// </summary>
        public ushort ClientIPOffset;

        /// <summary>
        /// The size of the client IP subnet mask referenced by the ClientIPMaskOffset field. 
        /// </summary>
        public ushort ClientIPMaskSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ClientIPMask field. 
        /// </summary>
        public ushort ClientIPMaskOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the AdapterName field. 
        /// </summary>
        public ushort AdapterNameOffset;

        /// <summary>
        /// The size of the network adapter Media Access Control (MAC) address referenced by the MacAddressOffset field. 
        /// </summary>
        public ushort MacAddressSize;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the MacAddress field. 
        /// </summary>
        public ushort MacAddressOffset;

        /// <summary>
        /// A flag that shows the mode in which the client is running. 
        /// </summary>
        public ClientModeFlag ClientMode;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A null-terminated Unicode string that contains the client computer name. 
        /// </summary>
        public MAPIString MachineName;

        /// <summary>
        /// A null-terminated Unicode string that contains the user's account name. 
        /// </summary>
        public MAPIString UserName;

        /// <summary>
        /// The client's IP address. 
        /// </summary>
        public byte?[] ClientIP;

        /// <summary>
        /// The client's IP subnet mask. 
        /// </summary>
        public byte?[] ClientIPMask;

        /// <summary>
        /// A null-terminated Unicode string that contains the client network adapter name.
        /// </summary>
        public MAPIString AdapterName;

        /// <summary>
        /// The client's network adapter MAC address. 
        /// </summary>
        public byte?[] MacAddress;

        /// <summary>
        /// Parse the AUX_PERF_CLIENTINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_CLIENTINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.AdapterSpeed = this.ReadUint();
            this.ClientID = this.ReadUshort();
            this.MachineNameOffset = this.ReadUshort();
            this.UserNameOffset = this.ReadUshort();
            this.ClientIPSize = this.ReadUshort();
            this.ClientIPOffset = this.ReadUshort();
            this.ClientIPMaskSize = this.ReadUshort();
            this.ClientIPMaskOffset = this.ReadUshort();
            this.AdapterNameOffset = this.ReadUshort();
            this.MacAddressSize = this.ReadUshort();
            this.MacAddressOffset = this.ReadUshort();
            this.ClientMode = (ClientModeFlag)this.ReadUshort();
            this.Reserved = this.ReadUshort();

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
                this.ClientIP = this.ConvertArray(this.ReadBytes(this.ClientIPSize));
            }

            if (this.ClientIPMaskSize > 0 && this.ClientIPMaskOffset != 0)
            {
                this.ClientIPMask = this.ConvertArray(this.ReadBytes(this.ClientIPMaskSize));
            }

            if (this.AdapterNameOffset != 0)
            {
                this.AdapterName = new MAPIString(Encoding.Unicode);
                this.AdapterName.Parse(s);
            }

            if (this.MacAddressSize > 0 && this.MacAddressOffset != 0)
            {
                this.MacAddress = this.ConvertArray(this.ReadBytes(this.MacAddressSize));
            }
        }
    }

    #endregion

    #region  Section 2.2.2.2.5   AUX_PERF_SERVERINFO Auxiliary Block Structure

    /// <summary>
    ///  A class indicates the AUX_PERF_SERVERINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_SERVERINFO : BaseStructure
    {
        /// <summary>
        /// The client-assigned server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The server type assigned by client. 
        /// </summary>
        public ServerType ServerType;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerDN field. 
        /// </summary>
        public ushort ServerDNOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerName field. 
        /// </summary>
        public ushort ServerNameOffset;

        /// <summary>
        /// A null-terminated Unicode string that contains the DN of the server. 
        /// </summary>
        public MAPIString ServerDN;

        /// <summary>
        /// A null-terminated Unicode string that contains the server name. 
        /// </summary>
        public MAPIString ServerName;

        /// <summary>
        /// Parse the AUX_PERF_SERVERINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_SERVERINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ServerID = this.ReadUshort();
            this.ServerType = (ServerType)this.ReadUshort();
            this.ServerDNOffset = this.ReadUshort();
            this.ServerNameOffset = this.ReadUshort();

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

    #endregion

    #region Section 2.2.2.2.6   AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_PROCESSINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_PROCESSINFO : BaseStructure
    {
        /// <summary>
        /// The client-assigned process identification number.
        /// </summary>
        public ushort ProcessID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The GUID representing the client process to associate with the process identification number in the ProcessID field.
        /// </summary>
        public Guid ProcessGuid;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ProcessName field. 
        /// </summary>
        public ushort ProcessNameOffset;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved2;

        /// <summary>
        /// A null-terminated Unicode string that contains the client process name. 
        /// </summary>
        public MAPIString ProcessName;

        /// <summary>
        /// Parse the AUX_PERF_PROCESSINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_PROCESSINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = this.ReadUshort();
            this.Reserved1 = this.ReadUshort();
            this.ProcessGuid = this.ReadGuid();
            this.ProcessNameOffset = this.ReadUshort();
            this.Reserved2 = this.ReadUshort();

            if (this.ProcessNameOffset != 0)
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
        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// Parse the AUX_PERF_DEFMDB_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_DEFMDB_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToCompleteRequest = this.ReadUint();
            this.RequestID = this.ReadUshort();
            this.Reserved = this.ReadUshort();
        }
    }

    #endregion

    #region Section 2.2.2.2.8   AUX_PERF_DEFGC_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_DEFGC_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_DEFGC_SUCCESS : BaseStructure
    {
        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// The client-defined operation that was successful.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_DEFGC_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_DEFGC_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToCompleteRequest = this.ReadUint();
            this.RequestOperation = this.ReadByte();
            this.Reserved = this.ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.9   AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_MDB_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_MDB_SUCCESS : BaseStructure
    {
        /// <summary>
        /// The client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// Parse the AUX_PERF_MDB_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_MDB_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = this.ReadUshort();
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.RequestID = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToCompleteRequest = this.ReadUint();
        }
    }
    #endregion

    #region Section 2.2.2.2.10   AUX_PERF_MDB_SUCCESS_V2 Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_MDB_SUCCESS_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_MDB_SUCCESS_V2 : BaseStructure
    {
        /// <summary>
        /// The process identification number.
        /// </summary>
        public ushort ProcessID;

        /// <summary>
        /// The client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// Parse the AUX_PERF_MDB_SUCCESS_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_MDB_SUCCESS_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = this.ReadUshort();
            this.ClientID = this.ReadUshort();
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.RequestID = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToCompleteRequest = this.ReadUint();
        }
    }
    #endregion

    #region Section 2.2.2.2.11   AUX_PERF_GC_SUCCESS Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_GC_SUCCESS Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_GC_SUCCESS : BaseStructure
    {
        /// <summary>
        /// The client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// The client-defined operation that was successful.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved2;

        /// <summary>
        /// Parse the AUX_PERF_GC_SUCCESS structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_GC_SUCCESS structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = this.ReadUshort();
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.Reserved1 = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToCompleteRequest = this.ReadUint();
            this.RequestOperation = this.ReadByte();
            this.Reserved2 = this.ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.12   AUX_PERF_GC_SUCCESS_V2 Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_GC_SUCCESS_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_GC_SUCCESS_V2 : BaseStructure
    {
        /// <summary>
        /// The process identification number.
        /// </summary>
        public ushort ProcessID;

        /// <summary>
        /// The client identification number. 
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The number of milliseconds since a successful request occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the successful request took to complete.
        /// </summary>
        public uint TimeToCompleteRequest;

        /// <summary>
        /// The client-defined operation that was successful.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_GC_SUCCESS_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_GC_SUCCESS_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = this.ReadUshort();
            this.ClientID = this.ReadUshort();
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToCompleteRequest = this.ReadUint();
            this.RequestOperation = this.ReadByte();
            this.Reserved = this.ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.13   AUX_PERF_FAILURE Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_PERF_FAILURE Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE : BaseStructure
    {
        /// <summary>
        /// The client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// The number of milliseconds since a request failure occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the failed request took to complete.
        /// </summary>
        public uint TimeToFailRequest;

        /// <summary>
        /// The error code returned for the failed request. 
        /// </summary>
        public uint ResultCode;

        /// <summary>
        /// The client-defined operation that failed.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_FAILURE structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = this.ReadUshort();
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.RequestID = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToFailRequest = this.ReadUint();
            this.ResultCode = this.ReadUint();
            this.RequestOperation = this.ReadByte();
            this.Reserved = this.ReadBytes(3);
        }
    }
    #endregion

    #region Section 2.2.2.2.14   AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_PERF_FAILURE_V2 Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_FAILURE_V2 : BaseStructure
    {
        /// <summary>
        /// The process identification number.
        /// </summary>
        public ushort ProcessID;

        /// <summary>
        /// The client identification number.
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// The server identification number.
        /// </summary>
        public ushort ServerID;

        /// <summary>
        /// The session identification number.
        /// </summary>
        public ushort SessionID;

        /// <summary>
        /// The request identification number.
        /// </summary>
        public ushort RequestID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved1;

        /// <summary>
        /// The number of milliseconds since a request failure occurred.
        /// </summary>
        public uint TimeSinceRequest;

        /// <summary>
        /// The number of milliseconds the request failure took to complete.
        /// </summary>
        public uint TimeToFailRequest;

        /// <summary>
        /// The error code returned for the failed request. 
        /// </summary>
        public uint ResultCode;

        /// <summary>
        /// The client-defined operation that failed.
        /// </summary>
        public byte RequestOperation;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public byte[] Reserved2;

        /// <summary>
        /// Parse the AUX_PERF_FAILURE_V2 structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_FAILURE_V2 structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ProcessID = this.ReadUshort();
            this.ClientID = this.ReadUshort();
            this.ServerID = this.ReadUshort();
            this.SessionID = this.ReadUshort();
            this.RequestID = this.ReadUshort();
            this.Reserved1 = this.ReadUshort();
            this.TimeSinceRequest = this.ReadUint();
            this.TimeToFailRequest = this.ReadUint();
            this.ResultCode = this.ReadUint();
            this.RequestOperation = this.ReadByte();
            this.Reserved2 = this.ReadBytes(3);
        }
    }

    #endregion

    #region Section 2.2.2.2.15   AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_CLIENT_CONTROL Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONTROL : BaseStructure
    {
        /// <summary>
        /// The flags that instruct the client to either enable or disable behavior. 
        /// </summary>
        public EnableFlags EnableFlags;

        /// <summary>
        /// The number of milliseconds the client keeps unsent performance data before the data is expired. 
        /// </summary>
        public uint ExpiryTime;

        /// <summary>
        /// Parse the AUX_CLIENT_CONTROL structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_CLIENT_CONTROL structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.EnableFlags = (EnableFlags)this.ReadUint();
            this.ExpiryTime = this.ReadUint();
        }
    }

    #endregion

    #region Section 2.2.2.2.16   AUX_OSVERSIONINFO Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_OSVERSIONINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_OSVERSIONINFO : BaseStructure
    {
        /// <summary>
        /// The size of this AUX_OSVERSIONINFO structure.
        /// </summary>
        public uint OSVersionInfoSize;

        /// <summary>
        /// The major version number of the operating system of the server.
        /// </summary>
        public uint MajorVersion;

        /// <summary>
        /// The minor version number of the operating system of the server.
        /// </summary>
        public uint MinorVersion;

        /// <summary>
        /// The build number of the operating system of the server.
        /// </summary>
        public uint BuildNumber;

        /// <summary>
        /// Reserved and MUST be ignored when received. 
        /// </summary>
        public byte[] Reserved1;

        /// <summary>
        /// The major version number of the latest operating system service pack that is installed on the server.
        /// </summary>
        public ushort ServicePackMajor;

        /// <summary>
        /// The minor version number of the latest operating system service pack that is installed on the server.
        /// </summary>
        public ushort ServicePackMinor;

        /// <summary>
        /// Reserved and MUST be ignored when received. 
        /// </summary>
        public uint Reserved2;

        /// <summary>
        /// Parse the AUX_OSVERSIONINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_OSVERSIONINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.OSVersionInfoSize = this.ReadUint();
            this.MajorVersion = this.ReadUint();
            this.MinorVersion = this.ReadUint();
            this.BuildNumber = this.ReadUint();
            this.Reserved1 = this.ReadBytes(132);
            this.ServicePackMajor = this.ReadUshort();
            this.ServicePackMinor = this.ReadUshort();
            this.Reserved2 = this.ReadUint();
        }
    }

    #endregion

    #region Section 2.2.2.2.17   AUX_EXORGINFO Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_EXORGINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_EXORGINFO : BaseStructure
    {
        /// <summary>
        /// The OrgFlags
        /// </summary>
        public OrgFlags OrgFlags;

        /// <summary>
        /// Parse the AUX_EXORGINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_EXORGINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.OrgFlags = (OrgFlags)this.ReadUint();
        }
    }

    #endregion

    #region Section 2.2.2.2.18   AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// <summary>
    /// A class indicates the AUX_PERF_ACCOUNTINFO Auxiliary Block Structure
    /// </summary>
    public class AUX_PERF_ACCOUNTINFO : BaseStructure
    {
        /// <summary>
        /// The client-assigned identification number. 
        /// </summary>
        public ushort ClientID;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field. 
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// A GUID representing the client account information that relates to the client identification number in the ClientID field.
        /// </summary>
        public Guid Account;

        /// <summary>
        /// Parse the AUX_PERF_ACCOUNTINFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PERF_ACCOUNTINFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ClientID = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.Account = this.ReadGuid();
        }
    }

    #endregion

    #region Section 2.2.2.2.19  AUX_ENDPOINT_CAPABILITIES
    /// <summary>
    ///  A class indicates the AUX_ENDPOINT_CAPABILITIES Auxiliary Block Structure
    /// </summary>
    public class AUX_ENDPOINT_CAPABILITIES : BaseStructure
    {
        /// <summary>
        /// A flag that indicates that the server combines capabilities on a single endpoint.
        /// </summary>
        public EndpointCapabilityFlag EndpointCapabilityFlag;

        /// <summary>
        /// Parse the AUX_ENDPOINT_CAPABILITIES structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_ENDPOINT_CAPABILITIES structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.EndpointCapabilityFlag = (EndpointCapabilityFlag)this.ReadUint();
        }
    }

    #endregion

    #region Section 2.2.2.2.20   AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_CLIENT_CONNECTION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_CLIENT_CONNECTION_INFO : BaseStructure
    {
        /// <summary>
        /// The GUID of the connection to the server.
        /// </summary>
        public Guid ConnectionGUID;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ConnectionContextInfo field.
        /// </summary>
        public ushort OffsetConnectionContextInfo;

        /// <summary>
        /// Padding to enforce alignment of the data on a 4-byte field.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The number of connection attempts.
        /// </summary>
        public uint ConnectionAttempts;

        /// <summary>
        /// A flag designating the mode of operation.
        /// </summary>
        public ConnectionFlags ConnectionFlags;

        /// <summary>
        /// A null-terminated Unicode string that contains opaque connection context information to be logged by the server.
        /// </summary>
        public MAPIString ConnectionContextInfo;

        /// <summary>
        /// Parse the AUX_CLIENT_CONNECTION_INFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_ENDPOINT_CAPABILITIES structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.ConnectionGUID = this.ReadGuid();
            this.OffsetConnectionContextInfo = this.ReadUshort();
            this.Reserved = this.ReadUshort();
            this.ConnectionAttempts = this.ReadUint();
            this.ConnectionFlags = (ConnectionFlags)this.ReadUint();

            if (this.OffsetConnectionContextInfo != 0)
            {
                this.ConnectionContextInfo = new MAPIString(Encoding.Unicode);
                this.ConnectionContextInfo.Parse(s);
            }
        }
    }

    #endregion

    #region Section 2.2.2.2.21   AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    /// <summary>
    ///  A class indicates the AUX_SERVER_SESSION_INFO Auxiliary Block Structure
    /// </summary>
    public class AUX_SERVER_SESSION_INFO : BaseStructure
    {
        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the ServerSessionContextInfo field. 
        /// </summary>
        public ushort OffsetServerSessionContextInfo;

        /// <summary>
        /// A null-terminated Unicode string that contains opaque server session context information to be logged by the client. 
        /// </summary>
        public MAPIString ServerSessionContextInfo;

        /// <summary>
        /// Parse the AUX_SERVER_SESSION_INFO structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_SERVER_SESSION_INFO structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.OffsetServerSessionContextInfo = this.ReadUshort();

            if (this.OffsetServerSessionContextInfo != 0)
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
        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure, as specified in section 2.2.2.2, to the DeviceManufacturer field. 
        /// </summary>
        public ushort DeviceManufacturerOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceModel field. 
        /// </summary>
        public ushort DeviceModelOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceSerialNumber field. 
        /// </summary>
        public ushort DeviceSerialNumberOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceVersion field. 
        /// </summary>
        public ushort DeviceVersionOffset;

        /// <summary>
        /// The offset from the beginning of the AUX_HEADER structure to the DeviceFirmwareVersion field. 
        /// </summary>
        public ushort DeviceFirmwareVersionOffset;

        /// <summary>
        /// A null-terminated Unicode string that contains the name of the manufacturer of the device. 
        /// </summary>
        public MAPIString DeviceManufacturer;

        /// <summary>
        /// A null-terminated Unicode string that contains the model name of the device. 
        /// </summary>
        public MAPIString DeviceModel;

        /// <summary>
        /// A null-terminated Unicode string that contains the serial number of the device. 
        /// </summary>
        public MAPIString DeviceSerialNumber;

        /// <summary>
        /// A null-terminated Unicode string that contains the version number of the device. 
        /// </summary>
        public MAPIString DeviceVersion;

        /// <summary>
        /// A null-terminated Unicode string that contains the firmware version of the device. 
        /// </summary>
        public MAPIString DeviceFirmwareVersion;

        /// <summary>
        /// Parse the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_PROTOCOL_DEVICE_IDENTIFICATION structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.DeviceManufacturerOffset = this.ReadUshort();
            this.DeviceModelOffset = this.ReadUshort();
            this.DeviceSerialNumberOffset = this.ReadUshort();
            this.DeviceVersionOffset = this.ReadUshort();
            this.DeviceFirmwareVersionOffset = this.ReadUshort();

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
        /// <summary>
        /// The size of the AUX_HEADER structure plus any additional payload data.
        /// </summary>
        public ushort Size;

        /// <summary>
        /// The version information of the payload data.
        /// </summary>
        public PayloadDataVersion Version;

        /// <summary>
        /// The type of auxiliary block data structure. The Type should be AuxiliaryBlockType_1 or AuxiliaryBlockType_2.
        /// </summary>
        public object Type;

        /// <summary>
        /// Parse the AUX_HEADER structure.
        /// </summary>
        /// <param name="s">A stream containing the AUX_HEADER structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Size = this.ReadUshort();
            this.Version = (PayloadDataVersion)this.ReadByte();

            if (this.Version == PayloadDataVersion.AUX_VERSION_1)
            {
                this.Type = (AuxiliaryBlockType_1)this.ReadByte();
            }
            else
            {
                this.Type = (AuxiliaryBlockType_2)this.ReadByte();
            }
        }
    }

    #endregion

    #region rgbIn Input Buffer
    /// <summary>
    /// The rgbInputBuffer contains the ROP request payload. It is defined in section 3.1.4.2.1.1.1 of MS-OXCRPC.
    /// </summary>
    public class RgbInputBuffer : BaseStructure
    {
        /// <summary>
        /// The RPC_HEADER_EXT structure provides information about the payload.
        /// </summary>
        public ExtendedBuffer_Input[] Buffers;

        /// <summary>
        /// A unsigned int value indicates the total buffers size
        /// </summary>
        private uint ropBufferSize;

        /// <summary>
        /// Initializes a new instance of the RgbInputBuffer class
        /// </summary>
        /// <param name="buffersize">The buffer size</param>
        public RgbInputBuffer(uint buffersize)
        {
            this.ropBufferSize = buffersize;
        }

        /// <summary>
        /// Parse the rgbInputBuffer. 
        /// </summary>
        /// <param name="s">A stream containing the rgbInputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            List<ExtendedBuffer_Input> extendedBuffer_Inputs = new List<ExtendedBuffer_Input>();
            MapiInspector.MAPIInspector.InputPayLoadCompressedXOR = new List<byte[]>();
            MapiInspector.MAPIInspector.BuffersIsCompressed = new List<bool>();

            while (this.ropBufferSize > 0)
            {
                ExtendedBuffer_Input extendedBuffer_Input = new ExtendedBuffer_Input(index);
                extendedBuffer_Input.Parse(s);
                extendedBuffer_Inputs.Add(extendedBuffer_Input);
                this.ropBufferSize -= (uint)(extendedBuffer_Input.RPCHEADEREXT.Size + 8);
                index += 1;
            }

            this.Buffers = extendedBuffer_Inputs.ToArray();
        }
    }

    /// <summary>
    /// The ExtendedBuffer_Input class
    /// </summary>
    public class ExtendedBuffer_Input : BaseStructure
    {
        /// <summary>
        /// The RPC_HEADER_EXT structure provides information about the payload.
        /// </summary>
        public RPC_HEADER_EXT RPCHEADEREXT;

        /// <summary>
        /// A structure of bytes that constitute the ROP request payload. 
        /// </summary>
        public object Payload;

        /// <summary>
        /// Buffer index in one session
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the ExtendedBuffer_Input class
        /// </summary>
        /// <param name="num">The number for extended buffer</param>
        public ExtendedBuffer_Input(int num)
        {
            this.index = num;
        }

        /// <summary>
        /// Parse the rgbInputBuffer. 
        /// </summary>
        /// <param name="s">A stream containing the rgbInputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RPCHEADEREXT = new RPC_HEADER_EXT();
            this.RPCHEADEREXT.Parse(s);

            if (this.RPCHEADEREXT.Size > 0)
            {
                byte[] payloadBytes = this.ReadBytes((int)this.RPCHEADEREXT.Size);
                bool isCompressedXOR = false;

                if (((ushort)this.RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)this.RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)this.RPCHEADEREXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (this.index > 0)
                {
                    if (isCompressedXOR)
                    {
                        if (!MapiInspector.MAPIInspector.BuffersIsCompressed.Contains(true))
                        {
                            MapiInspector.MAPIInspector.InputPayLoadCompressedXOR = new List<byte[]>();
                        }

                        MapiInspector.MAPIInspector.InputPayLoadCompressedXOR.Add(payloadBytes);
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(true);
                    }
                    else
                    {
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(false);
                    }
                }
                else
                {
                    MapiInspector.MAPIInspector.BuffersIsCompressed = new List<bool>();

                    if (isCompressedXOR)
                    {
                        MapiInspector.MAPIInspector.InputPayLoadCompressedXOR = new List<byte[]>();
                        MapiInspector.MAPIInspector.InputPayLoadCompressedXOR.Add(payloadBytes);
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(true);
                    }
                    else
                    {
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(false);
                    }
                }

                Stream stream = new MemoryStream(payloadBytes);

                if (MapiInspector.MAPIInspector.IsOnlyGetServerHandle)
                {
                    ROPInputBuffer_WithoutCROPS inputBufferWithoutCROPS = new ROPInputBuffer_WithoutCROPS();
                    inputBufferWithoutCROPS.Parse(stream);
                    this.Payload = inputBufferWithoutCROPS;
                }
                else
                {
                    ROPInputBuffer inputBuffer = new ROPInputBuffer();
                    inputBuffer.Parse(stream);
                    this.Payload = inputBuffer;
                }
            }
        }
    }
    #endregion

    #region rgbOut Output Buffer
    /// <summary>
    /// The rgbOutputBuffer contains the ROP request payload. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class RgbOutputBuffer : BaseStructure
    {
        /// <summary>
        /// The RPC_HEADER_EXT structure provides information about the payload.
        /// </summary>
        public RPC_HEADER_EXT RPCHEADEREXT;

        /// <summary>
        /// A structure of bytes that constitute the ROP responses payload. 
        /// </summary>
        public object Payload;

        /// <summary>
        /// Indicates the index of this rgbOutputBuffer in all buffers
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the RgbOutputBuffer class
        /// </summary>
        /// <param name="num">The number for rgbOutputBuffer</param>
        public RgbOutputBuffer(int num)
        {
            this.index = num;
        }

        /// <summary>
        /// Parse the rgbOutputBuffer. 
        /// </summary>
        /// <param name="s">A stream containing the rgbOutputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.RPCHEADEREXT = new RPC_HEADER_EXT();
            this.RPCHEADEREXT.Parse(s);

            if (this.RPCHEADEREXT.Size > 0)
            {
                byte[] payloadBytes = this.ReadBytes((int)this.RPCHEADEREXT.Size);
                bool isCompressedXOR = false;

                if (((ushort)this.RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)this.RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)this.RPCHEADEREXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (this.index > 0)
                {
                    if (isCompressedXOR)
                    {
                        if (!MapiInspector.MAPIInspector.BuffersIsCompressed.Contains(true))
                        {
                            MapiInspector.MAPIInspector.OutputPayLoadCompressedXOR = new List<byte[]>();
                        }

                        MapiInspector.MAPIInspector.OutputPayLoadCompressedXOR.Add(payloadBytes);
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(true);
                    }
                    else
                    {
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(false);
                    }
                }
                else
                {
                    MapiInspector.MAPIInspector.BuffersIsCompressed = new List<bool>();

                    if (isCompressedXOR)
                    {
                        MapiInspector.MAPIInspector.OutputPayLoadCompressedXOR = new List<byte[]>();
                        MapiInspector.MAPIInspector.OutputPayLoadCompressedXOR.Add(payloadBytes);
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(true);
                    }
                    else
                    {
                        MapiInspector.MAPIInspector.BuffersIsCompressed.Add(false);
                    }
                }

                Stream stream = new MemoryStream(payloadBytes);

                if (MapiInspector.MAPIInspector.IsOnlyGetServerHandle)
                {
                    ROPOutputBuffer_WithoutCROPS outputBufferWithoutCROPS = new ROPOutputBuffer_WithoutCROPS();
                    outputBufferWithoutCROPS.Parse(stream);
                    this.Payload = outputBufferWithoutCROPS;
                }
                else
                {
                    ROPOutputBuffer outputBuffer = new ROPOutputBuffer();
                    outputBuffer.Parse(stream);
                    this.Payload = outputBuffer;
                }
            }
        }
    }

    /// <summary>
    /// The rgbOutputBufferPack contains multiple rgbOutputBuffer structure. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class RgbOutputBufferPack : BaseStructure
    {
        /// <summary>
        /// An unsigned int indicates the total size of the rgbOutputBuffers, this is a customized value.
        /// </summary>
        private uint RopBufferSize;

        /// <summary>
        /// rgbOutputBuffer packing.
        /// </summary>
        public RgbOutputBuffer[] RgbOutputBuffers;

        /// <summary>
        /// Initializes a new instance of the RgbOutputBufferPack class.
        /// </summary>
        /// <param name="ropBufferSize">The RopBuffer size</param>
        public RgbOutputBufferPack(uint ropBufferSize)
        {
            this.RopBufferSize = ropBufferSize;
        }

        /// <summary>
        /// Parse the rgbOutputBufferPack. 
        /// </summary>
        /// <param name="s">A stream containing the rgbOutputBufferPack.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            List<RgbOutputBuffer> rgbOutputBufferList = new List<RgbOutputBuffer>();
            long startPosition = s.Position;
            MapiInspector.MAPIInspector.OutputPayLoadCompressedXOR = new List<byte[]>();
            MapiInspector.MAPIInspector.BuffersIsCompressed = new List<bool>();

            while (s.Position - startPosition < this.RopBufferSize)
            {
                RgbOutputBuffer buffer = new RgbOutputBuffer(index);
                buffer.Parse(s);
                rgbOutputBufferList.Add(buffer);
                index += 1;
            }

            this.RgbOutputBuffers = rgbOutputBufferList.ToArray();
        }
    }
    #endregion

    #region Parse common message methods
    /// <summary>
    /// Parse the additional headers in Common Response Format
    /// </summary>
    public class ParseMAPIMethod : BaseStructure
    {
        /// <summary>
        /// ParseAddtionlHeader method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        /// <param name="metaTags">MetaTags string</param>
        /// <param name="additionalHeaders">AdditionalHeaders string</param>
        public void ParseAddtionlHeader(Stream s, out List<MAPIString> metaTags, out List<MAPIString> additionalHeaders)
        {
            this.Parse(s);
            string str = null;
            List<MAPIString> tempmetaTags = new List<MAPIString>();
            List<MAPIString> tempadditionalHeaders = new List<MAPIString>();

            while ((str != string.Empty) && (s.Position < s.Length - 1))
            {
                str = this.ReadString(Encoding.ASCII, "\r\n");
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
                        if (str != string.Empty)
                        {
                            tempadditionalHeaders.Add(tempString);
                            break;
                        }
                        else
                        {
                            tempString.Value = string.Empty;
                            tempadditionalHeaders.Add(tempString);
                            break;
                        }
                }
            }

            metaTags = tempmetaTags;
            additionalHeaders = tempadditionalHeaders;
        }

        /// <summary>
        /// Override parse method. 
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
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
        /// <param name="actualLength">The expected size of the decompressed output stream.</param>
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
                else
                {
                    // Otherwise copy the data specified by MetaData (offset, length) pair
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
        /// The function is used to get the MetaData from raw request data
        /// </summary>
        /// <param name="encodedBuffer">The raw request data</param>
        /// <param name="decodingPosition">The decoding position for the raw request data</param>
        /// <param name="shareByteCache">The shared bytes stack</param>
        /// <param name="offset">The returned offset value</param>
        /// <param name="length">The returned length value</param>
        public static void GetMetaDataValue(byte[] encodedBuffer, ref int decodingPosition, ref byte? shareByteCache, out int offset, out int length)
        {
            // Initialize: To encode a length between 3 and 9, we use the 3 bits that are "in-line" in the 2-byte MetaData.
            ushort inlineMetadata = 0;
            inlineMetadata = BitConverter.ToUInt16(encodedBuffer, decodingPosition);
            decodingPosition += 2;

            offset = inlineMetadata >> 3;
            offset++;
            length = inlineMetadata & 0x0007;

            // Add the minimum match - 3 bytes
            length += 3;

            // Every other time that the length is greater than 9, 
            // an additional byte follows the initial 2-byte MetaData
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