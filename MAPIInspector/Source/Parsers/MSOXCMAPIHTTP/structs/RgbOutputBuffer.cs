using System;
using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 3.1.4.1.1.1.2 rgbAuxOut Output Buffer
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
            index = num;
        }

        /// <summary>
        /// Parse the rgbOutputBuffer.
        /// </summary>
        /// <param name="s">A stream containing the rgbOutputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RPCHEADEREXT = new RPC_HEADER_EXT();
            RPCHEADEREXT.Parse(s);

            if (RPCHEADEREXT._Size > 0)
            {
                byte[] payloadBytes = ReadBytes(RPCHEADEREXT._Size);
                bool isCompressedXOR = false;

                if (((ushort)RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.XorMagic) == (ushort)RpcHeaderFlags.XorMagic)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.XOR(payloadBytes);
                    isCompressedXOR = true;
                }

                if (((ushort)RPCHEADEREXT.Flags & (ushort)RpcHeaderFlags.Compressed) == (ushort)RpcHeaderFlags.Compressed)
                {
                    payloadBytes = CompressionAndObfuscationAlgorithm.LZ77Decompress(payloadBytes, (int)RPCHEADEREXT.SizeActual);
                    isCompressedXOR = true;
                }

                if (index > 0)
                {
                    if (isCompressedXOR)
                    {
                        if (!MapiInspector.MAPIParser.BuffersIsCompressed.Contains(true))
                        {
                            MapiInspector.MAPIParser.OutputPayLoadCompressedXOR = new List<byte[]>();
                        }

                        MapiInspector.MAPIParser.OutputPayLoadCompressedXOR.Add(payloadBytes);
                        MapiInspector.MAPIParser.BuffersIsCompressed.Add(true);
                    }
                    else
                    {
                        MapiInspector.MAPIParser.BuffersIsCompressed.Add(false);
                    }
                }
                else
                {
                    MapiInspector.MAPIParser.BuffersIsCompressed = new List<bool>();

                    if (isCompressedXOR)
                    {
                        MapiInspector.MAPIParser.OutputPayLoadCompressedXOR = new List<byte[]>();
                        MapiInspector.MAPIParser.OutputPayLoadCompressedXOR.Add(payloadBytes);
                        MapiInspector.MAPIParser.BuffersIsCompressed.Add(true);
                    }
                    else
                    {
                        MapiInspector.MAPIParser.BuffersIsCompressed.Add(false);
                    }
                }

                Stream stream = new MemoryStream(payloadBytes);

                if (MapiInspector.MAPIParser.IsOnlyGetServerHandle)
                {
                    ROPOutputBuffer_WithoutCROPS outputBufferWithoutCROPS = new ROPOutputBuffer_WithoutCROPS();
                    outputBufferWithoutCROPS.Parse(stream);
                    Payload = outputBufferWithoutCROPS;
                }
                else
                {
                    try
                    {
                        ROPOutputBuffer outputBuffer = new ROPOutputBuffer();
                        outputBuffer.Parse(stream);
                        Payload = outputBuffer;
                    }
                    catch (MissingInformationException) { throw; }
                    catch (MissingPartialInformationException) { throw; }
                    catch (Exception e)
                    {
                        Payload = e.ToString();
                    }
                }
            }
        }
    }
}