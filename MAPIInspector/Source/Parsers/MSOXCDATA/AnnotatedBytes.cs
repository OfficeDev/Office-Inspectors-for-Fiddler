using System.IO;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// The AnnotatedBytes class to a byte stream with an alternate version of it (typically ConvertByteArrayToString)
    /// </summary>
    public class AnnotatedBytes : AnnotatedData
    {
        /// <summary>
        /// Bytes as byte array.
        /// </summary>
        public byte[] bytes;
        private int size;

        /// <summary>
        /// Initializes a new instance of the AnnotatedBytes class with parameters.
        /// </summary>
        /// <param name="_size">Size of the byte array</param>
        public AnnotatedBytes(int _size)
        {
            size = _size;
        }

        /// <summary>
        /// Parse method
        /// </summary>
        /// <param name="s">The stream to parse</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            var offset = (int)s.Position;
            bytes = ReadBytes(size);
            ParsedValue = MapiInspector.Utilities.ConvertArrayToHexString(bytes);
            this["string"] = MapiInspector.Utilities.ConvertByteArrayToString(bytes);
        }

        public override int Size { get { return bytes.Length; } }
    }
}