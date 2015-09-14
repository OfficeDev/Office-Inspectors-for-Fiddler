using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MapiInspector;
using System.Reflection;

namespace MAPIInspector.Parsers
{
    public class ConnectRequestBodyType : BaseStructure
    {
        [HelpAttribute(StringEncoding.ASCII, 1)]
        public string UserDn;
        public uint Flags;
        public uint DefaultCodePage;
        public uint LcidSort;
        public uint LcidString;
        public uint AuxiliaryBufferSize;
        public ExtendedBuffer AuxiliaryBuffer;

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

    public class ExtendedBuffer : BaseStructure
    {
        public RPC_HEADER_EXT RPC_HEADER_EXT;
        public byte[] Payload;

        public ExtendedBuffer(bool isAuxiliaryBuffer)
        {

        }

        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.RPC_HEADER_EXT = new RPC_HEADER_EXT();
            this.RPC_HEADER_EXT.Parse(s);
            this.Payload = ReadBytes(RPC_HEADER_EXT.Size);
        }
    }

    public class RPC_HEADER_EXT : BaseStructure
    {
        public ushort Version;
        public RpcHeaderFlags Flags;
        public ushort Size;
        public ushort SizeActual;

        public override void Parse(Stream s)
        {
            base.Parse(s);
            this.Version = ReadUshort();
            this.Flags = (RpcHeaderFlags)ReadUshort();
            this.Size = ReadUshort();
            this.SizeActual = ReadUshort();
        }       
    }

    public enum RpcHeaderFlags : ushort
    {
        Compressed = 0x0001,
        XorMagic = 0x0002,
        Last = 0x0004
    }

    class ConnectResponseBodyType : BaseStructure
    {
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] MetaTags;
        [HelpAttribute(StringEncoding.ASCII, 2)]
        public string[] AdditionalHeaders;
        public uint StatusCode;
        public uint ErrorCode;
        public uint PollsMax;
        public uint RetryCount;
        public uint RetryDelay;
        [HelpAttribute(StringEncoding.ASCII, 1)]
        public string DnPrefix;
        [HelpAttribute(StringEncoding.Unicode, 2)]
        public string DisplayName;
        public uint AuxiliaryBufferSize;
        public ExtendedBuffer AuxiliaryBuffer;

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
            this.ErrorCode = ReadUint();
            this.PollsMax = ReadUint();
            this.RetryCount = ReadUint();
            this.RetryDelay = ReadUint();
            this.DnPrefix = ReadString();
            this.DisplayName = ReadString(Encoding.Unicode);
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
}