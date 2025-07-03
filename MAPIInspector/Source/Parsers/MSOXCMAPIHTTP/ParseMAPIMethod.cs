using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Parse the additional headers in Common Response Format
    /// </summary>
    public class ParseMAPIMethod : BaseStructure
    {
        /// <summary>
        /// ParseAddtionlHeader method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        /// <param name="metaTags">MetaTags string</param>
        /// <param name="additionalHeaders">AdditionalHeaders string</param>
        public void ParseAddtionlHeader(Stream s, out List<MAPIString> metaTags, out List<MAPIString> additionalHeaders)
        {
            Parse(s);
            string str = null;
            List<MAPIString> tempmetaTags = new List<MAPIString>();
            List<MAPIString> tempadditionalHeaders = new List<MAPIString>();

            while ((str != string.Empty) && (s.Position < s.Length - 1))
            {
                str = ReadString(Encoding.ASCII, "\r\n");
                MAPIString tempString = new MAPIString(Encoding.ASCII, "\r\n");
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

        /// <summary>
        /// Override parse method.
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
        }
    }
}