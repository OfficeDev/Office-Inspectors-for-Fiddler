using BlockParser;
using System;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 3.1.4.1.1.1.2 rgbAuxOut Output Buffer
    /// The rgbOutputBufferPack contains multiple rgbOutputBuffer structure. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class RgbOutputBufferPack : Block
    {
        /// <summary>
        /// An unsigned int indicates the total size of the rgbOutputBuffers, this is a customized value.
        /// </summary>
        private uint RopBufferSize;

        /// <summary>
        /// rgbOutputBuffer packing.
        /// </summary>
        public RgbOutputBuffer[] RgbOutputBuffers;

        /// <summary>
        /// Initializes a new instance of the RgbOutputBufferPack class.
        /// </summary>
        /// <param name="ropBufferSize">The RopBuffer size</param>
        public RgbOutputBufferPack(uint ropBufferSize)
        {
            RopBufferSize = ropBufferSize;
        }

        /// <summary>
        /// Parse the rgbOutputBufferPack.
        /// </summary>
        protected override void Parse()
        {
            int index = 0;
            var rgbOutputBufferList = new List<RgbOutputBuffer>();
            var startPosition = parser.Offset;
            MapiInspector.MAPIParser.OutputPayLoadCompressedXOR = new List<byte[]>();
            MapiInspector.MAPIParser.BuffersIsCompressed = new List<bool>();

            while (parser.Offset - startPosition < RopBufferSize)
            {
                var buffer = new RgbOutputBuffer(index);
                try
                {
                    buffer.Parse(parser);
                }
                catch (MissingInformationException) { throw; }
                catch (MissingPartialInformationException) { throw; }
                catch (Exception e)
                {
                    AddChild(BlockException.Create("Exception", e, 0));
                }

                rgbOutputBufferList.Add(buffer);
                index += 1;
            }

            RgbOutputBuffers = rgbOutputBufferList.ToArray();
        }

        protected override void ParseBlocks()
        {
            SetText("rgbOutputBufferPack");
            AddLabeledChildren(RgbOutputBuffers, "RgbOutputBuffers");
        }
    }
}