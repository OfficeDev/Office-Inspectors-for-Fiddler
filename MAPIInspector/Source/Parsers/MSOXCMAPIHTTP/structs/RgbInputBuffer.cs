using BlockParser;
using System.Collections.Generic;
using System.Diagnostics;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 3.1.4.1.1.1.1 rgbAuxIn Input Buffer
    /// The rgbInputBuffer contains the ROP request payload. It is defined in section 3.1.4.2.1.1.1 of MS-OXCRPC.
    /// </summary>
    public class RgbInputBuffer : Block
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
            ropBufferSize = buffersize;
        }

        /// <summary>
        /// Parse the rgbInputBuffer.
        /// </summary>
        protected override void Parse()
        {
            int index = 0;
            var extendedBuffer_Inputs = new List<ExtendedBuffer_Input>();
            MapiInspector.MAPIParser.InputPayLoadCompressedXOR = new List<byte[]>();
            MapiInspector.MAPIParser.BuffersIsCompressed = new List<bool>();

            while (ropBufferSize > 0)
            {
                ExtendedBuffer_Input extendedBuffer_Input = new ExtendedBuffer_Input(index);
                extendedBuffer_Input.Parse(parser);
                extendedBuffer_Inputs.Add(extendedBuffer_Input);
                ropBufferSize -= (uint)(extendedBuffer_Input.RPCHEADEREXT._Size + 8);
                index += 1;
            }

            Buffers = extendedBuffer_Inputs.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("RgbInputBuffer");
            AddLabeledChildren(Buffers, "Buffers");
        }
    }
}