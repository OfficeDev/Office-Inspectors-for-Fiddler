using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// Represents a command in GLOBSET.
    /// 2.2.2.6 GLOBSET Structure
    /// </summary>
    public abstract class Command : Block
    {
        protected override void Parse() { }
    }
}
