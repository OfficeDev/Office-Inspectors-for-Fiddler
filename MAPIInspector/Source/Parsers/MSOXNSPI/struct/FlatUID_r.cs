using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.3.1 Property Values
    /// 2.3.1.1 FlatUID_r
    /// A class indicates the FlatUID_r structure.
    /// </summary>
    public class FlatUID_r : Block
    {
        /// <summary>
        /// Encodes the ordered bytes of the FlatUID data structure.
        /// </summary>
        public BlockGuid Ab;

        /// <summary>
        /// Parse the FlatUID_r payload of session.
        /// </summary>
        protected override void Parse()
        {
            Ab = Parse<BlockGuid>();
        }

        protected override void ParseBlocks()
        {
            SetText("FlatUID_r");
            this.AddChildGuid(Ab, "Ab");
        }
    }
}
