using System;
using System.Collections.Generic;

namespace MapiInspector
{
    /// <summary>
    /// The utilities class for MAPI Inspector.
    /// </summary>
    class Utilities
    {
        /// <summary>
        /// Convert the data format from uint to string 
        /// </summary>
        /// <param name="data">The uint data</param>
        /// <returns>The converted string result</returns>
        public static string ConvertUintToString(uint data)
        {
            return data.ToString() + " (0x" + data.ToString("X8") + ")"; 
        }

        /// <summary>
        /// Convert the data format from ushort to string 
        /// </summary>
        /// <param name="data">The ushort data</param>
        /// <returns>The converted string result</returns>
        public static string ConvertUshortToString(ushort data)
        {
            return data.ToString() + " (0x" + data.ToString("X4") + ")"; 
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
                    chunkSize = chunkSize * 16 + b;
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
            } while (chunkSize > 0);            
            return payload.ToArray();
        }
    }
}