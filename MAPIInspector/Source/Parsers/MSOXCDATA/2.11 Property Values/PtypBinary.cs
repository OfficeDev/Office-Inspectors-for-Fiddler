namespace MAPIInspector.Parsers
{
    using System.IO;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a COUNT field followed by that many bytes.
    /// </summary>
    public class PtypBinary : BaseStructure
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public object Count;

        /// <summary>
        /// The binary value.
        /// </summary>
        public byte[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypBinary class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypBinary type.</param>
        public PtypBinary(CountWideEnum wide)
        {
            countWide = wide;
        }

        /// <summary>
        /// Parse the PtypBinary structure.
        /// </summary>
        /// <param name="s">A stream containing the PtypBinary structure</param>
        public override void Parse(Stream s)
        {
            base.Parse(s);
            HelpMethod help = new HelpMethod();
            Count = help.ReadCount(countWide, s);
            Value = ReadBytes(Count.GetHashCode());
        }
    }
}
