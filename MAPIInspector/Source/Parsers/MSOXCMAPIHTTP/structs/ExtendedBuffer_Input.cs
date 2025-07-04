using System.Collections.Generic;
using System.IO;

namespace MAPIInspector.Parsers
{
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
            index = num;
        }

        /// <summary>
        /// Parse the rgbInputBuffer.
        /// </summary>
        /// <param name="s">A stream containing the rgbInputBuffer.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);

            RPCHEADEREXT = new RPC_HEADER_EXT();
            RPCHEADEREXT.Parse(s);

            if (RPCHEADEREXT._Size > 0)
            {
                byte[] payloadBytes = ReadBytes((int)RPCHEADEREXT._Size);
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

                Stream stream = new MemoryStream(payloadBytes);

                if (MapiInspector.MAPIParser.IsOnlyGetServerHandle)
                {
                    ROPInputBuffer_WithoutCROPS inputBufferWithoutCROPS = new ROPInputBuffer_WithoutCROPS();
                    inputBufferWithoutCROPS.Parse(stream);
                    Payload = inputBufferWithoutCROPS;
                }
                else
                {
                    ROPInputBuffer inputBuffer = new ROPInputBuffer();
                    inputBuffer.Parse(stream);
                    Payload = inputBuffer;
                }
            }
        }
    }
}