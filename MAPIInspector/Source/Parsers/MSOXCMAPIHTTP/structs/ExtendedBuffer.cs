using BlockParser;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 3.1.4.1.1.1 Extended Buffer Format
    /// The auxiliary blocks sent from the server to the client in the rgbAuxOut parameter auxiliary buffer on the EcDoConnectEx method. It is defined in section 3.1.4.1.1.1 of MS-OXCRPC.
    /// </summary>
    public class ExtendedBuffer : Block
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
        protected override void Parse()
        {
            try
            {
                RPCHEADEREXT = Parse<RPC_HEADER_EXT>();

                if (RPCHEADEREXT._Size > 0)
                {
                    BlockBytes payloadBytes = ParseBytes((int)RPCHEADEREXT.Size);
                    bool isCompressedXOR = false;

                    if (((ushort)RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                    {
                        payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes.Data);
                        isCompressedXOR = true;
                    }

                    if (((ushort)RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                    {
                        payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)RPCHEADEREXT.SizeActual);
                        isCompressedXOR = true;
                    }

                    if (isCompressedXOR)
                    {
                        MapiInspector.MAPIParser.AuxPayLoadCompressedXOR = payloadBytes;
                    }

                    Stream stream = new MemoryStream(payloadBytes);
                    List<AuxiliaryBufferPayload> payload = new List<AuxiliaryBufferPayload>();

                    for (int length = 0; length < RPCHEADEREXT.Size;)
                    {
                        var buffer = Parse<AuxiliaryBufferPayload>();
                        payload.Add(buffer);
                        length += buffer.AUXHEADER._Size;
                    }

                    Payload = payload.ToArray();
                }
            }
            catch { }
        }

        protected override void ParseBlocks()
        {
            SetText("ExtendedBuffer");
            AddChild(RPCHEADEREXT, "RPCHEADEREXT");
            AddLabeledChildren(Payload, "Payload");
        }
    }
}