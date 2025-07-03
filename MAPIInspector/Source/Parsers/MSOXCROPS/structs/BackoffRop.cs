using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.15.2.1.1 BackoffRop Structure
    /// A class indicates the BackoffRop structure which is defined in section 2.2.15.2.1.1.
    /// </summary>
    public class BackoffRop : Block
    {
        /// <summary>
        /// An unsigned integer index that identifies the ROP to apply the ROP BackOff to
        /// </summary>
        public BlockT<byte> RopIdBackoff;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds to apply a ROP BackOff.
        /// </summary>
        public BlockT<uint> Duration;

        /// <summary>
        /// Parse the BackoffRop structure.
        /// </summary>
        protected override void Parse()
        {
            RopIdBackoff = ParseT<byte>();
            Duration = ParseT<uint>();
        }

        protected override void ParseBlocks()
        {
            SetText("BackoffRop");
            AddChildBlockT(RopIdBackoff, "RopIdBackoff");
            AddChildBlockT(Duration, "Duration");
        }
    }
}
