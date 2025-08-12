using System.Drawing;

namespace BlockParser
{
    /// <summary>
    /// Represents an interesting block of data.
    /// </summary>
    public abstract class BlockInteresting : Block
    {
        public virtual Color BackColor => Color.PaleVioletRed;
        public virtual string InterestingLabel => "Interesting block";
    }
}
