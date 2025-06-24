namespace MAPIInspector.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 3.1.4.1.1.1.2 rgbAuxOut Output Buffer
    /// The rgbOutputBufferPack contains multiple rgbOutputBuffer structure. It is defined in section 3.1.4.2.1.1.2 of MS-OXCRPC.
    /// </summary>
    public class RgbOutputBufferPack : BaseStructure
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
        /// <param name="s">A stream containing the rgbOutputBufferPack.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            int index = 0;
            List<RgbOutputBuffer> rgbOutputBufferList = new List<RgbOutputBuffer>();
            long startPosition = s.Position;
            MapiInspector.MAPIParser.OutputPayLoadCompressedXOR = new List<byte[]>();
            MapiInspector.MAPIParser.BuffersIsCompressed = new List<bool>();

            while (s.Position - startPosition < RopBufferSize)
            {
                RgbOutputBuffer buffer = new RgbOutputBuffer(index);
                try
                {
                    buffer.Parse(s);
                }
                catch (MissingInformationException) { throw; }
                catch (MissingPartialInformationException) { throw; }
                catch (Exception e)
                {
                    buffer.Payload = e.ToString();
                }

                rgbOutputBufferList.Add(buffer);
                index += 1;
            }

            RgbOutputBuffers = rgbOutputBufferList.ToArray();
        }
    }
}