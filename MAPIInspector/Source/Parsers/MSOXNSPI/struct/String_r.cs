using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.11 PROP_VAL_UNION
    /// A class indicates the String_r structure.
    /// </summary>
    public class String_r : Block
    {
        /// <summary>
        /// A single 8-bit character string value. value is NULL-terminated.
        /// </summary>
        public BlockString Value; // ascii

        /// <summary>
        /// Parse the String_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseStringA();
        }

        protected override void ParseBlocks()
        {
            Text = "String_r";
            AddChildString(Value, "Value");
        }
    }
}
