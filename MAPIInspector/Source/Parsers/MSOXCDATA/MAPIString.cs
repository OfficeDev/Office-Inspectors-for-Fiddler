using System.IO;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MAPIString class to record the related attributes of string.
    /// </summary>
    public class MAPIString : AnnotatedData
    {
        /// <summary>
        /// The string value
        /// </summary>
        public string Value;

        /// <summary>
        /// The string Encoding : ASCII or Unicode
        /// </summary>
        private Encoding Encode;

        /// <summary>
        /// The string Terminator. Default is "\0"
        /// </summary>
        private string Terminator;

        /// <summary>
        /// If the StringLength is not 0, The StringLength will be as the string length
        /// </summary>
        private int StringLength;

        /// <summary>
        /// If the Encoding is Unicode, and it is reduced Unicode, it is true
        /// </summary>
        private bool ReducedUnicode;

        /// <summary>
        /// Initializes a new instance of the MAPIString class with parameters.
        /// </summary>
        /// <param name="encode">The encode type</param>
        /// <param name="terminator">Specify the terminator of the string</param>
        /// <param name="stringLength">Length of the string</param>
        /// <param name="reducedUnicode">Indicate Whether the terminator is reduced</param>
        public MAPIString(Encoding encode, string terminator = "\0", int stringLength = 0, bool reducedUnicode = false)
        {
            Encode = encode;
            Terminator = terminator;
            StringLength = stringLength;
            ReducedUnicode = reducedUnicode;
        }

        /// <summary>
        /// Parse method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            Value = ReadString(Encode, Terminator, StringLength, ReducedUnicode);
        }
        public override string ToString() => Value;
        public override int Size
        {
            get
            {
                var len = 0;
                if (Encode == Encoding.Unicode)
                {
                    // If the StringLength is not equal 0, the StringLength will be basis for size
                    if (StringLength != 0)
                    {
                        len = StringLength * 2;
                    }
                    else
                    {
                        if (Value != null)
                        {
                            len = Value.Length * 2;
                        }

                        if (ReducedUnicode)
                        {
                            len -= 1;
                        }

                        len += Terminator.Length * 2;
                    }
                }
                else
                {
                    // If the Encoding is ASCII.
                    if (StringLength != 0)
                    {
                        // If the StringLength is not equal 0, the StringLength will be basis for size
                        len = StringLength;
                    }
                    else
                    {
                        if (Value != null)
                        {
                            len = Value.Length;
                        }

                        len += Terminator.Length;
                    }
                }

                return len;
            }
        }
    }
}
