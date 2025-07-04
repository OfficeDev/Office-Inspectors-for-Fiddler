using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 3.1.4.1.1.1 Extended Buffer Format
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
            try
            {
                base.Parse(s);

                RPCHEADEREXT = new RPC_HEADER_EXT();
                RPCHEADEREXT.Parse(s);

                if (RPCHEADEREXT._Size > 0)
                {
                    byte[] payloadBytes = ReadBytes(RPCHEADEREXT._Size);
                    bool isCompressedXOR = false;

                    if (RPCHEADEREXT.Flags.Data.HasFlag(RpcHeaderFlags.XorMagic))
                    {
                        payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                        isCompressedXOR = true;
                    }

                    if (RPCHEADEREXT.Flags.Data.HasFlag(RpcHeaderFlags.Compressed))
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

                    for (int length = 0; length < RPCHEADEREXT._Size;)
                    {
                        AuxiliaryBufferPayload buffer = new AuxiliaryBufferPayload();
                        buffer.Parse(stream);
                        payload.Add(buffer);
                        length += buffer.AUXHEADER.Size;
                    }

                    Payload = payload.ToArray();
                }
            }
            catch { }
        }
    }
}