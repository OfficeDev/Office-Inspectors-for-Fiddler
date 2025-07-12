using BlockParser;
using System.Collections.Generic;

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
        public static void ParseAdditionalHeader(BinaryParser parser, out BlockString[] metaTags, out BlockString[] additionalHeaders)
        {
            BlockString str = null;
            var tempmetaTags = new List<BlockString>();
            var tempadditionalHeaders = new List<BlockString>();

            while (!parser.Empty)
            {
                str = Block.ParseStringLineA(parser);
                switch (str)
                {
                    case "PROCESSING":
                    case "PENDING":
                    case "DONE":
                        tempmetaTags.Add(str);
                        break;
                    default:
                        tempadditionalHeaders.Add(str);
                        break;
                }

                if (str.Empty) break;
            }

            metaTags = tempmetaTags.ToArray();
            additionalHeaders = tempadditionalHeaders.ToArray();
        }
    }
}