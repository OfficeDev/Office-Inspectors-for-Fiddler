using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapiInspector
{
    /// <summary>
    /// The utilities class for MAPI Inspector.
    /// </summary>
    public class Utilities
    {
        /// <summary>
        /// Get the valid response from HTTP chunked response body.
        /// </summary>
        /// <param name="responseBodyFromFiddler">The raw response body from Fiddler.</param>
        /// <returns>The valid response bytes</returns>
        public static byte[] GetPayloadFromChunkedBody(byte[] responseBodyFromFiddler)
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
        /// The SealTheObject Class is used to sealing the parse result.
        /// </summary>
        public class SealTheObject
        {
            private string title;

            private string message;

            private object obj;

            public string Title
            {
                get
                {
                    return title;
                }
                set
                {
                    title = value;
                }
            }

            public string Message
            {
                get
                {
                    return message;
                }
                set
                {
                    message = value;
                }
            }

            public object Obj
            {
                get
                {
                    return obj;
                }
                set
                {
                    obj = value;
                }
            }

            public SealTheObject(int id, bool isRequest, object obj)
            {
                title = "Frame" + id + (isRequest ? "Request" : "Response");
                message = obj.GetType().Name;
                Obj = obj;
            }
        }

        /// <summary>
        /// Method to seal parse result to Json string
        /// Used in test automation
        /// </summary>
        /// <param name="id">The id of Fiddler session</param>
        /// <param name="isRequest">Bool value indicates the session is a HttpRequest message or HttpResponse message</param>
        /// <param name="obj">The object of parse result</param>
        /// <returns>Json string converted by parse result</returns>
        public static string ConvertCSharpToJson(int id, bool isRequest, object obj)
        {
            SealTheObject sealTheObject = new SealTheObject(id, isRequest, obj);
            return JsonConvert.SerializeObject((object)sealTheObject);
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

        /// <summary>
        /// Read bits value from byte
        /// </summary>
        /// <param name="b">The byte.</param>
        /// <param name="index">The bit index to read</param>
        /// <param name="length">The bit length to read</param>
        /// <returns>bits value</returns>
        static public byte GetBits(byte b, int index, int length)
        {
            int bit = 0;
            int tempBit = 0;

            if ((index >= 8) || (length > 8))
            {
                throw new Exception("The range for index or length should be 0~7.");
            }

            for (int i = 0; i < length; i++)
            {
                tempBit = ((b & (1 << (7 - index - i))) > 0) ? 1 : 0;
                bit = (bit << 1) | tempBit;
            }

            return (byte)bit;
        }
    }
}