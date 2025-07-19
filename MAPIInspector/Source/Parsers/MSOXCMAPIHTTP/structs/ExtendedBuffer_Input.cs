using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The ExtendedBuffer_Input class
    /// </summary>
    public class ExtendedBuffer_Input : Block
    {
        /// <summary>
        /// The RPC_HEADER_EXT structure provides information about the payload.
        /// </summary>
        public RPC_HEADER_EXT RPCHEADEREXT;

        /// <summary>
        /// A structure of bytes that constitute the ROP request payload.
        /// </summary>
        public Block Payload;

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
            index = num;
        }

        /// <summary>
        /// Parse the rgbInputBuffer.
        /// </summary>
        protected override void Parse()
        {
            RPCHEADEREXT = Parse<RPC_HEADER_EXT>();

            if (RPCHEADEREXT._Size > 0)
            {
                var payloadOffset = parser.Offset; // remember the offset for the payload
                var payloadBlock = ParseBytes(RPCHEADEREXT._Size);
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
                            MapiInspector.MAPIParser.InputPayLoadCompressedXOR = new List<byte[]>();
                        }

                        MapiInspector.MAPIParser.InputPayLoadCompressedXOR.Add(payloadBytes);
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
                        MapiInspector.MAPIParser.InputPayLoadCompressedXOR = new List<byte[]>();
                        MapiInspector.MAPIParser.InputPayLoadCompressedXOR.Add(payloadBytes);
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
                    Payload = Parse<ROPInputBuffer>(newParser);
                }

                Payload.ShiftOffset(payloadOffset); // shift the offset to the original position
            }
        }

        protected override void ParseBlocks()
        {
            Text = "ExtendedBuffer_Input";
            AddChild(RPCHEADEREXT, "RPCHEADEREXT");
            AddChild(Payload, "Payload");
        }
    }
}