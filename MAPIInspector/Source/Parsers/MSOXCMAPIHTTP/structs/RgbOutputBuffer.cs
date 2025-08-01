using BlockParser;
using System;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCRPC] 3.1.4.1.1.1.2 rgbAuxOut Output Buffer
    /// The rgbOutputBuffer contains the ROP request payload. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class RgbOutputBuffer : Block
    {
        /// <summary>
        /// The RPC_HEADER_EXT structure provides information about the payload.
        /// </summary>
        public RPC_HEADER_EXT RPCHEADEREXT;

        /// <summary>
        /// A structure of bytes that constitute the ROP responses payload.
        /// </summary>
        public Block Payload;

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
        protected override void Parse()
        {
            RPCHEADEREXT = Parse<RPC_HEADER_EXT>();

            if (RPCHEADEREXT._Size > 0)
            {
                var payloadOffset = parser.Offset; // remember the offset for the payload
                var payloadBlock = ParseBytes((int)RPCHEADEREXT._Size);
                var payloadBytes = payloadBlock.Data;
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

                var newParser = new BinaryParser(payloadBytes);

                if (MapiInspector.MAPIParser.IsOnlyGetServerHandle)
                {
                    Payload = Parse<ROPBufferServerObjectTable>(newParser);
                }
                else
                {
                    try
                    {
                        Payload = Parse<ROPOutputBuffer>(newParser);
                        Payload.ShiftOffset(payloadOffset); // shift the offset to the original position
                    }
                    catch (MissingInformationException) { throw; }
                    catch (MissingPartialInformationException) { throw; }
                    catch (Exception e)
                    {
                        AddChild(BlockException.Create("Exception", e, 0));
                    }
                }
            }
        }

        protected override void ParseBlocks()
        {
            Text = $"rgbOutputBuffer";
            AddChild(RPCHEADEREXT, "RPC_HEADER_EXT");
            AddChild(Payload, "Payload");
        }
    }
}
