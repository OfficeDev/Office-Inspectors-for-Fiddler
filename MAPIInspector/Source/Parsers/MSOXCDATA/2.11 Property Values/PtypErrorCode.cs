namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// 4 bytes; a 32-bit integer encoding error information as specified in section 2.4.1.
    /// </summary>
    public class PtypErrorCode : Block
    {
        /// <summary>
        /// 32-bit integer encoding error information.
        /// </summary>
        public BlockT<AdditionalErrorCodes> Value;

        /// <summary>
        /// Parse the PtypErrorCode structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypErrorCode structure</param>
        protected override void Parse()
        {
            Value = ParseT<AdditionalErrorCodes>();
        }

        protected override void ParseBlocks()
        {
            // TODO: Map the error
            Text = $"{Value.Data}";
        }
    }
}
