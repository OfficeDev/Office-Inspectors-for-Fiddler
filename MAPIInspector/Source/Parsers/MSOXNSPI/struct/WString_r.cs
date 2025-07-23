using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.2 Property Values
    /// [MS-OXNSPI] 2.2.2.11 PROP_VAL_UNION Structure
    /// A class indicates the WString_r structure.
    /// </summary>
    public class WString_r : Block
    {
        /// <summary>
        /// A single Unicode string value. value is NULL-terminated.
        /// </summary>
        public BlockString Value;

        /// <summary>
        /// Parse the WString_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            Value = ParseStringW();
        }

        protected override void ParseBlocks()
        {
            Text = "WString_r";
            AddChildString(Value, "Value");
        }
    }
}
