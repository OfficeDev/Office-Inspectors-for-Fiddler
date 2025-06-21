namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// Any: this property type value matches any type;
    /// </summary>
    public class PtypUnspecified : Block
    {
        /// <summary>
        /// Parse the PtypUnspecified structure.
        /// </summary>
        protected override void Parse() { }

        protected override void ParseBlocks()
        {
            SetText("MSOXCDATA: Not implemented type definition - PtypUnspecified");
        }
    }
}
