using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXNSPI] 2.2.2 Property Values
    /// [MS-OXNSPI] 2.2.2.1 FlatUID_r Structure
    /// [MS-OXCDATA] 2.5.2 FlatUID_r Structure
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
            Text = "FlatUID_r";
            this.AddChildGuid(Ab, "Ab");
        }
    }
}
