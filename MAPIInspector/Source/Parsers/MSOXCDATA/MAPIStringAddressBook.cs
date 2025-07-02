using BlockParser;
using System.Text;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The MAPIString class to record the related attributes of string.
    /// </summary>
    public class MAPIStringAddressBook : Block
    {
        /// <summary>
        /// The string value
        /// </summary>
        public BlockString Value;

        /// <summary>
        /// TDI#76879 tell us the real MapiHttp traffic will add the magic byte 'FF' for the string or binary based property value.
        /// 2.2.1.1 AddressBookPropertyValue Structure
        /// </summary>
        public BlockT<bool> HasValue;

        /// <summary>
        /// The string Encoding : ASCII or Unicode
        /// </summary>
        public Encoding Encode;

        /// <summary>
        /// Initializes a new instance of the MAPIStringAddressBook class with parameters.
        /// </summary>
        /// <param name="encode">The encoding type</param>
        public MAPIStringAddressBook(Encoding encode)
        {
            Encode = encode;
        }

        /// <summary>
        /// The Parse method
        /// </summary>
        protected override void Parse()
        {
            HasValue = ParseAs<byte, bool>();
            if (Encode == Encoding.Unicode)
            {
                Value = ParseStringW();
            }
            else if (Encode == Encoding.ASCII)
            {
                Value = ParseStringA();
            }
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(HasValue, "HasValue");
            AddChildString(Value, "Value");
        }
    }
}
