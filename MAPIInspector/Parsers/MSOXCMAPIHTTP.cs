using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MapiInspector;
using System.Reflection;

namespace MAPIInspector.Parsers
{

    /// <summary>
    /// The auxiliary blocks sent from the server to the client in the rgbAuxOut parameter auxiliary buffer on the EcDoConnectEx method.
    /// </summary>
    public class ExtendedBuffer : BaseStructure
    {
        // The RPC_HEADER_EXT structure provides information about the payload.
        public RPC_HEADER_EXT RPC_HEADER_EXT;
        // TODO: A structure of bytes that constitute the auxiliary payload data returned from the server. 
        public byte[] Payload;
        /// <summary>
        /// The Constructors of ExtendedBuffer.
        /// </summary>
        /// <param name="isAuxiliaryBuffer">The bool value that speicfied if it is an auxiliary buffer.</param>
        public ExtendedBuffer(bool isAuxiliaryBuffer)
        {

        }

        /// <summary>
        /// Parse the ExtendedBuffer. 
        /// </summary>
        /// <param name="s">An stream of the extended buffers.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RPC_HEADER_EXT = new RPC_HEADER_EXT();
            this.RPC_HEADER_EXT.Parse(s);
            this.Payload = ReadBytes(RPC_HEADER_EXT.Size);
        }
    }

    /// <summary>
    /// The RPC_HEADER_EXT structure provides information about the payload.
    /// </summary>
    public class RPC_HEADER_EXT : BaseStructure
    {
        //The version of the structure. This value MUST be set to 0x0000.
        public ushort Version;
        //The flags that specify how data that follows this header MUST be interpreted. 
        public RpcHeaderFlags Flags;
        //The total length of the payload data that follows the RPC_HEADER_EXT structure. 
        public ushort Size;
        //The length of the payload data after it has been uncompressed.
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
    /// The enum flags that specify how data that follows this header MUST be interpreted. 
    /// </summary>
    public enum RpcHeaderFlags : ushort
    {
        //The data that follows the RPC_HEADER_EXT structure is compressed. 
        Compressed = 0x0001,
        //The data following the RPC_HEADER_EXT structure has been obfuscated. 
        XorMagic = 0x0002,
        //No other RPC_HEADER_EXT structure follows the data of the current RPC_HEADER_EXT structure. 
        Last = 0x0004
    }

    #region 2.2.4.1	Connect

    /// <summary>
    ///  A class indicates the Connect request type.
    /// </summary>
    public class ConnectRequestBody : BaseStructure
    {
        //A null-terminated ASCII string that specifies the DN of the user who is requesting the connection. 
        [HelpAttribute(StringEncoding.ASCII, 1)]
        public string UserDn;
        //A set of flags that designate the type of connection being requested. 
        public uint Flags;
        //An unsigned integer that specifies the code page that the server is being requested to use for string values of properties. 
        public uint DefaultCodePage;
        //An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for sorting. 
        public uint LcidSort;
        //An unsigned integer that specifies the language code identifier (LCID), as specified in [MS-LCID], to be used for everything other than sorting. 
        public uint LcidString;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.UserDn = ReadString();
            this.Flags = ReadUint();
            this.DefaultCodePage = ReadUint();
            this.LcidSort = ReadUint();
            this.LcidString = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] MetaTags;
        // A string array that specifies additional header information.
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] AdditionalHeaders;
        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;
        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;
        //An unsigned integer that specifies the number of milliseconds for the maximum polling interval.
        public uint PollsMax;
        //): An unsigned integer that specifies the number of times to retry request types.
        public uint RetryCount;
        //An unsigned integer that specifies the number of milliseconds for the client to wait before retrying a failed request type. 
        public uint RetryDelay;
        //A null-terminated ASCII string that specifies the DN prefix to be used for building message recipients. 
        [HelpAttribute(StringEncoding.ASCII, 1)]
        public string DnPrefix;
        //A null-terminated Unicode string that specifies the display name of the user who is specified in the UserDn field of the Connect request type request body. 
        [HelpAttribute(StringEncoding.Unicode, 2)]
        public string DisplayName;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            string str = null;
            List<string> metaTags = new List<string>();
            List<string> additionalHeaders = new List<string>();

            while (str != "")
            {
                str = ReadString("\r\n");
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        metaTags.Add(str);
                        break;
                    default:
                        if (str != "")
                        {
                            additionalHeaders.Add(str);
                            break;
                        }
                        else
                        {
                            additionalHeaders.Add("");
                            break;
                        }
                }
            }
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.PollsMax = ReadUint();
                this.RetryCount = ReadUint();
                this.RetryDelay = ReadUint();
                this.DnPrefix = ReadString();
                this.DisplayName = ReadString(Encoding.Unicode);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        // TODO: An array of bytes that constitute the ROP requests payload. 
        public byte[] RopBuffer;
        // An unsigned integer that specifies the maximum size for the RopBuffer field of the Execute request type success response body.
        public uint MaxRopOut;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
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
            this.RopBuffer = new byte[this.RopBufferSize];
            this.RopBuffer = ReadBytes((int)this.RopBufferSize);
            this.MaxRopOut = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] MetaTags;
        // A string array that specifies additional header information.
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] AdditionalHeaders;
        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;
        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;
        // The reserved flag. The server MUST set this field to 0x00000000 and the client MUST ignore this field.
        public uint Flags;
        // An unsigned integer that specifies the size, in bytes, of the RopBuffer field.
        public uint RopBufferSize;
        // An array of bytes that constitute the ROP responses payload. 
        public byte[] RopBuffer;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            string str = null;
            List<string> metaTags = new List<string>();
            List<string> additionalHeaders = new List<string>();

            while (str != "")
            {
                str = ReadString("\r\n");
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        metaTags.Add(str);
                        break;
                    default:
                        if (str != "")
                        {
                            additionalHeaders.Add(str);
                            break;
                        }
                        else
                        {
                            additionalHeaders.Add("");
                            break;
                        }
                }
            }
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.Flags = ReadUint();
                this.RopBufferSize = ReadUint();
                this.RopBuffer = new byte[this.RopBufferSize];
                this.RopBuffer = ReadBytes((int)this.RopBufferSize);
            }
            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
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
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] MetaTags;
        // A string array that specifies additional header information.
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] AdditionalHeaders;
        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;
        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            string str = null;
            List<string> metaTags = new List<string>();
            List<string> additionalHeaders = new List<string>();

            while (str != "")
            {
                str = ReadString("\r\n");
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        metaTags.Add(str);
                        break;
                    default:
                        if (str != "")
                        {
                            additionalHeaders.Add(str);
                            break;
                        }
                        else
                        {
                            additionalHeaders.Add("");
                            break;
                        }
                }
            }

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
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        //Reserved. The client MUST set this field to 0x00000000 and the server MUST ignore this field.
        public uint Flags;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;
        public override void Parse(Stream s)
        {
            base.Parse(s);

            this.Flags = ReadUint();
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] MetaTags;
        // A string array that specifies additional header information.
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] AdditionalHeaders;
        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;
        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;
        //An unsigned integer that indicates whether an event is pending on the Session Context. 
        public uint EventPending;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field. 
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client. 
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            string str = null;
            List<string> metaTags = new List<string>();
            List<string> additionalHeaders = new List<string>();

            while (str != "")
            {
                str = ReadString("\r\n");
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        metaTags.Add(str);
                        break;
                    default:
                        if (str != "")
                        {
                            additionalHeaders.Add(str);
                            break;
                        }
                        else
                        {
                            additionalHeaders.Add("");
                            break;
                        }
                }
            }
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
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }

    #endregion

    #region 2.2.5.1  Bind
    /// <summary>
    ///  A class indicates the Bind request type request body.
    /// </summary>
    public class BindRequest : BaseStructure
    {
        // An unsigned integer that specify the authentication type for the connection.
        public uint Flags;
        // A Boolean value that specifies whether the State field is present.
        public byte HasState;
        // An array of bytes that specifies the state of a specific address book container. 
        public byte[] State;
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
            if (HasState == 1)
            {
                this.State = ReadBytes(36);
            }
            else
            {
                this.State = null;
            }

            this.AuxiliaryBufferSize = ReadUint();
            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
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
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] MetaTags;
        // A string array that specifies additional header information.
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] AdditionalHeaders;
        // An unsigned integer that specifies the status of the request.
        public uint StatusCode;
        // An unsigned integer that specifies the return status of the operation.
        public uint ErrorCode;
        // A GUID that is associated with a specific address book server.
        public Guid ServerGuid;
        // An unsigned integer that specifies the size, in bytes, of the AuxiliaryBuffer field.  
        public uint AuxiliaryBufferSize;
        //An array of bytes that constitute the auxiliary payload data sent from the client.
        public ExtendedBuffer AuxiliaryBuffer;

        /// <summary>
        /// Parse the HTTP payload of session.
        /// </summary>
        /// <param name="s">An stream of HTTP payload of session</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            string str = null;
            List<string> metaTags = new List<string>();
            List<string> additionalHeaders = new List<string>();

            while (str != "")
            {
                str = ReadString("\r\n");
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        metaTags.Add(str);
                        break;
                    default:
                        if (str != "")
                        {
                            additionalHeaders.Add(str);
                            break;
                        }
                        else
                        {
                            additionalHeaders.Add("");
                            break;
                        }
                }
            }
            this.MetaTags = metaTags.ToArray();
            this.AdditionalHeaders = additionalHeaders.ToArray();
            this.StatusCode = ReadUint();
            if (this.StatusCode == 0)
            {
                this.ErrorCode = ReadUint();
                this.ServerGuid = new Guid(ReadBytes(16));
            }
            this.AuxiliaryBufferSize = ReadUint();

            if (this.AuxiliaryBufferSize > 0)
            {
                this.AuxiliaryBuffer = new ExtendedBuffer(true);
                this.AuxiliaryBuffer.Parse(s);
            }
            else
            {
                this.AuxiliaryBuffer = null;
            }
        }
    }
    #endregion


}