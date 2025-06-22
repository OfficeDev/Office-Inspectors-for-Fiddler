namespace MAPIInspector.Parsers
{
    using BlockParser;

    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a COUNT field followed by that many bytes.
    /// </summary>
    public class PtypBinary : Block
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        private Block _count;
        public uint Count;

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
                    _count = ParseT<ushort>();
                    Count = (_count as BlockT<ushort>).Data;
                    break;
                default:
                case CountWideEnum.fourBytes:
                    _count = ParseT<uint>();
                    Count = (_count as BlockT<uint>).Data;
                    break;
            }
            Value = ParseBytes((int)Count);
        }

        protected override void ParseBlocks()
        {
            AddChild(_count, $"Count:{Count}");
            AddChild(Value, $"Value:{Value.ToHexString(false)}");
        }
    }
}
