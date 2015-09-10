using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MapiInspector;
using System.Reflection;

namespace MAPIInspector.Parsers
{

    class ConnectRequestBodyType : BaseStructure
    {
        public struct ConnectRequest
        {
            public string UserDn;
            public uint Flags;
            public uint DefaultCodePage;
            public uint LcidSort;
            public uint LcidString;
            public uint AuxiliaryBufferSize;
            public ExtendedBuffer AuxiliaryBuffer;
        }
       
        public ConnectRequest connectRequest;
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RawData rawData = new RawData(s);
            Type t= typeof(ConnectRequest);
            FieldInfo[] fields = t.GetFields();
			object testa = Activator.CreateInstance(t);
            ulong start = 0;
            ulong offset = 0;
            ulong totalOffset = 0;
            object result = null;
			
            foreach(FieldInfo f in fields)
            {       
               if (f.Name != "AuxiliaryBuffer" && Enum.IsDefined(typeof(RawData.DataType), f.FieldType.Name))
               {    
                    RawData.DataType dataType = (RawData.DataType)Enum.Parse(typeof(RawData.DataType), f.FieldType.Name);
                    result = rawData.ConsumeUsingKind(dataType, out start, out offset);

                    fieldsInfoStart.Add(f, start);
                    fieldsInfoLength.Add(f, offset);
                    f.SetValue(testa, result);
                    totalOffset += offset;
               }
               else
               {
                   if (((ConnectRequest)testa).AuxiliaryBufferSize > 0)
                   {
                        f.SetValue(testa, new ExtendedBuffer(true));
                        s.Position = (long)rawData.CurrentBitPosition;
                        ((ConnectRequest)testa).AuxiliaryBuffer.Parse(s);
                   }
                   else
                   {
                       f.SetValue(testa, null);
                   }
               }
          } 
 
            connectRequest = (ConnectRequest)testa;
            typeResult.Add(connectRequest, totalOffset);
        }

        public override void AddTreeChildren(TreeNode node)
        {
            if (fieldsInfoStart != null)
            {
                foreach (var feild in fieldsInfoStart)
                {
                    object obj = null;
                    foreach (var key in typeResult.Keys)
                    {
                        obj = key;
                    }

                    TreeNode treeNode = new TreeNode(string.Format("{0}：{1:x8}", feild.Key.Name, feild.Key.GetValue(obj)));
                    node.Nodes.Add(treeNode);
                    int[] a = new int[2]{(int)fieldsInfoStart[feild.Key]/8, (int)fieldsInfoLength[feild.Key]/8};
                    TreeNodeOffsetAndLength.Add(treeNode, a);
                }
            }
        }
   }

    class ExtendedBuffer : BaseStructure
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

        public override void AddTreeChildren(TreeNode node)
        {
            TreeNode n1 = new TreeNode("RPC_HEADER_EXT: ");
            RPC_HEADER_EXT.AddTreeChildren(n1);
            node.Nodes.Add(n1);
            
            if (Payload.Length > 0)
            {
                TreeNode n2 = new TreeNode("Payload: " + Payload.Length.ToString() + " bytes");
                foreach (byte b in Payload)
                {
                    n2.Nodes.Add(new TreeNode(b.ToString()));
                }
                node.Nodes.Add(n2);
            }
        }
    }

    class RPC_HEADER_EXT : BaseStructure
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

        public override void AddTreeChildren(TreeNode node)
        {
            base.AddTreeChildren(node);
            node.Nodes.Add(new TreeNode("Version: " + Utilities.ConvertUintToString(Version)));
            node.Nodes.Add(new TreeNode("Flags: " + Enum.GetName(typeof(RpcHeaderFlags), Flags)));
            node.Nodes.Add(new TreeNode("Size: " + Utilities.ConvertUintToString(Size)));
            node.Nodes.Add(new TreeNode("SizeActual: " + Utilities.ConvertUintToString(SizeActual)));
        }
    }

    enum RpcHeaderFlags : ushort
    {
        Compressed = 0x0001,
        XorMagic = 0x0002,
        Last = 0x0004
    }

    class ConnectResponseBodyType : BaseStructure
    {
        public string[] MetaTags;
        public string[] AdditionalHeaders;
        public uint StatusCode;
        public uint ErrorCode;
        public uint PollsMax;
        public uint RetryCount;
        public uint RetryDelay;
        public string DnPrefix;
        public string DisplayName;
        public uint AuxiliaryBufferSize;
        public ExtendedBuffer AuxiliaryBuffer;

        public override void Parse(Stream s)
        {
            base.Parse(s);
            string str = null;
            List<string> metaTags = new List<string>();
            List<string> additionalHeaders = new List<string>();

            while(str != "")
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
                        }
                        break;
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

        public override void AddTreeChildren(TreeNode node)
        {
            TreeNode metaTags = new TreeNode("MetaTags:");
            foreach(string str in this.MetaTags)
            {
                metaTags.Nodes.Add(new TreeNode(str));
            }
            node.Nodes.Add(metaTags);

            TreeNode additionalHeaders = new TreeNode("AdditionalHeaders:");
            foreach (string str in this.AdditionalHeaders)
            {
                additionalHeaders.Nodes.Add(new TreeNode(str));
            }
            node.Nodes.Add(additionalHeaders);

            node.Nodes.Add(new TreeNode("StatusCode: " + Utilities.ConvertUintToString(StatusCode)));
            node.Nodes.Add(new TreeNode("ErrorCode: " + Utilities.ConvertUintToString(ErrorCode)));
            node.Nodes.Add(new TreeNode("PollsMax: " + Utilities.ConvertUintToString(PollsMax)));
            node.Nodes.Add(new TreeNode("RetryCount: " + Utilities.ConvertUintToString(RetryCount)));
            node.Nodes.Add(new TreeNode("RetryDelay: " + Utilities.ConvertUintToString(RetryDelay)));
            node.Nodes.Add(new TreeNode("DnPrefix: " + DnPrefix));
            node.Nodes.Add(new TreeNode("DisplayName: " + DisplayName));
            node.Nodes.Add(new TreeNode("AuxiliaryBufferSize: " + Utilities.ConvertUintToString(AuxiliaryBufferSize)));

            if (AuxiliaryBuffer != null)
            {
                TreeNode AuxiliaryBufferNode = new TreeNode("AuxiliaryBuffer: " + AuxiliaryBuffer.ToString());
                AuxiliaryBuffer.AddTreeChildren(AuxiliaryBufferNode);
                node.Nodes.Add(AuxiliaryBufferNode);
            }
        }
    }    
}