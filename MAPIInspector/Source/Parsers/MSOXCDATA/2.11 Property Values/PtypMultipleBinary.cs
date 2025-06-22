namespace MAPIInspector.Parsers
{
    using BlockParser;
    using System.Collections.Generic;

    // There is one variation in the width of count fields.In the context of ROP buffers,
    // such as the RopGetPropertiesSpecific ROP([MS-OXCROPS] section 2.2.8.3), byte counts
    // for PtypBinary property values are 16 bits wide and value counts for all PtypMultiple
    // property values are 32 bits wide.However, in the context of extended rules, as
    // specified in [MS - OXORULE] section 2.2.4, and in the context of the MAPI extensions
    // for HTTP, as specified in [MS - OXCMAPIHTTP] section 2.2.5, byte counts for PtypBinary
    // property values and value counts for PtypMultiple property values are 32 bits wide.
    // Such count fields have a width designation of COUNT, as specified in section 2.11.1.1,
    // rather than an explicit width, as throughout section 2.11.

    /// <summary>
    /// 2.11.1 Property Data Types
    /// Variable size; a COUNT field followed by that many PtypBinary values.
    /// </summary>
    public class PtypMultipleBinary : Block
    {
        /// <summary>
        /// COUNT values are typically used to specify the size of an associated field.
        /// </summary>
        private BlockT<uint> Count;

        /// <summary>
        /// The array of binary value.
        /// </summary>
        public PtypBinary[] Value;

        /// <summary>
        /// The Count wide size.
        /// </summary>
        private CountWideEnum countWide;

        /// <summary>
        /// Initializes a new instance of the PtypMultipleBinary class
        /// </summary>
        /// <param name="wide">The Count wide size of PtypMultipleBinary type.</param>
        public PtypMultipleBinary(CountWideEnum wide)
        {
            countWide = wide;
        }

        /// <summary>
        /// Parse the PtypMultipleBinary structure.
        /// </summary>
        protected override void Parse()
        {
            Count = ParseT<uint>();

            List<PtypBinary> tempvalue = new List<PtypBinary>();
            for (int i = 0; i < Count.Data; i++)
            {
                var binary = new PtypBinary(countWide);
                binary.Parse(parser);
                tempvalue.Add(binary);
            }

            Value = tempvalue.ToArray();
        }

        protected override void ParseBlocks()
        {
            AddChildBlockT(Count, "Count");
            AddLabeledChildren(Value, "Value");
        }
    }
}
