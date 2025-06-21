namespace MAPIInspector.Parsers
{
    using System;
    using System.IO;

    /// <summary>
    /// The AnnotatedGuid class.
    /// </summary>
    public class AnnotatedGuid : AnnotatedData
    {
        /// <summary>
        /// uint value
        /// </summary>
        public Guid value;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public AnnotatedGuid(Stream s)
        {
            base.Parse(s);
            value = ReadGuid();
            ParsedValue = Guids.ToString(value);
        }

        public override int Size { get; } = 16; // sizeof(Guid)
    }
}