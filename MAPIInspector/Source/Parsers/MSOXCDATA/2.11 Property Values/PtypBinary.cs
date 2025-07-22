using BlockParser;

namespace MAPIInspector.Parsers
{
    /// <summary>
    /// [MS-OXCDATA] 2.11.1 Property Data Types
    /// Variable size; a COUNT field followed by that many bytes.
    /// </summary>
    public class PtypBinary : Block
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        public BlockT<uint> Count;

        /// <summary>
        /// The binary value.
        /// </summary>
        public BlockBytes Value;

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
        protected override void Parse()
        {
            switch (countWide)
            {
                case CountWideEnum.twoBytes:
                    Count= ParseAs<ushort,uint>();
                    break;
                default:
                case CountWideEnum.fourBytes:
                    Count = ParseT<uint>();
                    break;
            }

            Value = ParseBytes(Count);
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Count, "Count");
            AddChildBytes(Value, "Value");
        }
    }
}
