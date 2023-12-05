namespace MapiInspector
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The utilities class for MAPI Inspector.
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Convert the data format from uInt to string 
        /// </summary>
        /// <param name="data">The UInt data</param>
        /// <returns>The converted string result</returns>
        public static string ConvertUintToString(uint data)
        {
            return data.ToString() + " (0x" + data.ToString("X8") + ")";
        }

        /// <summary>
        /// Convert the data format from uShort to string 
        /// </summary>
        /// <param name="data">The uShort data</param>
        /// <returns>The converted string result</returns>
        public static string ConvertUshortToString(ushort data)
        {
            return data.ToString() + " (0x" + data.ToString("X4") + ")";
        }

        public static string ConvertByteArrayToString(byte[] bin, uint? limit = null)
        {
            if (bin == null || bin.Length == 0) return string.Empty;

            var szText = new StringBuilder();
            long length = bin.Length;
            if (limit.HasValue) length = Math.Min(length, limit.Value);
            for (uint i = 0; i < length; i++)
            {
                if (bin[i] <= 0x8)
                {
                    szText.Append(".");
                }
                else if (bin[i] >= 0xA && bin[i] <= 0x1F)
                {
                    szText.Append(".");
                }
                else if (bin[i] > 0xff)
                {
                    szText.Append(".");
                }
                else
                {
                    szText.Append((char)bin[i]);
                }
            }

            return szText.ToString();
        }

        // Array type just display the first 30 values if the array length is more than 30.
        public static string ConvertArrayToHexString(Array bin, int? limit = 30)
        {
            var result = new StringBuilder();
            int displayLength = limit ?? bin.Length;
            result.Append("[");

            foreach (var b in bin)
            {
                result.Append($"{b:X2},");

                if (displayLength <= 1)
                {
                    result.Insert(result.Length - 1, "...");
                    break;
                }

                displayLength--;
            }

            result.Remove(result.Length - 1, 1);
            result.Append("]");
            return result.ToString();
        }

        /// <summary>
        /// Get the valid response from HTTP chunked response body.
        /// </summary>
        /// <param name="responseBodyFromFiddler">The raw response body from Fiddler.</param>
        /// <returns>The valid response bytes</returns>
        public static byte[] GetPaylodFromChunkedBody(byte[] responseBodyFromFiddler)
        {
            int length = responseBodyFromFiddler.Length;
            List<byte> payload = new List<byte>();

            int chunkSize;
            int i = 0;
            do
            {
                chunkSize = 0;
                while (true)
                {
                    int b = responseBodyFromFiddler[i];

                    if (b >= 0x30 && b <= 0x39)
                    {
                        b -= 0x30;
                    }
                    else if (b >= 0x41 && b <= 0x46)
                    {
                        b -= 0x41 - 10;
                    }
                    else if (b >= 0x61 && b <= 0x66)
                    {
                        b -= 0x61 - 10;
                    }
                    else
                    {
                        break;
                    }

                    chunkSize = (chunkSize * 16) + b;
                    i++;
                }

                if (responseBodyFromFiddler[i] != 0x0D || responseBodyFromFiddler[i + 1] != 0x0A)
                {
                    throw new Exception();
                }

                i += 2;
                for (int k = 0; k < chunkSize; k++, i++)
                {
                    payload.Add(responseBodyFromFiddler[i]);
                }

                if (responseBodyFromFiddler[i] != 0x0D || responseBodyFromFiddler[i + 1] != 0x0A)
                {
                    throw new Exception();
                }

                i += 2;
            }
            while (chunkSize > 0);
            return payload.ToArray();
        }

        /// <summary>
        /// Converts a simple (non-flag) enum to string. If the value is not present in the underlying enum, converts to a hex string.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string EnumToString(object obj)
        {
            if (Enum.IsDefined(obj.GetType(), obj))
            {
                return obj.ToString();
            }
            else
            {
                return $"0x{Convert.ToUInt64(obj):X}";
            }
        }
    }
}