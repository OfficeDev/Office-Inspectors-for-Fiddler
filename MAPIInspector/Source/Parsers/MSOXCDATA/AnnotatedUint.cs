namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// The AnnotatedUint class.
    /// </summary>
    public class AnnotatedUint : AnnotatedData
    {
        /// <summary>
        /// uint value
        /// </summary>
        public uint value;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public AnnotatedUint(Stream s)
        {
            base.Parse(s);
            value = ReadUint();
        }

        public override int Size { get; } = sizeof(uint);
    }
}