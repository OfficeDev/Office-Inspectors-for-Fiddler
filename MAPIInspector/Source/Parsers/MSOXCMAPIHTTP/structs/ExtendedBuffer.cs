using BlockParser;
using System.Collections.Generic;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [[MS-OXCRPC]] 3.1.4.1.1.1 Extended Buffer Format
    /// [[MS-OXCRPC]] 3.1.4.2.1.1 Extended Buffer Format
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

                if (isCompressedXOR)
                {
                    MapiInspector.MAPIParser.AuxPayLoadCompressedXOR = payloadBytes;
                }

                var newParser = new BinaryParser(payloadBytes);
                var payload = new List<AuxiliaryBufferPayload>();

                for (int length = 0; length < RPCHEADEREXT._Size;)
                {
                    var buffer = Parse<AuxiliaryBufferPayload>(newParser);
                    buffer.ShiftOffset(payloadOffset); // shift the offset to the original position
                    payload.Add(buffer);
                    length += buffer.AUXHEADER._Size;
                }

                Payload = payload.ToArray();
            }
        }

        protected override void ParseBlocks()
        {
            Text = "ExtendedBuffer";
            AddChild(RPCHEADEREXT, "RPCHEADEREXT");
            AddLabeledChildren(Payload, "Payload");
        }
    }
}
