using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.11.1 Property Data Types
    /// None: This property is a placeholder.
    /// </summary>
    public class PtypNull : Block
    {
        /// <summary>
        /// Parse the PtypNull structure.
        /// </summary>
        protected override void Parse() { }

        protected override void ParseBlocks()
        {
            Text = "MSOXCDATA: PtypNull placeholder";
        }
    }
}
