using System;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The DecodingContext is shared between some ROP request and response.
    /// </summary>
    public class CompressionAndObfuscationAlgorithm
    {
        /// <summary>
        /// Obfuscates payload in the stream by applying XOR to each byte of the data with the value 0xA5
        /// </summary>
        /// <param name="data">The bytes to be obfuscated.</param>
        /// <returns>The obfuscated bytes</returns>
        public static byte[] XOR(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("inputStream");
            }

            byte[] byteArray = data;

            for (int i = 0; i < data.Length; i++)
            {
                byteArray[i] ^= 0xA5;
            }

            return byteArray;
        }

        /// <summary>
        /// Decodes stream using Direct2 algorithm and decompresses using LZ77 algorithm.
        /// </summary>
        /// <param name="inputStream">The input stream needed to be decompressed.</param>
        /// <param name="actualLength">The expected size of the decompressed output stream.</param>
        /// <returns>Returns the decompressed stream.</returns>
        public static byte[] LZ77Decompress(byte[] inputStream, int actualLength)
        {
            byte? shareByteCache = null;
            int bitMaskIndex = 0;
            uint bitMask = 0x00000000;
            int inputPosition = 0;
            int outputPosition = 0;
            byte[] outputBuffer = new byte[actualLength];

            while (inputPosition < inputStream.Length)
            {
                // If the bitMaskIndex = 0, it represents the entire "bitMask" has been
                // consumed or we are just starting to do the decompress.
                if (bitMaskIndex == 0)
                {
                    bitMask = BitConverter.ToUInt32(inputStream, inputPosition);
                    inputPosition += 4;
                    bitMaskIndex = 32;
                    continue;
                }

                bool hasMetaData = (bitMask & 0x80000000) != 0;
                bitMask = bitMask << 1;
                bitMaskIndex--;

                // If it's data, just copy.
                if (!hasMetaData)
                {
                    outputBuffer[outputPosition] = inputStream[inputPosition];
                    outputPosition++;
                    inputPosition++;
                }
                else
                {
                    // Otherwise copy the data specified by MetaData (offset, length) pair
                    int offset = 0;
                    int length = 0;
                    GetMetaDataValue(inputStream, ref inputPosition, ref shareByteCache, out offset, out length);

                    while (length != 0)
                    {
                        outputBuffer[outputPosition] = outputBuffer[outputPosition - offset];
                        outputPosition++;
                        length--;
                    }
                }
            }

            return outputBuffer;
        }

        /// <summary>
        /// The function is used to get the MetaData from raw request data
        /// </summary>
        /// <param name="encodedBuffer">The raw request data</param>
        /// <param name="decodingPosition">The decoding position for the raw request data</param>
        /// <param name="shareByteCache">The shared bytes stack</param>
        /// <param name="offset">The returned offset value</param>
        /// <param name="length">The returned length value</param>
        public static void GetMetaDataValue(byte[] encodedBuffer, ref int decodingPosition, ref byte? shareByteCache, out int offset, out int length)
        {
            // Initialize: To encode a length between 3 and 9, we use the 3 bits that are "in-line" in the 2-byte MetaData.
            ushort inlineMetadata = 0;
            inlineMetadata = BitConverter.ToUInt16(encodedBuffer, decodingPosition);
            decodingPosition += 2;

            offset = inlineMetadata >> 3;
            offset++;
            length = inlineMetadata & 0x0007;

            // Add the minimum match - 3 bytes
            length += 3;

            // Every other time that the length is greater than 9,
            // an additional byte follows the initial 2-byte MetaData
            if (length > 9)
            {
                int additiveLength = 0;
                if (shareByteCache != null)
                {
                    additiveLength = (shareByteCache.Value >> 4) & 0x0f;
                    shareByteCache = null;
                }
                else
                {
                    shareByteCache = encodedBuffer[decodingPosition];
                    decodingPosition++;
                    additiveLength = shareByteCache.Value & 0x0f;
                }

                length += additiveLength;
            }

            // If the length is more than 24, the next byte is also used in the length calculation
            if (length > 24)
            {
                length += encodedBuffer[decodingPosition];
                decodingPosition++;
            }

            // For lengths that are equal to 280 or greater, the length is calculated only
            // from these last 2 bytes and is not added to the previous length bits.
            if (length > 279)
            {
                length = BitConverter.ToInt16(encodedBuffer, decodingPosition) + 3;
                decodingPosition += 2;
            }
        }
    }
}