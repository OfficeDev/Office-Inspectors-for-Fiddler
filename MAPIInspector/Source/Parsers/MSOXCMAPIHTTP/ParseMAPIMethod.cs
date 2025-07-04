using BlockParser;
using System.Collections.Generic;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Parse the additional headers in Common Response Format
    /// </summary>
    public class ParseMAPIMethod
    {
        /// <summary>
        /// ParseAdditionalHeader method
        /// </summary>
        /// <param name="parser">The stream to parse</param>
        /// <param name="metaTags">MetaTags string</param>
        /// <param name="additionalHeaders">AdditionalHeaders string</param>
        public static void ParseAdditionalHeader(BinaryParser parser, out List<BlockString> metaTags, out List<BlockString> additionalHeaders)
        {
            string str = null;
            var tempmetaTags = new List<BlockString>();
            var tempadditionalHeaders = new List<BlockString>();

            while ((str != string.Empty) && (s.Position < s.Length - 1))
            {
                str = ReadString(Encoding.ASCII, "\r\n");
                var tempString = new BlockString(Encoding.ASCII, "\r\n");
                tempString.Value = str;
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        tempmetaTags.Add(tempString);
                        break;
                    default:
                        if (str != string.Empty)
                        {
                            tempadditionalHeaders.Add(tempString);
                            break;
                        }
                        else
                        {
                            tempString.Value = string.Empty;
                            tempadditionalHeaders.Add(tempString);
                            break;
                        }
                }
            }

            metaTags = tempmetaTags;
            additionalHeaders = tempadditionalHeaders;
        }
    }
}