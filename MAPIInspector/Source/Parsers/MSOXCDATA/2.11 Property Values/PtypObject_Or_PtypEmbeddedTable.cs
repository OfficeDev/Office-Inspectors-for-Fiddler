using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
    /// IN FUTURE: How to distinguish PtypObject from PtypEmbeddedTable since they share the same value
    /// </summary>
    public class PtypObject_Or_PtypEmbeddedTable : Block
    {
        /// <summary>
        /// Parse the PtypUnspecified structure.
        /// </summary>
        protected override void Parse() { }

        protected override void ParseBlocks()
        {
            Text = "MSOXCDATA: Not implemented type definition - PtypObject_Or_PtypEmbeddedTable";
        }
    }
}
