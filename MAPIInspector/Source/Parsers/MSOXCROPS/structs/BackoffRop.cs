using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// 2.2.15.2.1.1 BackoffRop Structure
    /// A class indicates the BackoffRop structure which is defined in section 2.2.15.2.1.1.
    /// </summary>
    public class BackoffRop : BaseStructure
    {
        /// <summary>
        /// An unsigned integer index that identifies the ROP to apply the ROP BackOff to
        /// </summary>
        public byte RopIdBackoff;

        /// <summary>
        /// An unsigned integer that specifies the number of milliseconds to apply a ROP BackOff.
        /// </summary>
        public uint Duration;

        /// <summary>
        /// Parse the BackoffRop structure.
        /// </summary>
        /// <param name="s">A stream containing BackoffRop structure.</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            RopIdBackoff = ReadByte();
            Duration = ReadUint();
        }
    }
}
